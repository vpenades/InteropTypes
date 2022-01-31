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
    /// <see cref="ImageAsset"/> is part of <see cref="ImageStyle"/>, which can be used at<br/>
    /// <see cref="IImagesCanvas2D.DrawImage(in System.Numerics.Matrix3x2, in ImageStyle)"/>.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Source} ({Left}, {Top}) ({Width}, {Height}) Scale:{Scale}")]
    public class ImageAsset
    {
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

        public static ImageAsset CreateFromBitmap(object source, Point2 size, Point2 pivot, bool pivotPrecedence = false)
        {
            return new ImageAsset(source, (0, 0), size, pivot, pivotPrecedence);
        }

        public ImageAsset(object source, Point2 origin, Point2 size, Point2 pivot, bool pivotPrecedence = false)
        {
            _Source = source;
            _SourceUVMin = origin.ToNumerics();
            _SourceUVMax = _SourceUVMin + size.ToNumerics();

            _Pivot = pivot.ToNumerics();
            _PivotPrecedence = pivotPrecedence;

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

        public ImageAsset WithExpandedSource(float expand)
        {
            var v2exp = new XY(expand);
            var v2min = _SourceUVMin;
            var v2siz = _SourceUVMax - _SourceUVMin;

            _SourceUVMin -= v2exp;
            _SourceUVMin += v2exp;

            var xs = _SourceUVMax - _SourceUVMin;

            _Scale = _Scale * v2siz / xs;
            _Pivot = _Pivot * xs / v2siz;

            return this;
        }

        public void CopyTo(ImageAsset other, XY pivotOffset)
        {
            other._Source = Source;
            other._SourceUVMin = _SourceUVMin;
            other._SourceUVMax = _SourceUVMax;
            other._Scale = _Scale;
            other._Pivot = _Pivot + pivotOffset; // should multiply by this.Scale ??
            other._PivotPrecedence = _PivotPrecedence;
            other._CalculateMatrices();
        }

        #endregion

        #region data

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
        /// Gets the Left pixel coordinate within the <see cref="Source"/> asset.
        /// </summary>
        public float Left => _SourceUVMin.X;

        /// <summary>
        /// Gets the top pixel coordinate within the <see cref="Source"/> asset.
        /// </summary>
        public float Top => _SourceUVMin.Y;

        /// <summary>
        /// Gets the width of the image, in pixels.
        /// </summary>
        public float Width => _SourceUVMax.X - _SourceUVMin.X;

        /// <summary>
        /// Gets the Height of the image, in pixels.
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
                if (Source == null) return false;
                if (_Scale.X == 0 || _Scale.Y == 0) return false;
                if (Width == 0 || Height == 0) return false;
                return true;
            }
        }

        #endregion

        #region API

        private void _CalculateMatrices()
        {
            for (int i = 0; i < _Transforms.Length; ++i)
            {
                var flags = (ImageStyle.Orientation)i;

                var h = flags.HasFlag(ImageStyle.Orientation.FlipHorizontal);
                var v = flags.HasFlag(ImageStyle.Orientation.FlipVertical);

                _Transforms[i] = _GetImageMatrix(h, v, _PivotPrecedence);
            }
        }

        private System.Numerics.Matrix3x2 _GetImageMatrix(bool hflip, bool vflip, bool fpivot = false)
        {
            var sx = hflip ? -_Scale.X : +_Scale.X;
            var sy = vflip ? -_Scale.Y : +_Scale.Y;

            if (fpivot) // pivot after flip
            {
                var final = System.Numerics.Matrix3x2.CreateScale(Width * sx, Height * sy);
                final.Translation += new XY(Math.Max(0, -final.M11), Math.Max(0, -final.M22));
                final.Translation -= _Pivot * _Scale;
                return final;
            }
            else // pivot before flip
            {
                var final = System.Numerics.Matrix3x2.CreateScale(Width, Height);
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
