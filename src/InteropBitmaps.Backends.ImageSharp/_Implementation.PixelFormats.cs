using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace InteropBitmaps
{
    partial class _Implementation
    {
        public static bool TryGetExactPixelFormat<TPixel>(out PixelFormat fmt)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (typeof(TPixel) == typeof(A8)) { fmt = Pixel.Alpha8.Format; return true; }
            if (typeof(TPixel) == typeof(L8)) { fmt = Pixel.Luminance8.Format; return true; }

            if (typeof(TPixel) == typeof(L16)) { fmt = Pixel.Luminance16.Format; return true; }
            if (typeof(TPixel) == typeof(Bgr565)) { fmt = Pixel.BGR565.Format; return true; }
            if (typeof(TPixel) == typeof(Bgra4444)) { fmt = Pixel.BGRA4444.Format; return true; }
            if (typeof(TPixel) == typeof(Bgra5551)) { fmt = Pixel.BGRA5551.Format; return true; }


            if (typeof(TPixel) == typeof(Bgr24)) { fmt = Pixel.BGR24.Format; return true; }
            if (typeof(TPixel) == typeof(Rgb24)) { fmt = Pixel.RGB24.Format; return true; }

            if (typeof(TPixel) == typeof(Argb32)) { fmt = Pixel.ARGB32.Format; return true; }
            if (typeof(TPixel) == typeof(Bgra32)) { fmt = Pixel.BGRA32.Format; return true; }
            if (typeof(TPixel) == typeof(Rgba32)) { fmt = Pixel.RGBA32.Format; return true; }

            if (typeof(TPixel) == typeof(RgbaVector)) { fmt = Pixel.RGBA128F.Format; return true; }

            fmt = default;
            return false;
        }

        public static bool TryGetExactPixelType(PixelFormat fmt, out Type type)
        {
            switch (fmt.Code)
            {
                case Pixel.Alpha8.Code: type = typeof(A8); return true;

                case Pixel.Luminance8.Code: type = typeof(L8); return true;
                case Pixel.Luminance16.Code: type = typeof(L16); return true;

                case Pixel.BGR565.Code: type = typeof(Bgr565); return true;
                case Pixel.BGRA4444.Code: type = typeof(Bgra4444); return true;
                case Pixel.BGRA5551.Code: type = typeof(Bgra5551); return true;

                case Pixel.RGB24.Code: type = typeof(Rgb24); return true;
                case Pixel.BGR24.Code: type = typeof(Bgr24); return true;

                case Pixel.RGBA32.Code: type = typeof(Rgba32); return true;
                case Pixel.BGRA32.Code: type = typeof(Bgra32); return true;
                case Pixel.ARGB32.Code: type = typeof(Argb32); return true;

                case Pixel.RGBA128F.Code: type = typeof(RgbaVector); return true;

                default: type = null; return false;
            }
        }        
    }
}