using System;
using System.Collections.Generic;
using System.Text;

using INTEROPFMT = InteropBitmaps.PixelFormat;
using SKIACOLOR = SkiaSharp.SKColorType;
using SKIAALPHA = SkiaSharp.SKAlphaType;

namespace InteropBitmaps
{
    static class _Implementation
    {
        #region pixel format

        public static (SKIACOLOR Color, SKIAALPHA Alpha) ToSkia(PixelFormat fmt)
        {
            switch (fmt)
            {
                case INTEROPFMT.Packed.GRAY8: return (SKIACOLOR.Gray8, SKIAALPHA.Opaque);

                case INTEROPFMT.Packed.BGR565: return (SKIACOLOR.Rgb565, SKIAALPHA.Opaque);

                case INTEROPFMT.Packed.RGBA32: return (SKIACOLOR.Rgba8888, SKIAALPHA.Unpremul);
                case INTEROPFMT.Packed.BGRA32: return (SKIACOLOR.Bgra8888, SKIAALPHA.Unpremul);
            }

            throw new NotImplementedException();
        }

        private static INTEROPFMT ToUnpremulInterop(SKIACOLOR color)
        {
            switch (color)
            {
                case SKIACOLOR.Gray8: return INTEROPFMT.Standard.GRAY8;
                case SKIACOLOR.Rgba8888: return INTEROPFMT.Standard.RGBA32;
                case SKIACOLOR.Bgra8888: return INTEROPFMT.Standard.BGRA32;
            }
            throw new NotImplementedException();
        }

        public static INTEROPFMT ToInterop(SKIACOLOR color, SKIAALPHA alpha)
        {
            switch(alpha)
            {
                case SKIAALPHA.Opaque: return ToUnpremulInterop(color);
                case SKIAALPHA.Unpremul: return ToUnpremulInterop(color);
            }
            throw new NotImplementedException();
        }

        #endregion


        public static BitmapInfo ToBitmapInfo(SkiaSharp.SKImageInfo info)
        {
            var fmt = ToInterop(info.ColorType, info.AlphaType);

            return new BitmapInfo(info.Width, info.Height, fmt, info.RowBytes);
        }

        public static SkiaSharp.SKImageInfo ToSkia(BitmapInfo binfo)
        {
            var (color, alpha) = ToSkia(binfo.PixelFormat);

            return new SkiaSharp.SKImageInfo(binfo.Width, binfo.Height, color, alpha);
        }

        public static PointerBitmap AsPointerBitmap(SkiaSharp.SKPixmap map)
        {
            var ptr = map.GetPixels();
            if (ptr == IntPtr.Zero) throw new ArgumentNullException();

            var binfo = ToBitmapInfo(map.Info);

            return new PointerBitmap(ptr, binfo);
        }

        public static SpanBitmap AsSpanBitmap(SkiaSharp.SKPixmap map)
        {
            var ptr = map.GetPixels();
            if (ptr == IntPtr.Zero) throw new ArgumentNullException();

            var binfo = ToBitmapInfo(map.Info);

            return new SpanBitmap(ptr, binfo);
        }

        public static SpanBitmap AsSpanBitmap(SkiaSharp.SKBitmap bmp)
        {
            var ptr = bmp.GetPixels();
            if (ptr == IntPtr.Zero) throw new ArgumentNullException();

            var binfo = ToBitmapInfo(bmp.Info);

            // bmp.NotifyPixelsChanged();

            return new SpanBitmap(ptr, binfo);
        }

       

        

        public static SkiaSharp.SKBitmap ToSKBitmap(SpanBitmap bmp)
        {
            var (color, alpha) = ToSkia(bmp.PixelFormat);

            var img = new SkiaSharp.SKBitmap(bmp.Width, bmp.Height, color, alpha);

            var binfo = ToBitmapInfo(img.Info);

            var ptr = img.GetPixels();
            if (ptr == IntPtr.Zero) throw new ArgumentNullException();            

            var dst = new SpanBitmap(ptr, binfo);

            dst.SetPixels(0, 0, bmp);

            img.NotifyPixelsChanged();

            return img;
        }

        public static SkiaSharp.SKImage ToSKImage(SpanBitmap bmp)
        {
            var (color, alpha) = ToSkia(bmp.PixelFormat);

            var info = new SkiaSharp.SKImageInfo(bmp.Width, bmp.Height, color, alpha);

            return SkiaSharp.SKImage.FromPixelCopy(info, bmp.ReadableSpan, bmp.ScanlineSize);
        }

        public static SkiaSharp.SKImage ToSKImage(PointerBitmap bmp)
        {
            var (color, alpha) = ToSkia(bmp.Info.PixelFormat);

            var info = new SkiaSharp.SKImageInfo(bmp.Info.Width, bmp.Info.Height, color, alpha);

            return SkiaSharp.SKImage.FromPixelCopy(info, bmp.Pointer, bmp.Info.ScanlineSize);
        }

        public static SkiaSharp.SKImage AsSKImage(PointerBitmap bmp)
        {
            var (color, alpha) = ToSkia(bmp.Info.PixelFormat);

            var info = new SkiaSharp.SKImageInfo(bmp.Info.Width, bmp.Info.Height, color, alpha);

            return SkiaSharp.SKImage.FromPixels(info, bmp.Pointer, bmp.Info.ScanlineSize);
        }

    }
}
