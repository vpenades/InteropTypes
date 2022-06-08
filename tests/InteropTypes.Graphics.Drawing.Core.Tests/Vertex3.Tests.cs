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
        public void Vertex3Tests()
        {
            Assert.AreEqual(12+4+8, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vertex3)));

            Assert.AreEqual(new ColorStyle(-1), new ColorStyle(uint.MaxValue));
        }
    }
}
