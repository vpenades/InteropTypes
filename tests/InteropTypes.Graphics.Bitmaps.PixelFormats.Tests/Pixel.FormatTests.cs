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
            default(Pixel.Luminance32F).To<Pixel.RGBA32>();

            bool fail = false;

            foreach (var fmt in PixelFormat.AllFormats)
            {
                var pixelType = fmt.GetPixelTypeOrNull();
                Assert.NotNull(pixelType);

                var pixel = Activator.CreateInstance(pixelType);                

                if (pixel is Pixel.IConvertTo pixelTo)
                {
                    try { var dstp1 = pixelTo.To<Pixel.RGB24>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to RGB24"); }
                    try { var dstp1 = pixelTo.To<Pixel.RGB96F>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to RGB96F"); }

                    try { var dstp1 = pixelTo.To<Pixel.RGBA32>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to RGBA32"); }
                    try { var dstp1 = pixelTo.To<Pixel.RGBA128F>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to RGBA128F"); }

                    try { var dstp1 = pixelTo.To<Pixel.RGBP32>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to RGBP32"); }
                    try { var dstp1 = pixelTo.To<Pixel.RGBP128F>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to RGBP128F"); }

                    try { var dstp1 = pixelTo.To<Pixel.Luminance8>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to Luminance8"); }
                    try { var dstp1 = pixelTo.To<Pixel.Luminance32F>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to Luminance32F"); }
                }
            }

            Assert.IsFalse(fail);
        }
    }
}
