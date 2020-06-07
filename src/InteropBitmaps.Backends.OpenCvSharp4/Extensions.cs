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

        public static unsafe SpanBitmap AsSpanBitmap(this OpenCvSharp.Mat src) { return _Implementation.WrapAsSpanBitmap(src); }

        public static unsafe SpanBitmap<TPixel> AsSpanBitmap<TPixel>(this OpenCvSharp.Mat<TPixel> src)
            where TPixel : unmanaged
        { return _Implementation.WrapAsSpanBitmap(src); }

        public static OpenCvSharp.Mat AsMat(this PointerBitmap src)
        {
            return _Implementation.WrapAsMat(src);
        }

        public static unsafe OpenCvSharp.Mat<TPixel> ToMat<TPixel>(this SpanBitmap<TPixel> srcSpan)
            where TPixel:unmanaged
        { return _Implementation.CloneToMat(srcSpan); }           

        #endregion
    }
}
