using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Backends
{
    [Category("WPF Backend")]
    public class WpfMediaTests
    {
        [TestCase("Resources\\diagram.jpg")]
        [TestCase("Resources\\white.png")]
        public void LoadImage(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var uri = new Uri(filePath, UriKind.Absolute);

            var image = new System.Windows.Media.Imaging.BitmapImage(uri);            

            var memory = image.ToMemoryBitmap();

            memory.AttachToCurrentTest("Result.png");

            var writable = new System.Windows.Media.Imaging.WriteableBitmap(image);
            var span = writable.AsSpanBitmap();
        }
    }
}
