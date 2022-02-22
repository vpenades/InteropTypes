using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Drawing
{
    internal partial class TypeTests
    {

        [Test]
        public void CameraTransform2DTests()
        {
            var cam1 = new CameraTransform2D(Matrix3x2.Identity, (500, 500));     // Top-Down
            var cam2 = new CameraTransform2D(Matrix3x2.Identity, (500, -500));    // Bottom-Top

            Point2 p1 = Vector2.Transform(Vector2.Zero, cam1.CreateFinalMatrix((500, 500)));
            Assert.AreEqual(Point2.Zero, p1);
            p1 = Vector2.Transform(Vector2.Zero, cam2.CreateFinalMatrix((500, 500)));
            Assert.AreEqual(new Point2(0,500), p1);

            p1 = Vector2.Transform(Vector2.Zero, cam1.CreateFinalMatrix((1000, 500)));
            Assert.AreEqual(new Point2(250,0), p1);
        }
    }
}
