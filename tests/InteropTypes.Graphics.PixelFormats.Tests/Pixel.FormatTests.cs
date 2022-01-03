using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps
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

                var pixelType = fmt.GetDefaultPixelType();
                Assert.NotNull(pixelType);

                var pixel = Activator.CreateInstance(pixelType);

                if (pixel is Pixel.IPixelConvertible<Pixel.BGRA32> pixelToRGBA32)
                {
                    var dstp = pixelToRGBA32.ToPixel();
                }

                if (pixel is Pixel.IPixelConvertible<Pixel.RGBA128F> pixelToRGBA128F)
                {
                    var dstp = pixelToRGBA128F.ToPixel();
                }
            }            
        }
    }
}
