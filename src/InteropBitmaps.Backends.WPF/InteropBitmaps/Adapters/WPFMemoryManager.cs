using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropBitmaps;

namespace InteropTypes.Graphics.Backends
{
    sealed class WPFMemoryManager : InteropBitmaps.MemoryManagers.BitmapMemoryManager
    {
        #region lifecycle

        public WPFMemoryManager(System.Windows.Media.Imaging.WriteableBitmap bmp)
        {
            bmp.Lock();

            if (!_Implementation.TryGetExactPixelFormat(bmp.Format, out var bmpFmt))
            {
                throw new InteropBitmaps.Diagnostics.PixelFormatNotSupportedException(bmp.Format, nameof(bmp));
            }
            
            var nfo = new BitmapInfo(bmp.PixelWidth, bmp.PixelHeight, bmpFmt, bmp.BackBufferStride);
            var ptr = new PointerBitmap(bmp.BackBuffer, nfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap.adddirtyrect?view=netframework-4.8

                if (_BitmapSource != null)
                {
                    var rect = new System.Windows.Int32Rect(0, 0, _BitmapSource.PixelWidth, _BitmapSource.PixelHeight);
                    _BitmapSource.AddDirtyRect(rect);
                    _BitmapSource.Unlock();
                    _BitmapSource = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region data

        private System.Windows.Media.Imaging.WriteableBitmap _BitmapSource;

        #endregion
    }
}
