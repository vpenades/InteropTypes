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
}
