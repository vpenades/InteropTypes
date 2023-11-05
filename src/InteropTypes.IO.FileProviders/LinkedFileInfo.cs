using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

using Microsoft.Extensions.FileProviders;

using XFILEINFO = Microsoft.Extensions.FileProviders.IFileInfo;

namespace InteropTypes.IO
{
    /// <summary>
    /// Represents a <see cref="XFILEINFO"/> linked to its parent <see cref="IDirectoryContents"/>.
    /// </summary>
    /// <remarks>
    /// One drawback of <see cref="XFILEINFO"/> is that it cannot navigate back to its parent,
    /// so when we do a forward naviagation, we can keep the recursive parents as a linked list.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{ToString(),nq}")]
    public class LinkedFileInfo : IEquatable<LinkedFileInfo>, XFILEINFO, IServiceProvider
    {
        #region lifecycle

        public static LinkedFileInfo Create(XFILEINFO xinfo)
        {
            if (xinfo == null) throw new ArgumentNullException(nameof(xinfo));

            // if it's already a linked file, return it
            if (xinfo is LinkedFileInfo other) return other;

            return xinfo.IsDirectory
                ? new _LinkedDirectoryInfo(null, xinfo)
                : new LinkedFileInfo(null, xinfo);
        }

        public static LinkedFileInfo Create(IDirectoryContents parent, XFILEINFO xinfo)
        {
            if (parent == null) return Create(xinfo);

            if (xinfo == null) throw new ArgumentNullException(nameof(xinfo));

            if (xinfo is LinkedFileInfo) throw new ArgumentException("already has a parent", nameof(xinfo));

            return xinfo.IsDirectory
                ? new _LinkedDirectoryInfo(parent, xinfo)
                : new LinkedFileInfo(parent, xinfo);
        }

        public static LinkedFileInfo Create(IDirectoryContents[] path, XFILEINFO xinfo)
        {
            if (path == null || path.Length == 0) return Create(xinfo);
            if (path.Any(part => part == null)) throw new ArgumentException("null path", nameof(path));
            if (path.OfType<XFILEINFO>().Any(part => !part.IsDirectory)) throw new ArgumentException("invalid path", nameof(path));

            if (xinfo is LinkedFileInfo) throw new ArgumentException("already has a parent", nameof(xinfo));

            IDirectoryContents parent = null;

            foreach(var part in path)
            {
                if (part is XFILEINFO xpart) parent = Create(parent, xpart) as IDirectoryContents;
                else parent = part;
            }

            return Create(parent, xinfo);
        }

        protected LinkedFileInfo(IDirectoryContents parent, XFILEINFO xinfo)
        {
            if (xinfo == null) throw new ArgumentNullException(nameof(xinfo));            

            Parent = parent;
            Entry = xinfo;
        }

        #endregion

        #region data

        /// <summary>
        /// Represents the parent Directory, or null.
        /// </summary>
        public IDirectoryContents Parent { get; }

        /// <summary>
        /// Represents a File or a Directory.
        /// </summary>
        public XFILEINFO Entry { get; }        

        public override int GetHashCode() { return HashCode.Combine(Parent, Entry); }
        public override bool Equals(object obj)
        {
            return obj is LinkedFileInfo other && Equals(other);
        }
        public static bool operator ==(LinkedFileInfo left, LinkedFileInfo right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(LinkedFileInfo left, LinkedFileInfo right)
        {
            return !left.Equals(right);
        }
        public bool Equals(LinkedFileInfo other)
        {
            if (!Object.Equals(this.Parent, other.Parent)) return false;
            if (!Object.Equals(this.Entry, other.Entry)) return false;
            return true;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out IDirectoryContents parent, out XFILEINFO entry)
        {
            parent = this.Parent;
            entry = this.Entry;
        }

        #endregion

        #region properties

        public bool Exists => Entry.Exists;

        public long Length => Entry.Length;

        public string PhysicalPath => Entry.PhysicalPath;

        public string Name => Entry.Name;

        public DateTimeOffset LastModified => Entry.LastModified;

        public bool IsDirectory => Entry.IsDirectory;

        public int Depth
        {
            get
            {
                if (Parent == null) return 0;
                if (Parent is LinkedFileInfo linked) return linked.Depth + 1;
                return 1;
            }
        }

        #endregion

        #region API

        public Stream CreateReadStream()
        {
            return Entry.CreateReadStream();
        }        

        public override string ToString()
        {
            return ToString(Path.DirectorySeparatorChar);
        }

        public string ToString(char separator)
        {
            var xpath = this.Name;

            var parent = this.Parent;

            while(parent is XFILEINFO xparent)
            {
                xpath = xparent.Name + separator + xpath;

                if (parent is LinkedFileInfo linked)
                {
                    parent = linked.Parent;
                }
            }            

            return xpath;
        }

        public IEnumerable<XFILEINFO> GetDirectoryPath()
        {
            var xparent = Parent as XFILEINFO;

            if (xparent == null) return Enumerable.Empty<XFILEINFO>();

            var tail = new[] { xparent };

            if (Parent is LinkedFileInfo linkedParent)
            {
                return linkedParent.GetDirectoryPath().Concat(tail);
            }

            return tail;
        }        

        public object GetService(Type serviceType)
        {
            return Entry is IServiceProvider srv
                ? srv.GetService(serviceType) 
                : null;
        }

        #endregion

        #region static API

        public static IEnumerable<LinkedFileInfo> EnumerateFiles(XFILEINFO dinfo, SearchOption searchOption)
        {
            dinfo ??= new NotFoundFileInfo("null");

            return XFile.TryGetDirectoryContents(dinfo, out var contents)
                ? EnumerateFiles(contents, searchOption)
                : Enumerable.Empty<LinkedFileInfo>();
        }

        public static IEnumerable<LinkedFileInfo> EnumerateFiles(IDirectoryContents contents, SearchOption searchOption)
        {
            contents ??= NotFoundDirectoryContents.Singleton;

            var basePath = new[] { contents };

            if (searchOption == SearchOption.TopDirectoryOnly)
            {
                return contents.Select(entry => Create(entry));
            }

            return _RecursivelyEnumerateFiles(basePath)
                ?? Array.Empty<LinkedFileInfo>();
        }

        private static IEnumerable<LinkedFileInfo> _RecursivelyEnumerateFiles(IDirectoryContents[] basePath)
        {
            var entries = basePath.Last();

            // dig tree

            foreach (var entry in entries)
            {
                if (XFile.TryGetDirectoryContents(entry, out var contents))
                {
                    var subPath = basePath.Concat(new[] { contents }).ToArray();

                    foreach (var child in _RecursivelyEnumerateFiles(subPath))
                    {
                        yield return child;
                    }
                }
                else
                {
                    yield return Create(basePath, entry);
                }
            }
        }        

        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("{ToString(),nq}")]
    sealed class _LinkedDirectoryInfo : LinkedFileInfo , IDirectoryContents
    {
        internal _LinkedDirectoryInfo(IDirectoryContents parent, XFILEINFO xinfo)
            : base(parent, xinfo)
        {
            if (!xinfo.IsDirectory) throw new ArgumentException("must be a directory", nameof(xinfo));
            
        }

        IEnumerator<XFILEINFO> IEnumerable<XFILEINFO>.GetEnumerator()
        {
            return _GetEntryContents().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _GetEntryContents().GetEnumerator();
        }

        private IDirectoryContents _GetEntryContents()
        {
            return XFile.TryGetDirectoryContents(Entry, out var contents)
                ? contents
                : NotFoundDirectoryContents.Singleton;
        }
    }
        
}
