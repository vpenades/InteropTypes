using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Vision.IO
{
    /// <summary>
    /// Represents the base class for an object or device able to stream a sequence of video frames.
    /// </summary>
    /// <remarks>
    /// Classes deriving from <see cref="VideoCaptureDevice"/> must also implement:
    /// <list type="table">
    /// <item><see cref="ICaptureDeviceInfo"/></item>
    /// </list>
    /// </remarks>
    public abstract class VideoCaptureDevice :
        VideoCaptureFrameArgs.ISource,
        IDisposable        
    {
        #region lifecycle

        private static Func<VideoCaptureDevice> _DefaultDeviceFactory;

        public static void SetDeviceFactory(Func<VideoCaptureDevice> factory) { _DefaultDeviceFactory = factory; }

        public static VideoCaptureDevice CreateDefaultCaptureDevice()
        {
            if (_DefaultDeviceFactory != null) return _DefaultDeviceFactory();

            return _ReflectionUtils.CreateDefaultCaptureDevice();
        }

        protected VideoCaptureDevice()
        {
            _CurrentFrame = new Lazy<VideoCaptureFrameArgs>(CreateFrameContainer, true);
        }

        /// <summary>
        /// Called once by the underlaying implementation to construct an object<br/>
        /// derived from <see cref="VideoCaptureFrameArgs"/>
        /// </summary>
        /// <returns></returns>
        protected abstract VideoCaptureFrameArgs CreateFrameContainer();

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            _Disposed = true;
        }
        
        protected abstract void Dispose(bool disposing);

        #endregion

        #region data

        private readonly FrameRateCounter _FPS = new FrameRateCounter();        

        private readonly Lazy<VideoCaptureFrameArgs> _CurrentFrame;

        private bool _Disposed;

        #endregion

        #region API

        private bool _CheckDisposed()
        {
            System.Diagnostics.Debug.Assert(!_Disposed);
            if (_Disposed)
            {
                LastException = new ObjectDisposedException(nameof(VideoCaptureDevice));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Starts or resumes the underlaying device.
        /// </summary>
        public bool Start()
        {
            if (_CheckDisposed()) return false;

            try
            {
                StartDevice();
                LastException = null;
                return true;
            }
            catch(Exception ex)
            {                
                RaiseError(ex);
                return false;
            }
        }

        /// <summary>
        /// Pauses the underlaying device.
        /// </summary>
        public bool Pause()
        {
            if (_CheckDisposed()) return false;

            try
            {
                PauseDevice();
                LastException = null;
                return true;
            }
            catch(Exception ex)
            {                
                RaiseError(ex);
                return false;
            }
        }

        /// <summary>
        /// Starts or resumes the underlaying device.
        /// </summary>
        protected abstract void StartDevice();

        /// <summary>
        /// Pauses the underlaying device.
        /// </summary>
        protected abstract void PauseDevice();

        /// <summary>
        /// Called by the underlaying device when it receives a new capture bitmap,
        /// it broadcasts an <see cref="VideoCaptureFrameArgs"/> at <see cref="OnFrameReceived"/>.
        /// </summary>
        /// <param name="bmp"></param>
        protected void RaiseFrameReceived(PointerBitmap bmp)
        {
            if (_CheckDisposed()) return;

            var time = DateTime.UtcNow;
            _FPS.AddFrame();

            if (_CurrentFrame.Value.CaptureDevice != this) throw new InvalidOperationException("Owner mismatch");

            _CurrentFrame.Value._CaptureTime = time;
            _CurrentFrame.Value._CapturedBitmap = bmp;            

            OnFrameReceived?.Invoke(this, _CurrentFrame.Value);
        }
        
        /// <summary>
        /// Stores <paramref name="ex"/> in <see cref="LastException"/> and invokes <see cref="OnError"/>.
        /// </summary>
        /// <param name="ex">The exception to raise</param>
        protected void RaiseError(Exception ex)
        {
            if (ex == null) return;
            LastException = ex;
            OnError?.Invoke(this, new System.IO.ErrorEventArgs(ex));
        }

        #endregion        

        #region properties

        /// <summary>
        /// Gets the last exception, or null
        /// </summary>
        public Exception LastException { get; private set; }

        /// <summary>
        /// Gets the current realtime framerate
        /// </summary>
        public int FrameRate => _FPS.FrameRate;

        /// <inheritdoc/>
        public event EventHandler<VideoCaptureFrameArgs> OnFrameReceived;

        public event EventHandler<System.IO.ErrorEventArgs> OnError;

        #endregion
    }

    /// <summary>
    /// Represents a video capture frame container.
    /// </summary>
    /// <remarks>
    /// This container is created only once, and updated every new frame is received.<br/>
    /// so it must not be used to keep multiple frames in a collection.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{CaptureDevice}/{CaptureTime.RelativeTime}/{Bitmap.Info.ToDebuggerDisplayString(),nq}")]
    public class VideoCaptureFrameArgs : EventArgs
    {
        #region lifecycle        

        public static implicit operator VideoCaptureFrameArgs(InferenceInput<PointerBitmap> frame)
        {
            var args = new VideoCaptureFrameArgs(frame.CaptureDevice as ISource);
            args._CaptureTime = frame.CaptureTime;
            args._CapturedBitmap = frame.Content;
            return args;
        }

        /// <summary>
        /// Used mostly for testing
        /// </summary>        
        public static implicit operator VideoCaptureFrameArgs((DateTime t, PointerBitmap b) frame)
        {
            return new VideoCaptureFrameArgs(frame.t, frame.b);
        }

        protected VideoCaptureFrameArgs(ISource device)
        {
            _CaptureDevice = device;
            _CaptureTime = DateTime.UtcNow;
        }

        private VideoCaptureFrameArgs(DateTime ts, PointerBitmap bmp)
        {
            _CaptureDevice = new _SingleImageSource();
            _CapturedBitmap = bmp;
            _CaptureTime = ts;
        }

        #endregion

        #region data

        private readonly ISource _CaptureDevice;

        /// <summary>
        /// Updated by <see cref="VideoCaptureDevice.RaiseFrameReceived(PointerBitmap)"/>
        /// </summary>
        internal DateTime _CaptureTime;

        /// <summary>
        /// Updated by <see cref="VideoCaptureDevice.RaiseFrameReceived(PointerBitmap)"/>
        /// </summary>
        internal PointerBitmap _CapturedBitmap;

        #endregion

        #region properties

        /// <summary>
        /// Gets access to the underlaying capture device object, or null.
        /// </summary>
        /// <remarks>
        /// Try cast to <see cref="ICaptureDeviceInfo"/> to get additional information from the device.
        /// </remarks>
        public ISource CaptureDevice => _CaptureDevice;

        /// <summary>
        /// Gets the time at which this bitmap was captured.
        /// </summary>
        public DateTime CaptureTime => _CaptureTime;

        /// <summary>
        /// Gets the captured bitmap.
        /// </summary>
        public PointerBitmap Content => _CapturedBitmap;        

        #endregion

        #region API        
        
        public Cropped Crop(BitmapBounds bounds)
        {
            return new Cropped(this, bounds);
        }

        #endregion

        #region nested types

        // todo: ICameraIntrinsics
        // todo: ICameraExtrinsics  : imu, accelerometer, etc        

        public interface ISource
        {
            /// <summary>
            /// Raised every time a new <see cref="VideoCaptureFrameArgs"/> is received.
            /// </summary>
            event EventHandler<VideoCaptureFrameArgs> OnFrameReceived;
        }

        struct _SingleImageSource : ISource, ICaptureDeviceInfo
        {
            private DateTime? _CaptureTime;

            public string Name => "Single Frame";

            public DateTime CaptureStart
            {
                get
                {
                    if (!_CaptureTime.HasValue) _CaptureTime = DateTime.Now;
                    return _CaptureTime.Value;
                }
            }

            public event EventHandler<VideoCaptureFrameArgs> OnFrameReceived;
        }

        public readonly struct Cropped
        {
            public Cropped(VideoCaptureFrameArgs frame, BitmapBounds bounds)
            {
                Source = frame;
                Bounds = bounds;
            }

            public readonly VideoCaptureFrameArgs Source;
            public readonly BitmapBounds Bounds;

            public DateTime TimeStamp => Source.CaptureTime;

            /// <summary>
            /// The cropped bitmap
            /// </summary>
            public PointerBitmap Bitmap => Source.Content.Slice(Bounds);

            /// <summary>
            /// gets a cropped region of the current crop.
            /// </summary>
            /// <param name="bounds">The region to crop</param>
            /// <returns>A new cropped region</returns>
            public Cropped Crop(BitmapBounds bounds)
            {
                return new Cropped(Source, Bounds.Clipped(bounds));
            }
        }

        #endregion
    }
}
