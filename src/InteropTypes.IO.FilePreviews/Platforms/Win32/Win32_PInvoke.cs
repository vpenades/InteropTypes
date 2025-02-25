using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

using InteropTypes.IO;
using InteropTypes.Graphics;

using SHFILEATTRIBUTES = Windows.Win32.Storage.FileSystem.FILE_FLAGS_AND_ATTRIBUTES;
using SHSHELLFLAGS = Windows.Win32.UI.Shell.SHGFI_FLAGS;


namespace InteropTypes.Platforms.Win32
{
    [SupportedOSPlatform("windows6.0.6000")]
    internal static class FilePreview_Win32_PInvoke
    {
        public static WindowsBitmap GetPreviewOrDefault(System.IO.FileInfo finfo)
        {
            using (var bmp = GetFilePreviewBitmap(finfo.FullName))
            {
                if (bmp == null) return null;                

                return bmp.GetBitmap(PixelFormat.Format32bppRgb);
            }
        }
        
        public static System.Drawing.Bitmap GetFilePreviewBitmap(string filePath)
        {
            var shinfo = new Windows.Win32.UI.Shell.SHFILEINFOW();

            Windows.Win32.PInvoke.SHGetFileInfo(filePath, SHFILEATTRIBUTES.FILE_ATTRIBUTE_NORMAL, ref shinfo, SHSHELLFLAGS.SHGFI_ICON);

            if (shinfo.hIcon == IntPtr.Zero) return null;

            Bitmap bitmap = null;

            using (var icon = Icon.FromHandle(shinfo.hIcon))
            {
                bitmap = icon.ToBitmap();
            }

            // It is only necessary to call DestroyIcon for icons and cursors created with the following functions:
            // CreateIconFromResourceEx (if called without the LR_SHARED flag),
            // CreateIconIndirect, and CopyIcon.
            // Do not use this function to destroy a shared icon.
            // A shared icon is valid as long as the module from which it was loaded remains in memory.
            // The following functions obtain a shared icon.

            // Windows.Win32.PInvoke.DestroyIcon(shinfo.hIcon);

            return bitmap;
        }        
    }
}
