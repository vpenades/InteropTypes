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
    public class PhysicalFileProvider : IFileProvider
    {
        #region constants

        private static readonly char[] _pathSeparators = new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        #endregion

        #region lifecycle

        /// <summary>
        /// Initializes a new instance of a PhysicalFileProvider at the given root directory.
        /// </summary>
        /// <param name="root">The root directory. This should be an absolute path.</param>
        /// <param name="filters">Specifies which files or directories are excluded.</param>
        public PhysicalFileProvider(string root)
        {
            if (!Path.IsPathRooted(root))
            {
                throw new ArgumentException("The path must be absolute.", nameof(root));
            }

            string fullRoot = Path.GetFullPath(root);

            // When we do matches in GetFullPath, we want to only match full directory names.
            fullRoot = PathUtils.EnsureTrailingSlash(fullRoot);

            Root = new System.IO.DirectoryInfo(fullRoot);

            if (!Root.Exists)
            {
                throw new DirectoryNotFoundException(fullRoot);
            }            
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
            catch (DirectoryNotFoundException)
            {
            }
            catch (IOException)
            {
            }
            return NotFoundDirectoryContents.Singleton;
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region API factory

        public virtual PhysicalFileInfo CreateFileInfo(FileInfo fileInfo)
        {
            return new PhysicalFileInfo(fileInfo, this);
        }

        public virtual PhysicalDirectoryInfo CreateDirectoryInfo(DirectoryInfo dirInfo)
        {
            return new PhysicalDirectoryInfo(dirInfo, this);
        }

        #endregion

        #region extras

        public IEnumerable<PhysicalFileInfo> EnumerateFiles()
        {
            return this
                .Root
                .EnumerateFiles()
                .Select(CreateFileInfo);
        }

        public IEnumerable<PhysicalFileInfo> EnumerateFiles(string searchPattern)
        {
            return this
                .Root
                .EnumerateFiles(searchPattern)
                .Select(CreateFileInfo);
        }

        public IEnumerable<PhysicalFileInfo> EnumerateFiles(string searchPattern, SearchOption options)
        {
            return this
                .Root
                .EnumerateFiles(searchPattern, options)
                .Select(CreateFileInfo);
        }

        public IEnumerable<PhysicalDirectoryInfo> EnumerateDirectories()
        {
            return this
                .Root
                .EnumerateDirectories()
                .Select(CreateDirectoryInfo);
        }

        public IEnumerable<PhysicalDirectoryInfo> EnumerateDirectories(string searchPattern)
        {
            return this
                .Root
                .EnumerateDirectories(searchPattern)
                .Select(CreateDirectoryInfo);
        }

        public IEnumerable<PhysicalDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption options)
        {
            return this
                .Root
                .EnumerateDirectories(searchPattern, options)
                .Select(CreateDirectoryInfo);
        }

        #endregion
    }
}