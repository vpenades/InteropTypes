using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    using Diagnostics;
    
    static class _ResizeNearestImplementation
    {
        public static void FitPixelsNearest<TPixel>(SpanBitmap<TPixel> dst, SpanBitmap<TPixel> src)
            where TPixel : unmanaged
        {
            // TODO: cannot run in parallel because src & dst cannot be passed to lambdas
            // we would need to pin and use PointerBitmaps            

            for (int y = 0; y < dst.Height; ++y)
            {
                var yy = y * src.Height / dst.Height;                
                var srcRow = src.GetScanlinePixels(yy);
                var dstRow = dst.UseScanlinePixels(y);

                for (int x = 0; x < dstRow.Length; ++x)
                {
                    dstRow[x] = srcRow[x * src.Width / dst.Width];
                }
            }
        }

        public static void FitPixelsNearest<TSrcPixel, TdstPixel>(SpanBitmap<TdstPixel> dst, SpanBitmap<TSrcPixel> src)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
            where TdstPixel : unmanaged
        {
            // TODO: cannot run in parallel because src & dst cannot be passed to lambdas
            // we would need to pin and use PointerBitmaps            

            for (int y = 0; y < dst.Height; ++y)
            {
                var yy = y * src.Height / dst.Height;
                var srcRow = src.GetScanlinePixels(yy);
                var dstRow = dst.UseScanlinePixels(y);

                for (int x = 0; x < dstRow.Length; ++x)
                {
                    dstRow[x] = srcRow[x * src.Width / dst.Width].To<TdstPixel>();
                }
            }
        }
    }
}
