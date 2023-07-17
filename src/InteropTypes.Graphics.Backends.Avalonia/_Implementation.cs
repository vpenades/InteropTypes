using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Platform;

using InteropTypes.Graphics.Bitmaps;

using AVALONIABMP = Avalonia.Media.Imaging.Bitmap;

namespace InteropTypes.Graphics.Backends
{
    partial class _Implementation
    {
        public static BitmapInfo GetBitmapInfo(AVALONIABMP image)
        {
            if (TryGetBitmapInfo(image, out var binfo)) return binfo;
            throw new Diagnostics.PixelFormatNotSupportedException(image.GetType().Name, nameof(image));
        }

        public static bool TryGetBitmapInfo(AVALONIABMP image, out BitmapInfo binfo)
        {
            var fmt = ToPixelFormatAlpha(image.Format ?? default, false);

            binfo = new BitmapInfo(image.PixelSize.Width, image.PixelSize.Height, fmt);
            return true;
        }

        public static void CopyPixels(AVALONIABMP src,  MemoryBitmap dst)
        {
            // https://github.com/AvaloniaUI/Avalonia/issues/12169            

            var srcRect = new Avalonia.PixelRect(0, 0, src.PixelSize.Width, src.PixelSize.Height);

            void _copyPixels(PointerBitmap dstPtr)
            {
                src.CopyPixels(srcRect, dstPtr.Pointer, dstPtr.ByteSize, dstPtr.StepByteSize);
            }

            dst.AsSpanBitmap().PinWritablePointer(_copyPixels);
        }

        public static void CopyPixels(MemoryBitmap src, Avalonia.Media.Imaging.WriteableBitmap dst)
        {
            using (var dstLock = dst.Lock())
            {
                var dstPtr = AsPointerBitmap(dstLock);
                dstPtr.AsSpanBitmap().SetPixels(0, 0, src);
            }
        }

        public static void CopyPixels(SpanBitmap src, Avalonia.Media.Imaging.WriteableBitmap dst)
        {
            using (var dstLock = dst.Lock())
            {
                var dstPtr = AsPointerBitmap(dstLock);
                dstPtr.AsSpanBitmap().SetPixels(0, 0, src);
            }
        }

        public static PointerBitmap AsPointerBitmap(ILockedFramebuffer frameBuffer)
        {
            var dstFmt = ToPixelFormatAlpha(frameBuffer.Format);
            var bmpInfo = new BitmapInfo(frameBuffer.Size.Width, frameBuffer.Size.Height, dstFmt, frameBuffer.RowBytes);
            return new PointerBitmap(frameBuffer.Address, bmpInfo);
        }

        public static AVALONIABMP CreateAvaloniaBitmap(SpanBitmap src)
        {
            return src.PinReadablePointer(CreateAvaloniaBitmap);            
        }

        public static AVALONIABMP CreateAvaloniaBitmap(PointerBitmap src)
        {
            var (color, alpha) = ToPixelFormat(src.PixelFormat);

            return new AVALONIABMP(color, alpha, src.Pointer, new Avalonia.PixelSize(src.Width, src.Height), new Avalonia.Vector(96, 96), src.StepByteSize);
        }
    }
}
