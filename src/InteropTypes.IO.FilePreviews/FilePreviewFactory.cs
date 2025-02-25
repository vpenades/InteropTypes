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
        [return: NotNull]
        public static WindowsBitmap GetPreviewOrDefault(System.IO.FileInfo finfo)
        {
            if (finfo == null || !finfo.Exists) return null;

            // if (!_FilterExtension(finfo.Extension)) return null;

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return Platforms.Win32.FilePreview_Win32_COM.GetPreviewOrDefault(finfo);
            }

            return null;
        }

        [return: NotNull]
        public static async Task<WindowsBitmap> GetPreviewOrDefaultAsync(System.IO.FileInfo finfo)
        {
            if (finfo == null || !finfo.Exists) return null;

            // if (!_FilterExtension(finfo.Extension)) return null;

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return await Task.Run(() => Platforms.Win32.FilePreview_Win32_COM.GetPreviewOrDefault(finfo)).ConfigureAwait(true);
            }

            return null;
        }

        private static bool _FilterExtension(string ext)
        {
            ext = ext.ToUpperInvariant();

            // positives
            if (ext.EndsWith(".JPG")) return true;
            if (ext.EndsWith(".PNG")) return true;
            if (ext.EndsWith(".GIF")) return true;
            if (ext.EndsWith(".BMP")) return true;
            if (ext.EndsWith(".WEBP")) return true;

            // negatives
            if (ext.EndsWith(".MD")) return false;
            if (ext.EndsWith(".TXT")) return false;
            if (ext.EndsWith(".PDF")) return false;
            if (ext.EndsWith(".DOC")) return false;
            if (ext.EndsWith(".DOCX")) return false;

            if (ext.EndsWith(".DLL")) return false;
            if (ext.EndsWith(".EXE")) return false;
            if (ext.EndsWith(".JSON")) return false;

            return true;
        }
    }
}
