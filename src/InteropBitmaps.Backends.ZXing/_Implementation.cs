using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <summary>
    /// 
    /// </summary>
    /// <see href="https://github.com/micjahn/ZXing.Net"/>
    class _Implementation
    {
        public static ZXing.RGBLuminanceSource.BitmapFormat ToZXing(PixelFormat enc)
        {
            switch (enc.PackedFormat)
            {
                case PixelFormat.Packed.Gray8: return ZXing.RGBLuminanceSource.BitmapFormat.Gray8;
                case PixelFormat.Packed.Gray16: return ZXing.RGBLuminanceSource.BitmapFormat.Gray16;

                case PixelFormat.Packed.BGR565: return ZXing.RGBLuminanceSource.BitmapFormat.RGB565; // notice that colors here are inverted                

                case PixelFormat.Packed.RGB24: return ZXing.RGBLuminanceSource.BitmapFormat.RGB24;
                case PixelFormat.Packed.BGR24: return ZXing.RGBLuminanceSource.BitmapFormat.BGR24;

                case PixelFormat.Packed.RGBA32: return ZXing.RGBLuminanceSource.BitmapFormat.RGBA32;
                case PixelFormat.Packed.BGRA32: return ZXing.RGBLuminanceSource.BitmapFormat.BGRA32;
                case PixelFormat.Packed.ARGB32: return ZXing.RGBLuminanceSource.BitmapFormat.ARGB32;

                default: return ZXing.RGBLuminanceSource.BitmapFormat.Unknown;
            }
        }

        public static ZXing.LuminanceSource CreateLuminanceSource(SpanBitmap src, ref Byte[] persistentBuffer)
        {
            var fmt = ToZXing(src.PixelFormat);
            if (fmt == ZXing.RGBLuminanceSource.BitmapFormat.Unknown) return null;

            var len = src.Width * src.Height * src.PixelSize;

            if (persistentBuffer == null || persistentBuffer.Length != len) persistentBuffer = new byte[len];

            var tmp = new SpanBitmap(persistentBuffer, src.Width, src.Height, src.PixelFormat);
            tmp.SetPixels(0, 0, src);

            // ZXing.PlanarYUVLuminanceSource            

            return new ZXing.RGBLuminanceSource(persistentBuffer, tmp.Width, tmp.Height, fmt);
        }
    }
}
