using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Drawing
{
    internal partial class TypeTests
    {
        /*
        private static Byte __Premul(uint vv, uint aa) { vv |= vv << 8; return (Byte)((vv * aa) >> 24); }

        public void ToPremul(ColorStyle value)
        {
            var aa = (uint)value.A; aa |= aa << 8;
            this.PreB = __Premul((uint)value.B, aa);
            this.PreG = __Premul((uint)value.G, aa);
            this.PreR = __Premul((uint)value.R, aa);
            this.A = (Byte)value.A;
        }

        
        public void SetValue(ColorStyle premul)
        {
            if (value.A == 0) this = default;
            else
            {
                uint rcpA = (255 * 256) / (uint)value.A;
                B = (Byte)Math.Min(255, (uint)value.PreB * rcpA / 256);
                G = (Byte)Math.Min(255, (uint)value.PreG * rcpA / 256);
                R = (Byte)Math.Min(255, (uint)value.PreR * rcpA / 256);
            }
        }*/

        

        [Test]
        public void ColorStyleTests()
        {
            Assert.AreEqual(4, System.Runtime.InteropServices.Marshal.SizeOf(typeof(ColorStyle)));            
        }


        [Test]
        public void ColorStylePremulTests()
        {
            // https://stackoverflow.com/questions/35285324/how-to-divide-16-bit-integer-by-255-with-using-sse

            for (int a = 1; a < 256; ++a)
            {
                for (int r = 0; r < 256; ++r)
                {
                    // slow premul
                    var premulSlow = (r * a) / 255;                    

                    // fast premul
                    var aa = 257 * a;
                    var premulFast = (r * aa + 255) >> 16;

                    Assert.LessOrEqual(premulFast, aa);
                    Assert.AreEqual(premulSlow, premulFast);                    
                }
            }
        }


        [Test]
        public void ColorStylePremulAndUnpremulTests()
        {
            // https://stackoverflow.com/questions/56430849/how-to-make-premultiplied-alpha-function-faster-using-simd-instructions
            
            for (int a = 1; a < 256; ++a)
            {
                for (int r = 0; r < 256; ++r)
                {
                    // slow premul
                    var premulSlow = (r * a) / 255;

                    // slow unpremul
                    var unpremulSlow = (premulSlow * 255) / a;

                    // fast premul
                    var aa = 257 * a;
                    var premulFast = (r * aa + 255) >> 16;
                    Assert.LessOrEqual(premulFast, aa);

                    // fast unpremul
                    var rcp = (65536 * 255) / a;
                    var unpremulFast = (premulSlow * rcp + 255) >> 16;
                    Assert.LessOrEqual(unpremulFast, 255);

                    // checks

                    Assert.AreEqual(premulSlow, premulFast);
                    Assert.AreEqual(unpremulSlow, unpremulFast);                    
                }
            }            
        }
    }
}
