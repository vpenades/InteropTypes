using System;
using System.Numerics;

using RECT = System.Drawing.Rectangle;
using PTRBMP = InteropBitmaps.PointerBitmap;

namespace InteropVision
{
    /// <summary>
    /// Represents the input for an inference engine.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{ToDebuggerDisplayString(),nq}")]
    public class InferenceInput<T>
    {
        #region diagnostics

        protected string ToDebuggerDisplayString()
        {
            var text = Device?.Name ?? string.Empty;
            if (text.Length > 0) text += "/";

            text += Time.ToString();
            text += "/";

            if (Content is PTRBMP ptrBmp)
            {
                text += ptrBmp.Info.ToDebuggerDisplayString();
            }
            else if (Content is InteropBitmaps.MemoryBitmap memBmp)
            {
                text += memBmp.Info.ToDebuggerDisplayString();
            }
            else
            {
                text += Device?.ToString() ?? "<NULL>";
            }

            return text;
        }

        #endregion

        #region constructor

        public static implicit operator InferenceInput<T>(T frameImage)
        {
            return new InferenceInput<T>(frameImage);
        }        

        public static implicit operator InferenceInput<T>((T Image, DateTime Time) frame)
        {
            return new InferenceInput<T>(frame.Image, frame.Time);
        }

        public InferenceInput(IO.ICaptureDeviceInfo device)
        {
            Device = device;
        }

        public InferenceInput(T frameImage)
        {
            Content = frameImage;
            Time = DateTime.UtcNow;
        }

        public InferenceInput(T frameImage, DateTime frameTime)
        {
            Content = frameImage;
            Time = frameTime;
        }

        #endregion

        #region data
        public IO.ICaptureDeviceInfo Device { get; private set; }
        public DateTime Time { get; private set; }
        public T Content { get; private set; }        

        #endregion

        #region API        

        public void Update(T frameImage)
        {
            Time = DateTime.UtcNow;
            Content = frameImage;            
        }

        public void Update(T frameImage, DateTime frameTime)
        {
            Time = frameTime;
            Content = frameImage;
        }

        #endregion
    }

    /// <inheritdoc/>
    [System.Diagnostics.DebuggerDisplay("{ToDebuggerDisplayString(),nq}")]
    public class PointerBitmapInput : InferenceInput<PTRBMP>
    {
        #region constructor

        public static implicit operator PointerBitmapInput(PTRBMP sourceImage)
        {
            return new PointerBitmapInput(sourceImage);
        }        

        public static implicit operator PointerBitmapInput((PTRBMP img, DateTime t) src)
        {
            return new PointerBitmapInput(src.img, src.t);
        }

        public PointerBitmapInput(PTRBMP sourceImage)
            : base(sourceImage) { }

        public PointerBitmapInput(PTRBMP sourceImage, DateTime t)
            : base(sourceImage, t) { }

        #endregion                
    }    

    /// <summary>
    /// Represents how the input image needs to be formatted before
    /// copying it to the input tensor.
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
