using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;

using STREAM = System.IO.Stream;

using BYTESSEGMENT = System.ArraySegment<byte>;

namespace InteropTypes.IO
{
    public static partial class XStream    
    {
        public static STREAM CreateFrom<TList>(TList bytes, FileMode mode = FileMode.Open)
            where TList: IReadOnlyList<Byte>
        {
            return _ListWrapperStream<TList>.Open(bytes, mode);
        }

        public static STREAM CreateFrom(BYTESSEGMENT bytes, bool writable = false)
        {
            return bytes.Count == 0
                ? new System.IO.MemoryStream(Array.Empty<byte>(), writable)
                : (STREAM)new System.IO.MemoryStream(bytes.Array, bytes.Offset, bytes.Count, writable);
        }

        public static STREAM CreateFrom(System.IO.Compression.ZipArchiveEntry zentry, FileMode mode)
        {
            if (mode == FileMode.Open)
            {
                if (zentry.Archive.Mode == System.IO.Compression.ZipArchiveMode.Create) return null;
                var stream = zentry.Open();
                if (!stream.CanRead) { stream.Dispose(); stream = null; }
                return stream;
            }

            if (mode == FileMode.CreateNew)
            {
                if (zentry.Archive.Mode == System.IO.Compression.ZipArchiveMode.Read) return null;
                var stream = zentry.Open();
                if (!stream.CanWrite) { stream.Dispose(); stream = null; }
                return stream;
            }

            throw new ArgumentException($"Unsupported mode: {mode}", nameof(mode));
        }

        public static STREAM WrapWithCloseActions(STREAM stream, Action<long> onClosed, Func<STREAM, bool> onClosing = null)
        {
            if (stream == null) return null;
            if (onClosing == null && onClosed == null) return stream;
            return new _CloseActionStream(stream, false, onClosing, onClosed);
        }

        public static bool IsReadable(STREAM stream) { return stream?.CanRead ?? false; }

        public static bool IsWritable(STREAM stream) { return stream?.CanWrite ?? false; }

        public static bool IsSeekable(STREAM stream) { return stream?.CanSeek ?? false; }

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

        public static void GuardSeekable(STREAM stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanSeek) throw new ArgumentException("Can't seek strean", nameof(stream));
        }

        public static bool TryGetBaseStream(STREAM stream, out STREAM baseStream)
        {
            baseStream = null;

            if (stream == null) return false;

            try // with known types
            {
                switch (stream)
                {
                    case BufferedStream derived: baseStream = derived.UnderlyingStream; break;

                    case System.IO.Compression.GZipStream derived: baseStream = derived.BaseStream; break;
                    case System.IO.Compression.BrotliStream derived: baseStream = derived.BaseStream; break;
                    case System.IO.Compression.DeflateStream derived: baseStream = derived.BaseStream; break;                    

                    #if !NETSTANDARD
                    case System.IO.Compression.ZLibStream derived: baseStream = derived.BaseStream; break;
                    #endif

                    case _CloseActionStream derived: baseStream = derived.BaseStream; break;
                }
            }
            catch(ObjectDisposedException) { return false; }

            try // with unknown types with reflection
            {
                baseStream ??= stream
                    .GetType()
                    .GetProperty("BaseStream")
                    ?.GetValue(stream, null) as STREAM;

                baseStream ??= stream
                    .GetType()
                    .GetProperty("UnderlyingStream")
                    ?.GetValue(stream, null) as STREAM;
            }
            catch (Exception ex)
            when (ex is AmbiguousMatchException || ex is MethodAccessException || ex is TargetException || ex is TargetInvocationException)
            { }

            return baseStream != null;
        }

        public static bool TryGetFileInfo(STREAM stream, out FileInfo fileInfo)
        {
            while(true)            
            {
                // try to retrieve a FileInfo

                if (stream is System.IO.FileStream fs)
                {
                    fileInfo = new FileInfo(fs.Name);
                    return true;
                }

                if (stream is IServiceProvider sp)
                {
                    if (sp.GetService(typeof(FileInfo)) is FileInfo finfo)
                    {
                        fileInfo = finfo; return true;
                    }
                }

                // dig into the stream for the underlaying stream:

                if (!TryGetBaseStream(stream, out var baseStream)) break;
                
                stream = baseStream;
            }

            fileInfo = null;
            return false;
        }

        /// <summary>
        /// Tries to get the length of an open stream.
        /// </summary>
        /// <param name="stream">An open stream</param>
        /// <param name="length">the length of the stream</param>
        /// <returns>true if the length was successfully retrieved</returns>
        public static bool TryGetLength(STREAM stream, out long length)
        {
            if (stream == null) { length = 0; return false; }            

            try
            {                
                length = stream.Length;

                // Some file systems (e.g. procfs on Linux) return 0 for length even when there's content;
                // also there is non-seekable file stream.
                // Thus we need to assume 0 doesn't mean empty.

                return length != 0;
            }
            catch(Exception ex)
            when (ex is NotSupportedException || ex is ObjectDisposedException)
            {
                length = 0;
                return false;
            }
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

        public static string WriteAllBytes(Func<STREAM> stream, BYTESSEGMENT bytes)
        {
            using (var s = stream())
            {
                return WriteAllBytes(stream, bytes);
            }
        }

        public static void WriteAllBytes(STREAM stream, BYTESSEGMENT bytes)
        {
            GuardWriteable(stream);

            if (bytes.Count == 0) return;

            stream.Write(bytes.Array, bytes.Offset, bytes.Count);
        }

        public static BYTESSEGMENT ReadAllBytes(Func<STREAM> stream)
        {
            using (var s = stream())
            {
                return ReadAllBytes(stream);
            }
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

                using (var m = new System.IO.MemoryStream())
                {
                    stream.CopyTo(m);
                    return m.TryGetBuffer(out var buffer)
                        ? buffer
                        : m.ToArray();
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

                using (var m = new System.IO.MemoryStream())
                {
                    await stream.CopyToAsync(m, ctoken).ConfigureAwait(false);
                    return m.TryGetBuffer(out var buffer)
                        ? buffer
                        : m.ToArray();
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
        
    }
}
