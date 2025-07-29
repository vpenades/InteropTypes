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
        public static async Task<System.IO.Stream> GetStreamOrDefaultAsync([MaybeNull] System.IO.FileInfo finfo, FilePreviewOptions options = null)
        {
            if (finfo == null || !finfo.Exists) return null;

            #if WINDOWS10_0_19041_0_OR_GREATER
            return await Platforms.UWP.WindowsThumbnailService.GetThumbnailStreamAsync(finfo, options).ConfigureAwait(true);
            #endif

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return await Task.Run(() => GetStreamOrDefault(finfo, options)).ConfigureAwait(true);
            }

            return null;
        }

        [return: MaybeNull]
        public static System.IO.Stream GetStreamOrDefault([MaybeNull] System.IO.FileInfo finfo, FilePreviewOptions options = null)
        {
            if (finfo == null || !finfo.Exists) return null;

            #if WINDOWS10_0_19041_0_OR_GREATER

            return Platforms.UWP.WindowsThumbnailService.GetThumbnailStreamAsync(finfo, options).GetAwaiter().GetResult();

            #elif NET8_0_OR_GREATER

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return Platforms.Win32.FilePreview_GeneratedCOM.GetStreamOrDefault(finfo, options);
            }

            #elif NET6_0

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return Platforms.Win32.FilePreview_Win32_COM.GetStreamOrDefault(finfo, options);
            }

            #endif

            return null;
        }

        [return: MaybeNull]
        public static async Task<WindowsBitmap> GetPreviewOrDefaultAsync([MaybeNull] System.IO.FileInfo finfo, FilePreviewOptions options = null)
        {
            if (finfo == null || !finfo.Exists) return null;
            
            #if WINDOWS10_0_19041_0_OR_GREATER

            return await Platforms.UWP.WindowsThumbnailService.GetThumbnailAsync(finfo, options).ConfigureAwait(true);

            #endif

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return await Task.Run(() => GetPreviewOrDefault(finfo, options)).ConfigureAwait(true);
            }

            return null;
        }        

        [return: MaybeNull]
        public static WindowsBitmap GetPreviewOrDefault([MaybeNull] System.IO.FileInfo finfo, FilePreviewOptions options = null)
        {
            if (finfo == null || !finfo.Exists) return null;

            #if WINDOWS10_0_19041_0_OR_GREATER

            return Platforms.UWP.WindowsThumbnailService.GetThumbnailAsync(finfo, options).GetAwaiter().GetResult();

            #elif NET8_0_OR_GREATER

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return Platforms.Win32.FilePreview_GeneratedCOM.GetPreviewOrDefault(finfo, options);
            }

            #elif NET6_0

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return Platforms.Win32.FilePreview_Win32_COM.GetPreviewOrDefault(finfo, options);
            }
            
            #endif

            return null;
        }        
    }
}
