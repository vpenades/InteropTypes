using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using InteropTypes.Graphics;
using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

using PhotoSauce.MagicScaler;

namespace InteropTypes.Codecs
{
    
    [System.Diagnostics.DebuggerDisplay("MagicScaler Codec")]
    public sealed class MagicScalerCodec : IBitmapDecoder, IBitmapEncoder
    {
        #region lifecycle

        static MagicScalerCodec() { }

        private MagicScalerCodec() { }        

        public static MagicScalerCodec Default { get; } = new MagicScalerCodec();

        #endregion

        #region API

        /// <inheritdoc/>
        public bool TryRead(BitmapDecoderContext context, out MemoryBitmap bitmap)
        {
            try
            {
                var settings = new ProcessImageSettings
                {
                    // The default profile mode may convert to other well-known color spaces,
                    // but there is currently no way to retrieve the profile. Normalizing to
                    // sRGB ensures the color space is known.
                    ColorProfileMode = ColorProfileMode.ConvertToSrgb
                };

                using var pipeline = MagicImageProcessor.BuildPipeline(context.Stream, settings);

                bitmap = _Implementation.ToMemoryBitmap(pipeline.PixelSource);

                return true;
            }
            catch
            {
                bitmap = default;
                return false;
            }
        }

        /// <inheritdoc/>
        public bool TryWrite(Lazy<Stream> stream, CodecFormat format, SpanBitmap bmp)
        {
            string mimeType = null;

            switch(format)
            {
                case CodecFormat.Jpeg: mimeType = ImageMimeTypes.Jpeg; break;
                case CodecFormat.Bmp: mimeType = ImageMimeTypes.Bmp; break;
                case CodecFormat.Dds: mimeType = ImageMimeTypes.Dds; break;
                case CodecFormat.Gif: mimeType = ImageMimeTypes.Gif; break;
                case CodecFormat.Png: mimeType = ImageMimeTypes.Png; break;
                case CodecFormat.Tiff: mimeType = ImageMimeTypes.Tiff; break;
                case CodecFormat.WebpLossless: mimeType = ImageMimeTypes.Webp; break;
                case CodecFormat.WebpLossy: mimeType = ImageMimeTypes.Webp; break;
            }

            if (mimeType == null) return false;            

            var settings = new ProcessImageSettings
            {
                // The default profile mode may convert to other well-known color spaces,
                // but there is currently no way to retrieve the profile. Normalizing to
                // sRGB ensures the color space is known.
                ColorProfileMode = ColorProfileMode.ConvertToSrgb
            };            

            if (!settings.TrySetEncoderFormat(mimeType)) return false;

            var s = stream.Value;

            if (!s.CanWrite || !s.CanSeek) return false;

            var src = new _BitmapAdapter(bmp);

            using var pipeline = MagicImageProcessor.BuildPipeline(src, settings);

            pipeline.WriteOutput(s);

            return true;
        }

        #endregion
    }
}
