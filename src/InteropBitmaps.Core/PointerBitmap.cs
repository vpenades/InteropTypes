using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <summary>
    /// Represents an <see cref="IntPtr"/> pointing to a bitmap in memory.
    /// </summary>
    /// <remarks>
    /// This is the lowest possible level bitmap representation, so it is assumed a developer knows how to use it.
    /// This structure just wraps the pointer; in order to access it, use <see cref="Bitmap"/> or <see cref="OfType{TPixel}"/>.    
    /// </remarks>
    public readonly struct PointerBitmap
    {
        #region constructors        

        public unsafe PointerBitmap(System.Buffers.MemoryHandle ptr, BitmapInfo info, bool isReadOnly = false)
        {
            _Pointer = new IntPtr(ptr.Pointer);
            _Info = info;
            _IsReadOnly = isReadOnly;
        }

        public PointerBitmap(IntPtr ptr, BitmapInfo info, bool isReadOnly = false)
        {
            _Pointer = ptr;
            _Info = info;
            _IsReadOnly = isReadOnly;
        }

        #endregion

        #region data

        private readonly IntPtr _Pointer;
        private readonly BitmapInfo _Info;
        private readonly Boolean _IsReadOnly;

        #endregion

        #region properties

        /// <summary>
        /// Gets the memory pointer to the first byte of the first pixel.
        /// </summary>
        public IntPtr Pointer => _Pointer;

        /// <summary>
        /// Gets the structural information of the bitmap; Width, Height, PixelFormat, etc.
        /// </summary>
        public BitmapInfo Info => _Info;

        /// <summary>
        /// Gets a value indicating whether this instance is read-only.
        /// </summary>
        public Boolean IsReadOnly => _IsReadOnly;

        /// <summary>
        /// Gets a <see cref="SpanBitmap"/> instance from the memory pointer.
        /// </summary>
        public SpanBitmap Bitmap => new SpanBitmap(_Pointer, _Info, _IsReadOnly);

        #endregion

        #region API

        public PointerBitmap Slice(in BitmapBounds rect)
        {
            var (offset, info) = Info.Slice(rect);

            var ptr = IntPtr.Add(this.Pointer, offset);

            return new PointerBitmap(ptr, info);
        }

        /// <summary>
        /// Casts this <see cref="PointerBitmap"/> to a <see cref="SpanBitmap{TPixel}"/>.
        /// </summary>
        /// <typeparam name="TPixel">The type to use for a single Pixel.</typeparam>
        /// <returns>A <see cref="SpanBitmap{TPixel}"/> instance.</returns>
        public SpanBitmap<TPixel> OfType<TPixel>()
            where TPixel:unmanaged
        {
            return new SpanBitmap<TPixel>(_Pointer, _Info,_IsReadOnly);
        }

        private System.Buffers.MemoryManager<T> CreateManager<T>()
            where T:unmanaged
        {
            return new MemoryManagers.UnmanagedMemoryManager<T>(_Pointer, _Info.BitmapByteSize);
        }

        #endregion
    }
}
