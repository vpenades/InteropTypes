using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{
    using PEF = PixelComponentID;

    public class ComponentsTests
    {
        [Test]
        public void TestComponentMemoryFootprint()
        {
            Assert.That(System.Runtime.InteropServices.Marshal.SizeOf(typeof(PixelComponent)), Is.EqualTo(1));            
        }

        [Test]
        public void TestComponentID()
        {
            #if DEBUG // requires debug mode

            Assert.That(default(PixelComponent).Id, Is.EqualTo(PEF.Empty));

            // PixelFormat._GetBitLen(c);

            Assert.That((int)PEF.Empty, Is.EqualTo(0));

            var values = Enum.GetValues(typeof(PEF))
                .Cast<PEF>()
                .ToArray();

            // all values of ElementId must be contained within 0 and 255 to fit in 1 byte
            Assert.That(values.Select(item => (int)item).Min(), Is.LessThanOrEqualTo(0));
            Assert.That(values.Select(item => (int)item).Max(), Is.GreaterThanOrEqualTo(255));

            int lastLen = -1;

            // check name suffix
            foreach (var c in values)
            {
                var name = c.ToString();
                var element = new PixelComponent(c);

                var blen = element.BitCount;

                if (c == PEF.Empty) { Assert.That(blen, Is.EqualTo(0)); continue; }

                Assert.That(blen, Is.GreaterThan(0));

                var xlen = -1;
                var xflt = false;

                if (name.EndsWith('1')) xlen = 1;
                if (name.EndsWith('3')) xlen = 3;                
                if (name.EndsWith('5')) xlen = 5;
                if (name.EndsWith('7')) xlen = 7;
                if (name.EndsWith('8')) xlen = 8;

                if (name.EndsWith("16")) xlen = 16;
                else if (name.EndsWith('6')) xlen = 6;

                if (name.EndsWith("24")) xlen = 24;
                else if (name.EndsWith('4')) xlen = 4;

                if (name.EndsWith("32")) xlen = 32;
                else if (name.EndsWith('2')) xlen = 2;


                if (name.EndsWith("32F")) { xlen = 32; xflt = true; }
                if (name.EndsWith("64F")) { xlen = 64; xflt = true; }

                Assert.That(blen, Is.EqualTo(xlen), "Reported bit length and suffix must match.");
                Assert.That(xlen, Is.GreaterThanOrEqualTo(lastLen), "Bit lengths must be declared in ascending order.");

                if (!name.StartsWith("Millimeter"))
                {
                    Assert.That(element.IsFloating, Is.EqualTo(xflt), "Reported bit length and suffix must match.");
                }                

                lastLen = xlen;
            }

            #endif            
        }
    }
}
