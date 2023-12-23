using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

using NUnit.Framework;

namespace InteropTypes.Graphics.Backends
{
    [Category("Backends")]
    public class OpenCvTests
    {
        [SetUp]
        public void SetUp()
        {
            Assert.That(IntPtr.Size, Is.EqualTo(8), "x64 test environment required");
        }

        [TestCase("shannon.jpg")]
        [TestCase("diagram.jpg")]
        [TestCase("white.png")]
        public void LoadImage(string filePath)
        {
            filePath = ResourceInfo.From(filePath);

            var bitmap = MemoryBitmap.Load(filePath, InteropTypes.Codecs.OpenCvCodec.Default);

            bitmap.Save(new AttachmentInfo("Result.png"));
        }

        [Test]
        public void WarpAffineTransform()
        {
            var filePath = ResourceInfo.From("shannon.jpg");            

            var src = MemoryBitmap.Load(filePath, InteropTypes.Codecs.OpenCvCodec.Default);
            var dst = new MemoryBitmap(512, 512, src.Info.PixelFormat);

            var xform = System.Numerics.Matrix3x2.CreateScale(1.3f, 1.3f) * System.Numerics.Matrix3x2.CreateRotation(0.25f);
            xform.Translation = new System.Numerics.Vector2(5, 40);            

            using (PerformanceBenchmark.Run(t => TestContext.WriteLine($"OpenCV {t}")))
            {
                using var bm = PerformanceBenchmark.Run(result => TestContext.WriteLine(result.TotalMilliseconds) );

                OpenCvSharp4Toolkit.WarpAffine(src, dst, xform);
            }
            dst.Save(new AttachmentInfo("result.opencv.jpg"));            

            dst.AsSpanBitmap().WritableBytes.Fill(0);
            using (PerformanceBenchmark.Run(t => TestContext.WriteLine($"Soft {t}")))
            {
                using var bm = PerformanceBenchmark.Run(result => TestContext.WriteLine(result.TotalMilliseconds));

                dst.AsSpanBitmap().SetPixels(xform, src);
            }
            dst.Save(new AttachmentInfo("result.soft.jpg"));
        }

        [TestCase("Aruco\\capture1.jpg")]
        [TestCase("Aruco\\capture2.jpg")]
        public void TryDetectAruco4x4(string filePath)
        {
            filePath = ResourceInfo.From(filePath);
            var bitmap = MemoryBitmap.Load(filePath, InteropTypes.Codecs.OpenCvCodec.Default);            

            var arucoContext = new Vision.Backends.MarkersContext();

            using (var arucoEstimator = new Vision.Backends.MarkersContext.ArucoEstimator())
            {
                arucoEstimator.SetCameraCalibrationDefault();
                arucoEstimator.Inference(arucoContext, bitmap);
            }

            foreach(var item in arucoContext.Markers)
            {
                TestContext.WriteLine($"{item.Id} {item.A} {item.B} {item.C} {item.D}");
            }

            bitmap
                .CreateDrawingContext()
                .DrawAsset(System.Numerics.Matrix3x2.Identity, arucoContext);

            bitmap.Save(new AttachmentInfo("Result.png"));
        }


    }
}
