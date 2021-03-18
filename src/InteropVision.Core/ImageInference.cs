using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Numerics;

using RECT = System.Drawing.Rectangle;


namespace InteropVision
{
    /// <summary>
    /// Represents the input image for an inference engine.
    /// </summary>
    public class InferenceInput
    {
        #region constructor

        public static implicit operator InferenceInput(InteropBitmaps.PointerBitmap sourceImage)
        {
            return new InferenceInput(sourceImage);
        }

        public static implicit operator InferenceInput((InteropBitmaps.PointerBitmap img, TimeSpan t) src)
        {
            return new InferenceInput(src.img, new FrameTime(src.t));
        }

        public static implicit operator InferenceInput((InteropBitmaps.PointerBitmap img, FrameTime t) src)
        {
            return new InferenceInput(src.img, src.t);
        }

        public InferenceInput(InteropBitmaps.PointerBitmap sourceImage)
        {
            Image = sourceImage;            
            Time = FrameTime.Now;
        }

        public InferenceInput(InteropBitmaps.PointerBitmap sourceImage, FrameTime t)
        {
            Image = sourceImage;            
            Time = t;
        }

        #endregion

        #region data

        public InteropBitmaps.PointerBitmap Image { get; private set; }        

        public FrameTime Time { get; private set; }

        #endregion

        #region API

        public InteropBitmaps.PointerBitmap GetWindow(ref RECT? window)
        {
            if (Image.IsEmpty) return default;

            if (!window.HasValue)
            {
                window = new RECT(Point.Empty, Image.Size);
                return Image;
            }

            if (window.Value.Width == 0) return default;
            if (window.Value.Height == 0) return default;

            var r = InteropBitmaps.BitmapBounds.Clip(window.Value, Image.Bounds);
            if (r.Width * r.Height == 0) return default;

            window = r;

            var tmp = Image.Slice(window.Value);

            System.Diagnostics.Debug.Assert(tmp.Size == window.Value.Size);

            return tmp;
        }

        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("{_Model}")]
    public abstract class ConcurrentImageInference<T> : IImageInference<T>
    {
        #region lifecycle

        protected virtual void SetModel(IModelGraph model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            _Release();

            lock (_Mutex)
            {
                _Model = model;
                _Session = model.CreateSession();
            }
        }

        public void Dispose() { Dispose(true); }

        protected virtual void Dispose(bool disposing) { if (disposing) { _Release(); } }

        private void _Release()
        {
            lock (_Mutex)
            {
                _Session?.Dispose();
                _Session = null;

                _Model?.Dispose();
                _Model = null;
            }
        }

        #endregion

        #region data

        private readonly object _Mutex = new object();

        private IModelGraph _Model;
        private IModelSession _Session;

        #endregion

        #region API

        public void Inference(T result, InferenceInput input, RECT? inputWindow = null)
        {
            lock(_Mutex)
            {
                Inference(_Session, input, inputWindow);
            }
        }

        protected abstract void Inference(IModelSession session, InferenceInput input, RECT? inputWindow);

        #endregion
    }

    /// <summary>
    /// Represents how the input image is to be copied to the tensor
    /// </summary>
    public class TensorImageSettings
    {
        #region constructors

        public static TensorImageSettings PassThrough
        {
            get
            {
                return new TensorImageSettings
                {
                    DepthMean = 0,
                    DepthScale = 1,
                    ColorScale = new Vector3(104.0f, 177.0f, 123.0f)
                };
            }
        }

        public static TensorImageSettings Normalized
        {
            get
            {
                return new TensorImageSettings
                {
                    DepthMean = 0,
                    DepthScale = 1.0f / 255.0f,
                    ColorScale = new Vector3(104.0f, 177.0f, 123.0f)
                };
            }
        }

        public TensorImageSettings WithInputSize(int w, int h)
        {
            var clone = this;
            clone.InputSize = new System.Drawing.Size(w, h);
            return clone;
        }

        #endregion

        #region data

        public float DepthMean;
        public float DepthScale;
        public Vector3 ColorScale;

        public System.Drawing.Size InputSize;

        #endregion        
    }
}
