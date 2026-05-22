using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using InteropTypes.Graphics;
using InteropTypes.IO.Mvvm;

using AVLIMAGE = Avalonia.Media.IImage;

namespace InteropTypes.IO.Controls
{
    /// <summary>
    /// An image loader that loads images from the windows cache if possible.
    /// </summary>
    class _FileThumbnailFactory : _FileThumbnailFallbackFactory
    {
        // a stategy would be to request

        public static IFileThumbnailServer<AVLIMAGE> Create()
        {
            return new _FileThumbnailFactory();
        }

        public FilePreviewOptions Options { get; set; }

        public override async Task<bool> LoadAndAssignImageAsync(IFileThumbnailClient<AVLIMAGE> item)
        {
            ArgumentNullException.ThrowIfNull(item);

            var info = item.GetSource();
            if (info == null) return false;

            if (!string.IsNullOrWhiteSpace(info.PhysicalPath))
            {
                switch (info.IsDirectory)
                {
                    case false: item.SetImage(GetThumbnail(new System.IO.FileInfo(info.PhysicalPath))); return true;
                    case true: item.SetImage(GetThumbnail(new System.IO.DirectoryInfo(info.PhysicalPath))); return true;
                }
            }

            return await base.LoadAndAssignImageAsync(item);
        }

        public AVLIMAGE GetThumbnail(System.IO.FileSystemInfo info)
        {
            if (info is not System.IO.FileInfo finfo) return null;

            var bmp = FilePreviewFactory.GetPreviewOrDefault(finfo, Options);
            if (bmp == null) return null;

            return ConvertToBitmap(bmp);
        }

        private static Avalonia.Media.Imaging.Bitmap ConvertToBitmap(WindowsBitmap srcBmp)
        {
            if (srcBmp == null) return null;

            using (var m = srcBmp.OpenRead())
            {
                if (m == null) return null;
                var avlbmp = new Avalonia.Media.Imaging.Bitmap(m);

                if (avlbmp.PixelSize.Width != srcBmp.Width || avlbmp.PixelSize.Height != srcBmp.Height)
                {
                    avlbmp.Dispose();
                    return null;
                }

                return avlbmp;
            }
        }
    }


    /// <summary>
    /// An image loader that simply loads images
    /// </summary>
    class _FileThumbnailFallbackFactory : IFileThumbnailServer<AVLIMAGE>
    {
        public virtual async Task<bool> LoadAndAssignImageAsync(IFileThumbnailClient<AVLIMAGE> item)
        {
            ArgumentNullException.ThrowIfNull(item);

            var info = item.GetSource();
            if (info == null) return false;
            if (info.IsDirectory) return false;

            // ToDo: exit if is not image            

            try
            {
                using var s = info.CreateReadStream();
                if (s == null) return false;
                var img = Avalonia.Media.Imaging.Bitmap.DecodeToHeight(s, 256);
                item.SetImage(img);

            }
            catch (NullReferenceException) // this is the exception produced by DecodeToHeight when it does not have a codec
            {
                return false;
            }

            return true;
        }
    }
}
