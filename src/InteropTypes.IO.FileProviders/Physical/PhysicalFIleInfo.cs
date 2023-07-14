using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    /// <summary>
    /// Represents a file on a physical filesystem
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{PhysicalPath}")]
    #if !NETSTANDARD
    // this is required to prevent CreateWriteStream and IServiceProvider methods from being trimmed
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)]
    #endif
    public partial class PhysicalFileInfo
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

        internal protected FileInfo File { get; }

        public override int GetHashCode()
        {
            return FileSystemInfoComparer<FileInfo>.Default.GetHashCode(File);
        }

        public bool Equals(IFileInfo obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;

            return obj is PhysicalFileInfo other && FileSystemInfoComparer<FileInfo>.Default.Equals(this.File, other.File);
        }

        #endregion

        #region properties

        /// <summary>
        /// Always false.
        /// </summary>
        public bool IsDirectory => false;

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

        #endregion

        #region API

        /// <inheritdoc />
        public Stream CreateReadStream()
        {
            return File?.OpenRead();
        }
        
        public Stream CreateWriteStream()
        {
            File?.Directory?.Create();
            return File?.Create();
        }

        public virtual object GetService(Type serviceType)
        {            
            if (serviceType == typeof(Func<FileMode, Stream>)) return (Func<FileMode, Stream>)_Open1;
            if (serviceType == typeof(Func<FileMode, FileAccess, Stream>)) return (Func<FileMode, FileAccess, Stream>)_Open2;
            if (serviceType == typeof(Func<FileMode, FileAccess, FileShare, Stream>)) return (Func<FileMode, FileAccess, FileShare, Stream>)_Open3;

            if (serviceType == typeof(FileInfo)) return File;
            if (serviceType == typeof(FileSystemInfo)) return File;
            return null;
        }        

        private Stream _Open1(FileMode mode)
        {
            _EnsureDirectory(mode);
            return File?.Open(mode);
        }        

        private Stream _Open2(FileMode mode, FileAccess access)
        {
            _EnsureDirectory(mode);
            return File?.Open(mode, access);
        }

        private Stream _Open3(FileMode mode, FileAccess access, FileShare share)
        {
            _EnsureDirectory(mode);
            return File?.Open(mode, access, share);
        }

        private void _EnsureDirectory(FileMode mode)
        {
            if (mode == FileMode.Create || mode == FileMode.CreateNew || mode == FileMode.OpenOrCreate)
            {
                File?.Directory?.Create();
            }
        }

        #endregion
    }
}