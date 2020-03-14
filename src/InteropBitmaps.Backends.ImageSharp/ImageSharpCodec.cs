using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InteropBitmaps.Codecs
{
    [System.Diagnostics.DebuggerDisplay("ImageSharp Codec")]
    public sealed class ImageSharpCodec : IBitmapDecoding, IBitmapEncoding
    {
        #region lifecycle

        static ImageSharpCodec() { }

        private ImageSharpCodec() { }

        private static readonly ImageSharpCodec _Default = new ImageSharpCodec();

        public static ImageSharpCodec Default => _Default;

        #endregion

        #region API

        public MemoryBitmap Read(Stream s)
        {
            using (var img = SixLabors.ImageSharp.Image.Load(s))
            {
                return img.AsSpanBitmap().ToMemoryBitmap();
            }
        }

        public void Write(Stream s, CodecFormat format, SpanBitmap bmp)
        {
            var fmt = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager.FindFormatByFileExtension(format.ToString().ToLower());
            if (fmt == null) throw new CodecException();

            var encoder = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager.FindEncoder(fmt);
            if (encoder == null) throw new CodecException();

            using (var img = _Implementation.ToImageSharp(bmp))
            {
                img.Save(s, encoder);
            }
        }

        #endregion
    }
}
