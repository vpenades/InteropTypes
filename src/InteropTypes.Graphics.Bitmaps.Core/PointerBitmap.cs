﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

using SIZE = System.Drawing.Size;

namespace InteropTypes.Graphics.Bitmaps
{
    using Diagnostics;

    /// <summary>
    /// Represents a Bitmap backed by a native <see cref="IntPtr"/> memory pointer.
    /// </summary>
    /// <remarks>
    /// This is the lowest possible level bitmap representation, so it is assumed a developer knows how to use it.
    /// This structure just wraps the pointer; in order to access it, use <see cref="AsSpanBitmap"/> or <see cref="AsSpanBitmapOfType{TPixel}"/>.    
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Pointer} {Info.ToDebuggerDisplayString(),nq}")]
    public readonly partial struct PointerBitmap        
    {
        #region constructors

        public unsafe PointerBitmap(System.Buffers.MemoryHandle bitmapData, BitmapInfo bitmapInfo, bool isReadOnly = false)
        {
            if (bitmapData.Pointer == null) throw new ArgumentNullException(nameof(bitmapData));

            _Pointer = new IntPtr(bitmapData.Pointer);
            _Info = bitmapInfo;
            _IsReadOnly = isReadOnly;
        }

        public PointerBitmap(IntPtr bitmapData, BitmapInfo bitmapInfo, bool isReadOnly = false)
        {
            if (bitmapData == IntPtr.Zero) throw new ArgumentNullException(nameof(bitmapData));

            _Pointer = bitmapData;
            _Info = bitmapInfo;
            _IsReadOnly = isReadOnly;
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly IntPtr _Pointer;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly BitmapInfo _Info;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly Boolean _IsReadOnly;

        #endregion

        #region properties

        /// <summary>
        /// Gets the memory pointer to the first byte of the first pixel.
        /// </summary>
        public IntPtr Pointer => _Pointer;

        public int ByteSize => _Info.BitmapByteSize;

        /// <summary>
        /// Gets a value indicating whether this instance is read-only.
        /// </summary>
        public Boolean IsReadOnly => _IsReadOnly;

        public Boolean IsEmpty => Pointer == IntPtr.Zero || _Info.IsEmpty;

        #endregion

        #region properties - Info

        /// <summary>
        /// Gets the layout information of the bitmap; Width, Height, PixelFormat, etc.
        /// </summary>
        public BitmapInfo Info => _Info;

        /// <summary>
        /// Gets the pixel format of the bitmap.
        /// </summary>
        public PixelFormat PixelFormat => _Info.PixelFormat;

        /// <summary>
        /// Gets the size of the bitmap, in pixels.
        /// </summary>        
        public SIZE Size => _Info.Size;

        /// <summary>
        /// Gets the width of the bitmap, in pixels.
        /// </summary>        
        public int Width => _Info.Width;

        /// <summary>
        /// Gets the height of the bitmap, in pixels.
        /// </summary>        
        public int Height => _Info.Height;

        /// <summary>
        /// Gets the size of a single pixel, in bytes.
        /// </summary>
        public int PixelByteSize => _Info.PixelByteSize;

        /// <summary>
        /// Gets the number of bytes required to jump from one row to the next, in bytes. This is also known as the ByteStride.
        /// </summary>
        public int StepByteSize => _Info.StepByteSize;

        /// <summary>
        /// Gets the bounds of the bitmap, in pixels.
        /// </summary>        
        public BitmapBounds Bounds => _Info.Bounds;

        #endregion        

        #region API - Buffers

        [System.Diagnostics.DebuggerStepThrough]
        public unsafe Span<byte> UseScanlineBytes(int y)
        {
            if (_Pointer == IntPtr.Zero) throw new InvalidOperationException();
            var span = new Span<Byte>(_Pointer.ToPointer(), _Info.BitmapByteSize);
            return _Info.UseScanlineBytes(span, y);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public unsafe ReadOnlySpan<byte> GetScanlineBytes(int y)
        {
            if (_Pointer == IntPtr.Zero) throw new InvalidOperationException();
            var span = new Span<Byte>(_Pointer.ToPointer(), _Info.BitmapByteSize);
            return _Info.GetScanlineBytes(span, y);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public PointerBitmap Slice(in BitmapBounds rect)
        {
            var (offset, info) = Info.Slice(rect);

            var ptr = IntPtr.Add(this.Pointer, offset);

            return new PointerBitmap(ptr, info, this.IsReadOnly);
        }

        #endregion

        #region API - Cast

        public static implicit operator SpanBitmap(PointerBitmap bmp) { return new SpanBitmap(bmp._Pointer, bmp._Info); }

        

        /// <summary>
        /// Casts this <see cref="PointerBitmap"/> to a <see cref="SpanBitmap{TPixel}"/>.
        /// </summary>
        /// <typeparam name="TPixel">The type to use for a single Pixel.</typeparam>
        /// <returns>A <see cref="SpanBitmap{TPixel}"/> instance.</returns>
        public SpanBitmap<TPixel> AsSpanBitmapOfType<TPixel>()
            where TPixel : unmanaged
        {
            return new SpanBitmap<TPixel>(_Pointer, _Info, _IsReadOnly);
        }

        public MemoryBitmap.IDisposableSource UsingMemoryBitmap()
        {
            return new _MemoryManager(this);
        }

        public System.Buffers.IMemoryOwner<TPixel> UsingMemory<TPixel>()
            where TPixel:unmanaged
        {
            this.Info.ArgumentIsCompatiblePixelFormat<TPixel>();

            return new MemoryManagers.UnmanagedMemoryManager<TPixel>(this.Pointer, this.Info.BitmapByteSize);
        }
            
        #endregion

        #region API - Pixel Ops

        

        public bool CopyTo(ref MemoryBitmap other)
        {
            var refreshed = false;

            if (!this.Info.Equals(other.Info))
            {
                other = new MemoryBitmap(this.Info);
                refreshed = true;
            }

            other.SetPixels(0, 0, this);

            return refreshed;
        }

        public bool CopyTo<TPixel>(ref MemoryBitmap<TPixel> other)
            where TPixel:unmanaged
        {
            var refreshed = false;

            var info = this.Info.WithPixelFormat<TPixel>(other.Info.PixelFormat);

            if (!info.Equals(other.Info))
            {
                other = new MemoryBitmap<TPixel>(info);
                refreshed = true;
            }

            other.AsTypeless().SetPixels(0, 0, this);

            return refreshed;
        }

        public bool CopyTo(ref BitmapInfo otherInfo, ref Byte[] otherData)
        {
            if (!this.Info.Equals(otherInfo)) otherInfo = this.Info;

            var refreshed = false;

            if (otherData == null || otherData.Length < otherInfo.BitmapByteSize)
            {
                otherData = new byte[this.Info.BitmapByteSize];
                refreshed = true;
            }

            new SpanBitmap(otherData, otherInfo).SetPixels(0, 0, this);

            return refreshed;
        }        

        #endregion

        #region nested types

        sealed unsafe class _MemoryManager : MemoryManagers.BitmapMemoryManager
        {
            public _MemoryManager(PointerBitmap bmp)
            {
                Initialize(bmp);
            }        
        }        

        #endregion
    }
}
