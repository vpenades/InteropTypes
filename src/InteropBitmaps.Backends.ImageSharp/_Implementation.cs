using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace InteropBitmaps
{
    static class _Implementation
    {
        #region pixel formats

        public static SpanBitmap AsSpanBitmap(Image src)
        {
            if (src is Image<Alpha8> a8) return a8.AsSpanBitmap();
            if (src is Image<Gray8> b8) return b8.AsSpanBitmap();

            if (src is Image<Gray16> a16) return a16.AsSpanBitmap();
            if (src is Image<Bgr565> b16) return b16.AsSpanBitmap();
            if (src is Image<Bgra5551> c16) return c16.AsSpanBitmap();
            if (src is Image<Bgra4444> d16) return d16.AsSpanBitmap();

            if (src is Image<Rgb24> a24) return a24.AsSpanBitmap();
            if (src is Image<Bgr24> b24) return b24.AsSpanBitmap();

            if (src is Image<Rgba32> a32) return a32.AsSpanBitmap();
            if (src is Image<Bgra32> b32) return b32.AsSpanBitmap();
            if (src is Image<Argb32> c32) return c32.AsSpanBitmap();
            if (src is Image<Rgba1010102> d32) return d32.AsSpanBitmap();

            if (src is Image<Rgb48> a48) return a48.AsSpanBitmap();

            if (src is Image<Rgba64> a64) return a64.AsSpanBitmap();

            if (src is Image<HalfSingle> ah) return ah.AsSpanBitmap();
            if (src is Image<HalfVector2> bh) return bh.AsSpanBitmap();
            if (src is Image<HalfVector4> ch) return ch.AsSpanBitmap();

            if (src is Image<RgbaVector> av) return av.AsSpanBitmap();

            throw new NotImplementedException();
        }

        public static SpanBitmap<TPixel> AsSpanBitmap<TPixel>(Image<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var span = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Byte>(src.GetPixelSpan());
            var pfmt = GetPixelFormat<TPixel>();

            return new SpanBitmap<TPixel>(span, src.Width, src.Height, pfmt);
        }

        public static Image CreateImageSharp(PixelFormat fmt, int width, int height)
        {
            switch (fmt.PackedFormat)
            {
                case PixelFormat.Packed.ALPHA8: return new Image<Alpha8>(width, height);
                case PixelFormat.Packed.GRAY8: return new Image<Gray8>(width, height);

                case PixelFormat.Packed.GRAY16: return new Image<Gray16>(width, height);
                case PixelFormat.Packed.BGR565: return new Image<Bgr565>(width, height);
                case PixelFormat.Packed.BGRA5551: return new Image<Bgra5551>(width, height);
                case PixelFormat.Packed.BGRA4444: return new Image<Bgra4444>(width, height);

                case PixelFormat.Packed.RGB24: return new Image<Rgb24>(width, height);
                case PixelFormat.Packed.BGR24: return new Image<Bgr24>(width, height);

                case PixelFormat.Packed.RGBA32: return new Image<Rgba32>(width, height);
                case PixelFormat.Packed.BGRA32: return new Image<Bgra32>(width, height);
                case PixelFormat.Packed.ARGB32: return new Image<Argb32>(width, height);

                default: throw new NotImplementedException();
            }
        }

        public static PixelFormat GetPixelFormat<TPixel>()
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (typeof(TPixel) == typeof(Gray8)) return PixelFormat.Standard.GRAY8;
            if (typeof(TPixel) == typeof(Alpha8)) return PixelFormat.Standard.ALPHA8;

            if (typeof(TPixel) == typeof(Gray16)) return PixelFormat.Standard.GRAY16;
            if (typeof(TPixel) == typeof(Bgr565)) return PixelFormat.Standard.BGR565;
            if (typeof(TPixel) == typeof(Bgra5551)) return PixelFormat.Standard.BGRA5551;
            if (typeof(TPixel) == typeof(Bgra4444)) return PixelFormat.Standard.BGRA4444;

            if (typeof(TPixel) == typeof(Bgr24)) return PixelFormat.Standard.BGR24;
            if (typeof(TPixel) == typeof(Rgb24)) return PixelFormat.Standard.RGB24;

            if (typeof(TPixel) == typeof(Argb32)) return PixelFormat.Standard.ARGB32;
            if (typeof(TPixel) == typeof(Bgra32)) return PixelFormat.Standard.BGRA32;
            if (typeof(TPixel) == typeof(Rgba32)) return PixelFormat.Standard.RGBA32;

            throw new NotImplementedException();
        }

        public static Type ToImageSharp(PixelFormat fmt)
        {
            switch (fmt.PackedFormat)
            {
                case PixelFormat.Packed.ALPHA8: return typeof(Alpha8);
                case PixelFormat.Packed.GRAY8: return typeof(Gray8);
                case PixelFormat.Packed.GRAY16: return typeof(Gray16);

                case PixelFormat.Packed.BGR565: return typeof(Bgr565);
                case PixelFormat.Packed.BGRA5551: return typeof(Bgra5551);
                case PixelFormat.Packed.BGRA4444: return typeof(Bgra4444);

                case PixelFormat.Packed.RGB24: return typeof(Rgb24);
                case PixelFormat.Packed.BGR24: return typeof(Bgr24);

                case PixelFormat.Packed.RGBA32: return typeof(Rgba32);
                case PixelFormat.Packed.BGRA32: return typeof(Bgra32);
                case PixelFormat.Packed.ARGB32: return typeof(Argb32);

                default: throw new NotImplementedException();
            }
        }

        #endregion

        #region clone        

        public static Image ToImageSharp(SpanBitmap src)
        {
            var dst = src.PixelFormat.CreateImageSharp(src.Width, src.Height);

            dst.AsSpanBitmap().SetPixels(0, 0, src);

            return dst;
        }

        public static Image<TPixel> ToImageSharp<TPixel>(SpanBitmap src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            System.Diagnostics.Debug.Assert(ToImageSharp(src.PixelFormat) == typeof(TPixel));

            return ToImageSharp(src.OfType<TPixel>());
        }

        public static Image<TPixel> ToImageSharp<TPixel>(SpanBitmap<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var dst = new Image<TPixel>(src.Width, src.Height);

            for (int y = 0; y < dst.Height; ++y)
            {
                var srcLine = src.GetPixelsScanline(y);
                var dstLine = dst.Frames[0].GetPixelRowSpan(y);
                srcLine.CopyTo(dstLine);
            }

            return dst;
        }

        public static void Mutate<TPixel>(SpanBitmap<TPixel> src, Action<IImageProcessingContext> operation)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var tmp = ToImageSharp(src))
            {
                tmp.Mutate(operation);

                // if size has changed, throw error.
                if (tmp.Width != src.Width || tmp.Height != src.Height) throw new ArgumentException("Operations that resize the source image are not allowed.", nameof(operation));

                // transfer pixels back to src.
                src.SetPixels(0, 0, AsSpanBitmap(tmp));
            }
        }

        #endregion
    }
}
