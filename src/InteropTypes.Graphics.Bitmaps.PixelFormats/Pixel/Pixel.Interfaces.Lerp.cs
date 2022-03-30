using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        public interface IFloatingInterpolator<TSrc, TDst>
            where TSrc : unmanaged
            where TDst : unmanaged
        {            
            TDst InterpolateLinear(in TSrc left, in TSrc right, float wx);           
            TDst InterpolateBilinear(in TSrc tl, in TSrc tr, in TSrc bl, in TSrc br, float wx, float wy);
        }

        public interface IQuantizedInterpolator<TSrc, TDst>
            where TSrc : unmanaged
            where TDst : unmanaged
        {
            /// <summary>
            /// Number of bits to shift the quantized weights used by the interpolators.
            /// </summary>
            int QuantizedLerpShift { get; }

            /// <summary>
            /// Interpolates left and right values using the given weights.
            /// </summary>
            /// <param name="left">>The left pixel</param>
            /// <param name="right">The right pixel</param>
            /// <param name="wx">The weight of the right pixel, from 0 to (1 &lt;&lt; <see cref="QuantizedLerpShift"/>)</param>
            /// <returns>the interpolated value</returns>
            TDst InterpolateLinear(TSrc left, TSrc right, uint wx);

            /// <summary>
            /// Interpolates four other pixels using the given weights.
            /// </summary>
            /// <param name="tl">The top left pixel</param>
            /// <param name="tr">The top right pixel</param>
            /// <param name="bl">The bottom left pixel</param>
            /// <param name="br">The bottom right pixel</param>
            /// <param name="wx">The weight of the right side, from 0 to (1 &lt;&lt; <see cref="QuantizedLerpShift"/>)</param>
            /// <param name="wy">The weight of the bottom side, from 0 to (1 &lt;&lt; <see cref="QuantizedLerpShift"/>)</param>
            /// <returns>the interpolated value</returns>
            TDst InterpolateBilinear(TSrc tl, TSrc tr, TSrc bl, TSrc br, uint wx, uint wy);
        }        


        partial struct BGR24
        {
            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static RGBP128F LerpToRGBP128F(BGR24 left, BGR24 right, float rx)
            {
                // calculate quantized weights                
                var wl = (1f - rx) / 255f;
                var wr = rx / 255f;

                var r = new System.Numerics.Vector4(left.R, left.G, left.B, 255f) * wl;
                r += new System.Numerics.Vector4(right.R, right.G, right.B, 255f) * wr;

                return new RGBP128F(r);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static RGBP128F LerpToRGBP128F(BGR24 tl, BGR24 tr, BGR24 bl, BGR24 br, float rx, float by)
            {
                // calculate quantized weights                
                var lx = 1f - rx;
                var ty = 1f - by;
                var wwww = new System.Numerics.Vector4(lx * ty, rx * ty, lx * by, rx * by) / 255f;

                var r = new System.Numerics.Vector4(tl.R, tl.G, tl.B, 255f) * wwww.X;
                r += new System.Numerics.Vector4(tr.R, tr.G, tr.B, 255f) * wwww.Y;
                r += new System.Numerics.Vector4(bl.R, bl.G, bl.B, 255f) * wwww.Z;
                r += new System.Numerics.Vector4(br.R, br.G, br.B, 255f) * wwww.W;

                return new RGBP128F(r);
            }
        }

        partial struct BGRA32
        {
            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static RGBP128F LerpToRGBP128F(BGRA32 tl, BGRA32 tr, BGRA32 bl, BGRA32 br, float rx, float by)
            {
                // calculate quantized weights                
                var lx = 1f - rx;
                var ty = 1f - by;
                var wwww = new System.Numerics.Vector4(lx * ty, rx * ty, lx * by, rx * by) / 255f;

                // calculate final alpha
                var aaaa = new System.Numerics.Vector4(tl.A, tr.A, bl.A, br.A);
                var a = System.Numerics.Vector4.Dot(aaaa, wwww);
                if (a == 0) return default;

                var r = new System.Numerics.Vector3(tl.R, tl.G, tl.B) * wwww.X;
                r += new System.Numerics.Vector3(tr.R, tr.G, tr.B) * wwww.Y;
                r += new System.Numerics.Vector3(bl.R, bl.G, bl.B) * wwww.Z;
                r += new System.Numerics.Vector3(br.R, br.G, br.B) * wwww.W;

                return new RGBP128F(r, a);
            }            
        }

        partial struct BGRA128F
        {
            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static RGBP128F LerpToRGBP128F(in BGRA128F tl, in BGRA128F tr, in BGRA128F bl, in BGRA128F br, float rx, float by)
            {
                // calculate quantized weights                
                var lx = 1f - rx;
                var ty = 1f - by;
                var wwww = new System.Numerics.Vector4(lx * ty, rx * ty, lx * by, rx * by);

                // calculate final alpha
                var aaaa = new System.Numerics.Vector4(tl.A, tr.A, bl.A, br.A);
                var a = System.Numerics.Vector4.Dot(aaaa, wwww);
                if (a == 0) return default;

                var r = new System.Numerics.Vector3(tl.R, tl.G, tl.B) * wwww.X;
                r += new System.Numerics.Vector3(tr.R, tr.G, tr.B) * wwww.Y;
                r += new System.Numerics.Vector3(bl.R, bl.G, bl.B) * wwww.Z;
                r += new System.Numerics.Vector3(br.R, br.G, br.B) * wwww.W;

                return new RGBP128F(r, a);
            }
        }
    }
}
