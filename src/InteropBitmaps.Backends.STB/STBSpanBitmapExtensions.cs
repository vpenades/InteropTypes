using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

using STBREAD = StbImageSharp;

namespace InteropTypes.Graphics.Backends
{
    public static class STBSharpExtensions
    {
        public static SpanBitmap AsSpanBitmap(this STBREAD.ImageResult image)
        {
            return _Implementation.AsSpanBitmap(image);
        }
    }
}
