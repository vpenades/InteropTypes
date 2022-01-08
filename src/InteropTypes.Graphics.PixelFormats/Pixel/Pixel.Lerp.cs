using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial class Pixel
    {
        public delegate TDst QuadSamplerDelegate<TSrc, TDst>(in TSrc p00, in TSrc p01, in TSrc p10, in TSrc p11, int x, int y);

        public static QuadSamplerDelegate<TSrc,TDst> TryGetQuadSampler<TSrc,TDst>()
        {
            var instance = default(TSrc) as IDelegateProvider<QuadSamplerDelegate<TSrc, TDst>>;
            return instance?.GetDelegate();
        }

        public interface IPixelLerpQuantizedOperator<TPixel>
        {
            TPixel LerpTo(TPixel value, int amount);
        }

        partial struct RGBA32
            : IPixelLerpQuantizedOperator<RGBA32>
            , IDelegateProvider<QuadSamplerDelegate<RGBA32,RGBA32>>
        {
            QuadSamplerDelegate<RGBA32, RGBA32> IDelegateProvider<QuadSamplerDelegate<RGBA32, RGBA32>>.GetDelegate() { return Lerp; }

            public RGBA32 LerpTo(RGBA32 other, int amount)
            {
                return Lerp(this,other,amount);
            }

            

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public static RGBA32 Lerp(in RGBA32 p00, in RGBA32 p01, in RGBA32 p10, in RGBA32 p11, int rx, int by)
            {
                // calculate quantized weights
                var lx = 16384 - rx;
                var ty = 16384 - by;
                var w00 = lx * ty / 16384;
                var w01 = rx * ty / 16384;
                var w10 = lx * by / 16384;
                var w11 = rx * by / 16384;

                System.Diagnostics.Debug.Assert((w00 + w01 + w10 + w11) == 16384);

                // calculate final alpha

                int a = (p00.A * w00 + p01.A * w01 + p10.A * w10 + p11.A * w11) / 16384;

                if (a == 0) return default;

                // calculate premultiplied RGB

                w00 *= p00.A;
                w01 *= p01.A;
                w10 *= p10.A;
                w11 *= p11.A;

                int r = (p00.R * w00 + p01.R * w01 + p10.R * w10 + p11.R * w11) / 16384;
                int g = (p00.G * w00 + p01.G * w01 + p10.G * w10 + p11.G * w11) / 16384;
                int b = (p00.B * w00 + p01.B * w01 + p10.B * w10 + p11.B * w11) / 16384;

                // unpremultiply RGB

                r /= a;
                g /= a;
                b /= a;

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
