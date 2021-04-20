using System;
using System.Collections.Generic;
using System.Text;

using STBREAD = StbImageSharp;

namespace InteropBitmaps
{
    public static class STBSharpExtensions
    {
        public static SpanBitmap AsSpanBitmap(this STBREAD.ImageResult image)
        {
            return _Implementation.AsSpanBitmap(image);
        }
    }
}
