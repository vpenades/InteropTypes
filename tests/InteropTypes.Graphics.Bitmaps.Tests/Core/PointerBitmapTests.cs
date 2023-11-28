using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{
    [Category("Core")]
    public class PointerBitmapTests
    {

        [Test]
        public void TestPointerBitmapSlice()
        {
            void _testSlice(PointerBitmap ptr)
            {
                ptr = ptr.Slice((1, 1, 2, 2));

                Assert.That(ptr.IsReadOnly);

                Assert.That(ptr.Width, Is.EqualTo(2));
                Assert.That(ptr.Height, Is.EqualTo(2));
                Assert.That(ptr.StepByteSize, Is.EqualTo(16));

                var ptrSpan = ptr.AsSpanBitmapOfType<int>();

                Assert.That(ptrSpan.GetPixel(0, 0), Is.Zero);
                Assert.That(ptrSpan.GetPixel(1, 0), Is.Zero);
                Assert.That(ptrSpan.GetPixel(0, 1), Is.Zero);
                Assert.That(ptrSpan.GetPixel(1, 1), Is.Zero);
            }

            var bmp = new MemoryBitmap<int>(4, 4, Pixel.ARGB32.Format);

            bmp.SetPixels(int.MaxValue);
            bmp.Slice((1, 1, 2, 2)).SetPixels(0);

            bmp.AsSpanBitmap().PinReadablePointer(_testSlice);
        }

    }
}
