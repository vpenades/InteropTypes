using System;
using System.Collections.Generic;
using System.Text;

using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a graphic resource defined as a bitmap source and a region within that bitmap.
    /// </summary>
    /// <remarks>
    /// <see cref="ImageAsset"/> is part of <see cref="ImageStyle"/>, which can be used by<br/>
    /// <see cref="ICoreCanvas2D.DrawImage(in System.Numerics.Matrix3x2, ImageStyle)"/>.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Source} ({Left}, {Top}) ({Width}, {Height}) Scale:{Scale}")]
    public class ImageAsset : ICloneable
    {
        #region diagnostics

        private string ToDebuggerDisplay()
        {
            if (_Source == null) return "Empty";

            var tmp = _Source.ToString();

            tmp += $" O:{_SourceUVMin} WH:{_SourceUVMax - _SourceUVMin}";

            if (_Pivot != XY.Zero) tmp += $" P:{_Pivot}";
            if (_OutputScale != XY.One) tmp += $" S:{_OutputScale}";

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
        }

        public ImageAsset() { }

        public ImageAsset WithPivot(int x, int y)
        {
            _Pivot = new XY(x, y);
            _Transforms = null;
            return this;
        }

        public ImageAsset WithScale(float scale)
        {            
            _OutputScale = new XY(scale);
            _Transforms = null;
            return this;
        }

        public ImageAsset WithFlags(bool hflip, bool vflip)
        {
            var o = ImageStyle.Orientation.None;
            if (hflip) o |= ImageStyle.Orientation.FlipHorizontal;
            if (vflip) o |= ImageStyle.Orientation.FlipVertical;
            if (_OrientationMask == o) return this;
            _OrientationMask = o;
            _Transforms = null;
            return this;
        }

        public ImageAsset WithImageSize(int width, int height)
        {
            if (width == _SourceWidth && height == _SourceHeight) return this;

            _SourceWidth = width;
            _SourceHeight = height;
            _Transforms = null;
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

            _OutputScale = _OutputScale * v2siz / xs;
            _Pivot = _Pivot * xs / v2siz;

            _Transforms = null;

            return this;
        }

        Object ICloneable.Clone()
        {
            var other = new ImageAsset();
            this.CopyTo(other, XY.Zero);
            return other;
        }

        public ImageAsset Clone()
        {
            var other = new ImageAsset();
            this.CopyTo(other, XY.Zero);
            return other;
        }

        public void CopyTo(ImageAsset other, XY pivotOffset)
        {
            other._Source = this.Source;
            other._SourceWidth = this._SourceWidth;
            other._SourceHeight = this._SourceHeight;
            other._SourceUVMin = this._SourceUVMin;
            other._SourceUVMax = this._SourceUVMax;
            other._OutputScale = this._OutputScale;
            other._Pivot = this._Pivot + pivotOffset; // should multiply by this.Scale ??
            other._PivotPrecedence = this._PivotPrecedence;
            other._OrientationMask = this._OrientationMask;
            other._Transforms = pivotOffset == XY.Zero ? this._Transforms : null;
        }

        #endregion

        #region data

        /// <summary>
        /// The bitmap source, which can be a device texture, a image file path, etc.
        /// </summary>        
        private object _Source;

        /// <summary>
        /// The source width, in pixels, set by the backend using
        /// <see cref="WithImageSize"/> when the image is loaded.
        /// </summary>
        private int _SourceWidth;

        /// <summary>
        /// The source height, in pixels, set by the backend using
        /// <see cref="WithImageSize"/> when the image is loaded.
        /// </summary>
        private int _SourceHeight;

        /// <summary>
        /// TopLeft rectangle coordinate of the crop, in pixels
        /// </summary>
        internal XY _SourceUVMin;

        /// <summary>
        /// BottomRight rectangle coordinate of the crop, in pixels
        /// </summary>
        internal XY _SourceUVMax;

        /// <summary>
        /// The rotation pivot, relative to <see cref="_SourceUVMin"/>
        /// </summary>
        private XY _Pivot;

        /// <summary>
        /// Determines the flip order vs Pivot transform precedence.
        /// </summary>
        private bool _PivotPrecedence = false;

        /// <summary>
        /// The output scale of the image
        /// </summary>
        private XY _OutputScale = XY.One;

        /// <summary>
        /// Default image orientation
        /// </summary>
        internal ImageStyle.Orientation _OrientationMask;
        
        /// <summary>
        /// Once the asset has been initialized, it gets the transforms to apply to the image.
        /// </summary>
        private _ImageAssetTransforms _Transforms;

        #endregion

        #region properties

        /// <summary>
        /// Gets a value indicating whether this asset can be rendered.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                if (Source == null) return false;
                if (_OutputScale.X == 0 || _OutputScale.Y == 0) return false;

                var wh = _SourceUVMax - _SourceUVMin;

                return wh.X != 0 && wh.Y != 0;
            }
        }

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

        #endregion

        #region API        

        /// <summary>
        /// Gets the region within the source image, defined by the UV0,UV1,UV2,V3 coordinates.
        /// </summary>
        /// <returns>A rectangle.</returns>
        public System.Drawing.RectangleF GetSourceRectangle()
        {
            var wh = _SourceUVMax - _SourceUVMin;
            return new System.Drawing.RectangleF(_SourceUVMin.X, _SourceUVMin.Y, wh.X, wh.Y);
        }        

        internal System.Numerics.Matrix3x2 _GetImageMatrix(bool hflip, bool vflip)
        {
            var size = _SourceUVMax - _SourceUVMin;

            var sx = hflip ? -_OutputScale.X : +_OutputScale.X;
            var sy = vflip ? -_OutputScale.Y : +_OutputScale.Y;

            if (_PivotPrecedence) // pivot after flip
            {
                var final = System.Numerics.Matrix3x2.CreateScale(size.X * sx, size.Y * sy);
                final.Translation += new XY(Math.Max(0, -final.M11), Math.Max(0, -final.M22));
                final.Translation -= _Pivot * _OutputScale;
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

        internal _ImageAssetTransforms UseTransforms()
        {
            if (_Transforms != null) return _Transforms;
            _Transforms = new _ImageAssetTransforms(this, _SourceWidth, _SourceHeight);
            return _Transforms;
        }

        #endregion
    }

    /// <summary>
    /// Represents the cached calculated transforms of a <see cref="ImageAsset"/>.
    /// </summary>
    class _ImageAssetTransforms
    {
        #region lifecycle

        public _ImageAssetTransforms(ImageAsset asset, int width, int height)
        {
            for (int i = 0; i < _Transforms.Length; ++i)
            {
                var flags = (ImageStyle.Orientation)i;

                flags ^= asset._OrientationMask;

                var h = flags.HasFlag(ImageStyle.Orientation.FlipHorizontal);
                var v = flags.HasFlag(ImageStyle.Orientation.FlipVertical);

                _Transforms[i] = asset._GetImageMatrix(h, v);
            }            

            if (width > 0 && height > 0)
            {
                XY uv0 = asset._SourceUVMin;
                XY uv1 = new XY(asset._SourceUVMax.X, asset._SourceUVMin.Y);
                XY uv2 = asset._SourceUVMax;
                XY uv3 = new XY(asset._SourceUVMin.X, asset._SourceUVMax.Y);

                var s = new XY(width, height);
                this._UV0 = uv0 / s;
                this._UV1 = uv1 / s;
                this._UV2 = uv2 / s;
                this._UV3 = uv3 / s;
            }
            else
            {
                this._UV0 = XY.Zero;
                this._UV1 = XY.UnitX;
                this._UV2 = XY.One;
                this._UV3 = XY.UnitY;
            }
        }

        #endregion

        #region data
        
        private XY _UV0; // Top left texture coordinate (normalized).        
        private XY _UV1; // Top right texture coordinate (normalized).        
        private XY _UV2; // Bottom right texture coordinate (normalized).        
        private XY _UV3; // Bottom left texture coordinate (normalized).
        
        /// Matrices baked from pivot, scale, and flip flags        
        private readonly System.Numerics.Matrix3x2[] _Transforms = new System.Numerics.Matrix3x2[4];

        #endregion

        #region API

        public System.Numerics.Matrix3x2 GetImageMatrix(ImageStyle.Orientation orientation)
        {
            return _Transforms[(int)orientation];
        }        
        
        public void TransformVertices(Span<Vertex3> vertices, System.Numerics.Matrix3x2 xform, ImageStyle.Orientation o, uint color, float depthZ = 1)
        {
            xform = _Transforms[(int)o] * xform;

            vertices[0].Position = new XYZ(XY.Transform(XY.Zero, xform), depthZ);
            vertices[0].Color = color;
            vertices[0].TextureCoord = _UV0;

            vertices[1].Position = new XYZ(XY.Transform(XY.UnitX, xform), depthZ);
            vertices[1].Color = color;
            vertices[1].TextureCoord = _UV1;

            vertices[2].Position = new XYZ(XY.Transform(XY.One, xform), depthZ);
            vertices[2].Color = color;
            vertices[2].TextureCoord = _UV2;

            vertices[3].Position = new XYZ(XY.Transform(XY.UnitY, xform), depthZ);
            vertices[3].Color = color;
            vertices[3].TextureCoord = _UV3;
        }
        
        public void TransformVertices(Span<Vertex2> vertices, System.Numerics.Matrix3x2 xform, ImageStyle.Orientation o, uint color)
        {
            xform = _Transforms[(int)o] * xform;

            vertices[0].Position = XY.Transform(XY.Zero, xform);
            vertices[0].Color = color;
            vertices[0].TextureCoord = _UV0;

            vertices[1].Position = XY.Transform(XY.UnitX, xform);
            vertices[1].Color = color;
            vertices[1].TextureCoord = _UV1;

            vertices[2].Position = XY.Transform(XY.One, xform);
            vertices[2].Color = color;
            vertices[2].TextureCoord = _UV2;

            vertices[3].Position = XY.Transform(XY.UnitY, xform);
            vertices[3].Color = color;
            vertices[3].TextureCoord = _UV3;
        }
        
        public void TransformVertices(Span<XY> vertices, System.Numerics.Matrix3x2 xform, ImageStyle.Orientation o)
        {
            xform = _Transforms[(int)o] * xform;

            vertices[0] = XY.Transform(XY.Zero, xform);
            vertices[1] = XY.Transform(XY.UnitX, xform);
            vertices[2] = XY.Transform(XY.One, xform);
            vertices[3] = XY.Transform(XY.UnitY, xform);
        }

        #endregion
    }
}
