using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{
    public class FormatTests
    {
        [Test]
        public void TestFormatMemoryFootprint()
        {            
            Assert.AreEqual(4, System.Runtime.InteropServices.Marshal.SizeOf(typeof(PixelFormat)));
        }

        [Test]
        public void TestAllFormats()
        {
            foreach(var fmt in PixelFormat.AllFormats)
            {
                TestContext.WriteLine($"{fmt}");

                var pixelType = fmt.GetPixelTypeOrNull();
                Assert.NotNull(pixelType);

                var pixel = Activator.CreateInstance(pixelType);

                if (pixel is Pixel.IConvertTo pixelTo)
                {
                    var dstp1 = pixelTo.To<Pixel.RGBA32>();
                    var dstp2 = pixelTo.To<Pixel.RGBA128F>();
                }
            }            
        }
    }
}
