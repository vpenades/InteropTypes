using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using GDIFMT = System.Drawing.Imaging.PixelFormat;

namespace InteropBitmaps
{
    public readonly ref struct GDIAdapter       
    {
        #region constructor

        public GDIAdapter(SpanBitmap bmp) { _Bitmap = bmp; }

        #endregion

        #region data

        private readonly SpanBitmap _Bitmap;

        #endregion

        #region API

        public Bitmap CloneToGDI(GDIFMT pixFmt)
        {
            if (_Bitmap.PixelSize != pixFmt.GetPixelSize()) throw new ArgumentException(nameof(pixFmt));

            var dst = new Bitmap(_Bitmap.Width, _Bitmap.Height, pixFmt);

            dst.SetPixels(0, 0, _Bitmap);

            return dst;
        }

        public void Save(string filePath, GDIFMT pixFmt)
        {
            var finfo = new System.IO.FileInfo(filePath);
            Save(finfo, pixFmt);
        }

        public void Save(System.IO.FileInfo finfo, GDIFMT pixFmt)
        {
            using (var img = CloneToGDI(pixFmt))
            {
                img.Save(finfo.FullName);
            }
        }

        public void Save(System.IO.FileInfo finfo, GDIFMT pixFmt, System.Drawing.Imaging.ImageFormat imgFmt)
        {
            using (var img = CloneToGDI(pixFmt))
            {
                img.Save(finfo.FullName, imgFmt);
            }
        }

        #endregion
    }
}
