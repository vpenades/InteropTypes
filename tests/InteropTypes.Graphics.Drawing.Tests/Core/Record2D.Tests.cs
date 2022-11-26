using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
 
    [Category("InteropDrawing Core")]
    public class Model2DTests
    {
        [Test]
        public void CreateModel2D()
        {
            var mdl = new Record2D();
            mdl.DrawLine((1, 2), (3, 4), 5, COLOR.Red);
            mdl.DrawCircle((4, 5), 6, COLOR.Blue);
            mdl.DrawPolygon(COLOR.Gray, (1, 1), (2, 2), (3, 3));
            mdl.DrawAsset(System.Numerics.Matrix3x2.Identity, "Hello");
            // mdl.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(100, 100), new ImageSource(string.Empty, (0, 0), (10, 10), (5, 5)));
            // mdl.DrawTextLine((0, 0), "hello world", 20, COLOR.Red);

            var rect = mdl.BoundingRect;

            var lines = mdl.ToLog().ToArray();

            // mdl.DrawRectangle(rect, (COLOR.Red, 1));

            mdl.AttachToCurrentTest("result.svg");
            mdl.AttachToCurrentTest("result.png");
        }

        [Test]
        public void ComputeBoundingBox()
        {
            var mdl = new Record2D();
            mdl.DrawLine((1, 2), (3, 4), 5, COLOR.Red);
            mdl.DrawCircle((4, 5), 6, COLOR.Blue);
            mdl.DrawPolygon(COLOR.Gray, (1, 1), (2, 2), (3, 3));
            mdl.DrawAsset(System.Numerics.Matrix3x2.Identity, "Hello");
            mdl.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(100,100), new ImageSource(string.Empty, (0,0), (10,10), (5,5)));
            mdl.DrawTextLine((0, 0), "hello world", 20, COLOR.Red);

            var rect = mdl.BoundingRect;
        }

    }
}
