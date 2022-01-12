using System;
using System.Collections.Generic;
using System.Text;

using XY = System.Numerics.Vector2;

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
            this._Source = source;           

            this._Pivot = pivot.ToNumerics();
            
            this._SourceUVMin = origin.ToNumerics();
            this._SourceUVMax = this._SourceUVMin + size.ToNumerics();

            this._CalculateMatrices();
        }

        public SpriteAsset() { }

        public SpriteAsset WithPivot(int x, int y)
        {
            _Pivot = new XY(x, y);
            this._CalculateMatrices();
            return this;
        }

        public SpriteAsset WithScale(float scale)
        {
            _Scale = scale;
            this._CalculateMatrices();
            return this;
        }

        public void CopyTo(SpriteAsset other, XY pivotOffset)
        {
            other._Source = this.Source;
            other._SourceUVMin = this._SourceUVMin;
            other._SourceUVMax = this._SourceUVMax;
            other._Scale = this.Scale;
            other._Pivot = this.Pivot + pivotOffset; // should multiply by this.Scale ??
            other._CalculateMatrices();
        }        

        #endregion

        #region data

        private object _Source;
        private XY _SourceUVMin;
        private XY _SourceUVMax;

        private XY _Pivot;

        private float _Scale = 1f;

        private readonly System.Numerics.Matrix3x2[] _Transforms = new System.Numerics.Matrix3x2[4];        

        #endregion

        #region properties

        /// <summary>
        /// device dependant reference that points, or is a bitmap.
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
        public Object Source => _Source;

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
        public XY Pivot => _Pivot;

        /// <summary>
        /// Gets the rendering scale of the sprite.
        /// </summary>
        public float Scale => _Scale;

        /// <summary>
        /// Gets the Left pixel coordinate within the <see cref="Source"/> asset.
        /// </summary>
        public float Left => _SourceUVMin.X;

        /// <summary>
        /// Gets the top pixel coordinate within the <see cref="Source"/> asset.
        /// </summary>
        public float Top => _SourceUVMin.Y;

        /// <summary>
        /// Gets the width of the sprite, in pixels.
        /// </summary>
        public float Width => _SourceUVMax.X - _SourceUVMin.X;

        /// <summary>
        /// Gets the Height of the sprite, in pixels.
        /// </summary>
        public float Height => _SourceUVMax.Y - _SourceUVMin.Y;

        public XY UV0 => _SourceUVMin;
        public XY UV1 => new XY(_SourceUVMax.X, _SourceUVMin.Y);
        public XY UV2 => _SourceUVMax;
        public XY UV3 => new XY(_SourceUVMin.X, _SourceUVMax.Y);

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

        private void _CalculateMatrices()
        {
            _Transforms[0] = _GetSpriteMatrix(false, false);
            _Transforms[1] = _GetSpriteMatrix(false, true);
            _Transforms[2] = _GetSpriteMatrix(true, false);
            _Transforms[3] = _GetSpriteMatrix(true, true);

        }

        private System.Numerics.Matrix3x2 _GetSpriteMatrix(bool hflip, bool vflip)
        {
            var sx = hflip ? -Scale : +Scale;
            var sy = vflip ? -Scale : +Scale;

            var final = System.Numerics.Matrix3x2.CreateScale(Width, Height);
            final *= System.Numerics.Matrix3x2.CreateTranslation(-Pivot);
            final *= System.Numerics.Matrix3x2.CreateScale(sx, sy);
            return final;
        }

        public System.Numerics.Matrix3x2 GetSpriteMatrix(bool hflip, bool vflip)
        {
            var index = (hflip ? 2 : 0) | (vflip ? 1 : 0);

            return _Transforms[index];
        }

        public void PrependTransform(ref System.Numerics.Matrix3x2 xform, bool hflip, bool vflip)
        {
            var index = (hflip ? 2 : 0) | (vflip ? 1 : 0);

            xform = _Transforms[index] * xform;
        }

        #endregion
    }
}
