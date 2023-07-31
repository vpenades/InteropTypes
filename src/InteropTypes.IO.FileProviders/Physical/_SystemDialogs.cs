﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.IO
{
    internal static class _SystemDialogs
    {
        public static bool TryBrowseFolderDialog(out System.IO.DirectoryInfo xinfo, Environment.SpecialFolder? rootFolder, string initialDir, Guid? clientId)
        {
            xinfo = null;



            #if !WINDOWS

            return false;

            #else

            void configure(System.Windows.Forms.FolderBrowserDialog dlg)
            {
                dlg.ShowNewFolderButton = true;

                if (rootFolder.HasValue) dlg.RootFolder = rootFolder.Value;
                dlg.InitialDirectory = initialDir;
                if (clientId.HasValue) dlg.ClientGuid = clientId.Value;
            }

            return TryBrowseFolderDialog(out xinfo, configure);

            #endif
        }

        #if WINDOWS

        public static bool TryBrowseFolderDialog(out System.IO.DirectoryInfo dinfo, Action<System.Windows.Forms.FolderBrowserDialog> configureDialog, System.Windows.Forms.IWin32Window parent = null)
        {
            dinfo = null;            

            using(var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                configureDialog(dlg);

                var r = parent == null
                    ? dlg.ShowDialog()
                    : dlg.ShowDialog(parent);

                if (r != System.Windows.Forms.DialogResult.OK) return false;

                dinfo = new System.IO.DirectoryInfo(dlg.SelectedPath);                

                return true;
            }            
        }

        #endif
    }
}
