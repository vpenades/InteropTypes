using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps
{
    using PEF = Pixel.ElementID;

    [Category("Core")]
    public class PixelFormatTests
    {
        private static readonly PEF[] _Bits1 = new[] { PEF.Undefined1, PEF.Alpha1 };
        private static readonly PEF[] _Bits4 = new[] { PEF.Undefined4, PEF.Alpha4, PEF.Red4, PEF.Green4, PEF.Blue4 };
        private static readonly PEF[] _Bits5 = new[] { PEF.Undefined5, PEF.Red5, PEF.Green5, PEF.Blue5 };
        private static readonly PEF[] _Bits6 = new[] { PEF.Undefined6, PEF.Green6 };

        [Test]
        public void TestPixelFormatStructure()
        {
            Assert.AreEqual(PEF.Empty, default(Pixel.Element).Id);

            Assert.AreEqual(1, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Pixel.Element)));
            Assert.AreEqual(4, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Pixel.Format)));
        }

        [Test]
        public void ComponentFormatEnumeration()
        {
            #if DEBUG // requires debug mode

            // PixelFormat._GetBitLen(c);

            Assert.AreEqual(0, (int)PEF.Empty);

            var values = Enum.GetValues(typeof(PEF))
                .Cast<PEF>()
                .ToArray();

            foreach(var c in values)
            {
                var name = c.ToString();

                var len = new Pixel.Element(c).BitCount;

                if (c == PEF.Empty) Assert.AreEqual(0, len);
                else Assert.Greater(len, 0);

                if (name.EndsWith("1")) Assert.AreEqual(1, len);
                if (name.EndsWith("4")) Assert.AreEqual(4, len);
                if (name.EndsWith("5")) Assert.AreEqual(5, len);
                if (name.EndsWith("8")) Assert.AreEqual(8, len);

                if (name.EndsWith("16")) Assert.AreEqual(16, len);
                else if (name.EndsWith("6")) Assert.AreEqual(6, len);

                if (name.EndsWith("32")) Assert.AreEqual(32, len);
                if (name.EndsWith("32F")) Assert.AreEqual(32, len);
            }

            #endif

            // todo: check the index of any "bit" group is less than the next group.
        }
    }
}
