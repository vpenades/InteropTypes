using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InteropTypes.IO.FileProviders;
using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    /// <summary>
    /// Represents a file on a physical filesystem
    /// </summary>
    public class PhysicalFileInfo
        : IFileInfo
        , IEquatable<IFileInfo>
        , IServiceProvider
        // IProgress<Stream>, IProgress<ReadOnlySpan<Byte>> for writing
    {
        #region lifecycle

        public static IEnumerable<PhysicalFileInfo> Enumerate(string path)
        {
            return new PhysicalFileProvider(path).EnumerateFiles();
        }

        public static IEnumerable<PhysicalFileInfo> Enumerate(string path, string searchPattern)
        {
            return new PhysicalFileProvider(path).EnumerateFiles(searchPattern);
        }

        public static IEnumerable<PhysicalFileInfo> Enumerate(string path, string searchPattern, SearchOption options)
        {
            return new PhysicalFileProvider(path).EnumerateFiles(searchPattern, options);
        }

        /// <summary>
        /// Initializes an instance of <see cref="PhysicalFileInfo"/> that wraps an instance of <see cref="System.IO.FileInfo"/>
        /// </summary>
        /// <param name="info">The <see cref="System.IO.FileInfo"/></param>
        public PhysicalFileInfo(FileInfo info, PhysicalFileProvider parent = null)
        {
            File = info;
        }

        #endregion

        #region data

        protected readonly FileInfo File;        

        public override int GetHashCode()
        {
            return File.FullName.ToLower().GetHashCode();
        }

        public bool Equals(IFileInfo obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;

            return obj is PhysicalFileInfo other && PathUtils.IsSameResource(this.File, other.File);
        }

        #endregion

        #region properties

        /// <inheritdoc />
        public bool Exists => File.Exists;

        /// <inheritdoc />
        public long Length => File.Length;

        /// <inheritdoc />
        public string PhysicalPath => File.FullName;

        /// <inheritdoc />
        public string Name => File.Name;

        /// <inheritdoc />
        public DateTimeOffset LastModified => File.LastWriteTimeUtc;

        /// <summary>
        /// Always false.
        /// </summary>
        public bool IsDirectory => false;

        #endregion

        #region API

        /// <inheritdoc />
        public Stream CreateReadStream()
        {
            // We are setting buffer size to 1 to prevent FileStream from allocating it's internal buffer
            // 0 causes constructor to throw
            int bufferSize = 1;
            return new FileStream(
                PhysicalPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite,
                bufferSize,
                FileOptions.Asynchronous | FileOptions.SequentialScan);
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(FileInfo)) return File;
            if (serviceType == typeof(FileSystemInfo)) return File;
            if (serviceType == typeof(ArchiveFileProvider)) return ArchiveFileProvider.Create(File.FullName);

            return null;
        }        

        #endregion
    }
}