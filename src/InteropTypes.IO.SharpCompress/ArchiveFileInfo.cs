using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    [System.Diagnostics.DebuggerDisplay("{_Entry.Key}")]
    public class ArchiveFileInfo
        : IFileInfo
        , IServiceProvider
    {
        #region lifecycle

        public ArchiveFileInfo(SharpCompress.Archives.IArchiveEntry entry)
        {
            _Entry = entry;
        }

        #endregion

        #region data

        private readonly SharpCompress.Archives.IArchiveEntry _Entry;

        #endregion

        #region properties

        public bool Exists => _Entry != null;

        public long Length => _Entry.Size;

        public string PhysicalPath => _Entry.Key;

        public string Name => System.IO.Path.GetFileName(_Entry.Key);

        public DateTimeOffset LastModified => _Entry.LastModifiedTime ?? DateTimeOffset.MinValue;

        public bool IsDirectory => _Entry.IsDirectory;

        #endregion

        #region API

        public Stream CreateReadStream()
        {
            return _Entry.OpenEntryStream();
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(SharpCompress.Archives.IArchiveEntry)) return _Entry;

            if (serviceType == typeof(Func<FileMode, Stream>)) return (Func<FileMode, Stream>)_Open1;
            if (serviceType == typeof(Func<FileMode, FileAccess, Stream>)) return (Func<FileMode, FileAccess, Stream>)_Open2;
            if (serviceType == typeof(Func<FileMode, FileAccess, FileShare, Stream>)) return (Func<FileMode, FileAccess, FileShare, Stream>)_Open3;            

            if (_Entry is SharpCompress.Archives.Zip.ZipArchiveEntry zipEntry) // zip metadata from entry comment.
            {
                try
                {
                    if (serviceType == typeof(string)) return zipEntry.Comment;                    

                    if (serviceType == typeof(JsonDocument))
                    {
                        return string.IsNullOrWhiteSpace(zipEntry.Comment)
                            ? null
                            : (object)JsonDocument.Parse(zipEntry.Comment);
                    }

                    if (serviceType == typeof(System.Xml.Linq.XDocument))
                    {
                        return string.IsNullOrWhiteSpace(zipEntry.Comment)
                            ? null
                            : (object)System.Xml.Linq.XDocument.Parse(zipEntry.Comment);
                    }
                }
                catch { }
            }            

            return null;
        }

        private Stream _Open1(FileMode mode)
        {
            // if (_Entry.Archive.) value whether it's an read or write mode would be useful

            var s = _Entry.OpenEntryStream();
            if (mode == FileMode.Create && s.CanWrite) return s;
            if (mode == FileMode.Open && s.CanRead) return s;

            s.Dispose();
            return null;
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
