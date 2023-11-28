using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

using NUnit.Framework;

namespace InteropTypes.Graphics.Backends
{
    [Category("Backends")]
    public class WPFTests
    {
        [SetUp]
        public void SetUp()
        {
            Assert.That(IntPtr.Size, Is.EqualTo(8), "x64 test environment required");
        }

        [TestCase("Resources\\diagram.jpg")]
        [TestCase("Resources\\white.png")]
        [TestCase("Resources\\shannon.jpg")]
        public void LoadImageWPF(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var uri = new Uri(filePath, UriKind.Absolute);

            var image = new System.Windows.Media.Imaging.BitmapImage(uri);            

            var memory = image.ToMemoryBitmap();

            memory.Save(AttachmentInfo.From("Result.png"));

            var writable = new System.Windows.Media.Imaging.WriteableBitmap(image);
        }        

        [Test]
        public void TestCopyWritableBitmap()
        {
            var src256 = new MemoryBitmap<Byte>(256, 256, Pixel.Luminance8.Format)
                .AsSpanBitmap()
                .WithWPF();            

            var src512 = new MemoryBitmap<Byte>(512, 512, Pixel.Luminance8.Format)
                .AsSpanBitmap()
                .WithWPF();           

            WriteableBitmap dst = null;

            bool allPixelsEqualTo(WriteableBitmap bmp, Byte pix)
            {
                return bmp.ToMemoryBitmap()
                    .OfType<Byte>()
                    .EnumeratePixels()
                    .All(item => item.Pixel == pix);
            }

            src256.Source.SetPixels<Byte>(100);
            src256.CopyTo(ref dst);
            Assert.That(dst, Is.Not.Null);
            Assert.That(dst.GetType(), Is.EqualTo(typeof(WriteableBitmap)));
            Assert.That(dst.PixelWidth, Is.EqualTo(256));
            Assert.That(dst.PixelHeight, Is.EqualTo(256));
            Assert.That(allPixelsEqualTo(dst, 100));

            src256.Source.SetPixels<Byte>(105);
            src256.CopyTo(ref dst);
            Assert.That(allPixelsEqualTo(dst, 105));

            src512.Source.SetPixels<Byte>(130);
            src512.CopyTo(ref dst);
            Assert.That(dst, Is.Not.Null);
            Assert.That(dst.GetType(), Is.EqualTo(typeof(WriteableBitmap)));
            Assert.That(dst.PixelWidth, Is.EqualTo(512));
            Assert.That(dst.PixelHeight, Is.EqualTo(512));
            Assert.That(allPixelsEqualTo(dst, 130));

            src512.Source.SetPixels<Byte>(135);
            src512.CopyTo(ref dst);
            Assert.That(allPixelsEqualTo(dst, 135));
        }
    }
}
