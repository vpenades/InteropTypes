using System;
using System.Collections.Generic;
using System.Text;

namespace StbImageLib
{
    public static class STBSpanBitmapExtensions
    {
        public static InteropBitmaps.SpanBitmap AsSpanBitmap(this ImageResult image)
        {
            return InteropBitmaps._Implementation.AsSpanBitmap(image);
        }
    }
}
