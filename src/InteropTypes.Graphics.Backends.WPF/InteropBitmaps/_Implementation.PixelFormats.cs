using System;

// https://github.com/dotnet/wpf
// https://github.com/dotnet/wpf/blob/master/src/Microsoft.DotNet.Wpf/src/PresentationCore/System/Windows/Media/Imaging/WriteableBitmap.cs    

// SharpDX uses WIC directly:
// http://sharpdx.org/wiki/class-library-api/wic/
// https://docs.microsoft.com/es-es/windows/win32/wic/-wic-lh?redirectedfrom=MSDN

using WIC_FORMAT = System.Windows.Media.PixelFormat;
using WIC_FORMATS = System.Windows.Media.PixelFormats;

using INTEROPFMT = InteropTypes.Graphics.Bitmaps.PixelFormat;
using INTEROPPIX = InteropTypes.Graphics.Bitmaps.Pixel;

namespace InteropTypes.Graphics
{
    partial class _Implementation
    {
        #region pixel formats

        public static bool TryGetExactPixelFormat(WIC_FORMAT inFmt, out INTEROPFMT outFmt)
        {
            if (inFmt == WIC_FORMATS.Gray8) { outFmt = INTEROPPIX.Luminance8.Format; return true; }
            if (inFmt == WIC_FORMATS.Gray16) { outFmt = INTEROPPIX.Luminance16.Format; return true; }
            if (inFmt == WIC_FORMATS.Gray32Float) { outFmt = INTEROPPIX.Luminance32F.Format; return true; }

            if (inFmt == WIC_FORMATS.Bgr555) { outFmt = INTEROPPIX.BGRA5551.Format; return true; }
            if (inFmt == WIC_FORMATS.Bgr565) { outFmt = INTEROPPIX.BGR565.Format; return true; }

            if (inFmt == WIC_FORMATS.Bgr24) { outFmt = INTEROPPIX.BGR24.Format; return true; }
            if (inFmt == WIC_FORMATS.Rgb24) { outFmt = INTEROPPIX.RGB24.Format; return true; }

            if (inFmt == WIC_FORMATS.Bgr32) { outFmt = INTEROPPIX.BGRA32.Format; return true; }
            if (inFmt == WIC_FORMATS.Bgra32) { outFmt = INTEROPPIX.BGRA32.Format; return true; }

            if (inFmt == WIC_FORMATS.Pbgra32) { outFmt = INTEROPPIX.BGRA32.Format; return true; } // not right

            if (inFmt == WIC_FORMATS.Rgba128Float) { outFmt = INTEROPPIX.RGBA128F.Format; return true; }

            outFmt = default;
            return false;
        }
        
        public static bool TryGetExactPixelFormat(INTEROPFMT inFmt, out WIC_FORMAT outFmt)
        {
            switch (inFmt.Code)
            {
                case INTEROPPIX.Luminance8.Code: outFmt= WIC_FORMATS.Gray8; return true;
                case INTEROPPIX.Luminance16.Code: outFmt = WIC_FORMATS.Gray16; return true;
                case INTEROPPIX.Luminance32F.Code: outFmt = WIC_FORMATS.Gray32Float; return true;

                case INTEROPPIX.BGR565.Code: outFmt = WIC_FORMATS.Bgr565; return true;

                case INTEROPPIX.BGR24.Code: outFmt = WIC_FORMATS.Bgr24; return true;
                case INTEROPPIX.RGB24.Code: outFmt = WIC_FORMATS.Rgb24; return true;

                case INTEROPPIX.BGRA32.Code: outFmt = WIC_FORMATS.Bgra32; return true;

                case INTEROPPIX.RGBA128F.Code: outFmt = WIC_FORMATS.Rgba128Float; return true;                
            }

            outFmt = default;
            return false;
        }        

        public static WIC_FORMAT GetCompatiblePixelFormat(INTEROPFMT fmt)
        {
            if (TryGetExactPixelFormat(fmt, out var exact)) return exact;

            switch (fmt.Code)
            {
                case INTEROPPIX.Alpha8.Code:
                case INTEROPPIX.BGRA4444.Code:
                case INTEROPPIX.BGRA5551.Code:
                case INTEROPPIX.RGBA32.Code:
                case INTEROPPIX.ARGB32.Code:
                    return WIC_FORMATS.Bgra32;

                // case Pixel.ARGB32P.Code:
                case INTEROPPIX.RGBP32.Code:
                case INTEROPPIX.BGRP32.Code:
                    return WIC_FORMATS.Pbgra32;

                case INTEROPPIX.BGR96F.Code:
                case INTEROPPIX.BGRA128F.Code:
                    return WIC_FORMATS.Rgba128Float;                

                default: throw new NotImplementedException();
            }
        }

        #endregion
    }
}