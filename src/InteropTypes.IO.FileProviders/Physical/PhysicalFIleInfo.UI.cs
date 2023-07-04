using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.IO
{
    partial class PhysicalFIleInfo
    {
        public static bool TryOpenFileDialog(out PhysicalFileInfo xinfo, string filter, Guid? clientId)
        {
            xinfo = null;

            #if !WINDOWS            

            return false;

            #else

            void configure(System.Windows.Forms.OpenFileDialog dlg)
            {
                dlg.Multiselect = false;
                dlg.Filter = filter;
                dlg.RestoreDirectory = true;
                if (clientId.HasValue) dlg.ClientGuid = clientId.Value;
            }

            return TryOpenFileDialog(out xinfo, configure);

            #endif
        }

        #if WINDOWS

        public static bool TryOpenFileDialog(out PhysicalFileInfo xinfo, Action<System.Windows.Forms.OpenFileDialog> configureDialog, System.Windows.Forms.IWin32Window parent = null)
        {
            xinfo = null;

            using(var dlg = new System.Windows.Forms.OpenFileDialog())
            {
                configureDialog(dlg);

                var r = parent == null
                    ? dlg.ShowDialog()
                    : dlg.ShowDialog(parent);

                if (r != System.Windows.Forms.DialogResult.OK) return false;

                var finfo = new System.IO.FileInfo(dlg.FileName);

                xinfo = new PhysicalFileInfo(finfo);

                return true;
            }            
        }

        public static bool TrySaveFileDialog(out PhysicalFileInfo xinfo, Action<System.Windows.Forms.SaveFileDialog> configureDialog, System.Windows.Forms.IWin32Window parent = null)
        {
            xinfo = null;

            using (var dlg = new System.Windows.Forms.SaveFileDialog())
            {
                configureDialog(dlg);

                var r = parent == null
                    ? dlg.ShowDialog()
                    : dlg.ShowDialog(parent);

                if (r != System.Windows.Forms.DialogResult.OK) return false;

                var finfo = new System.IO.FileInfo(dlg.FileName);

                xinfo = new PhysicalFileInfo(finfo);

                return true;
            }
        }

        #endif

    }
}
