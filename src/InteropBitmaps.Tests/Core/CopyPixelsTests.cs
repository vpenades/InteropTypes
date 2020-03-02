using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Core
{
    [Category("Core")]
    public class CopyPixelsTests
    {
        [Test]
        public void CropAndCopyPixels()
        {
            var dst = new MemoryBitmap(16, 16, PixelFormat.Standard.GRAY8);
            var src = new MemoryBitmap(8, 8, PixelFormat.Standard.GRAY8);

            dst.SetPixels(4, 4, src);

            dst.SetPixels(-4, -4, src);


        }
    }
}
