using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Extensions.FileProviders;



namespace InteropTypes.IO.Archives.Primitives
{
    [System.Diagnostics.DebuggerDisplay("📁 {_SubPath}")]
    readonly struct ArchiveDirectoryInfoBase<TEntry> : IFileInfo, IDirectoryContents
    {
        public ArchiveDirectoryInfoBase(ArchiveFileProviderBase<TEntry> provider, string subPath)
        {
            _Archive = provider;
            _SubPath = subPath;
        }

        private readonly ArchiveFileProviderBase<TEntry> _Archive;
        private readonly string _SubPath;

        public bool Exists => true;

        long IFileInfo.Length => -1;

        public string PhysicalPath => _SubPath;

        public string Name => Path.GetFileName(_SubPath);

        public DateTimeOffset LastModified => DateTimeOffset.MinValue;

        public bool IsDirectory => true;

        Stream IFileInfo.CreateReadStream() { throw new NotSupportedException(); }

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            return _Archive.GetEntries(_SubPath).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Archive.GetEntries(_SubPath).GetEnumerator();
        }        
    }
}
