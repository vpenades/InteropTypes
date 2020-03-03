using System;
using System.Collections.Generic;
using System.Text;

using GDIFMT = System.Drawing.Imaging.PixelFormat;

namespace InteropBitmaps
{
    partial class GDIToolkit
    {
        public static PixelFormat ToInteropPixelFormat(this GDIFMT fmt)
        {
            return _Implementation.ToInteropPixelFormat(fmt);
        }

        public static GDIFMT ToGDIPixelFormat(this PixelFormat fmt, bool allowCompatibleFormats = false)
        {
            return _Implementation.ToGDIPixelFormat(fmt, allowCompatibleFormats);
        }

        public static int GetPixelSize(this GDIFMT fmt)
        {
            return _Implementation.GetPixelSize(fmt);
        }        
    }
}
