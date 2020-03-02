using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    static class _Implementation
    {
        public static BitmapInfo ToBitmapInfo(this SkiaSharp.SKImageInfo info)
        {
            return new BitmapInfo(info.Width, info.Height, PixelFormat.Standard.RGB24, info.RowBytes);
        }

        public static PointerBitmap AsPointerBitmap(this SkiaSharp.SKPixmap map)
        {
            var ptr = map.GetPixels();
            if (ptr == IntPtr.Zero) throw new ArgumentNullException();

            var binfo = map.Info.ToBitmapInfo();

            return new PointerBitmap(ptr, binfo);
        }

        public static SpanBitmap AsSpanBitmap(this SkiaSharp.SKPixmap map)
        {
            var ptr = map.GetPixels();
            if (ptr == IntPtr.Zero) throw new ArgumentNullException();

            var binfo = map.Info.ToBitmapInfo();

            return new SpanBitmap(ptr, binfo);
        }

        public static SpanBitmap AsSpanBitmap(this SkiaSharp.SKBitmap bmp)
        {
            var ptr = bmp.GetPixels();
            if (ptr == IntPtr.Zero) throw new ArgumentNullException();

            var binfo = bmp.Info.ToBitmapInfo();

            // bmp.NotifyPixelsChanged();

            return new SpanBitmap(ptr, binfo);
        }

        public static SkiaSharp.SKBitmap Read(string filePath)
        {
            return SkiaSharp.SKBitmap.Decode(filePath);
        }

        public static SkiaSharp.SKBitmap Read(System.IO.Stream stream)
        {
            return SkiaSharp.SKBitmap.Decode(stream);
        }

    }
}
