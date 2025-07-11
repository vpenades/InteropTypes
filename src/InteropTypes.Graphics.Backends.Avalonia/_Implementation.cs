﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Platform;

using InteropTypes.Graphics.Bitmaps;

using AVABITMAP = Avalonia.Media.Imaging.Bitmap;
using AVARWBITMAP = Avalonia.Media.Imaging.WriteableBitmap;

using AVAPIXRECT = Avalonia.PixelRect;
using AVAPIXSIZE = Avalonia.PixelSize;
using AVAVECTOR2 = Avalonia.Vector;

namespace InteropTypes.Graphics.Backends
{
    partial class _Implementation
    {
        public static BitmapInfo GetBitmapInfo(AVABITMAP image)
        {
            if (TryGetBitmapInfo(image, out var binfo)) return binfo;
            throw new Diagnostics.PixelFormatNotSupportedException(image.GetType().Name, nameof(image));
        }

        public static bool TryGetBitmapInfo(AVABITMAP image, out BitmapInfo binfo)
        {
            if (image.Format == null || image.AlphaFormat == null)
            {
                binfo = default;
                return false;
            }

            var fmt = ToPixelFormatAlpha(image.Format ?? default, false);

            binfo = new BitmapInfo(image.PixelSize.Width, image.PixelSize.Height, fmt);
            return true;
        }

        public static MemoryBitmap ToMemoryBitmap(AVABITMAP src)
        {
            var avfmt = (src.Format, src.AlphaFormat);
            var dstFmt = _Implementation.ToPixelFormat(avfmt);
            var dst = new MemoryBitmap(src.PixelSize.Width, src.PixelSize.Height, dstFmt);
            CopyPixels(src,dst);
            return dst;
        }

        public static void CopyPixels(AVABITMAP src, MemoryBitmap dst)
        {
            if (src == null) return;

            // https://github.com/AvaloniaUI/Avalonia/issues/12169            

            var srcRect = new AVAPIXRECT(0, 0, src.PixelSize.Width, src.PixelSize.Height);

            void _copyPixels(PointerBitmap dstPtr)
            {
                src.CopyPixels(srcRect, dstPtr.Pointer, dstPtr.ByteSize, dstPtr.StepByteSize);
            }

            dst.AsSpanBitmap().PinWritablePointer(_copyPixels);
        }

        public static MemoryBitmap ToMemoryBitmap(AVARWBITMAP src)
        {
            if (src == null) return default;

            using (var srcLock = src.Lock())
            {
                return AsPointerBitmap(srcLock).AsSpanBitmap().ToMemoryBitmap();
            }
        }

        public static void CopyPixels(MemoryBitmap src, AVARWBITMAP dst)
        {
            if (src.IsEmpty) return;

            using (var dstLock = dst.Lock())
            {
                var dstPtr = AsPointerBitmap(dstLock);
                dstPtr.AsSpanBitmap().SetPixels(0, 0, src);
            }
        }

        public static void CopyPixels(SpanBitmap src, AVARWBITMAP dst)
        {
            if (src.IsEmpty) return;

            using (var dstLock = dst.Lock())
            {
                var dstPtr = AsPointerBitmap(dstLock);
                dstPtr.AsSpanBitmap().SetPixels(0, 0, src);
            }
        }

        public static PointerBitmap AsPointerBitmap(ILockedFramebuffer frameBuffer)
        {
            if (frameBuffer == null) return default;

            var dstFmt = ToPixelFormatAlpha(frameBuffer.Format);
            var bmpInfo = new BitmapInfo(frameBuffer.Size.Width, frameBuffer.Size.Height, dstFmt, frameBuffer.RowBytes);
            return new PointerBitmap(frameBuffer.Address, bmpInfo);
        }

        public static AVABITMAP CreateAvaloniaBitmap(SpanBitmap src)
        {
            if (src.IsEmpty) return null;

            return src.PinReadablePointer(CreateAvaloniaBitmap);            
        }

        public static AVABITMAP CreateAvaloniaBitmap(PointerBitmap src)
        {
            if (src.IsEmpty) return null;

            var (color, alpha) = ToPixelFormat(src.PixelFormat);

            return new AVABITMAP(color, alpha, src.Pointer, new AVAPIXSIZE(src.Width, src.Height), new AVAVECTOR2(96, 96), src.StepByteSize);
        }
    }
}
