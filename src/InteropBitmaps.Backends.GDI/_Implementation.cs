using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace InteropBitmaps
{
    static class _Implementation
    {
        public static Bitmap CloneToGDI(SpanBitmap src, bool allowCompatibleFormats = false)
        {
            var pixFmt = src.PixelFormat.ToGDIPixelFormat(allowCompatibleFormats);

            if (!allowCompatibleFormats && src.PixelSize != pixFmt.GetPixelSize()) throw new ArgumentException(nameof(pixFmt));

            var dst = new Bitmap(src.Width, src.Height, pixFmt);

            dst.SetPixels(0, 0, src);

            return dst;
        }

        public static void Save(string filePath, SpanBitmap src)
        {
            var finfo = new System.IO.FileInfo(filePath);
            Save(finfo, src);
        }

        public static void Save(System.IO.FileInfo finfo, SpanBitmap src)
        {
            using (var img = CloneToGDI(src, true))
            {
                img.Save(finfo.FullName);
            }
        }

        public static void Save(System.IO.FileInfo finfo, SpanBitmap src, System.Drawing.Imaging.ImageFormat imgFmt)
        {
            using (var img = CloneToGDI(src, true))
            {
                img.Save(finfo.FullName, imgFmt);
            }
        }
    }
}
