using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

using NUnit.Framework;

namespace InteropTypes.Codecs
{
    [Category("Codecs")]
    public class LoadAndSaveTests
    {
        [SetUp]
        public void SetUp()
        {
            Assert.AreEqual(8, IntPtr.Size, "x64 test environment required");
        }

        [TestCase("Resources\\shannon.jpg")]
        [TestCase("Resources\\diagram.jpg")]
        [TestCase("Resources\\white.png")]
        public void LoadImageWithDefaultCodec(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var bitmap = MemoryBitmap.Load(filePath);
            Assert.IsFalse(bitmap.IsEmpty);
            TestContext.WriteLine(bitmap.Info);
            sw.Stop();
        }


        [TestCase("Resources\\shannon.jpg")]
        [TestCase("Resources\\diagram.jpg")]
        [TestCase("Resources\\white.png")]
        public void LoadImage(string filePath)
        {
            TestContext.CurrentContext.AttachFolderBrowserShortcut();

            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var codecs = new IBitmapDecoder[] { OpenCvCodec.Default, GDICodec.Default, WPFCodec.Default, ImageSharpCodec.Default, STBCodec.Default, SkiaCodec.Default };

            foreach (var decoder in codecs.OfType<IBitmapDecoder>())
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var bitmap = MemoryBitmap.Load(filePath, decoder);
                sw.Stop();

                TestContext.WriteLine($"Loading {System.IO.Path.GetFileName(filePath)} with {decoder} tool {sw.ElapsedMilliseconds}");                

                foreach (var encoder in codecs.OfType<IBitmapEncoder>())
                {
                    // System.Diagnostics.Debug.Assert(!(decoder is WPFCodec && encoder is SkiaCodec));

                    var fname = $"{decoder.GetType().Name}-To-{encoder.GetType().Name}.png";                    
                    bitmap.Save(new AttachmentInfo(fname), encoder);                    

                    fname = $"{decoder.GetType().Name}-To-{encoder.GetType().Name}.jpg";                    
                    bitmap.Save(new AttachmentInfo(fname), encoder);                    

                    fname = $"{decoder.GetType().Name}-To-{encoder.GetType().Name}-Gray.jpg";                    
                    bitmap
                        .AsSpanBitmap()
                        .ToMemoryBitmap(Pixel.Luminance8.Format)
                        .Save(new AttachmentInfo(fname), encoder);
                }
            }
        }


        // [TestCase("Resources\\shannon.dds")]
        [TestCase("Resources\\shannon.jpg")]
        [TestCase("Resources\\shannon.tif")]        
        [TestCase("Resources\\shannon.psd")]
        [TestCase("Resources\\shannon.ico")]
        [TestCase("Resources\\shannon.webp")]
        public void LoadWithMultiCodec(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);
            
            var img = MemoryBitmap.Load(filePath, Codecs.STBCodec.Default, Codecs.OpenCvCodec.Default, Codecs.ImageSharpCodec.Default, Codecs.GDICodec.Default, Codecs.SkiaCodec.Default);

            Assert.NotNull(img);
            Assert.AreEqual(512, img.Width);
            Assert.AreEqual(512, img.Height);
        }

        // [TestCase("Resources\\shannon.dds")]
        [TestCase("Resources\\shannon.jpg")]
        [TestCase("Resources\\shannon.tif")]
        [TestCase("Resources\\shannon.psd")]
        [TestCase("Resources\\shannon.ico")]
        [TestCase("Resources\\shannon.webp")]
        public void LoadWithConversion(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var bgr = MemoryBitmap<Pixel.BGR24>.Load(filePath, STBCodec.Default, OpenCvCodec.Default, ImageSharpCodec.Default, GDICodec.Default, SkiaCodec.Default);
            Assert.NotNull(bgr);

            var rgb = MemoryBitmap<Pixel.BGR24>.Load(filePath, STBCodec.Default, OpenCvCodec.Default, ImageSharpCodec.Default, GDICodec.Default, SkiaCodec.Default);
            Assert.NotNull(rgb);

            Assert.AreEqual(512, bgr.Width);
            Assert.AreEqual(512, bgr.Height);
        }

        [Test]
        public void LoadWebpToImageSharp()
        {
            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\shannon.webp");

            // load a bitmap with SkiaSharp, which is known to support WEBP.
            var mem = MemoryBitmap.Load(filePath, Codecs.SkiaCodec.Default);

            using(var proxy = mem.UsingImageSharp())
            {
                proxy.Image.AttachToCurrentTest("ImageSharp.png");
            }            
        }


        [Test]
        public void SaveMjpeg()
        {
            var f1 = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\shannon.jpg");
            var f2 = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\shannon-blurred.jpg");

            var ff1 = MemoryBitmap.Load(f1, GDICodec.Default);
            var ff2 = MemoryBitmap.Load(f2, GDICodec.Default);

            IEnumerable<MemoryBitmap> _getFrames()
            {
                for (int i = 0; i < 10; ++i)
                {
                    yield return ff1;
                    yield return ff1;
                    yield return ff1;
                    yield return ff1;
                    yield return ff2;
                    yield return ff2;
                    yield return ff2;
                    yield return ff2;
                }
            }

            _getFrames().AttachAviToCurrentTest("video.avi");
        }


        [Explicit("Performance test must run in release mode")]
        [TestCase("Resources\\shannon.jpg")]
        [TestCase("Resources\\shutterstock_267944357.jpg")]
        [TestCase("Resources\\ios-11-camera-qr-code-scan.jpg")]
        [TestCase("Resources\\2awithqr.jpg")]
        [TestCase("Resources\\dog.jpeg")]
        public void LoadJpegPerformanceTest(string filePath)
        {
            TestContext.WriteLine(filePath);
            TestContext.WriteLine("");

            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            void _writeToTest(string hdr, TimeSpan t)
            {
                TestContext.WriteLine($"{hdr} {t.TotalMilliseconds}ms");
            }

            void _doNothing(string hdr, TimeSpan t)
            {
                
            }            


            Action<string, TimeSpan> _action1 = _writeToTest;
            Action<string, TimeSpan> _action2 = _writeToTest;

            for (int i = 0; i < 3; ++i)
            {
                _action1 = _writeToTest;
                if (i < 2) _action1 = _doNothing;

                _action2 = _doNothing;
                if (i == 0) _action2 = _writeToTest;

                using (var perf = PerformanceBenchmark.Run(result => _action1("OpenCV", result)))
                {
                    var bmp = MemoryBitmap.Load(filePath, OpenCvCodec.Default);
                    _action2($"OpenCV  OutputFmt: {bmp.PixelFormat}", TimeSpan.Zero);
                }

                using (var perf = PerformanceBenchmark.Run(result => _action1("Skia", result)))
                {
                    var bmp = MemoryBitmap.Load(filePath, SkiaCodec.Default);
                    _action2($"Skia  OutputFmt: {bmp.PixelFormat}", TimeSpan.Zero);
                }

                using (var perf = PerformanceBenchmark.Run(result => _action1("WPF", result)))
                {
                    var bmp = MemoryBitmap.Load(filePath, WPFCodec.Default);
                    _action2($"WPF  OutputFmt: {bmp.PixelFormat}", TimeSpan.Zero);
                }

                using (var perf = PerformanceBenchmark.Run(result => _action1("ImageSharp", result)))
                {
                    var bmp = MemoryBitmap.Load(filePath, ImageSharpCodec.Default);
                    _action2($"ImageSharp  OutputFmt: {bmp.PixelFormat}", TimeSpan.Zero);
                }

                using (var perf = PerformanceBenchmark.Run(result => _action1("GDI", result)))
                {
                    var bmp = MemoryBitmap.Load(filePath, GDICodec.Default);
                    _action2($"GDI  OutputFmt: {bmp.PixelFormat}", TimeSpan.Zero);
                }

                using (var perf = PerformanceBenchmark.Run(result => _action1("STB", result)))
                {
                    var bmp = MemoryBitmap.Load(filePath, STBCodec.Default);
                    _action2($"STB  OutputFmt: {bmp.PixelFormat}", TimeSpan.Zero);
                }

                TestContext.WriteLine("");
            }
        }


    }
}
