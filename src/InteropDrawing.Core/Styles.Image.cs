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

        public static implicit operator ImageStyle(ImageAsset asset) { return new ImageStyle(asset, ColorStyle.White, false, false); }

        public static implicit operator ImageStyle((ImageAsset asset, ColorStyle color) args) { return new ImageStyle(args.asset, args.color, false, false); }

        public static implicit operator ImageStyle((ImageAsset asset, ColorStyle color, bool, bool) tuple) { return new ImageStyle(tuple.asset, tuple.color, tuple.Item3, tuple.Item4); }

        public static implicit operator ImageStyle((ImageAsset asset, bool, bool) tuple) { return new ImageStyle(tuple.asset, ColorStyle.White, tuple.Item2, tuple.Item3); }

        #endregion

        #region constructor

        public ImageStyle(ImageAsset bitmap, ColorStyle color, bool flipHorizontal, bool flipVertical)
        {
            Bitmap = bitmap;
            Color = color;

            _Orientation = Orientation.None;
            _Orientation |= flipHorizontal ? Orientation.FlipHorizontal : Orientation.None;
            _Orientation |= flipVertical ? Orientation.FlipVertical : Orientation.None;
        }

        public ImageStyle(ImageAsset bitmap, ColorStyle color, int flags)
        {
            Bitmap = bitmap;
            Color = color;

            _Orientation = (Orientation)flags;
        }

        #endregion

        #region data

        /// <summary>
        /// The image source.
        /// </summary>
        public ImageAsset Bitmap;

        /// <summary>
        /// The color tint to apply to the bitmap.
        /// </summary>
        public ColorStyle Color;

        /// <summary>
        /// The orientation of the bitmap.
        /// </summary>
        internal Orientation _Orientation;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var h = Bitmap?.GetHashCode() ?? 0;
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
                this.Bitmap == other.Bitmap &&
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

        public bool IsVisible => Bitmap.IsVisible && Color.IsVisible;

        public bool FlipHorizontal
        {
            get => _Orientation.HasFlag(Orientation.FlipHorizontal);
            set => _Orientation = _Orientation & ~Orientation.FlipHorizontal | (value ? Orientation.FlipHorizontal : Orientation.None);
        }

        public bool FlipVertical
        {
            get => _Orientation.HasFlag(Orientation.FlipVertical);
            set => _Orientation = _Orientation & ~Orientation.FlipVertical | (value ? Orientation.FlipVertical : Orientation.None);
        }

        public int Flags => (int)_Orientation;

        #endregion

        #region API
        
        /// <summary>
        /// Gets the local transform associated to this image (Pivot, scale, mirroring).
        /// </summary>
        /// <returns>A transform matrix.</returns>
        public System.Numerics.Matrix3x2 GetTransform() { return Bitmap.UseTransforms().GetImageMatrix(_Orientation); }

        /// <summary>
        /// Gets the local transform associated to this image (Pivot, scale, mirroring).
        /// </summary>
        /// <param name="hflip">True to mirror horizontally.</param>
        /// <param name="vflip">True to mirror vertically.</param>
        /// <returns>A transform matrix.</returns>
        public System.Numerics.Matrix3x2 GetTransform(bool hflip, bool vflip)
        {
            var o = _Orientation;
            o ^= hflip ? Orientation.FlipHorizontal : Orientation.None;
            o ^= vflip ? Orientation.FlipVertical : Orientation.None;

            return Bitmap.UseTransforms().GetImageMatrix(o);
        }        

        /// <summary>
        /// Fills the vertices to their final values so they can be send to the rastering pipeline.
        /// </summary>
        /// <param name="vertices">The vertices to be initialized. It must have a length of 4</param>
        /// <param name="xform">the transform to apply</param>        
        /// <param name="depthZ">the depth to set in the Position.Z of the vertices</param>
        public void TransformVertices(Span<Vertex3> vertices, System.Numerics.Matrix3x2 xform, float depthZ = 1)
        {
            Bitmap.UseTransforms().TransformVertices(vertices, xform, _Orientation, this.Color.Packed, depthZ);
        }

        /// <summary>
        /// Fills the vertices to their final values so they can be send to the rastering pipeline.
        /// </summary>
        /// <param name="vertices">The vertices to be initialized. It must have a length of 4</param>
        /// <param name="xform">the transform to apply</param>        
        public void TransformVertices(Span<Vertex2> vertices, System.Numerics.Matrix3x2 xform)
        {
            Bitmap.UseTransforms().TransformVertices(vertices, xform, _Orientation, this.Color.Packed);
        }

        /// <summary>
        /// Fills the vertices to their final values so they can be send to the rastering pipeline.
        /// </summary>
        /// <param name="vertices">The vertices to be initialized. It must have a length of 4</param>
        /// <param name="xform">the transform to apply</param>        
        public void TransformVertices(Span<XY> vertices, System.Numerics.Matrix3x2 xform)
        {
            Bitmap.UseTransforms().TransformVertices(vertices, xform, _Orientation);
        }

        #endregion

        #region nested types

        [Flags]
        internal enum Orientation
        {
            None = 0,
            FlipHorizontal = 1,
            FlipVertical = 2,
        }

        #endregion
    }
}
