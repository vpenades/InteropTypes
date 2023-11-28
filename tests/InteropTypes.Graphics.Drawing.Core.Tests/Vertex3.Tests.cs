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
            Assert.That(System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vertex3)), Is.EqualTo(12 +4+8));

            Assert.That(new ColorStyle(uint.MaxValue), Is.EqualTo(new ColorStyle(-1)));
        }
    }
}
