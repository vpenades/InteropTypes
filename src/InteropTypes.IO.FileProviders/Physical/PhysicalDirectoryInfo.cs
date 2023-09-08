using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    /// <summary>
    /// Represents a directory on a physical filesystem
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("📁 {PhysicalPath}")]
    #if !NETSTANDARD
    // this is required to prevent CreateWriteStream and IServiceProvider methods from being trimmed
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)]
    #endif
    public partial class PhysicalDirectoryInfo
        : PhysicalSystemInfo        
        , IEquatable<IFileInfo>
        , IDirectoryContents
        , IServiceProvider
    {
        #region lifecycle

        public static IEnumerable<PhysicalDirectoryInfo> Enumerate(string path)
        {
            var dinfo = new System.IO.DirectoryInfo(path);

            return new PhysicalDirectoryInfo(dinfo).EnumerateDirectories();
        }

        public static IEnumerable<PhysicalDirectoryInfo> Enumerate(string path, string searchPattern)
        {
            var dinfo = new System.IO.DirectoryInfo(path);

            return new PhysicalDirectoryInfo(dinfo).EnumerateDirectories(searchPattern);
        }

        public static IEnumerable<PhysicalDirectoryInfo> Enumerate(string path, string searchPattern, SearchOption options)
        {
            var dinfo = new System.IO.DirectoryInfo(path);

            return new PhysicalDirectoryInfo(dinfo).EnumerateDirectories(searchPattern, options);
        }

        /// <summary>
        /// Initializes an instance of <see cref="PhysicalDirectoryInfo"/> that wraps an instance of <see cref="System.IO.DirectoryInfo"/>
        /// </summary>
        /// <param name="dinfo">The directory</param>
        public PhysicalDirectoryInfo(DirectoryInfo dinfo, IPhysicalFileFactory parent = null)
        {
            Directory = dinfo;
            _Factory = parent ?? PhysicalFileProvider.UseRootProvider(dinfo);
        }

        #endregion

        #region data

        internal DirectoryInfo Directory { get; }

        private readonly IPhysicalFileFactory _Factory;

        public override int GetHashCode()
        {
            return FileSystemInfoComparer<DirectoryInfo>.Default.GetHashCode(Directory);
        }

        public bool Equals(IFileInfo obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;

            return obj is PhysicalDirectoryInfo other && FileSystemInfoComparer<DirectoryInfo>.Default.Equals(this.Directory, other.Directory);
        }

        protected sealed override FileSystemInfo GetSystemInfo() { return Directory; }

        #endregion

        #region properties        

        /// <summary>
        /// Always true.
        /// </summary>
        public sealed override bool IsDirectory => true;

        /// <summary>
        /// Always -1
        /// </summary>
        public sealed override long Length => -1;        

        #endregion

        #region API

        /// <summary>
        /// Always throws an exception because read streams are not support on directories.
        /// </summary>
        /// <exception cref="NotSupportedException">Always thrown</exception>
        /// <returns>Never returns</returns>
        public sealed override Stream CreateReadStream()
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
                    .Select(_Factory.CreateInfo)
                    .Where(item => item != null);
            }
            catch (Exception ex)
            when (ex is DirectoryNotFoundException || ex is IOException || ex is UnauthorizedAccessException)
            {
                return Enumerable.Empty<IFileInfo>();
            }
        }

        public override object GetService(Type serviceType)
        {
            // service used to create files and directories
            if (serviceType == typeof(Func<string, IFileInfo>)) return (Func<string, IFileInfo>)UseFileInfo;

            if (serviceType == typeof(DirectoryInfo)) return Directory;

            return base.GetService(serviceType);
        }

        /// <summary>
        /// Uses or creates an existing file or directory
        /// </summary>
        /// <param name="name"><br/>
        /// the name of the file or directory.
        /// If it ends with '/' or '\' it is considered a directory.
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public IFileInfo UseFileInfo(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            bool isDirectory = false;

            if (_EndsWithSeparator(name))
            {
                isDirectory = true;
                name = name.Substring(0, name.Length - 1);
            }

            if (Path.GetInvalidFileNameChars().Any(name.Contains))
            {
                throw new ArgumentException("invalid name chars", nameof(name));
            }

            return isDirectory
                ? UseDirectory(name)
                : UseFile(name);
        }

        private static bool _EndsWithSeparator(string path)
        {            
            if (path.EndsWith(System.IO.Path.DirectorySeparatorChar)) return true;
            if (path.EndsWith(System.IO.Path.AltDirectorySeparatorChar)) return true;
            
            return false;
        }

        internal IFileInfo UseFile(string name)
        {
            name = System.IO.Path.Combine(Directory.FullName, name);

            var file = new System.IO.FileInfo(name);
            return _Factory
                ?.CreateFileInfo(file)
                ?? new PhysicalFileInfo(file);
        }

        internal IFileInfo UseDirectory(string name)
        {
            name = System.IO.Path.Combine(Directory.FullName, name);

            var dir = new System.IO.DirectoryInfo(name);
            return _Factory
                ?.CreateDirectoryInfo(dir)
                ?? new PhysicalDirectoryInfo(dir);
        }

        #endregion

        #region extras

        public IEnumerable<PhysicalFileInfo> EnumerateFiles()
        {
            return this
                .Directory
                .EnumerateFiles()
                .Select(_Factory.CreateFileInfo);
        }

        public IEnumerable<PhysicalFileInfo> EnumerateFiles(string searchPattern)
        {
            return this
                .Directory
                .EnumerateFiles(searchPattern)
                .Select(_Factory.CreateFileInfo);
        }

        public IEnumerable<PhysicalFileInfo> EnumerateFiles(string searchPattern, SearchOption options)
        {
            return this
                .Directory
                .EnumerateFiles(searchPattern, options)
                .Select(_Factory.CreateFileInfo);
        }

        public IEnumerable<PhysicalDirectoryInfo> EnumerateDirectories()
        {
            return this
                .Directory
                .EnumerateDirectories()
                .Select(_Factory.CreateDirectoryInfo);
        }

        public IEnumerable<PhysicalDirectoryInfo> EnumerateDirectories(string searchPattern)
        {
            return this
                .Directory
                .EnumerateDirectories(searchPattern)
                .Select(_Factory.CreateDirectoryInfo);
        }

        public IEnumerable<PhysicalDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption options)
        {
            return this
                .Directory
                .EnumerateDirectories(searchPattern, options)
                .Select(_Factory.CreateDirectoryInfo);
        }

        #endregion
    }
}