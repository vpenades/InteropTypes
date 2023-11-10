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
    public class LinkedFileInfo : XFILEINFO, IServiceProvider
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

            // check for circular references
            var p = parent;
            while(p != null)
            {
                if (Object.ReferenceEquals(p, xinfo)) throw new ArgumentException("circular reference", nameof(xinfo));
                p = p is LinkedFileInfo plink ? plink.Parent : null;
            }

            return xinfo.IsDirectory
                ? new _LinkedDirectoryInfo(parent, xinfo)
                : new LinkedFileInfo(parent, xinfo);
        }        

        protected LinkedFileInfo(IDirectoryContents parent, XFILEINFO xinfo)
        {
            if (xinfo is LinkedFileInfo) throw new ArgumentException("xinfo cannot be a linked item", nameof(xinfo));

            Parent = parent;
            Entry = xinfo ?? throw new ArgumentNullException(nameof(xinfo));
        }

        #endregion

        #region data

        /// <summary>
        /// Represents the parent Directory, or null.
        /// </summary>
        /// <remarks>
        /// This property can be casted to <see cref="LinkedFileInfo"/>
        /// to navigate back to the previous Parent down to the root.
        /// </remarks>
        public IDirectoryContents Parent { get; }

        /// <summary>
        /// Represents a File or a Directory.
        /// </summary>
        public XFILEINFO Entry { get; }

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

        public static IEnumerable<LinkedFileInfo> Enumerate(XFILEINFO dinfo, SearchOption searchOption)
        {
            return XFile.TryCastToDirectoryContents(dinfo, out var contents)
                ? Enumerate(contents, searchOption)
                : Enumerable.Empty<LinkedFileInfo>();
        }

        public static IEnumerable<LinkedFileInfo> Enumerate(IDirectoryContents contents, SearchOption searchOption)
        {
            if (contents == null) return Enumerable.Empty<LinkedFileInfo>();

            return searchOption == SearchOption.TopDirectoryOnly
                ? contents.Select(Create)
                : _EnumerateRecursively(contents);
        }

        private static IEnumerable<LinkedFileInfo> _EnumerateRecursively(IDirectoryContents parent)
        {
            if (parent == null) yield break;

            foreach (var entry in parent)
            {
                System.Diagnostics.Debug.Assert(entry != null);
                if (entry == null) continue;

                var linkedEntry = Create(parent, entry);
                System.Diagnostics.Debug.Assert(linkedEntry != null);
                if (linkedEntry == null) continue;

                yield return linkedEntry;                

                if (XFile.TryCastToDirectoryContents(entry, out var contents))
                {
                    System.Diagnostics.Debug.Assert(entry.IsDirectory);

                    foreach (var child in _EnumerateRecursively(contents))
                    {
                        yield return child;
                    }
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
            return XFile.TryCastToDirectoryContents(Entry, out var contents)
                ? contents
                : NotFoundDirectoryContents.Singleton;
        }
    }
        
}
