using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Codecs
{
    public static class CodecFactory
    {
        private const string _CodecError000 = "incompatible codecs must not write to the stream.";

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

        internal static void Write(Lazy<System.IO.Stream> stream, CodecFormat format, IBitmapEncoder[] encoders, SpanBitmap bmp)
        {
            Guard.NotNull(nameof(stream), stream);
            Guard.NotNull(nameof(encoders), encoders);
            Guard.IsFalse(nameof(format), format == CodecFormat.Undefined);
            Guard.GreaterThan(nameof(encoders), encoders.Length, 0);

            // if the stream has been created already, keep the start position:

            long position = 0;
            bool keepOpen = false;

            if (stream.IsValueCreated)
            {
                Guard.IsTrue(nameof(stream), stream.Value.CanWrite);
                position = stream.Value.Position;
                keepOpen = true;
            }

            // loop over each encoder:

            foreach (var encoder in encoders)
            {
                if (encoder.TryWrite(stream, format, bmp)) return;

                if (!stream.IsValueCreated) continue;

                // if encoder failed, amend the stream
                
                if (!keepOpen) throw new ArgumentException(_CodecError000, nameof(encoders));

                if (stream.Value.Position != position)
                {
                    if (!stream.Value.CanSeek) throw new ArgumentException(_CodecError000, nameof(encoders));
                    
                    stream.Value.Position = position;
                }
            }

            throw new ArgumentException("invalid format", nameof(format));
        }

        public static MemoryBitmap Read(System.IO.Stream stream, IBitmapDecoder[] decoders, int? bytesToReadHint = null)
        {
            Guard.NotNull(nameof(stream), stream);
            Guard.IsTrue(nameof(stream), stream.CanRead);
            Guard.NotNull(nameof(decoders), decoders);
            Guard.GreaterThan(nameof(decoders), decoders.Length, 0);

            // it the stream position cannot be reset,
            // and there's more than one decoder, 
            // load the source into a temporary memory buffer:

            if (!stream.CanSeek && decoders.Length > 1)
            {
                using (var mem = new System.IO.MemoryStream())
                {
                    stream.CopyTo(mem);
                    stream.Flush();

                    mem.Position = 0;
                    return Read(mem, decoders);
                }
            }

            // loop over each decoder:            

            var startPos = stream.Position;

            foreach (var decoder in decoders)
            {
                var context = new BitmapDecoderContext
                {
                    Stream = stream,
                    BytesToRead = bytesToReadHint
                };

                if (decoder.TryRead(context, out MemoryBitmap bmp)) return bmp;

                // current decoder failed, amend the stream:

                stream.Position = startPos;
            }

            throw new ArgumentException("invalid format", nameof(stream));
        }
    }
}
