using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Backends
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

        #region As MemoryBitmap

        public static MemoryBitmap.IDisposableSource UsingMemoryBitmap(this SkiaSharp.SKBitmap bmp)
        {
            return new Adapters.SkiaMemoryManager(bmp);
        }

        #endregion

    }
}
