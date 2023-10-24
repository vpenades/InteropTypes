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



        [TestCase("Resources\\shannon.webp", 15)]
        [TestCase("Resources\\cat.png", 5)]
        public void Blur1(string resName, int ksize)
        {
            var filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, resName);
            
            var bmp = MemoryBitmap<Pixel.BGRA32>.Load(filePath, Codecs.SkiaCodec.Default);


            Processing.KernelNxM<Pixel.BGRA32>.Apply(bmp, (ksize, ksize), BoxGlow);

            bmp.Save(new AttachmentInfo("result.png"));
        }


        private static TPixel BoxGlow<TPixel>(in Processing.KernelNxM<TPixel> kernel)
            where TPixel : unmanaged
        {
            var top = default(Pixel.BGRP32);
            var bot = default(Pixel.BGRP32);

            top.Set(kernel[kernel.Width / 2, kernel.Height / 2]); // central
            bot.Set(BoxBlur(kernel));            

            bot.SetSourceOver(top, 256);

            return bot.To<TPixel>();
        }


        private static TPixel BoxBlur<TPixel>(in Processing.KernelNxM<TPixel> kernel)
            where TPixel : unmanaged
        {
            var xyzw = System.Numerics.Vector4.Zero;
            var rgbp = default(Pixel.RGBP32);

            for (int y = 0; y < kernel.Height; ++y)
            {
                for (int x = 0; x < kernel.Width; ++x)
                {
                    rgbp.Set(kernel[x, y]);
                    xyzw += new System.Numerics.Vector4(rgbp.PreR,rgbp.PreG, rgbp.PreB, rgbp.A) / 255f;
                }
            }

            xyzw /= kernel.Width * kernel.Height;

            xyzw = System.Numerics.Vector4.Min(System.Numerics.Vector4.One, xyzw);
            xyzw = System.Numerics.Vector4.Max(System.Numerics.Vector4.Zero, xyzw);            

            return new Pixel.RGBP128F(xyzw).To<TPixel>();
        }


        private static Pixel.BGRA32 Blur(in Processing.KernelNxM<Pixel.RGBP128F> kernel, Processing.KernelNxM<Pixel.Luminance32F> weights)
        {
            var v = System.Numerics.Vector4.Zero;            

            for (int y = 0; y < kernel.Height; ++y)
            {
                for (int x = 0; x < kernel.Width; ++x)
                {
                    var p = kernel[x, y];
                    var w = weights[x, y];

                    v += p.RGBP * w.L;                    
                }
            }
            
            v = System.Numerics.Vector4.Min(System.Numerics.Vector4.One, v);
            v = System.Numerics.Vector4.Max(System.Numerics.Vector4.Zero, v);

            return new Pixel.BGRA32(new Pixel.RGBP128F(v));
        }

        

    }
}
