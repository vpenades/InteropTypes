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

                for (int x = 0; x < dst.Height; ++x)
                {
                    var xx = x * src.Width / dst.Width;

                    var p = src.GetPixel(xx, yy);

                    dst.SetPixel(x, y, p);
                }
            }
        }
    }
}
