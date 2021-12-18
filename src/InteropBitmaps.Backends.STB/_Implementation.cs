using System;
using System.Collections.Generic;
using System.Text;

using STBREAD = StbImageSharp;
using STBWRITE = StbImageWriteSharp;

namespace InteropBitmaps
{
    /// <see href="https://github.com/StbSharp/StbImageLib"/>
    static class _Implementation
    {
        public static bool TryGetPixelFormat(STBREAD.ColorComponents components, int bitsPerChannel, out Pixel.Format fmt)
        {
            if (bitsPerChannel == 8)
            {
                switch (components)
                {
                    case STBREAD.ColorComponents.Grey: fmt = Pixel.Luminance8.Format; return true;
                    //case STBREAD.ColorComponents.GreyAlpha:;
                    case STBREAD.ColorComponents.RedGreenBlue: fmt = Pixel.RGB24.Format; return true;
                    case STBREAD.ColorComponents.RedGreenBlueAlpha: fmt = Pixel.RGBA32.Format; return true;                    
                }                
            }

            fmt = default;
            return false;
        }

        public static bool TryGetPixelFormat(STBWRITE.ColorComponents components, out Pixel.Format fmt)
        {            
            switch (components)
            {
                case STBWRITE.ColorComponents.Grey: fmt = Pixel.Luminance8.Format; return true;
                //case STBREAD.ColorComponents.GreyAlpha:;
                case STBWRITE.ColorComponents.RedGreenBlue: fmt = Pixel.RGB24.Format; return true;
                case STBWRITE.ColorComponents.RedGreenBlueAlpha: fmt = Pixel.RGBA32.Format; return true;
            }            

            fmt = default;
            return false;
        }

        public static STBWRITE.ColorComponents GetCompatibleFormat(Pixel.Format fmt)
        {
            switch(fmt.PackedFormat)
            {
                case Pixel.Luminance8.Code:
                case Pixel.Luminance16.Code:
                case Pixel.Luminance32F.Code:
                    return STBWRITE.ColorComponents.Grey;

                case Pixel.BGR565.Code:
                case Pixel.BGR24.Code:
                case Pixel.RGB24.Code:
                case Pixel.RGB96F.Code:
                case Pixel.BGR96F.Code:
                    return STBWRITE.ColorComponents.RedGreenBlue;

                case Pixel.BGRA4444.Code:
                case Pixel.BGRA5551.Code:
                case Pixel.BGRA32.Code:
                case Pixel.RGBA32.Code:
                case Pixel.ARGB32.Code:
                case Pixel.BGRA128F.Code:
                case Pixel.RGBA128F.Code:
                    return STBWRITE.ColorComponents.RedGreenBlueAlpha;
            }

            return STBWRITE.ColorComponents.RedGreenBlueAlpha;
        }

        public static BitmapInfo GetBitmapInfo(STBREAD.ImageResult image)
        {
            if (TryGetPixelFormat(image.ColorComponents, image.BitsPerChannel, out var fmt))
            {
                return new BitmapInfo(image.Width, image.Height, fmt);
            }

            throw new ArgumentException("Unsupported format", nameof(image));            
        }
        
        public static SpanBitmap AsSpanBitmap(STBREAD.ImageResult image)
        {
            var binfo = GetBitmapInfo(image);
            return new SpanBitmap(image.Data, binfo);
        }
    }
}
