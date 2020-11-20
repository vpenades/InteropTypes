using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InteropBitmaps.Codecs
{
    [System.Diagnostics.DebuggerDisplay("SkiaSharp Codec")]
    public class SkiaCodec: IBitmapDecoding, IBitmapEncoding
    {
        #region lifecycle

        static SkiaCodec() { }

        private SkiaCodec() { }

        private static readonly SkiaCodec _Default = new SkiaCodec();

        public static SkiaCodec Default => _Default;

        #endregion

        // TODO: we should try to operate directly with SkiaSharp.SKCodec

        public bool TryRead(Stream s, out MemoryBitmap bitmap)
        {
            bitmap = default;

            /*
            using (var skc = SkiaSharp.SKCodec.Create(s))
            {
                if (!skc.GetFrameInfo(0, out SkiaSharp.SKCodecFrameInfo finfo)) return null;                
            }*/

            using (var img = SkiaSharp.SKBitmap.Decode(s))
            {
                if (img == null) return false;
                bitmap = _Implementation.AsSpanBitmap(img).ToMemoryBitmap();
            }

            return true;
        }

        public bool TryWrite(Stream s, CodecFormat format, SpanBitmap bmp)
        {
            try
            {
                var fmt = GetFormatFromExtension(format);

                var clr = _Implementation.ToPixelFormat(Pixel.RGBA32.Format);
                if (bmp.PixelFormat.Elements.Any(item => item.IsGrey)) clr = _Implementation.ToPixelFormat(Pixel.Luminance8.Format);

                if (clr.Color == SkiaSharp.SKColorType.Unknown) return false;

                using (var skbmp = _Implementation.ToSKImage(bmp, clr))
                {
                    var data = skbmp.Encode(fmt, 95);

                    data.SaveTo(s);
                }
            }
            catch (ArgumentException) { return false; }

            return true;
        }

        private static SkiaSharp.SKEncodedImageFormat GetFormatFromExtension(CodecFormat xfmt)
        {
            switch(xfmt)
            {
                case CodecFormat.Astc: return SkiaSharp.SKEncodedImageFormat.Astc;
                case CodecFormat.Bmp: return SkiaSharp.SKEncodedImageFormat.Bmp;
                case CodecFormat.Dng: return SkiaSharp.SKEncodedImageFormat.Dng;
                case CodecFormat.Gif: return SkiaSharp.SKEncodedImageFormat.Gif;
                case CodecFormat.Heif: return SkiaSharp.SKEncodedImageFormat.Heif;
                case CodecFormat.Icon: return SkiaSharp.SKEncodedImageFormat.Ico;
                case CodecFormat.Jpeg: return SkiaSharp.SKEncodedImageFormat.Jpeg;
                case CodecFormat.Ktx: return SkiaSharp.SKEncodedImageFormat.Ktx;
                case CodecFormat.Pkm: return SkiaSharp.SKEncodedImageFormat.Pkm;
                case CodecFormat.Png: return SkiaSharp.SKEncodedImageFormat.Png;
                case CodecFormat.Wbmp: return SkiaSharp.SKEncodedImageFormat.Wbmp;
                case CodecFormat.Webp: return SkiaSharp.SKEncodedImageFormat.Webp;
                default: throw new ArgumentException();
            }            
        }
    }
}
