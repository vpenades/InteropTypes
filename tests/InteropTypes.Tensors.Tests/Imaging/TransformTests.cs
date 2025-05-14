using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using BGR24 = InteropTypes.Graphics.Bitmaps.Pixel.BGR24;

namespace InteropTypes.Tensors
{
    internal class TransformTests
    {       

        [Test]
        public void TestPixelTransferWithTransform1()
        {
            var src = new SpanTensor2<BGR24>(16, 16);
            var dst = new SpanTensor2<BGR24>(50, 50);

            for (int y = 0; y < src.Dimensions[0]; y++)
            {
                var row = src[y];

                for (int x = 0; x < src.Dimensions[1]; x++)
                {
                    row[x] = (y * 16, x * 16, x + y*2);
                }                
            }            

            var srcX =
                System.Numerics.Matrix3x2.CreateScale(1)
                * System.Numerics.Matrix3x2.CreateRotation(0.3f);
            srcX.Translation = new System.Numerics.Vector2(-15, -35);

            dst.AsTensorBitmap<Byte>(Imaging.ColorEncoding.Undefined).FillPixels(src.AsBitmapSampler(Imaging.ColorEncoding.BGR), (srcX, MultiplyAdd.Identity));

            // dst.FitPixels(src, MultiplyAdd.Identity, true);            

            dst.AttachToCurrentTest("tensorTransform.png");
        }


        [TestCase(false, 857224970, 4000518195)]
        [TestCase(true, 898913221, 886900266)]
        public void TestPixelTransferWithTransform2(bool useBilinear, long hashCodeResult1, long hashCodeResult2)
        {
            // load image
            var img = Graphics.Bitmaps.MemoryBitmap<BGR24>.Load(ResourceInfo.From("dog.jpeg"));            

            if (!img.AsSpanBitmap().TryGetAsSpanTensor(out var src)) throw new InvalidOperationException();            

            // some transform
            var xform
                = System.Numerics.Matrix3x2.CreateScale(0.4f)
                * System.Numerics.Matrix3x2.CreateRotation(0.3f);
            xform.Translation = new System.Numerics.Vector2(15, 35);            

            // bytes

            var dst1 = new SpanTensor2<BGR24>(384, 384);

            var time = System.Diagnostics.Stopwatch.StartNew();

            dst1.AsTensorBitmap<Byte>(Imaging.ColorEncoding.BGR)
                .FillPixels(src.AsBitmapSampler(Imaging.ColorEncoding.BGR), (xform, MultiplyAdd.Identity, useBilinear));
            time.Stop();

            TestContext.Out.WriteLine($"{time.Elapsed.TotalMilliseconds}");            

            dst1.AttachToCurrentTest($"dog.xform.{useBilinear}.png");            

            Assert.That(dst1.GetChecksum(), Is.EqualTo(hashCodeResult1));

            // floats

            var dst2 = new SpanTensor2<System.Numerics.Vector3>(384, 384);

            time = System.Diagnostics.Stopwatch.StartNew();
            dst2.AsTensorBitmap<float>(Imaging.ColorEncoding.BGR)
                .FillPixels(src.AsBitmapSampler(Imaging.ColorEncoding.BGR), (xform, MultiplyAdd.Identity, useBilinear));
            time.Stop();
            TestContext.Out.WriteLine($"{time.Elapsed.TotalMilliseconds}");

            Assert.That(dst2.GetChecksum(), Is.EqualTo(hashCodeResult2));
        }
    }
}
