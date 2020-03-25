using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Adapters
{
    sealed unsafe class BitmapMemoryManager<T> : System.Buffers.MemoryManager<T>
        where T:unmanaged
    {
        #region lifecycle
        
        public BitmapMemoryManager(System.Drawing.Bitmap bmp)
        {
            var rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);

            _Bitmap = bmp;
            _BitmapData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

            _Info = _Implementation.GetBitmapInfo(_BitmapData);            
        }

        /// <summary>
        /// Releases all resources associated with this object
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _Bitmap?.UnlockBits(_BitmapData);
                _Bitmap = null;
                _BitmapData = default;
            }
        }

        #endregion

        #region data

        private BitmapInfo _Info;
        private System.Drawing.Bitmap _Bitmap;
        private System.Drawing.Imaging.BitmapData _BitmapData;

        #endregion

        #region API

        /// <summary>
        /// Obtains a span that represents the region
        /// </summary>
        public override Span<T> GetSpan()
        {
            var ptr = _BitmapData.Scan0.ToPointer();
            return new Span<T>(ptr, _Info.BitmapByteSize); // is in bytes, or in elements?
        }

        /// <summary>
        /// Provides access to a pointer that represents the data (note: no actual pin occurs)
        /// </summary>
        public override System.Buffers.MemoryHandle Pin(int elementIndex = 0)
        {
            if (elementIndex < 0 || elementIndex >= _Info.BitmapByteSize) throw new ArgumentOutOfRangeException(nameof(elementIndex));

            var ptr = (T*)_BitmapData.Scan0.ToPointer();

            return new System.Buffers.MemoryHandle(ptr + elementIndex, pinnable:this);
        }

        /// <summary>
        /// Has no effect
        /// </summary>
        public override void Unpin() { }

        #endregion
    }
}
