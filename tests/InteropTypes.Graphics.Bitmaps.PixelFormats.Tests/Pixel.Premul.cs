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

            // TestPremultiply<Pixel.BGR565>();
            TestPremultiply<Pixel.BGR24>();
            TestPremultiply<Pixel.RGB24>();            
            TestPremultiply<Pixel.BGRA32>();
            TestPremultiply<Pixel.RGBA32>();
            TestPremultiply<Pixel.ARGB32>();
        }

        public static void TestPremultiply<TPixel>()
            where TPixel : unmanaged
            , Pixel.IConvertTo            
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
            , Pixel.IConvertTo            
        {
            // color

            var src = new Pixel.BGRA32(r, 1, 255, a);

            var color = default(TPixel);
            src.CopyTo(ref color);

            // references

            var premulRef0 = color.GetReferenceBGRP32();
            var rndtrpRef0 = premulRef0.GetReferenceBGRP32<TPixel>();

            // premul

            var premul1 = color.To<Pixel.BGRP32>();

            var premul2 = Pixel.BGRP32.From(color);

            var premul3 = default(Pixel.BGRP32);
            color.CopyTo(ref premul3);

            Assert.That(premul1, Is.EqualTo(premulRef0));
            Assert.That(premul2, Is.EqualTo(premulRef0));
            Assert.That(premul3, Is.EqualTo(premulRef0));

            // unpremul

            TPixel color1 = default;
            premul1.CopyTo(ref color1);

            var color2 = premul1.To<TPixel>();

            Assert.That(color1, Is.EqualTo(rndtrpRef0));
            Assert.That(color2, Is.EqualTo(rndtrpRef0));            
        }
    }
}
