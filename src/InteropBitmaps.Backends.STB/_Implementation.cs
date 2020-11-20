using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <see href="https://github.com/StbSharp/StbImageLib"/>
    static class _Implementation
    {
        public static Pixel.Format ToPixelFormat(StbImageLib.ColorComponents components, int bitsPerChannel)
        {
            if (bitsPerChannel == 8)
            {
                switch (components)
                {
                    case StbImageLib.ColorComponents.Grey: return Pixel.Luminance8.Format;
                    //case StbImageLib.ColorComponents.GreyAlpha:;
                    case StbImageLib.ColorComponents.RedGreenBlue: return Pixel.RGB24.Format;
                    case StbImageLib.ColorComponents.RedGreenBlueAlpha: return Pixel.RGBA32.Format;
                    default: throw new NotImplementedException();
                }
            }

            throw new NotImplementedException();
        }

        public static BitmapInfo GetBitmapInfo(StbImageLib.ImageResult image)
        {
            var fmt = ToPixelFormat(image.ColorComponents, image.BitsPerChannel);

            return new BitmapInfo(image.Width, image.Height, fmt);
        }

        public static SpanBitmap AsSpanBitmap(StbImageLib.ImageResult image)
        {
            var binfo = GetBitmapInfo(image);
            return new SpanBitmap(image.Data, binfo);
        }
    }
}
