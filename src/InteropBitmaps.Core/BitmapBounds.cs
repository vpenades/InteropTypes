using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <summary>
    /// Defines yet another rectangle structure.
    /// </summary>
    /// <remarks>
    /// Pretty much every imaging library around defines some sort of Rectangle structure:    
    /// - (GDI+) System.Drawing.Rectangle
    /// - (GDI+) System.Drawing.RectangleF
    /// - (WPF) System.Windows.Rect (WPF)
    /// - (WPF) System.Windows.Int32Rect
    /// - (ImageSharp) Sixlabors.Primitives.Rectangle
    /// - (OpenCV) OpenCvSharp.Rect
    /// - (Skia) SkiaSharp.SKRectI:
    /// - (Monogame) Microsoft.Xna.Framework.Rectangle:
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{X},{Y} {Width}x{Height}")]
    public readonly struct BitmapBounds : IEquatable<BitmapBounds>
    {
        #region constructor

        public static implicit operator BitmapBounds(in System.Drawing.Rectangle rect)
        {
            return new BitmapBounds(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static implicit operator BitmapBounds(in System.Drawing.RectangleF rect)
        {
            return System.Drawing.Rectangle.Truncate(rect);
        }

        public static implicit operator System.Drawing.Rectangle(in BitmapBounds rect)
        {
            return new System.Drawing.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static implicit operator System.Drawing.RectangleF(in BitmapBounds rect)
        {
            return new System.Drawing.RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static implicit operator BitmapBounds(in (int x, int y, int w, int h) rect)
        {
            return new BitmapBounds(rect.x, rect.y, rect.w, rect.h);
        }
        
        public BitmapBounds(int x, int y,int w,int h)
        {
            this.X = x;
            this.Y = y;
            this.Width = Math.Max(0, w);
            this.Height = Math.Max(0, h);
        }

        #endregion

        #region data

        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        public override int GetHashCode() { return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode(); }

        public static bool AreEqual(in BitmapBounds a, in BitmapBounds b)
        {
            if (a.X != b.X) return false;
            if (a.Y != b.Y) return false;
            if (a.Width != b.Width) return false;
            if (a.Height != b.Height) return false;
            return true;
        }

        public bool Equals(BitmapBounds other) { return AreEqual(this, other); }

        public override bool Equals(object obj) { return obj is BitmapBounds other ? AreEqual(this, other) : false; }

        public static bool operator ==(in BitmapBounds a, in BitmapBounds b) { return AreEqual(a, b); }

        public static bool operator !=(in BitmapBounds a, in BitmapBounds b) { return !AreEqual(a, b); }

        #endregion

        #region properties

        public int Area => Width * Height;

        public (int X, int Y) Origin => (X, Y);

        public (int Width, int Height) Size => (Width, Height);

        public int Left => this.X;

        public int Top => this.Y;

        public int Right => this.X + this.Width;

        public int Bottom => this.Y + this.Height;

        #endregion

        #region API

        public bool Contains(in BitmapBounds other)
        {
            if (other.X < this.X) return false;
            if (other.Y < this.Y) return false;

            if (other.X + other.Width > this.X + this.Width) return false;
            if (other.Y + other.Height > this.Y + this.Height) return false;

            return true;
        }

        public static BitmapBounds Clamp(in BitmapBounds value, in BitmapBounds limits)
        {
            var x = value.X;
            var y = value.Y;
            var w = value.Width;
            var h = value.Height;

            if (x < limits.X) { w -= (limits.X - x); x = limits.X; }
            if (y < limits.Y) { h -= (limits.Y - y); y = limits.Y; }

            if (x + w > limits.X + limits.Width) w -= (x + w) - (limits.X + limits.Width);
            if (y + h > limits.Y + limits.Height) h -= (y + h) - (limits.Y + limits.Height);

            if (w < 0) w = 0;
            if (h < 0) h = 0;

            return new BitmapBounds(x, y, w, h);
        }

        #endregion

        #region nested types

        public enum Anchor
        {
            Undefined
            , TopLeft, Top, TopRight
            , Left, Center, Right
            , BottomLeft, Bottom, BottomRight
        }

        #endregion
    }
}
