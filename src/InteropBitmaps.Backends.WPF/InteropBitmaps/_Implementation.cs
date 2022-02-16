using System;
using System.Collections.Generic;
using System.Linq;

// https://github.com/dotnet/wpf
// https://github.com/dotnet/wpf/blob/master/src/Microsoft.DotNet.Wpf/src/PresentationCore/System/Windows/Media/Imaging/WriteableBitmap.cs    

// SharpDX uses WIC directly:
// http://sharpdx.org/wiki/class-library-api/wic/
// https://docs.microsoft.com/es-es/windows/win32/wic/-wic-lh?redirectedfrom=MSDN

using WIC_WRITABLE = System.Windows.Media.Imaging.WriteableBitmap;
using WIC_READABLE = System.Windows.Media.Imaging.BitmapSource;

using InteropBitmaps;
using InteropTypes.Graphics.Backends;

namespace InteropTypes.Graphics
{    
    static partial class _Implementation
    {
        #region blit

        public static BitmapInfo GetBitmapInfo(WIC_READABLE src)
        {
            if (src == null) throw new ArgumentNullException();
            if (src.IsDownloading) throw new NotImplementedException();

            var byteStride = src is WIC_WRITABLE wbmp ? wbmp.BackBufferStride : 0;

            return TryGetExactPixelFormat(src.Format, out var fmt)
                ? new BitmapInfo(src.PixelWidth, src.PixelHeight, fmt, byteStride)
                : throw new InteropBitmaps.Diagnostics.PixelFormatNotSupportedException(src.Format, nameof(fmt));
        }

        public static void SetPixels(WIC_WRITABLE dstBmp, int dstX, int dstY, SpanBitmap srcSpan)
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap.adddirtyrect?view=netframework-4.8

            srcSpan.PinReadablePointer(ptr => SetPixels(dstBmp, dstX, dstY, ptr));
        }

        public static void SetPixels(WIC_WRITABLE dstBmp, int dstX, int dstY, PointerBitmap srcPtr)
        {
            if (dstX < 0) throw new ArgumentOutOfRangeException(nameof(dstX));
            if (dstY < 0) throw new ArgumentOutOfRangeException(nameof(dstY));
            if (dstX + srcPtr.Width > dstBmp.PixelWidth) throw new ArgumentOutOfRangeException(nameof(srcPtr.Width));
            if (dstY + srcPtr.Height > dstBmp.PixelHeight) throw new ArgumentOutOfRangeException(nameof(srcPtr.Height));

            if (!TryGetExactPixelFormat(dstBmp.Format, out var dstFmt))
            {
                throw new InteropBitmaps.Diagnostics.PixelFormatNotSupportedException(dstBmp.Format, nameof(dstBmp));
            }

            if (srcPtr.PixelFormat == dstFmt)
            {
                var rect = new System.Windows.Int32Rect(dstX, dstY, dstBmp.PixelWidth, dstBmp.PixelHeight);

                dstBmp.WritePixels(rect, srcPtr.Pointer, srcPtr.Info.BitmapByteSize, srcPtr.Info.StepByteSize);
                return;
            }
            
            try
            {
                dstBmp.Lock();
                
                var nfo = new BitmapInfo(dstBmp.PixelWidth, dstBmp.PixelHeight, dstFmt, dstBmp.BackBufferStride);
                var dstPtr = new PointerBitmap(dstBmp.BackBuffer, nfo);

                dstPtr.AsSpanBitmap().SetPixels(0, 0, srcPtr.AsSpanBitmap());

            }
            finally
            {
                dstBmp.Unlock();
            }

            
        }

        public static MemoryBitmap ToMemoryBitmap(WIC_READABLE src)
        {
            var binfo = src.GetBitmapInfo();

            var dst = new MemoryBitmap(binfo);

            src.CopyPixels(dst.ToByteArray(), binfo.StepByteSize, 0);

            return dst;
        }

        public static WIC_WRITABLE ToWritableBitmap(BitmapInfo info)
        {
            var fmt = GetCompatiblePixelFormat(info.PixelFormat);

            return new WIC_WRITABLE(info.Width, info.Height, 96, 96, fmt, null);
        }

        public static WIC_WRITABLE ToWritableBitmap(SpanBitmap src)
        {
            var dst = ToWritableBitmap(src.Info);            
            dst.SetPixels(0, 0, src);
            return dst;
        }        

        #endregion          
    }
}
