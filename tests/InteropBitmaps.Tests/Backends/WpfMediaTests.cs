using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Backends
{
    [Category("Backends")]
    public class WPFTests
    {
        [SetUp]
        public void SetUp()
        {
            Assert.AreEqual(8, IntPtr.Size, "x64 test environment required");
        }

        [TestCase("Resources\\diagram.jpg")]
        [TestCase("Resources\\white.png")]
        [TestCase("Resources\\shannon.jpg")]
        public void LoadImage(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var uri = new Uri(filePath, UriKind.Absolute);

            var image = new System.Windows.Media.Imaging.BitmapImage(uri);            

            var memory = image.ToMemoryBitmap();

            memory.AttachToCurrentTest("Result.png");

            var writable = new System.Windows.Media.Imaging.WriteableBitmap(image);
            

        }
    }
}
