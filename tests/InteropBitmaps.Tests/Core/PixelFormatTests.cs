using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps
{
    [Category("Core")]
    public class PixelFormatTests
    {
        private static readonly ComponentFormat[] _Bits1 = new[] { ComponentFormat.Alpha1, ComponentFormat.Undefined1 };
        private static readonly ComponentFormat[] _Bits4 = new[] { ComponentFormat.Alpha4, ComponentFormat.Red4, ComponentFormat.Green4, ComponentFormat.Blue4, ComponentFormat.Undefined4 };
        private static readonly ComponentFormat[] _Bits5 = new[] { ComponentFormat.Red5, ComponentFormat.Green5, ComponentFormat.Blue5, ComponentFormat.Undefined5 };
        private static readonly ComponentFormat[] _Bits6 = new[] { ComponentFormat.Green6, ComponentFormat.Undefined6 };


        [Test]
        public void ComponentFormatEnumeration()
        {
            Assert.AreEqual(0, (int)ComponentFormat.Empty);

            var values = Enum.GetValues(typeof(ComponentFormat))
                .Cast<ComponentFormat>()
                .ToArray();

            foreach(var c in values)
            {
                var name = c.ToString();

                var len = PixelFormat._GetBitLen(c);

                if (c == ComponentFormat.Empty) Assert.AreEqual(0, len);
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

            // todo: check the index of any "bit" group is less than the next group.
        }
    }
}
