using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{
    internal class PixelValuesTests
    {
        [Test]
        public void Swap()
        {
            var a = 1;
            var b = 17;

            a ^= b;
            b ^= a;
            a ^= b;

            Assert.That(a, Is.EqualTo(17));
            Assert.That(b, Is.EqualTo(1));
        }


        /* Sadly, the value returned by the case is a defensive copy.
        [Test]
        public void TestSwitch()
        {
            Assert.AreEqual("hello", _TestSwitch(""));
            Assert.AreEqual(17, _TestSwitch(0));
        }


        private T _TestSwitch<T>(T value)
        {
            switch(value)
            {
                case string typedValue:  typedValue = "hello"; break;
                case int typedValue: typedValue = 17; break;
            }

            return value;
        }*/


        


        [Test]
        public void CheckPixelValues()
        {

            CheckImageSharp<Pixel.BGR565, SixLabors.ImageSharp.PixelFormats.Bgr565>();
            CheckImageSharp<Pixel.BGRA4444, SixLabors.ImageSharp.PixelFormats.Bgra4444>();
            CheckImageSharp<Pixel.BGRA5551, SixLabors.ImageSharp.PixelFormats.Bgra5551>();
            CheckImageSharp<Pixel.RGB24, SixLabors.ImageSharp.PixelFormats.Rgb24>();
            CheckImageSharp<Pixel.BGR24, SixLabors.ImageSharp.PixelFormats.Bgr24>();
            CheckImageSharp<Pixel.RGBA32, SixLabors.ImageSharp.PixelFormats.Rgba32>();
            CheckImageSharp<Pixel.ARGB32, SixLabors.ImageSharp.PixelFormats.Argb32>();
            CheckImageSharp<Pixel.BGRA32, SixLabors.ImageSharp.PixelFormats.Bgra32>();

            CheckImageSharp<Pixel.RGBA128F, SixLabors.ImageSharp.PixelFormats.RgbaVector>();            
        }

        private static unsafe void CheckImageSharp<TPixel,TRefPixel>()
            where TPixel : unmanaged, Pixel.IConvertTo
            where TRefPixel : unmanaged, SixLabors.ImageSharp.PixelFormats.IPixel<TRefPixel>
        {
            Assert.That(sizeof(TPixel), Is.EqualTo(sizeof(TRefPixel)));

            Span<byte> tmp = stackalloc byte[sizeof(TPixel)];
            var pixVal = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, TPixel>(tmp);
            var pixRef = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, TRefPixel>(tmp);

            var cr = new SixLabors.ImageSharp.PixelFormats.Rgba32(255, 127, 63, 31);
            pixRef[0].FromRgba32(cr);

            var cv = pixVal[0].To<Pixel.BGRA32>();
            var f = PixelFormat.TryIdentifyFormat<TPixel>();

            TestContext.Out.WriteLine($"{cv.R} {cv.G} {cv.B} {cv.A}");

            Assert.That(cv.R, Is.GreaterThan(cv.G));
            Assert.That(cv.G, Is.GreaterThan(cv.B));
            if (f.HasUnpremulAlpha) Assert.That(cv.B, Is.GreaterThan(cv.A));
        }
    }
}
