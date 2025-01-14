using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Backends
{
    public static class MagicScalerToolkit
    {
        public static void Rescale(SpanBitmap srcBitmap, SpanBitmap dstBitmap)
        {
            _Implementation.RescaleTo(srcBitmap, dstBitmap);
        }

        public static void Rescale(MemoryBitmap srcBitmap, SpanBitmap dstBitmap)
        {
            _Implementation.RescaleTo(srcBitmap, dstBitmap);
        }

        public static MemoryBitmap Rescale(MemoryBitmap srcBitmap, int dstWidth, int dstHeight)
        {
            return  _Implementation.Rescale(srcBitmap, dstWidth, dstHeight);
        }
    }
}
