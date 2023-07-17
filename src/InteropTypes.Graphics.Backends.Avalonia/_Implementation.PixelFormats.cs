using System;
using System.Collections.Generic;
using System.Text;

using INTEROPFMT = InteropTypes.Graphics.Bitmaps.PixelFormat;
using INTEROPPIX = InteropTypes.Graphics.Bitmaps.Pixel;



using AVALONIAFORMAT = Avalonia.Platform.PixelFormat;
using AVALONIAALPHA = Avalonia.Platform.AlphaFormat;

namespace InteropTypes.Graphics.Backends
{
    partial class _Implementation
    {
        public static (AVALONIAFORMAT Color, AVALONIAALPHA Alpha) ToPixelFormat(INTEROPFMT fmt, bool allowCompatibleFormats = false)
        {
            TryGetPixelFormat(fmt, out var sc, out var sa, allowCompatibleFormats);
            return (sc, sa);
        }

        public static bool TryGetPixelFormat(INTEROPFMT src, out AVALONIAFORMAT dstColor, out AVALONIAALPHA dstAlpha, bool allowCompatibleFormats = false)
        {
            switch (src)
            {
                case INTEROPPIX.Luminance8.Code: dstColor = Avalonia.Platform.PixelFormats.Gray8; dstAlpha = AVALONIAALPHA.Opaque; return true;
                case INTEROPPIX.Luminance16.Code: dstColor = Avalonia.Platform.PixelFormats.Gray16; dstAlpha = AVALONIAALPHA.Opaque; return true;
                case INTEROPPIX.Luminance32F.Code: dstColor = Avalonia.Platform.PixelFormats.Gray32Float; dstAlpha = AVALONIAALPHA.Opaque; return true;

                case INTEROPPIX.BGR565.Code: dstColor = Avalonia.Platform.PixelFormats.Rgb565; dstAlpha = AVALONIAALPHA.Opaque; return true;
                case INTEROPPIX.RGB24.Code: dstColor = Avalonia.Platform.PixelFormats.Rgb24; dstAlpha = AVALONIAALPHA.Opaque; return true;
                case INTEROPPIX.BGR24.Code: dstColor = Avalonia.Platform.PixelFormats.Bgr24; dstAlpha = AVALONIAALPHA.Opaque; return true;

                case INTEROPPIX.RGBA32.Code: dstColor = Avalonia.Platform.PixelFormats.Rgba8888; dstAlpha = AVALONIAALPHA.Unpremul; return true;
                case INTEROPPIX.BGRA32.Code: dstColor = Avalonia.Platform.PixelFormats.Bgra8888; dstAlpha = AVALONIAALPHA.Unpremul; return true;

                case INTEROPPIX.RGBP32.Code: dstColor = Avalonia.Platform.PixelFormats.Rgba8888; dstAlpha = AVALONIAALPHA.Premul; return true;
                case INTEROPPIX.BGRP32.Code: dstColor = Avalonia.Platform.PixelFormats.Bgra8888; dstAlpha = AVALONIAALPHA.Premul; return true;
            }

            if (allowCompatibleFormats)
            {
                
            }

            dstColor = default;
            dstAlpha = default;
            return false;
        }        

        public static INTEROPFMT ToPixelFormat((AVALONIAFORMAT Color, AVALONIAALPHA Alpha) fmt, bool allowCompatibleFormats = false)
        {
            switch (fmt.Alpha)
            {
                case AVALONIAALPHA.Opaque: return ToPixelFormatAlpha(fmt.Color, allowCompatibleFormats);
                case AVALONIAALPHA.Premul: return ToPixelFormatPremul(fmt.Color, allowCompatibleFormats);
                case AVALONIAALPHA.Unpremul: return ToPixelFormatAlpha(fmt.Color, allowCompatibleFormats);
            }
            throw new NotImplementedException();
        }

        private static INTEROPFMT ToPixelFormatAlpha(AVALONIAFORMAT color, bool allowCompatibleFormats = false)
        {
            if (color == Avalonia.Platform.PixelFormats.Gray8) return INTEROPPIX.Luminance8.Format;
            if (color == Avalonia.Platform.PixelFormats.Gray16) return INTEROPPIX.Luminance16.Format;
            if (color == Avalonia.Platform.PixelFormats.Gray32Float) return INTEROPPIX.Luminance32F.Format;

            if (color == Avalonia.Platform.PixelFormats.Rgb565) return INTEROPPIX.BGR565.Format;
            if (color == Avalonia.Platform.PixelFormats.Rgb24) return INTEROPPIX.RGB24.Format;
            if (color == Avalonia.Platform.PixelFormats.Bgr24) return INTEROPPIX.BGR24.Format;

            if (color == Avalonia.Platform.PixelFormats.Rgba8888) return INTEROPPIX.RGBA32.Format;
            if (color == Avalonia.Platform.PixelFormats.Bgra8888) return INTEROPPIX.BGRA32.Format;

            if (allowCompatibleFormats)
            {
                
            }

            throw new NotSupportedException();
        }

        private static INTEROPFMT ToPixelFormatPremul(AVALONIAFORMAT color, bool allowCompatibleFormats = false)
        {
            if (color == Avalonia.Platform.PixelFormats.Rgba8888) return INTEROPPIX.RGBP32.Format;
            if (color == Avalonia.Platform.PixelFormats.Bgra8888) return INTEROPPIX.BGRP32.Format;

            throw new NotSupportedException();
        }
    }
}