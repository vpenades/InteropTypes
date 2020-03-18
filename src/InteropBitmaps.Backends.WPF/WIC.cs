using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropBitmaps
{
    

    /// <summary>
    ///  Windows Imaging Component (WIC)
    /// </summary>
    public static partial class WPF
    {
        // BitmapSource -> WriteableBitmap, BitmapFrame, BitmapImage, CroppedBitmap, FormatConvertedBitmap,RenderTargetBitmap,TransformedBitmap        

        public static BitmapInfo GetBitmapInfo(this System.Windows.Media.Imaging.BitmapSource src)
        {
            return _Implementation.GetBitmapInfo(src);
        }        

        public static MemoryBitmap ToMemoryBitmap(this System.Windows.Media.Imaging.BitmapSource src)
        {
            return _Implementation.ToMemoryBitmap(src);
        }

        public static void Mutate(this System.Windows.Media.Imaging.WriteableBitmap bmp, Func<PointerBitmap, Boolean> onMutate)
        {
            var pfmt = _Implementation.ToInterop(bmp.Format);

            // // https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap.adddirtyrect?view=netframework-4.8

            try
            {
                // Reserve the back buffer for updates.
                bmp.TryLock(System.Windows.Duration.Forever);

                var binfo = new BitmapInfo(bmp.PixelWidth, bmp.PixelHeight, pfmt, bmp.BackBufferStride);

                var changed = onMutate(new PointerBitmap(bmp.BackBuffer, binfo));

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
    }    
}
