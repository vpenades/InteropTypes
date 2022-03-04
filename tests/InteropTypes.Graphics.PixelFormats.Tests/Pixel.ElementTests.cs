using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics
{
    using PEF = PixelComponentID;

    public class ComponentsTests
    {
        [Test]
        public void TestComponentMemoryFootprint()
        {
            Assert.AreEqual(1, System.Runtime.InteropServices.Marshal.SizeOf(typeof(PixelComponent)));            
        }

        [Test]
        public void TestComponentID()
        {
            #if DEBUG // requires debug mode

            Assert.AreEqual(PEF.Empty, default(PixelComponent).Id);

            // PixelFormat._GetBitLen(c);

            Assert.AreEqual(0, (int)PEF.Empty);

            var values = Enum.GetValues(typeof(PEF))
                .Cast<PEF>()
                .ToArray();

            // all values of ElementId must be contained within 0 and 255 to fit in 1 byte
            Assert.LessOrEqual(0, values.Select(item => (int)item).Min());
            Assert.GreaterOrEqual(255, values.Select(item => (int)item).Max());

            int lastLen = -1;

            // check name suffix
            foreach (var c in values)
            {
                var name = c.ToString();
                var element = new PixelComponent(c);

                var blen = element.BitCount;

                if (c == PEF.Empty) { Assert.AreEqual(0, blen); continue; }

                Assert.Greater(blen, 0);

                var xlen = -1;
                var xflt = false;

                if (name.EndsWith("1")) xlen = 1;
                if (name.EndsWith("3")) xlen = 3;                
                if (name.EndsWith("5")) xlen = 5;
                if (name.EndsWith("7")) xlen = 7;
                if (name.EndsWith("8")) xlen = 8;

                if (name.EndsWith("16")) xlen = 16;
                else if (name.EndsWith("6")) xlen = 6;

                if (name.EndsWith("24")) xlen = 24;
                else if (name.EndsWith("4")) xlen = 4;

                if (name.EndsWith("32")) xlen = 32;
                else if (name.EndsWith("2")) xlen = 2;


                if (name.EndsWith("32F")) { xlen = 32; xflt = true; }
                if (name.EndsWith("64F")) { xlen = 64; xflt = true; }

                Assert.AreEqual(xlen, blen, "Reported bit length and suffix must match.");
                Assert.GreaterOrEqual(xlen, lastLen, "Bit lengths must be declared in ascending order.");

                if (!name.StartsWith("Millimeter"))
                {
                    Assert.AreEqual(xflt, element.IsFloating, "Reported bit length and suffix must match.");
                }                

                lastLen = xlen;
            }

            #endif            
        }
    }
}
