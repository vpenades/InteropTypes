using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using GDIFMT = System.Drawing.Imaging.PixelFormat;

namespace InteropBitmaps.Adapters
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

        public void Update(ref Bitmap bmp)
        {
            // TODO: check compatible

            if (bmp == null) bmp = CloneToGDI(true);
            else bmp.SetPixels(0, 0, _Bitmap);
        }

        public Bitmap CloneToGDI(bool allowCompatibleFormats = false)
        {
            return _Implementation.CloneToGDIBitmap(_Bitmap, allowCompatibleFormats);
        }    
        
        public MemoryBitmap ToResizedMemoryBitmap(int width, int height)
        {
            return _Bitmap.PinReadableMemory(ptr => _Resize(ptr, width, height));
        }

        private static MemoryBitmap _Resize(PointerBitmap src, int width, int height)
        {
            using (var wsrc = _Implementation.WrapAsGDIBitmap(src))
            {
                using (var dst = new Bitmap(wsrc, width, height))
                {
                    return dst.ToMemoryBitmap(src.Info.PixelFormat);
                }
            }
        }

        public void Draw(Action<System.Drawing.Graphics> onDraw)
        {
            _Bitmap.PinWritableMemory(ptr => _Draw(ptr, onDraw));
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
