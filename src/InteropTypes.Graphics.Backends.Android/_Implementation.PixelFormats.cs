using System;
using System.Collections.Generic;
using System.Linq;

using ANDROIDGFX = Android.Graphics;
using ANDROIDBITMAP = Android.Graphics.Bitmap;
using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics
{
    partial class _Implementation
    {
        public static bool TryGetPixelFormat(ANDROIDGFX.Format src, bool srcIsPremultiplied, out PixelFormat dst)
        {
            if (srcIsPremultiplied) throw new Diagnostics.PixelFormatNotSupportedException(src, nameof(src));

            switch (src)
            {
                case ANDROIDGFX.Format.A8: dst = Pixel.Alpha8.Format; return true;
                case ANDROIDGFX.Format.L8: dst = Pixel.Luminance8.Format; return true;
                case ANDROIDGFX.Format.Rgb565: dst = Pixel.BGR565.Format; return true;
                case ANDROIDGFX.Format.Rgba4444: dst = Pixel.BGRA4444.Format; return true;
                case ANDROIDGFX.Format.Rgb888: dst = Pixel.RGB24.Format; return true;
                case ANDROIDGFX.Format.Rgba8888: dst = Pixel.RGBA32.Format; return true;
            }

            dst = default;
            return false;
        }

        public static PixelFormat ToInterop(ANDROIDGFX.Format fmt, PixelFormat? defFmt = null)
        {
            switch (fmt)
            {
                case ANDROIDGFX.Format.A8: return Pixel.Alpha8.Format;
                case ANDROIDGFX.Format.L8: return Pixel.Luminance8.Format;
                case ANDROIDGFX.Format.Rgb565: return Pixel.BGR565.Format;
                case ANDROIDGFX.Format.Rgba4444: return Pixel.BGRA4444.Format;
                case ANDROIDGFX.Format.Rgb888: return Pixel.RGB24.Format;
                case ANDROIDGFX.Format.Rgba8888: return Pixel.RGBA32.Format;
                default: if (defFmt.HasValue) return defFmt.Value; break;
            }

            throw new Diagnostics.PixelFormatNotSupportedException(fmt, nameof(fmt));
        }

        public static PixelFormat ToInterop(ANDROIDBITMAP.Config fmt, bool isPremultiplied, PixelFormat? defFmt = null)
        {
            if (isPremultiplied) throw new Diagnostics.PixelFormatNotSupportedException(fmt, nameof(fmt));

            if (fmt == ANDROIDBITMAP.Config.Alpha8) return Pixel.Alpha8.Format;
            if (fmt == ANDROIDBITMAP.Config.Rgb565) return Pixel.BGR565.Format;
            if (fmt == ANDROIDBITMAP.Config.Argb4444) return Pixel.BGRA4444.Format;
            if (fmt == ANDROIDBITMAP.Config.Argb8888) return Pixel.RGBA32.Format;
            if (defFmt.HasValue) return defFmt.Value;

            throw new Diagnostics.PixelFormatNotSupportedException(fmt, nameof(fmt));
        }

        public static ANDROIDBITMAP.Config ToAndroidBitmapConfig(PixelFormat fmt, ANDROIDBITMAP.Config defval = null)
        {
            switch (fmt.Code)
            {
                case Pixel.Alpha8.Code: return ANDROIDBITMAP.Config.Alpha8;
                case Pixel.BGR565.Code: return ANDROIDBITMAP.Config.Rgb565;
                case Pixel.BGRA4444.Code: return ANDROIDBITMAP.Config.Argb4444;
                case Pixel.RGBA32.Code: return ANDROIDBITMAP.Config.Argb8888;
                default: return defval;
            }
        }

        public static bool TryGetBitmapInfo(ANDROIDGFX.AndroidBitmapInfo src, bool srcIsPremultiplied, out BitmapInfo dst)
        {
            if (TryGetPixelFormat(src.Format, srcIsPremultiplied, out var dstFmt))
            {
                dst = new BitmapInfo((int)src.Width, (int)src.Height, dstFmt, (int)src.Stride);
                return true;
            }

            dst = default;
            return false;
        }
    }
}