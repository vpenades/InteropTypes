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
            Assert.That(System.Runtime.InteropServices.Marshal.SizeOf(typeof(Point3)), Is.EqualTo(12));

            Assert.That(new Point3(-3, 0, 0).DominantAxis, Is.EqualTo(0));
            Assert.That(new Point3(0, -3, 0).DominantAxis, Is.EqualTo(1));
            Assert.That(new Point3(0, 0, -3).DominantAxis, Is.EqualTo(2));
        }

        [Test]
        public void ComparePoint3Tests()
        {
            var sphere = new BoundingSphere((-5, 0, 0), 1);

            Assert.That(new Point3(-5, 0, 0).CompareTo(sphere), Is.EqualTo(-1)); // inside
            Assert.That(new Point3(-5, 1, 0).CompareTo(sphere), Is.EqualTo(0)); // at boundary
            Assert.That(new Point3(-5, 2, 0).CompareTo(sphere), Is.EqualTo(1)); // outside
        }

        /*
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
        }*/
    }
}
