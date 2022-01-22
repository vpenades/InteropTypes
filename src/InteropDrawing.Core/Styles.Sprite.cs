﻿using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    /// <summary>
    /// Style used for sprite rendering
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="IDrawing2D.DrawSprite(in System.Numerics.Matrix3x2, in SpriteStyle)"/>.
    /// </remarks>
    public struct SpriteStyle
    {
        #region implicit

        public static implicit operator SpriteStyle(SpriteAsset asset) { return new SpriteStyle(asset, COLOR.White, false, false); }

        // public static implicit operator SpriteStyle((BitmapCell bitmap, float opacity) args) { return new SpriteStyle(args.bitmap, new COLOR((Byte)255, (Byte)255, (Byte)255, ((Byte)(args.opacity * 255)).Clamp(0, 255)), false, false); }

        public static implicit operator SpriteStyle((SpriteAsset asset, COLOR color) args) { return new SpriteStyle(args.asset, args.color, false, false); }

        public static implicit operator SpriteStyle((SpriteAsset, COLOR, bool, bool) tuple) { return new SpriteStyle(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4); }

        public static implicit operator SpriteStyle((SpriteAsset, bool, bool) tuple) { return new SpriteStyle(tuple.Item1, COLOR.White, tuple.Item2, tuple.Item3); }

        #endregion

        #region constructor

        public SpriteStyle(SpriteAsset bitmap, COLOR color, bool flipHorizontal, bool flipVertical)
        {
            this.Bitmap = bitmap;
            this.Color = color;

            _Orientation = Orientation.None;
            _Orientation |= flipHorizontal ? Orientation.FlipHorizontal : Orientation.None;
            _Orientation |= flipVertical ? Orientation.FlipVertical : Orientation.None;
        }

        #endregion

        #region data

        public SpriteAsset Bitmap;

        public COLOR Color;

        internal Orientation _Orientation;

        #endregion

        #region properties

        public bool IsVisible => Bitmap.IsVisible && Color.A > 0;

        public bool FlipHorizontal
        {
            get => _Orientation.HasFlag(Orientation.FlipHorizontal);
            set => _Orientation = (_Orientation & ~Orientation.FlipHorizontal) | (value ? Orientation.FlipHorizontal : Orientation.None);
        }

        public bool FlipVertical
        {
            get => _Orientation.HasFlag(Orientation.FlipVertical);
            set => _Orientation = (_Orientation & ~Orientation.FlipVertical) | (value ? Orientation.FlipVertical : Orientation.None);
        }

        #endregion

        #region API

        public System.Numerics.Matrix3x2 Transform => Bitmap.GetSpriteMatrix(FlipHorizontal, FlipVertical);

        public System.Numerics.Matrix3x2 GetTransform()
        {
            return Bitmap.GetSpriteMatrix(FlipHorizontal, FlipVertical);
        }

        public System.Numerics.Matrix3x2 GetTransform(bool hflip, bool vflip)
        {
            return Bitmap.GetSpriteMatrix(FlipHorizontal ^ hflip, FlipVertical ^ vflip);
        }

        public void PrependTransform(ref System.Numerics.Matrix3x2 xform)
        {
            Bitmap.PrependTransform(ref xform, FlipHorizontal, FlipVertical);
        }

        public void PrependTransform(ref System.Numerics.Matrix3x2 xform, bool hflip, bool vflip)
        {
            Bitmap.PrependTransform(ref xform, FlipHorizontal ^ hflip, FlipVertical ^ vflip);
        }

        #endregion

        #region nested types

        [Flags]
        internal enum Orientation
        {
            None = 0,
            FlipHorizontal = 1,
            FlipVertical = 2
        }

        #endregion
    }
}
