using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.IO
{
    public static class ShellUtils
    {
        // use verb=open to reuse existing explorer instances
        // https://stackoverflow.com/questions/33826845/if-windows-explorer-is-open-at-a-specific-path-do-not-create-a-new-instance

        public static void StartFile(this System.IO.FileInfo finfo)
        {
            if (finfo == null || !finfo.Exists) return;

            var psi = new System.Diagnostics.ProcessStartInfo(finfo.FullName);
            psi.UseShellExecute = true;

            System.Diagnostics.Process.Start(psi)?.Dispose();
        }

        public static void StartUri(this Uri uri)
        {
            var psi = new System.Diagnostics.ProcessStartInfo(uri.OriginalString);
            psi.UseShellExecute = true;

            System.Diagnostics.Process.Start(psi)?.Dispose();
        }

        public static void ShowDirectoryInExplorer(this System.IO.DirectoryInfo dirInfo)
        {
            var psi = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = dirInfo.FullName,
                UseShellExecute = true,
                Verb = "open"
            };

            System.Diagnostics.Process.Start(psi)?.Dispose();
        }        

        public static void ShowFileInExplorer(this System.IO.FileInfo finfo)
        {
            if (finfo == null || !finfo.Exists) return;

            var psi = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = "explorer.exe",
                UseShellExecute = false,
                Arguments = "/select, " + '"' + finfo.FullName + '"'
            };

            System.Diagnostics.Process.Start(psi)?.Dispose();
        }
    }
}
