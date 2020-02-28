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
        public void PixelFormatEnumeration()
        {
            Assert.AreEqual(0, (int)ComponentFormat.Empty);

            // todo: check the index of any "bit" group is less than the next group.
        }
    }
}
