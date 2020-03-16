using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    sealed class BitmapLock : ISpanLock
    {
        #region lifecycle

        public BitmapLock(System.Drawing.Bitmap bmp, bool readOnly)
        {
            _Bitmap = bmp;

            var rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);

            var mode = readOnly ? System.Drawing.Imaging.ImageLockMode.ReadOnly : System.Drawing.Imaging.ImageLockMode.ReadWrite;

            _Bits = bmp.LockBits(rect, mode, bmp.PixelFormat);

            _Info = _Implementation.GetBitmapInfo(_Bits);
            _IsReadOnly = readOnly;
        }

        public void Dispose()
        {
            _Bitmap?.UnlockBits(_Bits);
            _Bitmap = null;
            _Info = default;
            _Bits = null;
        }

        #endregion

        #region data

        private System.Drawing.Bitmap _Bitmap;
        private System.Drawing.Imaging.BitmapData _Bits;
        private BitmapInfo _Info;
        private bool _IsReadOnly;

        #endregion

        #region Properties

        public BitmapInfo Info => _Info;

        public SpanBitmap Span => new SpanBitmap(_Bits.Scan0, _Info, _IsReadOnly);

        #endregion
    }
}
