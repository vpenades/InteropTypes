using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO.Collections
{
    /// <summary>
    /// Represents a generic container of <see cref="IFileInfo"/> objects
    /// </summary>
    class ReadOnlyDirectoryContents : IDirectoryContents
    {
        #region lifecycle

        public static ReadOnlyDirectoryContents MakeTree(IReadOnlyDictionary<string, IFileInfo> entries, string basePath, StringComparison pathComparer)
        {
            var content = _MakeTree(entries, basePath, pathComparer);
            return new ReadOnlyDirectoryContents(content, pathComparer);
        }

        protected static IReadOnlyList<IFileInfo> _MakeTree(IReadOnlyDictionary<string,IFileInfo> entries, string basePath, StringComparison pathComparer)
        {
            var content = new List<IFileInfo>();

            var dirs = new HashSet<string>();

            foreach(var entry in entries)
            {
                var key = entry.Key.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                key = key.TrimStart(_Separators);

                if (!string.IsNullOrEmpty(basePath))
                {
                    if (!key.StartsWith(basePath, pathComparer)) continue;
                    key = key.Substring(basePath.Length);
                    key = key.TrimStart(_Separators);
                }

                if (TrySplitFirstSeparator(key, out var keyHead, out var keyTail))
                {
                    dirs.Add(keyHead);
                }
                else if (!entry.Value.IsDirectory)
                {
                    content.Add(entry.Value);
                }
                else
                {
                    throw new InvalidOperationException();
                }                
            }

            foreach(var dir in dirs)
            {
                var dcontent = _MakeTree(entries, System.IO.Path.Combine(basePath, dir), pathComparer);

                var dinfo = new ReadOnlyDirectoryInfo(dir, dcontent, pathComparer);

                content.Add(dinfo);
            }

            return content;
        }

        public ReadOnlyDirectoryContents(IReadOnlyList<IFileInfo> contents, StringComparison pathComparer)
        {            
            _Contents = contents.ToDictionary(entry => entry.Name, entry=>entry, StringComparer.FromComparison(pathComparer));
            _PathComparer = pathComparer;
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly Dictionary<string, IFileInfo> _Contents;

        #if DEBUG
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
        private IReadOnlyList<IFileInfo> _Entries => _Contents.Values.ToArray();
        #endif

        private readonly StringComparison _PathComparer;

        #endregion

        #region API

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public bool Exists => true;
        public IEnumerator<IFileInfo> GetEnumerator() { return _Contents.Values.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _Contents.Values.GetEnumerator(); }

        public IFileInfo GetFileInfo(string subpath)
        {
            subpath ??= string.Empty;
            subpath = subpath.Trim(_Separators);

            if (string.IsNullOrEmpty(subpath)) return new NotFoundFileInfo(subpath);

            ReadOnlyDirectoryContents dcontents = this;

            if (TrySplitLastSeparator(subpath, out var path, out var fileName))
            {
                dcontents = GetDirectoryContents(path) as ReadOnlyDirectoryContents;
                subpath = fileName;
            }

            if (dcontents._Contents.TryGetValue(subpath, out var entry))
            {
                if (!entry.IsDirectory) return entry;
            }

            return new NotFoundFileInfo(subpath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            subpath ??= string.Empty;
            subpath = subpath.Trim(_Separators);

            if (string.IsNullOrEmpty(subpath)) return this;

            if (subpath.IndexOfAny(_Separators) < 0) // look for directory here
            {
                if (_Contents.TryGetValue(subpath, out var entry))
                {
                    if (entry is IDirectoryContents dcontents) return dcontents;
                }
            }

            if (TrySplitFirstSeparator(subpath, out var head, out var tail)) // look into subdirectories
            {
                if (_Contents.TryGetValue(head, out var entry) && entry is ReadOnlyDirectoryInfo dinfo)
                {
                    var dcontents = dinfo.GetDirectoryContents(tail);
                    if (dcontents != null) return dcontents;
                }
            }

            return NotFoundDirectoryContents.Singleton;
        }

        protected static bool TrySplitFirstSeparator(string path, out string head, out string tail)
        {
            var idx = path == null
                ? -1
                : path.IndexOfAny(_Separators);

            if (idx <= 0) { head = null; tail = null; return false; }

            head = path.Substring(0, idx);
            tail = path.Substring(idx + 1);
            return true;
        }

        protected static bool TrySplitLastSeparator(string path, out string head, out string tail)
        {
            var idx = path == null
                ? -1
                : path.LastIndexOfAny(_Separators);

            if (idx <= 0) { head = null; tail = null; return false; }

            head = path.Substring(0, idx);
            tail = path.Substring(idx + 1);
            return true;
        }

        private static readonly char[] _Separators = new[]
        {
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar
        };

        #endregion
    }


    /// <summary>
    /// Represents a generic directory.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("📁 {Name}")]
    class ReadOnlyDirectoryInfo : ReadOnlyDirectoryContents, IFileInfo
    {
        #region lifecycle

        public ReadOnlyDirectoryInfo(string name, IReadOnlyList<IFileInfo> contents, StringComparison pathComparer)
            : base(contents, pathComparer)
        {
            this.Name = name;
            LastModified = contents.Count == 0
                ? DateTimeOffset.MinValue
                : contents
                .Select(item => item.LastModified)
                .Aggregate((a,b) => a.Ticks > b.Ticks ? a:b);
        }

        #endregion

        #region data

        public string Name { get; }
        public DateTimeOffset LastModified { get; }

        #endregion

        #region constants

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        long IFileInfo.Length => -1;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        string IFileInfo.PhysicalPath => null;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        bool IFileInfo.IsDirectory => true;
        Stream IFileInfo.CreateReadStream() { throw new NotImplementedException(); }

        #endregion
    }
}
