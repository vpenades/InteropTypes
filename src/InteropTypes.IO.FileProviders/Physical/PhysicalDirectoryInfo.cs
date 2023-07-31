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

        internal protected DirectoryInfo Directory { get; }

        internal readonly PhysicalFileProvider Parent;

        public override int GetHashCode()
        {
            return FileSystemInfoComparer<DirectoryInfo>.Default.GetHashCode(Directory);
        }

        public bool Equals(IFileInfo obj)
        {
            if (object.ReferenceEquals(this, obj)) return true;

            return obj is PhysicalDirectoryInfo other && FileSystemInfoComparer<DirectoryInfo>.Default.Equals(this.Directory, other.Directory);
        }

        #endregion

        #region properties

        /// <summary>
        /// Always true.
        /// </summary>
        public bool IsDirectory => true;

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
                    #if !NETSTANDARD
                    .Where(item => item.LinkTarget == null) // Skip Links (unless we have a better option)
                    #endif
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

        public virtual object GetService(Type serviceType)
        {
            // service used to create files and directories
            if (serviceType == typeof(Func<string, IFileInfo>)) return (Func<string, IFileInfo>)UseFileInfo;

            if (serviceType == typeof(DirectoryInfo)) return Directory;
            if (serviceType == typeof(FileSystemInfo)) return Directory;

            return null;
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
            return Parent
                ?.CreateFileInfo(file)
                ?? new PhysicalFileInfo(file);
        }

        internal IFileInfo UseDirectory(string name)
        {
            name = System.IO.Path.Combine(Directory.FullName, name);

            var dir = new System.IO.DirectoryInfo(name);
            return Parent
                ?.CreateDirectoryInfo(dir)
                ?? new PhysicalDirectoryInfo(dir);
        }

        #endregion
    }
}