using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;

using NUnit.Framework;

namespace InteropTypes.Graphics.Backends
{
    internal class MagicScalerTests
    {
        [Test]
        public void TestRescale()
        {
            var bitmap = MemoryBitmap.Load(ResourceInfo.From("shannon.jpg"), InteropTypes.Codecs.GDICodec.Default);

            bitmap = MagicScalerToolkit.Rescale(bitmap, 30, 30);

            AttachmentInfo.From("result.jpg").WriteObject(f => bitmap.Save(f));
        }

        [Test]
        public void TestTransform()
        {
            var bitmap = MemoryBitmap.Load(ResourceInfo.From("shannon.jpg"), InteropTypes.Codecs.GDICodec.Default);

            var xform = Matrix3x2.CreateScale(0.3f) * Matrix3x2.CreateRotation(0.3f) * Matrix3x2.CreateTranslation(20, 20);

            var xformer = MagicScalerToolkit.CreateMatrixTransform(xform, 200, 200);
            var adapter = MagicScalerToolkit.AsPixelSource(bitmap);
            xformer.Init(adapter);

            var result = MagicScalerToolkit.ToMemoryBitmap(xformer);

            AttachmentInfo.From("result.jpg").WriteObject(f => result.Save(f));
        }
    }
}
