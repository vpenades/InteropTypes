using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO.Archives.Primitives
{
    [System.Diagnostics.DebuggerDisplay("🗎 {ToDebugString(),nq}")]
    internal readonly struct ArchiveFileInfo<TEntry> : IFileInfo, IServiceProvider
    {
        #region lifecycle

        internal string ToDebugString()
        {
            if (_Archive == null) return "<NULL>";

            var entryKey = _Archive.GetEntryKey(_Entry);

            return $"🗎 {Name} 🡒 <{entryKey} {Length}>";
        }

        public static IFileInfo Create(IArchiveAccessor<TEntry> archive, TEntry entry)
        {
            return new ArchiveFileInfo<TEntry>(archive, entry);
        }

        public ArchiveFileInfo(IArchiveAccessor<TEntry> archive, TEntry entry)
        {
            _Archive = archive;
            _Entry = entry;
        }

        #endregion

        #region data

        private readonly IArchiveAccessor<TEntry> _Archive;
        private readonly TEntry _Entry;

        #endregion

        #region properties        
        public bool IsDirectory => false;
        public bool Exists => _Archive != null && _Entry != null;
        public long Length => _Archive.GetEntryFileLength(_Entry);
        public string PhysicalPath => null;
        public string Name => Path.GetFileName(_Archive.GetEntryKey(_Entry));
        public DateTimeOffset LastModified => _Archive.GetEntryLastWriteTime(_Entry);

        #endregion

        #region API

        /// <inheritdoc/>
        public Stream CreateReadStream()
        {
            var s = _Archive.OpenEntryReadStream(_Entry, FileMode.Open);

            if (s == null) return null;

            if (s.CanRead) return s;

            s.Dispose();
            return null;
        }

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(TEntry)) return _Entry;

            // services used to get extended open for writing options

            if (serviceType == typeof(Func<FileMode, Stream>)) return (Func<FileMode, Stream>)_Open1;
            if (serviceType == typeof(Func<FileMode, FileAccess, Stream>)) return (Func<FileMode, FileAccess, Stream>)_Open2;
            if (serviceType == typeof(Func<FileMode, FileAccess, FileShare, Stream>)) return (Func<FileMode, FileAccess, FileShare, Stream>)_Open3;

            // services used to parse metadata from entry comment (if available)

            if (serviceType == typeof(string)) return _Archive.GetEntryComment(_Entry);

            if (serviceType == typeof(JsonDocument))
            {
                try
                {

                    var comment = _Archive.GetEntryComment(_Entry);
                    return string.IsNullOrWhiteSpace(comment)
                        ? null
                        : (object)JsonDocument.Parse(comment);
                }
                catch { return null; }
            }

            if (serviceType == typeof(System.Xml.Linq.XDocument))
            {
                try
                {
                    var comment = _Archive.GetEntryComment(_Entry);
                    return string.IsNullOrWhiteSpace(comment)
                        ? null
                        : (object)System.Xml.Linq.XDocument.Parse(comment);
                }
                catch { return null; }
            }

            return null;
        }

        private Stream _Open1(FileMode mode)
        {
            return _Archive.OpenEntryReadStream(_Entry, mode);
        }
        private Stream _Open2(FileMode mode, FileAccess access)
        {
            return _Open1(mode);
        }
        private Stream _Open3(FileMode mode, FileAccess access, FileShare share)
        {
            return _Open1(mode);
        }

        #endregion
    }
}
