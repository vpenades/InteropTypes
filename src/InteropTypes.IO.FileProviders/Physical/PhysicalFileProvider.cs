using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using InteropTypes.IO.FileProviders;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace InteropTypes.IO
{
    /// <summary>
    /// Looks up files using the on-disk file system
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Root.FullPath}")]
    public partial class PhysicalFileProvider : IFileProvider , IPhysicalFileFactory
    {
        #region constants

        private static readonly char[] _pathSeparators = new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        #endregion

        #region lifecycle
        
        private PhysicalFileProvider(System.IO.DirectoryInfo root)
        {
            Root = root;
        }

        #endregion

        #region properties

        /// <summary>
        /// The root directory for this instance.
        /// </summary>
        public System.IO.DirectoryInfo Root { get; }

        #endregion

        #region API

        private string GetFullPath(string path)
        {
            if (PathUtils.PathNavigatesAboveRoot(path)) { return null; }

            string fullPath;

            try { fullPath = Path.GetFullPath(Path.Combine(Root.FullName, path)); }
            catch { return null; }

            if (!IsUnderneathRoot(fullPath))
            {
                return null;
            }

            return fullPath;
        }

        private bool IsUnderneathRoot(string fullPath)
        {
            return fullPath.StartsWith(Root.FullName, StringComparison.OrdinalIgnoreCase);
        }

        // <summary>
        /// Locate a file at the given path by directly mapping path segments to physical directories.
        /// </summary>
        /// <param name="subpath">A path under the root directory</param>
        /// <returns>The file information. Caller must check <see cref="IFileInfo.Exists"/> property. </returns>
        public IFileInfo GetFileInfo(string subpath)
        {
            if (string.IsNullOrEmpty(subpath) || PathUtils.HasInvalidPathChars(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            // Relative paths starting with leading slashes are okay
            subpath = subpath.TrimStart(_pathSeparators);

            // Absolute paths not permitted.
            if (Path.IsPathRooted(subpath)) return new NotFoundFileInfo(subpath);

            string fullPath = GetFullPath(subpath);
            if (fullPath == null) return new NotFoundFileInfo(subpath);

            var fileInfo = new FileInfo(fullPath);
            if (!fileInfo.Exists) return new NotFoundFileInfo(subpath);

            return CreateFileInfo(fileInfo);
        }

        /// <summary>
        /// Enumerate a directory at the given path, if any.
        /// </summary>
        /// <param name="subpath">A path under the root directory. Leading slashes are ignored.</param>
        /// <returns>
        /// Contents of the directory. Caller must check <see cref="IDirectoryContents.Exists"/> property. <see cref="NotFoundDirectoryContents" /> if
        /// <paramref name="subpath" /> is absolute, if the directory does not exist, or <paramref name="subpath" /> has invalid
        /// characters.
        /// </returns>
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            try
            {
                if (subpath == null || PathUtils.HasInvalidPathChars(subpath))
                {
                    return NotFoundDirectoryContents.Singleton;
                }

                // Relative paths starting with leading slashes are okay
                subpath = subpath.TrimStart(_pathSeparators);

                // Absolute paths not permitted.
                if (Path.IsPathRooted(subpath))
                {
                    return NotFoundDirectoryContents.Singleton;
                }

                string fullPath = GetFullPath(subpath);                

                if (fullPath == null || !Directory.Exists(fullPath))
                {
                    return NotFoundDirectoryContents.Singleton;
                }

                return CreateDirectoryInfo(new System.IO.DirectoryInfo(fullPath))
                    as IDirectoryContents
                    ?? NotFoundDirectoryContents.Singleton;
            }
            catch (Exception ex)
            when (ex is DirectoryNotFoundException || ex is IOException || ex is UnauthorizedAccessException)
            {
                return NotFoundDirectoryContents.Singleton;
            }            
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        #endregion

        #region API factory

        public virtual PhysicalSystemInfo CreateInfo(FileSystemInfo xinfo)
        {
            #if !NETSTANDARD
            if (xinfo.LinkTarget != null) return null; // Skip Links (unless we have a better option)
            #endif

            if (xinfo is FileInfo file) return CreateFileInfo(file);
            if (xinfo is DirectoryInfo dir) return CreateDirectoryInfo(dir);

            // shouldn't happen unless BCL introduces new implementation of base type
            throw new InvalidOperationException("Unsupported: " + xinfo.GetType().FullName);
        }

        public virtual PhysicalFileInfo CreateFileInfo(FileInfo fileInfo)
        {
            return new PhysicalFileInfo(fileInfo);
        }

        public virtual PhysicalDirectoryInfo CreateDirectoryInfo(DirectoryInfo dirInfo)
        {
            return new PhysicalDirectoryInfo(dirInfo, this);
        }

        #endregion        
    }
}