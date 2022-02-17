using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
            /*
            bmp = bmp
                .WithOpenCv()
                .ToResizedMemoryBitmap(50, 50, OpenCvSharp.InterpolationFlags.Lanczos4);
                */

            using (var proxy = bmp.UsingOpenCv())
            {
                proxy.Mat.Circle(new OpenCvSharp.Point(150, 150), 35, OpenCvSharp.Scalar.Red, 10);

                // proxy.Mat.Blur(new OpenCvSharp.Size(4, 4));
                // OpenCvSharp.Cv2.Blur(proxy.Mat, proxy.Mat, new OpenCvSharp.Size(4, 4));                
            }            

            using (var proxy = bmp.UsingGDI())
            {
                // Use GDI to draw a triangle:
                var a = new System.Drawing.Point(5, 5);
                var b = new System.Drawing.Point(50, 50);
                var c = new System.Drawing.Point(5, 50);

                proxy.Canvas.DrawPolygon(System.Drawing.Pens.Red, new[] { a, b, c });
            }

            using(var proxy = bmp.UsingImageSharp())
            {
                proxy.Image.Mutate(ipc => ipc.DrawPolygon(SixLabors.ImageSharp.Color.Blue, 2, (50, 5), (50, 50), (5, 5)));
            }

            using(var proxy = bmp.UsingSkiaSharp())
            {
                var p0 = new SkiaSharp.SKPoint(5, 25);
                var p1 = new SkiaSharp.SKPoint(45, 25);

                using var skiaPaint = new SkiaSharp.SKPaint
                {
                    TextSize = 64.0f,
                    IsAntialias = true,
                    StrokeWidth = 20,
                    Color = new SkiaSharp.SKColor(0, 255, 0),
                    Style = SkiaSharp.SKPaintStyle.Fill
                };

                proxy.Canvas.DrawLine(p0, p1, skiaPaint);
                proxy.Canvas.DrawText("SkiaSharp", new SkiaSharp.SKPoint(5, 200), skiaPaint);
            }

            // Use Imagesharp to save to PNG
            bmp.AttachToCurrentTest("shannon.png");
        }

        [Test]
        public void Example2()
        {
            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\shannon.webp");

            // Use SkiaSharp to load a WEBP image:
            var bmp = MemoryBitmap.Load(filePath, Codecs.SkiaCodec.Default);
            
            // Use OpenCV to resize the image:
            var span = bmp
                .WithOpenCv()
                .ToResizedMemoryBitmap(50, 50, OpenCvSharp.InterpolationFlags.Lanczos4)
                .AsSpanBitmap();

            // Use GDI to draw a triangle:
            var a = new System.Drawing.Point(5, 5);
            var b = new System.Drawing.Point(45, 45);
            var c = new System.Drawing.Point(5, 45);

            span
                .WithGDI()
                .Draw(dc => dc.DrawPolygon(System.Drawing.Pens.Red, new[] { a, b, c }));            

            // Use Imagesharp to save to PNG
            span.AttachToCurrentTestAll("shannon.png");
        }

        [Test(Description ="Simple conversion From ImageSharp to System.Drawing")]
        public void Example3()
        {
            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\shannon.jpg");

            using var image = SixLabors.ImageSharp.Image.Load<Bgra32>(filePath);

            // direct conversion.

            using var bitmap = image.ReadAsSpanBitmap(self => self.WithGDI().ToBitmap());

            bitmap.AttachToCurrentTest("result.jpg");

            // resized conversion.

            using var resized = image.ReadAsSpanBitmap(self => self.WithGDI().ToResizedBitmap(32, 32));            

            resized.AttachToCurrentTest("resized.jpg");

            // using MemoryBitmap facade.

            void _gdiDraw(SpanBitmap image)
            {
                using var gdi = image.ToMemoryBitmap().UsingGDI(); // using memoryBitmap as a System.Drawing.Bitmap

                gdi.Canvas
                    .DrawLine(System.Drawing.Pens.Red, new System.Drawing.Point(2, 2), new System.Drawing.Point(500, 500));

                gdi.Bitmap.AttachToCurrentTest("drawn.jpg");
            }

            image.MutateAsSpanBitmap(_gdiDraw);            

        }
    }
}
