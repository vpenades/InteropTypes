using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO.Archives.Primitives
{
    [System.Diagnostics.DebuggerDisplay("{EntryKey}  {Length}")]
    internal readonly struct ArchiveFileInfoBase<TEntry> : IFileInfo, IServiceProvider
    {
        #region lifecycle

        public ArchiveFileInfoBase(ArchiveFileProviderBase<TEntry> provider, TEntry entry)
        {
            _Provider = provider;
            _Entry = entry;
        }

        #endregion

        #region data

        private readonly ArchiveFileProviderBase<TEntry> _Provider;
        private readonly TEntry _Entry;

        #endregion

        #region properties

        public string EntryKey => _Provider.GetEntryKey(_Entry);

        /// <inheritdoc/>        
        public bool Exists => _Entry != null;

        /// <inheritdoc/>
        public long Length => _Provider.GetEntryFileLength(_Entry);

        /// <inheritdoc/>
        public string PhysicalPath => null; // file is not directly accesible, so we must return null as per IFileInfo specification.

        /// <inheritdoc/>
        public string Name => Path.GetFileName(EntryKey);

        /// <inheritdoc/>
        public DateTimeOffset LastModified => _Provider.GetEntryLastWriteTime(_Entry);

        /// <inheritdoc/>
        public bool IsDirectory => false;

        #endregion

        #region API

        /// <inheritdoc/>
        public Stream CreateReadStream()
        {
            var s =  _Provider.OpenEntryReadStream(_Entry, FileMode.Open);

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

            if (serviceType == typeof(string)) return _Provider.GetEntryComment(_Entry);

            if (serviceType == typeof(JsonDocument))
            {
                try
                {

                    var comment = _Provider.GetEntryComment(_Entry);
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
                    var comment = _Provider.GetEntryComment(_Entry);
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
            return _Provider.OpenEntryReadStream(_Entry, mode);
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
