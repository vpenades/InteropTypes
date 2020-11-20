using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://github.com/dotnet/wpf
// https://github.com/dotnet/wpf/blob/master/src/Microsoft.DotNet.Wpf/src/PresentationCore/System/Windows/Media/Imaging/WriteableBitmap.cs    

// SharpDX uses WIC directly:
// http://sharpdx.org/wiki/class-library-api/wic/
// https://docs.microsoft.com/es-es/windows/win32/wic/-wic-lh?redirectedfrom=MSDN

using WIC_WRITABLE = System.Windows.Media.Imaging.WriteableBitmap;
using WIC_READABLE = System.Windows.Media.Imaging.BitmapSource;
using WIC_ENCODER = System.Windows.Media.Imaging.BitmapEncoder;
using WIC_FRAME = System.Windows.Media.Imaging.BitmapFrame;
using WIC_FORMATS = System.Windows.Media.PixelFormats;

using INTEROPFMT = InteropBitmaps.Pixel.Format;

namespace InteropBitmaps
{    
    static class _Implementation
    {
        #region pixel formats

        public static INTEROPFMT ToInterop(System.Windows.Media.PixelFormat fmt)
        {
            // if (fmt == WIC_FORMATS.Default) return PixelFormat.GetUndefinedOfSize(fmt.BitsPerPixel / 8);

            if (fmt == WIC_FORMATS.Gray8) return Pixel.Luminance8.Format;

            if (fmt == WIC_FORMATS.Gray16) return Pixel.Luminance16.Format;
            if (fmt == WIC_FORMATS.Bgr555) return Pixel.BGRA5551.Format;
            if (fmt == WIC_FORMATS.Bgr565) return Pixel.BGR565.Format;

            if (fmt == WIC_FORMATS.Bgr24) return Pixel.BGR24.Format;
            if (fmt == WIC_FORMATS.Rgb24) return Pixel.RGB24.Format;

            if (fmt == WIC_FORMATS.Bgr32) return Pixel.BGRA32.Format;
            if (fmt == WIC_FORMATS.Bgra32) return Pixel.BGRA32.Format;

            if (fmt == WIC_FORMATS.Pbgra32) return Pixel.BGRA32.Format; // NOT RIGHT


            if (fmt == WIC_FORMATS.Rgba128Float) return Pixel.VectorRGBA.Format;

            throw new NotImplementedException();
        }

        public static System.Windows.Media.PixelFormat ToPixelFormat(INTEROPFMT fmt)
        {
            switch (fmt.PackedFormat)
            {
                case Pixel.Luminance8.Code: return WIC_FORMATS.Gray8;
                case Pixel.Luminance16.Code: return WIC_FORMATS.Gray16;

                case Pixel.BGRA5551.Code: return WIC_FORMATS.Bgr555;
                case Pixel.BGR565.Code: return WIC_FORMATS.Bgr565;

                case Pixel.BGR24.Code: return WIC_FORMATS.Bgr24;
                case Pixel.RGB24.Code: return WIC_FORMATS.Rgb24;

                case Pixel.BGRA32.Code: return WIC_FORMATS.Bgra32;

                case Pixel.VectorRGBA.Code: return WIC_FORMATS.Rgba128Float;

                default: throw new NotImplementedException();
            }
        }

        public static System.Windows.Media.PixelFormat ToBestMatch(INTEROPFMT fmt)
        {
            switch (fmt.PackedFormat)
            {
                case Pixel.Luminance8.Code: return WIC_FORMATS.Gray8;
                case Pixel.Luminance16.Code: return WIC_FORMATS.Gray16;

                case Pixel.BGRA4444.Code: return WIC_FORMATS.Bgra32;
                case Pixel.BGRA5551.Code: return WIC_FORMATS.Bgra32;
                case Pixel.BGR565.Code: return WIC_FORMATS.Bgr24;

                case Pixel.BGR24.Code: return WIC_FORMATS.Bgr24;
                case Pixel.RGB24.Code: return WIC_FORMATS.Rgb24;

                case Pixel.BGRA32.Code: return WIC_FORMATS.Bgra32;
                case Pixel.RGBA32.Code: return WIC_FORMATS.Bgra32;
                case Pixel.ARGB32.Code: return WIC_FORMATS.Bgra32;

                case Pixel.VectorBGR.Code: return WIC_FORMATS.Rgba128Float;
                case Pixel.VectorBGRA.Code: return WIC_FORMATS.Rgba128Float;
                case Pixel.VectorRGBA.Code: return WIC_FORMATS.Rgba128Float;

                default: throw new NotImplementedException();
            }
        }

        #endregion

        #region blit

        public static BitmapInfo GetBitmapInfo(WIC_READABLE src)
        {
            if (src == null) throw new ArgumentNullException();
            if (src.IsDownloading) throw new NotImplementedException();

            var byteStride = src is WIC_WRITABLE wbmp ? wbmp.BackBufferStride : 0;

            var pfmt = ToInterop(src.Format);
            return new BitmapInfo(src.PixelWidth, src.PixelHeight, pfmt, byteStride);
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

            if (srcPtr.PixelFormat == ToInterop(dstBmp.Format))
            {
                var rect = new System.Windows.Int32Rect(dstX, dstY, dstBmp.PixelWidth, dstBmp.PixelHeight);

                dstBmp.WritePixels(rect, srcPtr.Pointer, srcPtr.Info.BitmapByteSize, srcPtr.Info.StepByteSize);
                return;
            }
            
            try
            {
                dstBmp.Lock();

                var fmt = ToInterop(dstBmp.Format);
                var nfo = new BitmapInfo(dstBmp.PixelWidth, dstBmp.PixelHeight, fmt, dstBmp.BackBufferStride);
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
            var fmt = ToBestMatch(info.PixelFormat);

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
