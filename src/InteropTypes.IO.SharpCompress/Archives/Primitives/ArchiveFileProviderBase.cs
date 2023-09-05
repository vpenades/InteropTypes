using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using InteropTypes.IO.FileProviders;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace InteropTypes.IO.Archives.Primitives
{
    /// <summary>
    /// Represents an archive provider for a flattened list of entries.
    /// </summary>
    /// <typeparam name="TEntry"></typeparam>
    public abstract class ArchiveFileProviderBase<TEntry> : IFileProvider, IServiceProvider
    {
        #region API

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            subpath ??= string.Empty;
            return new ArchiveDirectoryInfoBase<TEntry>(this, subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var entry = GetAllEntries(false)
                .FirstOrDefault(entry => GetEntryKey(entry) == subpath);

            return entry != null
                ? new ArchiveFileInfoBase<TEntry>(this, entry)
                : (IFileInfo)new NotFoundFileInfo(subpath);
        }

        public virtual object GetService(Type serviceType)
        {
            return null;
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        #endregion

        #region implementation

        protected abstract IEnumerable<TEntry> GetAllEntries(bool onlyDirectories);

        /// <summary>
        /// Gets the subpath, or key string of the given entry.
        /// </summary>
        /// <param name="entry">the entry to query</param>
        /// <returns>a string representing an entry subpath or key</returns>
        protected internal abstract string GetEntryKey(TEntry entry);

        protected internal abstract long GetEntryFileLength(TEntry entry);

        protected internal abstract DateTimeOffset GetEntryLastWriteTime(TEntry entry);        

        protected internal virtual string GetEntryComment(TEntry entry) { return null; }

        protected internal abstract Stream OpenEntryReadStream(TEntry entry, FileMode mode);

        internal IEnumerable<IFileInfo> GetEntries(string subPath)
        {
            var level = PathUtils.SplitPath(subPath).Count();

            var dirs = _GetDirectoriesAtLevel(level);

            if (dirs != null)
            {
                foreach (var dir in dirs)
                {
                    yield return new ArchiveDirectoryInfoBase<TEntry>(this, dir);
                }
            }

            foreach (var entry in GetAllEntries(false).Where(entry => _IsContentPath(entry, subPath)))
            {
                yield return new ArchiveFileInfoBase<TEntry>(this, entry);
            }
        }

        private HashSet<string> _GetDirectoriesAtLevel(int levels)
        {
            // retrieve directories of a given level
            HashSet<string> dirs = null;

            foreach (var entry in GetAllEntries(true))
            {
                var entryKey = GetEntryKey(entry);

                if (entryKey.Count(PathUtils.IsPathSeparator) <= levels) continue;

                var dir = string.Empty;
                int i = levels + 1;

                foreach (var c in entryKey)
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

        private bool _IsContentPath(TEntry entry, string subPath)
        {
            var entryKey = GetEntryKey(entry);

            entryKey = _GetRelativePath(entryKey, subPath);

            if (entryKey.Length == 0) return false;

            return !PathUtils.ContainsSeparator(entryKey);
        }

        private static string _GetRelativePath(string entryKey, string subPath)
        {
            if (!entryKey.StartsWith(subPath)) return string.Empty;

            entryKey = entryKey.Substring(subPath.Length);

            if (entryKey.StartsWith(Path.DirectorySeparatorChar)) entryKey = entryKey.TrimStart(Path.DirectorySeparatorChar);
            else if (entryKey.StartsWith(Path.AltDirectorySeparatorChar)) entryKey = entryKey.TrimStart(Path.AltDirectorySeparatorChar);

            return entryKey;
        }

        #endregion
    }
}
