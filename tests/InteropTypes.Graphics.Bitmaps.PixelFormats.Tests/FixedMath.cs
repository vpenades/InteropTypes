﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{
    internal class FixedMathTests
    {
        [Test]
        public void Test255To16384()
        {
            Assert.That(FixedMathCC8.From255To16384(255), Is.EqualTo(0x4000));
            Assert.That(FixedMathCC8.From255To16384(127), Is.EqualTo(0x1fff));
            Assert.That(FixedMathCC8.From255To16384(128), Is.EqualTo(8256));

            for (uint i=0; i < 256; ++i)
            {
                var expanded = FixedMathCC8.From255To16384(i);
                var ii = FixedMathCC8.From16384To255(expanded);
                Assert.That(ii, Is.EqualTo(i));
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

                    Assert.That(mref, Is.LessThan(256));
                    Assert.That(dref, Is.LessThan(256));

                    TestContext.Out.WriteLine($"{i}x{j} = {mref}");
                    TestContext.Out.WriteLine($"{i}/{j} = {dref}");

                    // slow
                    var mval0 = (i * j) / 255;
                    var dval0 = Math.Min(255, (i * 255) / j);
                    Assert.That(dval0, Is.LessThan(256));

                    Assert.That(mval0, Is.EqualTo(mref).Within(1));
                    Assert.That(dval0, Is.EqualTo(dref).Within(1));

                    // expanded 65535

                    const uint U16 = 65535;                    

                    var i16 = FixedMathCC8.From255To65535(i);
                    var j16 = FixedMathCC8.From255To65535(j);

                    var mval1 = (i16 * j16) >> 16;
                    var dval1 = Math.Min(U16, (i16 * U16) / j16);

                    mval1 = FixedMathCC8.From65535To255(mval1);
                    dval1 = FixedMathCC8.From65535To255(dval1);

                    Assert.That(mval1, Is.EqualTo(mref).Within(1));
                    Assert.That(dval1, Is.EqualTo(dref).Within(1));

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

                    Assert.That(mval4, Is.EqualTo(mref).Within(1));
                    Assert.That(dval4, Is.EqualTo(dref).Within(1));

                    // reciprocal direct to 255 (12+12+8)

                    var rj12 = FixedMathCC8.ToReciprocalByte(j12);
                    dval4 = FixedMathCC8.ToByte(i12 ,rj12);

                    Assert.That(dval4, Is.EqualTo(dref).Within(1));
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

                    // Assert.That(color, Is.EqualTo(kmul));

                    if (kmul != color) TestContext.Out.WriteLine($"{color},{alpha} => {kmul}");
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

            Assert.That(final, Is.EqualTo(color));            
        }        
    }
}
