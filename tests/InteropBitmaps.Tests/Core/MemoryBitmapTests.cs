using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

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

        [Test]
        public void CopyGreyPixels()
        {
            var src = new MemoryBitmap<float>(177, 177).Slice((10, 10, 150, 150));
            var dst = new MemoryBitmap<Byte>(177, 177).Slice((10, 10, 150, 150));

            src.SetPixels(1);

            var minmax = SpanBitmap.MinMax(src);
            Assert.AreEqual(minmax.min, 1);
            Assert.AreEqual(minmax.max, 1);

            SpanBitmap.CopyPixels(src, dst, (0,128), (0, 255));
            Assert.IsTrue(dst.EnumeratePixels().All(p => p.Pixel == 128));

            SpanBitmap.CopyPixels(src, dst, (0,1), (10, 255));
            Assert.IsTrue(dst.EnumeratePixels().All(p => p.Pixel == 10));            

            SpanBitmap.CopyPixels(src, dst, (0,1), (2, 3));
            Assert.IsTrue(dst.EnumeratePixels().All(p => p.Pixel == 2));
        }

        [Test]
        public void CopyRGBPixels()
        {
            var src = new MemoryBitmap<Vector3>(177, 177).Slice((10, 10, 150, 150));
            var dst = new MemoryBitmap<PixelBGR>(177, 177).Slice((10, 10, 150, 150));

            src.SetPixels(Vector3.One);

            Assert.IsTrue(SpanBitmap.ArePixelsEqual(src, src));

            SpanBitmap.CopyPixels(src, dst, (0,128), (0, 255));
            Assert.IsTrue(dst.EnumeratePixels().All(p => p.Pixel.Equals(new PixelBGR(128))));

            SpanBitmap.CopyPixels(src, dst, (0,1), (10, 255));
            Assert.IsTrue(dst.EnumeratePixels().All(p => p.Pixel.Equals(new PixelBGR(10))));

            SpanBitmap.CopyPixels(src, dst, (0,1), (2, 3));
            Assert.IsTrue(dst.EnumeratePixels().All(p => p.Pixel.Equals(new PixelBGR(2))));

            Assert.IsTrue(SpanBitmap.ArePixelsEqual(dst, dst));
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PixelBGR : IEquatable<PixelBGR>
        {
            public PixelBGR(Byte gray) { R = G = B = gray; }

            public Byte B;
            public Byte G;
            public Byte R;

            public bool Equals(PixelBGR other)
            {
                if (this.B != other.B) return false;
                if (this.G != other.G) return false;
                if (this.R != other.R) return false;
                return true;
            }
        }
    }
}
