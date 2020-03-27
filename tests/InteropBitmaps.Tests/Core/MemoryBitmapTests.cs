using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps
{
    [Category("Core")]
    public class MemoryBitmapTests
    {
        [Test]
        public void CreateMemoryBitmap()
        {
            var m1 = new MemoryBitmap<UInt32>(16, 16, PixelFormat.Standard.RGBA32);
            m1.SetPixels(0xff406040);

            m1.AttachToCurrentTest("result.png");
        }

        [Test]
        public void SetPixels()
        {
            var dst = new MemoryBitmap<Byte>(16, 16, PixelFormat.Standard.Gray8);
            var src = new MemoryBitmap<Byte>(8, 8, PixelFormat.Standard.Gray8);

            src.SetPixels(50);
            dst.SetPixels(4, 4, src);

            src.SetPixels(255);
            dst.SetPixels(-4, -4, src);

            src.SetPixels(130);
            dst.SetPixels(12, -4, src);

            src.SetPixels(255);
            dst.SetPixels(12, 12, src);

            src.SetPixels(70);
            dst.SetPixels(-4, 12, src);

            dst.SetPixels(-50, 0, src);

            dst.AttachToCurrentTest("Result.png");
        }
    }
}
