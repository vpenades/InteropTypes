using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps
{
    public class BulkTests
    {
        [Test]
        public void TestDivideBy255()
        {
            for (int y = 0; y < 256; ++y)
            {
                for (int x = 0; x < 256; ++x)
                {
                    var r1 = (x * y) / 255;
                    var r2 = (x * y).DivideBy255();

                    Assert.AreEqual(r1, r2);
                }
            }
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
            where TSrcPixel : unmanaged, Pixel.IConvertible<Pixel.BGRA32>, Pixel.IPixelFactory<Pixel.BGRA32,TSrcPixel>
            where TDstPixel : unmanaged, Pixel.IConvertible<Pixel.BGRA32>
        {
            var srcFmt = PixelFormat.TryIdentifyPixel<TSrcPixel>();
            var dstFmt = PixelFormat.TryIdentifyPixel<TDstPixel>();

            var src = new TSrcPixel[5];
            var dst = new TDstPixel[5];            

            for(int i=0; i < 5; ++i)
            {
                src[i] = default(TSrcPixel).From(new Pixel.BGRA32(i * 50, 255 - i * 50, i * 30, 20 + i * 30));
            }
            
            Pixel.GetPixelCopyConverter<TSrcPixel,TDstPixel>().Invoke(src,dst);
            
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
            where TSrcPixel : unmanaged, Pixel.IConvertible<Pixel.BGRA32>, Pixel.IPixelFactory<Pixel.BGRA32, TSrcPixel>
            where TDstPixel : unmanaged, Pixel.IConvertible<Pixel.BGRA32>
        {
            var srcFmt = PixelFormat.TryIdentifyPixel<TSrcPixel>();
            var dstFmt = PixelFormat.TryIdentifyPixel<TDstPixel>();

            var src = new TSrcPixel[5];
            var dst = new TDstPixel[5];
            var cvt = Pixel.GetByteCopyConverter(srcFmt, dstFmt);

            for (int i = 0; i < 5; ++i)
            {
                src[i] = default(TSrcPixel).From(new Pixel.BGRA32(i * 50, 255 - i * 50, i * 30, 20 + i * 30));
            }

            Pixel.GetPixelCopyConverter<TSrcPixel, TDstPixel>().Invoke(src, dst);

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
    }
}
