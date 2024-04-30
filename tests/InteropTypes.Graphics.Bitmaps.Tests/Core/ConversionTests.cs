using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{
    public class ConversionTests
    {
        [Test]
        public void TestConvertBitmaps()
        {
            var src = new MemoryBitmap(1920, 1080, Pixel.BGRA32.Format);

            var dst = new MemoryBitmap(src.Info.WithPixelFormat<Pixel.RGBA32>());
            dst.SetPixels(0, 0, src);
        }

        [Test]
        public void TestCopyTo()
        {
            var src = new MemoryBitmap(10,10, Pixel.RGB24.Format);

            MemoryBitmap<Pixel.RGBA32> dst = default;

            src.CopyTo(ref dst);
        }


        [Test]
        public void TestReinterpretBitmaps()
        {
            var src = new MemoryBitmap<Pixel.BGRA32>(16, 16);

            src.SetPixel(0, 0, (255, 0, 0, 255));

            var dst = src.AsSpanBitmap().ReinterpretAs<Pixel.RGBA32>();            

            var p = dst.GetPixelUnchecked(0, 0);

            Assert.Multiple(() =>
            {
                Assert.That(p.R, Is.EqualTo(0));
                Assert.That(p.G, Is.EqualTo(0));
                Assert.That(p.B, Is.EqualTo(255)); // formerly R
                Assert.That(p.A, Is.EqualTo(255));
            });

            // float RGB to Vector3

            var bgr = new MemoryBitmap<Pixel.BGR96F>(16, 16);

            var xyz = bgr.AsSpanBitmap().ReinterpretAs<System.Numerics.Vector3>();
        }

    }
}
