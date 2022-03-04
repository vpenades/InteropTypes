using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics;
using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Vision
{
    /// <summary>
    /// 
    /// </summary>
    /// <see href="https://github.com/micjahn/ZXing.Net"/>
    class _Implementation
    {
        public static ZXing.RGBLuminanceSource.BitmapFormat ToZXing(PixelFormat enc)
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
            var fmt = ToZXing(src.PixelFormat);
            if (fmt == ZXing.RGBLuminanceSource.BitmapFormat.Unknown) return null;

            var len = src.Width * src.Height * src.PixelByteSize;

            if (persistentBuffer == null || persistentBuffer.Length != len) persistentBuffer = new byte[len];

            var tmp = new SpanBitmap(persistentBuffer, src.Width, src.Height, src.PixelFormat);
            tmp.SetPixels(0, 0, src);

            // ZXing.PlanarYUVLuminanceSource            

            return new ZXing.RGBLuminanceSource(persistentBuffer, tmp.Width, tmp.Height, fmt);
        }
    }
}
