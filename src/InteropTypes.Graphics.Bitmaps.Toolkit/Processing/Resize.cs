using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class BitmapsToolkit
    {
        public static void FitPixels(this in SpanBitmap dst, SpanBitmap src)
        {
            src = src.AsReadOnly();

            // _Implementation.FitPixelsNearest(this, src);

            Processing._ResizeBilinearImplementation.FitPixels(src, dst, (0, 1));
        }

        public static void FitPixels<TPixel>(this in SpanBitmap<TPixel> dst, SpanBitmap<TPixel> src)
            where TPixel : unmanaged
        {
            src = src.AsReadOnly();

            // _Implementation.FitPixelsNearest(this, src);

            Processing._ResizeBilinearImplementation.FitPixels(src, dst, (0, 1));
        }

        public static void FitPixels(SpanBitmap src, in SpanBitmap dst, (Single offset, Single scale) transform)
        {
            src = src.AsReadOnly();

            Processing._ResizeBilinearImplementation.FitPixels(src, dst, transform);
        }
    }
    
}
