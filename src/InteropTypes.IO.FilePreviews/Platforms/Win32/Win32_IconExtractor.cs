using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics;

namespace InteropTypes.Platforms.Win32
{
    [SupportedOSPlatform("windows6.1")]
    internal static partial class IconExtractor
    {
        // https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/how-to-extract-the-icon-associated-with-a-file-in-windows-forms


        public static WindowsBitmap GetManagedBmpOrNull(FILEINFO finfo, IO.FilePreviewOptions clientOptions = null)
        {
            try
            {
                using (var bmp = GetNativeBmpOrNull(finfo, true))
                {
                    if (bmp == null) return null;

                    return bmp.GetWindowsBitmap(clientOptions?.GetPixelFormat() ?? PixelFormat.Format24bppRgb);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static System.IO.Stream GetStreamOrDefault(FILEINFO finfo, IO.FilePreviewOptions clientOptions = null)
        {
            try
            {
                using (var bmp = GetNativeBmpOrNull(finfo, true))
                {
                    if (bmp == null) return default;

                    using (var mem = new System.IO.MemoryStream())
                    {
                        bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Png);

                        return mem;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Bitmap GetNativeBmpOrNull(FILEINFO finfo, bool large)
        {
            // extracts the icon from inside the file (for example, from EXEs and DLLs
            using (var icon = System.Drawing.Icon.ExtractIcon(finfo.FullName, 0))
            {
                if (icon != null) return icon.ToBitmap();
            }

            // gets the generic icon associated with the extension. (This is the one that should be cached)
            using(var icon = System.Drawing.Icon.ExtractAssociatedIcon(finfo.FullName))
            {
                if (icon != null) return icon.ToBitmap();
            }

            return null;
        }        
    }
}
