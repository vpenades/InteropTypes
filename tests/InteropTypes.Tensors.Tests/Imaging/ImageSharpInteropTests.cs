using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace InteropTypes.Tensors.Imaging
{
    internal class ImageSharpInteropTests
    {
        [TestCase(false, ColorEncoding.RGB)]
        [TestCase(true, ColorEncoding.RGB)]
        [TestCase(false, ColorEncoding.BGR)]
        [TestCase(true, ColorEncoding.BGR)]
        [TestCase(true, ColorEncoding.RGBA)]
        public void TestBasicTensorFactory(bool usePlanes, ColorEncoding dstEncoding)
        {
            AttachmentInfo.From("_readme.txt").WriteAllText("hello world");

            _TestBasicTensorFactory<Rgb24>(usePlanes, dstEncoding);            
            _TestBasicTensorFactory<Bgr24>(usePlanes, dstEncoding);

            _TestBasicTensorFactory<Rgba32>(usePlanes, dstEncoding);
        }

        private static void _TestBasicTensorFactory<TSrcPixel>(bool usePlanes, ColorEncoding dstEncoding)
            where TSrcPixel:unmanaged, IPixel<TSrcPixel>
        {
            var srcPixName = typeof(TSrcPixel).Name;

            var imgPath = ResourceInfo.From("dog.jpeg");

            using (var img = SixLabors.ImageSharp.Image.Load<TSrcPixel>(imgPath))
            {
                var tfactory = img.CreateTensorBitmapFactory<TSrcPixel,float>();

                var dst = _CreateDst(usePlanes, dstEncoding, img.Width, img.Height);
                tfactory.TryTransferPixelsToTensorBitmap(dst);
                dst.AttachToCurrentTest($"{srcPixName}-frame-transfer.png");

                var ranges = dst.EvaluateGetContentColorRanges();


                dst = _CreateDst(usePlanes, dstEncoding, 128, 256);
                tfactory.TryFitPixelsToTensorBitmap(dst);
                dst.AttachToCurrentTest($"{srcPixName}-frame-stretch.png");

                dst = _CreateDst(usePlanes, dstEncoding, 128, 256);
                tfactory.TryFitPixelsToTensorBitmap(dst, 0);
                dst.AttachToCurrentTest($"{srcPixName}-frame-fit-min.png");

                dst = _CreateDst(usePlanes, dstEncoding, 128, 256);
                tfactory.TryFitPixelsToTensorBitmap(dst, 1);
                dst.AttachToCurrentTest($"{srcPixName}-frame-fit-max.png");

                dst = _CreateDst(usePlanes, dstEncoding, 128, 256);
                tfactory.TryFitPixelsToTensorBitmap(dst, new System.Drawing.RectangleF(-64, -128, 128, 256), 0);
                dst.AttachToCurrentTest($"{srcPixName}-frame-fit-min-offs-out.png");

                dst = _CreateDst(usePlanes, dstEncoding, 128, 256);
                tfactory.TryFitPixelsToTensorBitmap(dst, System.Numerics.Matrix3x2.CreateScale(0.5f) * System.Numerics.Matrix3x2.CreateRotation(1) * System.Numerics.Matrix3x2.CreateTranslation(75,20));
                dst.AttachToCurrentTest($"{srcPixName}-frame-rotation.png");

                var dog = new System.Drawing.RectangleF(52, 4, 96, 172);

                dst = _CreateDst(usePlanes, dstEncoding, 128, 256);
                tfactory.TryFitPixelsToTensorBitmap(dst, dog);
                dst.AttachToCurrentTest($"{srcPixName}-dog-stretch.png");

                dst = _CreateDst(usePlanes, dstEncoding, 128, 256);
                tfactory.TryFitPixelsToTensorBitmap(dst, dog, 0);
                dst.AttachToCurrentTest($"{srcPixName}-dog-fit-min.png");

                dst = _CreateDst(usePlanes, dstEncoding, 128, 256);
                tfactory.TryFitPixelsToTensorBitmap(dst, dog, 1);
                dst.AttachToCurrentTest($"{srcPixName}-dog-fit-max.png");
            }
        }

        private static TensorBitmap<float> _CreateDst(bool usePlanes, ColorEncoding dstEncoding, int w, int h)
        {
            var range = new ColorRanges.Serializable();
            range.RedMin = -1;
            range.RedMax = 1;
            range.GreenMin = 0;
            range.GreenMax = 255;
            range.BlueMin = 0;
            range.BlueMax = 1;

            // range = ColorRanges.Serializable.Identity;

            return usePlanes
                ? Imaging.TensorBitmap<float>.CreateInterleaved(w,h, dstEncoding, range) // create as planes
                : Imaging.TensorBitmap<float>.CreatePlanes(w,h, dstEncoding, range);     // create as interleaved
        }
    }
}
