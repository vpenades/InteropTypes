using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Codecs
{
    using System.Linq;

    using Diagnostics;
    public static partial class BitmapCodecFactory
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

            throw new BitmapCodecException();
        }

        internal static void Write(Lazy<System.IO.Stream> stream, CodecFormat format, IBitmapEncoder[] encoders, SpanBitmap bmp)
        {
            if (encoders.Length == 0) encoders = GetDefaultEncoders().ToArray();

            Guard.NotNull(nameof(stream), stream);
            Guard.NotNull(nameof(encoders), encoders);
            Guard.IsFalse(nameof(format), format == CodecFormat.Undefined);
            Guard.GreaterThan(nameof(encoders), encoders.Length, 0);            

            long position = 0;
            bool keepOpen = false;

            // if the stream has been created already, remember the start position.
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
            if (decoders.Length == 0) decoders = GetDefaultDecoders().ToArray();

            Guard.NotNull(nameof(stream), stream);
            Guard.IsTrue(nameof(stream), stream.CanRead);
            Guard.NotNull(nameof(decoders), decoders);
            Guard.GreaterThan(nameof(decoders), decoders.Length, 0);
            Guard.GreaterThan(nameof(bytesToReadHint), bytesToReadHint ?? 1, 0);

            // it the stream position cannot be reset,
            // and there's more than one decoder, 
            // load the source into a temporary memory buffer:

            if (!stream.CanSeek && decoders.Length > 1)
            {
                using var mem = _ToMemoryStream(stream, bytesToReadHint);
                return Read(mem, decoders, (int)mem.Length);
            }            

            System.Diagnostics.Debug.Assert(stream.CanSeek);
            System.Diagnostics.Debug.Assert(stream.CanRead);

            // remember the current stream position in case we need to reset it:
            var startPos = stream.Position;

            // loop over each decoder:
            foreach (var decoder in decoders)
            {
                var context = new BitmapDecoderContext
                {
                    Stream = stream,
                    BytesToRead = bytesToReadHint
                };

                // try to load the file with the current decoder:
                if (decoder.TryRead(context, out MemoryBitmap bmp)) return bmp;

                // last decoder failed, reset the stream position:
                stream.Position = startPos;
            }

            throw new ArgumentException("invalid format or incompatible decoder.", nameof(stream));
        }

        private static System.IO.MemoryStream _ToMemoryStream(System.IO.Stream stream, int? bytesToReadHint = null)
        {
            if (bytesToReadHint.HasValue)
            {
                var buffer = new Byte[bytesToReadHint.Value];

                int p = 0;
                while (p < buffer.Length)
                {
                    var r = stream.Read(buffer, p, bytesToReadHint.Value - p);
                    p += r > 0 ? r : throw new System.IO.EndOfStreamException();
                }

                return new System.IO.MemoryStream(buffer, false);
            }
            else
            {
                var mem = new System.IO.MemoryStream();

                stream.CopyTo(mem);
                stream.Flush();

                mem.Position = 0;
                return mem;
            }
        }
    }
}
