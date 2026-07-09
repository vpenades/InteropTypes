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
            return CodeSugarExtensions.CreateTextWriter(stream,leaveStreamOpen,encoding);
        }

        public static StreamReader CreateTextReader(STREAM stream, bool leaveStreamOpen = true, Encoding encoding = null)
        {
            return CodeSugarExtensions.CreateTextReader(stream, leaveStreamOpen, encoding);
        }

        public static void WriteAllText(Func<STREAM> stream, string contents, Encoding encoding = null)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            CodeSugarExtensions.WriteAllText(s, contents, encoding);
        }        

        public static void WriteAllText(STREAM stream, string contents, Encoding encoding = null)
        {
            CodeSugarExtensions.WriteAllText(stream, contents, encoding);
        }        

        public static string ReadAllText(Func<STREAM> stream, Encoding encoding = null)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            using var s = stream();
            return CodeSugarExtensions.ReadAllText(s, encoding);
        }        

        public static string ReadAllText(STREAM stream, Encoding encoding = null)
        {
            return CodeSugarExtensions.ReadAllText(stream, encoding);
        }
    }
}
