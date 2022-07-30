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
    public class GDITests
    {
        [TestCase("Resources\\shannon.jpg")]
        [TestCase("Resources\\diagram.jpg")]
        [TestCase("Resources\\white.png")]
        public void LoadImage(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var bitmap = MemoryBitmap.Load(filePath, InteropTypes.Codecs.GDICodec.Default);

            bitmap.Save(new AttachmentInfo("Result.png"));
        }        

        [Test]
        public void FontRender()
        {
            using(var ff = new GDIFontGlyphFactory())
            {
                var bmp = ff.GetGlyph('A');

                bmp.Save(new AttachmentInfo("A.png"));
            }

        }
    }    
}
