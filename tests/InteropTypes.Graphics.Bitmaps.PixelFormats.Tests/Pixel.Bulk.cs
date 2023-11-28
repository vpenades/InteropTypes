using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
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

                    Assert.That(r1, Is.EqualTo(r2));
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
            where TSrcPixel : unmanaged, Pixel.IConvertTo
            where TDstPixel : unmanaged, Pixel.IConvertTo
        {
            var srcFmt = PixelFormat.TryIdentifyFormat<TSrcPixel>();
            var dstFmt = PixelFormat.TryIdentifyFormat<TDstPixel>();

            var src = new TSrcPixel[5];
            var dst = new TDstPixel[5];            

            for(int i=0; i < 5; ++i)
            {
                new Pixel.BGRA32(i * 50, 255 - i * 50, i * 30, 20 + i * 30).CopyTo(ref src[i]);
            }
            
            Pixel.GetPixelCopyConverter<TSrcPixel,TDstPixel>().Invoke(src,dst);            

            for (int i = 0; i < 5; ++i)
            {
                var srcP = src[i].To<Pixel.BGRA32>();
                var dstP = dst[i].To<Pixel.BGRA32>();

                if (!srcFmt.HasUnpremulAlpha || !dstFmt.HasUnpremulAlpha)
                {
                    srcP = new Pixel.BGRA32(srcP.R, srcP.G, srcP.B, (Byte)255);
                    dstP = new Pixel.BGRA32(dstP.R, dstP.G, dstP.B, (Byte)255);
                }

                Assert.That(srcP, Is.EqualTo(dstP));
            }            
        }

        public void ConversionPremulTest<TSrcPixel, TDstPixel>()
            where TSrcPixel : unmanaged, Pixel.IConvertTo
            where TDstPixel : unmanaged, Pixel.IConvertTo
        {
            var srcFmt = PixelFormat.TryIdentifyFormat<TSrcPixel>();
            var dstFmt = PixelFormat.TryIdentifyFormat<TDstPixel>();

            var src = new TSrcPixel[5];
            var dst = new TDstPixel[5];
            var cvt = Pixel.GetByteCopyConverter(srcFmt, dstFmt);

            for (int i = 0; i < 5; ++i)
            {
                new Pixel.BGRA32(i * 50, 255 - i * 50, i * 30, 20 + i * 30).CopyTo(ref src[i]);
            }

            Pixel.GetPixelCopyConverter<TSrcPixel, TDstPixel>().Invoke(src, dst);

            for (int i = 0; i < 5; ++i)
            {
                var srcA = src[i].To<Pixel.BGRA32>();
                var dstA = dst[i].To<Pixel.BGRA32>();

                var srcP = new Pixel.RGBP32(srcA);
                var dstP = new Pixel.RGBP32(dstA);

                Assert.Multiple(() =>
                {
                    Assert.That(dstP.PreR, Is.EqualTo(srcP.PreR).Within(1));
                    Assert.That(dstP.PreG, Is.EqualTo(srcP.PreG).Within(1));
                    Assert.That(dstP.PreB, Is.EqualTo(srcP.PreB).Within(1));
                    Assert.That(dstP.A, Is.EqualTo(srcP.A).Within(1));
                });
            }
        }        
    }
}
