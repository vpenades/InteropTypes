using System;
using System.Numerics;

using InteropTypes.Graphics;
using InteropTypes.Graphics.Bitmaps;

using CAPINFO = InteropTypes.Vision.IO.ICaptureDeviceInfo;
using CAPTIME = System.DateTime;

using PTRBMP = InteropTypes.Graphics.Bitmaps.PointerBitmap;
using MEMBMP = InteropTypes.Graphics.Bitmaps.MemoryBitmap;

using RECT = System.Drawing.Rectangle;

namespace InteropTypes.Vision
{
    /// <summary>
    /// Represents the input for an inference engine.
    /// </summary>    
    [System.Diagnostics.DebuggerDisplay("{ToDebuggerDisplayString(),nq}")]
    public class InferenceInput<TContent>
    {
        #region diagnostics

        protected string ToDebuggerDisplayString()
        {
            var text = CaptureDeviceName;
            if (text.Length > 0) text += "/";

            text += CaptureTime.ToString();
            text += "/";

            if (Content is PTRBMP ptrBmp)
            {
                text += ptrBmp.Info.ToDebuggerDisplayString();
            }
            else if (Content is BitmapInfo.ISource bmpNfo)
            {
                text += bmpNfo.Info.ToDebuggerDisplayString();
            }
            else
            {
                text += Content?.ToString() ?? "<NULL>";
            }

            return text;
        }

        #endregion

        #region constructor

        public static implicit operator InferenceInput<TContent>(TContent sourceImage)
        {            
            return new InferenceInput<TContent>(sourceImage);
        }

        public static implicit operator InferenceInput<TContent>((CAPTIME Time, TContent Bitmap) src)
        {         
            return new InferenceInput<TContent>(src.Time, src.Bitmap);
        }

        public static implicit operator InferenceInput<TContent>((CAPINFO info, CAPTIME Time, TContent Bitmap) src)
        {         
            return new InferenceInput<TContent>(src.info, src.Time, src.Bitmap);
        }

        public static implicit operator InferenceInput<TContent>((string info, CAPTIME Time, TContent Bitmap) src)
        {            
            return new InferenceInput<TContent>(new IO._CaptureDeviceInfo(src.info, src.Time), src.Time, src.Bitmap);
        }
        
        public InferenceInput(TContent content)
            : this(null, DateTime.UtcNow, content) { }

        public InferenceInput(CAPTIME captureTime, TContent content)
            : this(null, captureTime, content) { }        

        public InferenceInput(CAPINFO info, CAPTIME captureTime, TContent content)
        {
            this.CaptureDevice = info;
            this.CaptureTime = captureTime;
            this.Content = content;            
        }

        #endregion

        #region data
        public CAPINFO CaptureDevice { get; private set; }
        public CAPTIME CaptureTime { get; private set; }
        public TContent Content { get; private set; }

        #endregion

        #region properties

        public string CaptureDeviceName => CaptureDevice?.Name ?? "<UNKNOWN>";

        #endregion

        #region API        

        public void Update(TContent content) { Update(CAPTIME.UtcNow, content); }

        public virtual void Update(CAPTIME captureTime, TContent content)
        {
            CaptureTime = captureTime;
            Content = content;
        }

        #endregion

        #region specific

        [Obsolete("Use GetClippedPointerBitmap")]
        public PTRBMP GetWindow(ref RECT? window)
        {
            return GetClippedPointerBitmap(ref window);
        }

        /// <summary>
        /// If <see cref="Content"/> is of type <see cref="PTRBMP"/> and
        /// <paramref name="window"/> is defined, the bitmap is clipped, and
        /// <paramref name="window"/> is also updated to ensure it is contained
        /// within the original unclipped bitmap bounds.
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public PTRBMP GetClippedPointerBitmap(ref RECT? window)
        {
            if (!(this.Content is PTRBMP inputBitmap)) throw new NotSupportedException(); 

            if (inputBitmap.IsEmpty) return default;

            if (!window.HasValue)
            {
                window = new RECT(System.Drawing.Point.Empty, inputBitmap.Size);
                return inputBitmap;
            }

            if (window.Value.Width == 0) return default;
            if (window.Value.Height == 0) return default;

            var r = BitmapBounds.Clip(window.Value, inputBitmap.Bounds);
            if (r.Width * r.Height == 0) return default;

            window = r;

            var tmp = inputBitmap.Slice(window.Value);

            System.Diagnostics.Debug.Assert(tmp.Size == window.Value.Size);

            return tmp;
        }

        #endregion
    }

    /// <summary>
    /// Represents the input for an inference engine, which can be pinned to a PointerBitmap
    /// </summary>
    /// <remarks>
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{ToDebuggerDisplayString(),nq}")]
    public class PointerBitmapInput : InferenceInput<Object>
    {
        #region constructors        

        public static implicit operator PointerBitmapInput(MEMBMP src)
        {
            return new PointerBitmapInput(src);
        }

        public static implicit operator PointerBitmapInput((CAPTIME Time, MEMBMP Bitmap) src)
        {
            return new PointerBitmapInput(src.Time, src.Bitmap);
        }

        public static implicit operator PointerBitmapInput((CAPINFO info, CAPTIME Time, MEMBMP Bitmap) src)
        {
            return new PointerBitmapInput(src.info, src.Time, src.Bitmap);
        }

        public static implicit operator PointerBitmapInput((string info, CAPTIME Time, MEMBMP Bitmap) src)
        {
            return new PointerBitmapInput(new IO._CaptureDeviceInfo(src.info, src.Time), src.Time, src.Bitmap);
        }

        public static implicit operator PointerBitmapInput(PTRBMP src)
        {
            return new PointerBitmapInput(src);
        }

        public static implicit operator PointerBitmapInput((CAPTIME Time, PTRBMP Bitmap) src)
        {
            return new PointerBitmapInput(src.Time, src.Bitmap);
        }

        public static implicit operator PointerBitmapInput((CAPINFO info, CAPTIME Time, PTRBMP Bitmap) src)
        {
            return new PointerBitmapInput(src.info, src.Time, src.Bitmap);
        }

        public static implicit operator PointerBitmapInput((string info, CAPTIME Time, PTRBMP Bitmap) src)
        {
            return new PointerBitmapInput(new IO._CaptureDeviceInfo(src.info, src.Time), src.Time, src.Bitmap);
        }

        public PointerBitmapInput(Object content)
            : base(null, CAPTIME.UtcNow, content) { }

        public PointerBitmapInput(CAPTIME captureTime, Object content)
            : base(null, captureTime, content) { }

        public PointerBitmapInput(CAPINFO info, CAPTIME captureTime, Object content)
            : base(info, captureTime, content) { }

        #endregion

        #region data

        private MEMBMP _TempBitmap;

        #endregion

        #region API

        public void PinInput(Action<InferenceInput<PTRBMP>> onInputPinned)
        {
            PinBitmap(ptrBmp => onInputPinned((this.CaptureDevice, this.CaptureTime, ptrBmp)));
        }

        public void PinInput(PixelFormat expectedFormat, Action<InferenceInput<PTRBMP>> onInputPinned)
        {
            PinBitmap(expectedFormat, ptrBmp => onInputPinned((this.CaptureDevice, this.CaptureTime, ptrBmp)));
        }

        /// <summary>
        /// Override if you need to convert <see cref="InferenceInput{TContent}.Content"/> to <see cref="PTRBMP"/>.
        /// </summary>
        /// <param name="onBitmapPinned"></param>
        protected virtual void PinBitmap(Action<PTRBMP> onBitmapPinned)
        {
            if (this.Content is PTRBMP ptrBmp) onBitmapPinned(ptrBmp);

            if (this.Content is MEMBMP memBmp)
            {
                memBmp.AsSpanBitmap().PinReadablePointer(onBitmapPinned);
                return;
            }

            if (this.Content is MemoryBitmap<Pixel.BGR24> bgr24Bmp)
            {
                bgr24Bmp.AsSpanBitmap().PinReadablePointer(onBitmapPinned);
                return;
            }

            if (this.Content is MemoryBitmap<Pixel.RGBA32> rgba32Bmp)
            {
                rgba32Bmp.AsSpanBitmap().PinReadablePointer(onBitmapPinned);
                return;
            }

            if (this.Content is MemoryBitmap<Pixel.BGRA32> bgra32Bmp)
            {
                bgra32Bmp.AsSpanBitmap().PinReadablePointer(onBitmapPinned);
                return;
            }

            if (this.Content is MemoryBitmap<Pixel.ARGB32> argb32Bmp)
            {
                argb32Bmp.AsSpanBitmap().PinReadablePointer(onBitmapPinned);
                return;
            }

            throw new NotSupportedException(this.Content?.GetType()?.ToString() ?? "null");
        }

        /// <summary>
        /// Override if you can perform a faster conversion than the one provided by default.
        /// </summary>
        /// <param name="expectedFormat">The pixel format required to be used by the pointer bitmap.</param>
        /// <param name="onBitmapPinned">Action called to provide the pointer bitmap.</param>
        protected virtual void PinBitmap(PixelFormat expectedFormat, Action<PTRBMP> onBitmapPinned)
        {
            PinBitmap(ptrBmp => _OnPin(expectedFormat, ptrBmp, onBitmapPinned));            
        }

        private void _OnPin(PixelFormat expectedFormat, PTRBMP srcBitmap, Action<PTRBMP> onBitmapPinned)
        {
            if (srcBitmap.PixelFormat == expectedFormat) { onBitmapPinned(srcBitmap); return; }

            srcBitmap.AsSpanBitmap().CopyTo(ref _TempBitmap, expectedFormat);

            _TempBitmap.AsSpanBitmap().PinReadablePointer(onBitmapPinned);
        }

        #endregion

        #region nested types

        public interface IInference<TResult> :
            IDisposable
            where TResult : class
        {
            void Inference(TResult result, PointerBitmapInput input, RECT? inputWindow = null);
        }

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
