using System;
using System.Collections.Generic;
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
    public struct ImageStyle : IEquatable<ImageStyle>
    {
        #region implicit

        public static implicit operator ImageStyle(ImageSource asset) { return new ImageStyle(asset, ColorStyle.White, false, false); }

        public static implicit operator ImageStyle((ImageSource asset, ColorStyle color) args) { return new ImageStyle(args.asset, args.color, false, false); }

        public static implicit operator ImageStyle((ImageSource asset, ColorStyle color, bool, bool) tuple) { return new ImageStyle(tuple.asset, tuple.color, tuple.Item3, tuple.Item4); }

        public static implicit operator ImageStyle((ImageSource asset, bool, bool) tuple) { return new ImageStyle(tuple.asset, ColorStyle.White, tuple.Item2, tuple.Item3); }

        #endregion

        #region constructor

        public ImageStyle(ImageSource bitmap, ColorStyle color, bool flipHorizontal, bool flipVertical)
        {
            Image = bitmap;
            Color = color;

            _Orientation = _ImageFlags.None;
            _Orientation |= flipHorizontal ? _ImageFlags.FlipHorizontal : _ImageFlags.None;
            _Orientation |= flipVertical ? _ImageFlags.FlipVertical : _ImageFlags.None;
        }

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
        public ImageSource Image;

        /// <summary>
        /// The color tint to apply to the bitmap.
        /// </summary>
        public ColorStyle Color;

        /// <summary>
        /// The orientation of the bitmap.
        /// </summary>
        internal _ImageFlags _Orientation;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var h = Image?.GetHashCode() ?? 0;
            h ^= Color.GetHashCode();
            h ^= _Orientation.GetHashCode();
            return h;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) { return obj is ImageStyle other && Equals(other); }

        /// <inheritdoc/>
        public bool Equals(ImageStyle other)
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

        public bool IsEmpty => !IsVisible;

        public bool IsVisible => Image.IsVisible && Color.IsVisible;

        public bool FlipHorizontal
        {
            get => _Orientation.HasFlag(_ImageFlags.FlipHorizontal);
            set => _Orientation = _Orientation & ~_ImageFlags.FlipHorizontal | (value ? _ImageFlags.FlipHorizontal : _ImageFlags.None);
        }

        public bool FlipVertical
        {
            get => _Orientation.HasFlag(_ImageFlags.FlipVertical);
            set => _Orientation = _Orientation & ~_ImageFlags.FlipVertical | (value ? _ImageFlags.FlipVertical : _ImageFlags.None);
        }

        public int Flags => (int)_Orientation;

        #endregion

        #region API
        
        /// <summary>
        /// Gets the local transform associated to this image (Pivot, scale, mirroring).
        /// </summary>
        /// <returns>A transform matrix.</returns>
        public System.Numerics.Matrix3x2 GetTransform() { return Image.UseTransforms().GetImageMatrix(_Orientation); }

        /// <summary>
        /// Gets the local transform associated to this image (Pivot, scale, mirroring).
        /// </summary>
        /// <param name="mirrorX">True to mirror horizontally.</param>
        /// <param name="mirrorY">True to mirror vertically.</param>
        /// <returns>A transform matrix.</returns>
        public System.Numerics.Matrix3x2 GetTransform(bool mirrorX, bool mirrorY)
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
        /// <param name="depthZ">the depth to set in the Position.Z of the vertices</param>
        public void TransformVertices(Span<Vertex3> vertices, System.Numerics.Matrix3x2 xform, float depthZ = 1)
        {
            Image.UseTransforms().TransformVertices(vertices, xform, _Orientation, this.Color.Packed, depthZ);
        }

        /// <summary>
        /// Fills the vertices to their final values so they can be send to the rastering pipeline.
        /// </summary>
        /// <param name="vertices">The vertices to be initialized. It must have a length of 4</param>
        /// <param name="xform">the transform to apply</param>        
        public void TransformVertices(Span<Vertex2> vertices, System.Numerics.Matrix3x2 xform)
        {
            Image.UseTransforms().TransformVertices(vertices, xform, _Orientation, this.Color.Packed);
        }

        /// <summary>
        /// Fills the vertices to their final values so they can be send to the rastering pipeline.
        /// </summary>
        /// <param name="vertices">The vertices to be initialized. It must have a length of 4</param>
        /// <param name="xform">the transform to apply</param>        
        public void TransformVertices(Span<XY> vertices, System.Numerics.Matrix3x2 xform)
        {
            Image.UseTransforms().TransformVertices(vertices, xform, _Orientation);
        }

        #endregion
    }
}
