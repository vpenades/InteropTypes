using System;
using System.Collections.Generic;
using System.Linq;
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

            var x = Pixel.RGBA32.Lerp(a, b, c, d, 8192, 8192);

            // var sampler = Pixel.TryGetQuadSampler<Pixel.RGBA32, Pixel.RGBA32>();

            // var x = sampler(a, b, c, d, 8192, 8192);

        }

        [Test]
        public void BGRP32LerpTests()
        {
            Assert.AreEqual(2139045663, Pixel.RGBA32.LerpToBGRP32(RGBA32_A, RGBA32_B, 512).GetHashCode());
            Assert.AreEqual(2139045663, RGBA32_A.LerpToBGRP32(RGBA32_B, 512).GetHashCode());

            Assert.AreEqual(127, _TestLerpQuantized<Pixel.RGBA32, Pixel.Alpha8>(RGBA32_A, RGBA32_B).GetHashCode());

            Assert.AreEqual(-25281024, _TestLerpQuantized<Pixel.RGBA32, Pixel.RGB24>(RGBA32_A, RGBA32_B).GetHashCode());
            Assert.AreEqual(1048509952, _TestLerpQuantized<Pixel.RGBA32, Pixel.BGR24>(RGBA32_A, RGBA32_B).GetHashCode());

            Assert.AreEqual(2147384894, _TestLerpQuantized<Pixel.RGBA32, Pixel.BGRA32>(RGBA32_A, RGBA32_B).GetHashCode());
            Assert.AreEqual(2134802174, _TestLerpQuantized<Pixel.RGBA32, Pixel.RGBA32>(RGBA32_A, RGBA32_B).GetHashCode());
            Assert.AreEqual(1048510079, _TestLerpQuantized<Pixel.RGBA32, Pixel.ARGB32>(RGBA32_A, RGBA32_B).GetHashCode());

            Assert.AreEqual(2132688510, _TestLerpQuantized<Pixel.RGBA32, Pixel.RGBP32>(RGBA32_A, RGBA32_B).GetHashCode());
            Assert.AreEqual(2139045663, _TestLerpQuantized<Pixel.RGBA32, Pixel.BGRP32>(RGBA32_A, RGBA32_B).GetHashCode());
        }

        [Test]
        public void TestInterpolations()
        {
            var C0 = new Pixel.BGRA32(0, 100, 255, 1);
            var C1 = new Pixel.BGRA32(128, 1, 255, 2);
            var C2 = new Pixel.BGRA32(191, 1, 255, 4);
            var C3 = new Pixel.BGRA32(0, 1, 255, 7);
            var C4 = new Pixel.BGRA32(0, 0, 255, 64);


            foreach (var col in new[] { C4, C2, C1, C0, C3 })
            {
                var pp = new Pixel.BGRP32(col);
                var uu = new Pixel.BGRA32(pp);

                Assert.AreEqual(pp, col.To<Pixel.BGRP32>());
                Assert.AreEqual(uu, pp.To<Pixel.BGRA32>());

                var xx = Pixel.BGRA32.LerpToBGRP32(col, col, col, col, 0, 0);
                Assert.AreEqual(pp, xx);

                var yy = Pixel.BGRA32.Lerp(col, col, col, col, 0, 0);                
                Assert.AreEqual(uu, yy);
            }

            for (int a=0; a < 256; ++a)
            {
                TestContext.Progress.WriteLine($"Alpha {a}");

                for (int r = 0; r < 256; ++r)
                {
                    var color = new Pixel.BGRA32(r,1,255,a);

                    _CheckInterpolation<Pixel.BGRP32,Pixel.BGRP32>(new Pixel.BGRP32(color));
                    _CheckInterpolation<Pixel.BGRA32, Pixel.BGRP32>(color);
                    _CheckInterpolation<Pixel.BGRA32, Pixel.BGRA32>(color);
                }
            }            
        }        

        private void _CheckInterpolation<TSrcPixel, TDstPixel>(TSrcPixel srcColor)
            where TSrcPixel : unmanaged, Pixel.IReflection, Pixel.IConvertTo, Pixel.IValueSetter<Pixel.BGRP32>
            where TDstPixel : unmanaged, Pixel.IReflection, Pixel.IConvertTo
        {
            var premulRef = srcColor.To<Pixel.BGRP32>();
            var unpremulRef = default(TSrcPixel);
            unpremulRef.SetValue(premulRef);

            var interpolator = Pixel.GetQuantizedInterpolator<TSrcPixel, TDstPixel>();            

            var scale = 1 << interpolator.QuantizedLerpShift;

            for (uint y = 0; y < scale; y += 256)
            {
                for (uint x = 0; x < scale; x += 256)
                {
                    var result = interpolator
                        .InterpolateBilinear(srcColor, srcColor, srcColor, srcColor, x, y);

                    if (default(TDstPixel).IsPremultiplied)
                    {                        
                        Assert.AreEqual(premulRef, result);
                    }
                    else
                    {                        
                        Assert.AreEqual(unpremulRef, result);
                    }                    
                }
            }

            /*
            for (uint y = 0; y < scale; y += 256)
            {
                var result = interpolator.InterpolateLinear(srcColor, srcColor, y);

                if (default(TDstPixel).IsPremultiplied)
                {
                    var srcPremul = srcColor.To<Pixel.BGRP32>();
                    var srcResult = result.To<Pixel.BGRP32>();
                    Assert.AreEqual(srcPremul, srcResult);
                }
                else
                {
                    var srcRoundtrip = srcColor.PremulRoundtrip8();
                    Assert.AreEqual(srcRoundtrip, result);
                }
            }*/
        }


        private TDstPixel _TestLerpQuantized<TSrcPixel, TDstPixel>(TSrcPixel a, TSrcPixel b)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
            where TDstPixel : unmanaged, Pixel.IValueSetter<Pixel.BGRP32>
        {
            var ap = a.To<Pixel.BGRP32>();
            var bp = b.To<Pixel.BGRP32>();

            var rgbp = Pixel.BGRP32.Lerp(ap, bp, 8192);

            var final = default(TDstPixel);
            final.SetValue(rgbp);

            TestContext.WriteLine($"{a} ^ {b} = {final}");

            return final;
        }

        private TDstPixel _TestLerpFloating<TSrcPixel, TDstPixel>(TSrcPixel a, TSrcPixel b)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
            where TDstPixel : unmanaged, Pixel.IValueSetter<Pixel.BGRP128F>
        {
            var ap = a.To<Pixel.BGRP128F>();
            var bp = b.To<Pixel.BGRP128F>();

            // var rgbp = Pixel.BGRP128F.Lerp(ap, bp, 0.5f);
            // var final = default(TDstPixel).From(rgbp);
            // TestContext.WriteLine($"{a} ^ {b} = {final}");
            // return final;

            return default;
        }
    }
}
