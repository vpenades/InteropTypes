using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;

using NUnit.Framework;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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

        /*
        [Test]
        public void TestDrawSprites()
        {
            using var sprite1 = Image.Load<Rgba32>(ResourceInfo.From("cat.png"));            

            using (var dst = new Image<Rgb24>(1024,1024))
            {
                var xform = Matrix3x2.CreateScale(12, 12);
                xform *= Matrix3x2.CreateRotation(1);
                xform *= Matrix3x2.CreateTranslation(120, 120);

                ImageSharpToolkit.DrawSpriteTo(dst, xform, sprite1);

                ImageSharpToolkit.DrawSpriteTo(dst, Matrix3x2.CreateTranslation(10, 10), sprite1);
                ImageSharpToolkit.DrawSpriteTo(dst, Matrix3x2.CreateTranslation(50, 10), sprite1);

                ImageSharpToolkit.DrawSpriteTo(dst, Matrix3x2.CreateScale(3,3) * Matrix3x2.CreateTranslation(100, 10), sprite1);

                AttachmentInfo.From("result.png").WriteObject(dst.Save);
            }
        }*/


    }
}
