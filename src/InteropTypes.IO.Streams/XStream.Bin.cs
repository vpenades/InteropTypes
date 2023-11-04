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
        public static void WriteAllBytes(Func<STREAM> stream, BYTESSEGMENT bytes)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            WriteAllBytes(stream, bytes);
        }

        public static BYTESSEGMENT ReadAllBytes(Func<STREAM> stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            return ReadAllBytes(stream);
        }

        public static void WriteAllBytes(STREAM stream, BYTESSEGMENT bytes)
        {
            GuardWriteable(stream);

            if (bytes.Count == 0) return;
            stream.Write(bytes.Array, bytes.Offset, bytes.Count);
        }

        public static async Task WriteAllBytesAsync(STREAM stream, BYTESSEGMENT bytes, CancellationToken ctoken)
        {
            GuardWriteable(stream);

            if (bytes.Count == 0) return;
            await stream.WriteAsync(bytes, ctoken).ConfigureAwait(false);
        }

        public static BYTESSEGMENT ReadAllBytes(STREAM stream)
        {
            GuardReadable(stream);

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
