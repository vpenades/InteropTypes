using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropBitmaps
{
    // https://github.com/dotnet/wpf
    // https://github.com/dotnet/wpf/blob/master/src/Microsoft.DotNet.Wpf/src/PresentationCore/System/Windows/Media/Imaging/WriteableBitmap.cs

    public static class WPFMediaToolkit
    {
        // BitmapSource -> WriteableBitmap, BitmapFrame, BitmapImage, CroppedBitmap, FormatConvertedBitmap,RenderTargetBitmap,TransformedBitmap
        
        public static PixelFormat ToInteropFormat(this System.Windows.Media.PixelFormat fmt)
        {
            // if (fmt == System.Windows.Media.PixelFormats.Default) return PixelFormat.GetUndefinedOfSize(fmt.BitsPerPixel / 8);
            
            if (fmt == System.Windows.Media.PixelFormats.Gray8) return PixelFormat.Standard.GRAY8;

            if (fmt == System.Windows.Media.PixelFormats.Gray16) return PixelFormat.Standard.GRAY16;

            if (fmt == System.Windows.Media.PixelFormats.Bgr555) return PixelFormat.Standard.BGRA5551;
            if (fmt == System.Windows.Media.PixelFormats.Bgr565) return PixelFormat.Standard.BGR565;

            if (fmt == System.Windows.Media.PixelFormats.Bgr24) return PixelFormat.Standard.BGR24;
            if (fmt == System.Windows.Media.PixelFormats.Rgb24) return PixelFormat.Standard.RGB24;

            if (fmt == System.Windows.Media.PixelFormats.Bgr32) return PixelFormat.Standard.BGRA32;
            if (fmt == System.Windows.Media.PixelFormats.Bgra32) return PixelFormat.Standard.BGRA32;

            throw new NotImplementedException();
        }

        public static BitmapInfo GetBitmapInfo(this System.Windows.Media.Imaging.BitmapSource src)
        {
            var byteStride = src is System.Windows.Media.Imaging.WriteableBitmap wbmp ? wbmp.BackBufferStride : 0;

            var pfmt = src.Format.ToInteropFormat();
            return new BitmapInfo(src.PixelWidth, src.PixelHeight, pfmt, byteStride);
        }

        public static SpanBitmap AsSpanBitmap(this System.Windows.Media.Imaging.WriteableBitmap src)
        {            
            var info = src.GetBitmapInfo();
            return new SpanBitmap(src.BackBuffer, info);
        }

        public static MemoryBitmap ToMemoryBitmap(this System.Windows.Media.Imaging.BitmapImage src)
        {
            if (src.IsDownloading) throw new NotImplementedException();

            var binfo = src.GetBitmapInfo();
            var dst = new MemoryBitmap(binfo);

            src.CopyPixels(dst.ToArray(), binfo.ScanlineSize, 0);

            return dst;            
        }
    }

    public struct WritableBitmapData : IDisposable
    {
        internal WritableBitmapData(System.Windows.Media.Imaging.WriteableBitmap source)
        {
            _Source = source;
            _Binfo = _Source.GetBitmapInfo();
        }

        public void Dispose()
        {
            _Source?.Unlock();
            _Source = null;
            _Binfo = default(BitmapInfo);
        }

        private System.Windows.Media.Imaging.WriteableBitmap _Source;
        private BitmapInfo _Binfo;

        public SpanBitmap Span => new SpanBitmap(_Source.BackBuffer, _Binfo);
    }
}
