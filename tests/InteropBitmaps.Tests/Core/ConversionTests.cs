using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Core
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

    }
}
