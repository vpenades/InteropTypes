using System;
using System.IO;

using Microsoft.Extensions.FileProviders;

using XFILEINFO = Microsoft.Extensions.FileProviders.IFileInfo;

namespace InteropTypes.IO
{
    /// <summary>
    /// Equivalent to <see cref="System.IO.File"/>
    /// </summary>
    public static partial class XFile
    {
        public static bool TryUseFile(XFILEINFO xdir, string fileName, out XFILEINFO fileInfo)
        {
            GuardIsValidDirectory(xdir);

            if (xdir is PhysicalDirectoryInfo pdir)
            {
                fileInfo = pdir.UseFile(fileName);
                return true;
            }

            if (TryGetDirectoryInfo(xdir, out var dinfo))
            {
                pdir = new PhysicalDirectoryInfo(dinfo);
                fileInfo = pdir.UseFile(fileName);
                return true;
            }            

            fileInfo = default;
            return false;
        }

        public static bool TryUseDirectory(XFILEINFO xdir, string fileName, out XFILEINFO dirInfo)
        {
            GuardIsValidDirectory(xdir);

            if (xdir is PhysicalDirectoryInfo pdir)
            {
                dirInfo = pdir.UseDirectory(fileName);
                return true;
            }

            if (TryGetDirectoryInfo(xdir, out var dinfo))
            {
                pdir = new PhysicalDirectoryInfo(dinfo);
                dirInfo = pdir.UseDirectory(fileName);
                return true;
            }

            dirInfo = default;
            return false;
        }

        public static bool TryGetFileInfo(XFILEINFO xinfo, out FileInfo fileInfo)
        {
            fileInfo = null;
            if (xinfo == null) return false;
            if (xinfo.IsDirectory) return false;

            if (xinfo is IServiceProvider src)
            {
                if (src.GetService(typeof(FileInfo)) is FileInfo finfo)
                {
                    fileInfo = finfo; return true;
                }
            }            

            try
            {
                fileInfo = new System.IO.FileInfo(xinfo.PhysicalPath);
                return true;
            }
            catch
            {
                fileInfo = null;
                return false;
            }            
        }

        public static bool TryGetDirectoryInfo(XFILEINFO xinfo, out DirectoryInfo directoryInfo)
        {
            directoryInfo = null;
            if (xinfo == null) return false;            
            if (!xinfo.IsDirectory) return false;

            // We use IServiceProvider to try get the internal DirectoryInfo object
            // to prevent having multiple instances.
            if (xinfo is IServiceProvider src)
            {
                if (src.GetService(typeof(DirectoryInfo)) is DirectoryInfo dinfo)
                {
                    directoryInfo = dinfo;
                    return true;
                }
            }

            try // access PhysicalPath to derive a parent directory.
            {
                if (!string.IsNullOrEmpty(xinfo.PhysicalPath))
                {
                    directoryInfo = new System.IO.DirectoryInfo(xinfo.PhysicalPath);
                    if (directoryInfo.Exists) return true;
                }                
            }
            catch (Exception ex)
            when (ex is ArgumentException || ex is System.Security.SecurityException)
            { }

            directoryInfo = null;
            return false;
        }

        public static bool TryCastToDirectoryContents(XFILEINFO entry, out IDirectoryContents contents)
        {
            contents = NotFoundDirectoryContents.Singleton;

            if (entry == null) return false;
            if (!entry.Exists) return false;
            if (!entry.IsDirectory) return false;

            switch (entry)
            {
                case IDirectoryContents dcontents: { contents = dcontents; return true; }
                case IFileProvider fprovider: { contents = fprovider.GetDirectoryContents(string.Empty); return true; }
            }

            // https://github.com/dotnet/runtime/issues/86354

            if (TryGetDirectoryInfo(entry, out var dinfo))
            {
                contents = new PhysicalDirectoryInfo(dinfo);
                return true;
            }

            return false;
        }        

        public static bool TryGetParentContainer(XFILEINFO entry, out IDirectoryContents parent)
        {
            parent = NotFoundDirectoryContents.Singleton;

            switch(entry)
            {
                case null: return false;
                case LinkedFileInfo linkedFileInfo:
                    parent = linkedFileInfo.Parent ?? NotFoundDirectoryContents.Singleton;
                    return true;
                case IServiceProvider srv:
                    {
                        parent = srv.GetService(typeof(IDirectoryContents)) as IDirectoryContents;

                        // there's the risk that "parent" is actually the same instance as "entry",
                        // so we have to check for that.

                        if (parent != null && object.ReferenceEquals(parent, entry)) return true;

                        parent = null;
                        break;
                    }
            }

            if (TryGetDirectoryInfo(entry, out var dinfo))
            {
                parent = new PhysicalDirectoryInfo(dinfo);
                return true;
            }

            return false;
        }
    }    
}
