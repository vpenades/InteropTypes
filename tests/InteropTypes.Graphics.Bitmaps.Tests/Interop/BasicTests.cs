using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using SixLabors.ImageSharp.Drawing.Processing;
using InteropTypes.Graphics.Backends;
using SixLabors.ImageSharp;

namespace InteropTypes.Graphics.Bitmaps
{
    [Category("Backends")]
    public class BasicTests
    {
        [TestCase("shannon.jpg")]        
        public void CalculateImageBlurFactor(string filePath)
        {
            filePath = ResourceInfo.From(filePath);

            // load image using GDI
            var bitmap = MemoryBitmap
                .Load(filePath, Codecs.GDICodec.Default)
                .AsSpanBitmap();
            
            var blurfactor1 = Processing.SharpnessEvaluator.Evaluate(bitmap, 0.25);

            // blur with openCV
            bitmap.WithOpenCv().ApplyBlur((5, 5));
            
            var blurfactor2 = Processing.SharpnessEvaluator.Evaluate(bitmap, 0.25);

            // blur with openCV
            bitmap.WithOpenCv().ApplyBlur((32, 32));
            
            var blurfactor3 = Processing.SharpnessEvaluator.Evaluate(bitmap, 0.25);

            TestContext.Out.WriteLine($"{filePath} => {blurfactor1}, {blurfactor2}, {blurfactor3}");

            bitmap.AttachToCurrentTestAll("final.png");
        }


        [Test]
        public void DrawPrimitivesWithMultipleAdapters()
        {
            void _drawUsingMultipleDevices(SpanBitmap<Pixel.BGRA32> img)
            {
                var slice = img.Slice((10, 10, img.Width - 20, img.Height - 20)); // crop the source image with a 10 pixels margin

                // cast to OpenCV adapter to blur and find edges in the image.
                slice
                    .WithOpenCv()
                    .ApplyBlur((4, 4))
                    .ApplyCanny(100, 200);

                // cast to GDI Adapter to draw primitives.
                slice.MutateAsGDI(dc =>
                    {
                        var a = new System.Drawing.Point(5, 5);
                        var b = new System.Drawing.Point(50, 50);
                        var c = new System.Drawing.Point(5, 50);

                        var f = new System.Drawing.Font("arial", 30);

                    // draw a triangle
                    dc.DrawPolygon(System.Drawing.Pens.Yellow, new[] { a, b, c });
                    // draw text
                    dc.DrawString("GDI Text", f, System.Drawing.Brushes.Yellow, new System.Drawing.PointF(5, 60));
                    });

                // cast to SkiaSharp Adapter to draw primitives.
                slice
                    .WithSkiaSharp()
                    .Draw
                    (canvas =>
                    {
                        var p0 = new SkiaSharp.SKPoint(5, 120);
                        var p1 = new SkiaSharp.SKPoint(250, 120);

                        using var skiaPaint = new SkiaSharp.SKPaint
                        {
                            TextSize = 64.0f,
                            IsAntialias = true,
                            StrokeWidth = 20,
                            Color = new SkiaSharp.SKColor(0, 0, 255),
                            Style = SkiaSharp.SKPaintStyle.Fill
                        };

                        canvas.DrawLine(p0, p1, skiaPaint);
                        canvas.DrawText("SkiaSharp Text", new SkiaSharp.SKPoint(5, 200), skiaPaint);
                    });

                // cast to imagesharp Adapter to draw primitives
                slice.MutateAsImageSharp(ipc =>
                    {
                        ipc.FillPolygon(SixLabors.ImageSharp.Color.Green, (5, 250), (50, 250), (5, 300));
                        ipc.DrawText("ImageSharp Text", SixLabors.Fonts.SystemFonts.CreateFont("Arial", 40), SixLabors.ImageSharp.Color.Green, new SixLabors.ImageSharp.PointF(80, 250));
                    });

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
            }            

            // load an image with Sixlabors Imagesharp, notice we use BGRA32 because RGBA32 is NOT supported by GDI.
            var img = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Bgra32>(ResourceInfo.From("shannon.jpg"));

            // render using multiple devices

            img.WriteAsSpanBitmap(self => _drawUsingMultipleDevices(self));

            // save the result back with ImageSharp

            AttachmentInfo
                .From("result.jpg")
                .WriteObject(f => img.Save(f));

            img.Dispose();
        }


        [Test]
        public void DrawMemoryAsImageSharp()
        {
            var mem = MemoryBitmap.Load(ResourceInfo.From("shannon.jpg"), Codecs.GDICodec.Default);            

            mem.OfType<Pixel.BGRA32>()
                .AsSpanBitmap()
                .ReadAsImageSharp(img => {  AttachmentInfo.From("result.png").WriteImage(img); return 0; } );
        }

        [Test]
        public void DrawGDIAsImageSharp()
        {
            using var gdi = System.Drawing.Image.FromFile(ResourceInfo.From("shannon.jpg"));
            using var bmp = new System.Drawing.Bitmap(gdi);

            using var ptr = bmp.UsingPointerBitmap();

            ptr.AsPointerBitmap()
                .AsSpanBitmapOfType<Pixel.BGRA32>()
                .ReadAsImageSharp(img => AttachmentInfo.From("result.png").WriteImage(img));
        }
    }
}
