using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

using XFILEINFO = Microsoft.Extensions.FileProviders.IFileInfo;

namespace InteropTypes.IO
{
    /// <summary>
    /// Used to check equality between two <see cref="XFILEINFO"/> instances.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The comparison is not done simply on isolated instances, we also need a path to identify a unique resource.
    /// </para>
    /// <para>
    /// In order to improve equality checks, it could be possible to try some hidden mechanisms: <br/>
    /// GetService could expose some internal values that could be used for equality:
    /// - a guid.<br/>
    /// - a Sha256 of the file.<br/>
    /// - an Uri: this would be equivalent to a physical path, but it could also cover archive paths, using the pattern c:/whatever/tmp.zip?=file.txt
    /// </para>
    /// </remarks>
    public class FileInfoComparer : IEqualityComparer<XFILEINFO>
    {
        // https://stackoverflow.com/questions/430256/how-do-i-determine-whether-the-filesystem-is-case-sensitive-in-net
        // https://stackoverflow.com/questions/7344978/verifying-path-equality-with-net        

        // linux is case insensitive, but external drives on windows can also be.

        // so the procedure would be:
        // 1- check whether both files are located in the SAME DRIVE, if not, return false.
        // 2- check the drive type to identify whether to compare as case sensitive or not.

        public static FileInfoComparer OperatingSystem { get; } = new FileInfoComparer(FilePathUtils.PathComparisonMode);
        public static FileInfoComparer Ordinal { get; } = new FileInfoComparer(StringComparison.Ordinal);
        public static FileInfoComparer OrdinalIgnoreCase { get; } = new FileInfoComparer(StringComparison.OrdinalIgnoreCase);

        private FileInfoComparer(StringComparison cmp)
        {
            _Comparer = cmp;
        }

        private readonly StringComparison _Comparer;

        public virtual bool Equals(XFILEINFO x, XFILEINFO y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null) return false;
            if (y == null) return false;

            // file kind mismatch
            if (x.IsDirectory != y.IsDirectory) return false;

            // LastModified mismatch (references to the same resource must have matching LastModified value)
            if (x.LastModified != y.LastModified) return false;

            // Name mismatch
            if (!string.Equals(x.Name, y.Name, _Comparer)) return false;

            // NOTE: at this point, the ordering of the comparison can have different effects:
            // if PhysicalPath check is set before Type check, comparing Interop, Microsoft or any other
            // physical based implementation would result in success, but we may don't want that because
            // some types may provide additional information missing in the Microsoft implementation,
            // so we put the Type check before the physical path check.            

            // container type mismatch            
            if (x.GetType() != y.GetType()) return false;            

            // PhysicalPath match
            if (x.PhysicalPath != null && y.PhysicalPath != null)
            {
                // if there's a physical path match, there's no need to continue further
                if (string.Equals(GetSanitizedPath(x), GetSanitizedPath(y), _Comparer)) return true;
            }            

            // check for length mismatch
            if (!x.IsDirectory && !y.IsDirectory)
            {
                if (x.Exists && y.Exists)
                {
                    if (x.Length != y.Length) return false;
                }
            }

            // we cannot use PhysicalPath, so we need to try find out the path by traversing the path
            // back to the root, if that's possible

            // check path down the chain.
            var xpfound = XFile.TryGetParentContainer(x, out var xparent);
            var ypfound = XFile.TryGetParentContainer(y, out var yparent);
            if (xpfound != ypfound) return false;

            if (xpfound && ypfound)
            {
                if (object.ReferenceEquals(xparent, yparent)) return true;
                if (xparent == null) return false;
                if (yparent == null) return false;

                if (xparent is XFILEINFO xdir && yparent is XFILEINFO ydir)
                {
                    return Equals(xdir, ydir);
                }

                return false;
            }            

            // we do not have enough information to compare
            throw new NotSupportedException(x.GetType().Name + " =?= " + y.GetType().Name);            
        }

        public virtual int GetHashCode([DisallowNull] XFILEINFO obj)
        {
            if (obj == null) return 0;

            switch(obj)
            {
                case null: return 0;
                case PhysicalFileInfo typed: return typed.GetHashCode();
                case PhysicalDirectoryInfo typed: return typed.GetHashCode();
            }

            var h = GetSanitizedPath(obj).GetHashCode(_Comparer);

            return HashCode.Combine(obj.GetType(), obj.IsDirectory, obj.LastModified, h);
        }

        protected static string GetSanitizedPath(XFILEINFO xinfo)
        {
            var path = xinfo.PhysicalPath ?? xinfo.Name;
            return path.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.PathSeparator);
        }
    }
}
