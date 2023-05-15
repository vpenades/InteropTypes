using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Extensions.FileProviders;

using SharpCompress.Archives;

namespace InteropTypes.IO
{
    public class ArchiveDirectoryInfo : IFileInfo, IDirectoryContents
    {
        public ArchiveDirectoryInfo(IArchive archive, string subPath)
        {
            _Archive = archive;
            _SubPath = subPath;
        }

        private IArchive _Archive;
        private string _SubPath;

        public bool Exists => true;

        long IFileInfo.Length => -1;

        public string PhysicalPath => _SubPath;

        public string Name => System.IO.Path.GetFileName(_SubPath);

        public DateTimeOffset LastModified => DateTimeOffset.MinValue;

        public bool IsDirectory => true;

        Stream IFileInfo.CreateReadStream() { throw new NotSupportedException(); }

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            return GetEntries().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEntries().GetEnumerator();
        }

        private IEnumerable<IFileInfo> GetEntries()
        {
            try
            {
                return _Archive
                    .Entries
                    .Where(entry => IsContentPath(entry.Key))
                    .Select<IArchiveEntry, IFileInfo>(entry =>
                    {
                        if (entry.IsDirectory)
                        {
                            return new ArchiveDirectoryInfo(_Archive, entry.Key);
                        }
                        else
                        {
                            return new ArchiveFileInfo(entry);
                        }
                    });
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException || ex is IOException)
            {
                return Enumerable.Empty<IFileInfo>();
            }
        }

        private bool IsContentPath(string entryKey)
        {
            if (!entryKey.StartsWith(_SubPath)) return false;

            entryKey = entryKey.Substring(_SubPath.Length);

#if NETSTANDARD2_0
            if (entryKey.StartsWith(Path.DirectorySeparatorChar.ToString())) entryKey = entryKey.TrimStart(Path.DirectorySeparatorChar);
            else if (entryKey.StartsWith(Path.AltDirectorySeparatorChar.ToString())) entryKey = entryKey.TrimStart(Path.AltDirectorySeparatorChar);
#else
            if (entryKey.StartsWith(Path.DirectorySeparatorChar)) entryKey = entryKey.TrimStart(Path.DirectorySeparatorChar);
            else if (entryKey.StartsWith(Path.AltDirectorySeparatorChar)) entryKey = entryKey.TrimStart(Path.AltDirectorySeparatorChar);
#endif

            if (entryKey.Length == 0) return false;

            if (entryKey.Contains(Path.DirectorySeparatorChar)) return false;
            if (entryKey.Contains(Path.AltDirectorySeparatorChar)) return false;

            return true;
        }
    }
}
