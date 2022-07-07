using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps.Transform
{
    internal class Kernet3x3Tests
    {
        [Test]
        public void Laplacian1()
        {
            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\shannon.webp");

            // Use SkiaSharp to load a WEBP image:
            var bmp = MemoryBitmap<Pixel.BGR24>.Load(filePath, Codecs.SkiaCodec.Default);            

            bmp.AsSpanBitmap().Apply<Pixel.BGR24, Pixel.RGBA128F>(Laplacian);

            bmp.Save(new AttachmentInfo("laplacian.jpg"));
        }

        private static Pixel.BGR24 Laplacian(in Processing.Kernel3x3<Pixel.RGBA128F> kernel)
        {
            var v = System.Numerics.Vector4.Zero;
            v -= kernel.P11.RGBA;
            v -= kernel.P12.RGBA;
            v -= kernel.P13.RGBA;
            v -= kernel.P21.RGBA;
            v += kernel.P22.RGBA * 8;
            v -= kernel.P23.RGBA;
            v -= kernel.P31.RGBA;
            v -= kernel.P32.RGBA;
            v -= kernel.P33.RGBA;
            v = System.Numerics.Vector4.Min(System.Numerics.Vector4.One, v);
            v = System.Numerics.Vector4.Max(System.Numerics.Vector4.Zero, v);            

            return new Pixel.BGR24(new Pixel.RGBA128F(v));
        }

    }
}
