using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropBitmaps
{
    partial class Pixel
    {
        public interface IPixelPremulComposition<TPixel>
        {
            TPixel AlphaBlendWith(in Vector4 premulRGBA, float opacity);
        }

        public interface IPixelIntegerComposition<TDstPixel>
        {
            TDstPixel AlphaBlendWith(TDstPixel dst, int opacity);
        }

        #region premul alphablend

        partial struct BGR24 : IPixelPremulComposition<BGR24>
        {
            public BGR24 AlphaBlendWith(in Vector4 premulRGBA, float opacity)
            {
                var bottom = new Vector4(this.R, this.G, this.B, 1) / 255f;
                var final = Vector4.Lerp(bottom, premulRGBA, premulRGBA.W * opacity);
                throw new NotImplementedException();
            }
        }

        #endregion

        #region integer alpha

        private const int _INTEGEROPACITY_MAXVALUE = 65536;

        partial struct BGRA32
            : IPixelIntegerComposition<BGR24>
            , IPixelIntegerComposition<RGB24>
        {
            public RGB24 AlphaBlendWith(RGB24 dst, int opacity)
            {
                var y = this.A * opacity / _INTEGEROPACITY_MAXVALUE;
                var x = 255 - y;
                var r = (dst.R * x + this.R * y) / 255;
                var g = (dst.G * x + this.G * y) / 255;
                var b = (dst.B * x + this.B * y) / 255;
                return new RGB24(r, g, b);
            }

            public BGR24 AlphaBlendWith(BGR24 dst, int opacity)
            {
                var y = this.A * opacity / _INTEGEROPACITY_MAXVALUE;
                var x = 255 - y;
                var r = (dst.R * x + this.R * y) / 255;
                var g = (dst.G * x + this.G * y) / 255;
                var b = (dst.B * x + this.B * y) / 255;
                return new BGR24(r, g, b);
            }
        }

        partial struct RGBA32
            : IPixelIntegerComposition<BGR24>
            , IPixelIntegerComposition<RGB24>
        {
            public RGB24 AlphaBlendWith(RGB24 dst, int opacity)
            {
                var y = this.A * opacity / _INTEGEROPACITY_MAXVALUE;
                var x = 255 - y;
                var r = (dst.R * x + this.R * y) / 255;
                var g = (dst.G * x + this.G * y) / 255;
                var b = (dst.B * x + this.B * y) / 255;
                return new RGB24(r, g, b);
            }

            public BGR24 AlphaBlendWith(BGR24 dst, int opacity)
            {
                var y = this.A * opacity / _INTEGEROPACITY_MAXVALUE;
                var x = 255 - y;
                var r = (dst.R * x + this.R * y) / 255;
                var g = (dst.G * x + this.G * y) / 255;
                var b = (dst.B * x + this.B * y) / 255;
                return new BGR24(r, g, b);
            }
        }

        partial struct ARGB32
            : IPixelIntegerComposition<BGR24>
            , IPixelIntegerComposition<RGB24>
        {
            public RGB24 AlphaBlendWith(RGB24 dst, int opacity)
            {
                var y = this.A * opacity / _INTEGEROPACITY_MAXVALUE;
                var x = 255 - y;
                var r = (dst.R * x + this.R * y) / 255;
                var g = (dst.G * x + this.G * y) / 255;
                var b = (dst.B * x + this.B * y) / 255;
                return new RGB24(r, g, b);
            }

            public BGR24 AlphaBlendWith(BGR24 dst, int opacity)
            {
                var y = this.A * opacity / _INTEGEROPACITY_MAXVALUE;
                var x = 255 - y;
                var r = (dst.R * x + this.R * y) / 255;
                var g = (dst.G * x + this.G * y) / 255;
                var b = (dst.B * x + this.B * y) / 255;
                return new BGR24(r, g, b);
            }
        }

        #endregion

        #region integer premultiplied

        partial struct BGRP32
            : IPixelIntegerComposition<BGR24>
            , IPixelIntegerComposition<RGB24>
        {
            public BGR24 AlphaBlendWith(BGR24 dst, int opacity)
            {
                var y = this.A * opacity / _INTEGEROPACITY_MAXVALUE;
                var x = 255 - y;
                var r = (dst.R * x + this.PreR) / 255;
                var g = (dst.G * x + this.PreG) / 255;
                var b = (dst.B * x + this.PreB) / 255;
                return new BGR24(r, g, b);
            }

            public RGB24 AlphaBlendWith(RGB24 dst, int opacity)
            {
                var y = this.A * opacity / _INTEGEROPACITY_MAXVALUE;
                var x = 255 - y;
                var r = (dst.R * x + this.PreR) / 255;
                var g = (dst.G * x + this.PreG) / 255;
                var b = (dst.B * x + this.PreB) / 255;
                return new RGB24(r, g, b);
            }
        }

        #endregion
    }
}
