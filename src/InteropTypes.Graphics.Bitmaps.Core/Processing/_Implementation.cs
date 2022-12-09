using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics
{
    static class _Implementation
    {
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
            if (src.IsEmpty || dst.IsEmpty) return;

            var dstCrop = Crop(dst, (+dstX, +dstY, src.Width, src.Height));
            var srcCrop = Crop(src, (-dstX, -dstY, dst.Width, dst.Height));            

            System.Diagnostics.Debug.Assert(dstCrop.Width == srcCrop.Width);
            System.Diagnostics.Debug.Assert(dstCrop.Height == srcCrop.Height);

            if (dstCrop.Width <= 0 || dstCrop.Height <= 0) return; // nothing to copy

            System.Diagnostics.Debug.Assert(srcCrop.StepByteSize == src.StepByteSize);
            System.Diagnostics.Debug.Assert(dstCrop.StepByteSize == dst.StepByteSize);
            
            if (dstCrop.PixelFormat == srcCrop.PixelFormat)
            {   // no conversion required

                for (int y = 0; y < dstCrop.Height; ++y)
                {
                    var srcRow = srcCrop.GetScanlineBytes(y);
                    var dstRow = dstCrop.UseScanlineBytes(y);

                    System.Diagnostics.Debug.Assert(srcRow.Length == dstRow.Length);

                    srcRow.CopyTo(dstRow);
                }                
            }            
            
            else
            {   // conversion required

                var converter = Pixel.GetByteCopyConverter(srcCrop.PixelFormat, dstCrop.PixelFormat);

                for (int y = 0; y < dstCrop.Height; ++y)
                {
                    var srcRow = srcCrop.GetScanlineBytes(y);
                    var dstRow = dstCrop.UseScanlineBytes(y);
                    converter(srcRow, dstRow);
                }                
            }
        }

        public static void ConvertPixels<TSrcPixel, TDstPixel>(SpanBitmap<TDstPixel> dst, int dstX, int dstY, SpanBitmap<TSrcPixel> src)
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged
        {
            if (typeof(TSrcPixel) == typeof(Pixel.Alpha8)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.Alpha8>());
            if (typeof(TSrcPixel) == typeof(Pixel.Luminance8)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.Luminance8>());
            if (typeof(TSrcPixel) == typeof(Pixel.Luminance16)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.Luminance16>());
            if (typeof(TSrcPixel) == typeof(Pixel.Luminance32F)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.Luminance32F>());

            if (typeof(TSrcPixel) == typeof(Pixel.ARGB32)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.ARGB32>());

            if (typeof(TSrcPixel) == typeof(Pixel.BGR565)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.BGR565>());
            if (typeof(TSrcPixel) == typeof(Pixel.BGRA5551)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.BGRA5551>());
            if (typeof(TSrcPixel) == typeof(Pixel.BGRA4444)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.BGRA4444>());
            if (typeof(TSrcPixel) == typeof(Pixel.BGR24)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.BGR24>());
            if (typeof(TSrcPixel) == typeof(Pixel.BGRA32)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.BGRA32>());
            if (typeof(TSrcPixel) == typeof(Pixel.BGRP32)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.BGRP32>());
            if (typeof(TSrcPixel) == typeof(Pixel.BGR96F)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.BGR96F>());
            if (typeof(TSrcPixel) == typeof(Pixel.BGRA128F)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.BGRA128F>());

            if (typeof(TSrcPixel) == typeof(Pixel.RGB24)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.RGB24>());
            if (typeof(TSrcPixel) == typeof(Pixel.RGBA32)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.RGBA32>());
            if (typeof(TSrcPixel) == typeof(Pixel.RGBP32)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.RGBP32>());
            if (typeof(TSrcPixel) == typeof(Pixel.RGB96F)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.RGB96F>());
            if (typeof(TSrcPixel) == typeof(Pixel.RGBA128F)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.RGBA128F>());
            if (typeof(TSrcPixel) == typeof(Pixel.RGBP128F)) _ConvertPixels(dst, dstX, dstY, src.AsExplicit<Pixel.RGBP128F>());
        }

        internal static void _ConvertPixels<TSrcPixel, TDstPixel>(SpanBitmap<TDstPixel> dst, int dstX, int dstY, SpanBitmap<TSrcPixel> src)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
            where TDstPixel : unmanaged
        {
            var dstCrop = Crop(dst, (+dstX, +dstY, src.Width, src.Height));
            var srcCrop = Crop(src, (-dstX, -dstY, dst.Width, dst.Height));

            System.Diagnostics.Debug.Assert(dstCrop.Width == srcCrop.Width);
            System.Diagnostics.Debug.Assert(dstCrop.Height == srcCrop.Height);

            if (dstCrop.Width <= 0 || dstCrop.Height <= 0) return;

            for (int y = 0; y < dstCrop.Height; ++y)
            {
                var srcRow = srcCrop.GetScanlinePixels(y);
                var dstRow = dstCrop.UseScanlinePixels(y);

                System.Diagnostics.Debug.Assert(srcRow.Length == srcRow.Length);

                if (typeof(TSrcPixel) == typeof(TDstPixel))
                {
                    srcRow.CopyTo(System.Runtime.InteropServices.MemoryMarshal.Cast<TDstPixel, TSrcPixel>(dstRow));
                }
                else
                {
                    for (int x = 0; x < dstRow.Length; ++x)
                    {
                        srcRow[x].CopyTo(ref dstRow[x]);
                    }
                }
            }
        }
    }
}
