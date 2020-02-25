using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using GDIFMT = System.Drawing.Imaging.PixelFormat;

namespace InteropBitmaps
{
    partial class GDIToolkit
    {
        public static MemoryBitmap LoadMemoryBitmapFromGDI(this System.IO.FileInfo finfo)
        {
            using (var s = finfo.OpenRead())
            {
                return s.ReadMemoryBitmapFromGDI();
            }
        }

        public static MemoryBitmap ReadMemoryBitmapFromGDI(this System.IO.Stream s)
        {
            using (var img = Image.FromStream(s))
            {
                return img.CloneToMemoryBitmap();
            }
        }        
    }
}
