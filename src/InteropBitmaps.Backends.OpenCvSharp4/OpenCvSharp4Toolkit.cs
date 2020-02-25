using System;

namespace InteropBitmaps
{

    // https://github.com/shimat/opencvsharp/wiki/Accessing-Pixel
    public static partial class OpenCvSharp4Toolkit
    {
        public static OpenCvSharp4Adapter AsOpenCVSharp(this MemoryBitmap bmp)
        {
            return new OpenCvSharp4Adapter(bmp);
        }

        public static OpenCvSharp4Adapter AsOpenCVSharp(this SpanBitmap bmp)
        {
            return new OpenCvSharp4Adapter(bmp);
        }

        public static OpenCvSharp4Adapter AsOpenCVSharp<TPixel>(this SpanBitmap<TPixel> bmp) where TPixel : unmanaged
        {
            return new OpenCvSharp4Adapter(bmp.AsSpanBitmap());
        }

        public static unsafe SpanBitmap AsSpanBitmap(this OpenCvSharp.Mat src)
        {
            // src.IsContinuous = true;
            // src.IsSubMatrix = false

            var mtype = src.Type();
            var psize = (mtype.Depth + 1) * mtype.Channels;

            return new SpanBitmap(src.Data, src.Width, src.Height, psize, (int)src.Step());
        }

        public static unsafe SpanBitmap<TPixel> AsSpanBitmap<TPixel>(this OpenCvSharp.Mat<TPixel> src)
            where TPixel : unmanaged
        {
            return new SpanBitmap<TPixel>(src.Data, src.Width, src.Height, (int)src.Step());
        }



        public static unsafe OpenCvSharp.Mat<TPixel> CopyToOpenCvSharp<TPixel>(this SpanBitmap<TPixel> srcSpan)
            where TPixel:unmanaged
        {
            var dst = new OpenCvSharp.Mat<TPixel>(srcSpan.Height, srcSpan.Width);
            var dstSpan = dst.AsSpanBitmap<TPixel>();
            dstSpan.SetPixels(0, 0, srcSpan);
            return dst;            
        }

        
    }
}
