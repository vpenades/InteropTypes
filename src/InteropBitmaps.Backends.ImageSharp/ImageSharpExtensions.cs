using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp.PixelFormats;

namespace SixLabors.ImageSharp
{
    public static class InteropBitmapsExtensions
    {
        public static InteropBitmaps.SpanBitmap AsSpanBitmap(this Image src)
        {
            return InteropBitmaps._Implementation.AsSpanBitmap(src);
        }

        public static InteropBitmaps.SpanBitmap<TPixel> AsSpanBitmap<TPixel>(this Image<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return InteropBitmaps._Implementation.AsSpanBitmap(src);
        }

        public static InteropBitmaps.MemoryBitmap<TPixel> ToMemoryBitmap<TPixel>(this Image<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return AsSpanBitmap(src).ToMemoryBitmap();
        }
    }
}
