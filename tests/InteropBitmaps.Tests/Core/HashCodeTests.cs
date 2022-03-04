using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SqlServer.Server;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{
    [Category("Core")]
    public class HashCodeTests
    {
        [Test]
        public void TestHashCodes()
        {
            var gray = new MemoryBitmap<Byte>(256, 256, Pixel.Luminance8.Format);
            Assert.AreEqual(256, gray.Info.StepByteSize);
            var grayWithStride = new MemoryBitmap<Byte>(256, 256, Pixel.Luminance8.Format, 320);
            Assert.AreEqual(320, grayWithStride.Info.StepByteSize);


            var rnd = new Random(117);
            for(int i=0; i < 256*256; ++i)
            {
                gray.SetPixel(i & 255, i / 256, (Byte)rnd.Next());
            }

            grayWithStride.SetPixels(0, 0, gray);

            Assert.AreEqual(gray.GetHashCode(), grayWithStride.GetHashCode());

            var span = gray.AsSpanBitmap();
            var less = span.AsTypeless();
            var hash = gray.GetHashCode();

            Assert.AreEqual(1953103375, hash);
            Assert.AreEqual(hash, gray.AsTypeless().GetHashCode());
            Assert.AreEqual(hash, span.GetHashCode());
            Assert.AreEqual(hash, less.GetHashCode());
        }
    }
}
