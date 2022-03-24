using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class BitmapsToolkit
    {
        public static void FitPixels(this SpanBitmap dst, SpanBitmap src)
        {
            // _Implementation.FitPixelsNearest(this, src);

            Processing._ResizeBilinearImplementation.FitPixels(src, dst, (0, 1));
        }

        public static void FitPixels<TPixel>(this SpanBitmap<TPixel> dst, SpanBitmap<TPixel> src)
            where TPixel : unmanaged
        {
            // _Implementation.FitPixelsNearest(this, src);

            Processing._ResizeBilinearImplementation.FitPixels(src, dst, (0, 1));
        }

        public static void FitPixels(SpanBitmap src, SpanBitmap dst, (Single offset, Single scale) transform)
        {
            src = src.AsReadOnly();

            Processing._ResizeBilinearImplementation.FitPixels(src, dst, transform);
        }
    }
    
}
