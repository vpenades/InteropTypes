using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Backends
{
    [Category("Backends GDI")]
    public class GDIToolkitTests
    {
        [TestCase("Resources\\shannon.jpg")]
        [TestCase("Resources\\diagram.jpg")]
        [TestCase("Resources\\white.png")]
        public void LoadImage(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var bitmap = MemoryBitmap.Load(filePath, new Codecs.GDICodec());

            bitmap.AttachToCurrentTest("Result.png");
        }
    }
}
