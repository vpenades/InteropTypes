using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        public interface IPixelBlendOps<TSrcPixel, TDstPixel>
        {
            /// <summary>
            /// calculates the average value between this and the other value
            /// </summary>
            /// <param name="other"></param>
            /// <returns>The average result</returns>
            /// <remarks>
            /// result = (this + other) / 2;
            /// </remarks>
            TDstPixel AverageWith(TSrcPixel other);            
        }

        partial struct Alpha8 : IPixelBlendOps<Alpha8, Alpha8>
        {
            public Alpha8 AverageWith(Alpha8 other)
            {
                return new Alpha8((Byte)((this.A + other.A) / 2));
            }
        }

        partial struct BGRA32 : IPixelBlendOps<BGRA32, BGRA32>
        {
            public BGRA32 AverageWith(BGRA32 other)
            {
                if (this.A == 0) return other;
                if (other.A == 0) return this;

                return new BGRA32(
                    (this.R + other.R) / 2,
                    (this.G + other.G) / 2,
                    (this.B + other.B) / 2,
                    (this.A + other.A) / 2);
            }
        }

        partial struct RGBA32 : IPixelBlendOps<RGBA32, RGBA32>
        {
            public RGBA32 AverageWith(RGBA32 other)
            {
                if (this.A == 0) return other;
                if (other.A == 0) return this;

                return new RGBA32(
                    (this.R + other.R) / 2,
                    (this.G + other.G) / 2,
                    (this.B + other.B) / 2,
                    (this.A + other.A) / 2);
            }
        }

        partial struct ARGB32 : IPixelBlendOps<ARGB32, ARGB32>
        {
            public ARGB32 AverageWith(ARGB32 other)
            {
                if (this.A == 0) return other;
                if (other.A == 0) return this;

                return new ARGB32(
                    (this.R + other.R) / 2,
                    (this.G + other.G) / 2,
                    (this.B + other.B) / 2,
                    (this.A + other.A) / 2);
            }
        }

        partial struct BGR24 : IPixelBlendOps<BGR24, BGR24>
        {
            public BGR24 AverageWith(BGR24 other)
            {
                return new BGR24(
                    (this.R + other.R) / 2,
                    (this.G + other.G) / 2,
                    (this.B + other.B) / 2);
            }
        }

        partial struct RGB24 : IPixelBlendOps<RGB24, RGB24>
        {
            public RGB24 AverageWith(RGB24 other)
            {
                return new RGB24(
                    (this.R + other.R) / 2,
                    (this.G + other.G) / 2,
                    (this.B + other.B) / 2);
            }
        }

        partial struct RGBP32 : IPixelBlendOps<RGBP32, RGBP32>
        {
            public RGBP32 AverageWith(RGBP32 other)
            {
                if (this.A == 0) return other;
                if (other.A == 0) return this;

                return new RGBP32(
                    (this.PreR + other.PreR) / 2,
                    (this.PreG + other.PreG) / 2,
                    (this.PreB + other.PreB) / 2,
                    (this.A + other.A) / 2);
            }
        }

        partial struct BGRP32 : IPixelBlendOps<BGRP32, BGRP32>
        {
            public BGRP32 AverageWith(BGRP32 other)
            {
                if (this.A == 0) return other;
                if (other.A == 0) return this;

                return new BGRP32(
                    (this.PreR + other.PreR) / 2,
                    (this.PreG + other.PreG) / 2,
                    (this.PreB + other.PreB) / 2,
                    (this.A + other.A) / 2);
            }
        }
    }
}
