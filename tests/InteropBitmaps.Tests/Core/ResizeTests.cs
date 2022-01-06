using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Core
{
    [Category("Core")]
    public class ResizeTests
    {
        [TestCase(333)]
        [TestCase(2021)]
        public void ResizeTest1(int dstSize)
        {
            var src = LoadShannonImage();
            var dst = new MemoryBitmap(dstSize, dstSize, Pixel.BGR24.Format , dstSize * 3 + 17);

            SpanBitmap.FitPixels(src, dst, (0, 1));

            var sw = System.Diagnostics.Stopwatch.StartNew();
            SpanBitmap.FitPixels(src, dst, (0, 1));
            var elapsed = sw.Elapsed;

            TestContext.WriteLine($"{elapsed.Milliseconds}ms {elapsed.Ticks}ticks");

            src.AttachToCurrentTest("input.png");
            dst.AttachToCurrentTest($"output_{dstSize}.png");
        }

        private static MemoryBitmap LoadShannonImage()
        {
            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\shannon.jpg");

            // load a bitmap with SkiaSharp, which is known to support WEBP.
            var mem = MemoryBitmap.Load(filePath, Codecs.GDICodec.Default);

            var mem2 = new MemoryBitmap(mem.Width, mem.Height, Pixel.BGR24.Format);
            mem2.SetPixels(0, 0, mem);

            return mem2;
        }

        [Test]
        public void TestFlip()
        {
            var src = LoadShannonImage();
            src.AttachToCurrentTest("input.png");

            src.AsSpanBitmap().ApplyMirror(true, false);
            src.AttachToCurrentTest("horizontalFlip.png");

            src.AsSpanBitmap().ApplyMirror(false, true);
            src.AttachToCurrentTest("VerticalFlip.png");
        }

        [TestCase(1920, 1080, false, true, false)]
        [TestCase(1920, 1080, false, false, true)]
        [TestCase(1920, 1080, false, true, true)]
        [TestCase(1920, 1080, true, true, false)]
        [TestCase(1920, 1080, true, false, true)]
        [TestCase(1920, 1080, true, true, true)]
        public void TestFlipPerformance(int w, int h, bool multiThread, bool hflip, bool vflip)
        {
            var bmp = new MemoryBitmap<Pixel.RGB24>(w, h).AsSpanBitmap();            

            using (PerformanceBenchmark.Run(t => TestContext.WriteLine($"{w}x{h} HFlip:{hflip} VFlip:{vflip} {Math.Round(t.TotalMilliseconds / 1000)}ms")))
            {
                for (int r = 0; r < 1000; ++r)
                {
                    bmp.ApplyMirror(hflip, vflip, multiThread);
                }
            }
        }

        [Test]
        public void TestTransform()
        {
            var src = LoadShannonImage().OfType<Pixel.BGR24>();
            src.AttachToCurrentTest("input.png");

            var x = Matrix3x2.CreateScale(0.75f, 0.75f) * Matrix3x2.CreateRotation(0.25f);
            x.Translation = new Vector2(20, 20);
            Matrix3x2.Invert(x, out var xx);

            var dst = new MemoryBitmap<Pixel.BGR24>(512, 512);

            using(PerformanceBenchmark.Run(t => TestContext.WriteLine($"Transform {t}")))
            {
                dst.AsSpanBitmap().SetPixels(xx, src);                
            }


            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\cat.png");            
            var cat = MemoryBitmap.Load(filePath, Codecs.GDICodec.Default);
            var cat00 = cat.OfType<Pixel.RGBA32>().AsSpanBitmap();

            for(float r=0; r < 1; r+=0.3f)
            {
                var xform = Matrix3x2.CreateRotation(r) * Matrix3x2.CreateTranslation(50, 15);

                // offset
                xform = Matrix3x2.CreateTranslation(-50, -50) * xform * Matrix3x2.CreateTranslation(50, 50);

                dst.AsSpanBitmap().SetPixels(xform, cat00, r);
            }

            

            dst.AttachToCurrentTest("transformed.png");
        }
    }
}
