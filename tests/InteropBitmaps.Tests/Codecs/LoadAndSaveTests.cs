using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var codecs = new IBitmapDecoding[] { OpenCvCodec.Default, GDICodec.Default, WPFCodec.Default, ImageSharpCodec.Default, STBCodec.Default, SkiaCodec.Default };

            foreach (var decoder in codecs.OfType<IBitmapDecoding>())
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var bitmap = MemoryBitmap.Load(filePath, decoder);
                sw.Stop();

                TestContext.WriteLine($"Loading {System.IO.Path.GetFileName(filePath)} with {decoder} tool {sw.ElapsedMilliseconds}");                

                foreach (var encoder in codecs.OfType<IBitmapEncoding>())
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
                    bitmap.AsSpanBitmap().ToMemoryBitmap(PixelFormat.Standard.Gray8).Save(fname, encoder);
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
    }
}
