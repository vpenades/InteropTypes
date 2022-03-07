using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Backends;

namespace InteropTypes
{
    partial class _Implementation
    {
        #region copy pixels

        public static void CopyPixels(Image src, SpanBitmap dst)
        {
            if (src is Image<A8> srcA8) { CopyPixels(srcA8, dst); return; }

            if (src is Image<L8> srcL8) { CopyPixels(srcL8, dst); return; }
            if (src is Image<L16> srcL16) { CopyPixels(srcL16, dst); return; }

            if (src is Image<Bgr565> srcBgr565) { CopyPixels(srcBgr565, dst); return; }
            if (src is Image<Bgra4444> srcBgra4444) { CopyPixels(srcBgra4444, dst); return; }
            if (src is Image<Bgra5551> srcBgra5551) { CopyPixels(srcBgra5551, dst); return; }

            if (src is Image<Rgb24> srcRgb24) { CopyPixels(srcRgb24, dst); return; }
            if (src is Image<Bgr24> srcBgr24) { CopyPixels(srcBgr24, dst); return; }

            if (src is Image<Rgba32> srcRgba32) { CopyPixels(srcRgba32, dst); return; }
            if (src is Image<Bgra32> srcBgra32) { CopyPixels(srcBgra32, dst); return; }
            if (src is Image<Argb32> srcArgb32) { CopyPixels(srcArgb32, dst); return; }

            if (src is Image<RgbaVector> srcRgbaVector) { CopyPixels(srcRgbaVector, dst); return; }

            throw new NotImplementedException();
        }

        public static void CopyPixels(SpanBitmap src, Image dst)
        {
            if (dst is Image<A8> dstA8) { CopyPixels(src, dstA8); return; }

            if (dst is Image<L8> dstL8) { CopyPixels(src, dstL8); return; }
            if (dst is Image<L16> dstL16) { CopyPixels(src, dstL16); return; }

            if (dst is Image<Bgr565> dstBgr565) { CopyPixels(src, dstBgr565); return; }
            if (dst is Image<Bgra4444> dstBgra4444) { CopyPixels(src, dstBgra4444); return; }
            if (dst is Image<Bgra5551> dstBgra5551) { CopyPixels(src, dstBgra5551); return; }

            if (dst is Image<Rgb24> dstRgb24) { CopyPixels(src, dstRgb24); return; }
            if (dst is Image<Bgr24> dstBgr24) { CopyPixels(src, dstBgr24); return; }

            if (dst is Image<Rgba32> dstRgba32) { CopyPixels(src, dstRgba32); return; }
            if (dst is Image<Bgra32> dstBgra32) { CopyPixels(src, dstBgra32); return; }
            if (dst is Image<Argb32> dstArgb32) { CopyPixels(src, dstArgb32); return; }

            if (dst is Image<RgbaVector> dstRgbaVector) { CopyPixels(src, dstRgbaVector); return; }

            throw new NotImplementedException();
        }

        #endregion
    }
}