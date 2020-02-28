using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial class OpenCvSharp4Toolkit
    {
        public static PixelFormat ToInteropFormat(this OpenCvSharp.MatType fmt)
        {
            if (fmt.IsInteger)
            {
                if (fmt == OpenCvSharp.MatType.CV_8UC1) return PixelFormat.Standard.GRAY8;
                if (fmt == OpenCvSharp.MatType.CV_16UC1) return PixelFormat.Standard.GRAY16;

                if (fmt == OpenCvSharp.MatType.CV_8UC3) return PixelFormat.Standard.BGR24;
                if (fmt == OpenCvSharp.MatType.CV_8UC4) return PixelFormat.Standard.BGRA32;
            }

            throw new NotImplementedException();
        }
    }
}
