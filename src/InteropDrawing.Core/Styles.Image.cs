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
    /// Style used by <see cref="IPrimitiveCanvas2D.DrawImage(in System.Numerics.Matrix3x2, ImageStyle)"/>.
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

        public System.Numerics.Matrix3x2 Transform => Bitmap.GetImageMatrix(_Orientation);

        public System.Numerics.Matrix3x2 GetTransform() { return Bitmap.GetImageMatrix(_Orientation); }

        public void PrependTransform(ref System.Numerics.Matrix3x2 xform) { Bitmap.PrependTransform(ref xform, _Orientation); }

        public System.Numerics.Matrix3x2 GetTransform(bool hflip, bool vflip)
        {
            var o = _Orientation;
            o ^= hflip ? Orientation.FlipHorizontal : Orientation.None;
            o ^= vflip ? Orientation.FlipVertical : Orientation.None;

            return Bitmap.GetImageMatrix(o);
        }

        public void PrependTransform(ref System.Numerics.Matrix3x2 xform, bool hflip, bool vflip)
        {
            var o = _Orientation;
            o ^= hflip ? Orientation.FlipHorizontal : Orientation.None;
            o ^= vflip ? Orientation.FlipVertical : Orientation.None;

            Bitmap.PrependTransform(ref xform, o);
        }

        /// <summary>
        /// Fills the vertices to their final values so they can be send to the rastering pipeline.
        /// </summary>
        /// <param name="vertices">The vertices to be initialized. It must have a length of 4</param>
        /// <param name="xform">the transform to apply</param>
        /// <param name="reciprocalTextureSize">The reciprocal of the source's texture size</param>
        /// <param name="depthZ">the depth to set in the Position.Z of the vertices</param>
        public void TransformVertices(Span<Vertex3> vertices, System.Numerics.Matrix3x2 xform, XY reciprocalTextureSize, float depthZ = 1)
        {
            Bitmap.PrependTransform(ref xform, _Orientation);

            vertices[0].Position = new XYZ(XY.Transform(XY.Zero, xform), depthZ);
            vertices[0].Color = Color.Packed;
            vertices[0].TextureCoord = Bitmap.UV0 * reciprocalTextureSize;

            vertices[1].Position = new XYZ(XY.Transform(XY.UnitX, xform), depthZ);
            vertices[1].Color = Color.Packed;
            vertices[1].TextureCoord = Bitmap.UV1 * reciprocalTextureSize;

            vertices[2].Position = new XYZ(XY.Transform(XY.One, xform), depthZ);
            vertices[2].Color = Color.Packed;
            vertices[2].TextureCoord = Bitmap.UV2 * reciprocalTextureSize;

            vertices[3].Position = new XYZ(XY.Transform(XY.UnitY, xform), depthZ);
            vertices[3].Color = Color.Packed;
            vertices[3].TextureCoord = Bitmap.UV3 * reciprocalTextureSize;
        }

        /// <summary>
        /// Fills the vertices to their final values so they can be send to the rastering pipeline.
        /// </summary>
        /// <param name="vertices">The vertices to be initialized. It must have a length of 4</param>
        /// <param name="xform">the transform to apply</param>
        /// <param name="reciprocalTextureSize">The reciprocal of the source's texture size</param>        
        public void TransformVertices(Span<Vertex2> vertices, System.Numerics.Matrix3x2 xform, XY reciprocalTextureSize)
        {
            Bitmap.PrependTransform(ref xform, _Orientation);

            vertices[0].Position = XY.Transform(XY.Zero, xform);
            vertices[0].Color = Color.Packed;
            vertices[0].TextureCoord = Bitmap.UV0 * reciprocalTextureSize;

            vertices[1].Position = XY.Transform(XY.UnitX, xform);
            vertices[1].Color = Color.Packed;
            vertices[1].TextureCoord = Bitmap.UV1 * reciprocalTextureSize;

            vertices[2].Position = XY.Transform(XY.One, xform);
            vertices[2].Color = Color.Packed;
            vertices[2].TextureCoord = Bitmap.UV2 * reciprocalTextureSize;

            vertices[3].Position = XY.Transform(XY.UnitY, xform);
            vertices[3].Color = Color.Packed;
            vertices[3].TextureCoord = Bitmap.UV3 * reciprocalTextureSize;
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
