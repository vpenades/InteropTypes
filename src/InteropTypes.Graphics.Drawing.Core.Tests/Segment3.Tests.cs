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
        public void Segment3Tests()
        {
            Assert.AreEqual(12 * 2, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Segment3)));

            var a = (0, 0, 0);
            var b = (10, 0, 0);

            Assert.AreEqual(0, Segment3.Create(a, b).DominantAxis);
            Assert.AreEqual(0, Segment3.Create(b, a).DominantAxis);
            Assert.AreEqual(Segment3.CreateOrdinal(a, b), Segment3.CreateOrdinal(b, a));            
        }
    }
}
