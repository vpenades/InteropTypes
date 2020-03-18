using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <see href="https://github.com/StbSharp/StbImageLib"/>
    static class _Implementation
    {
        public static PixelFormat ToPixelFormat(StbImageLib.ColorComponents components, int bitsPerChannel)
        {
            if (bitsPerChannel == 8)
            {
                switch (components)
                {
                    case StbImageLib.ColorComponents.Grey: return PixelFormat.Standard.GRAY8;
                    //case StbImageLib.ColorComponents.GreyAlpha:;
                    case StbImageLib.ColorComponents.RedGreenBlue: return PixelFormat.Standard.RGB24;
                    case StbImageLib.ColorComponents.RedGreenBlueAlpha: return PixelFormat.Standard.RGBA32;
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
