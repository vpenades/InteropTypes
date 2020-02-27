using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Backends
{
    [Category("Backend Takeuchi")]
    public class TakeuchiFaceRecognitionTests
    {
        [TestCase("Resources\\shannon.jpg")]        
        public void FaceRecognition(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var models = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\Models");

            using (var faceRecog = new TakeuchiFaceDetector(models))
            {
                using (var image = SixLabors.ImageSharp.Image.Load(filePath))
                {
                    var result = faceRecog.FindFaces(image.AsSpanBitmap()).ToArray();

                    TestContext.WriteLine($"{result[0].Left},{result[0].Top} {result[0].Right},{result[0].Bottom}");
                }
            }
        }
    }
}
