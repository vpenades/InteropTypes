using System;
using System.Collections.Generic;
using System.Text;

using InteropBitmaps;

namespace InteropVision.IO
{
    /// <summary>
    /// Represents the base class for an object or device able to stream a sequence of video frames.
    /// </summary>
    public abstract class VideoCaptureDevice : IDisposable, VideoCaptureFrame.ISource
    {
        #region lifecycle

        private static Func<VideoCaptureDevice> _DefaultDeviceFactory;

        public static void SetDeviceFactory(Func<VideoCaptureDevice> factory) { _DefaultDeviceFactory = factory; }

        public static VideoCaptureDevice CreateDefaultCaptureDevice()
        {
            if (_DefaultDeviceFactory != null) return _DefaultDeviceFactory();

            return _ReflectionUtils.CreateDefaultCaptureDevice();
        }

        public abstract void Dispose();

        #endregion

        #region data

        private readonly FrameRateCounter _FPS = new FrameRateCounter();        

        private VideoCaptureFrame _CurrentFrame;

        #endregion

        #region API

        /// <summary>
        /// Starts or resumes the underlaying device.
        /// </summary>
        public void Start()
        {
            StartDevice();
        }

        /// <summary>
        /// Pauses the underlaying device.
        /// </summary>
        public void Pause()
        {
            PauseDevice();
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
        /// Called once
        /// </summary>
        /// <returns></returns>
        protected abstract VideoCaptureFrame CreateFrameContainer();

        /// <summary>
        /// Called by the underlaying device when it receives a new capture bitmap,
        /// it broadcasts an <see cref="VideoCaptureFrame"/> at <see cref="OnFrameReceived"/>.
        /// </summary>
        /// <param name="bmp"></param>
        protected void RaiseFrameReceived(PointerBitmap bmp)
        {
            var time = FrameTime.Now;
            _FPS.AddFrame();

            if (_CurrentFrame == null) _CurrentFrame = CreateFrameContainer();

            _CurrentFrame._TimeStamp = time;
            _CurrentFrame._Bitmap = bmp;            

            OnFrameReceived?.Invoke(this, _CurrentFrame);
        }

        #endregion        

        #region properties

        public int FrameRate => _FPS.FrameRate;

        public event EventHandler<VideoCaptureFrame> OnFrameReceived;                

        #endregion
    }

    public class VideoCaptureFrame : EventArgs
    {
        #region lifecycle
        
        /// <summary>
        /// Used mostly for testing
        /// </summary>        
        public static implicit operator VideoCaptureFrame((PointerBitmap b, FrameTime t) frame)
        {
            return new VideoCaptureFrame(frame.b, frame.t);
        }

        /// <summary>
        /// Used mostly for testing
        /// </summary>        
        public static implicit operator VideoCaptureFrame((PointerBitmap b, TimeSpan t) frame)
        {
            return new VideoCaptureFrame(frame.b, new FrameTime(frame.t));
        }

        protected VideoCaptureFrame(VideoCaptureDevice device) { _Device = device; }

        private VideoCaptureFrame(PointerBitmap bmp, FrameTime ts)
        {
            _Device = null;
            _Bitmap = bmp;
            _TimeStamp = ts;
        }

        #endregion

        #region data

        private readonly VideoCaptureDevice _Device;

        internal FrameTime _TimeStamp;

        internal PointerBitmap _Bitmap;        

        #endregion

        #region properties

        public FrameTime TimeStamp => _TimeStamp;
        public PointerBitmap Bitmap => _Bitmap;
        public VideoCaptureDevice Device => _Device;

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
            event EventHandler<VideoCaptureFrame> OnFrameReceived;
        }

        public readonly struct Cropped
        {
            public Cropped(VideoCaptureFrame frame, BitmapBounds bounds)
            {
                Source = frame;
                Bounds = bounds;
            }

            public readonly VideoCaptureFrame Source;
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
