using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ELEMENT = InteropBitmaps.ComponentFormat;
using PEF = InteropBitmaps.ComponentFormat;

namespace InteropBitmaps
{
    // ImageSharpImageFactory
    // ImageSharpColorConverter

    partial class ImageSharpToolkit
    {
        public static SpanBitmap AsSpanBitmap(this Image src) { return _Implementation.AsSpanBitmap(src); }

        public static Image CreateImageSharp(this PixelFormat fmt, int width, int height) { return _Implementation.CreateImageSharp(fmt, width, height); }

        public static PixelFormat GetPixelFormat<TPixel>()
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return _Implementation.GetPixelFormat<TPixel>();
        }

        public static Type ToImageSharpPixelFormat(this PixelFormat fmt) { return _Implementation.ToImageSharp(fmt); }
    }
}
