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
            Assert.That(System.Runtime.InteropServices.Marshal.SizeOf(typeof(Segment3)), Is.EqualTo(12 * 2));

            var a = (0, 0, 0);
            var b = (10, 0, 0);

            Assert.That(Segment3.Create(a, b).DominantAxis, Is.EqualTo(0));
            Assert.That(Segment3.Create(b, a).DominantAxis, Is.EqualTo(0));
            Assert.That(Segment3.CreateOrdinal(b, a), Is.EqualTo(Segment3.CreateOrdinal(a, b)));            
        }

        [Test]
        public void CompareSegment3Tests()
        {
            var segment = Segment3.Create((-10, 0, 0), (10, 0, 0));

            Assert.That(segment.CompareTo(new BoundingSphere((0, 2, 0), 1)), Is.EqualTo(1));
            Assert.That(segment.CompareTo(new BoundingSphere((0, 1, 0), 1)), Is.EqualTo(0));
            Assert.That(segment.CompareTo(new BoundingSphere((0, 0, 0), 12)), Is.EqualTo(-1));

        }
    }
}
