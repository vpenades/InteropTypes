using System;
using System.IO;

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
        
        /// <summary>
        /// Use <see cref="Create(DirectoryInfo)"/> public API.
        /// </summary>
        /// <param name="root">root directory.</param>
        private PhysicalFileProvider(System.IO.DirectoryInfo root)
        {
            System.Diagnostics.Debug.Assert(root != null);
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
        
        public IFileInfo GetFileInfo(string subpath)
        {
            return _GetFileInfo(subpath, false);
        }
        
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return _GetFileInfo(subpath, true) is IDirectoryContents dir && dir.Exists
                ? dir
                : NotFoundDirectoryContents.Singleton;
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        #endregion

        #region API factory

        private IFileInfo _GetFileInfo(string subpath, bool isDirectory)
        {
            subpath ??= string.Empty;

            if (FilePathUtils.HasInvalidNameChars(subpath)) throw new ArgumentException("Invalid characters", nameof(subpath));            

            if (!isDirectory && string.IsNullOrEmpty(subpath)) return new NotFoundFileInfo(subpath);

            // Absolute paths not permitted.
            if (subpath.Length > 0 && Path.IsPathRooted(subpath)) return new NotFoundFileInfo(subpath);

            // Relative paths starting with leading slashes are okay
            subpath = subpath.TrimStart(_pathSeparators);            

            string fullPath = GetFullPath(subpath);
            if (fullPath == null) return new NotFoundFileInfo(subpath);

            if (isDirectory)
            {
                var dinfo = new DirectoryInfo(fullPath);
                if (dinfo.Exists) return CreateDirectoryInfo(dinfo);
            }
            else
            {
                var finfo = new FileInfo(fullPath);
                if (finfo.Exists) return CreateFileInfo(finfo);
            }

            return new NotFoundFileInfo(subpath);
        }

        private string GetFullPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return Root.FullName;

            if (FilePathUtils.PathNavigatesAboveRoot(path)) { return null; }

            try
            {
                var fullPath = Path.GetFullPath(Path.Combine(Root.FullName, path));

                return IsUnderneathRoot(fullPath) ? fullPath : null;
            }
            catch { return null; }
        }

        private bool IsUnderneathRoot(string fullPath)
        {
            return fullPath.StartsWith(Root.FullName, StringComparison.OrdinalIgnoreCase);
        }

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