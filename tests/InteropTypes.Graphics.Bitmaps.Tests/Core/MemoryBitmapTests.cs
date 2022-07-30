using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{
    [Category("Core")]
    public class MemoryBitmapTests
    {
        [Test]
        public void CreateMemoryBitmap()
        {
            var m1 = new MemoryBitmap<UInt32>(16, 16, Pixel.BGRA32.Format);
            m1.SetPixels(0xff406040);

            m1.Save(new AttachmentInfo("result.png"));
        }

        [Test]
        public void CreateMemoryBitmapWithKnownType()
        {
            var m1 = new MemoryBitmap<Pixel.BGRA32>(16, 16);
            Assert.AreEqual(Pixel.BGRA32.Format, m1.PixelFormat);

            var m2 = new MemoryBitmap<Pixel.BGRA32>(new Byte[256*4], 16, 16);
            Assert.AreEqual(Pixel.BGRA32.Format, m2.PixelFormat);
        }

        [Test]
        public void CopyPixelsWithStride()
        {
            var m1 = new MemoryBitmap<UInt32>(16, 16, Pixel.BGRA32.Format);            
            m1.SetPixel(0, 0, 0xff00ff00);
            m1.SetPixel(0, 1, 0xffff00ff);

            var m2 = new MemoryBitmap<UInt32>(16, 16, Pixel.BGRA32.Format, 17 * 4);
            m2.SetPixels(0, 0, m1.AsSpanBitmap());

            m2.Save(new AttachmentInfo("result.png"));

            Assert.AreEqual(0xff00ff00, m2.GetPixel(0, 0));
            Assert.AreEqual(0xffff00ff, m2.GetPixel(0, 1));            
        }

        [Test]
        public void SetPixels()
        {
            var dst = new MemoryBitmap<Byte>(16, 16, Pixel.Luminance8.Format);
            var src = new MemoryBitmap<Byte>(8, 8, Pixel.Luminance8.Format);

            src.SetPixels(50);
            dst.SetPixels(4, 4, src.AsSpanBitmap());

            src.SetPixels(255);
            dst.SetPixels(-4, -4, src.AsSpanBitmap());

            src.SetPixels(130);
            dst.SetPixels(12, -4, src.AsSpanBitmap());

            src.SetPixels(255);
            dst.SetPixels(12, 12, src.AsSpanBitmap());

            src.SetPixels(70);
            dst.SetPixels(-4, 12, src.AsSpanBitmap());

            dst.SetPixels(-50, 0, src.AsSpanBitmap());

            dst.Save(new AttachmentInfo("Result.png"));
        }

        [Test]
        public void CopyGreyPixels()
        {
            var src = new MemoryBitmap<float>(177, 177).Slice((10, 10, 150, 150));
            var dst = new MemoryBitmap<Byte>(177, 177).Slice((10, 10, 150, 150));

            src.SetPixels(1);

            var (min, max) = SpanBitmap.MinMax(src);
            Assert.AreEqual(min, 1);
            Assert.AreEqual(max, 1);

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

        [System.Diagnostics.DebuggerDisplay("{ToString(),nq}")]
        [StructLayout(LayoutKind.Sequential)]
        struct PixelBGR : IEquatable<PixelBGR>
        {
            public override string ToString()
            {
                return $"<{B:X2} {G:X2} {R:X2}>";
            }

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
