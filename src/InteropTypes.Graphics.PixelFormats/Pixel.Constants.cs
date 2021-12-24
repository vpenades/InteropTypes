using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace InteropBitmaps
{
    using PEF = Pixel.Format.ElementID;

    public static partial class Pixel
    {
        /// <summary>
        /// Predefined pixel formats encoded as const UInt32 values,
        /// which makes them suitable for switch blocks.
        /// </summary>
        static class _PackedPixelCodes
        {
            private const uint SHIFT0 = 1;
            private const uint SHIFT1 = 256;
            private const uint SHIFT2 = 256 * 256;
            private const uint SHIFT3 = 256 * 256 * 256;

            public const uint Empty = (uint)PEF.Empty;

            public const uint Luminance8 = SHIFT0 * (uint)PEF.Luminance8;
            public const uint Alpha8 = SHIFT0 * (uint)PEF.Alpha8;

            public const uint Luminance16 = SHIFT0 * (uint)PEF.Luminance16;
            public const uint BGR565 = SHIFT0 * (uint)PEF.Blue5 | SHIFT1 * (uint)PEF.Green6 | SHIFT2 * (uint)PEF.Red5;
            public const uint BGRA4444 = SHIFT0 * (uint)PEF.Blue4 | SHIFT1 * (uint)PEF.Green4 | SHIFT2 * (uint)PEF.Red4 | SHIFT3 * (uint)PEF.Alpha4;
            public const uint BGRA5551 = SHIFT0 * (uint)PEF.Blue5 | SHIFT1 * (uint)PEF.Green5 | SHIFT2 * (uint)PEF.Red5 | SHIFT3 * (uint)PEF.Alpha1;
            public const uint Millimeter16 = SHIFT0 * (uint)PEF.Millimeter16;

            public const uint RGB24 = SHIFT0 * (uint)PEF.Red8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Blue8;
            public const uint BGR24 = SHIFT0 * (uint)PEF.Blue8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Red8;

            public const uint RGBA32 = SHIFT0 * (uint)PEF.Red8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Blue8 | SHIFT3 * (uint)PEF.Alpha8;
            public const uint BGRA32 = SHIFT0 * (uint)PEF.Blue8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Red8 | SHIFT3 * (uint)PEF.Alpha8;
            public const uint ARGB32 = SHIFT0 * (uint)PEF.Alpha8 | SHIFT1 * (uint)PEF.Red8 | SHIFT2 * (uint)PEF.Green8 | SHIFT3 * (uint)PEF.Blue8;

            public const uint RGBP32 = SHIFT0 * (uint)PEF.Red8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Blue8 | SHIFT3 * (uint)PEF.Premul8;
            public const uint BGRP32 = SHIFT0 * (uint)PEF.Blue8 | SHIFT1 * (uint)PEF.Green8 | SHIFT2 * (uint)PEF.Red8 | SHIFT3 * (uint)PEF.Premul8;
            public const uint PRGB32 = SHIFT0 * (uint)PEF.Premul8 | SHIFT1 * (uint)PEF.Red8 | SHIFT2 * (uint)PEF.Green8 | SHIFT3 * (uint)PEF.Blue8;

            public const uint Luminance32F = SHIFT0 * (uint)PEF.Luminance32F;

            public const uint RGB96F = SHIFT0 * (uint)PEF.Red32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Blue32F;
            public const uint BGR96F = SHIFT0 * (uint)PEF.Blue32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Red32F;

            public const uint RGBA128F = SHIFT0 * (uint)PEF.Red32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Blue32F | SHIFT3 * (uint)PEF.Alpha32F;
            public const uint RGBP128F = SHIFT0 * (uint)PEF.Red32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Blue32F | SHIFT3 * (uint)PEF.Premul32F;

            public const uint BGRA128F = SHIFT0 * (uint)PEF.Blue32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Red32F | SHIFT3 * (uint)PEF.Alpha32F;
            public const uint BGRP128F = SHIFT0 * (uint)PEF.Blue32F | SHIFT1 * (uint)PEF.Green32F | SHIFT2 * (uint)PEF.Red32F | SHIFT3 * (uint)PEF.Premul32F;
        }

        
        public static readonly IReadOnlyList<Format> Formats = new Format[]
        {
            // Millimeter16.Code, WIP

            Alpha8.Format,

            Luminance8.Format,
            Luminance16.Format,
            Luminance32F.Format,

            BGR565.Format,
            BGRA5551.Format,
            BGRA4444.Format,

            RGB24.Format,
            BGR24.Format,

            BGRA32.Format,
            BGRP32.Format,

            RGBA32.Format,
            RGBP32.Format,

            ARGB32.Format,
            PRGB32.Format,

            RGB96F.Format,
            BGR96F.Format,

            RGBA128F.Format,
            RGBP128F.Format,

            BGRA128F.Format,
            BGRP128F.Format,
        };
        

        partial struct Millimeter16
        {
            public const uint Code = _PackedPixelCodes.Millimeter16;
            public static readonly Format Format = new Format(Code);
        }
        
        partial struct Alpha8
        {
            public const uint Code = _PackedPixelCodes.Alpha8;
            public static readonly Format Format = new Format(Code);            
        }
        
        partial struct Luminance8
        {
            public const uint Code = _PackedPixelCodes.Luminance8;
            public static readonly Format Format = new Format(Code);
        }
        
        partial struct Luminance16
        {
            public const uint Code = _PackedPixelCodes.Luminance16;
            public static readonly Format Format = new Format(Code);
        }

        partial struct Luminance32F
        {
            public const uint Code = _PackedPixelCodes.Luminance32F;
            public static readonly Format Format = new Format(Code);
        }

        partial struct BGR565
        {
            public const uint Code = _PackedPixelCodes.BGR565;
            public static readonly Format Format = new Format(Code);
        }

        partial struct BGRA5551
        {
            public const uint Code = _PackedPixelCodes.BGRA5551;
            public static readonly Format Format = new Format(Code);
        }

        partial struct BGRA4444
        {
            public const uint Code = _PackedPixelCodes.BGRA4444;
            public static readonly Format Format = new Format(Code);
        }

        partial struct BGRA32
        {        
            public const uint Code = _PackedPixelCodes.BGRA32;
            public static readonly Format Format = new Format(Code);
        }

        partial struct BGRP32
        {
            public const uint Code = _PackedPixelCodes.BGRP32;
            public static readonly Format Format = new Format(Code);
        }

        partial struct RGBA32
        {
            public const uint Code = _PackedPixelCodes.RGBA32;
            public static readonly Format Format = new Format(Code);
        }

        partial struct RGBP32
        {
            public const uint Code = _PackedPixelCodes.RGBP32;
            public static readonly Format Format = new Format(Code);
        }

        partial struct ARGB32
        {
            public const uint Code = _PackedPixelCodes.ARGB32;
            public static readonly Format Format = new Format(Code);
        }

        partial struct PRGB32
        {
            public const uint Code = _PackedPixelCodes.PRGB32;
            public static readonly Format Format = new Format(Code);
        }

        partial struct BGR24
        {
            public const uint Code = _PackedPixelCodes.BGR24;
            public static readonly Format Format = new Format(Code);
        }

        partial struct RGB24
        {            
            public const uint Code = _PackedPixelCodes.RGB24;
            public static readonly Format Format = new Format(Code);
        }

        partial struct RGB96F
        {
            public const uint Code = _PackedPixelCodes.RGB96F;
            public static readonly Format Format = new Format(Code);
        }

        partial struct BGR96F
        {
            public const uint Code = _PackedPixelCodes.BGR96F;
            public static readonly Format Format = new Format(Code);
        }

        partial struct BGRA128F
        {            
            public const uint Code = _PackedPixelCodes.BGRA128F;
            public static readonly Format Format = new Format(Code);
        }

        partial struct BGRP128F
        {
            public const uint Code = _PackedPixelCodes.BGRP128F;
            public static readonly Format Format = new Format(Code);
        }

        partial struct RGBA128F
        {
            public const uint Code = _PackedPixelCodes.RGBA128F;
            public static readonly Format Format = new Format(Code);
        }

        partial struct RGBP128F
        {
            public const uint Code = _PackedPixelCodes.RGBP128F;
            public static readonly Format Format = new Format(Code);
        }        
    }
}
