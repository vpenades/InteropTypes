using System;
using System.Collections.Generic;
using System.Text;

using InteropBitmaps;

namespace InteropVision.IO
{
    /// <summary>
    /// Represents the base class for an object or device able to stream a sequence of video frames.
    /// </summary>
    public abstract class VideoCaptureDevice :
        IDisposable,
        VideoCaptureFrameArgs.ISource
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

            var time = FrameTime.Now;
            _FPS.AddFrame();

            if (_CurrentFrame.Value.Device != this) throw new InvalidOperationException("Owner mismatch");

            _CurrentFrame.Value._TimeStamp = time;
            _CurrentFrame.Value._Bitmap = bmp;            

            OnFrameReceived?.Invoke(this, _CurrentFrame.Value);
        }
        
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
    public class VideoCaptureFrameArgs : EventArgs
    {
        #region lifecycle
        
        /// <summary>
        /// Used mostly for testing
        /// </summary>        
        public static implicit operator VideoCaptureFrameArgs((PointerBitmap b, FrameTime t) frame)
        {
            return new VideoCaptureFrameArgs(frame.b, frame.t);
        }

        /// <summary>
        /// Used mostly for testing
        /// </summary>        
        public static implicit operator VideoCaptureFrameArgs((PointerBitmap b, TimeSpan t) frame)
        {
            return new VideoCaptureFrameArgs(frame.b, new FrameTime(frame.t));
        }

        protected VideoCaptureFrameArgs(ISource device) { _Device = device; }

        private VideoCaptureFrameArgs(PointerBitmap bmp, FrameTime ts)
        {
            _Device = null;
            _Bitmap = bmp;
            _TimeStamp = ts;
        }

        #endregion

        #region data

        private readonly ISource _Device;

        /// <summary>
        /// Updated by <see cref="VideoCaptureDevice.RaiseFrameReceived(PointerBitmap)"/>
        /// </summary>
        internal FrameTime _TimeStamp;

        /// <summary>
        /// Updated by <see cref="VideoCaptureDevice.RaiseFrameReceived(PointerBitmap)"/>
        /// </summary>
        internal PointerBitmap _Bitmap;

        #endregion

        #region properties

        public ISource Device => _Device;
        public FrameTime TimeStamp => _TimeStamp;
        public PointerBitmap Bitmap => _Bitmap;        

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

        public readonly struct Cropped
        {
            public Cropped(VideoCaptureFrameArgs frame, BitmapBounds bounds)
            {
                Source = frame;
                Bounds = bounds;
            }

            public readonly VideoCaptureFrameArgs Source;
            public readonly BitmapBounds Bounds;

            public FrameTime TimeStamp => Source.TimeStamp;

            /// <summary>
            /// The cropped bitmap
            /// </summary>
            public PointerBitmap Bitmap => Source.Bitmap.Slice(Bounds);

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
