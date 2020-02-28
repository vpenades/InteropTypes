using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    public readonly struct PointerBitmap
    {
        public static implicit operator PointerBitmap((IntPtr,BitmapInfo) ptrbmp)
        {
            return new PointerBitmap(ptrbmp.Item1, ptrbmp.Item2);
        }

        public PointerBitmap(IntPtr ptr, BitmapInfo info, bool isReadOnly = false)
        {
            _Pointer = ptr;
            _Info = info;
            _IsReadOnly = isReadOnly;
        }

        private readonly IntPtr _Pointer;
        private readonly BitmapInfo _Info;
        private readonly Boolean _IsReadOnly;

        public IntPtr Pointer => _Pointer;
        public BitmapInfo Info => _Info;

        public Boolean IsReadOnly => _IsReadOnly;

        public SpanBitmap Span => new SpanBitmap(_Pointer, _Info, _IsReadOnly);

        public SpanBitmap<TPixel> AsSpanBitmap<TPixel>() where TPixel:unmanaged
        {
            return new SpanBitmap<TPixel>(_Pointer, _Info,_IsReadOnly);
        }
    }
}
