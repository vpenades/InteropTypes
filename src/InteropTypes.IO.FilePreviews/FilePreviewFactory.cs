using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics;

namespace InteropTypes.IO
{
    public static class FilePreviewFactory
    {
        [return: MaybeNull]
        public static async Task<System.IO.Stream> GetStreamOrDefaultAsync(FILEINFO finfo, FilePreviewOptions options = null)
        {
            if (finfo == null || !finfo.Exists) return null;

            if (FilePreviewOptions.ShouldExtractAssociatedIcon(finfo, options))
            {
                return await FilePreviewOptions._ThrottleAsync(options, () => GetStreamOrDefault(finfo, options));
            }

            #if WINDOWS10_0_19041_0_OR_GREATER
            return await Platforms.UWP.WindowsThumbnailService.GetThumbnailStreamAsync(finfo, options);
            #endif

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return await FilePreviewOptions._ThrottleAsync(options, () => GetStreamOrDefault(finfo, options));
            }

            return null;
        }

        [return: MaybeNull]
        public static System.IO.Stream GetStreamOrDefault(FILEINFO finfo, FilePreviewOptions options = null)
        {
            if (finfo == null || !finfo.Exists) return null;

            if (FilePreviewOptions.ShouldExtractAssociatedIcon(finfo, options) && OperatingSystem.IsWindowsVersionAtLeast(6, 1)) return Platforms.Win32.IconExtractor.GetStreamOrDefault(finfo, options);

            #if WINDOWS10_0_19041_0_OR_GREATER

            return Platforms.UWP.WindowsThumbnailService.GetThumbnailStreamAsync(finfo, options).GetAwaiter().GetResult();

            #elif NET8_0_OR_GREATER

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return Platforms.Win32.FilePreview_GeneratedCOM.GetStreamOrNull(finfo, options);
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
        public static async Task<WindowsBitmap> GetPreviewOrDefaultAsync(FILEINFO finfo, FilePreviewOptions options = null)
        {
            if (finfo == null || !finfo.Exists) return null;

            if (FilePreviewOptions.ShouldExtractAssociatedIcon(finfo, options))
            {
                return await FilePreviewOptions._ThrottleAsync(options, () => GetPreviewOrDefault(finfo, options));
            }
            
            #if WINDOWS10_0_19041_0_OR_GREATER

            return await Platforms.UWP.WindowsThumbnailService.GetThumbnailAsync(finfo, options);

            #endif

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return await FilePreviewOptions._ThrottleAsync(options, () => GetPreviewOrDefault(finfo, options));
            }

            return null;
        }        

        [return: MaybeNull]
        public static WindowsBitmap GetPreviewOrDefault(FILEINFO finfo, FilePreviewOptions options = null)
        {
            if (finfo == null || !finfo.Exists) return null;

            if (FilePreviewOptions.ShouldExtractAssociatedIcon(finfo, options) && OperatingSystem.IsWindowsVersionAtLeast(6, 1)) return Platforms.Win32.IconExtractor.GetManagedBmpOrNull(finfo, options);

            #if WINDOWS10_0_19041_0_OR_GREATER

            return Platforms.UWP.WindowsThumbnailService.GetThumbnailAsync(finfo, options).GetAwaiter().GetResult();

            #elif NET8_0_OR_GREATER

            if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
            {
                return Platforms.Win32.FilePreview_GeneratedCOM.GetManagedBmpOrNull(finfo, options);
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
