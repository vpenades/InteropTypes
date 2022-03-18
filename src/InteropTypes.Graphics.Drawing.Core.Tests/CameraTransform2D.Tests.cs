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
        public void MatrixFromVectors()
        {
            var xform = Matrix3x2.CreateRotation(1) * Matrix3x2.CreateTranslation(5, 5);

            Span<Vector2> points = stackalloc Vector2[3];
            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(10, 0);
            points[2] = new Vector2(0, 10);
            for(int i = 0; i < 3; i++) points[i] = Vector2.Transform(points[i], xform);

            var dx = points[1] - points[0];
            var dy = points[2] - points[0];
            var ww = dx.Length();
            var hh = dy.Length();

            dx = Vector2.Normalize(dx);
            dy = Vector2.Normalize(dy);

            var m = new Matrix3x2(dx.X, dx.Y, dy.X, dy.Y, points[0].X, points[0].Y);
        }


        [Test]
        public void CameraTransform2DTests()
        {
            var cam1 = CameraTransform2D.Create(Matrix3x2.Identity, (500, 500));     // Top-Down
            var cam2 = CameraTransform2D.Create(Matrix3x2.Identity, (500, -500));    // Bottom-Top

            Point2 p1 = Vector2.Transform(Vector2.Zero, cam1.CreateFinalMatrix((500, 500)));
            Assert.AreEqual(Point2.Zero, p1);

            p1 = Vector2.Transform(Vector2.Zero, cam2.CreateFinalMatrix((1000, 1000)));
            Assert.AreEqual(new Point2(0, 1000), p1);

            p1 = Vector2.Transform(Vector2.Zero, cam1.CreateFinalMatrix((1000, 500)));
            Assert.AreEqual(new Point2(250, 0), p1);
        }
        
        
        
    }
}
