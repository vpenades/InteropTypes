using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropBitmaps
{
    // https://github.com/dotnet/wpf
    // https://github.com/dotnet/wpf/blob/master/src/Microsoft.DotNet.Wpf/src/PresentationCore/System/Windows/Media/Imaging/WriteableBitmap.cs    

    /// <summary>
    ///  Windows Imaging Component (WIC)
    /// </summary>
    public static partial class WIC
    {
        // BitmapSource -> WriteableBitmap, BitmapFrame, BitmapImage, CroppedBitmap, FormatConvertedBitmap,RenderTargetBitmap,TransformedBitmap        

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
            _Implementation.SetPixels(bmp, dstX, dstY, spanSrc);
        }        


        public static void SaveAsWMI(this SpanBitmap bmp, System.IO.Stream s,  System.Windows.Media.Imaging.BitmapEncoder encoder)
        {

        }


        
    }    
}
