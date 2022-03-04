using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Backends;

using GDIFMT = System.Drawing.Imaging.PixelFormat;
using INTEROPFMT = InteropTypes.Graphics.PixelFormat;

using GDIICON = System.Drawing.Icon;
using GDIIMAGE = System.Drawing.Image;
using GDIBITMAP = System.Drawing.Bitmap;
using GDIPTR = System.Drawing.Imaging.BitmapData;


namespace InteropTypes.Graphics
{
    static partial class _Implementation
    {
        #region API

        public static GDIBITMAP WrapOrCloneAsGDIBitmap(PointerBitmap src)
        {
            return _TryWrap(src, out var dst, out _)
                ? dst
                : CloneAsGDIBitmap(src);
        }

        public static GDIBITMAP WrapAsGDIBitmap(PointerBitmap src)
        {
            return _TryWrap(src, out var dst, out var err)
                ? dst
                : throw new ArgumentException(err, nameof(src));
        }

        private static bool _TryWrap(PointerBitmap src, out GDIBITMAP dst, out string errorMsg)
        {
            dst = null;

            if (src.IsEmpty) { errorMsg = "Source is Empty"; return false; }

            if (!TryGetExactPixelFormat(src.Info.PixelFormat, out var fmt))
            {
                errorMsg = $"Invalid format {src.Info.PixelFormat}";
                return false;
            }
            
            if ((src.Info.StepByteSize & 3) != 0)
            {
                errorMsg = $"Stride Must be multiple of 4 but found {src.Info.StepByteSize}";
                return false;
            }

            errorMsg = null;
            dst = new GDIBITMAP(src.Info.Width, src.Info.Height, src.Info.StepByteSize, fmt, src.Pointer);            
            return true;
        }        

        

        public static GDIBITMAP CloneAsGDIBitmap(SpanBitmap src, GDIFMT? fmtOverride = null)
        {
            var fmt = fmtOverride ?? GetCompatiblePixelFormat(src.PixelFormat);

            var dst = new GDIBITMAP(src.Width, src.Height, fmt);

            dst.SetPixels(0, 0, src);

            return dst;
        }

        public static MemoryBitmap CloneAsMemoryBitmap(GDIIMAGE img, INTEROPFMT? fmtOverride = null)
        {
            using (var bmp = new GDIBITMAP(img))
            {
                return CloneAsMemoryBitmap(bmp, fmtOverride);
            }
        }

        public static MemoryBitmap CloneAsMemoryBitmap(GDIBITMAP bmp, INTEROPFMT? fmtOverride = null)
        {
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            GDIPTR bits = null;

            try
            {
                bits = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

                var span = bits.AsSpanBitmap();

                return fmtOverride.HasValue ? span.ToMemoryBitmap(fmtOverride.Value) : span.ToMemoryBitmap();
            }
            finally
            {
                if (bits != null) bmp.UnlockBits(bits);
            }
        }

        public static bool Reshape(ref MemoryBitmap dst, GDIBITMAP src, INTEROPFMT? fmtOverride = null)
        {
            if (src == null) { dst = default; return true; }

            BitmapInfo binfo;

            if (fmtOverride.HasValue)
            {
                binfo = new BitmapInfo(src.Width, src.Height, fmtOverride.Value);
            }
            else if (TryGetExactPixelFormat(src.PixelFormat, out var fmt))
            {
                binfo = new BitmapInfo(src.Width, src.Height, fmt);
            }
            else
            {
                throw new ArgumentException(nameof(src));
            }

            return MemoryBitmap.Reshape(ref dst, binfo);
        }

        public static bool Reshape(ref MemoryBitmap dst, GDIPTR src, INTEROPFMT? fmtOverride = null)
        {
            if (src == null) { dst = default; return true; }

            BitmapInfo binfo;

            if (fmtOverride.HasValue)
            {
                binfo = new BitmapInfo(src.Width, src.Height, fmtOverride.Value);
            }
            else if (TryGetExactPixelFormat(src.PixelFormat, out var fmt))
            {
                binfo = new BitmapInfo(src.Width, src.Height, fmt);
            }
            else
            {
                throw new ArgumentException(nameof(src));
            }

            return MemoryBitmap.Reshape(ref dst, binfo);
        }

        public static void Mutate(GDIBITMAP bmp, Action<PointerBitmap> action)
        {
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            GDIPTR bits = null;

            try
            {
                bits = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

                action(bits.AsPointerBitmap());
            }
            finally
            {
                bmp.UnlockBits(bits);
            }
        }
        
        public static void TransferSpan(SpanBitmap src, GDIBITMAP dst, SpanBitmap.Action2 action)
        {
            src = src.AsReadOnly();

            var rect = new Rectangle(0, 0, dst.Width, dst.Height);

            GDIPTR dstBits = null;

            try
            {
                dstBits = dst.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, dst.PixelFormat);

                var dstSpan = dstBits
                    .AsPointerBitmap()
                    .AsSpanBitmap();

                action(src, dstSpan);
            }
            finally
            {
                if (dstBits != null) dst.UnlockBits(dstBits);
            }
        }

        public static void TransferSpan(GDIBITMAP src, SpanBitmap dst, SpanBitmap.Action2 action)
        {
            var rect = new Rectangle(0, 0, dst.Width, dst.Height);

            GDIPTR srcBits = null;

            try
            {
                srcBits = src.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, src.PixelFormat);

                var srcSpan = srcBits
                    .AsPointerBitmap()
                    .AsSpanBitmap()
                    .AsReadOnly();

                action(srcSpan, dst);
            }
            finally
            {
                if (srcBits != null) src.UnlockBits(srcBits);
            }
        }

        #endregion
    }
}
