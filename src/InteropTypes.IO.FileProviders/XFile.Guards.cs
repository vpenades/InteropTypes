using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    partial class XFile
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static void GuardIsValidFile(IFileInfo xinfo)
        {
            if (xinfo == null) throw new ArgumentNullException(nameof(xinfo));
            if (xinfo.IsDirectory) throw new ArgumentException("not a file", nameof(xinfo));
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void GuardIsValidDirectory(IFileInfo xinfo)
        {
            if (xinfo == null) throw new ArgumentNullException(nameof(xinfo));
            if (!xinfo.IsDirectory) throw new ArgumentException("not a directory", nameof(xinfo));
        }
    }
}
