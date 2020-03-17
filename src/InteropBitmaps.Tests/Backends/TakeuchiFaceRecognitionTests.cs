using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SixLabors.ImageSharp.Processing;

using NUnit.Framework;

namespace InteropBitmaps.Backends
{
    [Category("Detector Takeuchi")]
    public class TakeuchiFaceRecognitionTests
    {
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

                    var result = faceRecog.FindFaces(image.AsSpanBitmap());

                    TestContext.WriteLine($"{result.Count()} Faces detected in {swatch.ElapsedMilliseconds}ms");

                    foreach(var r in result)
                    {
                        image.Mutate(dc => dc.DrawPolygon(SixLabors.ImageSharp.Color.Red, 2, (r.Left, r.Top), (r.Right, r.Top), (r.Right, r.Bottom), (r.Left, r.Bottom)));
                    }

                    image.AttachToCurrentTest("Result.png");
                }
            }
        }
    }
}
