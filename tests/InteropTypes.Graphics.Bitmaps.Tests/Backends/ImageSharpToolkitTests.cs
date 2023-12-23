using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;

using NUnit.Framework;

namespace InteropTypes.Graphics.Backends
{
    [Category("Backends")]
    public class ImageSharpTests
    {
        [TestCase("shannon.jpg")]
        [TestCase("diagram.jpg")]
        [TestCase("white.png")]
        public void LoadImage(string filePath)
        {
            filePath = ResourceInfo.From(filePath);

            var bitmap = MemoryBitmap.Load(filePath, InteropTypes.Codecs.ImageSharpCodec.Default);

            bitmap.Save(new AttachmentInfo("Result.png"));
        }
    }
}
