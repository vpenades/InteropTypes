using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    public class ArchiveFileInfo : IFileInfo
    {
        public ArchiveFileInfo(SharpCompress.Archives.IArchiveEntry entry)
        {
            _Entry = entry;
        }        

        private readonly SharpCompress.Archives.IArchiveEntry _Entry;

        public bool Exists => true;

        public long Length => _Entry.Size;

        public string PhysicalPath => _Entry.Key;

        public string Name => System.IO.Path.GetFileName(_Entry.Key);

        public DateTimeOffset LastModified => _Entry.LastModifiedTime ?? DateTimeOffset.MinValue;

        public bool IsDirectory => _Entry.IsDirectory;

        public Stream CreateReadStream()
        {
            return _Entry.OpenEntryStream();
        }        
    }
}
