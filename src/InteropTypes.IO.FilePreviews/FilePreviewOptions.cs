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
        internal static FilePreviewOptions _Default { get; } = new FilePreviewOptions();

        public bool AllowBigger { get; set; } = true;
        public bool CachedOnly { get; set; } = true;

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
