using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    static class _Implementation
    {
        public static SpanBitmap Crop(SpanBitmap src, BitmapBounds rect)
        {
            rect = rect.Clipped(src.Bounds);
            if (rect.Width <= 0 || rect.Height <= 0) return default;
            return src.Slice(rect);
        }

        public static SpanBitmap<TPixel> Crop<TPixel>(SpanBitmap<TPixel> src, BitmapBounds rect)
            where TPixel : unmanaged
        {
            rect = rect.Clipped(src.Bounds);
            if (rect.Width <= 0 || rect.Height <= 0) return default;
            return src.Slice(rect);
        }

        public static void CopyPixels(SpanBitmap dst, int dstX, int dstY, SpanBitmap src)
        {
            var dstCrop = Crop(dst, (+dstX, +dstY, src.Width, src.Height));
            var srcCrop = Crop(src, (-dstX, -dstY, dst.Width, dst.Height));            

            System.Diagnostics.Debug.Assert(dstCrop.Width == srcCrop.Width);
            System.Diagnostics.Debug.Assert(dstCrop.Height == srcCrop.Height);

            if (dstCrop.Width <= 0 || dstCrop.Height <= 0) return; // nothing to copy

            System.Diagnostics.Debug.Assert(srcCrop.StepByteSize == src.StepByteSize);
            System.Diagnostics.Debug.Assert(dstCrop.StepByteSize == dst.StepByteSize);
            
            if (dstCrop.PixelFormat == srcCrop.PixelFormat)
            // no conversion required
            {
                for (int y = 0; y < dstCrop.Height; ++y)
                {
                    var srcRow = srcCrop.GetScanlineBytes(y);
                    var dstRow = dstCrop.UseScanlineBytes(y);

                    System.Diagnostics.Debug.Assert(srcRow.Length == dstRow.Length);

                    srcRow.CopyTo(dstRow);
                }                
            }            
            
            else
            // conversion required
            {
                var converter = Pixel.GetByteCopyConverter(srcCrop.PixelFormat, dstCrop.PixelFormat);

                for (int y = 0; y < dstCrop.Height; ++y)
                {
                    var srcRow = srcCrop.GetScanlineBytes(y);
                    var dstRow = dstCrop.UseScanlineBytes(y);
                    converter(srcRow, dstRow);
                }                
            }
        }

        public static void ApplyPixels<TSrcPixel, TDstPixel>(SpanBitmap<TDstPixel> dst, int dstX, int dstY, SpanBitmap<TSrcPixel> src, Func<TDstPixel, TSrcPixel, TDstPixel> pixelFunc)
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged
        {
            var dstCrop = Crop(dst, (+dstX, +dstY, src.Width, src.Height));
            var srcCrop = Crop(src, (-dstX, -dstY, dst.Width, dst.Height));

            System.Diagnostics.Debug.Assert(dstCrop.Width == srcCrop.Width);
            System.Diagnostics.Debug.Assert(dstCrop.Height == srcCrop.Height);

            if (dstCrop.Width <= 0 || dstCrop.Height <= 0) return;

            for (int y = 0; y < dstCrop.Height; ++y)
            {
                var srcRow = srcCrop.UseScanlinePixels(y);
                var dstRow = dstCrop.UseScanlinePixels(y);

                System.Diagnostics.Debug.Assert(srcRow.Length == srcRow.Length);

                for (int x=0; x < dstRow.Length; ++x)
                {
                    var d = dstRow[x];
                    dstRow[x] = pixelFunc(d, srcRow[x]);
                }                
            }
        }        
    }
}
