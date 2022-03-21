using System;
using System.Collections.Generic;
using System.Text;

using INTEROPFMT = InteropTypes.Graphics.Bitmaps.PixelFormat;
using INTEROPPIX = InteropTypes.Graphics.Bitmaps.Pixel;

using SKIACOLOR = SkiaSharp.SKColorType;
using SKIAALPHA = SkiaSharp.SKAlphaType;

namespace InteropTypes
{
    partial class _Implementation
    {
        public static bool TryGetPixelFormat(INTEROPFMT src, out SKIACOLOR dstColor, out SKIAALPHA dstAlpha, bool allowCompatibleFormats = false)
        {
            switch (src)
            {
                case INTEROPPIX.Alpha8.Code: dstColor = SKIACOLOR.Alpha8; dstAlpha = SKIAALPHA.Opaque; return true;
                case INTEROPPIX.Luminance8.Code: dstColor = SKIACOLOR.Gray8; dstAlpha = SKIAALPHA.Opaque; return true;

                case INTEROPPIX.BGR565.Code: dstColor = SKIACOLOR.Rgb565; dstAlpha = SKIAALPHA.Opaque; return true;

                case INTEROPPIX.RGBA32.Code: dstColor = SKIACOLOR.Rgba8888; dstAlpha = SKIAALPHA.Unpremul; return true;
                case INTEROPPIX.BGRA32.Code: dstColor = SKIACOLOR.Bgra8888; dstAlpha = SKIAALPHA.Unpremul; return true;

                case INTEROPPIX.RGBA128F.Code: dstColor = SKIACOLOR.RgbaF32; dstAlpha = SKIAALPHA.Unpremul; return true;
            }

            if (allowCompatibleFormats)
            {
                switch (src)
                {
                    case INTEROPPIX.BGR24.Code: dstColor = SKIACOLOR.Rgb888x; dstAlpha = SKIAALPHA.Opaque; return true;
                    case INTEROPPIX.RGB24.Code: dstColor = SKIACOLOR.Rgb888x; dstAlpha = SKIAALPHA.Opaque; return true;
                    case INTEROPPIX.Luminance16.Code: dstColor = SKIACOLOR.Gray8; dstAlpha = SKIAALPHA.Opaque; return true;
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

        

        public static INTEROPFMT ToPixelFormat((SKIACOLOR Color, SKIAALPHA Alpha) fmt, bool allowCompatibleFormats = false)
        {
            switch (fmt.Alpha)
            {
                case SKIAALPHA.Opaque: return ToPixelFormatAlpha(fmt.Color, allowCompatibleFormats);
                case SKIAALPHA.Premul: return ToPixelFormatPremul(fmt.Color, allowCompatibleFormats);
                case SKIAALPHA.Unpremul: return ToPixelFormatAlpha(fmt.Color, allowCompatibleFormats);
            }
            throw new NotImplementedException();
        }

        private static INTEROPFMT ToPixelFormatAlpha(SKIACOLOR color, bool allowCompatibleFormats = false)
        {
            switch (color)
            {
                case SKIACOLOR.Alpha8: return INTEROPPIX.Alpha8.Format;
                case SKIACOLOR.Gray8: return INTEROPPIX.Luminance8.Format;
                case SKIACOLOR.Rgba8888: return INTEROPPIX.RGBA32.Format;
                case SKIACOLOR.Rgb888x: return INTEROPPIX.RGBA32.Format;
                case SKIACOLOR.Bgra8888: return INTEROPPIX.BGRA32.Format;
            }

            if (allowCompatibleFormats)
            {
                switch (color)
                {
                    case SKIACOLOR.Argb4444: return INTEROPPIX.BGRA4444.Format;
                    case SKIACOLOR.Rgb565: return INTEROPPIX.BGR565.Format;
                    case SKIACOLOR.Rgb888x: return INTEROPPIX.RGB24.Format;
                }
            }

            throw new NotSupportedException();
        }

        private static INTEROPFMT ToPixelFormatPremul(SKIACOLOR color, bool allowCompatibleFormats = false)
        {
            switch (color)
            {
                case SKIACOLOR.Alpha8: return INTEROPPIX.Alpha8.Format;
                case SKIACOLOR.Gray8: return INTEROPPIX.Luminance8.Format;
                case SKIACOLOR.Rgba8888: return INTEROPPIX.RGBP32.Format;
                case SKIACOLOR.Rgb888x: return INTEROPPIX.RGBP32.Format;
                case SKIACOLOR.Bgra8888: return INTEROPPIX.BGRP32.Format;
            }

            if (allowCompatibleFormats)
            {
                switch (color)
                {                    
                    case SKIACOLOR.Argb4444: return INTEROPPIX.RGBP32.Format;
                    case SKIACOLOR.Rgb565: return INTEROPPIX.RGBP32.Format;
                    case SKIACOLOR.Rgb888x: return INTEROPPIX.RGBP32.Format;
                }
            }

            throw new NotSupportedException();
        }
    }
}