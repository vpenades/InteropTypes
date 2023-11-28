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
        [Ignore("Hash code is based on underlaying Span<T>.HashCode")]
        [Test]
        public void TestHashCodes()
        {
            var gray = new MemoryBitmap<Byte>(256, 256, Pixel.Luminance8.Format);
            Assert.That(gray.Info.StepByteSize, Is.EqualTo(256));
            var grayWithStride = new MemoryBitmap<Byte>(256, 256, Pixel.Luminance8.Format, 320);
            Assert.That(grayWithStride.Info.StepByteSize, Is.EqualTo(320));


            var rnd = new Random(117);
            for(int i=0; i < 256*256; ++i)
            {
                gray.SetPixel(i & 255, i / 256, (Byte)rnd.Next());
            }

            grayWithStride.SetPixels(0, 0, gray.AsSpanBitmap());

            Assert.That(grayWithStride.GetHashCode(), Is.EqualTo(gray.GetHashCode()));

            var span = gray.AsSpanBitmap();
            var less = span.AsTypeless();
            var hash = gray.GetHashCode();

            Assert.That(hash, Is.EqualTo(1953103375));
            Assert.That(gray.AsTypeless().GetHashCode(), Is.EqualTo(hash));
            Assert.That(span.GetHashCode(), Is.EqualTo(hash));
            Assert.That(less.GetHashCode(), Is.EqualTo(hash));
        }
    }
}
