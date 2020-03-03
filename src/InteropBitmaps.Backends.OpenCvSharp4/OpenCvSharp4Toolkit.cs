using System;

namespace InteropBitmaps
{
    public static partial class OpenCvSharp4Toolkit
    {
        // https://github.com/shimat/opencvsharp/wiki/Accessing-Pixel

        public static OpenCvSharp4Adapter AsOpenCVSharp(this MemoryBitmap bmp) { return new OpenCvSharp4Adapter(bmp); }

        public static OpenCvSharp4Adapter AsOpenCVSharp(this SpanBitmap bmp) { return new OpenCvSharp4Adapter(bmp); }

        public static OpenCvSharp4Adapter AsOpenCVSharp<TPixel>(this SpanBitmap<TPixel> bmp)
            where TPixel : unmanaged
        {
            return new OpenCvSharp4Adapter(bmp.AsSpanBitmap());
        }

        public static unsafe SpanBitmap AsSpanBitmap(this OpenCvSharp.Mat src) { return _Implementation.AsSpanBitmap(src); }

        public static unsafe SpanBitmap<TPixel> AsSpanBitmap<TPixel>(this OpenCvSharp.Mat<TPixel> src)
            where TPixel : unmanaged
        {
            return _Implementation.AsSpanBitmap(src);
        }

        public static unsafe OpenCvSharp.Mat<TPixel> CreateMat<TPixel>(this SpanBitmap<TPixel> srcSpan)
            where TPixel:unmanaged
        {
            var dst = new OpenCvSharp.Mat<TPixel>(srcSpan.Height, srcSpan.Width);
            var dstSpan = dst.AsSpanBitmap<TPixel>();
            dstSpan.SetPixels(0, 0, srcSpan);
            return dst;            
        }

        public static OpenCvSharp.Mat AsMat(this in PointerBitmap src)
        {
            var mtype = OpenCvSharp.MatType.CV_8UC(src.Info.PixelSize);

            return new OpenCvSharp.Mat(src.Info.Height, src.Info.Width, mtype, src.Pointer, src.Info.ScanlineSize);
        }

        public static OpenCvSharp.Mat CreateMat(this in BitmapInfo desc)
        {
            var mtype = OpenCvSharp.MatType.CV_8UC(desc.PixelSize);

            return new OpenCvSharp.Mat(desc.Height, desc.Width, mtype);
        }
        
    }
}
