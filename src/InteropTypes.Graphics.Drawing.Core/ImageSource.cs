using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;



namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents an image resource defined as a bitmap source and a rectangular region within that bitmap.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="ImageSource"/> is part of <see cref="ImageStyle"/>, which can be used by<br/>
    /// <see cref="ICoreCanvas2D.DrawImage(in System.Numerics.Matrix3x2, ImageStyle)"/>.
    /// </para>
    /// <para>
    /// This class does not (usually) contain the image itself, but the to provide
    /// an image to the rendering backend.
    /// </para>
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{ToDebuggerDisplay(),nq}")]
    public class ImageSource : ICloneable
    {
        #region diagnostics

        private string ToDebuggerDisplay()
        {
            if (_Source == null) return "Empty";

            var tmp = _Source.ToString();

            tmp += $" Size:{_SourceWidth}, {_SourceHeight}";
            tmp += " => ";

            if (_SourceUVMin != XY.Zero && _SourceUVMax != new XY(_SourceWidth, _SourceHeight))
            {
                tmp += $" Min:{_SourceUVMin} Max:{_SourceUVMin}";
            }            

            if (_Pivot != XY.Zero) tmp += $" Pivot:{_Pivot}";
            if (_OutputScale != XY.One) tmp += $" Scale:{_OutputScale}";

            return tmp;
        }

        #endregion

        #region lifecycle        

        public static IEnumerable<ImageSource> CreateGrid(object source, int tileCount, int tileStride, Point2 tileSize, Point2 tilePivot, bool pivotPrecedence = false)
        {
            for (int idx = 0; idx < tileCount; ++idx)
            {
                var idx_x = idx % tileStride;
                var idx_y = idx / tileStride;

                yield return new ImageSource(source, (idx_x * tileSize.X, idx_y * tileSize.Y), (tileSize.X, tileSize.Y), tilePivot, pivotPrecedence);
            }
        }

        public static ImageSource CreateFromBitmap(object source, Point2 bitmapSize, Point2 pivot, bool pivotPrecedence = false)
        {
            return new ImageSource(source, (0, 0), bitmapSize, pivot, pivotPrecedence);
        }

        public static ImageSource Create(object source, Point2 origin, Point2 size, Point2 pivot, bool pivotPrecedence = false, bool mirrorX = false, bool mirrorY = false)
        {
            return new ImageSource(source, origin, size, pivot, pivotPrecedence, mirrorX, mirrorY);
        }

        public ImageSource(object source, Point2 origin, Point2 size, Point2 pivot, bool pivotPrecedence = false, bool mirrorX = false, bool mirrorY = false)
        {
            _Source = source;
            _SourceUVMin = origin.XY;
            _SourceUVMax = _SourceUVMin + size.XY;

            _Pivot = pivot.XY;
            _PivotPrecedence = pivotPrecedence;

            _OrientationMask = _ImageFlags.None;
            if (mirrorX) _OrientationMask |= _ImageFlags.FlipHorizontal;
            if (mirrorY) _OrientationMask |= _ImageFlags.FlipVertical;            
        }

        public ImageSource() { }        

        /// <inheritdoc/>
        Object ICloneable.Clone()
        {
            var other = new ImageSource();
            this.CopyTo(other, XY.Zero);
            return other;
        }

        /// <summary>
        /// Creates a new <see cref="ImageSource"/> that is a copy of the current instance.
        /// </summary>
        /// <returns>A new <see cref="ImageSource"/> that is a copy of this instance.</returns>
        public ImageSource Clone()
        {
            var other = new ImageSource();
            this.CopyTo(other, XY.Zero);
            return other;
        }

        /// <summary>
        /// Copies the content of the current <see cref="ImageSource"/> to another instance.
        /// </summary>
        /// <param name="other">The other instance to be overwritten.</param>        
        public void CopyTo(ImageSource other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            other._Source = this.Source;
            other._SourceWidth = this._SourceWidth;
            other._SourceHeight = this._SourceHeight;
            other._SourceUVMin = this._SourceUVMin;
            other._SourceUVMax = this._SourceUVMax;
            other._OutputScale = this._OutputScale;
            other._Pivot = this._Pivot;
            other._PivotPrecedence = this._PivotPrecedence;
            other._OrientationMask = this._OrientationMask;
            other._Transforms = this._Transforms;
        }

        /// <summary>
        /// Copies the content of the current <see cref="ImageSource"/> to another instance.
        /// </summary>
        /// <param name="other">The other instance to be overwritten.</param>
        /// <param name="pivotOffset">An optional pivot offset.</param>
        public void CopyTo(ImageSource other, Point2 pivotOffset)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            this.CopyTo(other);
            if (pivotOffset == XY.Zero) return;

            other._Pivot += pivotOffset;
            other._Transforms = null;
        }

        #endregion

        #region data

        /// <summary>
        /// The bitmap source, which can be a device texture, a image file path, etc.
        /// </summary>        
        private object _Source;

        /// <summary>
        /// The source width, in pixels, lazily set by the backend using
        /// <see cref="WithSourceSize"/> when the image is loaded.
        /// </summary>
        private int _SourceWidth;

        /// <summary>
        /// The source height, in pixels, lazily set by the backend using
        /// <see cref="WithSourceSize"/> when the image is loaded.
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
        private bool _PivotPrecedence;

        /// <summary>
        /// The output scale of the image
        /// </summary>
        private XY _OutputScale = XY.One;

        /// <summary>
        /// Default image orientation
        /// </summary>
        internal _ImageFlags _OrientationMask;
        
        /// <summary>
        /// Once the asset has been initialized, it gets the transforms to apply to the image.
        /// </summary>
        private _ImageSourceTransforms _Transforms;

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
        /// device dependant reference that points to, or is am image type.
        /// </summary>
        /// <remarks>
        /// This property can be cast to different data types depending on the context:
        /// <para>
        /// If it's a <see cref="string"/> or a <see cref="System.IO.FileInfo"/> it can point
        /// to an image in the file system.
        /// </para>
        /// <para>
        /// It can also be a known bitmap or texture type, in which case it should
        /// be used directly as the bitmap source.
        /// </para>
        /// </remarks>
        public object Source => _Source;

        #endregion

        #region Fluent API

        /// <summary>
        /// Sets a new pivot.
        /// </summary>
        /// <param name="pivot">The new pivot.</param>
        /// <returns>Self.</returns>
        public ImageSource WithPivot(Point2 pivot)
        {
            _Pivot = pivot.XY;
            _Transforms = null;
            return this;
        }

        /// <summary>
        /// Sets a new pivot/mirror precedence
        /// </summary>
        /// <param name="precedence">the mirror/pivot precedence.</param>
        /// <returns>Self.</returns>
        public ImageSource WithPivotPrecedence(bool precedence)
        {
            _PivotPrecedence = precedence;
            _Transforms = null;
            return this;
        }

        /// <summary>
        /// Sets the new output scale.
        /// </summary>
        /// <param name="scale">The scale.</param>
        /// <returns>Self.</returns>
        public ImageSource WithScale(float scale)
        {
            _OutputScale = new XY(scale);
            _Transforms = null;
            return this;
        }

        /// <summary>
        /// Sets the mirror flags.
        /// </summary>
        /// <param name="mirrorX">The horizontal mirror.</param>
        /// <param name="mirrorY">The vertical mirror.</param>
        /// <returns>Self.</returns>
        public ImageSource WithMirror(bool mirrorX, bool mirrorY)
        {
            var o = _ImageFlags.None;
            if (mirrorX) o |= _ImageFlags.FlipHorizontal;
            if (mirrorY) o |= _ImageFlags.FlipVertical;
            if (_OrientationMask == o) return this;
            _OrientationMask = o;
            _Transforms = null;
            return this;
        }

        /// <summary>
        /// Sets the source bitmap size.
        /// </summary>
        /// <param name="width">The image width, in pixels.</param>
        /// <param name="height">The image height, in pixels.</param>
        /// <returns>Self.</returns>
        public ImageSource WithSourceSize(int width, int height)
        {
            if (width == _SourceWidth && height == _SourceHeight) return this;

            _SourceWidth = width;
            _SourceHeight = height;
            _Transforms = null;
            return this;
        }

        /// <summary>
        /// Sets the source bitmap size.
        /// </summary>
        /// <param name="source">The new image source.</param>
        /// <param name="width">The image width, in pixels.</param>
        /// <param name="height">The image height, in pixels.</param>
        /// <returns>Self.</returns>
        public ImageSource WithSource(Object source, int width, int height)
        {
            if (source == _Source && width == _SourceWidth && height == _SourceHeight) return this;

            _Source = source;
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
        public ImageSource WithExpandedSource(float expand)
        {
            var v2exp = new XY(expand);

            var s1 = _SourceUVMax - _SourceUVMin;
            _SourceUVMin -= v2exp;
            _SourceUVMax += v2exp;
            var s2 = _SourceUVMax - _SourceUVMin;

            _OutputScale = _OutputScale * s1 / s2;
            _Pivot = _Pivot * s2 / s1;

            _Transforms = null;

            return this;
        }

        #endregion

        #region API        

        #pragma warning disable CA1024 // Use properties where appropriate

        /// <summary>
        /// Gets the region within the source image, defined by the UV0,UV1,UV2,V3 coordinates.
        /// </summary>
        /// <returns>A rectangle.</returns>
        public System.Drawing.RectangleF GetSourceRectangle()        
        {
            var wh = _SourceUVMax - _SourceUVMin;
            return new System.Drawing.RectangleF(_SourceUVMin.X, _SourceUVMin.Y, wh.X, wh.Y);
        }

        internal _ImageSourceTransforms UseTransforms()
        {
            _Transforms ??= new _ImageSourceTransforms(this, _SourceWidth, _SourceHeight);
            return _Transforms;
        }

        internal System.Numerics.Matrix3x2 _GetImageMatrix(bool mirrorX, bool mirrorY)
        {
            var size = _SourceUVMax - _SourceUVMin;

            var sx = mirrorX ? -_OutputScale.X : +_OutputScale.X;
            var sy = mirrorY ? -_OutputScale.Y : +_OutputScale.Y;

            if (_PivotPrecedence) // flip before pivot
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

        #pragma warning restore CA1024 // Use properties where appropriate

        /// <summary>
        /// Tries to open the stream of commonly implemented image sources.
        /// </summary>
        /// <param name="source">The object source declared at <see cref="_Source"/></param>
        /// <returns>A valid stream, or null.</returns>
        public static System.IO.Stream TryOpenRead(Object source)
        {
            if (source is System.ValueTuple<System.Reflection.Assembly, string> embeddedFile)
            {
                var allNames = embeddedFile.Item1.GetManifestResourceNames();

                var embeddedKey = allNames.FirstOrDefault(item => item.EndsWith(embeddedFile.Item2, StringComparison.OrdinalIgnoreCase));

                return embeddedFile.Item1.GetManifestResourceStream(embeddedKey);
            }

            if (source is Func<Stream> sfactory)
            {
                var s = sfactory.Invoke();
                if (s != null) return s;
            }

            if (source is string path)
            {
                try
                {
                    var f = new System.IO.FileInfo(path);
                    if (f.Exists) source = f;
                }
                catch { }
            }

            if (source is System.IO.FileInfo finfo)
            {
                return finfo.OpenRead();
            }

            return null;
        }

        #endregion
    }

    [Flags]
    internal enum _ImageFlags
    {
        None = 0,
        FlipHorizontal = 1,
        FlipVertical = 2,
    }

    /// <summary>
    /// Represents the cached calculated transforms of an <see cref="ImageSource"/>.
    /// </summary>
    class _ImageSourceTransforms
    {
        #region lifecycle

        public _ImageSourceTransforms(ImageSource asset, int width, int height)
        {
            for (int i = 0; i < _Transforms.Length; ++i)
            {
                var flags = (_ImageFlags)i;

                flags ^= asset._OrientationMask;

                var h = flags.HasFlag(_ImageFlags.FlipHorizontal);
                var v = flags.HasFlag(_ImageFlags.FlipVertical);

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
        
        private readonly XY _UV0; // Top left texture coordinate (normalized).        
        private readonly XY _UV1; // Top right texture coordinate (normalized).        
        private readonly XY _UV2; // Bottom right texture coordinate (normalized).        
        private readonly XY _UV3; // Bottom left texture coordinate (normalized).
        
        /// Matrices baked from pivot, scale, and flip flags        
        private readonly System.Numerics.Matrix3x2[] _Transforms = new System.Numerics.Matrix3x2[4];

        #endregion

        #region API

        public System.Numerics.Matrix3x2 GetImageMatrix(_ImageFlags orientation)
        {
            return _Transforms[(int)orientation];
        }        
        
        public void TransformVertices(Span<Vertex3> vertices, System.Numerics.Matrix3x2 xform, _ImageFlags o, ColorStyle color, float depthZ = 1)
        {
            var packed = color.PackedRGBA;

            xform = _Transforms[(int)o] * xform;
            var r0 = new XY(xform.M11, xform.M12);
            var r1 = new XY(xform.M21, xform.M22);

            vertices[0].Position = new XYZ(xform.Translation, depthZ);
            vertices[0].Color = packed;
            vertices[0].TextureCoord = _UV0;

            vertices[1].Position = new XYZ(xform.Translation + r0, depthZ);
            vertices[1].Color = packed;
            vertices[1].TextureCoord = _UV1;

            vertices[2].Position = new XYZ(xform.Translation + r0 + r1, depthZ);
            vertices[2].Color = packed;
            vertices[2].TextureCoord = _UV2;

            vertices[3].Position = new XYZ(xform.Translation + r1, depthZ);
            vertices[3].Color = packed;
            vertices[3].TextureCoord = _UV3;
        }
        
        public void TransformVertices(Span<Vertex2> vertices, System.Numerics.Matrix3x2 xform, _ImageFlags o, ColorStyle color)
        {
            var packed = color.PackedRGBA;

            xform = _Transforms[(int)o] * xform;
            var r0 = new XY(xform.M11, xform.M12);
            var r1 = new XY(xform.M21, xform.M22);

            vertices[0].Position = xform.Translation;
            vertices[0].Color = packed;
            vertices[0].TextureCoord = _UV0;

            vertices[1].Position = xform.Translation + r0;
            vertices[1].Color = packed;
            vertices[1].TextureCoord = _UV1;

            vertices[2].Position = xform.Translation + r0 + r1;
            vertices[2].Color = packed;
            vertices[2].TextureCoord = _UV2;

            vertices[3].Position = xform.Translation + r1;
            vertices[3].Color = packed;
            vertices[3].TextureCoord = _UV3;
        }
        
        public void TransformVertices(Span<XY> vertices, System.Numerics.Matrix3x2 xform, _ImageFlags o)
        {
            xform = _Transforms[(int)o] * xform;
            var r0 = new XY(xform.M11, xform.M12);
            var r1 = new XY(xform.M21, xform.M22);

            vertices[0] = xform.Translation;
            vertices[1] = xform.Translation + r0;
            vertices[2] = xform.Translation + r0 + r1;
            vertices[3] = xform.Translation + r1;
        }

        #endregion
    }
}
