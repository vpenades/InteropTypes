using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        public static TPixel GetColor<TPixel>(System.Drawing.Color color)
            where TPixel : unmanaged
        {
            var t = default(TPixel);
            new BGRA32(color).CopyTo(ref t);
            return t;
        }
    }
}
