using System;
using System.Collections.Generic;
using System.Text;

namespace InteropDrawing
{
    /// <summary>
    /// A graphic resource defined as a bitmap source and a region within that bitmap that can be
    /// used to display small bitmaps or "sprites" using <see cref="ISpritesDrawing2D"/>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Source} ({Left}, {Top}) ({Width}, {Height}) Scale:{Scale}")]
    public sealed class SpriteAsset
    {
        #region lifecycle

        public static IEnumerable<SpriteAsset> CreateGrid(string source, Point2 size, Point2 pivot, int count, int stride)
        {
            for (int idx = 0; idx < count; ++idx)
            {
                var idx_x = idx % stride;
                var idx_y = idx / stride;

                yield return new SpriteAsset(source, (idx_x * size.X, idx_y * size.Y), (size.X, size.Y), pivot);
            }
        }        

        public static SpriteAsset CreateFromBitmap(string source, Point2 size, Point2 pivot)
        {
            return new SpriteAsset(source, (0, 0), size, pivot);
        }        

        public SpriteAsset(String source, Point2 origin, Point2 size, Point2 pivot)
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
        /// The source asset that contains the <see cref="SpriteAsset"/>; it can be an image path, a resource, etc
        /// </summary>
        /// <remarks>
        /// Source should be an object
        /// if source is System.IO.FileInfo then it should load the file
        /// string should be reserved to lambdas
        /// </remarks>
        public string Source { get; private set; }

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

        public bool IsVisible
        {
            get
            {
                if (this.Scale == 0) return false;
                if (string.IsNullOrWhiteSpace(Source)) return false;
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
