using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    public static class AbstractFile
    {
        public static IEnumerable<IFileInfo> EnumerateFiles(IFileInfo dinfo, SearchOption searchOption)
        {
            return _GenericFileEnumerator.EnumerateContentFiles(dinfo, searchOption == SearchOption.AllDirectories);
        }

        public static IEnumerable<IFileInfo> EnumerateFiles(IDirectoryContents dinfo, SearchOption searchOption)
        {
            return _GenericFileEnumerator.EnumerateFiles(dinfo, searchOption == SearchOption.AllDirectories);
        }
    }

    static class _GenericFileEnumerator
    {
        public static IEnumerable<IFileInfo> EnumerateFiles(IDirectoryContents contents, bool allDirectories)
        {
            IEnumerable<IFileInfo> outEntries = null;

            if (allDirectories)
            {
                foreach (var entry in contents.Where(item => item.IsDirectory))
                {
                    outEntries = _Concat(outEntries, EnumerateContentFiles(entry, allDirectories));
                }
            }

            return _Concat(outEntries, contents.Where(item => !item.IsDirectory));
        }

        public static IEnumerable<IFileInfo> EnumerateContentFiles(IFileInfo entry, bool allDirectories)
        {
            if (!entry.IsDirectory) return null;

            if (entry is IDirectoryContents child) // this should be enough, but it's not
            {
                return EnumerateFiles(child, allDirectories);                
            }

            if (entry is IFileProvider fprovider) // but some implementations might use this
            {
                var xcontents = fprovider.GetDirectoryContents(string.Empty);
                return EnumerateFiles(xcontents, allDirectories);                
            }

            // hack for Microsoft PhysicalDirectoryInfo not implementing IDirectoryContents
            // https://github.com/dotnet/runtime/issues/86354
            if (entry.GetType().FullName == "Microsoft.Extensions.FileProviders.Physical.PhysicalDirectoryInfo")
            {
                var zprovider = new PhysicalFileProvider(entry.PhysicalPath);
                var zcontents = zprovider.GetDirectoryContents(string.Empty);
                return EnumerateFiles(zcontents, allDirectories);
                

                /*
                using (var xprovider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(entry.PhysicalPath))
                {
                    var xcotents = xprovider.GetDirectoryContents(string.Empty);
                    EnumerateFiles(xcontents);                    
                }*/
            }            

            return null;
        }        

        private static IEnumerable<IFileInfo> _Concat(IEnumerable<IFileInfo> a, IEnumerable<IFileInfo> b)
        {
            if (a == null) return b;
            if (b == null) return a;
            return a.Concat(b);
        }        
    }
}
