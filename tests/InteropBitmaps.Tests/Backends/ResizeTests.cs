using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;

using NUnit.Framework;

namespace InteropTypes.Graphics.Backends
{
    [Category("Backends")]
    public class ResizeTests
    {
        [Test]
        public void ResizeImage()
        {
            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\shannon.jpg");

            var bitmap = MemoryBitmap
                .Load(filePath, Codecs.SkiaCodec.Default)
                .AsSpanBitmap();

            bitmap
                .WithGDI()
                .ToResizedMemoryBitmap(32, 32)
                .AttachToCurrentTest("GDI_Resized.png");

            bitmap
                .WithOpenCv()
                .ToResizedMemoryBitmap(32, 32, OpenCvSharp.InterpolationFlags.Lanczos4)
                .AttachToCurrentTest("OpenCV_Resized.png");

            bitmap
                .WithImageSharp()
                .ToResizedMemoryBitmap(32,32)
                .AttachToCurrentTest("ImageSharp_Resized.png");
        }

    }
}
