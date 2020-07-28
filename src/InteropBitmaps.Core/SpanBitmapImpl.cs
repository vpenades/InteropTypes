using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    static class SpanBitmapImpl
    {
        public static unsafe void PinWritablePointer(Span<Byte> writable, in BitmapInfo binfo, Action<PointerBitmap> onPin)
        {
            if (writable.IsEmpty) throw new InvalidOperationException();

            fixed (byte* ptr = &writable.GetPinnableReference())
            {
                var ptrBmp = new PointerBitmap(new IntPtr(ptr), binfo);

                onPin(ptrBmp);
            }
        }

        public static unsafe void PinReadablePointer(ReadOnlySpan<Byte> readable, in BitmapInfo binfo, Action<PointerBitmap> onPin)
        {
            if (readable.IsEmpty) throw new InvalidOperationException();

            fixed (byte* ptr = &readable.GetPinnableReference())
            {
                var ptrBmp = new PointerBitmap(new IntPtr(ptr), binfo, true);

                onPin(ptrBmp);
            }
        }

        public static unsafe void PinTransferPointers(SpanBitmap src, SpanBitmap dst, Action<PointerBitmap, PointerBitmap> onPin)
        {
            fixed (byte* srcPtr = &src.ReadableSpan.GetPinnableReference())
            fixed (byte* dstPtr = &dst.WritableSpan.GetPinnableReference())
            {
                var srcBmp = new PointerBitmap(new IntPtr(srcPtr), src.Info, true);
                var dstBmp = new PointerBitmap(new IntPtr(dstPtr), dst.Info, false);

                onPin(srcBmp, dstBmp);
            }
        }

        public static unsafe TResult PinReadablePointer<TResult>(ReadOnlySpan<Byte> readable, in BitmapInfo binfo, Func<PointerBitmap, TResult> onPin)
        {
            if (readable.IsEmpty) throw new InvalidOperationException();

            fixed (byte* ptr = &readable.GetPinnableReference())
            {
                var ptrBmp = new PointerBitmap(new IntPtr(ptr), binfo, true);

                return onPin(ptrBmp);
            }
        }
    }
}
