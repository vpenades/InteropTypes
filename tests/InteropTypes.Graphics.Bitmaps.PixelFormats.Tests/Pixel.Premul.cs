using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{
    internal class PremultiplyTests
    {
        [Test]
        public void TestPremul()
        {
            TestPremultiply<Pixel.BGRA32>(1,0);

            TestPremultiply<Pixel.BGR24>();
            TestPremultiply<Pixel.RGB24>();
            TestPremultiply<Pixel.BGRA32>();
            TestPremultiply<Pixel.RGBA32>();
            TestPremultiply<Pixel.ARGB32>();
        }

        public static void TestPremultiply<TPixel>()
            where TPixel : unmanaged
            , Pixel.IValueSetter<Pixel.BGRA32>
            , Pixel.IValueGetter<Pixel.BGRP32>
            , Pixel.ICopyValueTo<Pixel.BGRP32>
        {
            for(int a=0; a < 256; ++a)
            {
                for(int r=0; r <256; ++r)
                {
                    TestPremultiply<TPixel>(a, r);
                }
            }
        }

        private static void TestPremultiply<TPixel>(int a, int r)
            where TPixel : unmanaged
            , Pixel.IValueSetter<Pixel.BGRA32>
            , Pixel.IValueGetter<Pixel.BGRP32>
            , Pixel.ICopyValueTo<Pixel.BGRP32>
        {
            var src = new Pixel.BGRA32(r, 1, 255, a);
            var tmp = default(TPixel);
            tmp.SetValue(src);

            var r0 = tmp.GetReferenceBGRP32();

            var r1 = tmp.GetValue();

            var r2 = default(Pixel.BGRP32);
            tmp.CopyTo(ref r2);

            Assert.AreEqual(r0, r1);
            Assert.AreEqual(r0, r2);
        }
    }
}
