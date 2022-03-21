using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        public static TPixel GetColor<TPixel>(System.Drawing.Color color)
            where TPixel : unmanaged, IValueSetter<BGRA32>
        {
            var t = default(TPixel);
            t.SetValue(new BGRA32(color));
            return t;
        }
    }
}
