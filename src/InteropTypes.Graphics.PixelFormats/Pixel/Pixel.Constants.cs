using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteropTypes.Graphics
{
    public static partial class Pixel
    {
        partial struct Millimeter16
        {
            public const uint Code = _PackedPixelCodes.Millimeter16;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }
        
        partial struct Alpha8
        {
            public const uint Code = _PackedPixelCodes.Alpha8;
            public static readonly PixelFormat Format = new PixelFormat(Code);            
        }
        
        partial struct Luminance8
        {
            public const uint Code = _PackedPixelCodes.Luminance8;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }
        
        partial struct Luminance16
        {
            public const uint Code = _PackedPixelCodes.Luminance16;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct Luminance32F
        {
            public const uint Code = _PackedPixelCodes.Luminance32F;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct BGR565
        {
            public const uint Code = _PackedPixelCodes.BGR565;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct BGRA5551
        {
            public const uint Code = _PackedPixelCodes.BGRA5551;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct BGRA4444
        {
            public const uint Code = _PackedPixelCodes.BGRA4444;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct BGRA32
        {        
            public const uint Code = _PackedPixelCodes.BGRA32;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct BGRP32
        {
            public const uint Code = _PackedPixelCodes.BGRP32;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct RGBA32
        {
            public const uint Code = _PackedPixelCodes.RGBA32;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct RGBP32
        {
            public const uint Code = _PackedPixelCodes.RGBP32;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct ARGB32
        {
            public const uint Code = _PackedPixelCodes.ARGB32;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        public partial struct PRGB32
        {
            public const uint Code = _PackedPixelCodes.PRGB32;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct BGR24
        {
            public const uint Code = _PackedPixelCodes.BGR24;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct RGB24
        {            
            public const uint Code = _PackedPixelCodes.RGB24;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct RGB96F
        {
            public const uint Code = _PackedPixelCodes.RGB96F;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct BGR96F
        {
            public const uint Code = _PackedPixelCodes.BGR96F;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct BGRA128F
        {            
            public const uint Code = _PackedPixelCodes.BGRA128F;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        public partial struct BGRP128F
        {
            public const uint Code = _PackedPixelCodes.BGRP128F;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        partial struct RGBA128F
        {
            public const uint Code = _PackedPixelCodes.RGBA128F;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }

        public partial struct RGBP128F
        {
            public const uint Code = _PackedPixelCodes.RGBP128F;
            public static readonly PixelFormat Format = new PixelFormat(Code);
        }        
    }
}
