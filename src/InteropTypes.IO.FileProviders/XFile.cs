using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.FileProviders;



namespace InteropTypes.IO
{
    /// <summary>
    /// Equivalent to <see cref="System.IO.File"/>
    /// </summary>
    public static partial class XFile
    {
        public static IFileInfo UseFile(IFileInfo xdir, string fileName)
        {
            GuardIsValidDirectory(xdir);

            if (xdir is PhysicalDirectoryInfo pdir)
            {
                return pdir.UseFile(fileName);
            }            

            throw new NotSupportedException();
        }

        public static bool TryGetFileInfo(IFileInfo xinfo, out FileInfo fileInfo)
        {
            fileInfo = null;
            if (xinfo == null) return false;
            if (xinfo.IsDirectory) return false;

            if (_IsMSPhysicalFile(xinfo))
            {
                fileInfo = new System.IO.FileInfo(xinfo.PhysicalPath);
                return true;
            }

            if (xinfo is IServiceProvider src)
            {
                if (src.GetService(typeof(FileInfo)) is FileInfo finfo)
                {
                    fileInfo = finfo; return true;
                }
            }

            return false;
        }

        public static bool TryGetDirectoryInfo(IFileInfo xinfo, out DirectoryInfo directoryInfo)
        {
            directoryInfo = null;
            if (xinfo == null) return false;
            if (!xinfo.IsDirectory) return false;

            if (_IsMSPhysicalDirectory(xinfo))
            {
                directoryInfo = new System.IO.DirectoryInfo(xinfo.PhysicalPath);
                return true;
            }

            if (xinfo is IServiceProvider src)
            {
                if (src.GetService(typeof(DirectoryInfo)) is DirectoryInfo dinfo)
                {
                    directoryInfo = dinfo; return true;
                }
            }

            return false;
        }

        internal static bool _IsMSPhysicalFile(IFileInfo xinfo)
        {
            if (xinfo == null) return false;
            if (xinfo.IsDirectory) return false;
            return xinfo.GetType().FullName == "Microsoft.Extensions.FileProviders.Physical.PhysicalFileInfo";
        }

        internal static bool _IsMSPhysicalDirectory(IFileInfo xinfo)
        {
            if (xinfo == null) return false;
            if (!xinfo.IsDirectory) return false;
            return xinfo.GetType().FullName == "Microsoft.Extensions.FileProviders.Physical.PhysicalDirectoryInfo";
        }

        

        public static IEnumerable<IFileInfo> EnumerateFiles(IFileInfo dinfo, SearchOption searchOption)
        {
            return _XFileEnumerator._EnumerateFiles(dinfo, searchOption == SearchOption.AllDirectories)
                ?? Array.Empty<IFileInfo>();
        }

        public static IEnumerable<IFileInfo> EnumerateFiles(IDirectoryContents dinfo, SearchOption searchOption)
        {
            return _XFileEnumerator._EnumerateFiles(dinfo, searchOption == SearchOption.AllDirectories)
                ?? Array.Empty<IFileInfo>();
        }
    }

    static class _XFileEnumerator
    {
        public static IEnumerable<IFileInfo> _EnumerateFiles(IEnumerable<IFileInfo> entries, bool allDirectories)
        {
            if (entries == null) return null;

            if (entries is IDirectoryContents dc && !dc.Exists) return null;

            IEnumerable<IFileInfo> outEntries = null;

            if (allDirectories)
            {
                foreach (var entry in entries.Where(item => item.IsDirectory))
                {
                    outEntries = _Concat(outEntries, _EnumerateFiles(entry, allDirectories));
                }
            }

            return _Concat(outEntries, entries.Where(item => !item.IsDirectory));
        }

        public static IEnumerable<IFileInfo> _EnumerateFiles(IFileInfo entry, bool allDirectories)
        {
            if (entry == null) return null;
            if (!entry.IsDirectory) return null;

            // this should be enough, but it's not...
            var contents = entry as IEnumerable<IFileInfo>;

            // some implementations might use this...
            if (contents == null && entry is IFileProvider fProvider)
            {
                contents = fProvider.GetDirectoryContents(string.Empty);
            }

            // hack for Microsoft PhysicalDirectoryInfo not implementing IDirectoryContents
            if (contents == null && XFile._IsMSPhysicalDirectory(entry))
            {
                // https://github.com/dotnet/runtime/issues/86354

                var dinfo = new System.IO.DirectoryInfo(entry.PhysicalPath);

                var zprovider = new PhysicalDirectoryInfo(dinfo);
                contents = zprovider;
            }

            return _EnumerateFiles(contents, allDirectories);            
        }        

        private static IEnumerable<IFileInfo> _Concat(IEnumerable<IFileInfo> a, IEnumerable<IFileInfo> b)
        {
            if (a == null) return b;
            if (b == null) return a;
            return a.Concat(b);
        }        
    }
}
