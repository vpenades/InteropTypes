using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial struct PixelFormat
    {
        public static readonly IReadOnlyList<PixelFormat> AllFormats = new PixelFormat[]
        {
            // Millimeter16.Code, WIP

            Pixel.Alpha8.Format,

            Pixel.Luminance8.Format,
            Pixel.Luminance16.Format,
            Pixel.Luminance32F.Format,

            Pixel.BGR565.Format,
            Pixel.BGRA5551.Format,
            Pixel.BGRA4444.Format,

            Pixel.RGB24.Format,
            Pixel.BGR24.Format,

            Pixel.BGRA32.Format,
            Pixel.BGRP32.Format,

            Pixel.RGBA32.Format,
            Pixel.RGBP32.Format,

            Pixel.ARGB32.Format,
            Pixel.PRGB32.Format,

            Pixel.RGB96F.Format,
            Pixel.BGR96F.Format,

            Pixel.RGBA128F.Format,
            Pixel.RGBP128F.Format,

            Pixel.BGRA128F.Format,
            Pixel.BGRP128F.Format,
            
            Pixel.YUV24.Format,
        };

        public static unsafe PixelFormat TryIdentifyFormat<TPixel>() where TPixel : unmanaged
        {
            if (typeof(TPixel) == typeof(Pixel.Alpha8)) return Pixel.Alpha8.Format;

            // Luminance

            if (typeof(TPixel) == typeof(Pixel.Luminance8)) return Pixel.Luminance8.Format;
            if (typeof(TPixel) == typeof(Pixel.Luminance16)) return Pixel.Luminance16.Format;
            if (typeof(TPixel) == typeof(Pixel.Luminance32F)) return Pixel.Luminance32F.Format;

            // RGB

            if (typeof(TPixel) == typeof(Pixel.BGR565)) return Pixel.BGR565.Format;
            if (typeof(TPixel) == typeof(Pixel.BGR24)) return Pixel.BGR24.Format;
            if (typeof(TPixel) == typeof(Pixel.RGB24)) return Pixel.RGB24.Format;

            // RGB + Alpha

            if (typeof(TPixel) == typeof(Pixel.BGRA4444)) return Pixel.BGRA4444.Format;
            if (typeof(TPixel) == typeof(Pixel.BGRA5551)) return Pixel.BGRA5551.Format;
            if (typeof(TPixel) == typeof(Pixel.BGRA32)) return Pixel.BGRA32.Format;
            if (typeof(TPixel) == typeof(Pixel.RGBA32)) return Pixel.RGBA32.Format;
            if (typeof(TPixel) == typeof(Pixel.ARGB32)) return Pixel.ARGB32.Format;                        

            if (typeof(TPixel) == typeof(Pixel.RGB96F)) return Pixel.RGB96F.Format;
            if (typeof(TPixel) == typeof(Pixel.BGR96F)) return Pixel.BGR96F.Format;

            if (typeof(TPixel) == typeof(Pixel.BGRA128F)) return Pixel.BGRA128F.Format;            
            if (typeof(TPixel) == typeof(Pixel.RGBA128F)) return Pixel.RGBA128F.Format;

            // RGB + Premul

            if (typeof(TPixel) == typeof(Pixel.BGRP32)) return Pixel.BGRP32.Format;
            if (typeof(TPixel) == typeof(Pixel.RGBP32)) return Pixel.RGBP32.Format;
            if (typeof(TPixel) == typeof(Pixel.PRGB32)) return Pixel.PRGB32.Format;

            if (typeof(TPixel) == typeof(Pixel.BGRP128F)) return Pixel.BGRP128F.Format;
            if (typeof(TPixel) == typeof(Pixel.RGBP128F)) return Pixel.RGBP128F.Format;

            // YUV
            
            if (typeof(TPixel) == typeof(Pixel.YUV24)) return Pixel.YUV24.Format;

            if (_TryIdentifyThirdPartyPixelFormat<TPixel>(out var fmt)) return fmt;

            return CreateUndefined<TPixel>();
        }

        public Type GetPixelTypeOrNull()
        {
            switch (Code)
            {
                case Pixel.Alpha8.Code: return typeof(Pixel.Alpha8);

                case Pixel.Luminance8.Code: return typeof(Pixel.Luminance8);
                case Pixel.Luminance16.Code: return typeof(Pixel.Luminance16);
                case Pixel.Luminance32F.Code: return typeof(Pixel.Luminance32F);

                case Pixel.BGR565.Code: return typeof(Pixel.BGR565);
                case Pixel.BGRA5551.Code: return typeof(Pixel.BGRA5551);
                case Pixel.BGRA4444.Code: return typeof(Pixel.BGRA4444);

                case Pixel.BGR24.Code: return typeof(Pixel.BGR24);
                case Pixel.RGB24.Code: return typeof(Pixel.RGB24);

                case Pixel.BGRA32.Code: return typeof(Pixel.BGRA32);
                case Pixel.RGBA32.Code: return typeof(Pixel.RGBA32);
                case Pixel.ARGB32.Code: return typeof(Pixel.ARGB32);
                case Pixel.PRGB32.Code: return typeof(Pixel.PRGB32);

                case Pixel.BGRP32.Code: return typeof(Pixel.BGRP32);
                case Pixel.RGBP32.Code: return typeof(Pixel.RGBP32);

                case Pixel.RGB96F.Code: return typeof(Pixel.RGB96F);
                case Pixel.BGR96F.Code: return typeof(Pixel.BGR96F);

                case Pixel.RGBA128F.Code: return typeof(Pixel.RGBA128F);
                case Pixel.BGRA128F.Code: return typeof(Pixel.BGRA128F);

                case Pixel.RGBP128F.Code: return typeof(Pixel.RGBP128F);
                case Pixel.BGRP128F.Code: return typeof(Pixel.BGRP128F);

                case Pixel.YUV24.Code: return typeof(Pixel.YUV24);
            }

            return null;
        }

        public static bool TryGetFormatAsRGBX(PixelFormat fmt, out PixelFormat newFmt)
        {
            newFmt = default;

            switch (fmt.Code)
            {
                case Pixel.RGB24.Code: newFmt = Pixel.RGB24.Format; break;
                case Pixel.BGR24.Code: newFmt = Pixel.RGB24.Format; break;

                case Pixel.RGBA32.Code: newFmt = Pixel.RGBA32.Format; break;
                case Pixel.BGRA32.Code: newFmt = Pixel.RGBA32.Format; break;
                case Pixel.ARGB32.Code: newFmt = Pixel.RGBA32.Format; break;
                case Pixel.RGBP32.Code: newFmt = Pixel.RGBP32.Format; break;
                case Pixel.BGRP32.Code: newFmt = Pixel.RGBP32.Format; break;

                case Pixel.RGB96F.Code: newFmt = Pixel.RGB96F.Format; break;
                case Pixel.BGR96F.Code: newFmt = Pixel.RGB96F.Format; break;

                case Pixel.RGBA128F.Code: newFmt = Pixel.RGBA128F.Format; break;
                case Pixel.BGRA128F.Code: newFmt = Pixel.RGBA128F.Format; break;
                case Pixel.RGBP128F.Code: newFmt = Pixel.RGBP128F.Format; break;
                case Pixel.BGRP128F.Code: newFmt = Pixel.BGRP128F.Format; break;
            }

            return newFmt != default;
        }

        public static bool TryGetFormatAsBGRX(PixelFormat fmt, out PixelFormat newFmt)
        {
            newFmt = default;

            switch (fmt.Code)
            {
                case Pixel.RGB24.Code: newFmt = Pixel.BGR24.Format; break;
                case Pixel.BGR24.Code: newFmt = Pixel.BGR24.Format; break;

                case Pixel.RGBA32.Code: newFmt = Pixel.BGRA32.Format; break;
                case Pixel.BGRA32.Code: newFmt = Pixel.BGRA32.Format; break;
                case Pixel.ARGB32.Code: newFmt = Pixel.BGRA32.Format; break;
                case Pixel.RGBP32.Code: newFmt = Pixel.BGRA32.Format; break;
                case Pixel.BGRP32.Code: newFmt = Pixel.BGRA32.Format; break;

                case Pixel.RGB96F.Code: newFmt = Pixel.BGR96F.Format; break;
                case Pixel.BGR96F.Code: newFmt = Pixel.BGR96F.Format; break;

                case Pixel.RGBA128F.Code: newFmt = Pixel.BGRA128F.Format; break;
                case Pixel.BGRA128F.Code: newFmt = Pixel.BGRA128F.Format; break;
                case Pixel.RGBP128F.Code: newFmt = Pixel.RGBP128F.Format; break;
                case Pixel.BGRP128F.Code: newFmt = Pixel.BGRP128F.Format; break;
            }

            return newFmt != default;
        }
    }
}
