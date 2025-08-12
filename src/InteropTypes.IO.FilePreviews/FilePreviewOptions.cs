using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.IO
{
    public class FilePreviewOptions
    {
        public static bool ShouldExtractAssociatedIcon(FILEINFO finfo, FilePreviewOptions options)
        {
            options ??= _Default;

            if (options.ExtensionAssociatedIcon) return true;

            var ext = finfo.Extension.TrimStart('.').ToLowerInvariant();

            switch(ext)
            {
                case "exe": return true;
                case "dll": return true;
                case "txt": return true;
                case "md": return true;
                case "xml": return true;
                case "json": return true;
                case "nfo": return true;
                case "bin": return true;
            }            

            return false;
        }

        internal static FilePreviewOptions _Default { get; } = new FilePreviewOptions();

        /// <summary>
        /// If set to true it will retrieve the icon associated with the extension
        /// </summary>
        public bool ExtensionAssociatedIcon { get; set; }

        public bool AllowBigger { get; set; } = true;
        public bool CachedOnly { get; set; } = false;

        public bool PrefferTransparency { get; set; } = false;

        public int Width { get; set; } = 128;

        public int Height { get; set; } = 128;


        [SupportedOSPlatform("windows")]
        internal System.Drawing.Imaging.PixelFormat GetPixelFormat()
        {
            if (PrefferTransparency) return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            return System.Drawing.Imaging.PixelFormat.Format24bppRgb;
        }
    }
}
