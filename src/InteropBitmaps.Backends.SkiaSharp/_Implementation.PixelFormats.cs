using System;
using System.Collections.Generic;
using System.Text;

using INTEROPFMT = InteropBitmaps.Pixel.Format;
using SKIACOLOR = SkiaSharp.SKColorType;
using SKIAALPHA = SkiaSharp.SKAlphaType;

namespace InteropBitmaps
{
    partial class _Implementation
    {
        public static bool TryGetPixelFormat(INTEROPFMT src, out SKIACOLOR dstColor, out SKIAALPHA dstAlpha, bool allowCompatibleFormats = false)
        {
            switch (src)
            {
                case Pixel.Alpha8.Code: dstColor = SKIACOLOR.Alpha8; dstAlpha = SKIAALPHA.Opaque; return true;
                case Pixel.Luminance8.Code: dstColor = SKIACOLOR.Gray8; dstAlpha = SKIAALPHA.Opaque; return true;

                case Pixel.BGR565.Code: dstColor = SKIACOLOR.Rgb565; dstAlpha = SKIAALPHA.Opaque; return true;

                case Pixel.RGBA32.Code: dstColor = SKIACOLOR.Rgba8888; dstAlpha = SKIAALPHA.Unpremul; return true;
                case Pixel.BGRA32.Code: dstColor = SKIACOLOR.Bgra8888; dstAlpha = SKIAALPHA.Unpremul; return true;

                case Pixel.VectorRGBA.Code: dstColor = SKIACOLOR.RgbaF32; dstAlpha = SKIAALPHA.Unpremul; return true;
            }

            if (allowCompatibleFormats)
            {
                switch (src)
                {
                    case Pixel.BGR24.Code: dstColor = SKIACOLOR.Rgb888x; dstAlpha = SKIAALPHA.Opaque; return true;
                    case Pixel.RGB24.Code: dstColor = SKIACOLOR.Rgb888x; dstAlpha = SKIAALPHA.Opaque; return true;
                    case Pixel.Luminance16.Code: dstColor = SKIACOLOR.Gray8; dstAlpha = SKIAALPHA.Opaque; return true;
                }
            }

            dstColor = SKIACOLOR.Unknown;
            dstAlpha = SKIAALPHA.Unknown;
            return false;
        }

        public static (SKIACOLOR Color, SKIAALPHA Alpha) ToPixelFormat(INTEROPFMT fmt, bool allowCompatibleFormats = false)
        {
            TryGetPixelFormat(fmt, out var sc, out var sa, allowCompatibleFormats);
            return (sc, sa);
        }

        

        public static INTEROPFMT ToPixelFormat(SKIACOLOR color, SKIAALPHA alpha, bool allowCompatibleFormats = false)
        {
            switch (alpha)
            {
                case SKIAALPHA.Opaque: return ToPixelFormatAlpha(color, allowCompatibleFormats);
                case SKIAALPHA.Premul: return ToPixelFormatPremul(color, allowCompatibleFormats);
                case SKIAALPHA.Unpremul: return ToPixelFormatAlpha(color, allowCompatibleFormats);
            }
            throw new NotImplementedException();
        }

        private static INTEROPFMT ToPixelFormatAlpha(SKIACOLOR color, bool allowCompatibleFormats = false)
        {
            switch (color)
            {
                case SKIACOLOR.Alpha8: return Pixel.Alpha8.Format;
                case SKIACOLOR.Gray8: return Pixel.Luminance8.Format;
                case SKIACOLOR.Rgba8888: return Pixel.RGBA32.Format;
                case SKIACOLOR.Rgb888x: return Pixel.RGBA32.Format;
                case SKIACOLOR.Bgra8888: return Pixel.BGRA32.Format;
            }

            if (allowCompatibleFormats)
            {
                switch (color)
                {
                    case SKIACOLOR.Argb4444: return Pixel.BGRA4444.Format;
                    case SKIACOLOR.Rgb565: return Pixel.BGR565.Format;
                    case SKIACOLOR.Rgb888x: return Pixel.RGB24.Format;
                }
            }

            throw new NotSupportedException();
        }

        private static INTEROPFMT ToPixelFormatPremul(SKIACOLOR color, bool allowCompatibleFormats = false)
        {
            switch (color)
            {
                case SKIACOLOR.Alpha8: return Pixel.Alpha8.Format;
                case SKIACOLOR.Gray8: return Pixel.Luminance8.Format;
                case SKIACOLOR.Rgba8888: return Pixel.RGBA32P.Format;
                case SKIACOLOR.Rgb888x: return Pixel.RGBA32P.Format;
                case SKIACOLOR.Bgra8888: return Pixel.BGRA32P.Format;
            }

            if (allowCompatibleFormats)
            {
                switch (color)
                {                    
                    case SKIACOLOR.Argb4444: return Pixel.RGBA32P.Format;
                    case SKIACOLOR.Rgb565: return Pixel.RGBA32P.Format;
                    case SKIACOLOR.Rgb888x: return Pixel.RGBA32P.Format;
                }
            }

            throw new NotSupportedException();
        }
    }
}