using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using XFILE = Microsoft.Extensions.FileProviders.IFileInfo;

namespace InteropTypes.IO.Mvvm
{
    /// <summary>
    /// Implemented by the UI control or the ViewModel.
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
        /// <param name="image">the image</param>
        void SetImage(TImage image);
    }

    /// <summary>
    /// Interface to a server object capable of loading the image requested by the client and set the loaded image back into the client.
    /// </summary>
    /// <typeparam name="TImage"></typeparam>
    public interface IFileThumbnailServer<TImage>
    {
        public IFileThumbnailServer<TImage> WrapWithSemaphore()
        {
            return new _SemaphoreThumbnailFactory<TImage>(this);
        }

        public IFileThumbnailServer<TImage> WrapWithQueue()
        {
            return new _QueuedThumbnailFactory<TImage>(this);
        }

        /// <summary>
        /// Loads the image declared by the client and sets it back
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<bool> LoadAndAssignImageAsync(IFileThumbnailClient<TImage> item);
    }

    /// <summary>
    /// A bulk image loading with a simple semaphoreslim throttle.
    /// </summary>
    /// <typeparam name="TImage"></typeparam>
    class _SemaphoreThumbnailFactory<TImage> : IFileThumbnailServer<TImage>
    {
        public _SemaphoreThumbnailFactory(IFileThumbnailServer<TImage> baseFactory)
        {
            _BaseFactory = baseFactory;
        }

        private readonly IFileThumbnailServer<TImage> _BaseFactory;

        private static readonly SemaphoreSlim _Throttle = new SemaphoreSlim(1, 1);

        public async Task<bool> LoadAndAssignImageAsync(IFileThumbnailClient<TImage> item)
        {
            try
            {
                await _Throttle.WaitAsync();
                return await _BaseFactory.LoadAndAssignImageAsync(item);
            }
            finally
            {
                _Throttle.Release();
            }
        }
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
        private readonly ConcurrentQueue<IFileThumbnailClient<TImage>> jobs = new ConcurrentQueue<IFileThumbnailClient<TImage>>();
        private readonly object lockObj = new object();

        private Task? workerTask;
        private CancellationTokenSource? cts;

        #endregion

        #region API

        public async Task<bool> LoadAndAssignImageAsync(IFileThumbnailClient<TImage> item)
        {
            EnqueueAndRun(item);

            return true;
        }

        private void EnqueueAndRun(IFileThumbnailClient<TImage> job)
        {
            jobs.Enqueue(job);

            lock (lockObj)
            {
                // Start worker if not already running
                if (workerTask == null || workerTask.IsCompleted)
                {
                    cts?.Dispose();
                    cts = new CancellationTokenSource();
                    workerTask = ProcessJobsAsync(cts.Token);
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
                    if (jobs.TryDequeue(out var job))
                    {
                        #if DEBUG
                        await Task.Delay(20);
                        #endif

                        await _BaseFactory.LoadAndAssignImageAsync(job);
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
