using System;
using System.Collections.Generic;
using System.Text;

using InteropBitmaps;

namespace InteropVision.With
{
    /// <summary>
    /// 
    /// </summary>
    /// <see href="https://github.com/micjahn/ZXing.Net"/>
    static class _Implementation
    {
        public static ZXing.RGBLuminanceSource.BitmapFormat ToZXing(this PixelFormat enc)
        {
            switch (enc.Code)
            {
                case Pixel.Luminance8.Code: return ZXing.RGBLuminanceSource.BitmapFormat.Gray8;
                case Pixel.Luminance16.Code: return ZXing.RGBLuminanceSource.BitmapFormat.Gray16;

                case Pixel.BGR565.Code: return ZXing.RGBLuminanceSource.BitmapFormat.RGB565; // notice that colors here are inverted                

                case Pixel.RGB24.Code: return ZXing.RGBLuminanceSource.BitmapFormat.RGB24;
                case Pixel.BGR24.Code: return ZXing.RGBLuminanceSource.BitmapFormat.BGR24;

                case Pixel.RGBA32.Code: return ZXing.RGBLuminanceSource.BitmapFormat.RGBA32;
                case Pixel.BGRA32.Code: return ZXing.RGBLuminanceSource.BitmapFormat.BGRA32;
                case Pixel.ARGB32.Code: return ZXing.RGBLuminanceSource.BitmapFormat.ARGB32;

                default: return ZXing.RGBLuminanceSource.BitmapFormat.Unknown;
            }
        }

        public static ZXing.LuminanceSource CreateLuminanceSource(SpanBitmap src, ref Byte[] persistentBuffer)
        {
            var fmt = src.PixelFormat.ToZXing();
            if (fmt == ZXing.RGBLuminanceSource.BitmapFormat.Unknown) return null;

            var tmpInfo = new BitmapInfo(src.Size, src.PixelFormat);
            Array.Resize(ref persistentBuffer, tmpInfo.BitmapByteSize);

            new SpanBitmap(persistentBuffer, tmpInfo).SetPixels(0, 0, src);

            // ZXing.LuminanceSource
            // ZXing.PlanarYUVLuminanceSource            

            return new ZXing.RGBLuminanceSource(persistentBuffer, tmpInfo.Width, tmpInfo.Height, fmt);
        }
    }
}
