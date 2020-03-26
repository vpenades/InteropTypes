using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    static class _Implementation
    {
        // https://github.com/shimat/opencvsharp/wiki/Accessing-Pixel

        public static PixelFormat ToPixelFormat(OpenCvSharp.MatType fmt)
        {
            if (fmt.IsInteger)
            {
                if (fmt == OpenCvSharp.MatType.CV_8UC1) return PixelFormat.Standard.Gray8;
                if (fmt == OpenCvSharp.MatType.CV_16UC1) return PixelFormat.Standard.Gray16;

                if (fmt == OpenCvSharp.MatType.CV_8UC3) return PixelFormat.Standard.BGR24;
                if (fmt == OpenCvSharp.MatType.CV_8UC4) return PixelFormat.Standard.BGRA32;
            }

            throw new NotImplementedException();
        }

        public static unsafe SpanBitmap WrapAsSpanBitmap(OpenCvSharp.Mat src)
        {
            // src.IsContinuous = true; ??
            // src.IsSubMatrix = false ??

            var fmt = ToPixelFormat(src.Type());

            var info = new BitmapInfo(src.Width, src.Height, fmt, (int)src.Step());

            return new SpanBitmap(src.Data, info);
        }

        public static unsafe SpanBitmap<TPixel> WrapAsSpanBitmap<TPixel>(OpenCvSharp.Mat<TPixel> src)
            where TPixel : unmanaged
        {
            var fmt = ToPixelFormat(src.Type());

            var info = new BitmapInfo(src.Width, src.Height, fmt, (int)src.Step());

            return new SpanBitmap<TPixel>(src.Data, info);
        }

        public static OpenCvSharp.Mat CloneToMat(SpanBitmap src)
        {
            var mtype = OpenCvSharp.MatType.CV_8UC(src.PixelSize);

            var dst = new OpenCvSharp.Mat(src.Height, src.Width, mtype);
            dst.AsSpanBitmap().SetPixels(0, 0, src);

            return dst;
        }

        public static unsafe OpenCvSharp.Mat<TPixel> CloneToMat<TPixel>(this SpanBitmap<TPixel> srcSpan)
            where TPixel : unmanaged
        {
            var dst = new OpenCvSharp.Mat<TPixel>(srcSpan.Height, srcSpan.Width);
            var dstSpan = dst.AsSpanBitmap<TPixel>();
            dstSpan.SetPixels(0, 0, srcSpan);
            return dst;
        }

        public static OpenCvSharp.Mat WrapAsMat(PointerBitmap src)
        {
            var mtype = OpenCvSharp.MatType.CV_8UC(src.Info.PixelByteSize);

            return new OpenCvSharp.Mat(src.Info.Height, src.Info.Width, mtype, src.Pointer, src.Info.ScanlineByteSize);
        }


    }
}
