using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using STREAM = System.IO.Stream;

using BYTESSEGMENT = System.ArraySegment<byte>;

namespace InteropTypes.IO
{
    partial class XStream
    {
        public static BinaryWriter CreateBinaryWriter(STREAM stream, bool leaveStreamOpen = true, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;

            return new System.IO.BinaryWriter(stream, encoding, leaveStreamOpen);
        }

        public static BinaryReader CreateBinaryReader(STREAM stream, bool leaveStreamOpen = true, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;

            return new System.IO.BinaryReader(stream, encoding, leaveStreamOpen);
        }




        public static void WriteAllBytes(Func<STREAM> stream, IReadOnlyList<Byte> bytes)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            WriteAllBytes(stream, bytes);
        }        

        public static void WriteAllBytes(STREAM stream, IReadOnlyList<Byte> bytes)
        {
            GuardWriteable(stream);

            if (bytes.Count == 0) return;

            switch(bytes)
            {
                case Byte[] array: stream.Write(array, 0, array.Length); break;
                case BYTESSEGMENT segment: stream.Write(segment.Array, segment.Offset, segment.Count); break;
                default:
                    using (var xlist = WrapList(bytes)) { xlist.CopyTo(stream); }
                    break;
            }            
        }

        public static async Task WriteAllBytesAsync(Func<STREAM> stream, IReadOnlyList<Byte> bytes, CancellationToken ctoken)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            await WriteAllBytesAsync(stream, bytes, ctoken).ConfigureAwait(false);
        }

        public static async Task WriteAllBytesAsync(STREAM stream, IReadOnlyList<Byte> bytes, CancellationToken ctoken)
        {
            GuardWriteable(stream);

            if (bytes.Count == 0) return;

            switch(bytes)
            {
                case Byte[] array: await stream.WriteAsync(array, ctoken).ConfigureAwait(false); break;
                case BYTESSEGMENT segment: await stream.WriteAsync(segment, ctoken).ConfigureAwait(false); break;
                default:
                    using (var xlist = WrapList(bytes)) { await xlist.CopyToAsync(stream, ctoken).ConfigureAwait(false); }
                    break;
            }            
        }




        public static BYTESSEGMENT ReadAllBytes(Func<STREAM> stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            return ReadAllBytes(stream);
        }

        public static BYTESSEGMENT ReadAllBytes(STREAM stream)
        {
            GuardReadable(stream);

            // fast path for MemoryStream

            if (stream is MemoryStream memStream)
            {
                if (memStream.TryGetBuffer(out var buffer))
                {
                    buffer = buffer.Slice((int)memStream.Position);
                    return buffer.ToArray(); // ReadAllBytes always return a copy;
                }
            }

            // taken from Net6's ReadAllBytes

            long fileLength = stream.CanSeek ? stream.Length : 0;

            if (fileLength > int.MaxValue)
            {
                throw new IOException("File too long");
            }

            if (fileLength == 0)
            {
                // Some file systems (e.g. procfs on Linux) return 0 for length even when there's content;
                // also there is non-seekable file stream.
                // Thus we need to assume 0 doesn't mean empty.

                using var m = new System.IO.MemoryStream();
                stream.CopyTo(m);
                return m.TryGetBuffer(out var buffer)
                    ? buffer
                    : m.ToArray();
            }

            int index = 0;
            int count = (int)fileLength;
            byte[] bytes = new byte[count];
            while (count > 0)
            {
                int n = stream.Read(bytes, index, count);
                if (n == 0) throw new System.IO.EndOfStreamException();

                index += n;
                count -= n;
            }
            return bytes;
        }

        public static async Task<BYTESSEGMENT> ReadAllBytesAsync(STREAM stream, CancellationToken ctoken)
        {
            GuardReadable(stream);

            // fast path for MemoryStream

            if (stream is MemoryStream memStream)
            {
                if (memStream.TryGetBuffer(out var buffer))
                {
                    buffer = buffer.Slice((int)memStream.Position);
                    return buffer.ToArray(); // ReadAllBytesAsync always return a copy;
                }
            }

            // taken from Net6's ReadAllBytes

            long fileLength = stream.CanSeek ? stream.Length : 0;

            if (fileLength > int.MaxValue)
            {
                throw new IOException("File too long");
            }

            if (fileLength == 0)
            {
                // Some file systems (e.g. procfs on Linux) return 0 for length even when there's content;
                // also there is non-seekable file stream.
                // Thus we need to assume 0 doesn't mean empty.

                using var m = new System.IO.MemoryStream();
                await stream.CopyToAsync(m, ctoken).ConfigureAwait(false);
                return m.TryGetBuffer(out var buffer)
                    ? buffer
                    : m.ToArray();
            }

            int index = 0;
            int count = (int)fileLength;
            byte[] bytes = new byte[count];
            while (count > 0)
            {
                var memory = new Memory<Byte>(bytes, index, count);

                int n = await stream.ReadAsync(memory, ctoken).ConfigureAwait(false);
                if (n == 0) throw new System.IO.EndOfStreamException();

                index += n;
                count -= n;
            }
            return bytes;
        }
    }
}
