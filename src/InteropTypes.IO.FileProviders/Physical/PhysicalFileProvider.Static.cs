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
        private static readonly ConcurrentDictionary<string, PhysicalFileProvider> _Providers = new ConcurrentDictionary<string, PhysicalFileProvider>();

        public static PhysicalFileProvider Create(System.IO.DirectoryInfo dinfo)
        {
            if (dinfo == null) throw new ArgumentNullException(nameof(dinfo));

            // when creating file providers, if the path matches a root drive, used the cached versions instead.

            if (dinfo.Root.FullName == dinfo.FullName) return UseRootProvider(dinfo);

            return new PhysicalFileProvider(dinfo);
        }

        public static IEnumerable<PhysicalFileProvider> GetDriveProviders()
        {
            return System.Environment.GetLogicalDrives()
                .Select(drive => new System.IO.DriveInfo(drive))
                .Select(UseRootProvider);
        }

        public static PhysicalFileProvider UseRootProvider(System.IO.DriveInfo dinfo)
        {
            if (dinfo == null) throw new ArgumentNullException(nameof(dinfo));

            return UseRootProvider(dinfo.RootDirectory);
        }

        internal static PhysicalFileProvider UseRootProvider(System.IO.DirectoryInfo dinfo)
        {
            if (dinfo == null) return null;

            dinfo = dinfo.Root;

            // use _Providers.AddOrUpdate for non fixed drives
            var drive = new DriveInfo(dinfo.Root.FullName);

            var provider = _Providers.GetOrAdd(dinfo.FullName, root => new PhysicalFileProvider(new System.IO.DirectoryInfo(root)));            

            // TODO: for external drives, the media might have changed, in which case we need to reset/refresh the provider

            return provider;
        }        

        
    }
}
