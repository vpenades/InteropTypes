using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.MemoryManagers
{
    /// <summary>
    /// Helper class that wraps a <see cref="PointerBitmap"/> to expose it as a <see cref="MemoryBitmap"/>
    /// </summary>    
    /// <see cref="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Buffers/MemoryManager.cs"/>
    [System.Diagnostics.DebuggerDisplay("{ToDebuggerDisplayString(),nq}")]
    public abstract unsafe class BitmapMemoryManager
        : System.Buffers.MemoryManager<Byte>
        , MemoryBitmap.ISource
        , PointerBitmap.ISource
    {
        #region debug

        protected virtual string ToDebuggerDisplayString()
        {
            if (_Disposed) return "⚠ DISPOSED";
            return _PointerBitmap.Info.ToDebuggerDisplayString();
        }

        #endregion

        #region lifecycle

        protected void Initialize(PointerBitmap ptrbmp)
        {
            _Disposed = false;
            _PointerBitmap = ptrbmp;            
        }

        /// <summary>
        /// Releases all resources associated with this object
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            _Disposed = true;
            _PointerBitmap = default;
        }

        private void _CheckDisposed()
        {
            if (_Disposed) throw new ObjectDisposedException(nameof(BitmapMemoryManager));
        }

        ~BitmapMemoryManager()
        {
            if (!_Disposed) throw new InvalidOperationException("Object must be disposed");
        }

        #endregion

        #region data

        private bool _Disposed = true;
        private PointerBitmap _PointerBitmap;        

        #endregion

        #region properties        

        public static implicit operator PointerBitmap(BitmapMemoryManager manager)
        {
            manager._CheckDisposed();
            return manager._PointerBitmap;
        }

        PointerBitmap PointerBitmap.ISource.Bitmap
        {
            get
            {
                _CheckDisposed();
                return _PointerBitmap;
            }
        }

        public static implicit operator MemoryBitmap(BitmapMemoryManager manager)
        {
            manager._CheckDisposed();
            return new MemoryBitmap(manager.Memory, manager._PointerBitmap.Info);
        }

        MemoryBitmap MemoryBitmap.ISource.Bitmap
        {
            get
            {
                _CheckDisposed();
                return new MemoryBitmap(this.Memory, _PointerBitmap.Info);
            }
        }

        #endregion

        #region MemoryManager<Byte> API

        /// <summary>
        /// Obtains a span that represents the region
        /// </summary>
        public override Span<Byte> GetSpan()
        {
            _CheckDisposed();
            var ptr = _PointerBitmap.Pointer.ToPointer();
            return new Span<Byte>(ptr, _PointerBitmap.Info.BitmapByteSize);
        }

        /// <summary>
        /// Provides access to a pointer that represents the data (note: no actual pin occurs)
        /// </summary>
        public override System.Buffers.MemoryHandle Pin(int elementIndex = 0)
        {
            _CheckDisposed();

            // if (elementIndex < 0 || elementIndex >= _Info.BitmapByteSize) throw new ArgumentOutOfRangeException(nameof(elementIndex));

            var ptr = (Byte*)_PointerBitmap.Pointer.ToPointer();

            return new System.Buffers.MemoryHandle(ptr + elementIndex, pinnable: this);
        }

        /// <summary>
        /// Has no effect
        /// </summary>
        public override void Unpin()
        {
            _CheckDisposed();
        }

        #endregion
    }
}
