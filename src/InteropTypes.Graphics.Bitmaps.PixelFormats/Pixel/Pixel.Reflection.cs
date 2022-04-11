using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        public static bool IsFloating<TPixel>()
            where TPixel : unmanaged
        {            
            if (typeof(TPixel) == typeof(Luminance32F)) return true;
            if (typeof(TPixel) == typeof(RGB96F)) return true;
            if (typeof(TPixel) == typeof(BGR96F)) return true;

            if (typeof(TPixel) == typeof(BGRA128F)) return true;
            if (typeof(TPixel) == typeof(RGBA128F)) return true;

            if (typeof(TPixel) == typeof(BGRP128F)) return true;

            if (typeof(TPixel) == typeof(float)) return true;
            if (typeof(TPixel) == typeof(System.Numerics.Vector2)) return true;
            if (typeof(TPixel) == typeof(System.Numerics.Vector3)) return true;
            if (typeof(TPixel) == typeof(System.Numerics.Vector4)) return true;
            if (typeof(TPixel) == typeof(System.Numerics.Quaternion)) return true;

            return false;
        }

        public static bool IsPremultiplied<TPixel>()
             where TPixel : unmanaged
        {
            if (typeof(TPixel) == typeof(BGRP32)) return true;
            if (typeof(TPixel) == typeof(RGBP32)) return true;
            if (typeof(TPixel) == typeof(BGRP128F)) return true;

            return false;
        }

        public static unsafe bool IsOpaque<TPixel>()
             where TPixel : unmanaged
        {
            if (typeof(TPixel) == typeof(Luminance8)) return true;
            if (typeof(TPixel) == typeof(Luminance16)) return true;
            if (typeof(TPixel) == typeof(Luminance32F)) return true;

            if (typeof(TPixel) == typeof(BGR565)) return true;
            if (typeof(TPixel) == typeof(BGR24)) return true;
            if (typeof(TPixel) == typeof(RGB24)) return true;

            if (typeof(TPixel) == typeof(BGR96F)) return true;
            if (typeof(TPixel) == typeof(RGB96F)) return true;

            if (typeof(TPixel) == typeof(System.Numerics.Vector3)) return true;

            if (sizeof(TPixel) == 3) return true;

            return false;
        }

        partial struct Alpha8 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public bool IsQuantized => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct Luminance8 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public bool IsQuantized => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct Luminance16 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public bool IsQuantized => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct Luminance32F : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public bool IsQuantized => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGR565 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public bool IsQuantized => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGRA5551 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public bool IsQuantized => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGRA4444 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public bool IsQuantized => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGR24 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public bool IsQuantized => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct RGB24 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public bool IsQuantized => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct RGBA32 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public bool IsQuantized => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGRA32 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public bool IsQuantized => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct ARGB32 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public bool IsQuantized => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct RGBP32 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => true;
            public bool IsQuantized => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGRP32 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => true;
            public bool IsQuantized => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct RGB96F : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public bool IsQuantized => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGR96F : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public bool IsQuantized => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct RGBA128F : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public bool IsQuantized => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGRA128F : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public bool IsQuantized => false;
            public PixelFormat GetPixelFormat() => Format;
        }        

        partial struct BGRP128F : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => true;
            public bool IsQuantized => false;
            public PixelFormat GetPixelFormat() => Format;
        }
    }
}
