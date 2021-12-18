using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps
{
    using PEF = Pixel.Format.ElementID;

    [Category("Core")]
    public class PixelFormatTests
    {
        [Test]
        public void TestPixelFormatStructure()
        {
            Assert.AreEqual(PEF.Empty, default(Pixel.Format.Element).Id);

            Assert.AreEqual(1, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Pixel.Format.Element)));
            Assert.AreEqual(4, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Pixel.Format)));
        }

        [Test]
        public void ComponentFormatEnumeration()
        {
            #if DEBUG // requires debug mode

            // PixelFormat._GetBitLen(c);

            Assert.AreEqual(0, (int)PEF.Empty);

            var values = Enum.GetValues(typeof(PEF))
                .Cast<PEF>()
                .ToArray();

            // all values of ElementId must be contained within 0 and 255 to fit in 1 byte
            Assert.LessOrEqual(0, values.Select(item => (int)item).Min());
            Assert.GreaterOrEqual(255, values.Select(item => (int)item).Max());

            foreach (var c in values)
            {
                var name = c.ToString();

                var len = new Pixel.Format.Element(c).BitCount;

                if (c == PEF.Empty) Assert.AreEqual(0, len);
                else Assert.Greater(len, 0);

                if (name.EndsWith("1")) Assert.AreEqual(1, len);
                if (name.EndsWith("4")) Assert.AreEqual(4, len);
                if (name.EndsWith("5")) Assert.AreEqual(5, len);
                if (name.EndsWith("8")) Assert.AreEqual(8, len);

                if (name.EndsWith("16")) Assert.AreEqual(16, len);
                else if (name.EndsWith("6")) Assert.AreEqual(6, len);

                if (name.EndsWith("32")) Assert.AreEqual(32, len);
                if (name.EndsWith("32F")) Assert.AreEqual(32, len);
            }

            #endif

            // todo: check the index of any "bit" group is less than the next group.
        }

        [Test]
        public void ConversionTest()
        {
            ConversionTest<Pixel.BGR24, Pixel.BGR24>();
            ConversionTest<Pixel.BGR24, Pixel.BGRA32>();
            ConversionTest<Pixel.BGR24, Pixel.RGBA32>();
            ConversionTest<Pixel.BGR24, Pixel.ARGB32>();

            ConversionTest<Pixel.BGRA32, Pixel.BGR24>();
            ConversionTest<Pixel.BGRA32, Pixel.BGRA32>();
            ConversionTest<Pixel.BGRA32, Pixel.RGBA32>();
            ConversionTest<Pixel.BGRA32, Pixel.ARGB32>();

            ConversionTest<Pixel.RGBA32, Pixel.BGR24>();
            ConversionTest<Pixel.RGBA32, Pixel.BGRA32>();
            ConversionTest<Pixel.RGBA32, Pixel.RGBA32>();
            ConversionTest<Pixel.RGBA32, Pixel.ARGB32>();

            ConversionTest<Pixel.ARGB32, Pixel.BGR24>();
            ConversionTest<Pixel.ARGB32, Pixel.BGRA32>();
            ConversionTest<Pixel.ARGB32, Pixel.RGBA32>();
            ConversionTest<Pixel.ARGB32, Pixel.ARGB32>();

            // premultiplied

            ConversionPremulTest<Pixel.ARGB32, Pixel.BGRP32>();
            ConversionPremulTest<Pixel.BGRP32, Pixel.ARGB32>();

            ConversionPremulTest<Pixel.ARGB32, Pixel.RGBP32>();
            ConversionPremulTest<Pixel.RGBP32, Pixel.ARGB32>();            
        }


        public void ConversionTest<TSrcPixel,TDstPixel>()
            where TSrcPixel : unmanaged, Pixel.IPixelConvertible<Pixel.BGRA32>, Pixel.IPixelFactory<Pixel.BGRA32,TSrcPixel>
            where TDstPixel : unmanaged, Pixel.IPixelConvertible<Pixel.BGRA32>
        {
            var srcFmt = Pixel.Format.TryIdentifyPixel<TSrcPixel>();
            var dstFmt = Pixel.Format.TryIdentifyPixel<TDstPixel>();

            var src = new TSrcPixel[5];
            var dst = new TDstPixel[5];            

            for(int i=0; i < 5; ++i)
            {
                src[i] = default(TSrcPixel).From(new Pixel.BGRA32(i * 50, 255 - i * 50, i * 30, 20 + i * 30));
            }
            
            Pixel.GetPixelConverter<TSrcPixel,TDstPixel>().Invoke(src,dst);
            
            for (int i = 0; i < 5; ++i)
            {
                var srcP = src[i].ToPixel();
                var dstP = dst[i].ToPixel();

                if (!srcFmt.HasAlpha || !dstFmt.HasAlpha)
                {
                    srcP = new Pixel.BGRA32(srcP.R, srcP.G, srcP.B, (Byte)255);
                    dstP = new Pixel.BGRA32(dstP.R, dstP.G, dstP.B, (Byte)255);
                }

                Assert.AreEqual(srcP, dstP);
            }            
        }

        public void ConversionPremulTest<TSrcPixel, TDstPixel>()
            where TSrcPixel : unmanaged, Pixel.IPixelConvertible<Pixel.BGRA32>, Pixel.IPixelFactory<Pixel.BGRA32, TSrcPixel>
            where TDstPixel : unmanaged, Pixel.IPixelConvertible<Pixel.BGRA32>
        {
            var srcFmt = Pixel.Format.TryIdentifyPixel<TSrcPixel>();
            var dstFmt = Pixel.Format.TryIdentifyPixel<TDstPixel>();

            var src = new TSrcPixel[5];
            var dst = new TDstPixel[5];
            var cvt = Pixel.GetByteConverter(srcFmt, dstFmt);

            for (int i = 0; i < 5; ++i)
            {
                src[i] = default(TSrcPixel).From(new Pixel.BGRA32(i * 50, 255 - i * 50, i * 30, 20 + i * 30));
            }

            Pixel.GetPixelConverter<TSrcPixel, TDstPixel>().Invoke(src, dst);

            for (int i = 0; i < 5; ++i)
            {
                var srcA = src[i].ToPixel();
                var dstA = dst[i].ToPixel();

                var srcP = new Pixel.RGBP32(srcA);
                var dstP = new Pixel.RGBP32(dstA);

                Assert.AreEqual(srcP.PreR, dstP.PreR, 1);
                Assert.AreEqual(srcP.PreG, dstP.PreG, 1);
                Assert.AreEqual(srcP.PreB, dstP.PreB, 1);
                Assert.AreEqual(srcP.A, dstP.A, 1);
            }
        }

        [Ignore("just an experiment")]
        [Test]
        public void TestIntegerAlpha255vs256()
        {
            for (int y = 0; y < 256; ++y)
            {
                for (int x = 0; x < 256; ++x)
                {
                    var r1 = (x * y) / 255;

                    
                    uint xx = (uint)x;
                    uint yy = (uint)y;                    
                    yy = yy + (yy >> 7);

                    var r2 = (xx * yy) / 256;

                    Assert.AreEqual(r1, r2);                    
                }
            }

        }

    }
}
