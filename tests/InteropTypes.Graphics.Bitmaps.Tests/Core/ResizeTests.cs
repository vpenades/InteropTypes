using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends;

using NUnit.Framework;

using XY = System.Numerics.Vector2;

namespace InteropTypes.Graphics.Bitmaps
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

            BitmapsToolkit.FitPixels(src, dst, (0, 1));

            var sw = System.Diagnostics.Stopwatch.StartNew();
            BitmapsToolkit.FitPixels(src, dst, (0, 1));
            var elapsed = sw.Elapsed;

            TestContext.WriteLine($"{elapsed.Milliseconds}ms {elapsed.Ticks}ticks");

            src.Save(AttachmentInfo.From("input.png"));
            dst.Save(AttachmentInfo.From($"output_{dstSize}.png"));
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
            src.Save(AttachmentInfo.From("input.png"));

            src.AsSpanBitmap().ApplyEffect(new Processing.BitmapMirror(true,false));
            src.Save(AttachmentInfo.From("horizontalFlip.png"));

            src.AsSpanBitmap().ApplyEffect(new Processing.BitmapMirror(false, true));
            src.Save(AttachmentInfo.From("VerticalFlip.png"));
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

            var mirrorEffect = new Processing.BitmapMirror(hflip,vflip,multiThread);

            using (PerformanceBenchmark.Run(t => TestContext.WriteLine($"{w}x{h} HFlip:{hflip} VFlip:{vflip} {Math.Round(t.TotalMilliseconds / 1000)}ms")))
            {
                for (int r = 0; r < 1000; ++r)
                {
                    bmp.ApplyEffect(mirrorEffect);
                }
            }
        }


        [Test]
        public void TestSampler()
        {
            var map = new MemoryBitmap<Pixel.BGRA32>(16,16);
            map.SetPixels(new Pixel.BGRA32(255,127,63,31));

            var sampler = new Processing._PixelsTransformImplementation.SpanQuantized8Sampler<Pixel.BGRA32, Pixel.BGRA32>(map);

            var pix = sampler.GetSourcePixelOrDefault(8, 8);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void TestTransform(bool useBilinear)
        {
            var src = LoadShannonImage().OfType<Pixel.BGR24>();
            src.Save(new AttachmentInfo("input.png"));

            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\cat.png");
            var cat00 = MemoryBitmap
                .Load(filePath, Codecs.GDICodec.Default)
                .OfType<Pixel.BGRA32>();
            

            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\QRCode.png");
            var qrcode = MemoryBitmap
                .Load(filePath, Codecs.GDICodec.Default)
                .OfType<Pixel.BGRA32>();

            var x = Matrix3x2.CreateScale(0.75f, 0.75f) * Matrix3x2.CreateRotation(0.25f);
            x.Translation = new Vector2(20, 20);
            Matrix3x2.Invert(x, out var xx);

            var dst = new MemoryBitmap<Pixel.BGR24>(512, 512);

            using(PerformanceBenchmark.Run(t => TestContext.WriteLine($"Transform {(int)t.TotalMilliseconds}ms")))
            {
                dst.AsSpanBitmap().SetPixels(xx, src.AsSpanBitmap(), useBilinear);
            }            

            for(float r=0.1f; r < 1; r+=0.2f)
            {
                // 1st API

                var xform = Matrix3x2.CreateRotation(r) * Matrix3x2.CreateTranslation(50, 15);                
                xform = Matrix3x2.CreateTranslation(-50, -50) * xform * Matrix3x2.CreateTranslation(50, 50);
                xform = xform * Matrix3x2.CreateScale(3, 3);
                
                dst.AsSpanBitmap().SetPixels(xform, cat00.AsSpanBitmap(), useBilinear, r);
                DrawBounds(dst, cat00.Bounds, xform, Colors.Red);

                // 2nd API

                xform *= Matrix3x2.CreateTranslation(0, 150);

                dst.AsSpanBitmap().SetPixels(xform, cat00.AsSpanBitmap(), useBilinear, r);
                DrawBounds(dst, cat00.Bounds, xform, Colors.Red);
            }            

            dst.AsSpanBitmap().SetPixels(Matrix3x2.CreateScale(3), cat00.AsSpanBitmap(), useBilinear);
            dst.AsSpanBitmap().SetPixels(Matrix3x2.CreateScale(-.6f) * Matrix3x2.CreateTranslation(40,200), cat00.AsSpanBitmap(), useBilinear);
            dst.AsSpanBitmap().SetPixels(Matrix3x2.CreateScale(.3f) * Matrix3x2.CreateRotation(1) * Matrix3x2.CreateTranslation(150, 300), qrcode.AsSpanBitmap(), useBilinear);

            dst.Save(new AttachmentInfo("transformed.png"));
        }

        [Test]
        public void TestTransformWithConversion()
        {
            var src = LoadShannonImage().OfType<Pixel.BGR24>();
            src.Save(new AttachmentInfo("input.png"));

            var dst = new MemoryBitmap<Pixel.RGB96F>(512,512);

            dst.AsSpanBitmap().SetPixels(Matrix3x2.CreateScale(0.7f), src.AsSpanBitmap(), true, 1);

            dst.Save(new AttachmentInfo("transformed.png"));
        }


        public static void DrawBounds<TPixel>(MemoryBitmap<TPixel> target, in BitmapBounds bounds, in Matrix3x2 xform, TPixel color)
            where TPixel : unmanaged
        {
            DrawBounds(target.AsSpanBitmap(), bounds, xform, color);
        }

        public static void DrawBounds<TPixel>(SpanBitmap<TPixel> target, in BitmapBounds bounds, in Matrix3x2 xform, TPixel color)
            where TPixel : unmanaged
        {
            var p0 = new XY(bounds.X, bounds.Y);
            var p1 = new XY(bounds.Width, bounds.Height);            

            var pa = XY.Transform(p0, xform);
            var pb = pa + XY.TransformNormal(p1 * XY.UnitX, xform);
            var pc = pa + XY.TransformNormal(p1 * XY.One, xform);
            var pd = pa + XY.TransformNormal(p1 * XY.UnitY, xform);
            
            target.DrawPixelLine(pa, pb, color);
            target.DrawPixelLine(pb, pc, color);
            target.DrawPixelLine(pc, pd, color);
            target.DrawPixelLine(pd, pa, color);
        }
    }
}
