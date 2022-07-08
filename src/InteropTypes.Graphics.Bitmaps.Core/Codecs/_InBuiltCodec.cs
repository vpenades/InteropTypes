using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Codecs
{
    internal class _InBuiltCodec : IBitmapDecoder, IBitmapEncoder
    {
        public bool EnableCompression { get; set; } = true;

        private static readonly Byte[] _SpanBitmapHeader = { 0xAB, 0x49, 0x6E, 0x74, 0x65, 0x72, 0x6F, 0x70, 0x42, 0x69, 0x74, 0x6D, 0x61, 0x70, 0x20, 0x31, 0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A };

        /// <inheritdoc/>
        public bool TryRead(BitmapDecoderContext readContext, out MemoryBitmap bitmap)
        {
            try
            {
                using(var r = new System.IO.BinaryReader(readContext.Stream, Encoding.UTF8, true))
                {
                    bitmap = ReadRaw(r).First();
                    return true;
                }
            }
            catch(System.IO.IOException)
            {
                bitmap = default;
                return false;
            }
        }

        /// <inheritdoc/>
        public bool TryWrite(Lazy<Stream> stream, CodecFormat format, SpanBitmap bmp)
        {
            if (format != CodecFormat.InteropBitmap) return false;
            
            using(var w = new System.IO.BinaryWriter(stream.Value, Encoding.UTF8, true))
            {
                WriteRaw(w,bmp, EnableCompression ? 1:0);
            }

            return true;            
        }

        public static void WriteRaw(System.IO.BinaryWriter writer, SpanBitmap src, int dataCompression)
        {
            writer.Write(_SpanBitmapHeader);
            writer.Write((int)0); // version
            writer.Write(BitConverter.IsLittleEndian);

            writer.Write((int)1); // planes count

            src.Info.Write(writer);

            var data = src.ReadableBytes;

            writer.Write(data.Length);            

            writer.Write((int)dataCompression); // deflate

            if (dataCompression == 0)
            {
                _WriteToStream(writer.BaseStream, data);
            }

            if (dataCompression == 1)
            {
                using (var ss = new System.IO.Compression.DeflateStream(writer.BaseStream, System.IO.Compression.CompressionLevel.Fastest, true))
                {
                    _WriteToStream(ss, data);
                }
            }
        }

        public static IEnumerable<MemoryBitmap> ReadRaw(System.IO.BinaryReader reader)
        {
            var hdr = reader.ReadBytes(_SpanBitmapHeader.Length);
            if (!hdr.SequenceEqual(_SpanBitmapHeader)) throw new System.IO.IOException("invalid header");

            var version = reader.ReadInt32();
            if (version != 0) throw new System.IO.IOException("invalid version");

            var endianness = reader.ReadBoolean();

            if (endianness != BitConverter.IsLittleEndian) throw new System.IO.IOException("invalid endianness");

            int planeCount = reader.ReadInt32();

            for (int i = 0; i < planeCount; i++)
            {
                var info = BitmapInfo.Read(reader);

                var dataLen = reader.ReadInt32();
                var dataCmp = reader.ReadInt32();

                var data = new Byte[dataLen];

                if (dataCmp == 0)
                {
                    _ReadFromStream(data, reader.BaseStream);
                }
                else if (dataCmp == 1)
                {
                    using (var ss = new System.IO.Compression.DeflateStream(reader.BaseStream, System.IO.Compression.CompressionMode.Decompress, true))
                    {
                        _ReadFromStream(data, ss);
                    }
                }

                yield return new MemoryBitmap(data, info);
            }
        }


        private static void _WriteToStream(System.IO.Stream s, ReadOnlySpan<Byte> data)
        {
            #if NETSTANDARD2_0
            for(int i=0; i < data.Length; ++i)
            {
                s.WriteByte(data[i]);
            }
            #else
            s.Write(data);
            #endif
        }

        private static void _ReadFromStream(Span<Byte> data, System.IO.Stream s)
        {
            #if NETSTANDARD2_0
            for (int i = 0; i < data.Length; ++i)
            {
                var value = s.ReadByte();
                if (value < 0) throw new System.IO.EndOfStreamException();
                data[i] = (byte)value;
            }
            #else
            s.Read(data);
            #endif
        }
    }
}
