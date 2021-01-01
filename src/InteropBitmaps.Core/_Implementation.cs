using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    static class _Implementation
    {
        public static Span<T> OfType<T>(this Span<Byte> span)
            where T:unmanaged
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, T>(span);
        }

        public static ReadOnlySpan<T> OfType<T>(this ReadOnlySpan<Byte> span)
            where T : unmanaged
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, T>(span);
        }        

        public static SpanBitmap Crop(SpanBitmap src, BitmapBounds rect)
        {
            rect = rect.Clipped(src.Bounds);
            if (rect.Width <= 0 || rect.Height <= 0) return default;
            return src.Slice(rect);
        }

        public static SpanBitmap<TPixel> Crop<TPixel>(SpanBitmap<TPixel> src, BitmapBounds rect)
            where TPixel : unmanaged
        {
            rect = rect.Clipped(src.Bounds);
            if (rect.Width <= 0 || rect.Height <= 0) return default;
            return src.Slice(rect);
        }

        public static void CopyPixels(SpanBitmap dst, int dstX, int dstY, SpanBitmap src)
        {
            var dstCrop = Crop(dst, (+dstX, +dstY, src.Width, src.Height));
            var srcCrop = Crop(src, (-dstX, -dstY, dst.Width, dst.Height));

            System.Diagnostics.Debug.Assert(dstCrop.Width == srcCrop.Width);
            System.Diagnostics.Debug.Assert(dstCrop.Height == srcCrop.Height);

            if (dstCrop.Width <= 0 || dstCrop.Height <= 0) return;

            // no conversion required
            if (dstCrop.PixelFormat == srcCrop.PixelFormat)
            {
                for (int y = 0; y < dstCrop.Height; ++y)
                {
                    var srcRow = srcCrop.GetScanlineBytes(y);
                    var dstRow = dstCrop.UseScanlineBytes(y);

                    System.Diagnostics.Debug.Assert(srcRow.Length == srcRow.Length);

                    srcRow.CopyTo(dstRow);
                }
            }

            // conversion required
            else
            {
                for (int y = 0; y < dstCrop.Height; ++y)
                {
                    var srcRow = srcCrop.GetScanlineBytes(y);
                    var dstRow = dstCrop.UseScanlineBytes(y);
                    Pixel.ConvertPixels(dstRow, dstCrop.PixelFormat, srcRow, srcCrop.PixelFormat);
                }                
            }
        }

        public static void ApplyPixels<TSrcPixel, TDstPixel>(SpanBitmap<TDstPixel> dst, int dstX, int dstY, SpanBitmap<TSrcPixel> src, Func<TDstPixel, TSrcPixel, TDstPixel> pixelFunc)
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged
        {
            var dstCrop = Crop(dst, (+dstX, +dstY, src.Width, src.Height));
            var srcCrop = Crop(src, (-dstX, -dstY, dst.Width, dst.Height));

            System.Diagnostics.Debug.Assert(dstCrop.Width == srcCrop.Width);
            System.Diagnostics.Debug.Assert(dstCrop.Height == srcCrop.Height);

            if (dstCrop.Width <= 0 || dstCrop.Height <= 0) return;

            for (int y = 0; y < dstCrop.Height; ++y)
            {
                var srcRow = srcCrop.UseScanlinePixels(y);
                var dstRow = dstCrop.UseScanlinePixels(y);

                System.Diagnostics.Debug.Assert(srcRow.Length == srcRow.Length);

                for (int x=0; x < dstRow.Length; ++x)
                {
                    var d = dstRow[x];
                    dstRow[x] = pixelFunc(d, srcRow[x]);
                }                
            }
        }


        /*
        static void CopyPixels<TSrc, TDst>(SpanBitmap<TDst> dst, int dstX, int dstY, SpanBitmap<TSrc> src)
            where TSrc:unmanaged
            where TDst:unmanaged
        {
            throw new NotImplementedException();

            // first problem: Vector<T> does not support ReadOnlySpan<T>

            var srcSpan = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TSrc>(src.WritableSpan);
            var dstSpan = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TDst>(dst.WritableSpan);

            var srcVector = new System.Numerics.Vector<TSrc>(srcSpan);
            var dstVector = new System.Numerics.Vector<TDst>(dstSpan);

            // second problem: to allow bulk conversions, it is still not possible to do a srcVector.CopyTo(dstVector);
        }*/


        public static void FitPixelsNearest<TPixel>(SpanBitmap<TPixel> dst, SpanBitmap<TPixel> src)
            where TPixel:unmanaged
        {
            // TODO: cannot run in parallel because src & dst cannot be passed to lambdas
            //       we would need to pin and use PointerBitmaps

            for(int y=0; y < dst.Height; ++y)
            {
                var yy = y * src.Height / dst.Height;

                for (int x = 0; x < dst.Height; ++x)
                {
                    var xx = x * src.Width / dst.Width;

                    var p = src.GetPixel(xx, yy);

                    dst.SetPixel(x, y, p);
                }
            }
        }


        public static int CalculateHashCode(ReadOnlySpan<Byte> data, in BitmapInfo info)
        {
            if (info.IsEmpty) return 0;            

            System.Diagnostics.Debug.Assert(data.Length == info.BitmapByteSize);

            // if the bytestride exceeds the actual row size, the region outside the
            // bitmap is not accounted for the hash.

            int rowSize = info.Width * info.PixelByteSize;
            int x = 0;
            int y = 0;
            int step = 1;
            int count = 0;

            ulong h = 0;

            while ((x + y) < data.Length)
            {
                h <<= 2;
                h += data[x + y];

                x += step;
                while(x > rowSize)
                {
                    x -= rowSize;
                    y += info.StepByteSize;
                }

                ++count; if (count > 5) { count = 0; step += 2; }
            }
            
            return h.GetHashCode();
        }
    }
}
