using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.IO
{
    partial class PhysicalDirectoryInfo
    {
        public static bool TryBrowseFolderDialog(out PhysicalDirectoryInfo xinfo, Environment.SpecialFolder? rootFolder, string initialDir, Guid? clientId)
        {
            if (_SystemDialogs.TryBrowseFolderDialog(out var dinfo, rootFolder, initialDir, clientId))
            {
                xinfo = new PhysicalDirectoryInfo(dinfo);
                return true;
            }

            xinfo = null;
            return false;
        }

        #if WINDOWS
        public static bool TryBrowseFolderDialog(out PhysicalDirectoryInfo xinfo, Action<System.Windows.Forms.FolderBrowserDialog> configureDialog, System.Windows.Forms.IWin32Window parent = null)
        {
            if (_SystemDialogs.TryBrowseFolderDialog(out var dinfo, configureDialog, parent))
            {
                xinfo = new PhysicalDirectoryInfo(dinfo);
                return true;
            }
            
            xinfo = null;
            return false;
        }
        #endif
    }
}
