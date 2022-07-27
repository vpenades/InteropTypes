using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{
    internal class LerpTests
    {
        private static readonly Pixel.RGBA32 RGBA32_A = (255, 127, 63, 255);
        private static readonly Pixel.RGBA32 RGBA32_B = (255, 255, 255, 0);

        [Test]
        public void SamplerTests()
        {
            var a = new Pixel.RGBA32(255, 255, 255, 255);
            var b = new Pixel.RGBA32(0, 255, 255, 255);
            var c = new Pixel.RGBA32(255, 0, 255, 255);
            var d = new Pixel.RGBA32(255, 0, 0, 0);

            var x = Pixel.RGBA32.Lerp(a, b, c, d, 512, 512);

            Assert.AreEqual(new Pixel.RGBA32(169, 169, 255, 191), x);
        }

        [Test]
        public void TestLinearInterpolations()
        {
            // overflow test

            var o0 = Pixel.BGRA32.LerpToBGRP32(Colors.White, Colors.White, 512);

            // early tests

            var C0 = new Pixel.BGRA32(0, 100, 255, 1);
            var C1 = new Pixel.BGRA32(128, 1, 255, 2);
            var C2 = new Pixel.BGRA32(191, 1, 255, 4);
            var C3 = new Pixel.BGRA32(0, 1, 255, 7);
            var C4 = new Pixel.BGRA32(0, 0, 255, 64);
            var C5 = new Pixel.BGRA32(2, 1, 255, 133);
            var C6 = new Pixel.BGRA32(0, 1, 255, 240);

            foreach (var col in new[] { C6, C5, C2, C1, C0, C3, C4 })
            {
                var pp = new Pixel.BGRP32(col);
                var uu = new Pixel.BGRA32(pp);

                Assert.AreEqual(pp, col.To<Pixel.BGRP32>());
                Assert.AreEqual(uu, pp.To<Pixel.BGRA32>());
                
                var r0 = _LerpToBGRP32(col, col, 0);
                col.AssertEqualsWithPremul(r0);
                
                var r1 = _LerpToBGRP32(col, col, 139);
                col.AssertEqualsWithPremul(r1);
                
                var r2 = _LerpToBGRP32(col, col, 819);
                col.AssertEqualsWithPremul(r2);

                var r3 = _LerpToBGRP32(col, col, 1);
                col.AssertEqualsWithPremul(r3);                
            }
        }

        [Test]
        public void TestBilinearInterpolations()
        {
            // overflow test

            var o0 = Pixel.BGRA32.LerpToBGRP32(Colors.White, Colors.White, Colors.White, Colors.White, 512, 512);

            // early tests

            var C0 = new Pixel.BGRA32(0, 100, 255, 1);
            var C1 = new Pixel.BGRA32(128, 1, 255, 2);
            var C2 = new Pixel.BGRA32(191, 1, 255, 4);
            var C3 = new Pixel.BGRA32(0, 1, 255, 7);
            var C4 = new Pixel.BGRA32(0, 0, 255, 64);
            var C5 = new Pixel.BGRA32(2, 1, 255, 133);

            foreach (var col in new[] { C5, C2, C1, C0, C3, C4 })
            {
                var pp = new Pixel.BGRP32(col);
                var uu = new Pixel.BGRA32(pp);

                Assert.AreEqual(pp, col.To<Pixel.BGRP32>());
                Assert.AreEqual(uu, pp.To<Pixel.BGRA32>());
                         
                var r0 = Pixel.BGRA32.LerpToBGRP32(col, col, col, col, 0, 0);
                col.AssertEqualsWithPremul(r0);
                
                var r1 = Pixel.BGRA32.LerpToBGRP32(col, col, col, col, 7, 139);
                col.AssertEqualsWithPremul(r1);

                // var r2 = LerpToBGRP32_1(col, col, col, col, 819, 819);
                var r2 = Pixel.BGRA32.LerpToBGRP32(col, col, col, col, 819, 819);
                col.AssertEqualsWithPremul(r2);

                // var r3 = Pixel.BGRA32.Lerp(col, col, col, col, 427, 31);
                // col.AssertEqualsWithPremul(r3);
            }            
        }


        [Test]
        [Ignore("Very long test")]
        public void TestLinearInterpolationsBruteForce()
        {
            for (int r = 0; r < 256; ++r)
            {
                var opaque = new Pixel.BGR24(1, r, 255);

                _CheckLinear<Pixel.BGR24, Pixel.BGR24>(opaque, result => Assert.AreEqual(opaque, result));
                _CheckLinear<Pixel.BGR24, Pixel.BGRP32>(opaque, result => Assert.AreEqual(opaque, result.To<Pixel.BGR24>()));

                for (int a = 0; a < 256; ++a)
                {
                    TestContext.Progress.WriteLine($"Alpha:{a} Red:{r}");

                    var color = new Pixel.BGRA32(r, 1, 255, a);

                    _CheckLinear<Pixel.BGRP32, Pixel.BGRP32>(new Pixel.BGRP32(color), result => color.AssertEqualsWithPremul(result));
                    _CheckLinear<Pixel.BGRA32, Pixel.BGRP32>(color, result => color.AssertEqualsWithPremul(result));
                }
            }
        }
        
        [Test]
        [Ignore("Very long test")]
        public void TestBilinearInterpolationsBruteForce()
        {
            for (int a = 0; a < 256; ++a)
            {
                for (int r = 0; r < 256; ++r)
                {
                    TestContext.Progress.WriteLine($"Alpha:{a} Red:{r}");

                    var color = new Pixel.BGRA32(r, 1, 255, a);

                    _CheckBilinear<Pixel.BGRP32, Pixel.BGRP32>(new Pixel.BGRP32(color), result => color.AssertEqualsWithPremul(result), 37 + (uint)(r & 15));
                    _CheckBilinear<Pixel.BGRA32, Pixel.BGRP32>(color, result => color.AssertEqualsWithPremul(result), 37 + (uint)(r & 15));
                    // _CheckInterpolation<Pixel.BGRA32, Pixel.BGRA32>(color);
                }
            }
        }


        private void _CheckLinear<TSrcPixel, TDstPixel>(TSrcPixel srcColor, Action<TDstPixel> resultAction)
            where TSrcPixel : unmanaged, Pixel.IReflection, Pixel.IConvertTo
            where TDstPixel : unmanaged, Pixel.IReflection, Pixel.IConvertTo
        {
            var premulRef = srcColor.To<Pixel.BGRP32>();
            var unpremulRef = default(TSrcPixel);
            premulRef.CopyTo(ref unpremulRef);

            var interpolator = Pixel.GetQuantizedInterpolator<TSrcPixel, TDstPixel>();

            var scale = 1 << interpolator.QuantizedLerpShift;
            
            for (uint y = 0; y < scale; y ++)
            {
                var result = interpolator.InterpolateLinear(srcColor, srcColor, y);

                resultAction(result);
            }
        }

        private void _CheckBilinear<TSrcPixel, TDstPixel>(TSrcPixel srcColor, Action<TDstPixel> resultAction, uint granularity = 256)
            where TSrcPixel : unmanaged, Pixel.IReflection, Pixel.IConvertTo
            where TDstPixel : unmanaged, Pixel.IReflection, Pixel.IConvertTo
        {
            var premulRef = srcColor.To<Pixel.BGRP32>();
            var unpremulRef = default(TSrcPixel);
            premulRef.CopyTo(ref unpremulRef);

            var interpolator = Pixel.GetQuantizedInterpolator<TSrcPixel, TDstPixel>();            

            var scale = 1 << interpolator.QuantizedLerpShift;

            for (uint y = 0; y < scale; y += granularity)
            {
                // TestContext.Progress.WriteLine($"    Y:{y}");

                for (uint x = 0; x < scale; x += granularity)
                {
                    var result = interpolator.InterpolateBilinear(srcColor, srcColor, srcColor, srcColor, x, y);

                    resultAction(result);
                }
            }

            
            /*
            for (uint y = 0; y < scale; y += 256)
            {
                var result = interpolator.InterpolateLinear(srcColor, srcColor, y);                
            }*/
        }        

        private TDstPixel _TestLerpFloating<TSrcPixel, TDstPixel>(TSrcPixel a, TSrcPixel b)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
            where TDstPixel : unmanaged
        {
            var ap = a.To<Pixel.BGRP128F>();
            var bp = b.To<Pixel.BGRP128F>();

            // var rgbp = Pixel.BGRP128F.Lerp(ap, bp, 0.5f);
            // var final = default(TDstPixel).From(rgbp);
            // TestContext.WriteLine($"{a} ^ {b} = {final}");
            // return final;

            return default;
        }

        public static Pixel.BGRP32 _LerpToBGRP32(Pixel.BGRA32 left, Pixel.BGRA32 right, uint rx)
        {
            return Pixel.BGRA32.LerpToBGRP32(left, right, rx);

            const int _QLERPSHIFT = 10;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;            
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT * 2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);

            // calculate quantized weights
            uint lx = _QLERPVALUE - rx;

            // calculate final alpha
            uint a = (left.A * lx + right.A * rx) >> _QLERPSHIFT;
            if (a == 0) return default;

            // Final adjustment to ensure exact results:
            const uint scale = 258;
            const uint offset = 1023 * 4;

            // calculate premultiplied weights
            lx = (lx * scale * (uint)left.A) >> (16 - _QLERPSHIFT);
            rx = (rx * scale * (uint)right.A) >> (16 - _QLERPSHIFT);
            System.Diagnostics.Debug.Assert((lx + rx) <= _QLERPVALUESQUARED);            

            System.Diagnostics.Debug.Assert(((left.R * lx + right.R * rx + offset) >> _QLERPSHIFTSQUARED) < 256);
            System.Diagnostics.Debug.Assert(((left.G * lx + right.G * rx + offset) >> _QLERPSHIFTSQUARED) < 256);
            System.Diagnostics.Debug.Assert(((left.B * lx + right.B * rx + offset) >> _QLERPSHIFTSQUARED) < 256);

            // set values
            #if NET5_0_OR_GREATER
            Unsafe.SkipInit<Pixel.BGRP32>(out var result);
            #else
            var result = default(Pixel.BGRP32);
            #endif

            result.PreR = (byte)((left.R * lx + right.R * rx + offset) >> _QLERPSHIFTSQUARED);
            result.PreG = (byte)((left.G * lx + right.G * rx + offset) >> _QLERPSHIFTSQUARED);
            result.PreB = (byte)((left.B * lx + right.B * rx + offset) >> _QLERPSHIFTSQUARED);
            result.A = (byte)a;
            return result;
        }
    }
}

