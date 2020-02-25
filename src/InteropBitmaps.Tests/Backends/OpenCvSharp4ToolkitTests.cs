using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Backends
{
    [Category("ImageSharp Backend")]
    public class OpenCvSharp4ToolkitTests
    {
        [TestCase("Resources\\diagram.jpg")]
        [TestCase("Resources\\white.png")]
        public void LoadImage(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var bitmap = new System.IO
                .FileInfo(filePath)
                .LoadMemoryBitmapFromOpenCvSharp4();

            TestContext.WriteLine($"{filePath} > ({bitmap.Width},{bitmap.Height})");
        }
    }
}
