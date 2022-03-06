using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Backends;

namespace InteropTypes
{
    partial class _Implementation
    {
        public static TResult ReadAsSpanBitmap<TResult>(Image self, SpanBitmap other, SpanBitmap.Function2<TResult> function)
        {
            if (other.IsEmpty) return ReadAsSpanBitmap<Byte, TResult>(self, default, (s,o) => function(s, default));

            switch(other.PixelFormat.Code)
            {
                case Pixel.Alpha8.Code: return ReadAsSpanBitmap(self, other.OfType<Pixel.Alpha8>(), (s,o) => function(s,o));
                default: throw other.PixelFormat.ThrowArgument(nameof(other));
            }
        }

        public static TResult ReadAsSpanBitmap<TOtherPixel, TResult>(Image self, SpanBitmap<TOtherPixel> other, SpanBitmap<TOtherPixel>.Function2<TResult> function)
            where TOtherPixel : unmanaged
        {
            // [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            TResult _run<TSelfPixel>(SpanBitmap<TOtherPixel> otherx)
                where TSelfPixel : unmanaged, IPixel<TSelfPixel>
            {
                if (!(self is Image<TSelfPixel> selfTyped)) throw new ArgumentNullException("null or format mismatch", nameof(self));

                return ReadAsSpanBitmap(selfTyped, otherx, function);
            }            

            if (typeof(TOtherPixel) == typeof(Pixel.Alpha8)) return _run<A8>(other);

            if (typeof(TOtherPixel) == typeof(Pixel.Luminance8)) return _run<L8>(other);
            if (typeof(TOtherPixel) == typeof(Pixel.Luminance16)) return _run<L16>(other);

            if (typeof(TOtherPixel) == typeof(Pixel.BGR565)) return _run<Bgr565>(other);
            if (typeof(TOtherPixel) == typeof(Pixel.BGRA4444)) return _run<Bgra4444>(other);
            if (typeof(TOtherPixel) == typeof(Pixel.BGRA5551)) return _run<Bgra5551>(other);

            if (typeof(TOtherPixel) == typeof(Pixel.RGB24)) return _run<Rgb24>(other);
            if (typeof(TOtherPixel) == typeof(Pixel.BGR24)) return _run<Bgr24>(other);

            if (typeof(TOtherPixel) == typeof(Pixel.RGBA32)) return _run<Rgba32>(other);
            if (typeof(TOtherPixel) == typeof(Pixel.BGRA32)) return _run<Bgra32>(other);
            if (typeof(TOtherPixel) == typeof(Pixel.ARGB32)) return _run<Argb32>(other);

            if (typeof(TOtherPixel) == typeof(Pixel.RGBA128F)) return _run<RgbaVector>(other);

            throw new NotImplementedException(self.GetType().Name);
        }        

        public static void WriteAsSpanBitmap(Image self, SpanBitmap other, SpanBitmap.Action2 action)
        {
            if (self is Image<A8> srcA8) { WriteAsSpanBitmap(srcA8, other.OfTypeOrDefault<Pixel.Alpha8>(), (s, o) => action(s, o)); return; }
            if (self is Image<L8> srcL8) { WriteAsSpanBitmap(srcL8, other.OfTypeOrDefault<Pixel.Luminance8>(), (s, o) => action(s, o)); return; }

            if (self is Image<Bgr565> srcBgr565) { WriteAsSpanBitmap(srcBgr565, other.OfTypeOrDefault<Pixel.BGR565>(), (s, o) => action(s, o)); return; }
            if (self is Image<Bgra4444> srcBgra4444) { WriteAsSpanBitmap(srcBgra4444, other.OfTypeOrDefault<Pixel.BGRA4444>(), (s, o) => action(s, o)); return; }
            if (self is Image<Bgra5551> srcBgra5551) { WriteAsSpanBitmap(srcBgra5551, other.OfTypeOrDefault<Pixel.BGRA5551>(), (s, o) => action(s, o)); return; }

            if (self is Image<Rgb24> srcRgb24) { WriteAsSpanBitmap(srcRgb24, other.OfTypeOrDefault<Pixel.BGR24>(), (s, o) => action(s, o)); return; }
            if (self is Image<Bgr24> srcBgr24) { WriteAsSpanBitmap(srcBgr24, other.OfTypeOrDefault<Pixel.RGB24>(), (s, o) => action(s, o)); return; }

            if (self is Image<Rgba32> srcRgba32) { WriteAsSpanBitmap(srcRgba32, other.OfTypeOrDefault<Pixel.RGBA32>(), (s, o) => action(s, o)); return; }
            if (self is Image<Bgra32> srcBgra32) { WriteAsSpanBitmap(srcBgra32, other.OfTypeOrDefault<Pixel.BGRA32>(), (s, o) => action(s, o)); return; }
            if (self is Image<Argb32> srcArgb32) { WriteAsSpanBitmap(srcArgb32, other.OfTypeOrDefault<Pixel.ARGB32>(), (s, o) => action(s, o)); return; }

            if (self is Image<RgbaVector> srcRgbaVector) { WriteAsSpanBitmap(srcRgbaVector, other.OfTypeOrDefault<Pixel.RGBA128F>(), (s, o) => action(s, o)); return; }

            throw new NotImplementedException();
        }

        public static void WriteAsImageSharp(MemoryBitmap self, Action<Image> action)
        {
            switch (self.Info.PixelFormat.Code)
            {
                case Pixel.Luminance8.Code: self.OfType<Pixel.Luminance8>().WriteAsImageSharp<Pixel.Luminance8, L8>(Image => action(Image)); break;
                case Pixel.BGR24.Code: self.OfType<Pixel.BGR24>().WriteAsImageSharp<Pixel.BGR24, Bgr24>(Image => action(Image)); break;
                case Pixel.RGBA32.Code: self.OfType<Pixel.RGBA32>().WriteAsImageSharp<Pixel.RGBA32, Rgba32>(Image => action(Image)); break;
                case Pixel.BGRA32.Code: self.OfType<Pixel.BGRA32>().WriteAsImageSharp<Pixel.BGRA32, Bgra32>(Image => action(Image)); break;
                case Pixel.ARGB32.Code: self.OfType<Pixel.ARGB32>().WriteAsImageSharp<Pixel.ARGB32, Argb32>(Image => action(Image)); break;
                default: throw new NotImplementedException();
            }
        }

        public static void WriteAsImageSharp(PointerBitmap self, Action<Image> action)
        {
            void _run<TPixel>(PointerBitmap bmp) where TPixel : unmanaged, IPixel<TPixel>
            {
                bmp.WriteAsImageSharp<TPixel>(Image => action(Image));
            }

            switch (self.Info.PixelFormat.Code)
            {
                case Pixel.Luminance8.Code: _run<L8>(self); break;
                case Pixel.BGR24.Code: _run<Bgr24>(self); break;
                case Pixel.RGBA32.Code: _run<Rgba32>(self); break;
                case Pixel.BGRA32.Code: _run<Bgra32>(self); break;
                case Pixel.ARGB32.Code: _run<Argb32>(self); break;
                default: throw new NotImplementedException();
            }
        }

        public static void MutateAsImageSharp<TPixel>(SpanBitmap<TPixel> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation)
            where TPixel : unmanaged
        {
            // [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            void _run<TPixelOut>(SpanBitmap<TPixel> bmp)
                where TPixelOut : unmanaged, IPixel<TPixelOut>
            {
                bmp.WriteAsImageSharp<TPixel, TPixelOut>(img => SixLabors.ImageSharp.Processing.ProcessingExtensions.Mutate(img, operation));
            }

            if (typeof(TPixel) == typeof(Pixel.Luminance8)) { _run<L8>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.BGR565)) { _run<Bgr565>(self); }
            if (typeof(TPixel) == typeof(Pixel.BGRA4444)) { _run<Bgra4444>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.BGRA5551)) { _run<Bgra5551>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.BGR24)) { _run<Bgr24>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.RGBA32)) { _run<Rgba32>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.BGRA32)) { _run<Bgra32>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.ARGB32)) { _run<Argb32>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.RGBA128F)) { _run<RgbaVector>(self); return; }

            // fallback

            MutateAsImageSharp(self.AsTypeless(), operation);
        }

        public static void MutateAsImageSharp(SpanBitmap self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation)
        {
            void _run<TPixelIn, TPixelOut>(SpanBitmap bmp)
                where TPixelIn : unmanaged
                where TPixelOut : unmanaged, IPixel<TPixelOut>
            {
                bmp.OfType<TPixelIn>().WriteAsImageSharp<TPixelIn, TPixelOut>(img => SixLabors.ImageSharp.Processing.ProcessingExtensions.Mutate(img, operation));
            }

            switch (self.Info.PixelFormat.Code)
            {
                case Pixel.Luminance8.Code: _run<Pixel.Luminance8, L8>(self); break;
                case Pixel.BGR565.Code: _run<Pixel.BGR565, Bgr565>(self); break;
                case Pixel.BGRA4444.Code: _run<Pixel.BGRA4444, Bgra4444>(self); break;
                case Pixel.BGRA5551.Code: _run<Pixel.BGRA5551, Bgra5551>(self); break;
                case Pixel.BGR24.Code: _run<Pixel.BGR24, Bgr24>(self); break;
                case Pixel.RGBA32.Code: _run<Pixel.RGBA32, Rgba32>(self); break;
                case Pixel.BGRA32.Code: _run<Pixel.BGRA32, Bgra32>(self); break;
                case Pixel.ARGB32.Code: _run<Pixel.ARGB32, Argb32>(self); break;
                case Pixel.RGBA128F.Code: _run<Pixel.RGBA128F, RgbaVector>(self); break;
                default: throw new NotImplementedException();
            }
        }

        public static void MutateAsImageSharp(PointerBitmap self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation)
        {
            void _run<TPixel>(PointerBitmap bmp) where TPixel : unmanaged, IPixel<TPixel>
            {
                bmp.WriteAsImageSharp<TPixel>(img => SixLabors.ImageSharp.Processing.ProcessingExtensions.Mutate(img, operation));
            }

            switch (self.Info.PixelFormat.Code)
            {
                case Pixel.Luminance8.Code: _run<L8>(self); break;
                case Pixel.BGR565.Code: _run<Bgr565>(self); break;
                case Pixel.BGRA4444.Code: _run<Bgra4444>(self); break;
                case Pixel.BGRA5551.Code: _run<Bgra5551>(self); break;
                case Pixel.BGR24.Code: _run<Bgr24>(self); break;
                case Pixel.RGBA32.Code: _run<Rgba32>(self); break;
                case Pixel.BGRA32.Code: _run<Bgra32>(self); break;
                case Pixel.ARGB32.Code: _run<Argb32>(self); break;
                case Pixel.RGBA128F.Code: _run<RgbaVector>(self); break;
                default: throw new NotImplementedException();
            }
        }

        #region copy pixels

        public static void CopyPixels(Image src, SpanBitmap dst)
        {
            if (src is Image<A8> srcA8) { CopyPixels(srcA8, dst); return; }

            if (src is Image<L8> srcL8) { CopyPixels(srcL8, dst); return; }
            if (src is Image<L16> srcL16) { CopyPixels(srcL16, dst); return; }

            if (src is Image<Bgr565> srcBgr565) { CopyPixels(srcBgr565, dst); return; }
            if (src is Image<Bgra4444> srcBgra4444) { CopyPixels(srcBgra4444, dst); return; }
            if (src is Image<Bgra5551> srcBgra5551) { CopyPixels(srcBgra5551, dst); return; }

            if (src is Image<Rgb24> srcRgb24) { CopyPixels(srcRgb24, dst); return; }
            if (src is Image<Bgr24> srcBgr24) { CopyPixels(srcBgr24, dst); return; }

            if (src is Image<Rgba32> srcRgba32) { CopyPixels(srcRgba32, dst); return; }
            if (src is Image<Bgra32> srcBgra32) { CopyPixels(srcBgra32, dst); return; }
            if (src is Image<Argb32> srcArgb32) { CopyPixels(srcArgb32, dst); return; }

            if (src is Image<RgbaVector> srcRgbaVector) { CopyPixels(srcRgbaVector, dst); return; }

            throw new NotImplementedException();
        }

        public static void CopyPixels(SpanBitmap src, Image dst)
        {
            if (dst is Image<A8> dstA8) { CopyPixels(src, dstA8); return; }

            if (dst is Image<L8> dstL8) { CopyPixels(src, dstL8); return; }
            if (dst is Image<L16> dstL16) { CopyPixels(src, dstL16); return; }

            if (dst is Image<Bgr565> dstBgr565) { CopyPixels(src, dstBgr565); return; }
            if (dst is Image<Bgra4444> dstBgra4444) { CopyPixels(src, dstBgra4444); return; }
            if (dst is Image<Bgra5551> dstBgra5551) { CopyPixels(src, dstBgra5551); return; }

            if (dst is Image<Rgb24> dstRgb24) { CopyPixels(src, dstRgb24); return; }
            if (dst is Image<Bgr24> dstBgr24) { CopyPixels(src, dstBgr24); return; }

            if (dst is Image<Rgba32> dstRgba32) { CopyPixels(src, dstRgba32); return; }
            if (dst is Image<Bgra32> dstBgra32) { CopyPixels(src, dstBgra32); return; }
            if (dst is Image<Argb32> dstArgb32) { CopyPixels(src, dstArgb32); return; }

            if (dst is Image<RgbaVector> dstRgbaVector) { CopyPixels(src, dstRgbaVector); return; }

            throw new NotImplementedException();
        }

        #endregion
    }
}