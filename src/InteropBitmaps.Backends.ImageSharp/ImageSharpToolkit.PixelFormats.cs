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
        public static PixelFormat GetPixelFormat<TPixel>()
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (typeof(TPixel) == typeof(Gray8)) return PixelFormat.Standard.GRAY8;
            if (typeof(TPixel) == typeof(Alpha8)) return PixelFormat.Standard.ALPHA8;

            if (typeof(TPixel) == typeof(Gray16)) return PixelFormat.Standard.GRAY16;
            if (typeof(TPixel) == typeof(Bgr565)) return PixelFormat.Standard.BGR565;
            if (typeof(TPixel) == typeof(Bgra4444)) return PixelFormat.Standard.BGRA4444;
            if (typeof(TPixel) == typeof(Bgra5551)) return PixelFormat.Standard.BGRA5551;            

            if (typeof(TPixel) == typeof(Bgr24)) return PixelFormat.Standard.BGR24;
            if (typeof(TPixel) == typeof(Rgb24)) return PixelFormat.Standard.RGB24;            

            if (typeof(TPixel) == typeof(Argb32)) return PixelFormat.Standard.ARGB32;
            if (typeof(TPixel) == typeof(Bgra32)) return PixelFormat.Standard.BGRA32;
            if (typeof(TPixel) == typeof(Rgba32)) return PixelFormat.Standard.RGBA32;            

            throw new NotImplementedException();
        }

        public static Image ImageSharpCreateEmptyImage(this PixelFormat fmt, int width, int height)
        {
            switch (fmt.PackedFormat)
            {
                case PixelFormat.Packed.GRAY8: return new Image<Gray8>(width, height);
                case PixelFormat.Packed.GRAY16: return new Image<Gray16>(width, height);

                case PixelFormat.Packed.ALPHA8: return new Image<Alpha8>(width, height);

                case PixelFormat.Packed.BGR565: return new Image<Bgr565>(width, height);
                case PixelFormat.Packed.BGRA4444: return new Image<Bgra4444>(width, height);

                case PixelFormat.Packed.RGB24: return new Image<Rgb24>(width, height);
                case PixelFormat.Packed.BGR24: return new Image<Bgr24>(width, height);

                case PixelFormat.Packed.RGBA32: return new Image<Rgba32>(width, height);
                case PixelFormat.Packed.BGRA32: return new Image<Bgra32>(width, height);
                case PixelFormat.Packed.ARGB32: return new Image<Argb32>(width, height);

                default: throw new NotImplementedException();
            }
        }
        

        public static Type ToImageSharpPixelFormat(this PixelFormat fmt)
        {
            switch (fmt.PackedFormat)
            {
                case PixelFormat.Packed.GRAY8: return typeof(Gray8);
                case PixelFormat.Packed.GRAY16: return typeof(Gray16);

                case PixelFormat.Packed.ALPHA8: return typeof(Alpha8);

                case PixelFormat.Packed.BGR565: return typeof(Bgr565);
                case PixelFormat.Packed.BGRA4444: return typeof(Bgra4444);

                case PixelFormat.Packed.RGB24: return typeof(Rgb24);
                case PixelFormat.Packed.BGR24: return typeof(Bgr24);

                case PixelFormat.Packed.RGBA32: return typeof(Rgba32);
                case PixelFormat.Packed.BGRA32: return typeof(Bgra32);
                case PixelFormat.Packed.ARGB32: return typeof(Argb32);

                default: throw new NotImplementedException();
            }
        }
    }
}
