using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Drawing
{
    internal partial class TypeTests
    {
        [Test]
        public void ColorStyleTests()
        {
            Assert.AreEqual(4, System.Runtime.InteropServices.Marshal.SizeOf(typeof(ColorStyle)));
        }
    }
}
