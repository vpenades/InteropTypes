using System;
using System.Collections.Generic;
using System.Text;

using INTEROPFMT = InteropBitmaps.Pixel.Format;
using SKIACOLOR = SkiaSharp.SKColorType;
using SKIAALPHA = SkiaSharp.SKAlphaType;

namespace InteropBitmaps
{
    static partial class _Implementation
    {
        #region Skia => Interop

        public static BitmapInfo GetBitmapInfo(SkiaSharp.SKImageInfo info, int rowBytes = 0)
        {
            var fmt = ToPixelFormat((info.ColorType, info.AlphaType));
            return new BitmapInfo(info.Width, info.Height, fmt, Math.Max(info.RowBytes, rowBytes));
        }

        public static PointerBitmap WrapAsPointer(SkiaSharp.SKPixmap src)
        {
            var wrapInfo = GetBitmapInfo(src.Info, src.RowBytes);            

            var wrapBytes = src.GetPixels();
            if (wrapBytes == IntPtr.Zero) throw new ArgumentNullException();            

            return new PointerBitmap(wrapBytes, wrapInfo);
        }

        public static SpanBitmap WrapAsSpan(SkiaSharp.SKPixmap src, bool readOnly = false)
        {
            var wrapInfo = GetBitmapInfo(src.Info, src.RowBytes);

            if (readOnly) return new SpanBitmap(src.GetPixelSpan(), wrapInfo);

            var wrapBytes = src.GetPixels();
            if (wrapBytes == IntPtr.Zero) throw new ArgumentNullException();

            return new SpanBitmap(wrapBytes, wrapInfo);
        }        

        public static SpanBitmap WrapAsSpan(SkiaSharp.SKBitmap bmp, bool readOnly = false)
        {
            var wrapInfo = GetBitmapInfo(bmp.Info, bmp.RowBytes);

            if (readOnly) return new SpanBitmap(bmp.GetPixelSpan(), wrapInfo);

            var wrapBytes = bmp.GetPixels();
            if (wrapBytes == IntPtr.Zero) throw new ArgumentNullException();

            return new SpanBitmap(wrapBytes, wrapInfo);

            // should call bmp.NotifyPixelsChanged(); afterwards
        }

        #endregion

        #region Interop => Skia

        public static SkiaSharp.SKImageInfo GetSKImageInfo(BitmapInfo binfo, bool allowCompatibleFormats = false)
        {
            return TryGetPixelFormat(binfo.PixelFormat, out var color, out var alpha, allowCompatibleFormats)
                ? new SkiaSharp.SKImageInfo(binfo.Width, binfo.Height, color, alpha)
                : throw new ArgumentException(nameof(binfo));
        }

        public static SkiaSharp.SKImage WrapAsSKImage(PointerBitmap src)
        {
            var wrapInfo = GetSKImageInfo(src.Info, false);

            return SkiaSharp.SKImage.FromPixels(wrapInfo, src.Pointer, src.Info.StepByteSize);
        }

        public static SkiaSharp.SKBitmap WrapAsSKBitmap(PointerBitmap src)
        {
            var wrapInfo = GetSKImageInfo(src.Info, false);

            var wrap = new SkiaSharp.SKBitmap();
            if (!wrap.InstallPixels(wrapInfo, src.Pointer, src.Info.StepByteSize)) throw new InvalidOperationException("InstallPixels");
            return wrap;
        }        

        public static SkiaSharp.SKBitmap CloneAsSKBitmap(SpanBitmap bmp)
        {
            if (!TryGetPixelFormat(bmp.PixelFormat, out var color, out var alpha))
                throw new ArgumentException("format",nameof(bmp));
                        
            var img = new SkiaSharp.SKBitmap(bmp.Width, bmp.Height, color, alpha);

            var binfo = GetBitmapInfo(img.Info, img.RowBytes);

            var ptr = img.GetPixels();
            if (ptr == IntPtr.Zero) throw new ArgumentNullException();            

            var dst = new SpanBitmap(ptr, binfo);

            dst.SetPixels(0, 0, bmp);

            img.NotifyPixelsChanged();

            return img;
        }

        public static SkiaSharp.SKImage CloneAsSKImage(SpanBitmap bmp, (SKIACOLOR Color, SKIAALPHA Alpha)? fmtOverride = null)
        {            
            var skinfo = fmtOverride.HasValue
                ? new SkiaSharp.SKImageInfo(bmp.Width,bmp.Height,fmtOverride.Value.Color,fmtOverride.Value.Alpha)
                : GetSKImageInfo(bmp.Info, true);

            var img = SkiaSharp.SKImage.Create(skinfo);

            var pix = img.PeekPixels();

            WrapAsSpan(pix).SetPixels(0,0,bmp);

            return img;            
        }

        public static SkiaSharp.SKImage CloneAsSKImage(PointerBitmap bmp)
        {
            var skinfo = GetSKImageInfo(bmp.Info, false);

            return SkiaSharp.SKImage.FromPixelCopy(skinfo, bmp.Pointer, bmp.Info.StepByteSize);
        }       

        #endregion
    }
}
