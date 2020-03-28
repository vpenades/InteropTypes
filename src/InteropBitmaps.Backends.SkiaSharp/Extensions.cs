using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    public static partial class SkiaSharpToolkit
    {
        public static Adapters.SkiaSpanAdapter WithSkiaSharp(this SpanBitmap bmp)
        {
            return new Adapters.SkiaSpanAdapter(bmp);
        }

        public static Adapters.SkiaSpanAdapter WithSkiaSharp<TPixel>(this SpanBitmap<TPixel> bmp)
            where TPixel:unmanaged
        {
            return new Adapters.SkiaSpanAdapter(bmp);
        }

        public static Adapters.SkiaMemoryAdapter UsingSkiaSharp(this MemoryBitmap bmp)
        {
            return new Adapters.SkiaMemoryAdapter(bmp);
        }

    }
}
