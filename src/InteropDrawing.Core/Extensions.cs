using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    public static class Extensions
    {
        public static COLOR WithAlpha(this COLOR color, int alpha)
        {
            return COLOR.FromArgb(alpha, color.R, color.G, color.B);
        }
    }
}
