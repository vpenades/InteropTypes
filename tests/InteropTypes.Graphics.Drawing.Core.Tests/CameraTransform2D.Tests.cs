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
            Assert.That(p1, Is.EqualTo(Point2.Zero));

            p1 = Vector2.Transform(Vector2.Zero, cam2.CreateFinalMatrix((1000, 1000)));
            Assert.That(p1, Is.EqualTo(new Point2(0, 1000)));

            p1 = Vector2.Transform(Vector2.Zero, cam1.CreateFinalMatrix((1000, 500)));
            Assert.That(p1, Is.EqualTo(new Point2(250, 0)));
        }

        [Test]
        public void CameraOuterBoundsTests()
        {
            // create physical viewport
            var backend = new DummyBackend2D(2000, 500);

            // virtual camera
            var innerCamera = CameraTransform2D.Create(Matrix3x2.CreateTranslation(700,0), (500, -500));

            // get virtual viewport
            Assert.That(Transforms.Canvas2DTransform.TryCreate(backend, innerCamera, out var virtualCanvas));

            // draw onto the virtual canvas
            virtualCanvas.DrawRectangle((2, 2), (1, 1), System.Drawing.Color.Red);

            // check bounds in the physical backend
            var (bMin, bMax) = backend.BoundingBox;
            Assert.That(bMax.X, Is.EqualTo(53));
            Assert.That(bMax.Y, Is.EqualTo(498));
            bMax -= bMin;
            Assert.That(bMax.X, Is.EqualTo(1));
            Assert.That(bMax.Y, Is.EqualTo(1));

            // check if we can retrieve the outer camera bounds
            Assert.That(CameraTransform2D.TryGetOuterCamera(virtualCanvas, out var outerCamera));

            var outerRect = outerCamera.GetOuterBoundingRect();
            Assert.That(outerRect.X, Is.EqualTo(-50));
            Assert.That(outerRect.Y, Is.EqualTo(0));
            Assert.That(outerRect.Width, Is.EqualTo(2000));
            Assert.That(outerRect.Height, Is.EqualTo(500));
        }        
    }
}
