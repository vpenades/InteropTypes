using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp.PixelFormats;
using ELEMENT = InteropBitmaps.PixelElementFormat;
using PEF = InteropBitmaps.PixelElementFormat;

namespace InteropBitmaps
{
    // ImageSharpImageFactory
    // ImageSharpColorConverter

    partial class ImageSharpToolkit
    {
        public static PixelFormat GetPixelFormat<TPixel>()
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (typeof(TPixel) == typeof(Alpha8)) return new PixelFormat(PEF.Alpha8);
            if (typeof(TPixel) == typeof(Argb32)) return new PixelFormat(PEF.Alpha8, PEF.Red8, PEF.Green8, PEF.Blue8);
            if (typeof(TPixel) == typeof(Bgr24)) return new PixelFormat(PEF.Blue8, PEF.Green8, PEF.Red8);
            if (typeof(TPixel) == typeof(Bgr565)) return new PixelFormat(PEF.Blue5, PEF.Green6, PEF.Red5);
            if (typeof(TPixel) == typeof(Bgra32)) return new PixelFormat(PEF.Blue8, PEF.Green8, PEF.Red8, PEF.Alpha8);
            if (typeof(TPixel) == typeof(Bgra4444)) return new PixelFormat(PEF.Blue4, PEF.Green4, PEF.Red4, PEF.Alpha4);
            if (typeof(TPixel) == typeof(Bgra5551)) return new PixelFormat(PEF.Blue5, PEF.Green5, PEF.Red5, PEF.Alpha1);
            if (typeof(TPixel) == typeof(Gray16)) return new PixelFormat(PEF.Gray16);
            if (typeof(TPixel) == typeof(Gray8)) return new PixelFormat(PEF.Gray8);
            if (typeof(TPixel) == typeof(Rgb24)) return new PixelFormat(PEF.Red8, PEF.Green8, PEF.Blue8);
            if (typeof(TPixel) == typeof(Rgba32)) return new PixelFormat(PEF.Red8, PEF.Green8, PEF.Blue8, PEF.Alpha8);

            throw new NotImplementedException();
        }
        

        public static Type ToImageSharpPixelFormat(this PixelFormat fmt)
        {
            throw new NotImplementedException();
        }
    }
}
