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

        #endregion
    }
}
