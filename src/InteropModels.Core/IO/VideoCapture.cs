using System;
using System.Collections.Generic;
using System.Text;

using InteropBitmaps;

namespace InteropModels.IO
{
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

        #endregion

        #region API

        public abstract void Dispose();
        public abstract void Start();        
        public abstract void Pause();

        protected abstract bool GetMirrorHorizontal();

        protected void RaiseFrameReceived(PointerBitmap bmp)
        {
            var time = FrameTime.Now;

            if (!_IsMirroredHorizontal.HasValue) _IsMirroredHorizontal = GetMirrorHorizontal();

            var evt = new VideoCaptureFrame();
            evt.Bitmap = bmp;
            evt.TimeStamp = time;

            evt.Source = this;            

            OnFrameReceived?.Invoke(this, evt);

            _FPS.AddFrame();
        }

        #endregion

        #region data

        private readonly FrameRateCounter _FPS = new FrameRateCounter();

        internal bool? _IsMirroredHorizontal;

        #endregion

        #region properties

        public int FrameRate => _FPS.FrameRate;

        public event EventHandler<VideoCaptureFrame> OnFrameReceived;                

        #endregion
    }

    public class VideoCaptureFrame : EventArgs
    {
        // todo: add horizontal mirroring info.
        // todo: add intrinsics info.        

        public static implicit operator VideoCaptureFrame((PointerBitmap b, FrameTime t) frame)
        {
            return new VideoCaptureFrame(frame.b, frame.t);
        }

        public static implicit operator VideoCaptureFrame((PointerBitmap b, TimeSpan t) frame)
        {
            return new VideoCaptureFrame(frame.b, new FrameTime(frame.t));
        }

        public VideoCaptureFrame() { }

        public VideoCaptureFrame(PointerBitmap bmp, FrameTime ts)
        {
            Bitmap = bmp;
            TimeStamp = ts;
        }

        public FrameTime TimeStamp { get; internal set; }
        public PointerBitmap Bitmap { get; internal set; }        

        public VideoCaptureDevice Source { get; internal set; }

        public bool IsMirroredHorizontal => Source?._IsMirroredHorizontal ?? true;

        public VideoCaptureFrame Slice(BitmapBounds bounds)
        {
            bounds = bounds.Clipped((System.Drawing.Point.Empty, this.Bitmap.Size));

            if (bounds.Area == 0) return new VideoCaptureFrame
            {
                Bitmap = default
             ,
                TimeStamp = this.TimeStamp
            };

            return new VideoCaptureFrame
            {
                Bitmap = this.Bitmap.Slice(bounds)
            ,
                TimeStamp = this.TimeStamp
            };
        }

        public interface ISource
        {
            event EventHandler<VideoCaptureFrame> OnFrameReceived;
        }
    }
}
