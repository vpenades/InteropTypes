using System;

namespace InteropBitmaps
{
    public static partial class OpenCvSharp4Toolkit
    {
        #region WithOpenCv

        public static Adapters.OpenCvSharp4SpanAdapter WithOpenCv(this MemoryBitmap bmp) { return new Adapters.OpenCvSharp4SpanAdapter(bmp); }

        public static Adapters.OpenCvSharp4SpanAdapter WithOpenCv(this SpanBitmap bmp) { return new Adapters.OpenCvSharp4SpanAdapter(bmp); }

        public static Adapters.OpenCvSharp4SpanAdapter WithOpenCv<TPixel>(this SpanBitmap<TPixel> bmp)
            where TPixel : unmanaged
        { return new Adapters.OpenCvSharp4SpanAdapter(bmp.AsTypeless()); }

        public static Adapters.OpenCvSharp4SpanAdapter WithOpenCv<TPixel>(this MemoryBitmap<TPixel> bmp)
            where TPixel : unmanaged
        { return new Adapters.OpenCvSharp4SpanAdapter(bmp.AsSpanBitmap()); }

        public static Adapters.OpenCvSharp4MemoryAdapter UsingOpenCv(this MemoryBitmap bmp)
        {
            return new Adapters.OpenCvSharp4MemoryAdapter(bmp);
        }

        #endregion

        #region API        

        public static SpanBitmap AsSpanBitmap(this OpenCvSharp.Mat src)
        {
            return _Implementation.TryWrapAsPointer(src, out var span)
                ? span
                : throw new ArgumentException(nameof(src));
        }

        public static SpanBitmap<TPixel> AsSpanBitmap<TPixel>(this OpenCvSharp.Mat<TPixel> src)
            where TPixel : unmanaged
        {
            return _Implementation.TryWrapAsPointer(src, out var dstPtr)
                ? dstPtr.AsSpanBitmap().OfType<TPixel>()
                : throw new ArgumentException(nameof(src));
        }

        public static OpenCvSharp.Mat WrapAsMat(this PointerBitmap src)
        {
            return _Implementation.WrapAsMat(src);
        }

        public static OpenCvSharp.Mat<TPixel> CloneAsMat<TPixel>(this SpanBitmap<TPixel> srcSpan)
            where TPixel:unmanaged
        {
            return _Implementation.CloneAsMat(srcSpan);
        }

        #endregion

        #region transforms

        public static void WarpAffine(this SpanBitmap src, SpanBitmap dst, System.Numerics.Matrix3x2 xform)
        {
            _Implementation.TransferCv(src, dst, (s, d) => _Implementation.WarpAffine(s, d, xform));
        }
        public static void WarpAffine(this PointerBitmap src, PointerBitmap dst, System.Numerics.Matrix3x2 xform)
        {
            _Implementation.TransferCv(src, dst, (s, d) => _Implementation.WarpAffine(s, d, xform));
        }

        #endregion
    }
}
