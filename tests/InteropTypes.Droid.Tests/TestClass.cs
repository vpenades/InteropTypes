using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using NUnit.Framework;

using InteropBitmaps;

namespace InteropTypes.Droid.Tests
{
    [TestFixture]
    public class Example0
    {
        [Test]
        public void CreateImmutableBitmap()
        {
            using var bmp1 = new BitmapInfo(256, 256, Pixel.RGBA32.Format)
                .ToAndroidFactory()
                .CreateCompatibleBitmap();

            using var bmp2 = bmp1.Copy(bmp1.GetConfig(), false);

            Assert.IsTrue(bmp1.IsMutable);
            Assert.IsFalse(bmp2.IsMutable);

            // it seems that inmutable bitmaps can still be locked:

            var bmp2Id = bmp2.GenerationId;
            var ptr = bmp2.LockPixels();
            Assert.AreNotEqual(IntPtr.Zero, ptr);
            bmp2.UnlockPixels();
            Assert.AreNotEqual(bmp2Id, bmp2.GenerationId);
        }

        [Test]
        public async Task RecycleBitmap()
        {
            Android.Graphics.Bitmap bmp = null;

            new MemoryBitmap<Pixel.RGBA32>(100, 101).AsTypeless().CopyTo(ref bmp);
            Assert.AreEqual(100, bmp.Width);
            Assert.AreEqual(101, bmp.Height);

            default(MemoryBitmap<Pixel.RGBA32>).AsTypeless().CopyTo(ref bmp);
            Assert.IsNull(bmp);

            new MemoryBitmap<Pixel.RGBA32>(101, 100).AsTypeless().CopyTo(ref bmp);
            Assert.IsFalse(bmp.IsRecycled);
            Assert.AreEqual(101, bmp.Width);
            Assert.AreEqual(100, bmp.Height);

            bmp.Dispose();

        }

        [Test]
        public async Task CreateBitmap()
        {
            using var bmp = new BitmapInfo(256, 256, Pixel.RGBA32.Format)
                .ToAndroidFactory()
                .CreateCompatibleBitmap();

            MemoryBitmap<Pixel.RGBA32> dst = default;

            bmp.CopyTo(ref dst);

            dst.AsSpanBitmap().AsTypeless().SetPixels(System.Drawing.Color.Red);
            dst.AsSpanBitmap().AsTypeless().Slice(new BitmapBounds(5,5,255-10,255-10)).SetPixels(new Random());
            dst.AsSpanBitmap().AsTypeless().CopyTo(bmp);

            TestContext.WriteLine($"{dst.Width} {dst.Height}");

            await TestContext.CurrentContext.AttachToCurrentTest("test1.png", bmp);
        }

        [Test]
        public async Task DrawBitmap()
        {
            using var bmp1 = new BitmapInfo(256, 256, Pixel.RGBA32.Format)
                .ToAndroidFactory()
                .CreateCompatibleBitmap();

            using var bmp2 = bmp1.UsingMemoryBitmap();            

            var dc = bmp2.Bitmap.CreateDrawingContext((64,64));
            dc.DrawEllipse((10, 10), 8, 8, System.Drawing.Color.Red);
            dc.DrawEllipse((20, 20), 8, 8, System.Drawing.Color.Green);
            dc.DrawEllipse((30, 30), 8, 8, System.Drawing.Color.Blue);
            dc.DrawConsoleFont((20, 8), "Hello!", System.Drawing.Color.Gray);

            await TestContext.CurrentContext.AttachToCurrentTest("test2.png", bmp1);
        }
    }
}
