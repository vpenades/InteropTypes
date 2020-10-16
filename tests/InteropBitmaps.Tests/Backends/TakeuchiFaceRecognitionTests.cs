using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

using POINTF = SixLabors.ImageSharp.PointF;



namespace InteropBitmaps.Backends
{
    [Category("Computer Vision")]
    public class TakeuchiTests
    {
        [SetUp]
        public void SetUp()
        {
            Assert.AreEqual(8, IntPtr.Size, "x64 test environment required");
        }

        [TestCase("Resources\\shannon.jpg")]        
        public void FaceRecognition(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var models = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\Models");

            using (var faceRecog = new Detectors.TakeuchiFaceDetector(models))
            {
                using (var image = SixLabors.ImageSharp.Image.Load(filePath))
                {
                    var swatch = System.Diagnostics.Stopwatch.StartNew();
                    var bounds = faceRecog.FindFaces(image.AsSpanBitmap());
                    TestContext.WriteLine($"{bounds.Count()} Faces detected in {swatch.ElapsedMilliseconds}ms");

                    foreach (var r in bounds)
                    {
                        image.Mutate(dc => dc.DrawPolygon(SixLabors.ImageSharp.Color.Red, 2, (r.Left, r.Top), (r.Right, r.Top), (r.Right, r.Bottom), (r.Left, r.Bottom)));
                    }

                    swatch.Restart();
                    var features = faceRecog.FindLandmarks(image.AsSpanBitmap());
                    TestContext.WriteLine($"{bounds.Count()} Features detected in {swatch.ElapsedMilliseconds}ms");

                    foreach (var r in features)
                    {
                        foreach(var cluster in r)
                        {
                            var points = cluster.Value
                                .Select(item => new POINTF(item.Point.X, item.Point.Y))
                                .ToArray();

                            image.Mutate(dc => dc.DrawLines(SixLabors.ImageSharp.Color.Blue, 2, points));
                        }                        
                    }

                    image.AttachToCurrentTest("Result.png");
                }
            }
        }
    }
}
