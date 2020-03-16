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
            return _Bitmap.PinReadableMemory<MemoryBitmap>(ptr => _Resize(width,height,ptr) );
        }

        private static MemoryBitmap _Resize(int width, int height, PointerBitmap ptr)
        {
            using (var src = _Implementation.WrapAsGDIBitmap(ptr))
            {
                using (var dst = new Bitmap(src, new Size(width, height)))
                {
                    return dst.ToMemoryBitmap(ptr.Info.PixelFormat);
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
