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
        public void TestReinterpretBitmaps()
        {
            var src = new MemoryBitmap<Pixel.BGRA32>(16, 16, Pixel.BGRA32.Format);

            src.SetPixel(0, 0, (255, 0, 0, 255));

            var dst = src.AsSpanBitmap().ReinterpretAs<Pixel.RGBA32>();            

            var p = dst.GetPixel(0, 0);

            Assert.AreEqual(0, p.R);
            Assert.AreEqual(0, p.G);
            Assert.AreEqual(255, p.B); // formerly R
            Assert.AreEqual(255, p.A);
        }

    }
}
