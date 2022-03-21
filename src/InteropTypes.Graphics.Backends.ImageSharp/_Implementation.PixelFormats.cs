
using System;

using InteropTypes.Graphics.Bitmaps;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace InteropTypes
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
            if (typeof(TPixel) == typeof(Bgra5551)) { fmt = Pixel.BGRA5551.Format; return true; }
            if (typeof(TPixel) == typeof(Bgra4444)) { fmt = Pixel.BGRA4444.Format; return true; }
            if (typeof(TPixel) == typeof(Bgr24)) { fmt = Pixel.BGR24.Format; return true; }
            if (typeof(TPixel) == typeof(Rgb24)) { fmt = Pixel.RGB24.Format; return true; }
            if (typeof(TPixel) == typeof(Bgra32)) { fmt = Pixel.BGRA32.Format; return true; }
            if (typeof(TPixel) == typeof(Rgba32)) { fmt = Pixel.RGBA32.Format; return true; }
            if (typeof(TPixel) == typeof(Argb32)) { fmt = Pixel.ARGB32.Format; return true; }
            if (typeof(TPixel) == typeof(RgbaVector)) { fmt = Pixel.RGBA128F.Format; return true; }

            fmt = default;
            return false;
        }

        public static bool TryGetExactPixelFormat(Image src, out PixelFormat fmt)
        {
            if (src is Image<A8>) { fmt = Pixel.Alpha8.Format; return true; }
            if (src is Image<L8>) { fmt = Pixel.Luminance8.Format; return true; }
            if (src is Image<L16>) { fmt = Pixel.Luminance16.Format; return true; }
            if (src is Image<Bgr565>) { fmt = Pixel.BGR565.Format; return true; }
            if (src is Image<Bgra5551>) { fmt = Pixel.BGRA5551.Format; return true; }
            if (src is Image<Bgra4444>) { fmt = Pixel.BGRA4444.Format; return true; }
            if (src is Image<Bgr24>) { fmt = Pixel.BGR24.Format; return true; }
            if (src is Image<Rgb24>) { fmt = Pixel.RGB24.Format; return true; }
            if (src is Image<Bgra32>) { fmt = Pixel.BGRA32.Format; return true; }
            if (src is Image<Rgba32>) { fmt = Pixel.RGBA32.Format; return true; }
            if (src is Image<Argb32>) { fmt = Pixel.ARGB32.Format; return true; }
            if (src is Image<RgbaVector>) { fmt = Pixel.RGBA128F.Format; return true; }

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
            case Pixel.BGRA5551.Code: type = typeof(Bgra5551); return true;
            case Pixel.BGRA4444.Code: type = typeof(Bgra4444); return true;
            case Pixel.BGR24.Code: type = typeof(Bgr24); return true;
            case Pixel.RGB24.Code: type = typeof(Rgb24); return true;
            case Pixel.BGRA32.Code: type = typeof(Bgra32); return true;
            case Pixel.RGBA32.Code: type = typeof(Rgba32); return true;
            case Pixel.ARGB32.Code: type = typeof(Argb32); return true;
            case Pixel.RGBA128F.Code: type = typeof(RgbaVector); return true;
                default: type = null; return false;
            }
        }

        public static bool TryWrapAsSpanBitmap(Image src, SpanBitmap.Action1 action)
        {
            if (src is Image<A8> imageA8) return TryWrapAsSpanBitmap<A8, Pixel.Alpha8>(imageA8, s => action(s) );
            if (src is Image<L8> imageL8) return TryWrapAsSpanBitmap<L8, Pixel.Luminance8>(imageL8, s => action(s) );
            if (src is Image<L16> imageL16) return TryWrapAsSpanBitmap<L16, Pixel.Luminance16>(imageL16, s => action(s) );
            if (src is Image<Bgr565> imageBgr565) return TryWrapAsSpanBitmap<Bgr565, Pixel.BGR565>(imageBgr565, s => action(s) );
            if (src is Image<Bgra5551> imageBgra5551) return TryWrapAsSpanBitmap<Bgra5551, Pixel.BGRA5551>(imageBgra5551, s => action(s) );
            if (src is Image<Bgra4444> imageBgra4444) return TryWrapAsSpanBitmap<Bgra4444, Pixel.BGRA4444>(imageBgra4444, s => action(s) );
            if (src is Image<Bgr24> imageBgr24) return TryWrapAsSpanBitmap<Bgr24, Pixel.BGR24>(imageBgr24, s => action(s) );
            if (src is Image<Rgb24> imageRgb24) return TryWrapAsSpanBitmap<Rgb24, Pixel.RGB24>(imageRgb24, s => action(s) );
            if (src is Image<Bgra32> imageBgra32) return TryWrapAsSpanBitmap<Bgra32, Pixel.BGRA32>(imageBgra32, s => action(s) );
            if (src is Image<Rgba32> imageRgba32) return TryWrapAsSpanBitmap<Rgba32, Pixel.RGBA32>(imageRgba32, s => action(s) );
            if (src is Image<Argb32> imageArgb32) return TryWrapAsSpanBitmap<Argb32, Pixel.ARGB32>(imageArgb32, s => action(s) );
            if (src is Image<RgbaVector> imageRgbaVector) return TryWrapAsSpanBitmap<RgbaVector, Pixel.RGBA128F>(imageRgbaVector, s => action(s) );

            throw new NotImplementedException();
        }
        
        public static Image TryWrapAsImageSharp(MemoryBitmap src)
        {
            switch(src.PixelFormat.Code)
            {                
                case Pixel.Alpha8.Code: return WrapAsImageSharp<A8>(src);
            case Pixel.Luminance8.Code: return WrapAsImageSharp<L8>(src);
            case Pixel.Luminance16.Code: return WrapAsImageSharp<L16>(src);
            case Pixel.BGR565.Code: return WrapAsImageSharp<Bgr565>(src);
            case Pixel.BGRA5551.Code: return WrapAsImageSharp<Bgra5551>(src);
            case Pixel.BGRA4444.Code: return WrapAsImageSharp<Bgra4444>(src);
            case Pixel.BGR24.Code: return WrapAsImageSharp<Bgr24>(src);
            case Pixel.RGB24.Code: return WrapAsImageSharp<Rgb24>(src);
            case Pixel.BGRA32.Code: return WrapAsImageSharp<Bgra32>(src);
            case Pixel.RGBA32.Code: return WrapAsImageSharp<Rgba32>(src);
            case Pixel.ARGB32.Code: return WrapAsImageSharp<Argb32>(src);
            case Pixel.RGBA128F.Code: return WrapAsImageSharp<RgbaVector>(src);
                default: throw src.PixelFormat.ThrowArgument(nameof(src));
            }
        }
        
        public static Image CreateImageSharp(PixelFormat fmt, int width, int height)
        {
            switch (fmt.Code)
            {
                case Pixel.Alpha8.Code: return new Image<A8>(width, height);
            case Pixel.Luminance8.Code: return new Image<L8>(width, height);
            case Pixel.Luminance16.Code: return new Image<L16>(width, height);
            case Pixel.BGR565.Code: return new Image<Bgr565>(width, height);
            case Pixel.BGRA5551.Code: return new Image<Bgra5551>(width, height);
            case Pixel.BGRA4444.Code: return new Image<Bgra4444>(width, height);
            case Pixel.BGR24.Code: return new Image<Bgr24>(width, height);
            case Pixel.RGB24.Code: return new Image<Rgb24>(width, height);
            case Pixel.BGRA32.Code: return new Image<Bgra32>(width, height);
            case Pixel.RGBA32.Code: return new Image<Rgba32>(width, height);
            case Pixel.ARGB32.Code: return new Image<Argb32>(width, height);
            case Pixel.RGBA128F.Code: return new Image<RgbaVector>(width, height);
                default: throw fmt.ThrowArgument(nameof(fmt));
            }
        }

    }
}