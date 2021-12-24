using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial class Pixel
    {
        public interface IPixelIntegetLerpOperator<TPixel>
        {
            TPixel LerpTo(TPixel value, int amount);
        }

        partial struct RGBA32 : IPixelIntegetLerpOperator<RGBA32>
        {
            public RGBA32 LerpTo(RGBA32 other, int amount)
            {
                var thisWeight = (255 - amount) * this.A;
                var otherWeight = amount * other.A;

                var div = thisWeight + otherWeight;
                if (div == 0) return default;

                var a = (this.A * (255-amount) + other.A * amount) / 255;
                var r = (this.R * thisWeight + other.R * otherWeight) / div;
                var g = (this.G * thisWeight + other.G * otherWeight) / div;
                var b = (this.B * thisWeight + other.B * otherWeight) / div;

                return new RGBA32(r, g, b, a);                
            }
        }

        public static void LerpArray<TSrcPixel, TDstPixel>(ReadOnlySpan<TSrcPixel> left, ReadOnlySpan<TSrcPixel> right, float amount, Span<TDstPixel> dst)
            where TSrcPixel : unmanaged, IPixelConvertible<RGBA128F>
            where TDstPixel : unmanaged, IPixelFactory<RGBA128F, TDstPixel>
        {
            for (int i = 0; i < dst.Length; ++i)
            {
                var v = System.Numerics.Vector4.Lerp(left[i].ToPixel().RGBA, right[i].ToPixel().RGBA, amount);
                dst[i] = default(TDstPixel).From(new RGBA128F(v));
            }
        }

        public static void LerpArray<TDstPixel>(ReadOnlySpan<System.Numerics.Vector3> left, ReadOnlySpan<System.Numerics.Vector3> right, float amount, Span<TDstPixel> dst)
            where TDstPixel : unmanaged, IPixelFactory<RGBA128F, TDstPixel>
        {
            for (int i = 0; i < dst.Length; ++i)
            {
                var v = System.Numerics.Vector3.Lerp(left[i], right[i], amount);
                dst[i] = default(TDstPixel).From(new RGBA128F(v));
            }
        }
    }
}
