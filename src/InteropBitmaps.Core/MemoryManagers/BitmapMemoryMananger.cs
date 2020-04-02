using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.MemoryManagers
{
    /// <summary>
    /// Helper class that wraps a <see cref="PointerBitmap"/> to expose it as a <see cref="MemoryBitmap"/>
    /// </summary>
    public abstract unsafe class BitmapMemoryManager
        : System.Buffers.MemoryManager<Byte>
        , IMemoryBitmapOwner
        , IPointerBitmapOwner
    {
        #region lifecycle

        protected void Initialize(PointerBitmap ptrbmp)
        {
            _PointerBitmap = ptrbmp;
        }

        /// <summary>
        /// Releases all resources associated with this object
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            _PointerBitmap = default;            
        }

        #endregion

        #region data

        private PointerBitmap _PointerBitmap;

        #endregion

        #region properties        

        PointerBitmap IPointerBitmapOwner.Bitmap => _PointerBitmap;

        MemoryBitmap IMemoryBitmapOwner.Bitmap => new MemoryBitmap(this.Memory, _PointerBitmap.Info);

        #endregion

        #region API

        /// <summary>
        /// Obtains a span that represents the region
        /// </summary>
        public override Span<Byte> GetSpan()
        {
            var ptr = _PointerBitmap.Pointer.ToPointer();
            return new Span<Byte>(ptr, _PointerBitmap.Info.BitmapByteSize);
        }

        /// <summary>
        /// Provides access to a pointer that represents the data (note: no actual pin occurs)
        /// </summary>
        public override System.Buffers.MemoryHandle Pin(int elementIndex = 0)
        {
            // if (elementIndex < 0 || elementIndex >= _Info.BitmapByteSize) throw new ArgumentOutOfRangeException(nameof(elementIndex));

            var ptr = (Byte*)_PointerBitmap.Pointer.ToPointer();

            return new System.Buffers.MemoryHandle(ptr + elementIndex, pinnable: this);
        }

        /// <summary>
        /// Has no effect
        /// </summary>
        public override void Unpin() { }

        #endregion
    }
}
