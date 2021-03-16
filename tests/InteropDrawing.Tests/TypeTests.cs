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
            Assert.AreEqual(sizeof(System.Numerics.Vector2), sizeof(InteropDrawing.Point2));
            Assert.AreEqual(sizeof(System.Drawing.PointF), sizeof(InteropDrawing.Point2));
            Assert.AreEqual(sizeof(System.Drawing.SizeF), sizeof(InteropDrawing.Point2));

            Assert.AreEqual(sizeof(System.Numerics.Vector3), sizeof(InteropDrawing.Point3));            
        }

    }
}
