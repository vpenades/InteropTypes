using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
 
    [Category("InteropDrawing Core")]
    public class Model2DTests
    {
        [Test]
        public void CreateModel2D()
        {
            var mdl = new Model2D();
            mdl.DrawLine((1, 2), (3, 4), 5, COLOR.Red);
            mdl.DrawCircle((4, 5), 6, COLOR.Blue);
            mdl.DrawPolygon(COLOR.Gray, (1, 1), (2, 2), (3, 3));
            mdl.DrawAsset(System.Numerics.Matrix3x2.Identity, "Hello", COLOR.White);

            var lines = mdl.ToLog().ToArray();

            mdl.AttachToCurrentTest("result.svg");
            mdl.AttachToCurrentTest("result.png");
        }

    }
}
