using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    using COLOR = Pixel.Color;

    partial class Pixel
    {
        public readonly struct Color
        {
            

            public Color(int r, int g, int b, int a = 255)
            {
                Red = (Byte)r;
                Green = (Byte)g;
                Blue = (Byte)b;
                Alpha = (Byte)a;
            }

            public readonly Byte Red;
            public readonly Byte Green;
            public readonly Byte Blue;
            public readonly Byte Alpha;

            public static implicit operator BGR24(Color color) { return color.ToBGR24(); }
            public static implicit operator RGB24(Color color) { return color.ToRGB24(); }

            public static implicit operator BGRA32(Color color) { return color.ToBGRA32(); }
            public static implicit operator RGBA32(Color color) { return color.ToRGBA32(); }
            public static implicit operator ARGB32(Color color) { return color.ToARGB32(); }

            public BGR24 ToBGR24() { return new BGR24(Red, Green, Blue); }
            public RGB24 ToRGB24() { return new RGB24(Red, Green, Blue); }
            public BGRA32 ToBGRA32() { return new BGRA32(Red, Green, Blue, Alpha); }
            public RGBA32 ToRGBA32() { return new RGBA32(Red, Green, Blue, Alpha); }
            public ARGB32 ToARGB32() { return new ARGB32(Red, Green, Blue, Alpha); }
        }

    }

    public static class Colors
    {
        public static readonly COLOR TransparentWhite = new COLOR(255, 255, 255, 0);
        public static readonly COLOR TransparentBlack = new COLOR(0, 0, 0, 0);

        public static readonly COLOR White = new COLOR(255, 255, 255);
        public static readonly COLOR Black = new COLOR(0, 0, 0);

        public static readonly COLOR Red = new COLOR(255, 0, 0);
        public static readonly COLOR Green = new COLOR(0, 255, 0);
        public static readonly COLOR Blue = new COLOR(0, 0, 255);
    }
}
