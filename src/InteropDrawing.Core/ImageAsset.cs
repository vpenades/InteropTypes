using System;
using System.Collections.Generic;
using System.Text;

using XY = System.Numerics.Vector2;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a graphic resource defined as a bitmap source and a region within that bitmap.
    /// </summary>
    /// <remarks>
    /// <see cref="ImageAsset"/> is part of <see cref="ImageStyle"/>, which can be used by<br/>
    /// <see cref="IPrimitiveCanvas2D.DrawImage(in System.Numerics.Matrix3x2, ImageStyle)"/>.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Source} ({Left}, {Top}) ({Width}, {Height}) Scale:{Scale}")]
    public class ImageAsset
    {
        #region diagnostics

        private string ToDebuggerDisplay()
        {
            if (_Source == null) return "Empty";

            var tmp = _Source.ToString();

            tmp += $" O:{_SourceUVMin} WH:{_SourceUVMax - _SourceUVMin}";

            if (_Pivot != XY.Zero) tmp += $" P:{_Pivot}";
            if (_Scale != XY.One) tmp += $" S:{_Scale}";

            return tmp;
        }

        #endregion

        #region lifecycle

        public static IEnumerable<ImageAsset> CreateGrid(object source, Point2 size, Point2 pivot, int count, int stride)
        {
            return CreateGrid(source, size, pivot, false, count, stride);
        }

        public static IEnumerable<ImageAsset> CreateGrid(object source, Point2 size, Point2 pivot, bool pivotPrecedence, int count, int stride)
        {
            for (int idx = 0; idx < count; ++idx)
            {
                var idx_x = idx % stride;
                var idx_y = idx / stride;

                yield return new ImageAsset(source, (idx_x * size.X, idx_y * size.Y), (size.X, size.Y), pivot, pivotPrecedence);
            }
        }

        public static ImageAsset CreateFromBitmap(object source, Point2 bitmapSize, Point2 pivot, bool pivotPrecedence = false)
        {
            return new ImageAsset(source, (0, 0), bitmapSize, pivot, pivotPrecedence);
        }

        public static ImageAsset Create(object source, Point2 origin, Point2 size, Point2 pivot, bool pivotPrecedence = false, bool hflip = false, bool vflip = false)
        {
            return new ImageAsset(source, origin, size, pivot, pivotPrecedence, hflip, vflip);
        }

        public ImageAsset(object source, Point2 origin, Point2 size, Point2 pivot, bool pivotPrecedence = false, bool hflip = false, bool vflip = false)
        {
            _Source = source;
            _SourceUVMin = origin.ToNumerics();
            _SourceUVMax = _SourceUVMin + size.ToNumerics();

            _Pivot = pivot.ToNumerics();
            _PivotPrecedence = pivotPrecedence;

            _OrientationMask = ImageStyle.Orientation.None;
            if (hflip) _OrientationMask |= ImageStyle.Orientation.FlipHorizontal;
            if (vflip) _OrientationMask |= ImageStyle.Orientation.FlipVertical;

            _CalculateMatrices();
        }

        public ImageAsset() { }

        public ImageAsset WithPivot(int x, int y)
        {
            _Pivot = new XY(x, y);
            _CalculateMatrices();
            return this;
        }

        public ImageAsset WithScale(float scale)
        {
            _Scale = new XY(scale);
            _CalculateMatrices();
            return this;
        }

        public ImageAsset WithFlags(bool hflip, bool vflip)
        {
            _OrientationMask = ImageStyle.Orientation.None;
            if (hflip) _OrientationMask |= ImageStyle.Orientation.FlipHorizontal;
            if (vflip) _OrientationMask |= ImageStyle.Orientation.FlipVertical;

            _CalculateMatrices();
            return this;
        }

        /// <summary>
        /// expands the Texture coordinates and compensates the scale so the actual screen size remains unchanged.
        /// </summary>
        /// <param name="expand">The size to expand, in pixels</param>
        /// <returns>itself.</returns>
        /// <remarks>
        /// This method is useful for tightly packed tiled bitmaps, where we want to contract the texture half a pixel (expand = -0.5f)
        /// </remarks>
        public ImageAsset WithExpandedSource(float expand)
        {
            var v2exp = new XY(expand);
            var v2min = _SourceUVMin;
            var v2siz = _SourceUVMax - _SourceUVMin;

            _SourceUVMin -= v2exp;
            _SourceUVMax += v2exp;

            var xs = _SourceUVMax - _SourceUVMin;

            _Scale = _Scale * v2siz / xs;
            _Pivot = _Pivot * xs / v2siz;

            return this;
        }

        public void CopyTo(ImageAsset other, XY pivotOffset)
        {
            other._Source = this.Source;
            other._SourceUVMin = this._SourceUVMin;
            other._SourceUVMax = this._SourceUVMax;
            other._Scale = this._Scale;
            other._Pivot = this._Pivot + pivotOffset; // should multiply by this.Scale ??
            other._PivotPrecedence = this._PivotPrecedence;
            other._OrientationMask = this._OrientationMask;
            other._CalculateMatrices();
        }

        #endregion

        #region data

        /// <summary>
        /// The bitmap source, which can be a device texture, a image file path, etc.
        /// </summary>
        private object _Source;

        /// <summary>
        /// TopLeft rectangle coordinate of the crop, in pixels
        /// </summary>
        private XY _SourceUVMin;

        /// <summary>
        /// BottomRight rectangle coordinate of the crop, in pixels
        /// </summary>
        private XY _SourceUVMax;

        /// <summary>
        /// The SRT origin of the image, relative to <see cref="_SourceUVMin"/>
        /// </summary>
        private XY _Pivot;

        /// <summary>
        /// Determines the flip order and Pivot precedence.
        /// </summary>
        private bool _PivotPrecedence = false;

        /// <summary>
        /// The output scale of the image
        /// </summary>
        private XY _Scale = XY.One;

        /// <summary>
        /// Default image orientation
        /// </summary>
        private ImageStyle.Orientation _OrientationMask;

        /// <summary>
        /// Matrices baked from pivot, scale, and flip flags
        /// </summary>
        private readonly System.Numerics.Matrix3x2[] _Transforms = new System.Numerics.Matrix3x2[4];

        #endregion

        #region properties

        /// <summary>
        /// device dependant reference that points, or is a bitmap.
        /// </summary>
        /// <remarks>
        /// This property can be cast to different data types depending on the context:
        /// <para>
        /// If it's a <see cref="string"/> or a <see cref="System.IO.FileInfo"/> it can point
        /// to an image in the file system.
        /// </para>
        /// <para>
        /// It can also be a known bitmap or texture object, in which case it should be used
        /// directly as the bitmap source.
        /// </para>
        /// </remarks>
        public object Source => _Source;

        /// <summary>
        /// Gets the coordinates of the center of the image, in pixels, relative to <see cref="Top"/> and <see cref="Left"/>.
        /// </summary>
        /// <example>
        /// We could define a image of size (20,20) with the rotation pivot located at its center like this:
        ///     Top: 33
        ///     Left: 55
        ///     Width: 20
        ///     Height: 20
        ///     Pivot: (10,10)        
        /// </example>
        [Obsolete("Use GetImageMatrix()")]
        public XY Pivot => _Pivot;

        /// <summary>
        /// Gets the rendering scale of the image.
        /// </summary>
        [Obsolete("Use GetImageMatrix()")]
        public XY Scale => _Scale;

        /// <summary>
        /// Top left texture coordinate (in pixels).
        /// </summary>
        public XY UV0 => _SourceUVMin;

        /// <summary>
        /// Top right texture coordinate (in pixels).
        /// </summary>
        public XY UV1 => new XY(_SourceUVMax.X, _SourceUVMin.Y);

        /// <summary>
        /// Bottom right texture coordinate (in pixels).
        /// </summary>
        public XY UV2 => _SourceUVMax;

        /// <summary>
        /// Bottom left texture coordinate (in pixels).
        /// </summary>
        public XY UV3 => new XY(_SourceUVMin.X, _SourceUVMax.Y);

        /// <summary>
        /// Gets a value indicating whether this asset can be rendered.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                if (Source == null) return false;
                if (_Scale.X == 0 || _Scale.Y == 0) return false;

                var wh = _SourceUVMax - _SourceUVMin;

                return wh.X != 0 && wh.Y != 0;
            }
        }

        #endregion

        #region API

        /// <summary>
        /// Gets the region within the source image, defined by the UV0,UV1,UV2,V3 coordinates.
        /// </summary>
        /// <returns>A rectangle.</returns>
        public System.Drawing.Rectangle GetSourceRectangle()
        {
            var wh = _SourceUVMax - _SourceUVMin;

            var x = (int)_SourceUVMin.X;
            var y = (int)_SourceUVMin.Y;
            var w = (int)wh.X;
            var h = (int)wh.Y;

            return new System.Drawing.Rectangle(x, y, w, h);
        }

        private void _CalculateMatrices()
        {
            for (int i = 0; i < _Transforms.Length; ++i)
            {
                var flags = (ImageStyle.Orientation)i;

                flags ^= _OrientationMask;

                var h = flags.HasFlag(ImageStyle.Orientation.FlipHorizontal);
                var v = flags.HasFlag(ImageStyle.Orientation.FlipVertical);

                _Transforms[i] = _GetImageMatrix(h, v, _PivotPrecedence);
            }
        }

        private System.Numerics.Matrix3x2 _GetImageMatrix(bool hflip, bool vflip, bool fpivot = false)
        {
            var size = _SourceUVMax - _SourceUVMin;

            var sx = hflip ? -_Scale.X : +_Scale.X;
            var sy = vflip ? -_Scale.Y : +_Scale.Y;

            if (fpivot) // pivot after flip
            {
                var final = System.Numerics.Matrix3x2.CreateScale(size.X * sx, size.Y * sy);
                final.Translation += new XY(Math.Max(0, -final.M11), Math.Max(0, -final.M22));
                final.Translation -= _Pivot * _Scale;
                return final;
            }
            else // pivot before flip
            {
                var final = System.Numerics.Matrix3x2.CreateScale(size);
                final.Translation = -_Pivot;
                final *= System.Numerics.Matrix3x2.CreateScale(sx, sy);
                return final;
            }
        }

        internal System.Numerics.Matrix3x2 GetImageMatrix(ImageStyle.Orientation orientation)
        {
            return _Transforms[(int)orientation];
        }

        internal void PrependTransform(ref System.Numerics.Matrix3x2 xform, ImageStyle.Orientation orientation)
        {
            xform = _Transforms[(int)orientation] * xform;
        }        

        #endregion
    }
}
