using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.IO
{
    partial struct PhysicalFIleInfo
    {
        public static async Task<PhysicalFileInfo> TryOpenFileDialogAsync(string filter, Guid? clientId)
        {
            #if WINDOWS

            void configure(System.Windows.Forms.OpenFileDialog dlg)
            {
                dlg.Multiselect = false;
                dlg.Filter = filter;
                dlg.RestoreDirectory = true;
                if (clientId.HasValue) dlg.ClientGuid = clientId.Value;
            }

            if (TryOpenFileDialog(out var xinfo, configure)) return xinfo;

            #endif

            #if ANDROID

            var options = Xamarin.Essentials.PickOptions.Default;

            var result = await Xamarin.Essentials.FilePicker.PickAsync(options).ConfigureAwait(false);

            if (result != null)
            {
                // actually it should wrap Xamarin.Essentials.FileBase

                var finfo = new System.IO.FileInfo(result.FullPath);

                return new PhysicalFileInfo(finfo);
            }

            #endif

            return null;
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
