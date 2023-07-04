using System;
using System.Collections.Generic;
using System.IO;

namespace InteropTypes.IO
{
    /// <summary>
    /// Used to compare equality between <see cref="FileInfo"/> and <see cref="FileSystemInfo"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FileSystemInfoComparer<T> : IEqualityComparer<T>
        where T : FileSystemInfo
    {
        private FileSystemInfoComparer() { }

        public static FileSystemInfoComparer<T> Default { get; } = new FileSystemInfoComparer<T>();

        public bool Equals(T x, T y)
        {
            if (x == null && y == null) return true;
            if (x == null) return false;
            if (y == null) return false;

            if (Object.ReferenceEquals(x, y)) return true;

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

            return string.Equals(apath, bpath, PathComparisonMode);
        }

        public int GetHashCode(T obj)
        {
            if (obj == null) return 0;            

            #if NETSTANDARD2_0
            var path = obj.FullName;
            if (PathComparisonMode != StringComparison.Ordinal) path = path.ToLower();
            return path.GetHashCode();            
            #else
            return obj.FullName.GetHashCode(PathComparisonMode);
            #endif
        }

        private static readonly StringComparison PathComparisonMode = _GetIsCaseSensitive()
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;

        private static bool _GetIsCaseSensitive()
        {
            // Proposal: Directory.GetCaseSensitivity() -> https://github.com/dotnet/runtime/issues/34235
            // Issue with System.IO.GetIsCaseSensitive -> https://github.com/dotnet/runtime/issues/54313
            // Change PathInternal.IsCaseSensitive to a constant -> https://github.com/dotnet/runtime/pull/54340

            // another way to determine case sensitivity is to pick a random file or directory,
            // then invert the letters capitalization, and check for its existence.

            #if !NETSTANDARD
            if (OperatingSystem.IsWindows()) return false;
            if (OperatingSystem.IsMacOS()) return false;
            if (OperatingSystem.IsMacCatalyst()) return false;
            if (OperatingSystem.IsIOS()) return false;
            if (OperatingSystem.IsTvOS()) return false;
            if (OperatingSystem.IsWatchOS()) return false;
            #endif

            return false;
        }        
    }
}
