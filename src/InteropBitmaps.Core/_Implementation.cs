using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    static class _Implementation
    {
        public static SpanBitmap Crop(SpanBitmap src, BitmapBounds rect)
        {
            rect = BitmapBounds.Clamp(rect, src.bounds);

            if (rect.Width <= 0 || rect.Height <= 0) return default;

            return src.Slice(rect);
        }

        public static void CopyPixels(SpanBitmap dst, int dstX, int dstY, SpanBitmap src)
        {
            var dstCrop = Crop(dst, (+dstX, +dstY, src.Width, src.Height));
            var srcCrop = Crop(src, (-dstX, -dstY, dst.Width, dst.Height));

            System.Diagnostics.Debug.Assert(dstCrop.Width == srcCrop.Width);
            System.Diagnostics.Debug.Assert(dstCrop.Height == srcCrop.Height);

            if (dstCrop.Width <= 0 || dstCrop.Height <= 0) return;

            if (dstCrop.PixelFormat == srcCrop.PixelFormat)
            {
                for (int y = 0; y < dstCrop.Height; ++y)
                {
                    var srcRow = srcCrop.GetBytesScanline(y);
                    var dstRow = dstCrop.UseBytesScanline(y);

                    System.Diagnostics.Debug.Assert(srcRow.Length == srcRow.Length);

                    srcRow.CopyTo(dstRow);
                }
            }
            else
            {
                var srcConverter = _PixelConverters.GetConverter(srcCrop.PixelFormat);
                var dstConverter = _PixelConverters.GetConverter(dstCrop.PixelFormat);

                Span<_PixelBGRA32> tmp = stackalloc _PixelBGRA32[dstCrop.Width];

                for (int y = 0; y < dstCrop.Height; ++y)
                {
                    var srcRow = srcCrop.GetBytesScanline(y);
                    var dstRow = dstCrop.UseBytesScanline(y);

                    srcConverter.ConvertFrom(tmp, srcRow);
                    dstConverter.ConvertTo(dstRow, tmp);
                }
            }
        }
    }
}
