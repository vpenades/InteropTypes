using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    #if !NETSTANDARD
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)]
    #endif
    [System.Diagnostics.DebuggerDisplay("{_Dinfo.FullName,nq}")]
    public readonly struct DirectoryInfoServices : IServiceProvider
    {
        #region constructors        

        public DirectoryInfoServices(string filePath, IPhysicalFileFactory factory)
        {
            _Dinfo = new System.IO.DirectoryInfo(filePath);
            _Factory = factory;
        }

        public DirectoryInfoServices(DirectoryInfo dinfo, IPhysicalFileFactory factory)
        {
            _Dinfo = dinfo;
            _Factory = factory;
        }

        #endregion

        #region data

        private readonly System.IO.DirectoryInfo _Dinfo;
        private readonly IPhysicalFileFactory _Factory;

        #endregion

        #region API

        public object GetService(Type serviceType)
        {
            // service used to create files and directories
            if (serviceType == typeof(Func<string, IFileInfo>)) return (Func<string, IFileInfo>)UseFileInfo;

            if (serviceType == typeof(DirectoryInfo)) return _Dinfo;

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
            name = System.IO.Path.Combine(_Dinfo.FullName, name);

            var file = new System.IO.FileInfo(name);
            return _Factory
                ?.CreateFileInfo(file)
                ?? new PhysicalFileInfo(file);
        }

        internal IFileInfo UseDirectory(string name)
        {
            name = System.IO.Path.Combine(_Dinfo.FullName, name);

            var dir = new System.IO.DirectoryInfo(name);
            return _Factory
                ?.CreateDirectoryInfo(dir)
                ?? new PhysicalDirectoryInfo(dir);
        }

        #endregion
    }
}
