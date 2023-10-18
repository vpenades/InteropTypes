using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;

using STREAM = System.IO.Stream;
using BYTESSEGMENT = System.ArraySegment<byte>;

namespace InteropTypes.IO
{

    #pragma warning disable CA1711 // Identifiers should not have incorrect suffix
    partial class XStream
    #pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    {
        /// <summary>
        /// Reads bytes from the stream until the end of the stream or until the destination buffer is full.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="bytes">The destination buffer.</param>
        /// <returns>the number of bytes read, or 0 if EOF</returns>
        public static int TryReadBytesToEnd(this STREAM stream, Span<Byte> bytes)
        {
            GuardReadable(stream);

            var bbb = bytes;

            while (bbb.Length > 0)
            {
                var l = stream.Read(bbb);
                if (l <= 0) return bytes.Length - bbb.Length;

                bbb = bbb.Slice(l);
            }

            return bytes.Length;
        }

        /// <summary>
        /// Reads bytes from the stream until the end of the stream or until the destination buffer is full.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="bytes">The destination buffer.</param>
        /// <returns>the number of bytes read, or 0 if EOF</returns>
        public static int TryReadBytesToEnd(this STREAM stream, Memory<Byte> bytes)
        {
            if (MemoryMarshal.TryGetArray(bytes, out BYTESSEGMENT array))
            {
                return TryReadBytesToEnd(stream, array);
            }

            GuardReadable(stream);            

            var bbb = bytes;

            while (bbb.Length > 0)
            {
                var l = stream.Read(bbb.Span);
                if (l <= 0) return bytes.Length - bbb.Length;

                bbb = bbb.Slice(l);
            }

            return bytes.Length;
        }

        /// <summary>
        /// Reads bytes from the stream until the end of the stream or until the destination buffer is full.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="bytes">The destination buffer.</param>
        /// <returns>the number of bytes read, or 0 if EOF</returns>
        public static int TryReadBytesToEnd(this STREAM stream, BYTESSEGMENT bytes)
        {
            GuardReadable(stream);

            var bbb = bytes;

            while (bbb.Count > 0)
            {
                var l = stream.Read(bbb.Array, bbb.Offset, bbb.Count);
                if (l <= 0) return bytes.Count - bbb.Count;

                bbb = bbb.Slice(l);
            }

            return bytes.Count;
        }

        /// <summary>
        /// Reads bytes from the stream until the end of the stream or until the destination buffer is full.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="bytes">The destination buffer.</param>
        /// <param name="ctoken">cancellation token</param>
        /// <returns>the number of bytes read, or 0 if EOF</returns>
        public static async ValueTask<int> TryReadBytesToEndAsync(this STREAM stream, Memory<Byte> bytes, CancellationToken ctoken)
        {
            GuardReadable(stream);

            var bbb = bytes;

            while (bbb.Length > 0)
            {
                var l = await stream.ReadAsync(bbb, ctoken).ConfigureAwait(false);
                if (l <= 0) return bytes.Length - bbb.Length;

                bbb = bbb.Slice(l);
            }

            return bytes.Length;
        }
    }
}
