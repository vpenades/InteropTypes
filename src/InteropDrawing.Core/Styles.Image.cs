using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents an image with a style applied to it.
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="IPrimitiveCanvas2D.DrawImage(in System.Numerics.Matrix3x2, in ImageStyle)"/>.
    /// </remarks>
    public struct ImageStyle
    {
        #region implicit

        public static implicit operator ImageStyle(ImageAsset asset) { return new ImageStyle(asset, COLOR.White, false, false); }

        public static implicit operator ImageStyle((ImageAsset asset, COLOR color) args) { return new ImageStyle(args.asset, args.color, false, false); }

        public static implicit operator ImageStyle((ImageAsset, COLOR, bool, bool) tuple) { return new ImageStyle(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4); }

        public static implicit operator ImageStyle((ImageAsset, bool, bool) tuple) { return new ImageStyle(tuple.Item1, COLOR.White, tuple.Item2, tuple.Item3); }

        #endregion

        #region constructor

        public ImageStyle(ImageAsset bitmap, COLOR color, bool flipHorizontal, bool flipVertical)
        {
            Bitmap = bitmap;
            Color = color;

            _Orientation = Orientation.None;
            _Orientation |= flipHorizontal ? Orientation.FlipHorizontal : Orientation.None;
            _Orientation |= flipVertical ? Orientation.FlipVertical : Orientation.None;
        }

        public ImageStyle(ImageAsset bitmap, COLOR color, int flags)
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
        public COLOR Color;

        /// <summary>
        /// The orientation of the bitmap.
        /// </summary>
        internal Orientation _Orientation;

        #endregion

        #region properties

        public bool IsVisible => Bitmap.IsVisible && Color.A > 0;

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
