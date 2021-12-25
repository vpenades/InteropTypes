using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace InteropBitmaps
{
    public delegate void TransferAction(SpanBitmap src, SpanBitmap dst);

    public delegate void MutateAction(SpanBitmap bmp);

    static partial class _Implementation
    {
        #region API

        public static Image TryWrapImageSharp(MemoryBitmap src)
        {
            if (!TryGetExactPixelType(src.PixelFormat, out var pixType)) throw new NotImplementedException();

            if (pixType == typeof(A8)) return TryWrapImageSharp<A8>(src);
            if (pixType == typeof(L8)) return TryWrapImageSharp<L8>(src);

            if (pixType == typeof(L16)) return TryWrapImageSharp<L16>(src);
            if (pixType == typeof(Bgr565)) return TryWrapImageSharp<Bgr565>(src);
            if (pixType == typeof(Bgra4444)) return TryWrapImageSharp<Bgra4444>(src);
            if (pixType == typeof(Bgra5551)) return TryWrapImageSharp<Bgra5551>(src);

            if (pixType == typeof(Bgr24)) return TryWrapImageSharp<Bgr24>(src);
            if (pixType == typeof(Rgb24)) return TryWrapImageSharp<Rgb24>(src);

            if (pixType == typeof(Argb32)) return TryWrapImageSharp<Argb32>(src);
            if (pixType == typeof(Bgra32)) return TryWrapImageSharp<Bgra32>(src);
            if (pixType == typeof(Rgba32)) return TryWrapImageSharp<Rgba32>(src);

            if (pixType == typeof(RgbaVector)) return TryWrapImageSharp<RgbaVector>(src);

            throw new NotImplementedException();
        }

        public static bool TryWrap<TPixel>(Image<TPixel> src, out SpanBitmap<TPixel> dst)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            dst = default;

            if (src == null) return false;

            if (!TryGetExactPixelFormat<TPixel>(out var pfmt)) return false;

            if (!src.TryGetSinglePixelSpan(out Span<TPixel> srcSpan)) return false;

            // We assume ImageSharp guarantees that memory is continuous.
            var span = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Byte>(srcSpan);

            dst = new SpanBitmap<TPixel>(span, src.Width, src.Height, pfmt);

            return true;
        }

        public static Image<TPixel> TryWrapImageSharp<TPixel>(this MemoryBitmap src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            // ImageSharp does not support non-continuous pixel rows.
            if (!src.Info.IsContinuous) return null;

            TryGetExactPixelType(src.PixelFormat, out var pixType);
            if (pixType != typeof(TPixel)) throw new Diagnostics.PixelFormatNotSupportedException(src.PixelFormat, nameof(src));

            var memMngr = new Adapters.CastMemoryManager<Byte, TPixel>(src.Memory);

            return Image.WrapMemory(memMngr, src.Width, src.Height);
        }

        public static Image CreateImageSharp(PixelFormat fmt, int width, int height)
        {
            switch (fmt.PackedFormat)
            {
                case Pixel.Alpha8.Code: return new Image<A8>(width, height);
                case Pixel.Luminance8.Code: return new Image<L8>(width, height);

                case Pixel.Luminance16.Code: return new Image<L16>(width, height);
                case Pixel.BGR565.Code: return new Image<Bgr565>(width, height);
                case Pixel.BGRA5551.Code: return new Image<Bgra5551>(width, height);
                case Pixel.BGRA4444.Code: return new Image<Bgra4444>(width, height);

                case Pixel.RGB24.Code: return new Image<Rgb24>(width, height);
                case Pixel.BGR24.Code: return new Image<Bgr24>(width, height);

                case Pixel.RGBP32.Code:
                case Pixel.RGBA32.Code: return new Image<Rgba32>(width, height);
                case Pixel.BGRP32.Code:
                case Pixel.BGRA32.Code: return new Image<Bgra32>(width, height);
                // case Pixel.ARGB32P.Code:
                case Pixel.ARGB32.Code: return new Image<Argb32>(width, height);

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

            if (TryGetExactPixelFormat<TPixel>(out var pfmt))
            {
                if (src.TryGetSinglePixelSpan(out Span<TPixel> srcSpan))
                {
                    // We assume ImageSharp guarantees that memory is continuous.
                    var span = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Byte>(srcSpan);

                    return new SpanBitmap<TPixel>(span, src.Width, src.Height, pfmt);
                }
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
            TryGetExactPixelType(src.PixelFormat, out var pixType);
            System.Diagnostics.Debug.Assert(pixType == typeof(TPixel));

            return CloneToImageSharp(src.OfType<TPixel>());
        }

        public static Image<TPixel> CloneToImageSharp<TPixel>(SpanBitmap<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var dst = new Image<TPixel>(src.Width, src.Height);

            for (int y = 0; y < dst.Height; ++y)
            {
                var srcLine = src.GetScanlinePixels(y);
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

        public static void Transfer<TPixel>(SpanBitmap<TPixel> src, Image<TPixel> dst, TransferAction action)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            src = src.AsReadOnly();

            if (TryWrap(dst, out var dstSpan))
            {
                action(src, dstSpan);                
            }
            else
            {
                dstSpan = dst.ToMemoryBitmap().AsSpanBitmap();
                action(src, dstSpan);                
            }
        }

        public static void Transfer<TPixel>(Image<TPixel> src, SpanBitmap<TPixel> dst, TransferAction action)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (TryWrap(src, out var srcSpan))
            {
                srcSpan = srcSpan.AsReadOnly();

                action(srcSpan, dst);
            }
            else
            {
                srcSpan = src
                    .ToMemoryBitmap()
                    .AsSpanBitmap()
                    .AsReadOnly();

                action(srcSpan, dst);
            }
        }

        #endregion
    }
}
