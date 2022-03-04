using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Adapters
{
    public readonly ref struct SkiaSpanAdapter
    {
        #region constructor

        public SkiaSpanAdapter(SpanBitmap bmp) { _Bitmap = bmp; }

        #endregion

        #region data

        private readonly SpanBitmap _Bitmap;

        #endregion

        #region API

        public void Draw(Action<SkiaSharp.SKCanvas> onDraw)
        {
            _Bitmap.PinWritablePointer(ptr => _OnDraw(ptr, onDraw));
            
        }

        private static void _OnDraw(PointerBitmap ptr, Action<SkiaSharp.SKCanvas> onDraw)
        {
            using(var dst = _Implementation.WrapAsSKBitmap(ptr))
            {
                using(var canvas = new SkiaSharp.SKCanvas(dst))
                {
                    onDraw(canvas);
                }
            }
        }

        #endregion
    }

}
