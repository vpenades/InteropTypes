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
            var dst = new MemoryBitmap(16, 16, PixelFormat.Standard.Gray8);
            var src = new MemoryBitmap(8, 8, PixelFormat.Standard.Gray8);

            src.Span.WritableSpan.Fill(50);
            dst.SetPixels(4, 4, src);

            src.Span.WritableSpan.Fill(255);
            dst.SetPixels(-4, -4, src);

            src.Span.WritableSpan.Fill(130);
            dst.SetPixels(12, -4, src);

            src.Span.WritableSpan.Fill(255);
            dst.SetPixels(12, 12, src);

            src.Span.WritableSpan.Fill(70);
            dst.SetPixels(-4, 12, src);

            dst.SetPixels(-50, 0, src);

            dst.AttachToCurrentTest("Result.png");
        }
    }
}
