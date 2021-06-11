using System;
using System.Collections.Generic;
using System.Text;

namespace InteropDrawing
{
    /// <summary>
    /// Represents a graphic resource defined as a bitmap source and a region within that bitmap.
    /// </summary>
    /// <remarks>
    /// <see cref="SpriteAsset"/> is part of <see cref="SpriteStyle"/>, which can be used with<br/>
    /// <see cref="IDrawing2D.DrawSprite(in System.Numerics.Matrix3x2, in SpriteStyle)"/>.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Source} ({Left}, {Top}) ({Width}, {Height}) Scale:{Scale}")]
    public sealed class SpriteAsset
    {
        #region lifecycle

        public static IEnumerable<SpriteAsset> CreateGrid(Object source, Point2 size, Point2 pivot, int count, int stride)
        {
            for (int idx = 0; idx < count; ++idx)
            {
                var idx_x = idx % stride;
                var idx_y = idx / stride;

                yield return new SpriteAsset(source, (idx_x * size.X, idx_y * size.Y), (size.X, size.Y), pivot);
            }
        }        

        public static SpriteAsset CreateFromBitmap(Object source, Point2 size, Point2 pivot)
        {
            return new SpriteAsset(source, (0, 0), size, pivot);
        }        

        public SpriteAsset(Object source, Point2 origin, Point2 size, Point2 pivot)
        {
            this.Source = source;
            this.Left = (int)origin.X;
            this.Top = (int)origin.Y;            
            this.Width = (int)size.X;
            this.Height = (int)size.Y;
            this.Pivot = pivot.ToNumerics();
        }

        public SpriteAsset() { }

        public SpriteAsset WithPivot(int x, int y)
        {
            Pivot = new System.Numerics.Vector2(x, y);
            return this;
        }

        public SpriteAsset WithScale(float scale)
        {
            Scale = scale;
            return this;
        }

        #endregion

        #region data

        /// <summary>
        /// Is, references, or points to the actual bitmap.
        /// </summary>
        /// <remarks>
        /// This property can be cast to different data types depending on the context:
        /// <para>
        /// If it's a <see cref="String"/> or a <see cref="System.IO.FileInfo"/> it can point
        /// to an image in the file system.
        /// </para>
        /// <para>
        /// It can also be a known bitmap or texture object, in which case it should be used
        /// directly as the bitmap source.
        /// </para>
        /// </remarks>
        public Object Source { get; private set; }

        /// <summary>
        /// Gets the Left pixel coordinate within the <see cref="Source"/> asset.
        /// </summary>
        public int Left { get; private set; }

        /// <summary>
        /// Gets the top pixel coordinate within the <see cref="Source"/> asset.
        /// </summary>
        public int Top { get; private set; }        

        /// <summary>
        /// Gets the width of the sprite, in pixels.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the Height of the sprite, in pixels.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the rendering scale of the sprite.
        /// </summary>
        public float Scale { get; private set; } = 1;

        /// <summary>
        /// Gets the coordinates of the center of the sprite, in pixels, relative to <see cref="Top"/> and <see cref="Left"/>.
        /// </summary>
        /// <example>
        /// We could define a sprite of size (20,20) with the rotation pivot located at its center like this:
        ///     Top: 33
        ///     Left: 55
        ///     Width: 20
        ///     Height: 20
        ///     Pivot: (10,10)        
        /// </example>
        public System.Numerics.Vector2 Pivot { get; private set; }

        #endregion

        #region properties

        /// <summary>
        /// Gets a value indicating whether this asset can be rendered.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                if (this.Source == null) return false;
                if (this.Scale == 0) return false;                
                if (this.Width == 0 || this.Height == 0) return false;
                return true;
            }
        }

        #endregion

        #region API

        public void CopyTo(SpriteAsset other, System.Numerics.Vector2 pivotOffset)
        {
            other.Source = this.Source;
            other.Left = this.Left;
            other.Top = this.Top;            
            other.Width = this.Width;
            other.Height = this.Height;
            other.Scale = this.Scale;
            other.Pivot = this.Pivot + pivotOffset; // should multiply by this.Scale ??
        }

        #endregion
    }
}
