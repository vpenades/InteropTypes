using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace InteropBitmaps.Interop
{
    static class OpenCVProcessing
    {
        public static Mat ExtractSubjectSiluette(this Mat image)
        {
            var bgPixVal = image.Get<Vec3b>(1, 1); // reference background pixel

            var upperB = new Scalar(bgPixVal.Item0 + 100, bgPixVal.Item1 + 100, bgPixVal.Item2 + 100);

            var binary = image.InRange((Scalar)bgPixVal, upperB);
            var output = new Mat(binary.Height, binary.Width, MatType.CV_8U);
            Cv2.BitwiseNot(binary, output);

            return output;
        }

        public static Mat ExtractColorBlob(Vec3b Col, Mat Image)
        {
            Scalar upperB = new Scalar(Col.Item0 + 10, Col.Item1 + 10, Col.Item2 + 10);
            Mat binary = new Mat(Image.Height, Image.Width, MatType.CV_8U);
            Cv2.InRange(Image, (Scalar)Col, upperB, binary);
            Mat output = new Mat(Image.Height, Image.Width, MatType.CV_8U);
            Cv2.BitwiseNot(binary, output);
            return output;
        }

        public static Mat ExtractBlobTexture(this Mat colorImage, Mat mask)
        {
            Mat outIm = Mat.Zeros(colorImage.Size(), colorImage.Type());
            colorImage.CopyTo(outIm, mask);
            return outIm;
        }

        public static Mat ExtractSegmentalMarker(this Mat image)
        {
            #pragma warning disable CA2000 // Dispose objects before losing scope
            var output = new Mat();
            #pragma warning restore CA2000 // Dispose objects before losing scope

            var SegMarkCol = image.Get<Vec3b>(5, 5);

            output = ExtractColorBlob(SegMarkCol, image);

            return output;
        }


        public static Mat EdgeDetector(this Mat image)
        {
            // Mat src = Cv2.ImRead("lenna.png", ImreadModes.GrayScale);
            Mat dst = new Mat();

            Cv2.Canny(image, dst, 50, 200);
            return dst;
        }        
    }
}
