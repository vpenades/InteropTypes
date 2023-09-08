using System;
using System.Collections.Generic;
using System.IO;

namespace InteropTypes.IO
{
    /// <summary>
    /// Used to compare equality between <see cref="FileInfo"/> and <see cref="DirectoryInfo"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FileSystemInfoComparer<T> : IEqualityComparer<T>
        where T : FileSystemInfo
    {
        private FileSystemInfoComparer() { }

        public static FileSystemInfoComparer<T> Default { get; } = new FileSystemInfoComparer<T>();

        public bool Equals(T x, T y)
        {
            if (Object.ReferenceEquals(x, y)) return true;            
            if (x == null) return false;
            if (y == null) return false;            

            if (x is FileInfo fa && y is FileInfo fb)
            {
                if (fa.Length != fb.Length) return false;                
            }

            var apath = x.FullName;
            var bpath = y.FullName;            

            // https://stackoverflow.com/questions/430256/how-do-i-determine-whether-the-filesystem-is-case-sensitive-in-net
            // https://stackoverflow.com/questions/7344978/verifying-path-equality-with-net

            // linux is case insensitive, but external drives on windows can also be.

            // so the procedure would be:
            // 1- check whether both files are located in the SAME DRIVE, if not, return false.
            // 2- check the drive type to identify whether to compare as case sensitive or not.

            return string.Equals(apath, bpath, FilePathUtils.PathComparisonMode);
        }

        public int GetHashCode(T obj)
        {
            return obj == null
                ? 0
                : obj.FullName.GetHashCode(FilePathUtils.PathComparisonMode);
        }

        
    }
}
