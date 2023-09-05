using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using STREAM = System.IO.Stream;

namespace InteropTypes.IO
{
    public static partial class XStream
    {
        public static void GuardReadable(STREAM stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead) throw new ArgumentException("Can't read from strean", nameof(stream));
        }

        public static void GuardWriteable(STREAM stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanWrite) throw new ArgumentException("Can't read from strean", nameof(stream));
        }

        public static bool TryGetFileInfo(STREAM stream, out FileInfo fileInfo)
        {
            if (stream is System.IO.FileStream fs)
            {
                fileInfo = new FileInfo(fs.Name);
                return true;
            }

            fileInfo = null;
            return false;
        }

        public static void WriteAllText(Func<STREAM> stream, string contents)
        {
            using(var s = stream.Invoke())
            {
                WriteAllText(s,contents);
            }
        }        

        public static void WriteAllText(Func<STREAM> stream, string contents, Encoding encoding)
        {
            using (var s = stream())
            {
                WriteAllText(s, contents, encoding);
            }
        }

        public static void WriteAllText(STREAM stream, string contents)
        {
            GuardWriteable(stream);

            contents ??= string.Empty;

            using (var ss = new StreamWriter(stream)) // uses UTF8NoBOM
            {
                ss.Write(contents);
                return;
            }
        }

        public static void WriteAllText(STREAM stream, string contents, Encoding encoding)
        {
            GuardWriteable(stream);

            contents ??= string.Empty;

            using (var ss = new StreamWriter(stream, encoding))
            {
                ss.Write(contents);
                return;
            }
        }

        public static string ReadAllText(Func<STREAM> stream)
        {
            using(var s = stream())
            {
                return ReadAllText(stream);
            }
        }

        public static string ReadAllText(Func<STREAM> stream, Encoding encoding)
        {
            using (var s = stream())
            {                
                return ReadAllText(s, encoding);
            }
        }

        public static string ReadAllText(STREAM stream)
        {
            GuardReadable(stream);

            using (var sr = new StreamReader(stream, detectEncodingFromByteOrderMarks: true))
            {
                return sr.ReadToEnd();
            }
        }        

        public static string ReadAllText(STREAM stream, Encoding encoding)
        {
            GuardReadable(stream);

            using (var sr = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true))
            {
                return sr.ReadToEnd();
            }
        }

        public static string WriteAllBytes(Func<STREAM> stream, byte[] bytes)
        {
            using (var s = stream())
            {
                return WriteAllBytes(stream, bytes);
            }
        }

        public static void WriteAllBytes(STREAM stream, byte[] bytes)
        {
            GuardWriteable(stream);

            bytes ??= Array.Empty<byte>();

            stream.Write(bytes,0,bytes.Length);
        }

        public static string ReadAllBytes(Func<STREAM> stream)
        {
            using (var s = stream())
            {
                return ReadAllBytes(stream);
            }
        }

        public static byte[] ReadAllBytes(STREAM stream)
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

                using (var m = new System.IO.MemoryStream())
                {
                    stream.CopyTo(m);
                    return m.ToArray();
                }
            }

            int index = 0;
            int count = (int)fileLength;
            byte[] bytes = new byte[count];
            while (count > 0)
            {
                int n = stream.Read(bytes, index, count);
                if (n == 0)
                {
                    throw new System.IO.EndOfStreamException();
                }

                index += n;
                count -= n;
            }
            return bytes;
        }

        public static async Task<byte[]> ReadAllBytesAsync(STREAM stream, CancellationToken ctoken)
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

                using (var m = new System.IO.MemoryStream())
                {
                    await stream.CopyToAsync(m, ctoken).ConfigureAwait(false);
                    return m.ToArray();
                }
            }

            int index = 0;
            int count = (int)fileLength;
            byte[] bytes = new byte[count];
            while (count > 0)
            {
                var memory = new Memory<Byte>(bytes, index, count);

                int n = await stream.ReadAsync(memory, ctoken).ConfigureAwait(false);
                if (n == 0)
                {
                    throw new System.IO.EndOfStreamException();
                }

                index += n;
                count -= n;
            }
            return bytes;
        }

        public static bool AreStreamsContentEqual(this Func<STREAM> a, Func<STREAM> b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));            

            using (var sa = a.Invoke())
            {
                using (var sb = b.Invoke())
                {
                    return AreStreamsContentEqual(sa, sb);
                }
            }
        }

        /// <summary>
        /// Compares the contents of two streams for equality
        /// </summary>
        /// <param name="x">the 1st stream</param>
        /// <param name="y">the 2nd stream</param>
        /// <returns>true if the content of both streams is equal</returns>
        public static bool AreStreamsContentEqual(STREAM x, STREAM y)
        {
            if (x == y) return true;
            if (x == null) return false;
            if (y == null) return false;

            GuardReadable(x);
            GuardReadable(y);

            Span<Byte> xbuff = stackalloc byte[8192];
            Span<Byte> ybuff = stackalloc byte[8192];

            while (true)
            {
                var xlen = x.TryReadBytes(xbuff);
                var ylen = y.TryReadBytes(ybuff);

                if (xlen != ylen) return false;
                if (xlen == 0) break;

                var xslice = xbuff.Slice(0, xlen);
                var yslice = ybuff.Slice(0, xlen);

                if (!xslice.SequenceEqual(yslice)) return false;
            }

            return true;
        }

        
    }
}
