using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InteropBitmaps.Codecs
{
    public class SkiaCodec: IBitmapDecoding, IBitmapEncoding
    {
        // note: we should try to operate directly with SkiaSharp.SKCodec

        public MemoryBitmap Read(Stream s)
        {
            using (var img = SkiaSharp.SKBitmap.Decode(s))
            {
                return _Implementation.AsSpanBitmap(img).ToMemoryBitmap();
            }
        }

        public void Write(Stream s, string formatExtension, SpanBitmap bmp)
        {
            var fmt = GetFormatFromExtension(formatExtension);

            using (var skbmp = _Implementation.ToSKImage(bmp))
            {
                skbmp.Encode(fmt, 95).SaveTo(s);
            }
        }

        private static SkiaSharp.SKEncodedImageFormat GetFormatFromExtension(string extension)
        {
            if (extension.EndsWith("png", StringComparison.OrdinalIgnoreCase)) return SkiaSharp.SKEncodedImageFormat.Png;
            if (extension.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)) return SkiaSharp.SKEncodedImageFormat.Jpeg;

            if (extension.EndsWith("gif", StringComparison.OrdinalIgnoreCase)) return SkiaSharp.SKEncodedImageFormat.Gif;
            if (extension.EndsWith("bmp", StringComparison.OrdinalIgnoreCase)) return SkiaSharp.SKEncodedImageFormat.Bmp;            
            if (extension.EndsWith("ico", StringComparison.OrdinalIgnoreCase)) return SkiaSharp.SKEncodedImageFormat.Ico;            

            if (extension.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)) return SkiaSharp.SKEncodedImageFormat.Jpeg;

            if (extension.EndsWith("webp", StringComparison.OrdinalIgnoreCase)) return SkiaSharp.SKEncodedImageFormat.Webp;
            if (extension.EndsWith("wbmp", StringComparison.OrdinalIgnoreCase)) return SkiaSharp.SKEncodedImageFormat.Wbmp;

            if (extension.EndsWith("ast", StringComparison.OrdinalIgnoreCase)) return SkiaSharp.SKEncodedImageFormat.Astc;
            if (extension.EndsWith("dng", StringComparison.OrdinalIgnoreCase)) return SkiaSharp.SKEncodedImageFormat.Dng;
            if (extension.EndsWith("heif", StringComparison.OrdinalIgnoreCase)) return SkiaSharp.SKEncodedImageFormat.Heif;
            if (extension.EndsWith("ktx", StringComparison.OrdinalIgnoreCase)) return SkiaSharp.SKEncodedImageFormat.Ktx;            

            throw new NotSupportedException();
        }
    }
}
