using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial class Pixel
    {
        public static TPixel GetColor<TPixel>(System.Drawing.Color color)
            where TPixel : unmanaged, IPixelFactory<BGRA32, TPixel>
        {
            return default(TPixel).From(new BGRA32(color));
        }
    }
}
