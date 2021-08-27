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

            ConversionTest<Pixel.ARGB32, Pixel.BGRA32P>();
            ConversionTest<Pixel.BGRA32P, Pixel.ARGB32>();

            ConversionTest<Pixel.ARGB32, Pixel.RGBA32P>();
            ConversionTest<Pixel.RGBA32P, Pixel.ARGB32>();            
        }


        public void ConversionTest<TSrcPixel,TDstPixel>()
            where TSrcPixel : unmanaged, Pixel.IPixelReflection<TSrcPixel>
            where TDstPixel : unmanaged, Pixel.IPixelReflection<TDstPixel>
        {
            var srcFmt = Pixel.Format.TryIdentifyPixel<TSrcPixel>();
            var dstFmt = Pixel.Format.TryIdentifyPixel<TDstPixel>();

            var src = new TSrcPixel[5];
            var dst = new TDstPixel[5];
            var cvt = Pixel.GetConverter(srcFmt,dstFmt);

            for(int i=0; i < 5; ++i)
            {
                src[i] = default(TSrcPixel).From(new Pixel.BGRA32(i * 50, 255 - i * 50, i * 30, 20 + i * 30));
            }

            cvt.Invoke
                (
                System.Runtime.InteropServices.MemoryMarshal.Cast<TSrcPixel, Byte>(src),
                System.Runtime.InteropServices.MemoryMarshal.Cast<TDstPixel, Byte>(dst)
                );

            if (srcFmt.HasPremul || dstFmt.HasPremul)
            {
                for (int i = 0; i < 5; ++i)
                {
                    var srcP = new Pixel.RGBA32P(src[i].ToBGRA32());
                    var dstP = new Pixel.RGBA32P(dst[i].ToBGRA32());                    

                    Assert.AreEqual(srcP.R, dstP.R, 1);
                    Assert.AreEqual(srcP.G, dstP.G, 1);
                    Assert.AreEqual(srcP.B, dstP.B, 1);
                    Assert.AreEqual(srcP.A, dstP.A, 1);
                }
            }
            else
            {
                for (int i = 0; i < 5; ++i)
                {
                    var srcP = src[i].ToBGRA32();
                    var dstP = dst[i].ToBGRA32();

                    if (!srcFmt.HasAlpha || !dstFmt.HasAlpha)
                    {
                        srcP = new Pixel.BGRA32(srcP.R, srcP.G, srcP.B, (Byte)255);
                        dstP = new Pixel.BGRA32(dstP.R, dstP.G, dstP.B, (Byte)255);
                    }

                    Assert.AreEqual(srcP, dstP);
                }
            }
        }

    }
}
