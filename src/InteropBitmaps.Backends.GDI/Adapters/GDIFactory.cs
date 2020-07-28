using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace InteropBitmaps.Adapters
{
    public readonly struct GDIFactory
    {
        #region lifecycle

        internal GDIFactory(BitmapInfo binfo)
        {
            _Info = binfo;
            _Exact = _Implementation.ToPixelFormat(_Info.PixelFormat, false);
            _Compatible = _Implementation.ToPixelFormat(_Info.PixelFormat, true);
        }

        #endregion

        #region data

        private readonly BitmapInfo _Info;
        private readonly System.Drawing.Imaging.PixelFormat _Exact;
        private readonly System.Drawing.Imaging.PixelFormat _Compatible;

        #endregion

        #region API

        public Bitmap CreateBitmap()
        {
            var fmt = _Implementation.ToPixelFormat(_Info.PixelFormat, false);
            if (fmt == System.Drawing.Imaging.PixelFormat.Undefined) throw new ArgumentException(nameof(_Info));

            return new Bitmap(_Info.Width, _Info.Height, fmt);
        }

        public void CopyTo(ref Bitmap bmp)
        {
            if (!IsExact(bmp)) { bmp?.Dispose(); bmp = null; }

            if (bmp == null) bmp = CreateBitmap();
        }

        public bool IsExact(Bitmap bmp)
        {
            if (bmp == null) return false;
            if (bmp.Width != _Info.Width) return false;
            if (bmp.Height != _Info.Height) return false;
            if (bmp.PixelFormat != _Exact) return false;
            return true;
        }

        #endregion
    }
}
