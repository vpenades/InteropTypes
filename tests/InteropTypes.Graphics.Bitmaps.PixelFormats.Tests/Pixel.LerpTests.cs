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
            Assert.AreEqual(2139045663, Pixel.BGRP32.Lerp(RGBA32_A, RGBA32_B, 8192).GetHashCode());
            Assert.AreEqual(2139045663, RGBA32_A.LerpToBGRP32(RGBA32_B, 8192).GetHashCode());

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
        public void TestBicubicLerp()
        {
            Pixel.RGBA32.LerpBGRP32(RGBA32_A, RGBA32_A, RGBA32_A, RGBA32_A, 100, 700);

        }


        private TDstPixel _TestLerpQuantized<TSrcPixel, TDstPixel>(TSrcPixel a, TSrcPixel b)
            where TSrcPixel : unmanaged, Pixel.IValueGetter<Pixel.BGRP32>
            where TDstPixel : unmanaged, Pixel.IValueSetter<Pixel.BGRP32>
        {
            var ap = a.GetValue();
            var bp = b.GetValue();

            var rgbp = Pixel.BGRP32.Lerp(ap, bp, 8192);

            var final = default(TDstPixel);
            final.SetValue(rgbp);

            TestContext.WriteLine($"{a} ^ {b} = {final}");

            return final;
        }

        private TDstPixel _TestLerpFloating<TSrcPixel, TDstPixel>(TSrcPixel a, TSrcPixel b)
            where TSrcPixel : unmanaged, Pixel.IValueGetter<Pixel.BGRP128F>
            where TDstPixel : unmanaged, Pixel.IValueSetter<Pixel.BGRP128F>
        {
            var ap = a.GetValue();
            var bp = b.GetValue();

            // var rgbp = Pixel.BGRP128F.Lerp(ap, bp, 0.5f);
            // var final = default(TDstPixel).From(rgbp);
            // TestContext.WriteLine($"{a} ^ {b} = {final}");
            // return final;

            return default;
        }
    }
}
