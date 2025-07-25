#if WINDOWS10_0_19041_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

namespace InteropTypes.Platforms.UWP
{
    internal static class WindowsThumbnailService
    {
        public static async Task<Graphics.WindowsBitmap> GetThumbnailAsync(System.IO.FileInfo finfo, IO.FilePreviewOptions clientOptions = null)
        {
            clientOptions ??= IO.FilePreviewOptions._Default;

            using (var s = await GetThumbnailStreamAsync(finfo, clientOptions))
            {
                if (s == null) return null;

                using (var bmp = new System.Drawing.Bitmap(s))
                {
                    return bmp.GetWindowsBitmap(clientOptions.GetPixelFormat());
                }
            }            
        }


        public static async Task<System.IO.Stream> GetThumbnailStreamAsync(System.IO.FileInfo finfo, IO.FilePreviewOptions clientOptions = null)
        {
            clientOptions ??= IO.FilePreviewOptions._Default;

            // https://learn.microsoft.com/en-us/windows/uwp/files/thumbnails

            // var stofile = StorageFile.GetFileFromApplicationUriAsync(...)

            var stofile = await StorageFile.GetFileFromPathAsync(finfo.FullName);

            var options = Windows.Storage.FileProperties.ThumbnailOptions.None;
            if (clientOptions.CachedOnly) options |= Windows.Storage.FileProperties.ThumbnailOptions.ReturnOnlyIfCached;
            if (!clientOptions.AllowBigger) options |= Windows.Storage.FileProperties.ThumbnailOptions.ResizeThumbnail;

            var size = (uint)Math.Max(clientOptions.Width, clientOptions.Height);

            var thumb = await stofile.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.SingleItem, size, options);

            if (thumb == null) return null;

            return thumb.AsStreamForRead();
        }        
    }
}

#endif
