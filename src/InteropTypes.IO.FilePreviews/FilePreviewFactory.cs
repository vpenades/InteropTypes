using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics;

namespace InteropTypes.IO
{
    public static class FilePreviewFactory
    {
        [return: MaybeNull]
        public static async Task<WindowsBitmap> GetPreviewOrDefaultAsync([MaybeNull] System.IO.FileInfo finfo)
        {
            if (finfo == null || !finfo.Exists) return null;            

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return await Task.Run(() => GetPreviewOrDefault(finfo)).ConfigureAwait(true);
            }

            return null;
        }

        [return: MaybeNull]
        public static WindowsBitmap GetPreviewOrDefault([MaybeNull] System.IO.FileInfo finfo)
        {
            if (finfo == null || !finfo.Exists) return null;            

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return Platforms.Win32.FilePreview_Win32_COM.GetPreviewOrDefault(finfo);
            }

            return null;
        }        
    }
}
