using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.IO
{
    partial class PhysicalFileProvider
    {
        static PhysicalFileProvider()
        {
            _Providers = new ConcurrentDictionary<string, PhysicalFileProvider>();
        }

        private static readonly ConcurrentDictionary<string, PhysicalFileProvider> _Providers;

        public static PhysicalFileProvider UseRootProvider(System.IO.DirectoryInfo dinfo)
        {
            if (dinfo == null) return null;

            dinfo = dinfo.Root;

            // use _Providers.AddOrUpdate for non fixed drives
            var drive = new DriveInfo(dinfo.Root.FullName);

            var provider = _Providers.GetOrAdd(dinfo.FullName, root => new PhysicalFileProvider(new System.IO.DirectoryInfo(root)));            

            // TODO: for external drives, the media might have changed, in which case we need to reset/refresh the provider

            return provider;
        }

        public static bool TryGetFileInfo(Microsoft.Extensions.FileProviders.IFileInfo src, out System.IO.FileInfo finfo)
        {
            if (src == null || src.IsDirectory)
            {
                finfo = null;
                return false;
            }

            try
            {
                if (src is IServiceProvider srv)
                {
                    finfo = srv.GetService(typeof(FileInfo)) as FileInfo;
                    if (finfo != null) return true;
                }

                finfo = new System.IO.FileInfo(src.PhysicalPath);
                return true;
            }
            catch
            {
                finfo = null;
                return false;
            }
        }

        public static bool TryGetDirectoryInfo(Microsoft.Extensions.FileProviders.IFileInfo src, out System.IO.DirectoryInfo finfo)
        {
            if (src == null || !src.IsDirectory)
            {
                finfo = null;
                return false;
            }

            try
            {
                if (src is IServiceProvider srv)
                {
                    finfo = srv.GetService(typeof(DirectoryInfo)) as DirectoryInfo;
                    if (finfo != null) return true;
                }

                finfo = new System.IO.DirectoryInfo(src.PhysicalPath);
                return true;
            }
            catch
            {
                finfo = null;
                return false;
            }
        }
    }
}
