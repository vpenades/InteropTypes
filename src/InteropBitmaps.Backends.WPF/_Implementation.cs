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

using STDPIXEL = InteropBitmaps.Pixel.Standard;
using PACKPIXEL = InteropBitmaps.Pixel.Packed;

namespace InteropBitmaps
{    
    static class _Implementation
    {
        #region pixel formats

        public static INTEROPFMT ToInterop(System.Windows.Media.PixelFormat fmt)
        {
            // if (fmt == WIC_FORMATS.Default) return PixelFormat.GetUndefinedOfSize(fmt.BitsPerPixel / 8);

            if (fmt == WIC_FORMATS.Gray8) return STDPIXEL.Gray8;

            if (fmt == WIC_FORMATS.Gray16) return STDPIXEL.Gray16;
            if (fmt == WIC_FORMATS.Bgr555) return STDPIXEL.BGRA5551;
            if (fmt == WIC_FORMATS.Bgr565) return STDPIXEL.BGR565;

            if (fmt == WIC_FORMATS.Bgr24) return STDPIXEL.BGR24;
            if (fmt == WIC_FORMATS.Rgb24) return STDPIXEL.RGB24;

            if (fmt == WIC_FORMATS.Bgr32) return STDPIXEL.BGRA32;
            if (fmt == WIC_FORMATS.Bgra32) return STDPIXEL.BGRA32;

            if (fmt == WIC_FORMATS.Pbgra32) return STDPIXEL.BGRA32; // NOT RIGHT


            if (fmt == WIC_FORMATS.Rgba128Float) return STDPIXEL.RGBA128F;

            throw new NotImplementedException();
        }

        public static System.Windows.Media.PixelFormat ToPixelFormat(INTEROPFMT fmt)
        {
            switch (fmt.PackedFormat)
            {
                case PACKPIXEL.Gray8: return WIC_FORMATS.Gray8;
                case PACKPIXEL.Gray16: return WIC_FORMATS.Gray16;

                case PACKPIXEL.BGRA5551: return WIC_FORMATS.Bgr555;
                case PACKPIXEL.BGR565: return WIC_FORMATS.Bgr565;

                case PACKPIXEL.BGR24: return WIC_FORMATS.Bgr24;
                case PACKPIXEL.RGB24: return WIC_FORMATS.Rgb24;

                case PACKPIXEL.BGRA32: return WIC_FORMATS.Bgra32;

                case PACKPIXEL.RGBA128F: return WIC_FORMATS.Rgba128Float;

                default: throw new NotImplementedException();
            }
        }

        public static System.Windows.Media.PixelFormat ToBestMatch(INTEROPFMT fmt)
        {
            switch (fmt.PackedFormat)
            {
                case PACKPIXEL.Gray8: return WIC_FORMATS.Gray8;
                case PACKPIXEL.Gray16: return WIC_FORMATS.Gray16;

                case PACKPIXEL.BGRA4444: return WIC_FORMATS.Bgra32;
                case PACKPIXEL.BGRA5551: return WIC_FORMATS.Bgra32;
                case PACKPIXEL.BGR565: return WIC_FORMATS.Bgr24;

                case PACKPIXEL.BGR24: return WIC_FORMATS.Bgr24;
                case PACKPIXEL.RGB24: return WIC_FORMATS.Rgb24;

                case PACKPIXEL.BGRA32: return WIC_FORMATS.Bgra32;
                case PACKPIXEL.RGBA32: return WIC_FORMATS.Bgra32;
                case PACKPIXEL.ARGB32: return WIC_FORMATS.Bgra32;

                case PACKPIXEL.BGR96F: return WIC_FORMATS.Rgba128Float;
                case PACKPIXEL.RGBA128F: return WIC_FORMATS.Rgba128Float;

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

            src.CopyPixels(dst.ToByteArray(), binfo.StepByteSize, 0);

            return dst;
        }

        public static WIC_WRITABLE ToWritableBitmap(BitmapInfo info)
        {
            var fmt = ToBestMatch(info.PixelFormat);

            return  new WIC_WRITABLE(info.Width, info.Height, 96, 96, fmt, null);
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
