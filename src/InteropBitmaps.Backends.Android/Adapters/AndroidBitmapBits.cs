using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropBitmaps.Adapters
{
    [System.Diagnostics.DebuggerDisplay("{ToDebuggerDisplayString(),nq}")]
    sealed class AndroidBitmapBits : MemoryManagers.BitmapMemoryManager
    {
        #region debug

        protected override string ToDebuggerDisplayString()
        {
            return $"Android.Graphics.Bitmap {base.ToDebuggerDisplayString()}";
        }

        #endregion

        public AndroidBitmapBits(Android.Graphics.Bitmap bmp)
        {
            var info = bmp.GetBitmapInfo().ToInterop();
            if (info.BitmapByteSize != bmp.ByteCount) throw new InvalidOperationException("Byte Size mismatch");

            var ptr = bmp.LockPixels();

            if (ptr == IntPtr.Zero)
            {
                bmp.UnlockPixels();
                throw new ArgumentNullException(nameof(bmp),"LockPixels Pointer is zero");
            }

            _AndroidBitmap = bmp;

            this.Initialize(new PointerBitmap(ptr, info, !bmp.IsMutable));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _AndroidBitmap?.UnlockPixels();
                _AndroidBitmap = null;
            }

            base.Dispose(disposing);            
        }

        private Android.Graphics.Bitmap _AndroidBitmap;        
    }
}
