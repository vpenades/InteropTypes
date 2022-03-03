using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Codecs;

using NUnit.Framework;

namespace InteropBitmaps.Codecs
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
        public void LoadImage(string filePath)
        {
            TestContext.CurrentContext.AttachShowDirLink();

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
                    fname = TestContext.CurrentContext.GetTestResultPath(fname);
                    bitmap.Save(fname, encoder);
                    TestContext.AddTestAttachment(fname);

                    fname = $"{decoder.GetType().Name}-To-{encoder.GetType().Name}.jpg";
                    fname = TestContext.CurrentContext.GetTestResultPath(fname);
                    bitmap.Save(fname, encoder);
                    TestContext.AddTestAttachment(fname);

                    fname = $"{decoder.GetType().Name}-To-{encoder.GetType().Name}-Gray.jpg";
                    fname = TestContext.CurrentContext.GetTestResultPath(fname);
                    bitmap.AsSpanBitmap().ToMemoryBitmap(Pixel.Luminance8.Format).Save(fname, encoder);
                    TestContext.AddTestAttachment(fname);
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

            var bgr = MemoryBitmap<Pixel.BGR24>.Load(filePath, Codecs.STBCodec.Default, Codecs.OpenCvCodec.Default, Codecs.ImageSharpCodec.Default, Codecs.GDICodec.Default, Codecs.SkiaCodec.Default);
            Assert.NotNull(bgr);

            var rgb = MemoryBitmap<Pixel.BGR24>.Load(filePath, Codecs.STBCodec.Default, Codecs.OpenCvCodec.Default, Codecs.ImageSharpCodec.Default, Codecs.GDICodec.Default, Codecs.SkiaCodec.Default);
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
    }
}
