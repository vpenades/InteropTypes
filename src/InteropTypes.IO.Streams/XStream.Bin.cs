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
            return CodeSugarForSystemIO.CreateBinaryWriter(stream, leaveStreamOpen, encoding);
        }

        public static BinaryReader CreateBinaryReader(STREAM stream, bool leaveStreamOpen = true, Encoding encoding = null)
        {
            return CodeSugarForSystemIO.CreateBinaryReader(stream, leaveStreamOpen, encoding);
        }

        public static void WriteAllBytes(Func<STREAM> stream, IReadOnlyList<Byte> bytes)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            CodeSugarForSystemIO.WriteAllBytes(s, bytes);
        }        

        public static void WriteAllBytes(STREAM stream, IReadOnlyList<Byte> bytes)
        {
            CodeSugarForSystemIO.WriteAllBytes(stream, bytes);
        }

        public static async Task WriteAllBytesAsync(Func<STREAM> stream, IReadOnlyList<Byte> bytes, CancellationToken ctoken)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            await CodeSugarForSystemIO.WriteAllBytesAsync(s, bytes, ctoken).ConfigureAwait(false);
        }

        public static async Task WriteAllBytesAsync(STREAM stream, IReadOnlyList<Byte> bytes, CancellationToken ctoken)
        {
            await CodeSugarForSystemIO.WriteAllBytesAsync(stream, bytes, ctoken).ConfigureAwait(false);
        }

        public static BYTESSEGMENT ReadAllBytes(Func<STREAM> stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            return CodeSugarForSystemIO.ReadAllBytes(s);
        }

        public static BYTESSEGMENT ReadAllBytes(STREAM stream)
        {
            return CodeSugarForSystemIO.ReadAllBytes(stream);
        }

        public static async Task<BYTESSEGMENT> ReadAllBytesAsync(STREAM stream, CancellationToken ctoken)
        {
            return await CodeSugarForSystemIO.ReadAllBytesAsync(stream, ctoken);
        }
    }
}
