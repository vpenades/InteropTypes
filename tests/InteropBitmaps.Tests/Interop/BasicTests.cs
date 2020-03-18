using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Interop
{
    [Category("Backends GDI")]
    public class BasicTests
    {
        [TestCase("Resources\\shannon.jpg")]        
        public void CalculateImageBlurFactor(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            // load image using GDI
            var bitmap = MemoryBitmap.Load(filePath, Codecs.GDICodec.Default);

            // Calculate blur factor using ImageSharp
            var blurfactor1 = bitmap
                .WithImageSharp()
                .CalculateBlurFactor();

            // blur with openCV
            bitmap.WithOpenCv().ApplyBlur((5, 5));

            // Calculate blur factor using ImageSharp
            var blurfactor2 = bitmap
                .WithImageSharp()
                .CalculateBlurFactor();

            // blur with openCV
            bitmap.WithOpenCv().ApplyBlur((32, 32));

            // Calculate blur factor using ImageSharp
            var blurfactor3 = bitmap
                .WithImageSharp()
                .CalculateBlurFactor();

            TestContext.WriteLine($"{filePath} => {blurfactor1}, {blurfactor2}, {blurfactor3}");

            bitmap.AttachToCurrentTest("final.png");
        }


        [Test]
        public void DrawGDIPrimitives()
        {
            // load an image with Sixlabors Imagesharp, notice we use BGRA32 because RGBA32 is NOT supported by GDI.
            var img = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Bgra32>(TestResources.ShannonJpg);

            // slice the central region of the source image

            var slice = img
                .AsSpanBitmap()
                .Slice((10, 10, img.Width - 20, img.Height - 20));

            // cast to OpenCV adapter to blur and find edges in the image.

            slice
                .WithOpenCv()                
                .ApplyBlur((4,4))
                .ApplyCanny(100, 200);

            // cast to GDI Adapter to draw primitives.

            slice
                .WithGDI()
                .Draw
                (dc =>
                {
                    var a = new System.Drawing.Point(5, 5);
                    var b = new System.Drawing.Point(50, 50);
                    var c = new System.Drawing.Point(5, 50);

                    var f = new System.Drawing.Font("arial", 30);

                    // draw a triangle
                    dc.DrawPolygon(System.Drawing.Pens.Red, new[] { a, b, c });
                    // draw text
                    dc.DrawString("Text drawn with GDI", f, System.Drawing.Brushes.Yellow, new System.Drawing.PointF(5, 60));
                }
                );

            // cast to SkiaSharp Adapter to draw primitives.

            var paint1 = new SkiaSharp.SKPaint
            {
                TextSize = 64.0f,
                IsAntialias = true,
                Color = new SkiaSharp.SKColor(0, 0, 255),
                Style = SkiaSharp.SKPaintStyle.Fill,
                StrokeWidth = 20
            };

            SkiaSharpToolkit.WithSkiaSharp(slice)
                .Draw
                (canvas =>
                {
                    var p0 = new SkiaSharp.SKPoint(5, 120);
                    var p1 = new SkiaSharp.SKPoint(250, 120);                                   

                    canvas.DrawLine(p0, p1, paint1);
                    canvas.DrawText("SkiaSharp", new SkiaSharp.SKPoint(5, 200), paint1);
                }
                );

            paint1.Dispose();

            img.AttachToCurrentTest("result.jpg");

            img.Dispose();
        }
    }
}
