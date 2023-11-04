using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;

using STREAM = System.IO.Stream;

namespace InteropTypes.IO
{
    public static partial class XStream    
    {
        #region diagnostics

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

        #endregion

        #region reflection        

        public static bool IsReadable(STREAM stream) { return stream?.CanRead ?? false; }

        public static bool IsWritable(STREAM stream) { return stream?.CanWrite ?? false; }

        public static bool IsSeekable(STREAM stream) { return stream?.CanSeek ?? false; }

        public static bool TryGetFileInfo(Uri uri, out FileInfo fileInfo)
        {
            fileInfo = null;
            if (uri == null) return false;
            if (!uri.IsFile) return false;

            try
            {
                fileInfo = new FileInfo(uri.LocalPath);
                return true;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is NotSupportedException || ex is System.Security.SecurityException || ex is System.UnauthorizedAccessException)
            {
                return false;
            }
        }

        public static bool TryGetFileInfo(STREAM stream, out FileInfo fileInfo)
        {
            // dig into the stream for the underlaying stream.
            stream = GetBaseStream(stream);

            switch (stream)
            {
                case System.IO.FileStream derived: fileInfo = new FileInfo(derived.Name); return true;
                case IServiceProvider service:
                    if (service.GetService(typeof(FileInfo)) is FileInfo finfo)
                    {
                        fileInfo = finfo;
                        return true;
                    }
                    break;
            }

            fileInfo = null;
            return false;
        }

        /// <summary>
        /// Peels the current stream until it finds the underlayig base stream.
        /// </summary>
        /// <param name="stream">The stream to peel.</param>        
        /// <returns>true if a base stream was found.</returns>
        public static STREAM GetBaseStream(STREAM stream)
        {
            if (stream == null) return null;

            try // with known types
            {
                switch (stream)
                {
                    case BufferedStream derived: stream = derived.UnderlyingStream; break;

                    case System.IO.Compression.GZipStream derived: stream = derived.BaseStream; break;
                    case System.IO.Compression.BrotliStream derived: stream = derived.BaseStream; break;
                    case System.IO.Compression.DeflateStream derived: stream = derived.BaseStream; break;

                    #if !NETSTANDARD
                    case System.IO.Compression.ZLibStream derived: stream = derived.BaseStream; break;
                    #endif

                    case _CloseActionStream derived: stream = derived.BaseStream; break;
                }
            }
            catch (ObjectDisposedException)
            {
                return null;
            }

            try // with unknown types with reflection
            {
                stream ??= stream
                    .GetType()
                    .GetProperty("BaseStream")
                    ?.GetValue(stream, null) as STREAM;

                stream ??= stream
                    .GetType()
                    .GetProperty("UnderlyingStream")
                    ?.GetValue(stream, null) as STREAM;
            }
            catch (Exception ex)
            when (ex is AmbiguousMatchException || ex is MethodAccessException || ex is TargetException || ex is TargetInvocationException)
            {
                return null;
            }

            return stream;
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
            catch (Exception ex)
            when (ex is NotSupportedException || ex is ObjectDisposedException)
            {
                length = 0;
                return false;
            }
        }

        #endregion

        #region special

        public static STREAM WrapList<TList>(TList bytes, FileMode mode = FileMode.Open)
            where TList: IReadOnlyList<Byte>
        {
            var s = _ListWrapperStream<TList>.Open(bytes, mode);
            System.Diagnostics.Debug.Assert(s == null || s.Position == 0);
            return s;
        }        

        public static STREAM OpenArchive(System.IO.Compression.ZipArchiveEntry zentry, FileMode mode)
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

        #endregion
    }
}
