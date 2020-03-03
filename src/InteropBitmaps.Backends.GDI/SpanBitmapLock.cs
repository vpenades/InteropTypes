using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    sealed class SpanBitmapLock : ISpanLock
    {
        #region lifecycle

        public SpanBitmapLock(System.Drawing.Bitmap bmp, bool readOnly)
        {
            _Bitmap = bmp;

            var rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);

            var mode = readOnly ? System.Drawing.Imaging.ImageLockMode.ReadOnly : System.Drawing.Imaging.ImageLockMode.ReadWrite;

            _Bits = bmp.LockBits(rect, mode, bmp.PixelFormat);
            _IsReadOnly = readOnly;
        }

        public void Dispose()
        {
            _Bitmap?.UnlockBits(_Bits);
            _Bitmap = null;
            _Bits = null;
        }

        #endregion

        #region data

        private System.Drawing.Bitmap _Bitmap;
        private System.Drawing.Imaging.BitmapData _Bits;
        private bool _IsReadOnly;

        #endregion

        #region Properties

        public BitmapInfo Info => _Bits.GetBitmapInfo();

        public SpanBitmap Span => new SpanBitmap(_Bits.Scan0, Info, _IsReadOnly);

        #endregion
    }
}
