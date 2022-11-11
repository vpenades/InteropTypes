using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace InteropTypes.Graphics.Bitmaps
{
    /// <summary>
    /// Bitmap that can be bound to a UI
    /// </summary>
    /// <remarks>
    /// Under WPF, using InteropTypes.Graphics.Backends.WPF:<br/>
    /// InteropTypes.Graphics.Backends.WPF.ImageEx can bind this object using BitmapSource property.<br/>
    /// Alternatively, it's possible to use DataTemplates to bind this object directly into a ContentControl's Content.
    /// </remarks>
    [DefaultProperty(nameof(Bitmap))]
    [System.Diagnostics.DebuggerDisplay("{Info.ToDebuggerDisplayString(),nq}")]
    public class BindableBitmap : INotifyPropertyChanged, SpanBitmap.ISource
    {
        #region lifecycle

        public static implicit operator BindableBitmap(MemoryBitmap bmp) { return new BindableBitmap(bmp); }

        public BindableBitmap() { }

        public BindableBitmap(MemoryBitmap bmp)
        {
            _Bitmap = bmp;
        }

        public BindableBitmap(MemoryBitmap bmp, PixelFormat format)
        {
            _Bitmap = bmp;
            _Format = format;
        }

        #endregion

        #region data

        private MemoryBitmap _Bitmap;        

        /// <summary>
        /// If not defined, <see cref="_Bitmap"/> will use the pixel format of the incoming bitmap.
        /// </summary>
        private PixelFormat? _Format;

        #endregion

        #region properties

        [Bindable(BindableSupport.Yes)]
        public MemoryBitmap Bitmap
        {
            get => _Bitmap;
            set
            {
                if (_Bitmap.Equals(value)) return;
                _Bitmap = value;
                Invalidate();
            }
        }

        [Bindable(BindableSupport.Yes)]
        public BitmapInfo Info => _Bitmap.Info;

        [Bindable(BindableSupport.Yes)]
        public System.Drawing.Size Size => Info.Size;

        [Bindable(BindableSupport.Yes)]
        public int Width => Info.Width;

        [Bindable(BindableSupport.Yes)]
        public int Height => Info.Height;


        public event PropertyChangedEventHandler PropertyChanged;        

        private static readonly PropertyChangedEventArgs _AllProperties = new PropertyChangedEventArgs(null);
        private static readonly PropertyChangedEventArgs _InfoProperty = new PropertyChangedEventArgs(nameof(Info));
        private static readonly PropertyChangedEventArgs _BitmapProperty = new PropertyChangedEventArgs(nameof(Bitmap));
        private static readonly PropertyChangedEventArgs _SizeProperty = new PropertyChangedEventArgs(nameof(Size));
        private static readonly PropertyChangedEventArgs _WidthProperty = new PropertyChangedEventArgs(nameof(Width));
        private static readonly PropertyChangedEventArgs _HeightProperty = new PropertyChangedEventArgs(nameof(Height));

        #endregion

        #region API

        /// <summary>
        /// Updates the bitmap from a thread other than the UI thread.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Under the hood, a copy of <paramref name="bmp"/> is created and enqueued, so the dispatcher can dequeue it.
        /// </para>
        /// <para>
        /// The returned action must be executed in the UI thread either by a dispatcher,
        /// or by the main loop in sequence.
        /// </para>        
        /// </remarks>
        /// <example>
        /// <code>
        /// System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke( bindable.Enqueue(bitmap) );
        /// </code>
        /// </example>
        /// <param name="bmp">The bitmap to set.</param>
        /// <returns>An action that needs to be executed by a dispatcher in the UI thread in order to complete the update.</returns>
        public Action Enqueue(SpanBitmap bmp, PixelFormat? format = null)
        {
            _Queue.Produce(bmp);

            void _update() { UpdateFromQueue(format); }

            return _update;
        }        

        /// <summary>
        /// If we have enqueued an image from another thread, we can call this method from the UI thread to dequeue it.
        /// </summary>
        /// <remarks>
        /// Always call from UI THREAD
        /// </remarks>
        /// <returns>true if there's still more frames into queue, false otherwise</returns>
        public bool UpdateFromQueue(PixelFormat? format = null, int repeat = int.MaxValue)
        {
            return _Queue.Consume(xbmp => Update(xbmp,format), repeat);
        }        

        /// <summary>
        /// Updates the underlaying image.
        /// </summary>
        /// <remarks>
        /// Always call from UI THREAD
        /// </remarks>
        /// <param name="srcBmp">the input image</param>
        /// <param name="dstFormat">overrides the source bitmat format.</param>
        public void Update(SpanBitmap srcBmp, PixelFormat? dstFormat = null)
        {
            var fmt = dstFormat ?? _Format;

            if (fmt.HasValue) srcBmp.CopyTo(ref _Bitmap, fmt.Value);
            else srcBmp.CopyTo(ref _Bitmap);

            Invalidate();
        }

        /// <summary>
        /// Updates the underlaying image.
        /// </summary>
        /// <remarks>
        /// Always call from UI THREAD
        /// </remarks>
        /// <param name="bmp">the input image</param>
        /// <param name="copyConverter">external custom copy conversion.</param>
        /// <param name="dstFormat">overrides the source bitmat format.</param>
        public void Update(SpanBitmap srcBmp, SpanBitmap.Action2 copyConverter, PixelFormat? dstFormat = null)
        {
            var fmt = dstFormat ?? _Format ?? srcBmp.PixelFormat;

            new BitmapInfo(srcBmp.Size, fmt).CreateBitmap(ref _Bitmap);

            copyConverter(srcBmp, _Bitmap);

            Invalidate();
        }

        /// <remarks>
        /// Always call from UI THREAD
        /// </remarks>
        public virtual void Invalidate()
        {
            PropertyChanged?.Invoke(this, _AllProperties);
            PropertyChanged?.Invoke(this, _InfoProperty);
            PropertyChanged?.Invoke(this, _SizeProperty);
            PropertyChanged?.Invoke(this, _WidthProperty);
            PropertyChanged?.Invoke(this, _HeightProperty);

            // update bitmap at the end
            PropertyChanged?.Invoke(this, _BitmapProperty);            
        }

        public SpanBitmap AsSpanBitmap()
        {
            return Bitmap.AsSpanBitmap();
        }

        #endregion

        #region dispatcher pool

        private readonly _ProducerConsumer _Queue = new _ProducerConsumer();
        private static void _NoAction() { }

        #endregion
    }

    class _ProducerConsumer
    {
        #region data

        private readonly ConcurrentBag<MemoryBitmap> _Pool = new ConcurrentBag<MemoryBitmap>();

        private readonly ConcurrentQueue<MemoryBitmap> _Queue = new ConcurrentQueue<MemoryBitmap>();

        #endregion

        #region properties

        public bool IsEmpty => _Queue.IsEmpty;

        #endregion

        #region API

        public void Produce(SpanBitmap bmp)
        {
            _Queue.Enqueue(_CopyFromPool(bmp));

            while (_Queue.Count > 2) // throttle
            {
                if (_Queue.TryDequeue(out var xbmp)) _ReturnToPool(xbmp);
            }
        }

        public bool Consume(SpanBitmap.Action1 updater, int repeat = int.MaxValue)
        {
            while (repeat > 0)
            {
                if (!_Queue.TryDequeue(out var xbmp)) break;

                updater(xbmp);
                _ReturnToPool(xbmp);
                --repeat;
            }

            return !_Queue.IsEmpty;
        }

        private MemoryBitmap _CopyFromPool(SpanBitmap bmp)
        {
            if (bmp.IsEmpty) return default;

            if (!_Pool.TryTake(out MemoryBitmap poolBmp)) poolBmp = default;

            bmp.CopyTo(ref poolBmp);
            return poolBmp;
        }

        private void _ReturnToPool(MemoryBitmap bmp)
        {
            if (bmp.IsEmpty) return;

            _Pool.Add(bmp);
        }

        #endregion
    }
}
