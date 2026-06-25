using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using XFILE = Microsoft.Extensions.FileProviders.IFileInfo;

namespace InteropTypes.IO.Mvvm
{
    /// <summary>
    /// Represents a UI view control over a "file" objects and accepts setting its image asynchronously.
    /// </summary>
    /// <typeparam name="TImage">The UI framwork image</typeparam>
    public interface IFileThumbnailClient<TImage>
    {
        /// <summary>
        /// Gets the file referenced by the client object.
        /// </summary>
        /// <returns></returns>
        XFILE GetSource();

        /// <summary>
        /// Sets the image associated to the file to the client.
        /// </summary>
        /// <remarks>
        /// The implementation must be aware that this might be called by the thread other than the UI
        /// </remarks>
        /// <param name="image">the image</param>
        void SetImage(TImage image);
    }

    /// <summary>
    /// Interface to a server object capable of loading the image requested by the client and set the loaded image back into the client.
    /// </summary>
    /// <typeparam name="TImage"></typeparam>
    public interface IFileThumbnailServer<TImage>
    {
        public IFileThumbnailServer<TImage> WrapWithCache()
        {
            return this is FileThumbnailServerCache<TImage> wrapped
                ? wrapped
                : new FileThumbnailServerCache<TImage>(this);
        }

        public IFileThumbnailServer<TImage> WrapWithSemaphore()
        {
            return this is _SemaphoreThumbnailFactory<TImage> wrapped
                ? wrapped
                : new _SemaphoreThumbnailFactory<TImage>(this);
        }

        public IFileThumbnailServer<TImage> WrapWithQueue()
        {
            return this is _QueuedThumbnailFactory<TImage> wrapped
                ? wrapped
                : new _QueuedThumbnailFactory<TImage>(this);
        }

        /// <summary>
        /// Loads the image requested by the client and sets it back to the client.
        /// </summary>
        /// <param name="item">The UI control implementing <see cref="IFileThumbnailClient{TImage}"/></param>
        /// <returns>true on success</returns>
        public Task<bool> UpdateClientAsync(IFileThumbnailClient<TImage> item);
    }


    

    /// <summary>
    /// A bulk image loading with a simple semaphoreslim throttle.
    /// </summary>
    /// <typeparam name="TImage"></typeparam>
    class _SemaphoreThumbnailFactory<TImage> : IFileThumbnailServer<TImage>
    {
        #region lifecycle
        public _SemaphoreThumbnailFactory(IFileThumbnailServer<TImage> baseFactory)
        {
            _BaseFactory = baseFactory;
        }

        #endregion

        #region data

        private readonly IFileThumbnailServer<TImage> _BaseFactory;

        private static readonly SemaphoreSlim _Throttle = new SemaphoreSlim(1, 1);

        #endregion

        #region API

        public async Task<bool> UpdateClientAsync(IFileThumbnailClient<TImage> item)
        {
            try
            {
                await _Throttle.WaitAsync();
                return await _BaseFactory.UpdateClientAsync(item);
            }
            finally
            {
                _Throttle.Release();
            }
        }

        #endregion
    }

    /// <summary>
    /// A bulk image loading with a queue that ensures a single task is uses no matter how many images are requested.
    /// </summary>
    /// <typeparam name="TImage"></typeparam>
    class _QueuedThumbnailFactory<TImage> : IFileThumbnailServer<TImage>
    {
        #region lifecycle
        public _QueuedThumbnailFactory(IFileThumbnailServer<TImage> baseFactory)
        {
            _BaseFactory = baseFactory;
        }

        #endregion

        #region data

        private readonly IFileThumbnailServer<TImage> _BaseFactory;
        private readonly ConcurrentQueue<IFileThumbnailClient<TImage>> _Jobs = new ConcurrentQueue<IFileThumbnailClient<TImage>>();
        private readonly object _Mutex = new object();

        private Task _WorkerTask;
        private CancellationTokenSource _CancelToken;

        #endregion

        #region API

        public async Task<bool> UpdateClientAsync(IFileThumbnailClient<TImage> item)
        {
            EnqueueAndRun(item);

            return true;
        }

        private void EnqueueAndRun(IFileThumbnailClient<TImage> job)
        {
            _Jobs.Enqueue(job);

            lock (_Mutex)
            {
                // Start worker if not already running
                if (_WorkerTask == null || _WorkerTask.IsCompleted)
                {
                    _CancelToken?.Dispose();
                    _CancelToken = new CancellationTokenSource();
                    _WorkerTask = ProcessJobsAsync(_CancelToken.Token);
                }
            }
        }

        private async Task ProcessJobsAsync(CancellationToken cancellationToken)
        {
            bool firstTry = true;

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (_Jobs.TryDequeue(out var job))
                    {
                        #if DEBUG
                        await Task.Delay(20);
                        #endif

                        await _BaseFactory.UpdateClientAsync(job);
                        firstTry = true;
                    }
                    else
                    {
                        // No jobs left, wait a bit before checking again
                        if (firstTry) await Task.Delay(100, cancellationToken);
                        else break;

                        firstTry = false;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
            }
        }

        #endregion        
    }
}
