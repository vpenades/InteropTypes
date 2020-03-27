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

using INTEROPFMT = InteropBitmaps.PixelFormat;

namespace InteropBitmaps
{    
    static class _Implementation
    {
        #region pixel formats

        public static INTEROPFMT ToInterop(System.Windows.Media.PixelFormat fmt)
        {
            // if (fmt == System.Windows.Media.PixelFormats.Default) return PixelFormat.GetUndefinedOfSize(fmt.BitsPerPixel / 8);

            if (fmt == System.Windows.Media.PixelFormats.Gray8) return INTEROPFMT.Standard.Gray8;

            if (fmt == System.Windows.Media.PixelFormats.Gray16) return INTEROPFMT.Standard.Gray16;
            if (fmt == System.Windows.Media.PixelFormats.Bgr555) return INTEROPFMT.Standard.BGRA5551;
            if (fmt == System.Windows.Media.PixelFormats.Bgr565) return INTEROPFMT.Standard.BGR565;

            if (fmt == System.Windows.Media.PixelFormats.Bgr24) return INTEROPFMT.Standard.BGR24;
            if (fmt == System.Windows.Media.PixelFormats.Rgb24) return INTEROPFMT.Standard.RGB24;

            if (fmt == System.Windows.Media.PixelFormats.Bgr32) return INTEROPFMT.Standard.BGRA32;
            if (fmt == System.Windows.Media.PixelFormats.Bgra32) return INTEROPFMT.Standard.BGRA32;

            if (fmt == System.Windows.Media.PixelFormats.Pbgra32) return INTEROPFMT.Standard.BGRA32; // NOT RIGHT


            if (fmt == System.Windows.Media.PixelFormats.Rgba128Float) return INTEROPFMT.Standard.RGBA128F;

            throw new NotImplementedException();
        }

        public static System.Windows.Media.PixelFormat ToPixelFormat(INTEROPFMT fmt)
        {
            switch (fmt.PackedFormat)
            {
                case INTEROPFMT.Packed.Gray8: return System.Windows.Media.PixelFormats.Gray8;
                case INTEROPFMT.Packed.Gray16: return System.Windows.Media.PixelFormats.Gray16;

                case INTEROPFMT.Packed.BGRA5551: return System.Windows.Media.PixelFormats.Bgr555;
                case INTEROPFMT.Packed.BGR565: return System.Windows.Media.PixelFormats.Bgr565;

                case INTEROPFMT.Packed.BGR24: return System.Windows.Media.PixelFormats.Bgr24;
                case INTEROPFMT.Packed.RGB24: return System.Windows.Media.PixelFormats.Rgb24;

                case INTEROPFMT.Packed.BGRA32: return System.Windows.Media.PixelFormats.Bgra32;

                case INTEROPFMT.Packed.RGBA128F: return System.Windows.Media.PixelFormats.Rgba128Float;

                default: throw new NotImplementedException();
            }
        }

        public static System.Windows.Media.PixelFormat ToBestMatch(INTEROPFMT fmt)
        {
            switch (fmt.PackedFormat)
            {
                case INTEROPFMT.Packed.Gray8: return System.Windows.Media.PixelFormats.Gray8;
                case INTEROPFMT.Packed.Gray16: return System.Windows.Media.PixelFormats.Gray16;

                case INTEROPFMT.Packed.BGRA4444: return System.Windows.Media.PixelFormats.Bgra32;
                case INTEROPFMT.Packed.BGRA5551: return System.Windows.Media.PixelFormats.Bgra32;
                case INTEROPFMT.Packed.BGR565: return System.Windows.Media.PixelFormats.Bgr24;

                case INTEROPFMT.Packed.BGR24: return System.Windows.Media.PixelFormats.Bgr24;
                case INTEROPFMT.Packed.RGB24: return System.Windows.Media.PixelFormats.Rgb24;

                case INTEROPFMT.Packed.BGRA32: return System.Windows.Media.PixelFormats.Bgra32;
                case INTEROPFMT.Packed.RGBA32: return System.Windows.Media.PixelFormats.Bgra32;
                case INTEROPFMT.Packed.ARGB32: return System.Windows.Media.PixelFormats.Bgra32;

                case INTEROPFMT.Packed.RGBA128F: return System.Windows.Media.PixelFormats.Rgba128Float;

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

            var pfmt = _Implementation.ToInterop(src.Format);
            return new BitmapInfo(src.PixelWidth, src.PixelHeight, pfmt, byteStride);
        }

        public static void SetPixels(WIC_WRITABLE bmp, int dstX, int dstY, SpanBitmap spanSrc)
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap.adddirtyrect?view=netframework-4.8

            var pfmt = ToInterop(bmp.Format);

            try
            {
                // Reserve the back buffer for updates.
                bmp.TryLock(System.Windows.Duration.Forever);

                var binfo = new BitmapInfo(bmp.PixelWidth, bmp.PixelHeight, pfmt, bmp.BackBufferStride);
                var spanDst = new SpanBitmap(bmp.BackBuffer, binfo);

                spanDst.SetPixels(dstX, dstY, spanSrc);

                var changed = true;

                // Specify the area of the bitmap that changed.
                if (changed)
                {
                    var rect = new System.Windows.Int32Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight);
                    bmp.AddDirtyRect(rect);
                }
            }
            finally
            {
                // Release the back buffer and make it available for display.
                bmp.Unlock();
            }
        }

        public static MemoryBitmap ToMemoryBitmap(WIC_READABLE src)
        {
            var binfo = src.GetBitmapInfo();

            var dst = new MemoryBitmap(binfo);

            src.CopyPixels(dst.ToArray(), binfo.ScanlineByteSize, 0);

            return dst;
        }

        public static WIC_WRITABLE ToWritableBitmap(SpanBitmap src)
        {
            var fmt = ToBestMatch(src.PixelFormat);

            var dst = new WIC_WRITABLE(src.Width, src.Height, 96, 96, fmt, null);
            dst.SetPixels(0, 0, src);

            return dst;
        }

        

        #endregion          
    }
}
