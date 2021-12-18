using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using GDIFMT = System.Drawing.Imaging.PixelFormat;
using GDIPTR = System.Drawing.Imaging.BitmapData;

using INTEROPFMT = InteropBitmaps.Pixel.Format;

namespace InteropBitmaps
{
    static partial class _Implementation
    {
        public static bool TryGetExactPixelFormat(GDIFMT inFmt, out INTEROPFMT outFmt)
        {
            switch (inFmt)
            {
                case GDIFMT.Format16bppGrayScale: outFmt = Pixel.Luminance16.Format; return true;
                case GDIFMT.Format16bppRgb565: outFmt = Pixel.BGR565.Format; return true;
                case GDIFMT.Format16bppRgb555: outFmt = Pixel.BGRA5551.Format; return true;
                case GDIFMT.Format16bppArgb1555: outFmt = Pixel.BGRA5551.Format; return true;

                case GDIFMT.Format24bppRgb: outFmt = Pixel.BGR24.Format; return true;

                case GDIFMT.Format32bppRgb: outFmt = Pixel.BGRA32.Format; return true;
                case GDIFMT.Format32bppArgb: outFmt = Pixel.BGRA32.Format; return true;
                case GDIFMT.Format32bppPArgb: outFmt = Pixel.BGRP32.Format; return true;
            }

            outFmt = default;
            return false;
        }

        public static bool TryGetExactPixelFormat(INTEROPFMT inFmt, out GDIFMT outFmt)
        {
            switch (inFmt)
            {
                case Pixel.Luminance16.Code: outFmt = GDIFMT.Format16bppGrayScale; return true;

                case Pixel.BGR565.Code: outFmt = GDIFMT.Format16bppRgb565; return true;
                case Pixel.BGRA5551.Code: outFmt = GDIFMT.Format16bppArgb1555; return true;

                case Pixel.BGR24.Code: outFmt = GDIFMT.Format24bppRgb; return true;

                case Pixel.BGRA32.Code: outFmt = GDIFMT.Format32bppArgb; return true;
            }

            outFmt = default;
            return false;
        }

        public static GDIFMT GetCompatiblePixelFormat(INTEROPFMT inFmt)
        {
            if (TryGetExactPixelFormat(inFmt, out var exact)) return exact;

            switch (inFmt)
            {
                case Pixel.Alpha8.Code:
                    return GDIFMT.Format32bppArgb;

                case Pixel.Luminance8.Code:
                case Pixel.Luminance16.Code:
                case Pixel.Luminance32F.Code:
                    //outFmt = GDIFMT.Format16bppGrayScale; return true; // Gray16 is listed, but not supported by GDI.
                    return GDIFMT.Format24bppRgb;

                case Pixel.RGB24.Code:
                case Pixel.BGR24.Code:
                case Pixel.BGR565.Code:
                case Pixel.RGB96F.Code:
                case Pixel.BGR96F.Code:
                    return GDIFMT.Format24bppRgb;

                case Pixel.BGRA32.Code:
                case Pixel.RGBA32.Code:
                case Pixel.ARGB32.Code:
                case Pixel.BGRA4444.Code:
                case Pixel.BGRA128F.Code:
                case Pixel.RGBA128F.Code:
                    return GDIFMT.Format32bppArgb;

                // case Pixel.ARGB32P.Code:
                case Pixel.BGRP32.Code:
                case Pixel.RGBP32.Code:
                    return GDIFMT.Format32bppPArgb;                

                default: throw new Diagnostics.PixelFormatNotSupportedException(inFmt, nameof(inFmt));
            }
        }

        public static bool TryWrap(GDIPTR src, out BitmapInfo dst)
        {
            if (TryGetExactPixelFormat(src.PixelFormat, out var fmt))
            {
                dst = new BitmapInfo(src.Width, src.Height, fmt, src.Stride);
                return true;
            }
            else
            {
                dst = default;
                return false;
            }
        }

        public static BitmapInfo GetBitmapInfo(GDIPTR bits)
        {
            return TryGetExactPixelFormat(bits.PixelFormat, out var fmt)
                ? new BitmapInfo(bits.Width, bits.Height, fmt, bits.Stride)
                : throw new Diagnostics.PixelFormatNotSupportedException(bits.PixelFormat, nameof(bits));
        }        
    }
}