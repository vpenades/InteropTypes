using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Core
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

                Assert.AreEqual(true, ptr.IsReadOnly);

                Assert.AreEqual(2, ptr.Width);
                Assert.AreEqual(2, ptr.Height);
                Assert.AreEqual(16, ptr.StepByteSize);

                var ptrSpan = ptr.AsSPanBitmapOfType<int>();

                Assert.AreEqual(0, ptrSpan.GetPixel(0, 0));
                Assert.AreEqual(0, ptrSpan.GetPixel(1, 0));
                Assert.AreEqual(0, ptrSpan.GetPixel(0, 1));
                Assert.AreEqual(0, ptrSpan.GetPixel(1, 1));
            }

            var bmp = new MemoryBitmap<int>(4, 4, PixelFormat.Standard.ARGB32);

            bmp.SetPixels(int.MaxValue);
            bmp.Slice((1, 1, 2, 2)).SetPixels(0);

            bmp.AsSpanBitmap().PinReadablePointer(_testSlice);
        }

    }
}
