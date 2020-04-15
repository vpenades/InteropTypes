using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps
{
    [Category("Backends")]
    public class BasicTests
    {
        [TestCase("Resources\\shannon.jpg")]        
        public void CalculateImageBlurFactor(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            // load image using GDI
            var bitmap = MemoryBitmap
                .Load(filePath, Codecs.GDICodec.Default)
                .AsSpanBitmap();

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
        public void DrawPrimitivesWithMultipleAdapters()
        {
            // load an image with Sixlabors Imagesharp, notice we use BGRA32 because RGBA32 is NOT supported by GDI.
            var img = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Bgra32>(TestResources.ShannonJpg);

            // slice the central region of the source image

            var slice = img
                .AsSpanBitmap()
                .Slice((10, 10, img.Width - 20, img.Height - 20)); // crop the source image with a 10 pixels margin

            // cast to OpenCV adapter to blur and find edges in the image.

            slice
                .WithOpenCv()                
                .ApplyBlur((4,4))
                .ApplyCanny(100, 200);

            // cast to GDI Adapter to draw primitives.

            slice
                .WithGDI()
                .Draw
                ( dc => {
                    var a = new System.Drawing.Point(5, 5);
                    var b = new System.Drawing.Point(50, 50);
                    var c = new System.Drawing.Point(5, 50);

                    var f = new System.Drawing.Font("arial", 30);

                    // draw a triangle
                    dc.DrawPolygon(System.Drawing.Pens.Red, new[] { a, b, c });
                    // draw text
                    dc.DrawString("Text drawn with GDI", f, System.Drawing.Brushes.Yellow, new System.Drawing.PointF(5, 60));
                } );

            // cast to SkiaSharp Adapter to draw primitives.            

            slice
                .WithSkiaSharp()
                .Draw
                ( canvas => {
                    var p0 = new SkiaSharp.SKPoint(5, 120);
                    var p1 = new SkiaSharp.SKPoint(250, 120);

                    using var skiaPaint = new SkiaSharp.SKPaint
                    {
                        TextSize = 64.0f, IsAntialias = true, StrokeWidth = 20,
                        Color = new SkiaSharp.SKColor(0, 0, 255),
                        Style = SkiaSharp.SKPaintStyle.Fill                        
                    };

                    canvas.DrawLine(p0, p1, skiaPaint);
                    canvas.DrawText("SkiaSharp", new SkiaSharp.SKPoint(5, 200), skiaPaint);                    
                } );

            // cast to imagesharp Adapter to draw primitives

            slice
                .WithImageSharp()
                .Mutate
                ( ipc=> {

                    ipc.FillPolygon(SixLabors.ImageSharp.Color.Green, (5, 250), (50, 250), (5, 300));

                } );

            // wpf

            /* not working yet
            slice
                .WithWPF()
                .Draw
                (dc =>
                {
                    dc.DrawEllipse(System.Windows.Media.Brushes.Red, null, new System.Windows.Point(5, 5), 10, 10);
                    dc.DrawEllipse(System.Windows.Media.Brushes.Blue, null, new System.Windows.Point(50, 50), 100, 100);
                }
                );*/

            // save the result back with ImageSharp

            img.AttachToCurrentTest("result.jpg");

            img.Dispose();
        }


        [Test]
        public void DrawMemoryAsImageSharp()
        {
            var mem = MemoryBitmap.Load(TestResources.ShannonJpg, Codecs.GDICodec.Default);

            var img = mem.TryUsingAsImageSharp();

            img.AttachToCurrentTest("result.png");

            img.Dispose();            
        }

        [Test]
        public void DrawGDIAsImageSharp()
        {
            using var gdi = System.Drawing.Image.FromFile(TestResources.ShannonJpg);

            using var bmp = new System.Drawing.Bitmap(gdi);

            using var ptr = bmp.UsingPointerBitmap();

            using var img = ptr.Bitmap.TryUsingImageSharp<SixLabors.ImageSharp.PixelFormats.Bgra32>();
            
            img.AttachToCurrentTest("result.png");
        }
    }
}
