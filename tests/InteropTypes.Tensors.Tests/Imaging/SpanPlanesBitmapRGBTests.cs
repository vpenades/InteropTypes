using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends;

using NUnit.Framework;

namespace InteropTypes.Tensors.Imaging
{    
    internal class SpanPlanesBitmapRGBTests
    {
        [Test]
        public void TestConversion()
        {
            var array = new byte[256 * 256 * 3];
            new Random().NextBytes(array);

            var image = new SpanTensor3<byte>(array, 256, 256, 3).UpCast<InteropTypes.Graphics.Bitmaps.Pixel.RGB24>();
            var tensor = new SpanTensor3<float>(3, 256, 256);

            var planes = new SpanPlanesBitmapRGB<float>(tensor, ColorEncoding.RGB);
            planes.SetBitmap<InteropTypes.Graphics.Bitmaps.Pixel.RGB24>(image, ColorEncoding.RGB);

            planes.ApplyMultiplyAdd(new MultiplyAdd(2f / 255f, -1));


            planes.ApplyMultiplyAdd(new MultiplyAdd(1, 1).ConcatMul(255f/2f));

            planes.CopyTo(image, ColorEncoding.RGB);


            var ascii = AsciiImage.Create(14, image.Span, pix => (byte)((pix.R + pix.G + pix.B) / 3), 256, 256, 256);

            var result = image.DownCast<byte>().Span.ToArray();

            Assert.That(result, Is.EqualTo(array));
        }

        [Test]
        public void RoundtripTensorConversion()
        {
            var tensor = new SpanTensor2<System.Numerics.Vector3>(128, 128);

            tensor.FitSixLaborsImage(ResourceInfo.From("shannon.jpg"));

            using(var attachDir = new AttachmentDirectory())
            {
                tensor.SaveToImageSharp(attachDir.Directory.DefineFile("result.jpg"));
            }
        }

        [Test]
        public void RoundtripPlanesConversion()
        {
            var planes = new SpanPlanesBitmapRGB<float>(128, 128);

            planes.FitSixLaborsImage(ResourceInfo.From("shannon.jpg"));

            using (var attachDir = new AttachmentDirectory())
            {
                planes.SaveToImageSharp(attachDir.Directory.DefineFile("result.jpg"));
            }
        }
    }
}
