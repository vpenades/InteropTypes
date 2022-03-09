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
                Assert.LessOrEqual(Math.Abs(volume - 1), BoundingSphere.SCALEDECOMPOSITIONEPSILON);
            }            
        }

        [Test]
        public void CreateSphereFromTwoPoints()
        {
            var sphere = BoundingSphere.FromPoints(new Point3[] { (-1, 0, 0), (1, 0, 0), (0,0,0) });

            Assert.AreEqual(new BoundingSphere(Point3.Zero, 1), sphere);
        }

        [Test]
        public void MergeTwoSpheres()
        {
            var sphere1 = new BoundingSphere((-2, 0, 0), 1);
            var sphere2 = new BoundingSphere((2, 0, 0), 1);
            
            var merged = BoundingSphere.Merge(sphere1,sphere2);

            Assert.AreEqual(new BoundingSphere(Point3.Zero, 3), merged);
        }

        [Test]
        public void CompareBoundingSphereTests()
        {
            var plane = new System.Numerics.Plane(System.Numerics.Vector3.UnitX, -3);

            Assert.AreEqual(-1, new BoundingSphere((-4.1f, 0, 0), 1).CompareTo(plane)); // behind plane
            Assert.AreEqual(0, new BoundingSphere((2.5f, 0, 0), 1).CompareTo(plane)); // overlapping plane
            Assert.AreEqual(1, new BoundingSphere((4.1f, 0, 0), 1).CompareTo(plane)); // over plane

            var sphere = new BoundingSphere((3, 0, 0), 1);

            Assert.AreEqual(1, new BoundingSphere((7.1f, 0, 0), 1).CompareTo(sphere)); // outside
            Assert.AreEqual(0, new BoundingSphere((2.5f, 0, 0), 1).CompareTo(sphere));  // overlapping bounds
            Assert.AreEqual(-1, new BoundingSphere((3.5f, 0, 0), 0.2f).CompareTo(sphere)); // inside
        }
    }
}
