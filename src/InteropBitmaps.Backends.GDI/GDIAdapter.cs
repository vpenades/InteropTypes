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

        public Bitmap CloneToGDI(bool allowCompatibleFormats = false)
        {
            var pixFmt = _Bitmap.PixelFormat.ToGDIPixelFormat(allowCompatibleFormats);            

            if (!allowCompatibleFormats && _Bitmap.PixelSize != pixFmt.GetPixelSize()) throw new ArgumentException(nameof(pixFmt));

            var dst = new Bitmap(_Bitmap.Width, _Bitmap.Height, pixFmt);

            dst.SetPixels(0, 0, _Bitmap);

            return dst;
        }

        public void Save(string filePath)
        {
            var finfo = new System.IO.FileInfo(filePath);
            Save(finfo);
        }

        public void Save(System.IO.FileInfo finfo)
        {
            using (var img = CloneToGDI(true))
            {
                img.Save(finfo.FullName);
            }
        }

        public void Save(System.IO.FileInfo finfo, System.Drawing.Imaging.ImageFormat imgFmt)
        {
            using (var img = CloneToGDI(true))
            {
                img.Save(finfo.FullName, imgFmt);
            }
        }

        #endregion
    }
}
