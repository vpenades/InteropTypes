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

        [Test]
        public void SerializeQuantized()
        {
            var data = new Queue<Byte>();

            var p1 = new Point3(5, -37, 47);
            
            Point3.SerializeQuantizedDirectionLength(data.Enqueue, p1);
            Point3.DeserializeQuantizedDirectionLength(data.Dequeue, out var p2);
            Assert.AreEqual(0, data.Count);

            Assert.AreEqual(p1.X, p2.X, 0.5f);
            Assert.AreEqual(p1.Y, p2.Y, 0.5f);
            Assert.AreEqual(p1.Z, p2.Z, 0.5f);

            Point3.SerializeQuantizedScaled(data.Enqueue, p1);
            Point3.DeserializeQuantizedScaled(data.Dequeue, out p2);
            Assert.AreEqual(0, data.Count);

            Assert.AreEqual(p1.X, p2.X);
            Assert.AreEqual(p1.Y, p2.Y);
            Assert.AreEqual(p1.Z, p2.Z);
        }
    }
}
