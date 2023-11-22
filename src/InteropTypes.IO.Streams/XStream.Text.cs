using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using STREAM = System.IO.Stream;

namespace InteropTypes.IO
{
    partial class XStream
    {
        #if NETSTANDARD
        private static readonly Encoding UTF8NoBOM = new UTF8Encoding(false);
        #endif

        public static StreamWriter CreateStreamWriter(STREAM stream, bool leaveStreamOpen = true, Encoding encoding = null)
        {
            GuardWriteable(stream);

            #if NETSTANDARD
            // NetStd implementation matching net6
            return new StreamWriter(stream, encoding ?? UTF8NoBOM, 1024, leaveStreamOpen);
            #else
            return new StreamWriter(stream, encoding: encoding, leaveOpen: leaveStreamOpen);
            #endif
        }

        public static StreamReader CreateStreamReader(STREAM stream, bool leaveStreamOpen = true, Encoding encoding = null)
        {
            GuardReadable(stream);

            #if NETSTANDARD
            // NetStd implementation matching net6
            return new StreamReader(stream, encoding ?? Encoding.UTF8, true, 1024, leaveStreamOpen);
            #else
            return new StreamReader(stream, encoding: encoding, leaveOpen: leaveStreamOpen);
            #endif    
        }


        public static void WriteAllText(Func<STREAM> stream, string contents, Encoding encoding = null)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            WriteAllText(s, contents, encoding);
        }        

        public static void WriteAllText(STREAM stream, string contents, Encoding encoding = null)
        {
            contents ??= string.Empty;

            using var ss = CreateStreamWriter(stream, true, encoding);

            ss.Write(contents);            
        }        

        public static string ReadAllText(Func<STREAM> stream, Encoding encoding = null)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            return ReadAllText(s, encoding);
        }        

        public static string ReadAllText(STREAM stream, Encoding encoding = null)
        {
            using var sr = CreateStreamReader(stream, true, encoding);

            return sr.ReadToEnd();
        }
    }
}
