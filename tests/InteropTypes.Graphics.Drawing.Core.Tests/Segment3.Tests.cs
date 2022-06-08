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

        [Test]
        public void CompareSegment3Tests()
        {
            var segment = Segment3.Create((-10, 0, 0), (10, 0, 0));

            Assert.AreEqual(1, segment.CompareTo(new BoundingSphere((0, 2, 0), 1)));
            Assert.AreEqual(0, segment.CompareTo(new BoundingSphere((0, 1, 0), 1)));            
            Assert.AreEqual(-1, segment.CompareTo(new BoundingSphere((0, 0, 0), 12)));

        }
    }
}
