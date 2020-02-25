using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial class OpenCvSharp4Toolkit
    {
        public static MemoryBitmap LoadMemoryBitmapFromOpenCvSharp4(this System.IO.FileInfo finfo, OpenCvSharp.ImreadModes mode = OpenCvSharp.ImreadModes.Color)
        {
            using (var mat = new OpenCvSharp.Mat(finfo.FullName, mode))
            {
                return mat.AsSpanBitmap().ToMemoryBitmap();
            }
        }
    }
}
