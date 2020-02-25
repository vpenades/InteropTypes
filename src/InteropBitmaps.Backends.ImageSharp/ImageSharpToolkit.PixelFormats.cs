using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp.PixelFormats;
using ELEMENT = InteropBitmaps.PixelElementFormat;

namespace InteropBitmaps
{
    // ImageSharpImageFactory
    // ImageSharpColorConverter

    partial class ImageSharpToolkit
    {
        public static PixelFormat GetInteropPixelFormat<TPixel>() where TPixel:struct,IPixel<TPixel>
        {
            return typeof(TPixel).GetImageSharpInteropPixelFormat();
        }

        private static readonly uint _Fmt_Rgb24 = new PixelFormat(ELEMENT.Red8, ELEMENT.Green8, ELEMENT.Blue8).PackedFormat;
        private static readonly uint _Fmt_Argb32 = new PixelFormat(ELEMENT.Alpha8, ELEMENT.Red8, ELEMENT.Green8, ELEMENT.Blue8).PackedFormat;
        private static readonly uint _Fmt_Rgba32 = new PixelFormat(ELEMENT.Red8, ELEMENT.Green8, ELEMENT.Blue8, ELEMENT.Alpha8).PackedFormat;
        private static readonly uint _Fmt_Bgra32 = new PixelFormat(ELEMENT.Blue8, ELEMENT.Green8, ELEMENT.Red8, ELEMENT.Alpha8).PackedFormat;
        private static readonly uint _Fmt_RgbaVector = new PixelFormat(ELEMENT.Red32F, ELEMENT.Green32F, ELEMENT.Blue32F, ELEMENT.Alpha32F).PackedFormat;

        public static PixelFormat GetImageSharpInteropPixelFormat(this Type pixelType)
        {
            if (pixelType == typeof(Rgb24)) return new PixelFormat(_Fmt_Rgb24);
            if (pixelType == typeof(Argb32)) return new PixelFormat(_Fmt_Argb32);
            if (pixelType == typeof(Rgba32)) return new PixelFormat(_Fmt_Rgba32);
            if (pixelType == typeof(Bgra32)) return new PixelFormat(_Fmt_Bgra32);
            if (pixelType == typeof(RgbaVector)) return new PixelFormat(_Fmt_RgbaVector);

            throw new NotImplementedException(pixelType.ToString());
        }

        public static Type ToImageSharpPixelFormat(this PixelFormat fmt)
        {
            throw new NotImplementedException();
        }
    }
}
