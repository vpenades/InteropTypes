using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Examples
{
    [Category("Examples")]
    public class ShowCases
    {
        [Test]
        public void Example1()
        {
            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\shannon.webp");

            // Use SkiaSharp to load a WEBP image:
            var bmp = MemoryBitmap.Load(filePath, Codecs.SkiaCodec.Default);

            // Use OpenCV to resize the image:
            bmp = bmp
                .WithOpenCv()
                .ToResizedMemoryBitmap(55, 55, OpenCvSharp.InterpolationFlags.Lanczos4);

            // Use GDI to draw a triangle:
            var a = new System.Drawing.Point(5, 5);
            var b = new System.Drawing.Point(50, 50);
            var c = new System.Drawing.Point(5, 50);

            bmp
                .WithGDI()
                .Draw(dc => dc.DrawPolygon(System.Drawing.Pens.Red, new[] { a, b, c }));

            // Use Imagesharp to save to PNG
            bmp.AttachToCurrentTest("shannon.png");
        }
    }
}
