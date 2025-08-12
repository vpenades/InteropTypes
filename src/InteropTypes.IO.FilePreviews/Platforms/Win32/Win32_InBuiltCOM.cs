using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Windows.Win32;

using InteropTypes.Graphics;

namespace InteropTypes.Platforms.Win32
{
    // https://github.com/microsoft/CsWin32/issues/1444
    // https://github.com/microsoft/CsWin32/issues/1422
    // https://github.com/microsoft/CsWin32/issues/882  < AnyCPU


    [SupportedOSPlatform("windows6.0.6000")]
    internal static class FilePreview_Win32_COM
    {
        public static System.IO.Stream GetStreamOrNull(FILEINFO finfo, IO.FilePreviewOptions clientOptions = null)
        {
            try
            {
                using (var bmp = GetThumbnail(finfo.FullName, clientOptions))
                {
                    if (bmp == null) return null;

                    var mem = new System.IO.MemoryStream();

                    bmp.Save(mem, ImageFormat.Bmp);

                    return mem;
                }
            }
            catch (COMException ex)
            {
                return null;
            }
        }

        public static WindowsBitmap GetManagedBmpOrNull(FILEINFO finfo, IO.FilePreviewOptions clientOptions = null)
        {
            try
            {
                using (var bmp = GetThumbnail(finfo.FullName, clientOptions))
                {
                    if (bmp == null) return null;

                    return bmp.GetWindowsBitmap(PixelFormat.Format24bppRgb);
                }
            }
            catch (COMException ex)
            {
                return null;
            }
        }        

        /// <summary>
        /// Gets the thumbnail of a system file
        /// </summary>
        /// <param name="filePath">path to the file</param>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        /// <returns>the bitmap of the thumbnail, or null</returns>
        public static Bitmap GetNativeBmpOrNull(string filePath, IO.FilePreviewOptions clientOptions = null)
        {
            clientOptions ??= IO.FilePreviewOptions._Default;

            // get the file's image factory
            var factory = Windows.Win32.PInvoke.GetShellItemImageFactory(filePath);
            if (factory == null) return null;

            var size = new Windows.Win32.Foundation.SIZE(clientOptions.Width, clientOptions.Height);

            Windows.Win32.UI.Shell.SIIGBF flags = default;
            if (clientOptions.AllowBigger) flags |= Windows.Win32.UI.Shell.SIIGBF.SIIGBF_BIGGERSIZEOK;
            if (clientOptions.CachedOnly) flags |= Windows.Win32.UI.Shell.SIIGBF.SIIGBF_THUMBNAILONLY;                

            using (var hbmp = factory.GetImage(size, flags))
            {
                if (hbmp == null) return null;

                return Image.FromHbitmap(hbmp.DangerousGetHandle());
            }
        }        
    }
}