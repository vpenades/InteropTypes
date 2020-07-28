using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using MathNet.Numerics.Statistics;

namespace InteropBitmaps
{
    static class _Utils
    {
        #region statistics (OpenCV like methods)        

        public static IEnumerable<(int X, int Y, TPixel Pixel)> EnumeratePixels<TPixel>(this Image<TPixel> image)
            where TPixel: unmanaged, IPixel<TPixel>
        {
            for(int y=0; y < image.Height; ++y)
            {
                for (int x = 0; x < image.Height; ++x)
                {
                    yield return (x, y, image[x, y]);
                }
            }
        }

        public static Double StandardDeviation(this Image<L8> image)
        {
            return image                
                .EnumeratePixels()
                .Select(item => (float)item.Pixel.PackedValue / 255.0f)
                .StandardDeviation();
        }

        public static Double StandardDeviation(this Image<L16> image)
        {
            return image                
                .EnumeratePixels()
                .Select(item => (float)item.Pixel.PackedValue / 65536.0f)
                .StandardDeviation();
        }

        public static Double GetBlurLevel(this Image img)
        {
            // https://github.com/justadudewhohacks/opencv4nodejs/issues/448#issuecomment-436341650

            // Convert to gray
            using (var gray = img.CloneAs<L16>())
            {
                // Detect blurry level
                var laplacian = new SixLabors.ImageSharp.Processing.Processors.Convolution.Laplacian3x3Processor(false);
                gray.Mutate(laplacian);

                // Get the standard deviation
                var stddev = gray.StandardDeviation();

                // Get the variation
                var result = 1 - Math.Pow(stddev, 2);

                return result; // Math.Pow(result,4); // to make values more meaningful
            }
        }

        #endregion
    }
}
