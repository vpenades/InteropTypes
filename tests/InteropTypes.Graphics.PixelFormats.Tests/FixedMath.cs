using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps
{
    internal class FixedMathTests
    {
        [Test]
        public void Test255To16384()
        {
            Assert.AreEqual(0x4000, FixedMath.From255To16384(255));
            Assert.AreEqual(0x1fff, FixedMath.From255To16384(127));
            Assert.AreEqual(8256, FixedMath.From255To16384(128));

            for (uint i=0; i < 256; ++i)
            {
                var expanded = FixedMath.From255To16384(i);
                var ii = FixedMath.From16384To255(expanded);
                Assert.AreEqual(i,ii);
            }
        }

        [Test]
        public void TestMultiplication()
        {
            var kmul = FixedMath.From255(255) * FixedMath.From255(255) >> FixedMath.UnitShift;
            var final = FixedMath.To255(kmul);
        }

        [Test]
        public void TestDivision()
        {
            int failed = 0;

            for (byte color = 255; color > 0; color--)
            {
                var kcolor = FixedMath.From255(color);

                for (byte alpha = 255; alpha > 0; alpha--)
                {
                    //uint kalpha = Quantized16384.From255(alpha);
                    uint kalpha = alpha;
                    kalpha <<= 8;

                    uint dividend = 0xffff0000;
                    
                    uint rcpa = dividend / kalpha;

                    var kmul = (rcpa * kcolor);

                    kmul >>= 16;

                    // Assert.AreEqual(kmul, color);

                    if (kmul != color) TestContext.WriteLine($"{color},{alpha} => {kmul}");
                }
            }
        }

        [TestCase(255, 255)]
        [TestCase(127, 255)]
        [TestCase(128, 255)]
        [TestCase(255, 127)]
        [TestCase(255, 128)]
        public void TestReciprocal(byte color, byte alpha)
        {
            

            var rcpa = FixedMath.GetUnboundedReciprocal8(FixedMath.From255(alpha));            

            var final = FixedMath.To255(FixedMath.From255(color), rcpa);

            Assert.AreEqual(color, final);            
        }

        [Test]
        public void TestQuantized16384()
        {
            uint v = 255;
            v <<= (FixedMath.UnitShift + FixedMath.RcpShift8);
            v >>= (FixedMath.UnitShift + FixedMath.RcpShift8);
            Assert.AreEqual(255, v);            

            for (uint i =0; i < 256; ++i)
            {
                var q = FixedMath.From255(i);                

                Assert.LessOrEqual(q, FixedMath.UnitValue);

                var ii = FixedMath.To255(q);
                Assert.AreEqual(i, ii);
            }
        }



        [Test]
        public void QuantizedMath()
        {


            var q1 = default(Pixel.QVectorBGRP);

            q1.SetValue(new Pixel.BGRA32(255,255,255,255));            
            Assert.AreEqual(q1.AQ8, 255);
            Assert.AreEqual(q1.RQ8, 255);
            Assert.AreEqual(q1.GQ8, 255);
            Assert.AreEqual(q1.BQ8, 255);

            q1.SetValue(new Pixel.BGRA32(255, 255, 255, 127));
            Assert.AreEqual(q1.A, 0x1fff);

            

            q1.SetValue(new Pixel.BGRA32(255, 127, 63, 255));            
            Assert.AreEqual(q1.G, 0x1fff);
            Assert.AreEqual(q1.B, 0xfff);
            Assert.AreEqual(q1.AQ8, 255);
            Assert.AreEqual(q1.RQ8, 255);
            Assert.AreEqual(q1.GQ8, 127);
            Assert.AreEqual(q1.BQ8, 63);

            for (uint i = 1; i < 256; ++i)
            {
                var ra = new Pixel.BGRA32(255, 127, 63, (Byte)i);                
                var rp = new Pixel.BGRA32(new Pixel.BGRP32(ra));

                q1.SetValue(ra);                
                Assert.AreEqual(q1.AQ8, rp.A);
                Assert.AreEqual(q1.RQ8, rp.R);
                Assert.AreEqual(q1.GQ8, rp.G);
                Assert.AreEqual(q1.BQ8, rp.B);

                var rgba = default(Pixel.BGRA32.Writeable); rgba.SetValue(q1);
                Assert.AreEqual(rgba.A, rp.A);
                Assert.AreEqual(rgba.R, rp.R);
                Assert.AreEqual(rgba.G, rp.G);
                Assert.AreEqual(rgba.B, rp.B);
            }

        }
    }
}
