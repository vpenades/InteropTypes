using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using STREAM = System.IO.Stream;

namespace InteropTypes.IO
{
    partial class XStream
    {
        public static StreamWriter CreateTextWriter(STREAM stream, bool leaveStreamOpen = true, Encoding encoding = null)
        {
            return CodeSugarForSystemIO.CreateTextWriter(stream,leaveStreamOpen,encoding);
        }

        public static StreamReader CreateTextReader(STREAM stream, bool leaveStreamOpen = true, Encoding encoding = null)
        {
            return CodeSugarForSystemIO.CreateTextReader(stream, leaveStreamOpen, encoding);
        }

        public static void WriteAllText(Func<STREAM> stream, string contents, Encoding encoding = null)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            CodeSugarForSystemIO.WriteAllText(s, contents, encoding);
        }        

        public static void WriteAllText(STREAM stream, string contents, Encoding encoding = null)
        {
            CodeSugarForSystemIO.WriteAllText(stream, contents, encoding);
        }        

        public static string ReadAllText(Func<STREAM> stream, Encoding encoding = null)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            return CodeSugarForSystemIO.ReadAllText(s, encoding);
        }        

        public static string ReadAllText(STREAM stream, Encoding encoding = null)
        {
            return CodeSugarForSystemIO.ReadAllText(stream, encoding);
        }
    }
}
