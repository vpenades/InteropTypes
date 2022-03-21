using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        public interface IPixelCompositionQ<TSrcPixel, TDstPixel>
        {
            TDstPixel AlphaBlendWith(TSrcPixel src, int opacity);
        }

        public interface IPixelCompositionF<TSrcPixel, TDstPixel>
        {
            TDstPixel AlphaBlendWith(TSrcPixel src, float opacity);
        }
                       
        partial struct BGR24 :
            IPixelCompositionQ<BGR24, BGR24>,
            IPixelCompositionQ<ARGB32, BGR24>,
            IPixelCompositionQ<RGBA32, BGR24>,
            IPixelCompositionQ<BGRA32, BGR24>,
            IPixelCompositionQ<RGBP32, BGR24>,
            IPixelCompositionQ<BGRP32, BGR24>
        {
            public BGR24 AlphaBlendWith(BGR24 src, int opacity)
            {                
                var x = 16384 - opacity;
                var r = (this.R * x + src.R * opacity) / 16384;
                var g = (this.G * x + src.G * opacity) / 16384;
                var b = (this.B * x + src.B * opacity) / 16384;
                return new BGR24(r, g, b);
            }

            public BGR24 AlphaBlendWith(ARGB32 src, int opacity)
            {
                opacity = opacity * src.A / 255;
                var x = 16384 - opacity;

                var r = (this.R * x + src.R * opacity) / 16384;
                var g = (this.G * x + src.G * opacity) / 16384;
                var b = (this.B * x + src.B * opacity) / 16384;
                return new BGR24(r, g, b);
            }

            public BGR24 AlphaBlendWith(RGBA32 src, int opacity)
            {
                opacity = opacity * src.A / 255;
                var x = 16384 - opacity;

                var r = (this.R * x + src.R * opacity) / 16384;
                var g = (this.G * x + src.G * opacity) / 16384;
                var b = (this.B * x + src.B * opacity) / 16384;
                return new BGR24(r, g, b);
            }

            public BGR24 AlphaBlendWith(BGRA32 src, int opacity)
            {
                opacity = opacity * src.A / 255;
                var x = 16384 - opacity;
                
                var r = (this.R * x + src.R * opacity) / 16384;
                var g = (this.G * x + src.G * opacity) / 16384;
                var b = (this.B * x + src.B * opacity) / 16384;
                return new BGR24(r, g, b);
            }

            public BGR24 AlphaBlendWith(BGRP32 src, int opacity)
            {
                var x = 16384 - opacity; // x before alpha
                opacity = opacity * src.A / 255;                

                var r = (this.R * x + src.R * opacity) / 16384;
                var g = (this.G * x + src.G * opacity) / 16384;
                var b = (this.B * x + src.B * opacity) / 16384;
                return new BGR24(r, g, b);
            }

            public BGR24 AlphaBlendWith(RGBP32 src, int opacity)
            {
                var x = 16384 - opacity; // x before alpha
                opacity = opacity * src.A / 255;                

                var r = (this.R * x + src.R * opacity) / 16384;
                var g = (this.G * x + src.G * opacity) / 16384;
                var b = (this.B * x + src.B * opacity) / 16384;
                return new BGR24(r, g, b);
            }
        }

        partial struct BGRP32
        {
            public void ApplyCompositionTo(ref BGR24 dst)
            {
                if (this.A == 0) return;
                var opacity = 16384 * this.A / 255;
                var x = 16384 - opacity;                
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new BGR24(_R, _G, _B);
            }            
        }
    }
}
