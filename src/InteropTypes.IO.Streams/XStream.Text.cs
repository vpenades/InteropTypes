using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using STREAM = System.IO.Stream;

namespace InteropTypes.IO
{
    partial class XStream
    {
        public static void WriteAllText(Func<STREAM> stream, string contents)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream.Invoke();
            WriteAllText(s, contents);
        }

        public static void WriteAllText(Func<STREAM> stream, string contents, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            WriteAllText(s, contents, encoding);
        }

        public static void WriteAllText(STREAM stream, string contents)
        {
            GuardWriteable(stream);

            contents ??= string.Empty;

            using var ss = new StreamWriter(stream); // uses UTF8NoBOM
            ss.Write(contents);            
        }

        public static void WriteAllText(STREAM stream, string contents, Encoding encoding)
        {
            GuardWriteable(stream);

            contents ??= string.Empty;

            using var ss = new StreamWriter(stream, encoding);
            ss.Write(contents);            
        }

        public static string ReadAllText(Func<STREAM> stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            return ReadAllText(stream);
        }

        public static string ReadAllText(Func<STREAM> stream, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            return ReadAllText(s, encoding);
        }

        public static string ReadAllText(STREAM stream)
        {
            GuardReadable(stream);

            using var sr = new StreamReader(stream, detectEncodingFromByteOrderMarks: true);
            return sr.ReadToEnd();
        }

        public static string ReadAllText(STREAM stream, Encoding encoding)
        {
            GuardReadable(stream);

            using var sr = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true);
            return sr.ReadToEnd();
        }
    }
}
