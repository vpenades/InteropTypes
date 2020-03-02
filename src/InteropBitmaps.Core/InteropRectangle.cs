using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <summary>
    /// Defines a rectangle.
    /// </summary>
    /// <remarks>
    /// The name of this structure is long and ugly on purpose, so it does not conflict
    /// with many other Rectangle definitions in other libraries:
    /// - System.Drawing.Rectangle
    /// - Sixlabors.Primitives.Rectangle
    /// </remarks>
    public readonly struct InteropRectangle
    {
        #region constructor

        public static implicit operator InteropRectangle(in (int x, int y, int w, int h) rect)
        {
            return new InteropRectangle(rect.x, rect.y, rect.w, rect.h);
        }
        
        public InteropRectangle(int x, int y,int w,int h)
        {
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
        }

        #endregion

        #region data

        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        #endregion

        #region API

        public bool Contains(in InteropRectangle other)
        {
            if (other.X < this.X) return false;
            if (other.Y < this.Y) return false;

            if (other.X + other.Width > this.X + this.Width) return false;
            if (other.Y + other.Height > this.Y + this.Height) return false;

            return true;
        }

        public static InteropRectangle Clamp(in InteropRectangle value, in InteropRectangle clamp)
        {
            var x = value.X;
            var y = value.Y;
            var w = value.Width;
            var h = value.Height;

            if (x < clamp.X) { w -= (clamp.X - x); x = clamp.X; }
            if (y < clamp.Y) { h -= (clamp.Y - y); y = clamp.Y; }

            if (x + w > clamp.X + clamp.Width) w -= (x + w) - (clamp.X + clamp.Width);
            if (y + h > clamp.Y + clamp.Height) h -= (y + h) - (clamp.Y + clamp.Height);

            w = Math.Abs(w);
            h = Math.Abs(h);

            return new InteropRectangle(x, y, w, h);
        }

        #endregion
    }    
}
