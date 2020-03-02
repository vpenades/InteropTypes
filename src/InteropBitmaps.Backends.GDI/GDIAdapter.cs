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
            return _Implementation.CloneToGDI(_Bitmap, allowCompatibleFormats);
        }

        public void Save(string filePath) { _Implementation.Save(filePath, _Bitmap); }

        public void Save(System.IO.FileInfo finfo) { _Implementation.Save(finfo, _Bitmap); }

        public void Save(System.IO.FileInfo finfo, System.Drawing.Imaging.ImageFormat imgFmt) { _Implementation.Save(finfo, _Bitmap, imgFmt); }

        #endregion
    }
}
