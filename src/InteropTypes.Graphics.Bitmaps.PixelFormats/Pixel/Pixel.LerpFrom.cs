using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        /// <summary>
        /// Lerp setter for integer types
        /// </summary>
        public interface ISetFromQuantizedLerp<TPixel>
            where TPixel : unmanaged
        {
            int QuantizedLerpShift { get; }

            /// <summary>
            /// Sets the value of this pixel from the interpolation of two other pixels
            /// </summary>
            /// <param name="left">The left pixel</param>
            /// <param name="right">The right pixel</param>
            /// <param name="amount">The weight of the right pixel, from 0 to (1 &lt;&lt; <see cref="QuantizedLerpShift"/>)</param>
            void SetFromLerp(TPixel left, TPixel right, uint amount);

            /// <summary>
            /// Sets the value of this pixel from the interpolation of four other pixels
            /// </summary>
            /// <param name="tl">The top left pixel</param>
            /// <param name="tr">The top right pixel</param>
            /// <param name="bl">The bottom left pixel</param>
            /// <param name="br">The bottom right pixel</param>
            /// <param name="x">The weight of the right side, from 0 to (1 &lt;&lt; <see cref="QuantizedLerpShift"/>)</param>
            /// <param name="y">The weight of the bottom side, from 0 to (1 &lt;&lt; <see cref="QuantizedLerpShift"/>)</param>
            void SetFromLerp(TPixel tl, TPixel tr, TPixel bl, TPixel br, uint x, uint y);
        }        
    }
}
