using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        public interface IQuantizedLerpFactory<TSrc, TDst>
            where TSrc : unmanaged
            where TDst : unmanaged
        {
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
    }
}
