using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

#if DISABLENUGETTEMPLATE
namespace InteropTypes.IO
#else
namespace $rootnamespace$
#endif
{
    internal static class _FileSystemInfoExtensions
    {
        private static readonly char[] _InvalidChars = System.IO.Path.GetInvalidFileNameChars();

        public static System.IO.FileInfo UseFile(this DirectoryInfo dinfo, params string[] relativePath)
        {
            var path = _GetPath(dinfo, relativePath);
            return new System.IO.FileInfo(path);
        }        

        public static System.IO.DirectoryInfo UseDirectory(this DirectoryInfo dinfo, params string[] relativePath)
        {            
            var path = _GetPath(dinfo, relativePath);
            return new System.IO.DirectoryInfo(path);
        }        

        private static string _GetPath(DirectoryInfo dinfo, string[] relativePath)
        {
            if (dinfo == null) throw new ArgumentNullException(nameof(dinfo));
            if (relativePath == null) throw new ArgumentNullException(nameof(relativePath));

            var path = dinfo.FullName;
            foreach (var part in relativePath)
            {
                if (string.IsNullOrWhiteSpace(part)) throw new ArgumentNullException(nameof(relativePath));
                if (part.IndexOfAny(_InvalidChars) >= 0) throw new ArgumentException($"{part} contains invalid chars", nameof(relativePath));
                path = System.IO.Path.Combine(path, part);
            }

            return path;
        }

        public static string GetRelativePath(this FileInfo finfo, DirectoryInfo baseInfo)
        {
            if (finfo == null) throw new ArgumentNullException(nameof(finfo));
            if (baseInfo == null) throw new ArgumentNullException(nameof (baseInfo));

            // TODO: check for different drives

            var path = finfo.FullName;            

            while(baseInfo != null)
            {
                if (baseInfo.Contais(finfo)) return baseInfo.FullName.Substring(path.Length).TrimStart('\\').TrimStart('/');
                baseInfo = baseInfo.Parent;
            }

            throw new ArgumentException("invalid path", nameof(baseInfo));
        }

        public static string Contains(this DirectoryInfo dinfo, FileSystemInfo sinfo)
        {
            return sinfo.FullNameStartsWith(dinfo.FullName);
        }

        /// <summary>
        /// Tries to get a composite extension of a file.
        /// </summary>
        /// <param name="fileName">the filename from where to get the extension.</param>
        /// <param name="dots">the number of dots used by the extension.</param>
        /// <param name="extension">the resulting extension.</param>
        /// <returns>true if an extension was found</returns>        
        public static bool TryGetCompositedExtension(this FileInfo finfo, int dots, out string extension)
        {
            if (finfo == null) throw new ArgumentNullException(nameof(finfo));

            var fileName = finfo.FullName;

            if (dots < 1) throw new ArgumentOutOfRangeException(nameof(dots), "Must be equal or greater than 1");

            var l = fileName.Length - 1;
            var r = -1;

            while (dots > 0 && l >= 0)
            {
                var c = fileName[l];

                if (IsDirectorySeparator(c) || c == ':' || c=='?' || c=='*') break;

                if (c == '.')
                {
                    r = l;
                    --dots;
                }

                --l;
            }

            if (dots != 0)
            {
                extension = null;
                return false;
            }

            extension = fileName.Substring(r);
            return true;
        }
        
        public static bool FullNameEquals(this FileSystemInfo a, FileSystemInfo b)
        {
            if (Object.ReferenceEquals(a, b)) return true;
            if (object.ReferenceEquals(a, null)) return false;
            if (object.ReferenceEquals(b, null)) return false;

            return a.FullNameEquals(b.FullName);
        }

        public static bool FullNameEquals(this FileSystemInfo a, string path)
        {            
            if (object.ReferenceEquals(a, null)) return false;            

            var aPath = a.FullName.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
            path = path.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);

            return string.Equals(aPath, path, StringComparison.OrdinalIgnoreCase);
        }

        public static bool FullNameStartsWith(this FileSystemInfo a, string path)
        {
            if (object.ReferenceEquals(a, null)) return false;

            var aPath = a.FullName.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
            path = path.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);

            return aPath.StartsWith(path, StringComparison.OrdinalIgnoreCase);
        }

        public static IEqualityComparer<FileInfo> GetDefaultComparer(this FileInfo finfo)
        {
            return _FileSystemInfoComparer<FileInfo>.Default;
        }


        sealed class _FileSystemInfoComparer<T> : IEqualityComparer<T>
        where T : FileSystemInfo
        {
            private _FileSystemInfoComparer() { }

            public static _FileSystemInfoComparer<T> Default { get; } = new _FileSystemInfoComparer<T>();

            public bool Equals(T x, T y)
            {
                if (Object.ReferenceEquals(x, y)) return true;            
                if (x == null) return false;
                if (y == null) return false;            

                if (x is FileInfo fa && y is FileInfo fb)
                {
                    if (fa.Length != fb.Length) return false;                
                }

                var apath = x.FullName.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
                var bpath = y.FullName.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);            

                // https://stackoverflow.com/questions/430256/how-do-i-determine-whether-the-filesystem-is-case-sensitive-in-net
                // https://stackoverflow.com/questions/7344978/verifying-path-equality-with-net

                // linux is case insensitive, but external drives on windows can also be.

                // so the procedure would be:
                // 1- check whether both files are located in the SAME DRIVE, if not, return false.
                // 2- check the drive type to identify whether to compare as case sensitive or not.

                return string.Equals(apath, bpath, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(T obj)
            {
                return obj == null
                    ? 0
                    : obj.FullName
                    .Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar)
                    .GetHashCode(StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
