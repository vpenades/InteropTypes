using System;

namespace InteropBitmaps.IO
{
    /// <summary>
    /// 
    /// </summary>
    /// <see href="https://github.com/StbSharp/StbImageLib"/>
    public static partial class STB
    {
        public static StbImageLib.ImageResult Load(string filePath)
        {
            using (var s = System.IO.File.OpenRead(filePath))
            {
                return StbImageLib.ImageResult.FromStream(s, StbImageLib.ColorComponents.RedGreenBlueAlpha);
            }
        }

        public static PixelFormat ToInteropFormat(this (StbImageLib.ColorComponents Components, int BitsPerChannel) fmt)
        {
            if (fmt.BitsPerChannel == 8)
            {
                switch (fmt.Components)
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

        public static BitmapInfo GetBitmapInfo(this StbImageLib.ImageResult image)
        {
            var fmt = (image.ColorComponents, image.BitsPerChannel).ToInteropFormat();

            return new BitmapInfo(image.Width, image.Height, fmt);
        }

        public static SpanBitmap AsSpanBitmap(this StbImageLib.ImageResult image)
        {
            var binfo = image.GetBitmapInfo();
            return new SpanBitmap(image.Data, binfo);

        }


    }
}
