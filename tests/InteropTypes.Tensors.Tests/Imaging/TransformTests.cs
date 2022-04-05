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
        public void TestPixelTransferWithTransform()
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

            dst.FillPixels(src, srcX, MultiplyAdd.Identity, true);

            // dst.FitPixels(src, MultiplyAdd.Identity, true);            

            dst.AttachToCurrentTest("tensorTransform.png");
        }
    }
}
