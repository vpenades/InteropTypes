using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    /// <summary>
    /// Represents a producer-consumer interlocked exchange bitmap.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is useful for devices that produce a stream of bitmaps in their own thread
    /// and a client application consuming these bitmaps in the UI thread.
    /// </para>
    /// <para>
    /// Known derived classes: InteropTypes.Graphics.Backends.WPF.WPFClientBitmap
    /// </para>    
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("Frames In:{_TotalFramesEnqueued} Out:{_TotalFramesDequeued}")]
    public class InterlockedBitmap
    {
        #region lifecycle

        public InterlockedBitmap(int bufferCount = 3)
        {
            if (bufferCount < 1) throw new ArgumentOutOfRangeException(nameof(bufferCount));

            _MaxQueue = bufferCount;
        }

        #endregion

        #region data

        private readonly int _MaxQueue;

        private int _TotalFramesEnqueued;
        private int _TotalFramesDequeued;

        private readonly System.Collections.Concurrent.ConcurrentQueue<MemoryBitmap> _Pool = new System.Collections.Concurrent.ConcurrentQueue<MemoryBitmap>();

        private readonly System.Collections.Concurrent.ConcurrentQueue<MemoryBitmap> _Queue = new System.Collections.Concurrent.ConcurrentQueue<MemoryBitmap>();

        private MemoryBitmap _LastDequeued;

        #endregion

        #region properties        

        /// <summary>
        /// Number of frames currently enqueued.
        /// </summary>
        public int EnqueuedFramesCount => _Queue.Count;

        /// <summary>
        /// Raised in the writing thread when a new frame has been enqueued.
        /// </summary>
        public event EventHandler FrameEnqueued;

        #endregion

        #region API

        private MemoryBitmap _GetBitmapFromPool()
        {
            return _Pool.TryDequeue(out var bitmap) ? bitmap : default;
        }

        public bool TryEnqueue(SpanBitmap bitmap)
        {
            _AddFps();

            while (_Queue.Count > _MaxQueue && _Queue.TryDequeue(out _)) { }

            var dst = _GetBitmapFromPool();

            dst = WriteFrame(dst, bitmap);

            _Queue.Enqueue(dst);

            ++_TotalFramesEnqueued;

            RaiseFrameEnqueued();

            return true;
        }

        /// <summary>
        /// copies the src bitmap to the bitmap that will be queued.
        /// </summary>
        /// <param name="pool">The reused bitmap taken from the pool.</param>
        /// <param name="src">the input bitmap.</param>
        /// <returns>The bitmap that will be stored into the queue.</returns>
        /// <remarks>
        /// At first, <paramref name="pool"/> and <paramref name="src"/> will not have the same dimensions and format,
        /// it's up to the developer to choose what to do here.
        /// </remarks>
        protected virtual MemoryBitmap WriteFrame(MemoryBitmap pool, SpanBitmap src)
        {
            src.Info.CreateBitmap(ref pool, false);
            pool.SetPixels(0, 0, src);
            return pool;
        }

        /// <summary>
        /// Raises <see cref="FrameEnqueued"/> event in the writing thread.
        /// </summary>        
        protected virtual void RaiseFrameEnqueued()
        {
            FrameEnqueued?.Invoke(this,EventArgs.Empty);
        }

        /// <summary>
        /// Always return the last queued frame.
        /// </summary>
        /// <param name="reader"></param>
        public void DequeueLastOrDefault(Action<MemoryBitmap> reader)
        {
            if (TryDropAndDequeueLast(reader)) return;
            reader(_LastDequeued);
        }

        /// <summary>
        /// Drops all the extra frames in the queue and returns the last frame of the queue if available.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public bool TryDropAndDequeueLast(Action<MemoryBitmap> reader)
        {
            int count = _Queue.Count;

            while (count > 1)
            {
                if (!_Queue.TryDequeue(out var bmp)) return false;
                _Pool.Enqueue(bmp);
            }

            return TryDequeue(reader);
        }

        public bool TryPeek(Action<MemoryBitmap> reader)
        {
            if (_Queue.TryPeek(out var bmp)) { reader(bmp); return true; }
            return false;
        }

        public bool TryDequeue(Action<MemoryBitmap> reader)
        {
            if (!_Queue.TryDequeue(out var bmp)) return false;

            PostprocessFrame(bmp);

            reader(bmp);

            _LastDequeued = bmp;

            ++_TotalFramesDequeued;

            return true;
        }

        /// <summary>
        /// This method is raised after dequeuing the frame and before giving it to the client.
        /// </summary>
        /// <param name="bmp">The image to postprocess</param>
        protected virtual void PostprocessFrame(MemoryBitmap bmp) { }        

        #endregion

        #region API - FPS counter        

        private static readonly System.Diagnostics.Stopwatch _Timer = System.Diagnostics.Stopwatch.StartNew();
        private static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);
        private readonly System.Collections.Concurrent.ConcurrentQueue<TimeSpan> _Elapsed = new System.Collections.Concurrent.ConcurrentQueue<TimeSpan>();

        /// <summary>
        /// Gets the number of frames enqueued in the last second.
        /// </summary>
        public int FrameRate
        {
            get
            {
                // removed old ones.

                var t = _Timer.Elapsed;

                while (_Elapsed.TryPeek(out TimeSpan last))
                {
                    if (t - last <= OneSecond) break;
                    if (!_Elapsed.TryDequeue(out _)) break;
                }

                return _Elapsed.Count;
            }
        }

        private void _AddFps()
        {            
            _Elapsed.Enqueue(_Timer.Elapsed);
        }

        #endregion
    }
}
