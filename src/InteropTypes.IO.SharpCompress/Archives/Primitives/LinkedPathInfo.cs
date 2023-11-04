using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace InteropTypes.IO
{
    /// <summary>
    /// Represents a linked list component of a file system path.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The path is always traversed from tail to head:<br/>
    /// File => Path => Path => Path => Path => null
    /// </para>
    /// <para>
    /// implemented by: <see cref="DirectoryPathLink"/> and <see cref="FilePathLink"/>
    /// </para>
    /// </remarks>
    abstract class PathLink
    {
        #region lifecycle
        protected PathLink(DirectoryPathLink parent, string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (name.IndexOfAny(_Separators) >= 0) throw new ArgumentException("must not contain separators", nameof(name));

            Parent = parent;
            Name = name;
        }

        #endregion

        #region data

        public DirectoryPathLink Parent { get; }
        public string Name { get; }
        public int GetHashCode(StringComparison comparisonType)
        {
            var h = Name.GetHashCode(comparisonType);
            return HashCode.Combine(h, Parent);
        }
        public bool Equals(PathLink other, StringComparison comparisonType)
        {
            if (object.ReferenceEquals(this, other)) return true;
            if (other == null) return false;

            // compare name
            
            if (!Name.Equals(other.Name, comparisonType)) return false;

            // compare parents

            if (Parent == null) return other.Parent == null;

            return Parent.Equals(other.Parent, comparisonType);            
        }

        #endregion

        #region properties

        public DirectoryPathLink Head
        {
            get
            {
                var h = this as DirectoryPathLink;
                while (h != null)
                {
                    if (h.Parent == null) return h;
                    h = h.Parent;
                }
                return null;
            }
        }

        public abstract bool IsDirectory { get; }

        #endregion

        #region API

        public override string ToString()
        {
            return ToString(Path.DirectorySeparatorChar);
        }

        public string ToString(char separator)
        {
            var n = Parent == null
                ? Name
                : Parent.ToString(separator) + separator + Name;

            return n;  // TODO: if this is a directory, we could append separator ??
        }

        #endregion

        #region static API

        private static readonly char[] _Separators = new[]
        {
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar
        };

        protected static bool EndsInSeparator(string path)
        {
            if (path == null) return false;
            if (path.Length == 0) return false;

            var c = path[path.Length - 1];
            return _Separators.Contains(c);
        }

        protected static string TrimSeparators(string path)
        {
            return path == null
                ? string.Empty
                : path.Trim(_Separators);
        }

        protected static string TrimStartSeparators(string path)
        {
            return path == null
                ? string.Empty
                : path.TrimStart(_Separators);
        }

        protected static string TrimEndSeparators(string path)
        {
            return path == null
                ? string.Empty
                : path.TrimEnd(_Separators);
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

        #endregion

        #region nested types

        public static IEqualityComparer<T> GetComparer<T>(StringComparison ctype)
            where T: PathLink
        {
            return new _EqualityComparer<T>(ctype);
        }

        readonly struct _EqualityComparer<T>
            : IEqualityComparer<T> where T:PathLink
        {
            public _EqualityComparer(StringComparison ctype)
            {
                _CompType = ctype;
            }

            private readonly StringComparison _CompType;

            public bool Equals(T x, T y)
            {                
                if (x != null) return x.Equals(y, _CompType);
                if (y != null) return y.Equals(x, _CompType);
                return true; // both null
            }

            public int GetHashCode([DisallowNull] T obj)
            {
                return obj.GetHashCode(_CompType);
            }
        }

        #endregion
    }


    [System.Diagnostics.DebuggerDisplay("📁 {ToString()}")]
    sealed class DirectoryPathLink : PathLink
    {
        #region lifecycle
        public static DirectoryPathLink Parse(string path, Dictionary<string,DirectoryPathLink> pathsLUT)
        {
            path = TrimEndSeparators(path);

            if (pathsLUT != null && pathsLUT.TryGetValue(path, out var dplink)) return dplink;

            if (TrySplitLastSeparator(path, out var head, out var tail))
            {
                var parent = Parse(head);
                dplink = new DirectoryPathLink(parent, tail);
            }
            else
            {
                dplink = new DirectoryPathLink(null, path);
            }

            if (pathsLUT != null) pathsLUT[path] = dplink;

            return dplink;
        }

        public static DirectoryPathLink Parse(string path)
        {
            path = TrimEndSeparators(path);
            
            if (TrySplitLastSeparator(path, out var head, out var tail))
            {
                var parent = Parse(head);
                return new DirectoryPathLink(parent, tail);
            }
            else
            {
                return new DirectoryPathLink(null, path);
            }
        }

        internal DirectoryPathLink(DirectoryPathLink parent, string name)
            : base(parent, name)
        {
            if (name == "." || name == "..") throw new ArgumentException("invalid path", nameof(name));
        }

        #endregion

        #region properties

        public override bool IsDirectory => true;

        #endregion

        #region API

        public DirectoryPathLink TrimHeadPath()
        {
            if (this.Parent == null) return null;
            return new DirectoryPathLink(Parent.TrimHeadPath(), this.Name);
        }

        public FilePathLink AppendFile(string name)
        {
            return new FilePathLink(this, name);
        }

        public DirectoryPathLink AppendPath(string name)
        {
            return new DirectoryPathLink(this, name);
        }

        public DirectoryPathLink PrependPath(string parentName)
        {
            var p = Parent == null
                ? new DirectoryPathLink(null, parentName)
                : Parent.PrependPath(parentName);

            return new DirectoryPathLink(p, this.Name);
        }

        public DirectoryPathLink PrependPath(DirectoryPathLink parent)
        {
            var p = Parent == null
                ? parent
                : Parent.PrependPath(parent);

            return new DirectoryPathLink(p, this.Name);
        }

        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("🗎 {ToString()}")]
    sealed class FilePathLink : PathLink
    {
        #region lifecycle

        public static IReadOnlyDictionary<FilePathLink, TEntry> ToDictionary<TEntry>(IEnumerable<TEntry> entries, Func<TEntry, string> pathSelector, StringComparison comparerType)
        {
            var comparer = GetComparer<FilePathLink>(comparerType);
            return ToDictionary(entries, pathSelector, comparer);
        }

        public static IReadOnlyDictionary<FilePathLink,TEntry> ToDictionary<TEntry>(IEnumerable<TEntry> entries, Func<TEntry, string> pathSelector, IEqualityComparer<FilePathLink> comparer)
        {
            var partDict = new Dictionary<string, DirectoryPathLink>();

            FilePathLink _getKey(TEntry entry)
            {                
                var path = pathSelector(entry);
                if (string.IsNullOrEmpty(path)) return null;
                if (EndsInSeparator(path)) return null;

                return Parse(path, partDict);
            }

            var pairs = entries
                .Select(entry=> (_getKey(entry), entry))
                .Where(pair => pair.Item1 != null);

            return pairs.ToDictionary(pair => pair.Item1, pair => pair.entry, comparer);
        }

        public static FilePathLink Parse(string path, Dictionary<string, DirectoryPathLink> pathsLUT = null)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            if (EndsInSeparator(path)) throw new ArgumentException("not a file", nameof(path));
            if (TrySplitLastSeparator(path, out var head, out var tail))
            {
                var parent = DirectoryPathLink.Parse(head, pathsLUT);
                return new FilePathLink(parent, tail);
            }
            else
            {
                return new FilePathLink(null, path);
            }
        }        

        internal FilePathLink(DirectoryPathLink parent, string name)
            : base(parent, name) { }

        #endregion

        #region properties

        public override bool IsDirectory => false;

        #endregion

        #region API

        public FilePathLink TrimHeadPath()
        {
            return this.Parent == null
                ? this
                : new FilePathLink(Parent.TrimHeadPath(), this.Name);
        }

        public FilePathLink PrependPath(string parentName)
        {
            var p = Parent == null
                ? new DirectoryPathLink(null, parentName)
                : Parent.PrependPath(parentName);

            return new FilePathLink(p, this.Name);
        }

        public FilePathLink PrependPath(DirectoryPathLink parent)
        {
            var p = Parent == null
                ? parent
                : Parent.PrependPath(parent);

            return new FilePathLink(p, this.Name);
        }

        #endregion
    }
}
