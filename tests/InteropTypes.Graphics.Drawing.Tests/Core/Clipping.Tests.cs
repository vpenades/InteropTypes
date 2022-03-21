using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Drawing.Transforms;

using NUnit.Framework;

namespace InteropTypes.Graphics.Drawing
{
    internal class ClippingTests
    {
        [Test]
        public void TestPolygonClipping()
        {
            var saw = new[] { Vector3.Zero, new Vector3(1, 5, 0), new Vector3(2, 1, 0), new Vector3(3, 5, 0), new Vector3(4, 0, 0) };

            Span<Vector3> sawOut1 = stackalloc Vector3[saw.Length * 2];

            var l = new Plane(Vector3.UnitY, 1).ClipPolygonToPlane(sawOut1, saw);
            Assert.AreEqual(5, l);

            l = new Plane(Vector3.UnitY, -2).ClipPolygonToPlane(sawOut1, saw);
            Assert.AreEqual(6, l);

            l = new Plane(-Vector3.UnitY, 2).ClipPolygonToPlane(sawOut1, saw);
            Assert.AreEqual(7, l);
        }

        [Test]
        public void TestPolygonClipping2()
        {
            var p = new Plane(new System.Numerics.Vector3(0, 0, 1), -0.1f);

            _Clip(p, (3.6963348f, -0.17063361f, 32.588318f), (3.6963348f, 0.35697514f, -68.17659f));
        }


        private static void _Clip(Plane plane, Point3 a, Point3 b)
        {
            var aa = a.XYZ;
            var bb = b.XYZ;

            Parametric.PolygonClipper3.ClipLineToPlane(ref aa, ref bb, plane);

            var u = Plane.DotCoordinate(plane, aa);
            Assert.IsTrue(u >= -0.001f);

            u = Plane.DotCoordinate(plane, bb);
            Assert.IsTrue(u >= -0.001f);
        }

        [TestCase("Scene1")]
        [TestCase("Thunderbird1")]
        public void TestClipScene3D(string sceneName)
        {
            TestContext.CurrentContext.AttachShowDirLink();

            var srcScene = SceneFactory.CreateRecord3D(sceneName);

            var bounds = srcScene.BoundingMatrix;                                  

            for (int i = -1; i <= 1; ++i)
            {
                var dstScene = new Record3D();

                srcScene.DrawTo(new PlaneClip3D(dstScene, new Plane(Vector3.UnitX, i)));

                bounds = dstScene.BoundingMatrix;

                dstScene.AttachToCurrentTest($"{sceneName}_{i}.glb");
                dstScene.AttachToCurrentTest($"{sceneName}_{i}.html");
            }
        }
    }
}
