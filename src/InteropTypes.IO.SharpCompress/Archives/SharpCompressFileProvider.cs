using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using SharpCompress.Archives;

namespace InteropTypes.IO.Archives
{
    [System.Diagnostics.DebuggerDisplay("{_Path}")]
    public class SharpCompressFileProvider : Primitives.ArchiveFileProviderBase<IArchiveEntry>, IDisposable
    {
        #region lifecycle

        public static SharpCompressFileProvider Open(System.IO.Stream stream)
        {
            try
            {
                var f = SharpCompress.Archives.ArchiveFactory.Open(stream);

                return new SharpCompressFileProvider(f, "<stream>");
            }
            catch { return null; }
        }

        public static SharpCompressFileProvider Open(string path)
        {
            try
            {
                var f = SharpCompress.Archives.ArchiveFactory.Open(path);

                return new SharpCompressFileProvider(f, path);
            }
            catch { return null; }
        }

        protected SharpCompressFileProvider(IArchive archive, string path)
        {
            _Archive = archive;
            _Path = path;
        }

        public void Dispose()
        {
            _Archive?.Dispose();
            _Archive = null;
        }

        #endregion

        #region data

        private string _Path;
        private IArchive _Archive;

        #endregion

        #region implementation

        protected override IEnumerable<IArchiveEntry> GetAllEntries(bool onlyDirectories)
        {
            return _Archive.Entries.Where(item => item.IsDirectory == onlyDirectories);
        }

        protected internal override Stream OpenEntryReadStream(IArchiveEntry entry, FileMode mode)
        {
            var s = entry.OpenEntryStream();

            if (mode == FileMode.Create && s.CanWrite) return s;
            if (mode == FileMode.Open && s.CanRead) return s;

            s.Dispose();
            return null;
        }

        protected internal override long GetEntryFileLength(IArchiveEntry entry)
        {
            return entry.Size;
        }

        protected internal override DateTimeOffset GetEntryLastWriteTime(IArchiveEntry entry)
        {
            return entry.LastModifiedTime ?? DateTime.MinValue;
        }

        protected internal override string GetEntryKey(IArchiveEntry entry)
        {
            return entry.Key;
        }

        protected internal override string GetEntryComment(IArchiveEntry entry)
        {
            if (entry is SharpCompress.Archives.Zip.ZipArchiveEntry zipEntry)
            {
                return zipEntry.Comment;
            }

            return base.GetEntryComment(entry);
        }

        #endregion
    }
}
