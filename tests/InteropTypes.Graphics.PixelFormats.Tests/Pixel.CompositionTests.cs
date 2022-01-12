using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps
{
    internal class CompositionTests
    {
        private static readonly Pixel.BGR24 BGR24_A = (10,10,10);

        private static readonly Pixel.BGRA32 BGRA32_A = (255, 127, 63, 255);
        private static readonly Pixel.BGRA32 BGRA32_B = (255, 127, 63, 127);
        private static readonly Pixel.BGRA32 BGRA32_C = (255, 255, 255, 0);
        

        [Test]
        public void TestComposition()
        {
            var raf = ComposeFast<Pixel.BGRA32, Pixel.BGR24>(BGRA32_B, (1, 2, 9), 16384);
            var ras = ComposeSlow<Pixel.BGRA32, Pixel.BGR24>(BGRA32_B, (1, 2, 9), 16384);
            var rpf = ComposeFast<Pixel.BGRP32, Pixel.BGR24>(new Pixel.BGRP32(BGRA32_B), (1, 2, 9), 16384);

            Assert.AreEqual(raf, ras);
            Assert.AreEqual(raf, rpf);
        }
        
        private TDstPixel ComposeFast<TSrcPixel, TDstPixel>(TSrcPixel src, TDstPixel dst, int amount)
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged, Pixel.IPixelCompositionQ<TSrcPixel, TDstPixel>
        {            
            return dst.AlphaBlendWith(src, amount);
        }


        private TDstPixel ComposeSlow<TSrcPixel,TDstPixel>(TSrcPixel src, TDstPixel dst, int amount)
            where TSrcPixel : unmanaged, Pixel.IConvertible<Pixel.BGRP32>
            where TDstPixel : unmanaged, Pixel.IPixelCompositionQ<Pixel.BGRP32, TDstPixel>
        {            
            return dst.AlphaBlendWith(src.ToPixel(), amount);
        }
    }
}
