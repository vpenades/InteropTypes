using System;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    partial class XFile
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static void GuardIsValidFile<T>(T xinfo) where T: System.IO.FileSystemInfo
        {
            switch(xinfo)
            {
                case null: throw new ArgumentNullException(nameof(xinfo));
                case System.IO.FileInfo finfo: return;
                default: throw new ArgumentException($"Expected a File, but it's {xinfo.GetType()}", nameof(xinfo));
            }            
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void GuardIsValidDirectory<T>(T xinfo) where T : System.IO.FileSystemInfo
        {
            switch (xinfo)
            {
                case null: throw new ArgumentNullException(nameof(xinfo));
                case System.IO.DirectoryInfo dinfo: return;
                default: throw new ArgumentException($"Expected a Directory, but it's {xinfo.GetType()}", nameof(xinfo));
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void GuardIsValidFile(IFileInfo xinfo)
        {
            if (xinfo == null) throw new ArgumentNullException(nameof(xinfo));
            if (xinfo.IsDirectory) throw new ArgumentException("Expected a File, but it's a Directory", nameof(xinfo));
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void GuardIsValidDirectory(IFileInfo xinfo)
        {
            if (xinfo == null) throw new ArgumentNullException(nameof(xinfo));
            if (!xinfo.IsDirectory) throw new ArgumentException("Expected a Directory, but it's a File", nameof(xinfo));
        }
    }
}
