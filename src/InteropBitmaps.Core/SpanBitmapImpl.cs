using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    static class SpanBitmapImpl
    {
        public static unsafe void PinWritablePointer(Span<Byte> writable, in BitmapInfo binfo, Action<PointerBitmap> onPinned)
        {
            if (writable.IsEmpty) throw new ArgumentNullException(nameof(writable));
            if (onPinned == null) throw new ArgumentNullException(nameof(onPinned));

            fixed (byte* ptr = &writable.GetPinnableReference())
            {
                var ptrBmp = new PointerBitmap(new IntPtr(ptr), binfo);

                onPinned(ptrBmp);
            }
        }

        public static unsafe void PinReadablePointer(ReadOnlySpan<Byte> readable, in BitmapInfo binfo, Action<PointerBitmap> onPinned)
        {
            if (readable.IsEmpty) throw new ArgumentNullException(nameof(readable));
            if (onPinned == null) throw new ArgumentNullException(nameof(onPinned));

            fixed (byte* ptr = &readable.GetPinnableReference())
            {
                var ptrBmp = new PointerBitmap(new IntPtr(ptr), binfo, true);

                onPinned(ptrBmp);
            }
        }

        public static unsafe TResult PinReadablePointer<TResult>(ReadOnlySpan<Byte> readable, in BitmapInfo binfo, PointerBitmap.Function1<TResult> onPinned)
        {
            if (readable.IsEmpty) throw new ArgumentNullException(nameof(readable));
            if (onPinned == null) throw new ArgumentNullException(nameof(onPinned));

            fixed (byte* ptr = &readable.GetPinnableReference())
            {
                var ptrBmp = new PointerBitmap(new IntPtr(ptr), binfo, true);

                return onPinned(ptrBmp);
            }
        }

        public static unsafe void PinTransferPointers(SpanBitmap src, SpanBitmap dst, PointerBitmap.Action2 onPinned)
        {
            if (src.ReadableBytes.IsEmpty) throw new ArgumentNullException(nameof(src));
            if (src.WritableBytes.IsEmpty) throw new ArgumentNullException(nameof(dst));
            if (onPinned == null) throw new ArgumentNullException(nameof(onPinned));

            fixed (byte* srcPtr = &src.ReadableBytes.GetPinnableReference())
            fixed (byte* dstPtr = &dst.WritableBytes.GetPinnableReference())
            {
                var srcBmp = new PointerBitmap(new IntPtr(srcPtr), src.Info, true);
                var dstBmp = new PointerBitmap(new IntPtr(dstPtr), dst.Info, false);

                onPinned(srcBmp, dstBmp);
            }
        }
    }
}
