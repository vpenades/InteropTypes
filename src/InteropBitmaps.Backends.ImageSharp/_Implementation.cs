﻿using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using STDPIXEL = InteropBitmaps.Pixel.Standard;
using PACKPIXEL = InteropBitmaps.Pixel.Packed;

namespace InteropBitmaps
{
    static class _Implementation
    {
        #region pixel formats       

        public static Pixel.Format GetPixelFormat<TPixel>()
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (typeof(TPixel) == typeof(A8)) return STDPIXEL.Alpha8;
            if (typeof(TPixel) == typeof(L8)) return STDPIXEL.Gray8;            

            if (typeof(TPixel) == typeof(L16)) return STDPIXEL.Gray16;
            if (typeof(TPixel) == typeof(Bgr565)) return STDPIXEL.BGR565;
            if (typeof(TPixel) == typeof(Bgra5551)) return STDPIXEL.BGRA5551;
            if (typeof(TPixel) == typeof(Bgra4444)) return STDPIXEL.BGRA4444;

            if (typeof(TPixel) == typeof(Bgr24)) return STDPIXEL.BGR24;
            if (typeof(TPixel) == typeof(Rgb24)) return STDPIXEL.RGB24;

            if (typeof(TPixel) == typeof(Argb32)) return STDPIXEL.ARGB32;
            if (typeof(TPixel) == typeof(Bgra32)) return STDPIXEL.BGRA32;
            if (typeof(TPixel) == typeof(Rgba32)) return STDPIXEL.RGBA32;

            throw new NotImplementedException();
        }

        public static Type ToPixelFormat(Pixel.Format fmt)
        {
            switch (fmt.PackedFormat)
            {
                case PACKPIXEL.Alpha8: return typeof(A8);
                case PACKPIXEL.Gray8: return typeof(L8);

                case PACKPIXEL.Gray16: return typeof(L16);
                case PACKPIXEL.BGR565: return typeof(Bgr565);
                case PACKPIXEL.BGRA5551: return typeof(Bgra5551);
                case PACKPIXEL.BGRA4444: return typeof(Bgra4444);

                case PACKPIXEL.RGB24: return typeof(Rgb24);
                case PACKPIXEL.BGR24: return typeof(Bgr24);

                case PACKPIXEL.RGBA32: return typeof(Rgba32);
                case PACKPIXEL.BGRA32: return typeof(Bgra32);
                case PACKPIXEL.ARGB32: return typeof(Argb32);

                default: throw new NotImplementedException();
            }
        }

        public static Image TryWrapImageSharp(MemoryBitmap src)
        {
            var pixType = ToPixelFormat(src.PixelFormat);

            if (pixType == typeof(A8)) return TryWrapImageSharp<A8>(src);
            if (pixType == typeof(L8)) return TryWrapImageSharp<L8>(src);

            if (pixType == typeof(L16)) return TryWrapImageSharp<L16>(src);
            if (pixType == typeof(Bgr565)) return TryWrapImageSharp<Bgr565>(src);
            if (pixType == typeof(Bgra5551)) return TryWrapImageSharp<Bgra5551>(src);
            if (pixType == typeof(Bgra4444)) return TryWrapImageSharp<Bgra4444>(src);

            if (pixType == typeof(Bgr24)) return TryWrapImageSharp<Bgr24>(src);
            if (pixType == typeof(Rgb24)) return TryWrapImageSharp<Rgb24>(src);

            if (pixType == typeof(Argb32)) return TryWrapImageSharp<Argb32>(src);
            if (pixType == typeof(Bgra32)) return TryWrapImageSharp<Bgra32>(src);
            if (pixType == typeof(Rgba32)) return TryWrapImageSharp<Rgba32>(src);

            throw new NotImplementedException();
        }


        #endregion

        #region API      

        public static Image<TPixel> TryWrapImageSharp<TPixel>(this MemoryBitmap src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            // ImageSharp does not support non-continuous pixel rows.
            if (!src.Info.IsContinuous) return null;

            var pixType = ToPixelFormat(src.PixelFormat);
            if (pixType != typeof(TPixel)) throw new ArgumentException(nameof(src.PixelFormat));

            var memMngr = new Adapters.CastMemoryManager<Byte, TPixel>(src.Memory);

            return Image<TPixel>.WrapMemory(memMngr, src.Width, src.Height);
        }

        public static Image CreateImageSharp(Pixel.Format fmt, int width, int height)
        {
            switch (fmt.PackedFormat)
            {
                case PACKPIXEL.Alpha8: return new Image<A8>(width, height);
                case PACKPIXEL.Gray8: return new Image<L8>(width, height);

                case PACKPIXEL.Gray16: return new Image<L16>(width, height);
                case PACKPIXEL.BGR565: return new Image<Bgr565>(width, height);
                case PACKPIXEL.BGRA5551: return new Image<Bgra5551>(width, height);
                case PACKPIXEL.BGRA4444: return new Image<Bgra4444>(width, height);

                case PACKPIXEL.RGB24: return new Image<Rgb24>(width, height);
                case PACKPIXEL.BGR24: return new Image<Bgr24>(width, height);

                case PACKPIXEL.RGBA32: return new Image<Rgba32>(width, height);
                case PACKPIXEL.BGRA32: return new Image<Bgra32>(width, height);
                case PACKPIXEL.ARGB32: return new Image<Argb32>(width, height);

                default: throw new NotImplementedException();
            }
        }

        public static SpanBitmap WrapAsSpanBitmap(Image src)
        {
            if (src == null) return default;

            if (src is Image<A8> a8) return WrapAsSpanBitmap(a8);
            if (src is Image<L8> b8) return WrapAsSpanBitmap(b8);

            if (src is Image<L16> a16) return WrapAsSpanBitmap(a16);
            if (src is Image<Bgr565> b16) return WrapAsSpanBitmap(b16);
            if (src is Image<Bgra5551> c16) return WrapAsSpanBitmap(c16);
            if (src is Image<Bgra4444> d16) return WrapAsSpanBitmap(d16);

            if (src is Image<Rgb24> a24) return WrapAsSpanBitmap(a24);
            if (src is Image<Bgr24> b24) return WrapAsSpanBitmap(b24);

            if (src is Image<Rgba32> a32) return WrapAsSpanBitmap(a32);
            if (src is Image<Bgra32> b32) return WrapAsSpanBitmap(b32);
            if (src is Image<Argb32> c32) return WrapAsSpanBitmap(c32);
            if (src is Image<Rgba1010102> d32) return WrapAsSpanBitmap(d32);

            if (src is Image<Rgb48> a48) return WrapAsSpanBitmap(a48);

            if (src is Image<Rgba64> a64) return WrapAsSpanBitmap(a64);

            if (src is Image<HalfSingle> ah) return WrapAsSpanBitmap(ah);
            if (src is Image<HalfVector2> bh) return WrapAsSpanBitmap(bh);
            if (src is Image<HalfVector4> ch) return WrapAsSpanBitmap(ch);

            if (src is Image<RgbaVector> av) return WrapAsSpanBitmap(av);

            throw new NotImplementedException();
        }

        public static SpanBitmap<TPixel> WrapAsSpanBitmap<TPixel>(Image<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (src == null) return default;

            var pfmt = GetPixelFormat<TPixel>();

            if (src.TryGetSinglePixelSpan(out Span<TPixel> srcSpan))
            {
                // We assume ImageSharp guarantees that memory is continuous.
                var span = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Byte>(srcSpan);

                return new SpanBitmap<TPixel>(span, src.Width, src.Height, pfmt);
            }

            throw new NotSupportedException();
        }

        public static Image CloneToImageSharp(SpanBitmap src)
        {
            var dst = CreateImageSharp(src.PixelFormat, src.Width, src.Height);

            dst.AsSpanBitmap().SetPixels(0, 0, src);

            return dst;
        }

        public static Image<TPixel> CloneToImageSharp<TPixel>(SpanBitmap src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            System.Diagnostics.Debug.Assert(ToPixelFormat(src.PixelFormat) == typeof(TPixel));

            return CloneToImageSharp(src.OfType<TPixel>());
        }

        public static Image<TPixel> CloneToImageSharp<TPixel>(SpanBitmap<TPixel> src)
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
        
        public static void Mutate(SpanBitmap src, Action<IImageProcessingContext> operation)            
        {
            using (var tmp = CloneToImageSharp(src))
            {
                tmp.Mutate(operation);

                // if size has changed, throw error.
                if (tmp.Width != src.Width || tmp.Height != src.Height) throw new ArgumentException("Operations that resize the source image are not allowed.", nameof(operation));

                // transfer pixels back to src.
                src.SetPixels(0, 0, WrapAsSpanBitmap(tmp));
            }
        }

        #endregion
    }
}
