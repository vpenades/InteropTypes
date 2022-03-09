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
        public void Point3Tests()
        {
            Assert.AreEqual(12, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Point3)));

            Assert.AreEqual(0, new Point3(-3, 0, 0).DominantAxis);
            Assert.AreEqual(1, new Point3(0, -3, 0).DominantAxis);
            Assert.AreEqual(2, new Point3(0, 0, -3).DominantAxis);
        }

        [Test]
        public void ComparePoint3Tests()
        {
            var sphere = new BoundingSphere((-5, 0, 0), 1);

            Assert.AreEqual(-1, new Point3(-5, 0, 0).CompareTo(sphere)); // inside
            Assert.AreEqual(0, new Point3(-5, 1, 0).CompareTo(sphere)); // at boundary
            Assert.AreEqual(1, new Point3(-5, 2, 0).CompareTo(sphere)); // outside
        }
    }
}
