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
        };
    }
}
