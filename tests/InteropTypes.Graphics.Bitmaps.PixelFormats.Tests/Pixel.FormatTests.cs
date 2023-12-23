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
            Assert.That(System.Runtime.InteropServices.Marshal.SizeOf<PixelFormat>(), Is.EqualTo(4), "PixelFormat must be 4 bytes size to ensure memory alignment and small memory footprint");
        }

        [Test]
        public void TestAllFormats()
        {
            default(Pixel.Luminance32F).To<Pixel.RGBA32>();

            bool fail = false;

            foreach (var fmt in PixelFormat.AllFormats)
            {
                var pixelType = fmt.GetPixelTypeOrNull();
                Assert.That(pixelType, Is.Not.Null);                

                var pixel = Activator.CreateInstance(pixelType);                

                if (pixel is Pixel.IConvertTo pixelTo)
                {
                    try { var dstp1 = pixelTo.To<Pixel.Luminance8>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to Luminance8"); }
                    try { var dstp1 = pixelTo.To<Pixel.Luminance16>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to Luminance16"); }
                    try { var dstp1 = pixelTo.To<Pixel.Luminance32F>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to Luminance32F"); }

                    try { var dstp1 = pixelTo.To<Pixel.RGB24>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to RGB24"); }
                    try { var dstp1 = pixelTo.To<Pixel.BGR24>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to BGR24"); }

                    try { var dstp1 = pixelTo.To<Pixel.RGB96F>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to RGB96F"); }
                    try { var dstp1 = pixelTo.To<Pixel.BGR96F>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to BGR96F"); }

                    try { var dstp1 = pixelTo.To<Pixel.RGBA32>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to RGBA32"); }
                    try { var dstp1 = pixelTo.To<Pixel.BGRA32>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to BGRA32"); }
                    try { var dstp1 = pixelTo.To<Pixel.ARGB32>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to ARGB32"); }

                    try { var dstp1 = pixelTo.To<Pixel.RGBA128F>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to RGBA128F"); }

                    try { var dstp1 = pixelTo.To<Pixel.RGBP32>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to RGBP32"); }
                    try { var dstp1 = pixelTo.To<Pixel.BGRP32>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to BGRP32"); }
                    // try { var dstp1 = pixelTo.To<Pixel.RGBP128F>(); } catch { fail = true; TestContext.WriteLine($"{pixelType.Name} to RGBP128F"); }


                }
            }

            Assert.That(fail, Is.False);
        }
    }
}
