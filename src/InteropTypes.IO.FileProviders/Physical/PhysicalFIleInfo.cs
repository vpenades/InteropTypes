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
        : PhysicalSystemInfo        
        , IEquatable<IFileInfo>
        // IProgress<Stream>, IProgress<ReadOnlySpan<Byte>> for writing
    {
        #region lifecycle

        public static IEnumerable<PhysicalFileInfo> Enumerate(string path)
        {
            var dinfo = new System.IO.DirectoryInfo(path);

            return new PhysicalDirectoryInfo(dinfo).EnumerateFiles();
        }

        public static IEnumerable<PhysicalFileInfo> Enumerate(string path, string searchPattern)
        {
            var dinfo = new System.IO.DirectoryInfo(path);

            return new PhysicalDirectoryInfo(dinfo).EnumerateFiles(searchPattern);
        }

        public static IEnumerable<PhysicalFileInfo> Enumerate(string path, string searchPattern, SearchOption options)
        {
            var dinfo = new System.IO.DirectoryInfo(path);

            return new PhysicalDirectoryInfo(dinfo).EnumerateFiles(searchPattern, options);
        }

        /// <summary>
        /// Initializes an instance of <see cref="PhysicalFileInfo"/> that wraps an instance of <see cref="System.IO.FileInfo"/>
        /// </summary>
        /// <param name="info">The <see cref="System.IO.FileInfo"/></param>
        public PhysicalFileInfo(FileInfo info)
        {
            File = info;

            // if we need to create PhysicalFileProviders, we can have one static provider per DRIVE, which is very few!!!
        }

        #endregion

        #region data
        
        internal FileInfo File { get; }

        public override int GetHashCode()
        {
            return FileSystemInfoComparer<FileInfo>.Default.GetHashCode(File);
        }

        public bool Equals(IFileInfo other)
        {
            if (object.ReferenceEquals(this, other)) return true;

            return other is PhysicalFileInfo pfinfo && FileSystemInfoComparer<FileInfo>.Default.Equals(this.File, pfinfo.File);
        }

        protected sealed override FileSystemInfo GetSystemInfo() { return File; }

        #endregion

        #region properties        

        /// <summary>
        /// Always false.
        /// </summary>
        public sealed override bool IsDirectory => false;
        
        public sealed override long Length => File.Exists? File.Length : -1;        

        #endregion

        #region API

        /// <inheritdoc />
        public sealed override Stream CreateReadStream()
        {
            return File?.OpenRead();
        }
        
        public Stream CreateWriteStream()
        {
            File?.Directory?.Create();
            return File?.Create();
        }

        public override object GetService(Type serviceType)
        {            
            if (serviceType == typeof(Func<FileMode, Stream>)) return (Func<FileMode, Stream>)_Open1;
            if (serviceType == typeof(Func<FileMode, FileAccess, Stream>)) return (Func<FileMode, FileAccess, Stream>)_Open2;
            if (serviceType == typeof(Func<FileMode, FileAccess, FileShare, Stream>)) return (Func<FileMode, FileAccess, FileShare, Stream>)_Open3;

            if (serviceType == typeof(FileInfo)) return File;

            return base.GetService(serviceType);
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