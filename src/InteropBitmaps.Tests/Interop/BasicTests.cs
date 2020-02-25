using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Interop
{
    [Category("Interop - Basics")]
    public class BasicTests
    {
        [TestCase("Resources\\shannon.jpg")]        
        public void CalculateImageBlurFactor(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            // load image using GDI
            var bitmap = new System.IO
                .FileInfo(filePath)
                .LoadMemoryBitmapFromGDI();

            // Calculate blur factor using ImageSharp
            var blurfactor1 = bitmap
                .AsImageSharp<SixLabors.ImageSharp.PixelFormats.Argb32>()
                .CalculateBlurFactor();

            // blur with openCV
            bitmap.AsOpenCVSharp().Blur((5, 5));

            // Calculate blur factor using ImageSharp
            var blurfactor2 = bitmap
                .AsImageSharp<SixLabors.ImageSharp.PixelFormats.Argb32>()
                .CalculateBlurFactor();

            // blur with openCV
            bitmap.AsOpenCVSharp().Blur((32, 32));

            // Calculate blur factor using ImageSharp
            var blurfactor3 = bitmap
                .AsImageSharp<SixLabors.ImageSharp.PixelFormats.Argb32>()
                .CalculateBlurFactor();

            TestContext.WriteLine($"{filePath} => {blurfactor1}, {blurfactor2}, {blurfactor3}");

            bitmap.AttachToCurrentTest("final.png");
        }
    }
}
