using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Codecs
{
    public static class CodecFactory
    {
        public static CodecFormat ParseFormat(string extension)
        {
            if (extension.EndsWith("png", StringComparison.OrdinalIgnoreCase)) return CodecFormat.Png;
            if (extension.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)) return CodecFormat.Jpeg;

            if (extension.EndsWith("gif", StringComparison.OrdinalIgnoreCase)) return CodecFormat.Gif;
            if (extension.EndsWith("bmp", StringComparison.OrdinalIgnoreCase)) return CodecFormat.Bmp;
            if (extension.EndsWith("ico", StringComparison.OrdinalIgnoreCase)) return CodecFormat.Icon;

            if (extension.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)) return CodecFormat.Jpeg;

            if (extension.EndsWith("webp", StringComparison.OrdinalIgnoreCase)) return CodecFormat.Webp;
            if (extension.EndsWith("wbmp", StringComparison.OrdinalIgnoreCase)) return CodecFormat.Wbmp;

            if (extension.EndsWith("ast", StringComparison.OrdinalIgnoreCase)) return CodecFormat.Astc;
            if (extension.EndsWith("dng", StringComparison.OrdinalIgnoreCase)) return CodecFormat.Dng;
            if (extension.EndsWith("heif", StringComparison.OrdinalIgnoreCase)) return CodecFormat.Heif;
            if (extension.EndsWith("ktx", StringComparison.OrdinalIgnoreCase)) return CodecFormat.Ktx;

            if (extension.EndsWith("tif", StringComparison.OrdinalIgnoreCase)) return CodecFormat.Tiff;

            throw new CodecException();
        }
    }
}
