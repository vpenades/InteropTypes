using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    public static partial class BitmapsToolkit
    {
        /// <summary>
        /// Determines the perceived sharpness of an image.
        /// </summary>
        /// <param name="image">The image to analyze</param>
        /// <returns>A value between (0-1), the higher the number, the sharper the image is</returns>
        /// <remarks>
        /// <see href="https://github.com/justadudewhohacks/opencv4nodejs/issues/448#issuecomment-436341650">Detect blurry images with Laplacian Variance</see>
        /// </remarks>
        public static Double GetPerceivedSharpness(this SpanBitmap image, double power = 1)
        {
            var sharpness = new Processing._SharpnessAnalyzer(power);

            sharpness.AddImage(image);            
                        
            return sharpness.SharpnessFactor;
        }        
    }
}
