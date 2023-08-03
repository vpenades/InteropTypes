using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    #if !NETSTANDARD
    // this is required to prevent CreateWriteStream and IServiceProvider methods from being trimmed
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)]
    #endif
    public abstract class PhysicalSystemInfo : IFileInfo, IServiceProvider
    {
        /// <summary>
        /// gets the underlaying system object.
        /// </summary>
        /// <returns>a <see cref="FileSystemInfo"/> object.</returns>
        protected abstract FileSystemInfo GetSystemInfo();        

        public bool Exists => GetSystemInfo().Exists;
        public string PhysicalPath => GetSystemInfo().FullName;
        public string Name => GetSystemInfo().Name;
        public DateTimeOffset LastModified => GetSystemInfo().LastWriteTime;

        public abstract long Length { get; }
        public abstract bool IsDirectory { get; }
        public abstract Stream CreateReadStream();

        public virtual object GetService(Type serviceType)
        {            
            if (serviceType == typeof(FileSystemInfo)) return GetSystemInfo();
            return null;
        }
    }
}
