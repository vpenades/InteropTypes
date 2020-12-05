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
        [Test]
        public void ResizeTest1()
        {
            var src = LoadShannonImage();
            var dst = new MemoryBitmap(333, 333, Pixel.BGR24.Format , 1000);

            SpanBitmap.FitPixels(src, dst, (0, 1));

            var sw = System.Diagnostics.Stopwatch.StartNew();
            SpanBitmap.FitPixels(src, dst, (0, 1));
            var elapsed = sw.Elapsed;

            TestContext.WriteLine($"{elapsed.Milliseconds}ms {elapsed.Ticks}ticks");

            src.AttachToCurrentTest("input.png");
            dst.AttachToCurrentTest("output.png");
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

            var watch = System.Diagnostics.Stopwatch.StartNew();

            for(int r=0; r < 1000; ++r)
            {
                bmp.ApplyMirror(hflip, vflip, multiThread);
            }

            watch.Stop();

            var ms = watch.ElapsedMilliseconds / 1000f;

            TestContext.WriteLine($"{w}x{h} HFlip:{hflip} VFlip:{vflip} {ms}ms");
        }
    }
}
