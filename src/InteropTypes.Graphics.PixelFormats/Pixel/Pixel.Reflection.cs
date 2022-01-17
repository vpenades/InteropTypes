using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial class Pixel
    {
        partial struct Alpha8 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct Luminance8 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct Luminance16 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct Luminance32F : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGR565 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGRA5551 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGRA4444 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGR24 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct RGB24 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct RGBA32 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGRA32 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct ARGB32 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct RGBP32 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGRP32 : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => true;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct RGB96F : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGR96F : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => true;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct RGBA128F : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }

        partial struct BGRA128F : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => false;
            public PixelFormat GetPixelFormat() => Format;
        }        

        partial struct BGRP128F : IReflection
        {
            public uint GetCode() => Code;
            public bool IsOpaque => false;
            public bool IsPremultiplied => true;
            public PixelFormat GetPixelFormat() => Format;
        }
    }
}
