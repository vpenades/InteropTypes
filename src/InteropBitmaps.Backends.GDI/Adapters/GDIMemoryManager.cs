using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Adapters
{
    /// <summary>
    /// Helper class that wraps a <see cref="System.Drawing.Bitmap"/> to expose it as a <see cref="MemoryBitmap"/>
    /// </summary>
    sealed unsafe class GDIMemoryManager : MemoryManagers.BitmapMemoryManager
    {
        #region lifecycle
        
        public GDIMemoryManager(System.Drawing.Bitmap bmp)
        {
            var rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);

            _BitmapSource = bmp;
            _BitmapData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

            var info = _Implementation.GetBitmapInfo(_BitmapData);

            System.Diagnostics.Debug.Assert(info.StepByteSize == _BitmapData.Stride);

            var ptr = new PointerBitmap(_BitmapData.Scan0, info);

            Initialize(ptr);
        }

        /// <summary>
        /// Releases all resources associated with this object
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _BitmapSource?.UnlockBits(_BitmapData);
                _BitmapSource = null;
                _BitmapData = default;
            }

            _BitmapInfo = default;

            base.Dispose(disposing);
        }

        #endregion

        #region data
        
        private System.Drawing.Bitmap _BitmapSource;
        private System.Drawing.Imaging.BitmapData _BitmapData;

        private BitmapInfo _BitmapInfo;        

        #endregion        
    }
}
