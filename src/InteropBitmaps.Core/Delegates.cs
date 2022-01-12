using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial struct PointerBitmap // these are not really needed, and it might be better to go with plain action and functions
    {
        public delegate void Action1(PointerBitmap pb);
        public delegate void Action2(PointerBitmap pb1, PointerBitmap pb2);

        public delegate TResult Function1<out TResult>(PointerBitmap pb);
    }

    partial struct SpanBitmap
    {
        public delegate void Action1(SpanBitmap sb);
        public delegate void Action2(SpanBitmap sb1, SpanBitmap sb2);     
    }
}
