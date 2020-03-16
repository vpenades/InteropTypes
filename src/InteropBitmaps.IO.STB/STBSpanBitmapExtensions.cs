using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    public static class STBSharpExtensions
    {
        public static SpanBitmap AsSpanBitmap(this StbImageLib.ImageResult image)
        {
            return _Implementation.AsSpanBitmap(image);
        }
    }
}
