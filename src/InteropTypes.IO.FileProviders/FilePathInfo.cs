using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InteropTypes.IO
{
    /// <summary>
    /// Retrieves information about a file system path
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Path}")]
    public readonly partial struct FilePathInfo : IEquatable<FilePathInfo>
    {
        #region lifecycle

        public static FilePathInfo CreateRandom()
        {
            var path = System.IO.Path.GetRandomFileName();
            return new FilePathInfo(path);
        }

        public static FilePathInfo CreateTempFile()
        {
            var path = System.IO.Path.GetTempFileName();
            return new FilePathInfo(path);
        }

        public FilePathInfo(string path)
        {
            Path = path;
        }

        public FilePathInfo(System.IO.DriveInfo dinfo)
        {
            Path = dinfo.RootDirectory.FullName;
        }

        public FilePathInfo(System.IO.FileSystemInfo fsinfo)
        {
            Path = fsinfo != null ? fsinfo.FullName : null;
        }

        public FilePathInfo(Uri uri)
        {
            Path = uri.LocalPath;
        }

        #endregion

        #region data

        public string Path { get; }

        public override int GetHashCode()
        {
            return Path.GetHashCode(FilePathUtils.PathComparisonMode);
        }

        public override bool Equals(object obj)
        {
            return obj is FilePathInfo other && Equals(other);
        }

        public bool Equals(FilePathInfo other)
        {
            return string.Equals(this.Path,other.Path,FilePathUtils.PathComparisonMode);
        }

        public static bool operator ==(FilePathInfo left, FilePathInfo right) => left.Equals(right);
        public static bool operator !=(FilePathInfo left, FilePathInfo right) => !left.Equals(right);

        #endregion

        #region properties        

        public bool IsValid => Path != null && !FilePathUtils.HasInvalidPathChars(Path);

        /// <summary>
        /// Returns true if the path is fixed to a specific drive or UNC path. This method does no
        /// validation of the path (URIs will be returned as relative as a result).
        /// Returns false if the path specified is relative to the current drive or working directory.
        /// </summary>
        /// <remarks>
        /// Handles paths that use the alternate directory separator.  It is a frequent mistake to
        /// assume that rooted paths <see cref="Path.IsPathRooted(string)"/> are not relative.  This isn't the case.
        /// "C:a" is drive relative- meaning that it will be resolved against the current directory
        /// for C: (rooted, but relative). "C:\a" is rooted and not relative (the current directory
        /// will not be used to modify the path).
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Thrown if path is null.
        /// </exception>
        public bool IsFullyQualified => System.IO.Path.IsPathFullyQualified(Path);

        // Tests if the given path contains a root. A path is considered rooted
        // if it starts with a backslash ("\") or a valid drive letter and a colon (":").
        public bool IsRooted => System.IO.Path.IsPathRooted(Path);

        public bool EndsInDirectorySeparator
        {
            get
            {                
                #if NETSTANDARD
                // Backported from net6
                return !string.IsNullOrEmpty(Path) && FilePathUtils.IsDirectorySeparator(Path[Path.Length-1]);
                #else
                return System.IO.Path.EndsInDirectorySeparator(Path);
                #endif
            }            
        }

        public string Name => System.IO.Path.GetFileName(Path);

        public string NameNoExt => System.IO.Path.GetFileNameWithoutExtension(Name);

        public string Extension => TryGetCompositeExtension(1, out var ext) ? ext : string.Empty;

        #endregion

        #region API

        public static FilePathInfo operator +(FilePathInfo left, string right)
        {
            var path = (left.Path ?? string.Empty) + right;
            return new FilePathInfo(path);
        }

        public static FilePathInfo operator /(FilePathInfo left, string right)
        {
            var path = System.IO.Path.Combine((left.Path ?? string.Empty), right);
            return new FilePathInfo(path);
        }

        public static FilePathInfo operator /(FilePathInfo left, FilePathInfo right)
        {
            var path = System.IO.Path.Combine((left.Path ?? string.Empty), right.Path);
            return new FilePathInfo(path);
        }

        public FilePathInfo GetPathRelativeTo(FilePathInfo other)
        {
            var path = System.IO.Path.GetRelativePath(other.Path, this.Path);
            return new FilePathInfo(path);
        }

        public FilePathInfo GetFullPath()
        {
            var path = System.IO.Path.GetFullPath(Path);
            return new FilePathInfo(path);
        }

        public FilePathInfo GetDirectoryPath()
        {
            var path = System.IO.Path.GetDirectoryName(Path);
            return new FilePathInfo(path);
        }        

        public bool TryGetCompositeExtension(int numDots, out string extension)
        {
            return FilePathUtils.TryGetCompositedExtension(Path,numDots, out extension);
        }
        
        public System.IO.FileInfo ToFileInfo() { return new System.IO.FileInfo(Path); }

        public System.IO.DirectoryInfo ToDirectoryInfo() { return new System.IO.DirectoryInfo(Path); }

        public string ToQuotedString() { return "\"" + Path + "\""; }

        #endregion
    }
}
