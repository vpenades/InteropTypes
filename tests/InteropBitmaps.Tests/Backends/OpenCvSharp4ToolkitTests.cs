using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Backends
{
    [Category("Backends")]
    public class OpenCvTests
    {
        [SetUp]
        public void SetUp()
        {
            Assert.AreEqual(8, IntPtr.Size, "x64 test environment required");
        }

        [TestCase("Resources\\shannon.jpg")]
        [TestCase("Resources\\diagram.jpg")]
        [TestCase("Resources\\white.png")]
        public void LoadImage(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var bitmap = MemoryBitmap.Load(filePath, Codecs.OpenCvCodec.Default);

            bitmap.AttachToCurrentTest("Result.png");
        }

        [Test]
        public void WarpAffine()
        {
            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\shannon.jpg");

            var src = MemoryBitmap.Load(filePath, Codecs.OpenCvCodec.Default);
            var dst = new MemoryBitmap(512, 512, src.Info.PixelFormat);

            var xform = System.Numerics.Matrix3x2.CreateScale(1.3f, 1.3f) * System.Numerics.Matrix3x2.CreateRotation(0.25f);
            xform.Translation = new System.Numerics.Vector2(5, 40);

            using (PerformanceBenchmark.Run(t => TestContext.WriteLine($"OpenCV {t}")))
            {
                OpenCvSharp4Toolkit.WarpAffine(src, dst, xform);
            }
            dst.AttachToCurrentTest("result.opencv.jpg");            

            dst.AsSpanBitmap().WritableBytes.Fill(0);
            using (PerformanceBenchmark.Run(t => TestContext.WriteLine($"Soft {t}")))
            {
                dst.AsSpanBitmap().SetPixels(xform, src);
            }
            dst.AttachToCurrentTest("result.soft.jpg");
        }

        
    }
}
