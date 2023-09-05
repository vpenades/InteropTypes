using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.IO
{
    partial class PhysicalDirectoryInfo
    {
        public static bool TryBrowseFolderDialog(out PhysicalDirectoryInfo xinfo, Environment.SpecialFolder? rootFolder, string initialDir, Guid? clientId, IntPtr parentHandle = default)
        {
            if (XShell.TryBrowseFolderDialog(out var dinfo, rootFolder, initialDir, clientId, parentHandle))
            {
                xinfo = new PhysicalDirectoryInfo(dinfo);
                return true;
            }

            xinfo = default;
            return false;
        }

        #if WINDOWS
        public static bool TryBrowseFolderDialog(out PhysicalDirectoryInfo xinfo, Action<System.Windows.Forms.FolderBrowserDialog> configureDialog, System.Windows.Forms.IWin32Window parent = null)
        {
            if (XShell.TryBrowseFolderDialog(out var dinfo, configureDialog, parent))
            {
                xinfo = new PhysicalDirectoryInfo(dinfo);
                return true;
            }
            
            xinfo = default;
            return false;
        }
        #endif
    }
}
