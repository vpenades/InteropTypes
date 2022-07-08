﻿using System;
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
