using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace InteropDrawing
{
    public class TypeTests
    {
        [Test]
        public unsafe void TestCoreTypes()
        {
            Assert.AreEqual(sizeof(System.Numerics.Vector2), sizeof(InteropTypes.Graphics.Drawing.Point2));
            Assert.AreEqual(sizeof(System.Drawing.PointF), sizeof(InteropTypes.Graphics.Drawing.Point2));
            Assert.AreEqual(sizeof(System.Drawing.SizeF), sizeof(InteropTypes.Graphics.Drawing.Point2));

            Assert.AreEqual(sizeof(System.Numerics.Vector3), sizeof(InteropTypes.Graphics.Drawing.Point3));            
        }

    }
}
