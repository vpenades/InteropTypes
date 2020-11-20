using System;
using System.Collections.Generic;
using System.Text;

using XY = System.Numerics.Vector2;
using SIZE = System.Drawing.Size;
using POINT = System.Drawing.Point;
using RECTI = System.Drawing.Rectangle;
using RECTS = System.Drawing.RectangleF;

namespace InteropBitmaps
{
    /// <summary>
    /// Defines yet another rectangle structure.
    /// </summary>
    /// <remarks>
    /// Pretty much every imaging library around defines some sort of Rectangle structure:    
    /// - (GDI+) <see cref="System.Drawing.Rectangle"/> <see href="https://github.com/dotnet/runtime/blob/master/src/libraries/System.Drawing.Primitives/src/System/Drawing/Rectangle.cs"/>
    /// - (GDI+) <see cref="System.Drawing.RectangleF"/>
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

        public static BitmapBounds Ceiling(RECTS value)
        {
            unchecked
            {
                return new BitmapBounds(
                    (int)Math.Ceiling(value.X),
                    (int)Math.Ceiling(value.Y),
                    (int)Math.Ceiling(value.Width),
                    (int)Math.Ceiling(value.Height));
            }
        }
        
        public static BitmapBounds Truncate(RECTS value)
        {
            unchecked
            {
                return new BitmapBounds(
                    (int)value.X,
                    (int)value.Y,
                    (int)value.Width,
                    (int)value.Height);
            }
        }
        
        public static BitmapBounds Round(RECTS value)
        {
            unchecked
            {
                return new BitmapBounds(
                    (int)Math.Round(value.X),
                    (int)Math.Round(value.Y),
                    (int)Math.Round(value.Width),
                    (int)Math.Round(value.Height));
            }
        }

        public BitmapBounds(int x, int y,int w,int h)
        {
            this.X = x;
            this.Y = y;
            this.Width = Math.Max(0, w);
            this.Height = Math.Max(0, h);
        }

        public BitmapBounds(POINT origin, SIZE size)
        {
            this.X = origin.X;
            this.Y = origin.Y;
            this.Width = Math.Max(0, size.Width);
            this.Height = Math.Max(0, size.Height);
        }

        #endregion

        #region operators

        public static implicit operator BitmapBounds(in RECTI rect)
        {
            return new BitmapBounds(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static implicit operator BitmapBounds(in (POINT Location, SIZE Size) rect)
        {
            return new BitmapBounds(rect.Location, rect.Size);
        }

        public static implicit operator BitmapBounds(in (int x, int y, int w, int h) rect)
        {
            return new BitmapBounds(rect.x, rect.y, rect.w, rect.h);
        }

        public static implicit operator RECTI(in BitmapBounds rect)
        {
            return new RECTI(rect.X, rect.Y, rect.Width, rect.Height);
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

        /// <summary>
        /// Gets the area of this object, in pixels
        /// </summary>
        public int Area => Width * Height;

        public POINT Location => new POINT(X, Y);

        public SIZE Size => new SIZE(Width, Height);

        public int Left => this.X;

        public int Top => this.Y;

        public int Right => unchecked(this.X + this.Width);

        public int Bottom => unchecked(this.Y + this.Height);

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

        /// <summary>
        /// Gets this instance clipped by <paramref name="clip"/>.
        /// </summary>
        /// <param name="clip">The clipping <see cref="BitmapBounds"/>.</param>
        /// <returns>A new <see cref="BitmapBounds"/> that is fully contained within <paramref name="clip"/>.</returns>
        public BitmapBounds Clipped(in BitmapBounds clip) { return _Clip(this, clip); }

        /// <summary>
        /// Clips <paramref name="rect"/> by <paramref name="clip"/>.
        /// </summary>
        /// <param name="rect">The <see cref="BitmapBounds"/> to clip.</param>
        /// <param name="clip">The clipping <see cref="BitmapBounds"/>.</param>
        /// <returns>A new <see cref="BitmapBounds"/> that is fully contained within <paramref name="clip"/>.</returns>
        private static BitmapBounds _Clip(in BitmapBounds rect, in BitmapBounds clip)
        {
            var x = rect.X;
            var y = rect.Y;
            var w = rect.Width;
            var h = rect.Height;

            if (x < clip.X) { w -= (clip.X - x); x = clip.X; }
            if (y < clip.Y) { h -= (clip.Y - y); y = clip.Y; }

            if (x + w > clip.X + clip.Width) w -= (x + w) - (clip.X + clip.Width);
            if (y + h > clip.Y + clip.Height) h -= (y + h) - (clip.Y + clip.Height);

            if (w < 0) w = 0;
            if (h < 0) h = 0;

            return new BitmapBounds(x, y, w, h);
        }        

        public override string ToString()
        {
            return "{" + $"X={X},Y={Y},Width={Width},Height={Height}" + "}";
        }

        #endregion

        #region System.Drawing Static API

        public static RECTI Clip(in RECTI rect, in RECTI clip)
        {
            var x = rect.X;
            var y = rect.Y;
            var w = rect.Width;
            var h = rect.Height;

            if (x < clip.X) { w -= (clip.X - x); x = clip.X; }
            if (y < clip.Y) { h -= (clip.Y - y); y = clip.Y; }

            if (x + w > clip.X + clip.Width) w -= (x + w) - (clip.X + clip.Width);
            if (y + h > clip.Y + clip.Height) h -= (y + h) - (clip.Y + clip.Height);

            if (w < 0) w = 0;
            if (h < 0) h = 0;

            return new RECTI(x, y, w, h);
        }

        public static RECTS Clip(in RECTS rect, in RECTS clip)
        {
            var x = rect.X;
            var y = rect.Y;
            var w = rect.Width;
            var h = rect.Height;

            if (x < clip.X) { w -= (clip.X - x); x = clip.X; }
            if (y < clip.Y) { h -= (clip.Y - y); y = clip.Y; }

            if (x + w > clip.X + clip.Width) w -= (x + w) - (clip.X + clip.Width);
            if (y + h > clip.Y + clip.Height) h -= (y + h) - (clip.Y + clip.Height);

            if (w < 0) w = 0;
            if (h < 0) h = 0;

            return new RECTS(x, y, w, h);
        }

        public static RECTS Lerp(in RECTS left, in RECTS right, Single amount)
        {
            var l = XY.Lerp(new XY(left.X, left.Y), new XY(right.X, right.Y), amount);
            var s = XY.Lerp(new XY(left.Width, left.Height), new XY(right.Width, right.Height), amount);
            return new RECTS(l.X, l.Y, s.X, s.Y);
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
