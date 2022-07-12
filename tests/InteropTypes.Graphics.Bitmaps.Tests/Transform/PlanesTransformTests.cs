using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    internal class PlanesTransformTests
    {

        [Test]
        public void DrawTransformedBitmapWithMulAdd()
        {
            _DrawTransformedBitmapWithMulAdd<Pixel.RGB24>(0.6f, 0.4f); // not supported yet
            _DrawTransformedBitmapWithMulAdd<Pixel.BGR24>(0.6f, 0.4f);
            _DrawTransformedBitmapWithMulAdd<Pixel.BGR96F>(0.6f, 0.4f);
            _DrawTransformedBitmapWithMulAdd<Pixel.RGB96F>(0.6f, 0.4f);
        }

        private static void _DrawTransformedBitmapWithMulAdd<TPixel>(float mul, float add)
            where TPixel:unmanaged
        {
            var src = MemoryBitmap<Pixel.BGR24>.Load(ResourceInfo.From("shannon.jpg"));

            var dst = new MemoryBitmap<TPixel>(256, 256);

            var xform = System.Numerics.Matrix3x2.CreateScale(dst.Width / (float)src.Width, dst.Height / (float)src.Height);

            dst.AsSpanBitmap().SetPixels<Pixel.BGR24>(xform, src, true, (mul, add));

            dst.Save(AttachmentInfo.From($"result-{typeof(TPixel).Name}.jpg"));
        }

        [Test]
        public void DrawToPlanesTest()
        {
            var src = MemoryBitmap<Pixel.BGR24>.Load(ResourceInfo.From("shannon.jpg"));

            var dst = new SpanPlanesXYZ<float>(256, 256);            

            var xform = System.Numerics.Matrix3x2.CreateScale(dst.Width / (float)src.Width, dst.Height / (float)src.Height);

            dst.SetPixels<Pixel.BGR24>(xform, src, true);            

            dst.Save(AttachmentInfo.From("result.png"));
        }

    }
}
