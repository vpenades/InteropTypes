using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    /// <summary>
    /// Represents a file on a physical filesystem
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("🗎 {PhysicalPath}")]
    #if !NETSTANDARD
    // this is required to prevent CreateWriteStream and IServiceProvider methods from being trimmed
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)]
    #endif
    public partial class PhysicalFileInfo
        : PhysicalSystemInfo
        , IFileInfoEx
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
            _File = info;

            // if we need to create PhysicalFileProviders, we can have one static provider per DRIVE, which is very few!!!
        }

        #endregion

        #region data

        private readonly FileInfo _File;        

        public override int GetHashCode()
        {
            return FileSystemInfoComparer<FileInfo>.Default.GetHashCode(File);
        }

        public override bool Equals(object obj)
        {
            return obj is IFileInfo other && Equals(other);
        }

        public bool Equals(IFileInfo other)
        {
            if (object.ReferenceEquals(this, other)) return true;

            if (XFile.TryGetFileInfo(other, out var finfo))
            {
                return FileSystemInfoComparer<FileInfo>.Default.Equals(this.File, finfo);
            }

            return false;
        }

        protected sealed override FileSystemInfo GetSystemInfo() { return File; }

        #endregion

        #region properties

        internal FileInfo File => _File;

        /// <summary>
        /// Always false.
        /// </summary>
        public sealed override bool IsDirectory => false;
        
        public sealed override long Length => File.Exists? File.Length : -1;        

        #endregion

        #region API
        
        public sealed override Stream CreateReadStream()
        {
            return File?.OpenRead();
        }
        
        public Stream CreateWriteStream()
        {
            return CreateStream(FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        }

        public Stream CreateStream(FileMode mode, FileAccess access, FileShare share)
        {            
            return new FileInfoServices(File).CreateStream(mode, access, share);
        }

        public override object GetService(Type serviceType)
        {   
            return new FileInfoServices(File)
                .GetService(serviceType)
                ?? base.GetService(serviceType);
        }
        
        

        #endregion
    }
}