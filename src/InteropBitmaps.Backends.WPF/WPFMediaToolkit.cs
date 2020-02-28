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

        public static System.Windows.Media.PixelFormat ToWMIFormat(this PixelFormat fmt)
        {
            switch(fmt.PackedFormat)
            {
                case PixelFormat.Packed.GRAY8: return System.Windows.Media.PixelFormats.Gray8;

                case PixelFormat.Packed.GRAY16: return System.Windows.Media.PixelFormats.Gray16;
                case PixelFormat.Packed.BGRA5551: return System.Windows.Media.PixelFormats.Bgr555;
                case PixelFormat.Packed.BGR565: return System.Windows.Media.PixelFormats.Bgr565;

                case PixelFormat.Packed.BGR24: return System.Windows.Media.PixelFormats.Bgr24;
                case PixelFormat.Packed.RGB24: return System.Windows.Media.PixelFormats.Rgb24;

                case PixelFormat.Packed.BGRA32: return System.Windows.Media.PixelFormats.Bgra32;

                default: throw new NotImplementedException();
            }
        }

        public static BitmapInfo GetBitmapInfo(this System.Windows.Media.Imaging.BitmapSource src)
        {
            if (src == null) throw new ArgumentNullException();
            if (src.IsDownloading) throw new NotImplementedException();

            var byteStride = src is System.Windows.Media.Imaging.WriteableBitmap wbmp ? wbmp.BackBufferStride : 0;

            var pfmt = src.Format.ToInteropFormat();
            return new BitmapInfo(src.PixelWidth, src.PixelHeight, pfmt, byteStride);
        }        

        public static MemoryBitmap ToMemoryBitmap(this System.Windows.Media.Imaging.BitmapSource src)
        {
            var binfo = src.GetBitmapInfo();

            var dst = new MemoryBitmap(binfo);

            src.CopyPixels(dst.ToArray(), binfo.ScanlineSize, 0);

            return dst;            
        }

        public static void Mutate(this System.Windows.Media.Imaging.WriteableBitmap bmp, Func<PointerBitmap, Boolean> onMutate)
        {
            var pfmt = bmp.Format.ToInteropFormat();

            // // https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap.adddirtyrect?view=netframework-4.8

            try
            {
                // Reserve the back buffer for updates.
                bmp.TryLock(System.Windows.Duration.Forever);

                var binfo = new BitmapInfo(bmp.PixelWidth, bmp.PixelHeight, pfmt, bmp.BackBufferStride);

                var changed = onMutate((bmp.BackBuffer, binfo));

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

        public static void SetPixels(this System.Windows.Media.Imaging.WriteableBitmap bmp, int dstX, int dstY, SpanBitmap spanSrc)
        {
            var pfmt = bmp.Format.ToInteropFormat();

            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap.adddirtyrect?view=netframework-4.8

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


        public static void SaveAsWMI(this SpanBitmap bmp, System.IO.Stream s,  System.Windows.Media.Imaging.BitmapEncoder encoder)
        {

        }


        public static void SaveAsWPF(this System.Windows.Media.Imaging.BitmapSource src, System.IO.Stream s)
        {
            System.Windows.Media.Imaging.BitmapEncoder.Create(Guid.NewGuid());

            var enc = new System.Windows.Media.Imaging.BmpBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(src));
            enc.Save(s);            
        }
    }    
}
