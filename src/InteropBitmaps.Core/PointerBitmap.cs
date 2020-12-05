using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

using SIZE = System.Drawing.Size;

namespace InteropBitmaps
{
    /// <summary>
    /// Represents a Bitmap backed by a native <see cref="IntPtr"/> memory pointer.
    /// </summary>
    /// <remarks>
    /// This is the lowest possible level bitmap representation, so it is assumed a developer knows how to use it.
    /// This structure just wraps the pointer; in order to access it, use <see cref="AsSpanBitmap"/> or <see cref="AsSpanBitmapOfType{TPixel}"/>.    
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Pointer} {Info._DebuggerDisplay(),nq}")]
    public readonly struct PointerBitmap
    {
        #region constructors        

        public unsafe PointerBitmap(System.Buffers.MemoryHandle ptr, BitmapInfo info, bool isReadOnly = false)
        {
            if (ptr.Pointer == null) throw new ArgumentNullException(nameof(ptr));

            _Pointer = new IntPtr(ptr.Pointer);
            _Info = info;
            _IsReadOnly = isReadOnly;
        }

        public PointerBitmap(IntPtr ptr, BitmapInfo info, bool isReadOnly = false)
        {
            if (ptr == IntPtr.Zero) throw new ArgumentNullException(nameof(ptr));

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
        /// Gets the layout information of the bitmap; Width, Height, PixelFormat, etc.
        /// </summary>
        public BitmapInfo Info => _Info;

        public int ByteSize => _Info.BitmapByteSize;

        /// <summary>
        /// Gets a value indicating whether this instance is read-only.
        /// </summary>
        public Boolean IsReadOnly => _IsReadOnly;

        public Boolean IsEmpty => Pointer == IntPtr.Zero || _Info.IsEmpty;

        public SIZE Size => _Info.Size;

        public int Width => _Info.Width;

        public int Height => _Info.Height;

        public int PixelSize => _Info.PixelByteSize;

        public Pixel.Format PixelFormat => _Info.PixelFormat;

        public int StepByteSize => _Info.StepByteSize;

        public BitmapBounds bounds => _Info.Bounds;

        #endregion        

        #region API - Cast

        public static implicit operator SpanBitmap(PointerBitmap bmp) { return new SpanBitmap(bmp._Pointer, bmp._Info); }

        public SpanBitmap AsSpanBitmap() { return this; }

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

        public IMemoryBitmapOwner UsingMemoryBitmap()
        {
            return new _MemoryManager(this);
        }

        public System.Buffers.IMemoryOwner<TPixel> UsingMemory<TPixel>()
            where TPixel:unmanaged
        {
            Guard.IsValidPixelFormat<TPixel>(this.Info);

            return new MemoryManagers.UnmanagedMemoryManager<TPixel>(this.Pointer, this.Info.BitmapByteSize);
        }
            
        #endregion

        #region API - Pixel Ops

        public PointerBitmap Slice(in BitmapBounds rect)
        {
            var (offset, info) = Info.Slice(rect);

            var ptr = IntPtr.Add(this.Pointer, offset);

            return new PointerBitmap(ptr, info, this.IsReadOnly);
        }

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


        internal void _HorizontalFlip(bool useMultiThreading = true)
        {
            switch (this.PixelFormat.ByteCount)
            {
                case 1: _HorizontalFlip<Byte>(this, useMultiThreading); return;
                case 2: _HorizontalFlip<ushort>(this, useMultiThreading); return;
                case 3: _HorizontalFlip<Pixel.RGB24>(this, useMultiThreading); return;
                case 4: _HorizontalFlip<uint>(this, useMultiThreading); return;
                case 8: _HorizontalFlip<ulong>(this, useMultiThreading); return;
                case 12: _HorizontalFlip<System.Numerics.Vector3>(this, useMultiThreading); return;
                case 16: _HorizontalFlip<System.Numerics.Vector4>(this, useMultiThreading); return;
            }

            throw new InvalidOperationException($"Unsupported pixel size: {this.PixelFormat.ByteCount}");
        }        

        private static void _HorizontalFlip<TPixel>(PointerBitmap ptrBmp, bool useMultiThreading = true)
            where TPixel:unmanaged
        {
            if (!useMultiThreading)
            {
                ptrBmp.AsSpanBitmapOfType<TPixel>()._HorizontalFlipRows(0,ptrBmp.Height);
                return;
            }

            const int threads = 4;            

            void _hflip(int idx)
            {
                var bmp = ptrBmp.AsSpanBitmapOfType<TPixel>();
                int y0 = bmp.Height * (idx + 0) / threads;
                int y1 = bmp.Height * (idx + 1) / threads;
                bmp._HorizontalFlipRows(y0, y1);
            }

            System.Threading.Tasks.Parallel.For(0, threads, _hflip);
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
