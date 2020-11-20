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
    }
}
