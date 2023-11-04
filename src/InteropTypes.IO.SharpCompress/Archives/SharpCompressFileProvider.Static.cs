using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Extensions.FileProviders;

using SharpCompress.Archives;
using SharpCompress.Common;

using SharpCompress.Readers;

namespace InteropTypes.IO.Archives
{
    partial class SharpCompressFileProvider
    {
        // Todo: Support ReadOnlySequence<Byte> which is provided by RecyclableMemoryStream which can exceed 4gb

        public static bool Contains(IArchive archive, IFileInfo other)
        {
            if (other == null) return false;
            if (other.IsDirectory || !other.Exists) return false;

            using var otherStream = XStream.CreateMemoryStream(other.CreateReadStream, true);

            return Contains(archive, otherStream);
        }

        public static bool Contains(IArchive archive, System.IO.MemoryStream memStream)
        {
            if (archive == null) return false;
            if (memStream == null) throw new ArgumentNullException(nameof(memStream));
            if (!memStream.CanSeek) throw new ArgumentException("can't seek", nameof(memStream));
            if (!memStream.CanRead) throw new ArgumentException("can't seek", nameof(memStream));

            Stream _openMemStream()
            {
                // prepares the stream to read from the beginning.
                memStream.Position = 0;
                return memStream;
            }

            bool _AreEqual(IArchiveEntry xentry)
            {
                if (xentry == null) return false;
                if (xentry.IsDirectory) return false;
                if (xentry.Size != memStream.Length) return false;

                return StreamEqualityComparer
                        .Default
                        .AreStreamsContentEqual(xentry.OpenEntryStream, _openMemStream);
            }

            if (archive.IsSolid)
            {
                var reader = archive.ExtractAllEntries();

                while (reader.MoveToNextEntry())
                {
                    using (var entry = new _ReaderEntry(archive, reader))
                    {
                        if (_AreEqual(entry)) return true;
                    }
                }
            }
            else
            {
                foreach (var arcEntry in archive.Entries)
                {
                    if (_AreEqual(arcEntry)) return true;
                }
            }

            return false;
        }

        [System.Diagnostics.DebuggerDisplay("{Key}")]
        struct _ReaderEntry : IArchiveEntry, IDisposable, IFileInfo
        {
            #region constructor

            public _ReaderEntry(IArchive arch, SharpCompress.Readers.IReader reader)
            {
                _Archive = arch;
                _Reader = reader;
            }

            public void Dispose()
            {
                _Archive = null;
                _Reader = null;
            }

            #endregion

            #region data

            private IArchive _Archive;
            private SharpCompress.Readers.IReader _Reader;

            #endregion

            #region properties

            public IArchive Archive => _Archive;

            public string Key => _Reader.Entry.Key;

            public bool IsDirectory => _Reader.Entry.IsDirectory;

            public bool IsComplete => throw new NotImplementedException();

            public bool IsEncrypted => _Reader.Entry.IsEncrypted;
            public long Size => _Reader.Entry.Size;
            public long CompressedSize => _Reader.Entry.CompressedSize;
            public CompressionType CompressionType => _Reader.Entry.CompressionType;

            public DateTime? ArchivedTime => _Reader.Entry.ArchivedTime;
            public DateTime? CreatedTime => _Reader.Entry.CreatedTime;
            public DateTime? LastAccessedTime => _Reader.Entry.LastAccessedTime;
            public DateTime? LastModifiedTime => _Reader.Entry.LastModifiedTime;



            public long Crc => _Reader.Entry.Crc;

            public string LinkTarget => _Reader.Entry.LinkTarget;
            public bool IsSplitAfter => _Reader.Entry.IsSplitAfter;
            public bool IsSolid => _Reader.Entry.IsSolid;
            public int VolumeIndexFirst => _Reader.Entry.VolumeIndexFirst;
            public int VolumeIndexLast => _Reader.Entry.VolumeIndexLast;
            public int? Attrib => _Reader.Entry.Attrib;            

            #endregion

            #region API

            public Stream OpenEntryStream()
            {
                return _Reader.OpenEntryStream();
            }

            #endregion

            #region IFileInfo

            bool IFileInfo.Exists => true;

            long IFileInfo.Length => this.Size;

            string IFileInfo.PhysicalPath => null;

            string IFileInfo.Name => System.IO.Path.GetFileName(Key);

            DateTimeOffset IFileInfo.LastModified => this.LastModifiedTime ?? DateTime.MinValue;

            Stream IFileInfo.CreateReadStream()
            {
                return _Reader.OpenEntryStream();
            }

            #endregion
        }

        class _ReaderArchive : IReader
        {
            public _ReaderArchive(IArchive archive)
            {
                _Archive = archive;
            }

            public void Dispose()
            {
                _EntryEnumerator?.Dispose();
                _EntryEnumerator = null;
                Entry = null;
            }

            private readonly IArchive _Archive;
            private IEnumerator<IArchiveEntry> _EntryEnumerator;

            public ArchiveType ArchiveType => throw new NotImplementedException();

            public IEntry Entry { get; private set; }

            public bool Cancelled { get; private set; }

            public event EventHandler<ReaderExtractionEventArgs<IEntry>> EntryExtractionProgress;
            public event EventHandler<CompressedBytesReadEventArgs> CompressedBytesRead;
            public event EventHandler<FilePartExtractionBeginEventArgs> FilePartExtractionBegin;

            public void Cancel() { Cancelled = true; }

            public bool MoveToNextEntry()
            {
                if (Cancelled) return false;

                _EntryEnumerator ??= _Archive.Entries.GetEnumerator();

                var r = _EntryEnumerator.MoveNext();

                if (r) Entry = _EntryEnumerator.Current;

                return r;
            }

            public EntryStream OpenEntryStream()
            {
                if (Entry is IArchiveEntry archEntry)
                {
                    var s = archEntry.OpenEntryStream();
                    // return new EntryStream(this, s);  // we could use reflection but....
                    throw new NotImplementedException();
                }

                throw new NotSupportedException();
            }

            public void WriteEntryTo(Stream writableStream)
            {
                using (var s = OpenEntryStream())
                {
                    s.CopyTo(writableStream);
                }
            }
        }
    }
}
