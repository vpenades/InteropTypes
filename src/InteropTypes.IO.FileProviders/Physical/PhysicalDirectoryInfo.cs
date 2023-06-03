using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InteropTypes.IO.FileProviders;
using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    /// <summary>
    /// Represents a directory on a physical filesystem
    /// </summary>
    public class PhysicalDirectoryInfo
        : IFileInfo
        , IEquatable<IFileInfo>
        , IDirectoryContents
        , IServiceProvider
    {
        #region lifecycle

        public static IEnumerable<PhysicalDirectoryInfo> Enumerate(string path)
        {
            return new PhysicalFileProvider(path).EnumerateDirectories();
        }

        public static IEnumerable<PhysicalDirectoryInfo> Enumerate(string path, string searchPattern)
        {
            return new PhysicalFileProvider(path).EnumerateDirectories(searchPattern);
        }

        public static IEnumerable<PhysicalDirectoryInfo> Enumerate(string path, string searchPattern, SearchOption options)
        {
            return new PhysicalFileProvider(path).EnumerateDirectories(searchPattern, options);
        }

        /// <summary>
        /// Initializes an instance of <see cref="PhysicalDirectoryInfo"/> that wraps an instance of <see cref="System.IO.DirectoryInfo"/>
        /// </summary>
        /// <param name="info">The directory</param>
        public PhysicalDirectoryInfo(DirectoryInfo info, PhysicalFileProvider parent = null)
        {
            Directory = info;
            Parent = parent;
        }

        #endregion

        #region data

        internal protected readonly DirectoryInfo Directory;

        internal readonly PhysicalFileProvider Parent;

        public override int GetHashCode()
        {
            return Directory.FullName.ToLower().GetHashCode();
        }

        public bool Equals(IFileInfo obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;

            return obj is PhysicalDirectoryInfo other && PathUtils.IsSameResource(this.Directory, other.Directory);
        }

        #endregion

        #region properties

        /// <inheritdoc />
        public bool Exists => Directory.Exists;

        /// <summary>
        /// Always equals -1.
        /// </summary>
        long IFileInfo.Length => -1;

        /// <inheritdoc />
        public string PhysicalPath => Directory.FullName;

        /// <inheritdoc />
        public string Name => Directory.Name;

        /// <summary>
        /// The time when the directory was last written to.
        /// </summary>
        public DateTimeOffset LastModified => Directory.LastWriteTimeUtc;

        /// <summary>
        /// Always true.
        /// </summary>
        public bool IsDirectory => true;

        #endregion

        #region API

        /// <summary>
        /// Always throws an exception because read streams are not support on directories.
        /// </summary>
        /// <exception cref="NotSupportedException">Always thrown</exception>
        /// <returns>Never returns</returns>
        Stream IFileInfo.CreateReadStream()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        IEnumerator<IFileInfo> IEnumerable<IFileInfo>.GetEnumerator()
        {
            return GetEntries()?.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEntries()?.GetEnumerator();
        }

        private IEnumerable<IFileInfo> GetEntries()
        {
            try
            {
                return Directory
                    .EnumerateFileSystemInfos()                    
                    .Select<FileSystemInfo, IFileInfo>(info =>
                    {
                        if (info is FileInfo file)
                        {
                            return Parent?.CreateFileInfo(file) ?? new PhysicalFileInfo(file);
                        }
                        else if (info is DirectoryInfo dir)
                        {
                            return Parent?.CreateDirectoryInfo(dir) ?? new PhysicalDirectoryInfo(dir);
                        }
                        // shouldn't happen unless BCL introduces new implementation of base type
                        throw new InvalidOperationException("Unsupported: " + info.GetType().FullName);
                    });
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException || ex is IOException)
            {
                return Enumerable.Empty<IFileInfo>();
            }
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(DirectoryInfo)) return Directory;
            if (serviceType == typeof(FileSystemInfo)) return Directory;
            return null;
        }

        #endregion
    }
}