using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.FileProviders;

using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Common.Options;
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
        struct _ReaderEntry : IArchiveEntry, IFileInfo, IDisposable
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

            public readonly IArchive Archive => _Archive;

            public readonly IReaderOptions Options => _Archive.ReaderOptions;

            public readonly string Key => _Reader.Entry.Key;

            public readonly bool IsDirectory => _Reader.Entry.IsDirectory;

            public readonly bool IsComplete => throw new NotImplementedException();

            public readonly bool IsEncrypted => _Reader.Entry.IsEncrypted;
            public readonly long Size => _Reader.Entry.Size;
            public readonly long CompressedSize => _Reader.Entry.CompressedSize;
            public readonly CompressionType CompressionType => _Reader.Entry.CompressionType;

            public readonly DateTime? ArchivedTime => _Reader.Entry.ArchivedTime;
            public readonly DateTime? CreatedTime => _Reader.Entry.CreatedTime;
            public readonly DateTime? LastAccessedTime => _Reader.Entry.LastAccessedTime;
            public readonly DateTime? LastModifiedTime => _Reader.Entry.LastModifiedTime;

            public readonly long Crc => _Reader.Entry.Crc;

            public readonly string LinkTarget => _Reader.Entry.LinkTarget;
            public readonly bool IsSplitAfter => _Reader.Entry.IsSplitAfter;
            public readonly bool IsSolid => _Reader.Entry.IsSolid;
            public readonly int VolumeIndexFirst => _Reader.Entry.VolumeIndexFirst;
            public readonly int VolumeIndexLast => _Reader.Entry.VolumeIndexLast;
            public readonly int? Attrib => _Reader.Entry.Attrib;            

            #endregion

            #region API

            public readonly Stream OpenEntryStream()
            {
                if (IsDirectory) throw new InvalidOperationException("directory");

                return _Reader.OpenEntryStream();
            }

            public readonly async ValueTask<Stream> OpenEntryStreamAsync(CancellationToken cancellationToken)
            {
                if (IsDirectory) throw new InvalidOperationException("directory");

                return await Task.FromResult(_Reader.OpenEntryStream());
            }

            #endregion

            #region IFileInfo            

            readonly bool IFileInfo.Exists => true;

            readonly long IFileInfo.Length => this.Size;

            readonly string IFileInfo.PhysicalPath => null;

            readonly string IFileInfo.Name => System.IO.Path.GetFileName(Key);

            readonly DateTimeOffset IFileInfo.LastModified => this.LastModifiedTime ?? DateTime.MinValue;

            readonly Stream IFileInfo.CreateReadStream()
            {
                if (IsDirectory) throw new InvalidOperationException("directory");

                return _Reader.OpenEntryStream();
            }            

            #endregion
        }

        class _ReaderArchive : IReader
        {
            #region lifecycle

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

            #endregion

            #region data

            private readonly IArchive _Archive;
            private IEnumerator<IArchiveEntry> _EntryEnumerator;

            #endregion

            #region properties

            /// <inheritdoc/>
            public ArchiveType ArchiveType => throw new NotImplementedException();

            /// <inheritdoc/>
            public IEntry Entry { get; private set; }

            /// <inheritdoc/>
            public bool Cancelled { get; private set; }

            #endregion

            #region API            

            /// <inheritdoc/>
            public void Cancel() { Cancelled = true; }

            /// <inheritdoc/>
            public async Task<bool> MoveToNextEntryAsync(CancellationToken cancellationToken = default)
            {
                if (cancellationToken.IsCancellationRequested) return false;
                return await Task.FromResult(MoveToNextEntry());
            }

            /// <inheritdoc/>
            public bool MoveToNextEntry()
            {
                if (Cancelled) return false;

                _EntryEnumerator ??= _Archive.Entries.GetEnumerator();

                var r = _EntryEnumerator.MoveNext();

                if (r) Entry = _EntryEnumerator.Current;

                return r;
            }

            /// <inheritdoc/>
            public async Task<EntryStream> OpenEntryStreamAsync(CancellationToken cancellationToken = default)
            {
                if (Entry is IArchiveEntry archEntry)
                {
                    var s = await archEntry.OpenEntryStreamAsync(cancellationToken);
                    if (s is EntryStream es) return es;
                    
                    throw new NotImplementedException();
                }

                throw new NotSupportedException();
            }

            /// <inheritdoc/>
            public EntryStream OpenEntryStream()
            {
                if (Entry is IArchiveEntry archEntry)
                {
                    var s = archEntry.OpenEntryStream();
                    if (s is EntryStream es) return es;
                    
                    throw new NotImplementedException();
                }

                throw new NotSupportedException();
            }

            /// <inheritdoc/>
            public async Task WriteEntryToAsync(Stream writableStream, CancellationToken cancellationToken = default)
            {
                using (var s = await OpenEntryStreamAsync(cancellationToken))
                {
                    await s.CopyToAsync(writableStream, cancellationToken);
                }
            }

            /// <inheritdoc/>
            public void WriteEntryTo(Stream writableStream)
            {
                if (Cancelled) return;

                using (var s = OpenEntryStream())
                {
                    s.CopyTo(writableStream);
                }
            }            

            #endregion
        }
    }
}
