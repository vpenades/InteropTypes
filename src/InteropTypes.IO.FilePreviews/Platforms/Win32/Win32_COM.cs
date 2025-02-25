using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Windows.Win32;

using InteropTypes.Graphics;
using InteropTypes.IO;

namespace InteropTypes.Platforms.Win32
{
    [SupportedOSPlatform("windows6.0.6000")]
    internal static class FilePreview_Win32_COM
    {
        public static WindowsBitmap GetPreviewOrDefault(System.IO.FileInfo finfo)
        {
            try
            {
                using (var bmp = GetThumbnail(finfo.FullName, 128, 128))
                {
                    if (bmp == null) return null;

                    return bmp.GetBitmap(PixelFormat.Format24bppRgb);
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
        public static Bitmap GetThumbnail(string filePath, int width, int height)
        {
            // get the file's image factory
            var factory = Windows.Win32.PInvoke.GetShellItemImageFactory(filePath);
            if (factory == null) return null;

            var size = new Windows.Win32.Foundation.SIZE(width, height);            

            var flags
                = Windows.Win32.UI.Shell.SIIGBF.SIIGBF_BIGGERSIZEOK
                // | Windows.Win32.UI.Shell.SIIGBF.SIIGBF_RESIZETOFIT                
                // | Windows.Win32.UI.Shell.SIIGBF.SIIGBF_THUMBNAILONLY // only if it has an actual thumbnail and not a generic extension image.
                ;

            using (var hbmp = factory.GetImage(size, flags))
            {
                if (hbmp == null) return null;

                return Image.FromHbitmap(hbmp.DangerousGetHandle());
            }
        }        
    }
}