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
            Assert.AreEqual(0x4000, FixedMathCC8.From255To16384(255));
            Assert.AreEqual(0x1fff, FixedMathCC8.From255To16384(127));
            Assert.AreEqual(8256, FixedMathCC8.From255To16384(128));

            for (uint i=0; i < 256; ++i)
            {
                var expanded = FixedMathCC8.From255To16384(i);
                var ii = FixedMathCC8.From16384To255(expanded);
                Assert.AreEqual(i,ii);
            }
        }

        [Test]
        public void TestDivMul()
        {
            for(uint i=1; i<256; ++i)
            {
                var ifp = i / 255f;

                for(uint j=1; j<256; ++j)
                {
                    var jfp = j / 255f;

                    // references
                    var dddd = Math.Min(1, ifp / jfp);

                    // var mref = (uint)((ifp * jfp) * 255f);                    
                    // var dref = (uint)( dddd * 255f);

                    var mref = (uint)Math.Round((ifp * jfp) * 255f);                    
                    var dref = (uint)Math.Round( dddd * 255f);

                    Assert.Less(mref, 256);
                    Assert.Less(dref, 256);

                    TestContext.WriteLine($"{i}x{j} = {mref}");
                    TestContext.WriteLine($"{i}/{j} = {dref}");

                    // slow
                    var mval0 = (i * j) / 255;
                    var dval0 = Math.Min(255, (i * 255) / j);
                    Assert.Less(dval0, 256);

                    Assert.AreEqual(mref, mval0, 1);
                    Assert.AreEqual(dref, dval0, 1);

                    // expanded 65535

                    const uint U16 = 65535;                    

                    var i16 = FixedMathCC8.From255To65535(i);
                    var j16 = FixedMathCC8.From255To65535(j);

                    var mval1 = (i16 * j16) >> 16;
                    var dval1 = Math.Min(U16, (i16 * U16) / j16);

                    mval1 = FixedMathCC8.From65535To255(mval1);
                    dval1 = FixedMathCC8.From65535To255(dval1);

                    Assert.AreEqual(mref, mval1, 1);
                    Assert.AreEqual(dref, dval1, 1);

                    // reciprocal 65535

                    var rj15 = Math.Min(U16, (U16 * U16) / j16);

                    var dval2 = (i16 * rj15) >> 16;                    

                    // expanded to 4095

                    const uint U12 = (1 << 12) - 1;

                    var i12 = FixedMathCC8.FromByte(i);
                    var j12 = FixedMathCC8.FromByte(j);

                    var mval4 = (i12 * j12) >> 12;
                    var dval4 = Math.Min(U12, (i12 * U12) / j12);

                    mval4 = FixedMathCC8.ToByte(mval4);
                    dval4 = FixedMathCC8.ToByte(dval4);

                    Assert.AreEqual(mref, mval4, 1);
                    Assert.AreEqual(dref, dval4, 1);

                    // reciprocal direct to 255 (12+12+8)

                    var rj12 = FixedMathCC8.ToReciprocalByte(j12);
                    dval4 = FixedMathCC8.ToByte(i12 ,rj12);

                    Assert.AreEqual(dref, dval4, 1);
                }
            }
        }




        [Test]
        public void TestMultiplication()
        {
            var kmul = FixedMathCC8.FromByte(255) * FixedMathCC8.FromByte(255) >> FixedMathCC8.UnitShift;
            var final = FixedMathCC8.ToByte(kmul);
        }

        [Test]
        public void TestDivision()
        {
            int failed = 0;

            for (byte color = 255; color > 0; color--)
            {
                var kcolor = FixedMathCC8.FromByte(color);

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
            var rcpa = FixedMathCC8.ToReciprocalByte(FixedMathCC8.FromByte(alpha));            

            var final = FixedMathCC8.ToByte(FixedMathCC8.FromByte(color), rcpa);

            Assert.AreEqual(color, final);            
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
            Assert.AreEqual(q1.A, 2039);

            

            q1.SetValue(new Pixel.BGRA32(255, 127, 63, 255));            
            Assert.AreEqual(q1.G, 2038);
            Assert.AreEqual(q1.B, 1010);
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
                Assert.AreEqual(q1.RQ8, rp.R, 255 - rp.A);
                Assert.AreEqual(q1.GQ8, rp.G, 255 - rp.A);
                Assert.AreEqual(q1.BQ8, rp.B, 255 - rp.A);

                var rgba = default(Pixel.BGRA32.Writeable); rgba.SetValue(q1);
                Assert.AreEqual(rgba.A, rp.A);
                Assert.AreEqual(rgba.R, rp.R, 255 - rp.A);
                Assert.AreEqual(rgba.G, rp.G, 255 - rp.A);
                Assert.AreEqual(rgba.B, rp.B, 255 - rp.A);
            }

        }
    }
}
