using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using InteropTypes.Graphics;
using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Codecs
{
    /// <remarks>
    /// Images are read in <see cref="Pixel.RGB24"/> format.
    /// </remarks>
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
            var ext = format.ToString().ToLower();

            #if NET6_0_OR_GREATER
            if (!SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager.TryFindFormatByFileExtension(ext, out var fmt)) return false;
            var encoder = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager.GetEncoder(fmt);
            #else
            var fmt = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager.FindFormatByFileExtension(ext);
            if (fmt == null) return false;
            var encoder = SixLabors.ImageSharp.Configuration.Default.ImageFormatsManager.FindEncoder(fmt);
            if (encoder == null) return false;
            #endif

            using (var img = _Implementation.CloneToImageSharp(bmp))
            {
                img.Save(stream.Value, encoder);
            }

            return true;
        }

        #endregion
    }
}
