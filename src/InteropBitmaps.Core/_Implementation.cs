using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    static class _Implementation
    {
        public static SpanBitmap Crop(SpanBitmap src, BitmapBounds rect)
        {
            rect = BitmapBounds.Clamp(rect, src.bounds);
            if (rect.Width <= 0 || rect.Height <= 0) return default;
            return src.Slice(rect);
        }

        public static SpanBitmap<TPixel> Crop<TPixel>(SpanBitmap<TPixel> src, BitmapBounds rect)
            where TPixel : unmanaged
        {
            rect = BitmapBounds.Clamp(rect, src.bounds);
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
                    var srcRow = srcCrop.GetBytesScanline(y);
                    var dstRow = dstCrop.UseBytesScanline(y);

                    System.Diagnostics.Debug.Assert(srcRow.Length == srcRow.Length);

                    srcRow.CopyTo(dstRow);
                }
            }

            // conversion required
            else
            {
                var srcConverter = _PixelConverters.GetConverter(srcCrop.PixelFormat);
                var dstConverter = _PixelConverters.GetConverter(dstCrop.PixelFormat);

                Span<_PixelBGRA32> tmp = stackalloc _PixelBGRA32[dstCrop.Width];

                for (int y = 0; y < dstCrop.Height; ++y)
                {
                    var srcRow = srcCrop.GetBytesScanline(y);
                    var dstRow = dstCrop.UseBytesScanline(y);

                    srcConverter.ConvertFrom(tmp, srcRow, y);
                    dstConverter.ConvertTo(dstRow, y, tmp);
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
                var srcRow = srcCrop.UsePixelsScanline(y);
                var dstRow = dstCrop.UsePixelsScanline(y);

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
    }
}
