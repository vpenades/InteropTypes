using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InteropBitmaps.Codecs
{
    [System.Diagnostics.DebuggerDisplay("ImageSharp Codec")]
    public sealed class ImageSharpCodec : IBitmapDecoder, IBitmapEncoder
    {
        #region lifecycle

        static ImageSharpCodec() { }

        private ImageSharpCodec() { }

        private static readonly ImageSharpCodec _Default = new ImageSharpCodec();

        public static ImageSharpCodec Default => _Default;

        #endregion

        #region API

        /// <inheritdoc/>
        public bool TryRead(BitmapDecoderContext context, out MemoryBitmap bitmap)
        {
            try
            {
                using (var img = SixLabors.ImageSharp.Image.Load(context.Stream))
                {
                    bitmap = img.ToMemoryBitmap();
                }

                return true;
            }
            catch(SixLabors.ImageSharp.UnknownImageFormatException)
            {
                bitmap = default;
                return false;
            }
        }

        /// <inheritdoc/>
        public bool TryWrite(Lazy<Stream> stream, CodecFormat format, SpanBitmap bmp)
        {
            var fmt = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager.FindFormatByFileExtension(format.ToString().ToLower());
            if (fmt == null) return false;

            var encoder = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager.FindEncoder(fmt);
            if (encoder == null) return false;

            using (var img = _Implementation.CloneToImageSharp(bmp))
            {
                img.Save(stream.Value, encoder);
            }

            return true;
        }

        #endregion
    }
}
