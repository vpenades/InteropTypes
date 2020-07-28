using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using GDIFMT = System.Drawing.Imaging.PixelFormat;

namespace InteropBitmaps.Adapters
{
    public readonly ref struct GDISpanAdapter
    {
        #region constructor

        public GDISpanAdapter(SpanBitmap bmp) { _Bitmap = bmp; }

        #endregion

        #region data

        private readonly SpanBitmap _Bitmap;

        #endregion

        #region API

        private bool IsCompatibleWith(Bitmap bmp, bool allowCompatibleFormats)
        {
            if (bmp == null) return false;
            if (bmp.Width != _Bitmap.Info.Width) return false;
            if (bmp.Height != _Bitmap.Info.Height) return false;

            var fmt = _Implementation.ToPixelFormat(_Bitmap.Info.PixelFormat, allowCompatibleFormats);

            return bmp.PixelFormat == fmt;
        }

        public void CopyTo(ref Bitmap bmp)
        {
            if (!IsCompatibleWith(bmp, false))
            {
                bmp?.Dispose();
                bmp = null;
            }

            if (bmp == null) bmp = ToBitmap(true);
            else bmp.SetPixels(0, 0, _Bitmap);
        }

        public Bitmap ToBitmap(bool allowCompatibleFormats = false)
        {
            return _Implementation.CloneToGDIBitmap(_Bitmap, allowCompatibleFormats);
        }

        public Bitmap ToResizedBitmap(int width, int height)
        {
            Bitmap _resize(PointerBitmap src)
            {
                using (var wsrc = _Implementation.WrapAsGDIBitmap(src))
                {
                    return new Bitmap(wsrc, width, height);
                }
            }

            return _Bitmap.PinReadablePointer(_resize);
        }

        public MemoryBitmap ToResizedMemoryBitmap(int width, int height)
        {
            using (var tmp = ToResizedBitmap(width, height))
            {
                return tmp.ToMemoryBitmap();
            }
        }

        

        public void Draw(Action<System.Drawing.Graphics> onDraw)
        {
            _Bitmap.PinWritablePointer(ptr => _Draw(ptr, onDraw));
        }

        private static void _Draw(PointerBitmap ptr, Action<System.Drawing.Graphics> onDraw)
        {
            using (var src = _Implementation.WrapAsGDIBitmap(ptr))
            {
                using(var gfx = System.Drawing.Graphics.FromImage(src))
                {
                    onDraw(gfx);
                }
            }
        }

        #endregion
    }    
}
