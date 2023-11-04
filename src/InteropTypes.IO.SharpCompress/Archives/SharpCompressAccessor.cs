using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using SharpCompress.Archives;

namespace InteropTypes.IO.Archives
{
    class SharpCompressArchiveAccessor : Primitives.IArchiveAccessor<IArchiveEntry>
    {
        #region lifecycle
        public SharpCompressArchiveAccessor(IArchive archive)
        {
            _Archive = archive;
        }

        #endregion

        #region data

        private IArchive _Archive;

        #endregion

        #region implementation

        public IEnumerable<IArchiveEntry> GetAllEntries() { return _Archive.Entries; }

        public bool GetEntryIsDirectory(IArchiveEntry entry) { return entry.IsDirectory; }

        public string GetEntryKey(IArchiveEntry entry) { return entry.Key; }

        public long GetEntryFileLength(IArchiveEntry entry) { return entry.Size; }

        public DateTimeOffset GetEntryLastWriteTime(IArchiveEntry entry)
        {
            return entry.LastModifiedTime ?? DateTime.MinValue;
        }

        public Stream OpenEntryReadStream(IArchiveEntry entry, FileMode mode)
        {
            var s = entry.OpenEntryStream();

            if (mode == FileMode.Create && s.CanWrite) return s;
            if (mode == FileMode.Open && s.CanRead) return s;

            s.Dispose();
            return null;
        }

        public string GetEntryComment(IArchiveEntry entry)
        {
            if (entry is SharpCompress.Archives.Zip.ZipArchiveEntry zipEntry)
            {
                return zipEntry.Comment;
            }

            return null;
        }

        #endregion
    }
}
