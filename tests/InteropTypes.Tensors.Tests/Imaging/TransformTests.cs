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

            dst.AsBitmap(Imaging.ColorEncoding.Undefined).FillPixels<BGR24>(src, (srcX, MultiplyAdd.Identity));

            // dst.FitPixels(src, MultiplyAdd.Identity, true);            

            dst.AttachToCurrentTest("tensorTransform.png");
        }


        [TestCase(false)]
        [TestCase(true)]
        public void TestPixelTransferWithTransform2(bool useBilinear)
        {
            var imgPath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", "dog.jpeg");

            var img = Graphics.Bitmaps.MemoryBitmap<BGR24>.Load(imgPath);

            if (!img.AsSpanBitmap().TryGetAsSpanTensor(out var src))
            {
                throw new InvalidOperationException();
            }
            
            var dst = new SpanTensor2<BGR24>(384, 384);
            var dst2 = new SpanTensor2<System.Numerics.Vector3>(384, 384);

            var srcX =
                System.Numerics.Matrix3x2.CreateScale(0.4f)
                * System.Numerics.Matrix3x2.CreateRotation(0.3f);
            srcX.Translation = new System.Numerics.Vector2(15, 35);

            var time = System.Diagnostics.Stopwatch.StartNew();
            dst.AsBitmap(Imaging.ColorEncoding.Undefined).FillPixels<BGR24>(src, (srcX, MultiplyAdd.Identity, useBilinear));
            time.Stop();
            TestContext.WriteLine($"{time.Elapsed.TotalMilliseconds}");

            time = System.Diagnostics.Stopwatch.StartNew();
            dst2.AsBitmap(Imaging.ColorEncoding.Undefined).FillPixels<BGR24>(src, (srcX, MultiplyAdd.Identity, useBilinear));
            time.Stop();
            TestContext.WriteLine($"{time.Elapsed.TotalMilliseconds}");   

            dst.AttachToCurrentTest($"tensorTransform-{useBilinear}.png");
        }
    }
}
