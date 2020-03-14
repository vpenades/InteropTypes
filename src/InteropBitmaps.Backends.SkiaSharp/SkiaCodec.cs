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

        public MemoryBitmap Read(Stream s)
        {
            /*
            using (var skc = SkiaSharp.SKCodec.Create(s))
            {
                if (!skc.GetFrameInfo(0, out SkiaSharp.SKCodecFrameInfo finfo)) return null;                
            }*/

            using (var img = SkiaSharp.SKBitmap.Decode(s))
            {
                return _Implementation.AsSpanBitmap(img).ToMemoryBitmap();
            }
        }

        public void Write(Stream s, CodecFormat format, SpanBitmap bmp)
        {
            var fmt = GetFormatFromExtension(format);

            var clr = _Implementation.ToSkia(PixelFormat.Standard.RGBA32);            
            if (bmp.PixelFormat.Elements.Any(item => PixelFormat.IsGrey(item))) clr = _Implementation.ToSkia(PixelFormat.Standard.GRAY8);

            using (var skbmp = _Implementation.ToSKImage(bmp, clr))
            {
                var data = skbmp.Encode(fmt, 95);
                    
                data.SaveTo(s);
            }
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
                default: throw new CodecException();
            }            
        }
    }
}
