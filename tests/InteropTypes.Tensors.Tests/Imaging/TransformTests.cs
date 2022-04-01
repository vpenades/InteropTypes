using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Tensors
{
    internal class TransformTests
    {
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public struct Pix
        {
            public static implicit operator Pix((int r, int g, int b) item) { return new Pix(item.r, item.g, item.b); }
            public Pix(int r, int g, int b)
            {
                R = (Byte)r; G = (Byte)g; B = (Byte)b;
            }

            public Byte R;
            public Byte G;
            public Byte B;

            public override int GetHashCode()
            {
                return R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode();
            }
        }

        [Test]
        public void TestPixelTransferWithTransform()
        {
            var src = new SpanTensor2<Pix>(16, 16);
            var dst = new SpanTensor2<Pix>(16, 16);

            for (int y = 0; y < src.Dimensions[0]; y++)
            {
                var row = src[y];

                for (int x = 0; x < src.Dimensions[1]; x++)
                {
                    row[x] = (y * 16, x * 16, x + y*2);
                }                
            }            

            var srcX =
                System.Numerics.Matrix3x2.CreateScale(5)
                * System.Numerics.Matrix3x2.CreateRotation(0);
            srcX.Translation = new System.Numerics.Vector2(-5, -5);

            SpanTensor2<Pix>.FillPixels(dst, src, srcX, true);
            
            for(int y=0; y < dst.Dimensions[0]; ++y)
            {
                string l = string.Empty;

                for(int x=0; x < dst.Dimensions[1]; ++x)
                {
                    l += $"{dst[y][x].B:000-}";
                }

                TestContext.WriteLine(l);
            }
        }
    }
}
