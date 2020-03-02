using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <summary>
    /// Represents an <see cref="IntPtr"/> pointing to a bitmap in memory.
    /// </summary>
    /// <remarks>
    /// This is the lowest level possible to handling bitmaps, so it is assumed a developer knows how to use it.
    /// This structure just wraps the pointer; in order to access it, use <see cref="Span"/> or <see cref="OfType{TPixel}"/>.    
    /// </remarks>
    public readonly struct PointerBitmap
    {
        #region constructors

        public static implicit operator PointerBitmap((IntPtr,BitmapInfo) ptrbmp)
        {
            return new PointerBitmap(ptrbmp.Item1, ptrbmp.Item2);
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

        public IntPtr Pointer => _Pointer;
        public BitmapInfo Info => _Info;

        public Boolean IsReadOnly => _IsReadOnly;

        public SpanBitmap Span => new SpanBitmap(_Pointer, _Info, _IsReadOnly);

        #endregion

        #region API

        public SpanBitmap<TPixel> OfType<TPixel>()
            where TPixel:unmanaged
        {
            return new SpanBitmap<TPixel>(_Pointer, _Info,_IsReadOnly);
        }

        #endregion
    }
}
