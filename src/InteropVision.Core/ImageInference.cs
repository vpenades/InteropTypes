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
    [System.Diagnostics.DebuggerDisplay("{Time.RelativeTime}")]
    public class InferenceInput<TImage>
    {
        #region constructor

        public static implicit operator InferenceInput<TImage>(TImage frameImage)
        {
            return new InferenceInput<TImage>(frameImage);
        }

        public static implicit operator InferenceInput<TImage>((TImage Image, TimeSpan Time) frame)
        {
            return new InferenceInput<TImage>(frame.Image, new FrameTime(frame.Time));
        }

        public static implicit operator InferenceInput<TImage>((TImage Image, FrameTime Time) frame)
        {
            return new InferenceInput<TImage>(frame.Image, frame.Time);
        }

        public InferenceInput(TImage frameImage)
        {
            Image = frameImage;
            Time = FrameTime.Now;
        }

        public InferenceInput(TImage frameImage, FrameTime frameTime)
        {
            Image = frameImage;
            Time = frameTime;
        }

        #endregion

        #region data
        public FrameTime Time { get; private set; }
        public TImage Image { get; private set; }        

        #endregion

        #region API

        public void Update(TImage frameImage)
        {
            Time = FrameTime.Now;
            Image = frameImage;            
        }

        public void Update(TImage frameImage, FrameTime frameTime)
        {
            Time = frameTime;
            Image = frameImage;
        }

        #endregion
    }

    /// <inheritdoc/>
    [System.Diagnostics.DebuggerDisplay("{Time.RelativeTime}: {Image.Info.ToDebuggerDisplayString(),nq}")]
    public class InferenceInput : InferenceInput<InteropBitmaps.PointerBitmap>
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
            : base(sourceImage) { }

        public InferenceInput(InteropBitmaps.PointerBitmap sourceImage, FrameTime t)
            : base(sourceImage, t) { }

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
