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
        public void CheckBoundingSphereScaleDecompositionEpsilon()
        {
            for(int i=0; i < 3; ++i)
            {
                var matrix = System.Numerics.Matrix4x4.CreateFromYawPitchRoll(1 + i, -i, i * 2);

                for (int j = 0; j < 2; ++j)
                {
                    matrix *= System.Numerics.Matrix4x4.CreateFromYawPitchRoll(-1, i, j);
                    matrix *= System.Numerics.Matrix4x4.CreateFromYawPitchRoll(-i, j, i * 2);
                }

                var det = matrix.GetDeterminant();
                var volume = Math.Abs(det);
                Assert.That(Math.Abs(volume - 1), Is.LessThanOrEqualTo(BoundingSphere.SCALEDECOMPOSITIONEPSILON));
            }            
        }

        [Test]
        public void CreateSphereFromDrawable()
        {
            var spheres = RandomSpheres.CreateRandom(new Random(117));
            var bounds = BoundingSphere.From(spheres);

            Assert.That(Point3.AreEqual(bounds.Center, (5.929292f, 3.881005f, 4.277648f), 0.00001f));
            Assert.That(bounds.Radius, Is.EqualTo(7.21709871f).Within(0.000001f));
        }

        [Test]
        public void CreateSphereFromTwoPoints()
        {
            var sphere = BoundingSphere.FromPoints(new Point3[] { (-1, 0, 0), (1, 0, 0), (0,0,0) });

            Assert.That(sphere, Is.EqualTo(new BoundingSphere(Point3.Zero, 1)));
        }

        [Test]
        public void MergeTwoSpheres()
        {
            var sphere1 = new BoundingSphere((-2, 0, 0), 1);
            var sphere2 = new BoundingSphere((2, 0, 0), 1);
            
            var merged = BoundingSphere.Merge(sphere1,sphere2);

            Assert.That(merged, Is.EqualTo(new BoundingSphere(Point3.Zero, 3)));
        }

        [Test]
        public void CompareBoundingSphereTests()
        {
            var plane = new System.Numerics.Plane(System.Numerics.Vector3.UnitX, -3);

            Assert.That(new BoundingSphere((-4.1f, 0, 0), 1).CompareTo(plane), Is.EqualTo(-1)); // behind plane
            Assert.That(new BoundingSphere((2.5f, 0, 0), 1).CompareTo(plane), Is.Zero); // overlapping plane
            Assert.That(new BoundingSphere((4.1f, 0, 0), 1).CompareTo(plane), Is.EqualTo(1)); // over plane

            var sphere = new BoundingSphere((3, 0, 0), 1);

            Assert.That(new BoundingSphere((7.1f, 0, 0), 1).CompareTo(sphere), Is.EqualTo(1)); // outside
            Assert.That(new BoundingSphere((2.5f, 0, 0), 1).CompareTo(sphere), Is.Zero);  // overlapping bounds
            Assert.That(new BoundingSphere((3.5f, 0, 0), 0.2f).CompareTo(sphere), Is.EqualTo(-1)); // inside
        }
    }
}
