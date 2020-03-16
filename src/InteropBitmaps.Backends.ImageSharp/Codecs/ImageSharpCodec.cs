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

        public bool TryRead(Stream s, out MemoryBitmap bitmap)
        {
            try
            {
                using (var img = SixLabors.ImageSharp.Image.Load(s))
                {
                    bitmap = img.AsSpanBitmap().ToMemoryBitmap();
                }

                return true;
            }
            catch(SixLabors.ImageSharp.UnknownImageFormatException)
            {
                bitmap = null;
                return false;
            }
        }

        public bool TryWrite(Stream s, CodecFormat format, SpanBitmap bmp)
        {
            var fmt = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager.FindFormatByFileExtension(format.ToString().ToLower());
            if (fmt == null) return false;

            var encoder = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager.FindEncoder(fmt);
            if (encoder == null) return false;

            using (var img = _Implementation.CloneToImageSharp(bmp))
            {
                img.Save(s, encoder);
            }

            return true;
        }

        #endregion
    }
}
