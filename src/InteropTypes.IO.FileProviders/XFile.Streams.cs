using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InteropTypes.IO
{
    partial class XFile
    {
        public static void WriteAllText(System.IO.Stream stream, string contents)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            contents ??= string.Empty;

            using (var ss = new System.IO.StreamWriter(stream)) // uses UTF8NoBOM
            {
                ss.Write(contents);
                return;
            }
        }

        public static void WriteAllText(System.IO.Stream stream, string contents, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            contents ??= string.Empty;

            using (var ss = new System.IO.StreamWriter(stream, encoding))
            {
                ss.Write(contents);
                return;
            }
        }

        public static string ReadAllText(System.IO.Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using (var sr = new StreamReader(stream, detectEncodingFromByteOrderMarks: true))
            {
                return sr.ReadToEnd();
            }
        }

        public static string ReadAllText(System.IO.Stream stream, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using (var sr = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true))
            {
                return sr.ReadToEnd();
            }
        }


        public static void WriteAllBytes(System.IO.Stream stream, byte[] bytes)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            bytes ??= Array.Empty<byte>();

            stream.Write(bytes,0,bytes.Length);
        }

        public static byte[] ReadAllBytes(System.IO.Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            // taken from Net6's ReadAllBytes

            long fileLength = stream.Length;
            if (stream.CanSeek && fileLength > int.MaxValue)
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
    }
}
