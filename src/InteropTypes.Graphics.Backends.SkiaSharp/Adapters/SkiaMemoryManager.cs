using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Adapters
{
    class SkiaMemoryManager : Bitmaps.MemoryManagers.BitmapMemoryManager
    {
        #region lifecycle

        public SkiaMemoryManager(SkiaSharp.SKBitmap bmp)
        {
            _BitmapSource = bmp;
            _BitmapInfo = _Implementation.GetBitmapInfo(bmp.Info, bmp.RowBytes);

            var ptr = bmp.GetPixels();
            if (ptr == IntPtr.Zero) throw new ArgumentNullException();

            Initialize(new PointerBitmap(ptr, _BitmapInfo));
        }

        /// <summary>
        /// Releases all resources associated with this object
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _BitmapSource?.NotifyPixelsChanged();
                _BitmapSource = null;
            }

            base.Dispose(disposing);
        }        

        #endregion

        #region data

        private SkiaSharp.SKBitmap _BitmapSource;        

        private BitmapInfo _BitmapInfo;

        #endregion
    }
}
