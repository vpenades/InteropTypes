using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    public static class SkiaSharpToolkit
    {
        public static Adapters.SkiaAdapter WithSkiaSharp(this SpanBitmap bmp) { return new Adapters.SkiaAdapter(bmp); }

    }
}
