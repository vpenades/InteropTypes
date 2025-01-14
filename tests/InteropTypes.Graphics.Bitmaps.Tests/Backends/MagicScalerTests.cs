using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
