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

            // cast to OpenCV adapter to blur and find edges in the image.

            img.AsSpanBitmap()
                .WithOpenCv()                
                .ApplyBlur((4,4))
                .ApplyCanny(100, 200);                

            // cast to GDI Adapter to draw primitives.

            img.AsSpanBitmap()
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
                    dc.DrawString("Text drawn with GDI", f, System.Drawing.SystemBrushes.Window, new System.Drawing.PointF(5, 60));
                }
                );            

            img.AttachToCurrentTest("result.jpg");
        }
    }
}
