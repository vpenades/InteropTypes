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
        #region constants

        private static readonly Byte[] _SpanBitmapHeader = { 0xAB, 0x49, 0x6E, 0x74, 0x65, 0x72, 0x6F, 0x70, 0x42, 0x69, 0x74, 0x6D, 0x61, 0x70, 0x20, 0x31, 0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A };

        #endregion

        #region properties

        public bool EnableCompression { get; set; } = true;

        public static _InBuiltCodec RawEncoder { get; } = new _InBuiltCodec { EnableCompression = false };

        #endregion

        #region API - Codec

        /// <inheritdoc/>
        public bool TryRead(BitmapDecoderContext readContext, out MemoryBitmap bitmap)
        {
            try
            {
                bitmap = ReadRaw(readContext.Stream).First();
                return true;
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

            return TryWrite(stream.Value, bmp);
        }

        public bool TryWrite(Stream stream, SpanBitmap bmp)
        {
            using (var w = new System.IO.BinaryWriter(stream, Encoding.UTF8, true))
            {
                WriteRaw(w, bmp, this);
            }

            return true;
        }

        #endregion

        #region API - Static

        public static void WriteRaw(System.IO.Stream stream, MemoryBitmap[] src, _InBuiltCodec settings)
        {
            using(var ss = new System.IO.BinaryWriter(stream, Encoding.UTF8, true))
            {
                WriteRaw(ss, src, settings);
            }
        }

        public static IEnumerable<MemoryBitmap> ReadRaw(System.IO.Stream stream)
        {
            using (var ss = new System.IO.BinaryReader(stream, Encoding.UTF8, true))
            {
                return ReadRaw(ss).ToList();
            }
        }

        public static void WriteRaw(System.IO.BinaryWriter writer, MemoryBitmap[] src, _InBuiltCodec settings)
        {
            _WriteHeader(writer);

            writer.Write(src.Length); // planes count            

            foreach(var bmp in src)
            {
                _WritePlane(writer, bmp, settings);
            }            
        }

        public static void WriteRaw(System.IO.BinaryWriter writer, SpanBitmap src, _InBuiltCodec settings)
        {
            _WriteHeader(writer);

            writer.Write((int)1); // planes count            

            _WritePlane(writer, src, settings);
        }

        private static void _WriteHeader(BinaryWriter writer)
        {
            writer.Write(_SpanBitmapHeader);
            writer.Write((int)0); // version
            writer.Write(BitConverter.IsLittleEndian);
        }

        private static void _WritePlane(BinaryWriter writer, SpanBitmap src, _InBuiltCodec settings)
        {
            src.Info.Write(writer);

            var cmp = settings.EnableCompression ? 1 : 0;

            var data = src.ReadableBytes;

            writer.Write(data.Length);
            writer.Write((int)cmp);
            _WriteToStream(writer.BaseStream, data, cmp);
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
                var data = _ReadFromStream(reader.BaseStream, dataLen, dataCmp);

                yield return new MemoryBitmap(data, info);
            }
        }        

        private static void _WriteToStream(System.IO.Stream s, ReadOnlySpan<Byte> data, int compression)
        {
            if (compression == 1)
            {
                using (var ss = new System.IO.Compression.DeflateStream(s, System.IO.Compression.CompressionLevel.Fastest, true))
                {
                    _WriteToStream(ss, data, 0);
                }

                return;
            }            

            if (compression == 0)
            {                
                s.Write(data);             

                return;
            }

            throw new ArgumentException("invalid compression", nameof(compression));
        }

        private static Byte[] _ReadFromStream(System.IO.Stream s, int byteLen, int compression)
        {
            if (compression == 1)
            {
                using (var ss = new System.IO.Compression.DeflateStream(s, System.IO.Compression.CompressionMode.Decompress, true))
                {
                    return _ReadFromStream(ss, byteLen, 0);
                }                
            }

            if (compression == 0)
            {
                var data = new Byte[byteLen];                

                var span = data.AsSpan();
                while(span.Length > 0)
                {
                    var r = s.Read(span);
                    if (r == 0) throw new System.IO.EndOfStreamException();
                    span = span.Slice(r);
                }                

                return data;
            }

            throw new ArgumentException("invalid compression", nameof(compression));
        }

        #endregion
    }
}
