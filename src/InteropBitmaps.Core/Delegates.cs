using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial struct PointerBitmap // these are not really needed, and it might be better to go with plain action and functions
    {
        public delegate void Action1(PointerBitmap self);
        public delegate void Action2(PointerBitmap self, PointerBitmap other);

        public delegate TResult Function1<out TResult>(PointerBitmap self);
    }

    partial struct SpanBitmap
    {
        public delegate void Action1(SpanBitmap self);
        public delegate void Action2(SpanBitmap self, SpanBitmap other);
        public delegate TResult Function1<out TResult>(SpanBitmap self);
        public delegate TResult Function2<out TResult>(SpanBitmap self, SpanBitmap other);
    }

    partial struct SpanBitmap<TPixel>
    {
        public delegate void Action1(SpanBitmap<TPixel> self);
        public delegate void Action2(SpanBitmap<TPixel> self, SpanBitmap<TPixel> other);
        public delegate TResult Function1<out TResult>(SpanBitmap<TPixel> self);
        public delegate TResult Function2<out TResult>(SpanBitmap<TPixel> self, SpanBitmap<TPixel> other);
    }
}
