using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    static class _Implementation
    {
        public static PixelFormat ToInterop(OpenCvSharp.MatType fmt)
        {
            if (fmt.IsInteger)
            {
                if (fmt == OpenCvSharp.MatType.CV_8UC1) return PixelFormat.Standard.GRAY8;
                if (fmt == OpenCvSharp.MatType.CV_16UC1) return PixelFormat.Standard.GRAY16;

                if (fmt == OpenCvSharp.MatType.CV_8UC3) return PixelFormat.Standard.BGR24;
                if (fmt == OpenCvSharp.MatType.CV_8UC4) return PixelFormat.Standard.BGRA32;
            }

            throw new NotImplementedException();
        }

        public static unsafe SpanBitmap AsSpanBitmap(OpenCvSharp.Mat src)
        {
            // src.IsContinuous = true; ??
            // src.IsSubMatrix = false ??

            var fmt = ToInterop(src.Type());

            var info = new BitmapInfo(src.Width, src.Height, fmt, (int)src.Step());

            return new SpanBitmap(src.Data, info);
        }

        public static unsafe SpanBitmap<TPixel> AsSpanBitmap<TPixel>(OpenCvSharp.Mat<TPixel> src)
            where TPixel : unmanaged
        {
            var fmt = ToInterop(src.Type());

            var info = new BitmapInfo(src.Width, src.Height, fmt, (int)src.Step());

            return new SpanBitmap<TPixel>(src.Data, info);
        }


    }
}
