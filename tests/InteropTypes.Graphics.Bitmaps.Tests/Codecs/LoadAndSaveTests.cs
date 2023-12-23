using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

using NUnit.Framework;

using SixLabors.ImageSharp;

namespace InteropTypes.Codecs
{
    [Category("Codecs")]
    public class LoadAndSaveTests
    {
        [SetUp]
        public void SetUp()
        {
            Assert.That(IntPtr.Size, Is.EqualTo(8), "x64 test environment required");
        }

        [TestCase("shannon.jpg")]
        [TestCase("diagram.jpg")]
        [TestCase("white.png")]
        public void LoadImageWithDefaultCodec(string filePath)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var bitmap = MemoryBitmap.Load(ResourceInfo.From(filePath));
            Assert.That(bitmap.IsEmpty, Is.False);
            TestContext.WriteLine(bitmap.Info);
            sw.Stop();
        }


        [TestCase("shannon.jpg")]
        [TestCase("diagram.jpg")]
        [TestCase("white.png")]
        public void LoadImage(string filePath)
        {
            TestContext.CurrentContext.AttachFolderBrowserShortcut();            

            var codecs = new IBitmapDecoder[] { OpenCvCodec.Default, GDICodec.Default, WPFCodec.Default, ImageSharpCodec.Default, STBCodec.Default, SkiaCodec.Default };

            foreach (var decoder in codecs.OfType<IBitmapDecoder>())
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var bitmap = MemoryBitmap.Load(ResourceInfo.From(filePath), decoder);
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
                        .Save(AttachmentInfo.From(fname), encoder);
                }
            }
        }


        // [TestCase("Resshannon.dds")]
        [TestCase("shannon.jpg")]
        [TestCase("shannon.tif")]        
        [TestCase("shannon.psd")]
        [TestCase("shannon.ico")]
        [TestCase("shannon.webp")]
        public void LoadWithMultiCodec(string filePath)
        {
            var img = MemoryBitmap.Load(ResourceInfo.From(filePath), Codecs.STBCodec.Default, Codecs.OpenCvCodec.Default, Codecs.ImageSharpCodec.Default, Codecs.GDICodec.Default, Codecs.SkiaCodec.Default);
            
            Assert.That(img.Width, Is.EqualTo(512));
            Assert.That(img.Height, Is.EqualTo(512));
        }

        // [TestCase("shannon.dds")]
        [TestCase("shannon.jpg")]
        [TestCase("shannon.tif")]
        [TestCase("shannon.psd")]
        [TestCase("shannon.ico")]
        [TestCase("shannon.webp")]
        public void LoadWithConversion(string filePath)
        {
            var bgr = MemoryBitmap<Pixel.BGR24>.Load(ResourceInfo.From(filePath), STBCodec.Default, OpenCvCodec.Default, ImageSharpCodec.Default, GDICodec.Default, SkiaCodec.Default);            

            var rgb = MemoryBitmap<Pixel.BGR24>.Load(ResourceInfo.From(filePath), STBCodec.Default, OpenCvCodec.Default, ImageSharpCodec.Default, GDICodec.Default, SkiaCodec.Default);            

            Assert.That(bgr.Width, Is.EqualTo(512));
            Assert.That(bgr.Height, Is.EqualTo(512));
        }

        [Test]
        public void LoadWebpToImageSharp()
        {
            // load a bitmap with SkiaSharp, which is known to support WEBP.
            var mem = MemoryBitmap.Load(ResourceInfo.From("shannon.webp"), Codecs.SkiaCodec.Default);

            using(var proxy = mem.UsingImageSharp())
            {
                mem.Save(AttachmentInfo.From("ImageSharp.png"));
            }            
        }


        [Test]
        public void SaveMjpeg()
        {
            var f1 = ResourceInfo.From("shannon.jpg");
            var f2 = ResourceInfo.From("shannon-blurred.jpg");

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

            AttachmentInfo
                .From("video.avi")
                .WriteAVI(_getFrames());
        }


        [Explicit("Performance test must run in release mode")]
        [TestCase("shannon.jpg")]
        [TestCase("shutterstock_267944357.jpg")]
        [TestCase("ios-11-camera-qr-code-scan.jpg")]
        [TestCase("2awithqr.jpg")]
        [TestCase("dog.jpeg")]
        public void LoadJpegPerformanceTest(string filePath)
        {
            TestContext.WriteLine(filePath);
            TestContext.WriteLine("");            

            void _writeToTest(string hdr, TimeSpan t)
            {
                TestContext.WriteLine($"{hdr} {t.TotalMilliseconds}ms");
            }

            void _doNothing(string hdr, TimeSpan t) { }
            
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
                    var bmp = MemoryBitmap.Load(ResourceInfo.From(filePath), OpenCvCodec.Default);
                    _action2($"OpenCV  OutputFmt: {bmp.PixelFormat}", TimeSpan.Zero);
                }

                using (var perf = PerformanceBenchmark.Run(result => _action1("Skia", result)))
                {
                    var bmp = MemoryBitmap.Load(ResourceInfo.From(filePath), SkiaCodec.Default);
                    _action2($"Skia  OutputFmt: {bmp.PixelFormat}", TimeSpan.Zero);
                }

                using (var perf = PerformanceBenchmark.Run(result => _action1("WPF", result)))
                {
                    var bmp = MemoryBitmap.Load(ResourceInfo.From(filePath), WPFCodec.Default);
                    _action2($"WPF  OutputFmt: {bmp.PixelFormat}", TimeSpan.Zero);
                }

                using (var perf = PerformanceBenchmark.Run(result => _action1("ImageSharp", result)))
                {
                    var bmp = MemoryBitmap.Load(ResourceInfo.From(filePath), ImageSharpCodec.Default);
                    _action2($"ImageSharp  OutputFmt: {bmp.PixelFormat}", TimeSpan.Zero);
                }

                using (var perf = PerformanceBenchmark.Run(result => _action1("GDI", result)))
                {
                    var bmp = MemoryBitmap.Load(ResourceInfo.From(filePath), GDICodec.Default);
                    _action2($"GDI  OutputFmt: {bmp.PixelFormat}", TimeSpan.Zero);
                }

                using (var perf = PerformanceBenchmark.Run(result => _action1("STB", result)))
                {
                    var bmp = MemoryBitmap.Load(ResourceInfo.From(filePath), STBCodec.Default);
                    _action2($"STB  OutputFmt: {bmp.PixelFormat}", TimeSpan.Zero);
                }

                TestContext.WriteLine("");
            }
        }

        [Test]
        public void LoadAndSaveInteropFormat()
        {
            TestContext.CurrentContext.AttachFolderBrowserShortcut();

            var img1 = MemoryBitmap<Pixel.BGR24>.Load(ResourceInfo.From("shannon.jpg"));

            var tmpPath = AttachmentInfo.From("shannon.interopbmp").WriteObject(f => img1.Save(f));

            var img2 = MemoryBitmap<Pixel.BGR24>.Load(tmpPath.FullName);

            img2.Save(AttachmentInfo.From("shannon.png"));
        }
    }
}
