using System;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace InteropBitmaps
{
    public static partial class ImageSharpToolkit
    {
        public static ImageSharpAdapter AsImageSharp(this SpanBitmap bitmap) { return new ImageSharpAdapter(bitmap); }

        public static ImageSharpAdapter AsImageSharp(this MemoryBitmap bitmap) { return new ImageSharpAdapter(bitmap.AsSpanBitmap()); }

        public static ImageSharpAdapter<TPixel> AsImageSharp<TPixel>(this SpanBitmap<TPixel> bitmap)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return new ImageSharpAdapter<TPixel>(bitmap);
        }

        public static ImageSharpAdapter<TPixel> AsImageSharp<TPixel>(this MemoryBitmap<TPixel> bitmap)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return new ImageSharpAdapter<TPixel>(bitmap);
        }        

        public static MemoryBitmap<TPixel> CopyToMemoryBitmap<TPixel>(this Image<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var dst = new MemoryBitmap<TPixel>(src.Width, src.Height, GetPixelFormat<TPixel>());

            for (int y = 0; y < dst.Height; ++y)
            {
                var srcLine = src.Frames[0].GetPixelRowSpan(y);
                var dstLine = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte,TPixel>(dst.UseBytesScanline(y));
                srcLine.CopyTo(dstLine);
            }

            return dst;
        }

        public static SpanBitmap<TPixel> AsSpanBitmap<TPixel>(this Image<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var span = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Byte>(src.GetPixelSpan());

            return new SpanBitmap<TPixel>(span, src.Width, src.Height, GetPixelFormat<TPixel>());
        }

        public static SpanBitmap AsSpanBitmap(this Image src)
        {
            if (src is Image<Alpha8> a8) return a8.AsSpanBitmap().AsSpanBitmap();
            if (src is Image<Gray8> b8) return b8.AsSpanBitmap().AsSpanBitmap();
            if (src is Image<Gray16> c8) return c8.AsSpanBitmap().AsSpanBitmap();            

            if (src is Image<Bgra4444> a16) return a16.AsSpanBitmap().AsSpanBitmap();
            if (src is Image<Bgra5551> b16) return b16.AsSpanBitmap().AsSpanBitmap();
            if (src is Image<Bgr565> c16) return c16.AsSpanBitmap().AsSpanBitmap();

            if (src is Image<Rgb24> a24) return a24.AsSpanBitmap().AsSpanBitmap();
            if (src is Image<Bgr24> b24) return b24.AsSpanBitmap().AsSpanBitmap();

            if (src is Image<Rgba32> a32) return a32.AsSpanBitmap().AsSpanBitmap();
            if (src is Image<Bgra32> b32) return b32.AsSpanBitmap().AsSpanBitmap();
            if (src is Image<Argb32> c32) return c32.AsSpanBitmap().AsSpanBitmap();

            throw new NotImplementedException();
        }


        public static Image CreateImageSharp(this BitmapInfo binfo)
        {
            return binfo.PixelFormat.CreateImageSharp(binfo.Width, binfo.Height);
        }

        public static Image CreateImageSharp(this PixelFormat fmt, int width, int height)
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


    }
}
