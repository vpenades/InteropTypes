using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using InteropTypes.IO.FileProviders;

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
            var level = PathUtils.SplitPath(_SubPath).Count();

            var dirs = _GetDirectoriesAtLevel(level);

            if (dirs != null)
            {
                foreach (var dir in dirs)
                {
                    yield return new ArchiveDirectoryInfo(_Archive, dir);
                }
            }

            foreach (var entry in _Archive.Entries.Where(entry => IsContentPath(entry.Key)))
            {
                if (!entry.IsDirectory) yield return new ArchiveFileInfo(entry);
            }
        }

        private HashSet<string> _GetDirectoriesAtLevel(int levels)
        {
            // retrieve directories of a given level
            HashSet<string> dirs = null;

            foreach (var entry in _Archive.Entries)
            {
                if (entry.Key.Count(PathUtils.IsPathSeparator) <= levels) continue;

                var dir = string.Empty;
                int i = levels + 1;

                foreach(var c in entry.Key)
                {
                    if (c.IsPathSeparator()) --i;
                    if (i == 0) break;

                    dir += c;
                }

                if (dir.Length > 0)
                {
                    dirs ??= new HashSet<string>();
                    dirs.Add(dir);
                }
            }

            return dirs;
        }

        private bool IsContentPath(string entryKey)
        {
            entryKey = GetRelativePath(entryKey);

            if (entryKey.Length == 0) return false;

            return !PathUtils.ContainsSeparator(entryKey);
        }

        private string GetRelativePath(string entryKey)
        {
            if (!entryKey.StartsWith(_SubPath)) return string.Empty;

            entryKey = entryKey.Substring(_SubPath.Length);

            #if NETSTANDARD2_0
            if (entryKey.StartsWith(Path.DirectorySeparatorChar.ToString())) entryKey = entryKey.TrimStart(Path.DirectorySeparatorChar);
            else if (entryKey.StartsWith(Path.AltDirectorySeparatorChar.ToString())) entryKey = entryKey.TrimStart(Path.AltDirectorySeparatorChar);
            #else
            if (entryKey.StartsWith(Path.DirectorySeparatorChar)) entryKey = entryKey.TrimStart(Path.DirectorySeparatorChar);
            else if (entryKey.StartsWith(Path.AltDirectorySeparatorChar)) entryKey = entryKey.TrimStart(Path.AltDirectorySeparatorChar);
            #endif

            return entryKey;
        }
    }    
}
