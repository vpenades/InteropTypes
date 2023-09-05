using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents an image with a style applied to it.
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="ICoreCanvas2D.DrawImage(in System.Numerics.Matrix3x2, ImageStyle)"/>.
    /// </remarks>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public readonly struct ImageStyle : IEquatable<ImageStyle>
    {
        #region implicit

        public static implicit operator ImageStyle(ImageSource asset) { return new ImageStyle(asset); }

        public static implicit operator ImageStyle((ImageSource asset, ColorStyle color) args) { return new ImageStyle(args.asset, args.color); }

        public static implicit operator ImageStyle((ImageSource asset, ColorStyle color, bool, bool) tuple) { return new ImageStyle(tuple.asset, tuple.color, tuple.Item3, tuple.Item4); }

        public static implicit operator ImageStyle((ImageSource asset, bool, bool) tuple) { return new ImageStyle(tuple.asset, ColorStyle.White, tuple.Item2, tuple.Item3); }

        #endregion

        #region constructor

        public ImageStyle(ImageSource bitmap)
        {
            Image = bitmap;
            Color = ColorStyle.White;
            _Orientation = _ImageFlags.None;            
        }

        public ImageStyle(ImageSource bitmap, ColorStyle color)
        {
            Image = bitmap;
            Color = color;
            _Orientation = _ImageFlags.None;
        }

        public ImageStyle(ImageSource bitmap, ColorStyle color, bool flipHorizontal, bool flipVertical)
        {
            Image = bitmap;
            Color = color;

            _Orientation = _ImageFlags.None;
            _Orientation |= flipHorizontal ? _ImageFlags.FlipHorizontal : _ImageFlags.None;
            _Orientation |= flipVertical ? _ImageFlags.FlipVertical : _ImageFlags.None;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public ImageStyle(ImageSource bitmap, ColorStyle color, int flags)
        {
            Image = bitmap;
            Color = color;
            _Orientation = (_ImageFlags)flags;
        }

        #endregion

        #region data

        /// <summary>
        /// The image source.
        /// </summary>
        public readonly ImageSource Image;

        /// <summary>
        /// The color tint to apply to the bitmap.
        /// </summary>
        public readonly ColorStyle Color;

        /// <summary>
        /// The orientation of the bitmap.
        /// </summary>
        internal readonly _ImageFlags _Orientation;

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
            var h = Image?.GetHashCode() ?? 0;
            h ^= Color.GetHashCode();
            h ^= _Orientation.GetHashCode();
            return h;
        }

        /// <inheritdoc/>
        public readonly override bool Equals(object obj) { return obj is ImageStyle other && Equals(other); }

        /// <inheritdoc/>
        public readonly bool Equals(ImageStyle other)
        {
            return
                this.Image == other.Image &&
                this.Color == other.Color && 
                this._Orientation == other._Orientation;            
        }

        /// <inheritdoc/>
        public static bool operator ==(ImageStyle a, ImageStyle b) => a.Equals(b);

        /// <inheritdoc/>
        public static bool operator !=(ImageStyle a, ImageStyle b) => !a.Equals(b);

        #endregion

        #region properties

        public readonly bool IsEmpty => !IsVisible;

        public readonly bool IsVisible => Image.IsVisible && Color.IsVisible;

        public readonly bool FlipHorizontal => _Orientation.HasFlag(_ImageFlags.FlipHorizontal);

        public readonly bool FlipVertical => _Orientation.HasFlag(_ImageFlags.FlipVertical);

        public readonly int Flags => (int)_Orientation;

        #endregion

        #region API

        public ImageStyle WithOpacity(float opacity)
        {
            return new ImageStyle(this.Image, this.Color.WithOpacity(opacity), this.Flags);
        }

        public ImageStyle WithMirror(bool horizontal, bool vertical)
        {
            var orientation = _Orientation & ~(_ImageFlags.FlipHorizontal | _ImageFlags.FlipVertical);
            if (horizontal) orientation |= _ImageFlags.FlipHorizontal;
            if (vertical) orientation |= _ImageFlags.FlipVertical;

            return new ImageStyle(this.Image, this.Color, (int)orientation);
        }        

        /// <summary>
        /// Gets the local transform associated to this image (Pivot, scale, mirroring).
        /// </summary>
        /// <returns>A transform matrix.</returns>
        public readonly System.Numerics.Matrix3x2 GetTransform() { return Image.UseTransforms().GetImageMatrix(_Orientation); }

        /// <summary>
        /// Gets the local transform associated to this image (Pivot, scale, mirroring).
        /// </summary>
        /// <param name="mirrorX">True to mirror horizontally.</param>
        /// <param name="mirrorY">True to mirror vertically.</param>
        /// <returns>A transform matrix.</returns>
        public readonly System.Numerics.Matrix3x2 GetTransform(bool mirrorX, bool mirrorY)
        {
            var o = _Orientation;
            o ^= mirrorX ? _ImageFlags.FlipHorizontal : _ImageFlags.None;
            o ^= mirrorY ? _ImageFlags.FlipVertical : _ImageFlags.None;

            return Image.UseTransforms().GetImageMatrix(o);
        }        

        /// <summary>
        /// Fills the vertices to their final values so they can be send to the rastering pipeline.
        /// </summary>
        /// <param name="vertices">The vertices to be initialized. It must have a length of 4</param>
        /// <param name="xform">the transform to apply</param>
        /// <param name="premultiplyColor">true to pass the color as premultiplied</param>
        /// <param name="depthZ">the depth to set in the Position.Z of the vertices</param>
        public readonly void TransformVertices(Span<Vertex3> vertices, in System.Numerics.Matrix3x2 xform, bool premultiplyColor = false, float depthZ = 1)
        {
            var c = premultiplyColor ? this.Color.ToPremul() : this.Color;

            Image.UseTransforms().TransformVertices(vertices, xform, _Orientation, c, depthZ);
        }

        /// <summary>
        /// Fills the vertices to their final values so they can be send to the rastering pipeline.
        /// </summary>
        /// <param name="vertices">The vertices to be initialized. It must have a length of 4</param>
        /// <param name="xform">the transform to apply</param>        
        /// <param name="premultiplyColor">true to pass the color as premultiplied</param>
        public readonly void TransformVertices(Span<Vertex2> vertices, in System.Numerics.Matrix3x2 xform, bool premultiplyColor = false)
        {
            var c = premultiplyColor ? this.Color.ToPremul() : this.Color;

            Image.UseTransforms().TransformVertices(vertices, xform, _Orientation, c);
        }

        /// <summary>
        /// Fills the vertices to their final values so they can be send to the rastering pipeline.
        /// </summary>
        /// <param name="vertices">The vertices to be initialized. It must have a length of 4</param>
        /// <param name="xform">the transform to apply</param>        
        public readonly void TransformVertices(Span<XY> vertices, in System.Numerics.Matrix3x2 xform)
        {
            Image.UseTransforms().TransformVertices(vertices, xform, _Orientation);
        }

        /// <summary>
        /// Calculates a destination transformed vertices bounding rectangle
        /// </summary>
        /// <param name="xform">transform</param>
        /// <returns>A bounding rectangle of the destination vertices</returns>
        public readonly RectangleF GetVerticesBounds(in System.Numerics.Matrix3x2 xform)
        {
            Span<XY> vertices = stackalloc XY[4];
            TransformVertices(vertices, in xform);

            var min = XY.Min(vertices[0], vertices[1]);
            min = XY.Min(min, vertices[2]);
            min = XY.Min(min, vertices[3]);

            var max = XY.Max(vertices[0], vertices[1]);
            max = XY.Max(max, vertices[2]);
            max = XY.Max(max, vertices[3]);

            var size = max - min;

            return new RectangleF(min.X, min.Y, size.X, size.Y);
        }

        #endregion
    }
}
