using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    public static partial class SkiaSharpToolkit
    {
        public static Adapters.SkiaAdapter WithSkiaSharp(this SpanBitmap bmp)
        {
            return new Adapters.SkiaAdapter(bmp);
        }

        public static Adapters.SkiaAdapter WithSkiaSharp<TPixel>(this SpanBitmap<TPixel> bmp)
            where TPixel:unmanaged
        {
            return new Adapters.SkiaAdapter(bmp);
        }

    }
}
