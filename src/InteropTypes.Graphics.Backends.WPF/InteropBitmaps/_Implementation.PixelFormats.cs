using System;

// https://github.com/dotnet/wpf
// https://github.com/dotnet/wpf/blob/master/src/Microsoft.DotNet.Wpf/src/PresentationCore/System/Windows/Media/Imaging/WriteableBitmap.cs    

// SharpDX uses WIC directly:
// http://sharpdx.org/wiki/class-library-api/wic/
// https://docs.microsoft.com/es-es/windows/win32/wic/-wic-lh?redirectedfrom=MSDN

using WIC_FORMAT = System.Windows.Media.PixelFormat;
using WIC_FORMATS = System.Windows.Media.PixelFormats;

using INTEROPFMT = InteropTypes.Graphics.PixelFormat;

namespace InteropTypes.Graphics
{
    partial class _Implementation
    {
        #region pixel formats

        public static bool TryGetExactPixelFormat(WIC_FORMAT inFmt, out INTEROPFMT outFmt)
        {
            if (inFmt == WIC_FORMATS.Gray8) { outFmt = Pixel.Luminance8.Format; return true; }
            if (inFmt == WIC_FORMATS.Gray16) { outFmt = Pixel.Luminance16.Format; return true; }
            if (inFmt == WIC_FORMATS.Gray32Float) { outFmt = Pixel.Luminance32F.Format; return true; }

            if (inFmt == WIC_FORMATS.Bgr555) { outFmt = Pixel.BGRA5551.Format; return true; }
            if (inFmt == WIC_FORMATS.Bgr565) { outFmt = Pixel.BGR565.Format; return true; }

            if (inFmt == WIC_FORMATS.Bgr24) { outFmt = Pixel.BGR24.Format; return true; }
            if (inFmt == WIC_FORMATS.Rgb24) { outFmt = Pixel.RGB24.Format; return true; }

            if (inFmt == WIC_FORMATS.Bgr32) { outFmt = Pixel.BGRA32.Format; return true; }
            if (inFmt == WIC_FORMATS.Bgra32) { outFmt = Pixel.BGRA32.Format; return true; }

            if (inFmt == WIC_FORMATS.Pbgra32) { outFmt = Pixel.BGRA32.Format; return true; } // not right

            if (inFmt == WIC_FORMATS.Rgba128Float) { outFmt = Pixel.RGBA128F.Format; return true; }

            outFmt = default;
            return false;
        }
        
        public static bool TryGetExactPixelFormat(INTEROPFMT inFmt, out WIC_FORMAT outFmt)
        {
            switch (inFmt.Code)
            {
                case Pixel.Luminance8.Code: outFmt= WIC_FORMATS.Gray8; return true;
                case Pixel.Luminance16.Code: outFmt = WIC_FORMATS.Gray16; return true;
                case Pixel.Luminance32F.Code: outFmt = WIC_FORMATS.Gray32Float; return true;

                case Pixel.BGR565.Code: outFmt = WIC_FORMATS.Bgr565; return true;

                case Pixel.BGR24.Code: outFmt = WIC_FORMATS.Bgr24; return true;
                case Pixel.RGB24.Code: outFmt = WIC_FORMATS.Rgb24; return true;

                case Pixel.BGRA32.Code: outFmt = WIC_FORMATS.Bgra32; return true;

                case Pixel.RGBA128F.Code: outFmt = WIC_FORMATS.Rgba128Float; return true;                
            }

            outFmt = default;
            return false;
        }        

        public static WIC_FORMAT GetCompatiblePixelFormat(INTEROPFMT fmt)
        {
            if (TryGetExactPixelFormat(fmt, out var exact)) return exact;

            switch (fmt.Code)
            {
                case Pixel.Alpha8.Code:
                case Pixel.BGRA4444.Code:
                case Pixel.BGRA5551.Code:
                case Pixel.RGBA32.Code:
                case Pixel.ARGB32.Code:
                    return WIC_FORMATS.Bgra32;

                // case Pixel.ARGB32P.Code:
                case Pixel.RGBP32.Code:
                case Pixel.BGRP32.Code:
                    return WIC_FORMATS.Pbgra32;

                case Pixel.BGR96F.Code:
                case Pixel.BGRA128F.Code:
                    return WIC_FORMATS.Rgba128Float;                

                default: throw new NotImplementedException();
            }
        }

        #endregion
    }
}