using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.TensorBitmaps;

using NUnit.Framework;

namespace InteropTypes.Vision.Tests
{
    internal class TensorBitmapTests
    {

        [Test]
        public void TestCreateBitmaps()
        {
            var bmp = TensorBitmap<float, Vector3>.Create(256, 267, TensorPixelFormat.Rgb96f);

            var slice = bmp.GetCropped(new System.Drawing.Rectangle(5, 5, 16, 16));
            Assert.That(slice.Width, Is.EqualTo(16));
            Assert.That(slice.Height, Is.EqualTo(16));

            for(int i=0; i < slice.Height; i++)
            {
                var row = slice.GetRowPixelsSpan(i);

                Assert.That(row.Length, Is.EqualTo(16));
            }

        }


    }
}
