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
        public void Vertex2Tests()
        {
            Assert.That(System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vertex2)), Is.EqualTo(8 +4+8));
        }
    }
}
