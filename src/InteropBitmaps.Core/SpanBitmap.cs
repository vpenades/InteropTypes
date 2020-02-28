using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <summary>
    /// Represents a Bitmap wrapped around a span of bytes.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Bitmap[{Width},{Height}]")]
    public readonly ref struct SpanBitmap
    {
        #region lifecycle
        
        public unsafe SpanBitmap(IntPtr data, in BitmapInfo info, bool isReadOnly = false)
        {
            _Info = info;

            var span = new Span<Byte>(data.ToPointer(), info.BitmapByteSize);

            _Readable = span;
            _Writable = isReadOnly ? null : span;
        }

        public SpanBitmap(Span<Byte> data, in BitmapInfo info)
        {
            _Info = info;
            _Readable = _Writable = data.Slice(0, _Info.BitmapByteSize);
        }

        public SpanBitmap(ReadOnlySpan<Byte> data, in BitmapInfo info)
        {
            _Info = info;
            _Readable = data.Slice(0, _Info.BitmapByteSize);
            _Writable = null;
        }

        public SpanBitmap(Span<Byte> data, int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, scanlineSize);
            _Readable = _Writable = data.Slice(0, _Info.BitmapByteSize);
        }

        public SpanBitmap(ReadOnlySpan<Byte> data, int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, scanlineSize);
            _Readable = data.Slice(0, _Info.BitmapByteSize);
            _Writable = null;
        }        
        
        #endregion

        #region data

        private readonly ReadOnlySpan<Byte>     _Readable;
        private readonly Span<Byte>             _Writable;
        private readonly BitmapInfo             _Info;

        #endregion

        #region properties

        public Span<Byte> WritableSpan => _Writable;

        public ReadOnlySpan<Byte> ReadableSpan => _Readable;

        public int Width => _Info.Width;

        public int Height => _Info.Height;

        public (int Width, int Height) Size => _Info.Size;

        public int PixelSize => _Info.PixelSize;

        public PixelFormat PixelFormat => new PixelFormat(_Info.PixelFormat);

        public int ScanlineSize => _Info.ScanlineSize;

        #endregion

        #region API

        public SpanBitmap Slice(int x, int y, int width, int height)
        {
            var (offset, info) = _Info.Slice(x, y, width, height);

            if (_Writable.IsEmpty)
            {
                var span = _Writable.Slice(offset, info.BitmapByteSize);
                return new SpanBitmap(span, info);
            }
            else
            {
                var span = _Readable.Slice(offset, info.BitmapByteSize);
                return new SpanBitmap(span, info);
            }
        }

        public ReadOnlySpan<Byte> GetBytesScanline(int y) { return _Info.GetScanline(_Readable, y); }

        public Span<Byte> UseBytesScanline(int y) { return _Info.UseScanline(_Writable, y); }

        public unsafe void PinWritableMemory(Action<PointerBitmap> onPin)
        {
            if (_Writable.Length == 0) throw new InvalidOperationException();

            fixed (byte* ptr = &_Writable.GetPinnableReference())
            {
                var ptrBmp = new PointerBitmap(new IntPtr(ptr), _Info);

                onPin(ptrBmp);
            }
        }

        public unsafe void PinReadableMemory(Action<PointerBitmap> onPin)
        {
            if (_Writable.Length == 0) throw new InvalidOperationException();

            fixed (byte* ptr = &_Readable.GetPinnableReference())
            {
                var ptrBmp = new PointerBitmap(new IntPtr(ptr), _Info, true);

                onPin(ptrBmp);
            }
        }

        public unsafe SpanBitmap<TPixel> OfType<TPixel>()
            where TPixel : unmanaged
        {
            Guard.IsValidPixelFormat<TPixel>(_Info);

            return _Writable.IsEmpty ? new SpanBitmap<TPixel>(_Readable, _Info) : new SpanBitmap<TPixel>(_Writable, _Info);
        }

        #endregion

        #region Bulk API

        public void SetPixels(int dstX, int dstY, SpanBitmap src) { SetPixels(dstX, dstY, src, (0, 0, src.Width, src.Height)); }

        public void SetPixels(int dstX, int dstY, SpanBitmap src, in (int x, int y, int w, int h) srcRect)
        {
            var xRect = srcRect.Clamp((0, 0, this.Width, this.Height));
            if (xRect.w <= 0 || xRect.h <= 0) return;

            dstX += xRect.x - srcRect.x;
            dstY += xRect.y - srcRect.y;

            if (this.PixelFormat == src.PixelFormat)
            {
                for (int y = 0; y < srcRect.h; ++y)
                {
                    var srcRow = src._Info.GetPixels(src._Readable, xRect.x, xRect.y + y, xRect.w);
                    var dstRow = this._Info.UsePixels(this._Writable, dstX, dstY + y, xRect.w);

                    srcRow.CopyTo(dstRow);
                }
            }
            else
            {
                var srcConverter = _PixelConverters.GetConverter(src.PixelFormat);
                var dstConverter = _PixelConverters.GetConverter(this.PixelFormat);

                Span<_PixelBGRA32> tmp = stackalloc _PixelBGRA32[xRect.w];

                for (int y = 0; y < srcRect.h; ++y)
                {
                    var srcRow = src._Info.GetPixels(src._Readable, xRect.x, xRect.y + y, xRect.w);
                    var dstRow = this._Info.UsePixels(this._Writable, dstX, dstY + y, xRect.w);

                    srcConverter.ConvertFrom(tmp, srcRow);
                    dstConverter.ConvertTo(dstRow, tmp);
                }
            }

        }

        public MemoryBitmap ToMemoryBitmap() { return new MemoryBitmap(_Readable.ToArray(), _Info); }

        public unsafe MemoryBitmap<TPixel> ToMemoryBitmap<TPixel>()
            where TPixel : unmanaged
        {
            return new MemoryBitmap<TPixel>(_Readable.ToArray(), _Info);
        }

        #endregion        
    }

    /// <summary>
    /// Represents a Bitmap wrapped around a span of bytes.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Bitmap[{Width},{Height}]")]
    public readonly ref struct SpanBitmap<TPixel>
        where TPixel : unmanaged
    {
        #region lifecycle        

        public static implicit operator SpanBitmap(SpanBitmap<TPixel> other) { return other.AsSpanBitmap(); }

        public unsafe SpanBitmap(IntPtr data, in BitmapInfo info, bool isReadOnly = false)
        {
            Guard.IsValidPixelFormat<TPixel>(info);

            _Info = info;

            var span = new Span<Byte>(data.ToPointer(), info.BitmapByteSize);

            _Readable = span;
            _Writable = isReadOnly ? null : span;
        }

        internal SpanBitmap(Span<Byte> data, in BitmapInfo info)
        {
            Guard.IsValidPixelFormat<TPixel>(info);
            _Info = info;
            _Readable = _Writable = data.Slice(0, _Info.BitmapByteSize);
        }

        internal SpanBitmap(ReadOnlySpan<Byte> data, in BitmapInfo info)
        {
            Guard.IsValidPixelFormat<TPixel>(info);
            _Info = info;
            _Readable = data.Slice(0, _Info.BitmapByteSize);
            _Writable = null;
        }

        public unsafe SpanBitmap(Span<Byte> data, int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, scanlineSize);
            Guard.IsValidPixelFormat<TPixel>(_Info);
            _Readable = _Writable = data.Slice(0, _Info.BitmapByteSize);
        }

        public unsafe SpanBitmap(Span<TPixel> data, int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)            
        {
            var span = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Byte>(data);

            _Info = new BitmapInfo(width, height, pixelFormat, scanlineSize);
            Guard.IsValidPixelFormat<TPixel>(_Info);
            _Readable = _Writable = span.Slice(0, _Info.BitmapByteSize);
        }

        public unsafe SpanBitmap(ReadOnlySpan<Byte> data, int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, scanlineSize);
            Guard.IsValidPixelFormat<TPixel>(_Info);
            _Readable = data.Slice(0, _Info.BitmapByteSize);
            _Writable = null;
        }

        #endregion

        #region data

        private readonly ReadOnlySpan<Byte> _Readable;
        private readonly Span<Byte>         _Writable;
        private readonly BitmapInfo         _Info;

        #endregion

        #region properties

        public Span<Byte> WritableSpan => _Writable;

        public ReadOnlySpan<Byte> ReadableSpan => _Readable;

        public int Width => _Info.Width;

        public int Height => _Info.Height;

        public (int Width, int Height) Size => _Info.Size;

        public int PixelSize => _Info.PixelSize;

        #endregion

        #region API

        public SpanBitmap<TPixel> Slice(int x, int y, int width, int height)
        {
            var (offset, info) = _Info.Slice(x, y, width, height);

            if (_Writable.IsEmpty)
            {
                var span = _Writable.Slice(offset, info.BitmapByteSize);
                return new SpanBitmap<TPixel>(span, info);
            }
            else
            {
                var span = _Readable.Slice(offset, info.BitmapByteSize);
                return new SpanBitmap<TPixel>(span, info);
            }
        }

        public ReadOnlySpan<Byte> GetBytesScanline(int y) { return _Info.GetScanline(_Readable,y); }

        public Span<Byte> UseBytesScanline(int y) { return _Info.UseScanline(_Writable, y); }

        public ReadOnlySpan<TPixel> GetPixelsScanline(int y)
        {
            var rowBytes = _Info.GetScanline(_Readable, y);

            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(rowBytes);
        }        

        public Span<TPixel> UsePixelsScanline(int y)
        {
            var rowBytes = _Info.UseScanline(_Writable, y);

            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(rowBytes);
        }

        public TPixel GetPixel(int x, int y) { return GetPixelsScanline(y)[x]; }

        public void SetPixel(int x, int y, TPixel value) { UsePixelsScanline(y)[x] = value; }
        
        #endregion

        #region API - Bulk operations

        public void SetPixels(int x, int y, IBitmap<TPixel> src) { SetPixels(x, y, src, (0, 0, src.Width, src.Height)); }
        
        public void SetPixels(int dstX, int dstY, SpanBitmap<TPixel> src) { SetPixels(dstX, dstY, src, (0, 0, src.Width, src.Height)); }

        public void SetPixels(int dstX, int dstY, SpanBitmap<TPixel> src, in (int x, int y, int w, int h) srcRect)
        {
            var scanlines = _GetScanlines(dstX, dstY, src.Size, srcRect);

            if (scanlines.IsEmpty) return;

            for (int y = 0; y < scanlines.Height; ++y)
            {
                var srcRow =  src.GetPixelsScanline(scanlines.SourceY + y).Slice(scanlines.SourceX, scanlines.Width);
                var dstRow = this.UsePixelsScanline(scanlines.TargetY + y).Slice(scanlines.TargetX, scanlines.Width);

                srcRow.CopyTo(dstRow);
            }
        }

        public void SetPixels(int dstX, int dstY, IBitmap<TPixel> src, in (int x, int y, int w, int h) srcRect)
        {
            var scanlines = _GetScanlines(dstX, dstY, (src.Width, src.Height), srcRect);

            if (scanlines.IsEmpty) return;

            for (int y = 0; y < scanlines.Height; ++y)
            {
                for (int x = 0; x < scanlines.Width; ++x)
                {
                    var pixel = src.GetPixel(scanlines.SourceX + x, scanlines.SourceY + y);
                    this.SetPixel(scanlines.TargetX + x, scanlines.TargetY + y, pixel);
                }
            }
        }

        private RectangleOverlap _GetScanlines(int dstX, int dstY, (int w, int h) srcSiz, (int x, int y, int w, int h) srcRect)
        {
            var scanlines = new RectangleOverlap();
            scanlines.SetSourceSize(srcSiz);
            scanlines.SetTargetSize(this.Size);
            scanlines.SetTransfer((dstX, dstY), (srcRect.x, srcRect.y), (srcRect.w, srcRect.h));
            return scanlines;
        }

        public unsafe SpanBitmap AsSpanBitmap()
        {
            return _Writable.IsEmpty ? new SpanBitmap(_Readable, _Info) : new SpanBitmap(_Writable, _Info);
        }
        
        public MemoryBitmap<TPixel> ToMemoryBitmap() { return new MemoryBitmap<TPixel>(_Readable.ToArray(), _Info); }

        #endregion

        #region pixel enumerator
        
        public PixelEnumerator GetPixelEnumerator() => new PixelEnumerator(this);
        
        public ref struct PixelEnumerator
        {            
            private readonly SpanBitmap<TPixel> _span;
            private ReadOnlySpan<TPixel> _Line;
            private int _indexX;
            private int _indexY;

            internal PixelEnumerator(SpanBitmap<TPixel> span)
            {
                _span = span;                
                _indexX = -1;
                _indexY = 0;
                _Line = span.GetPixelsScanline(_indexY);
            }
            
            public bool MoveNext()
            {
                int x = _indexX + 1;
                if (x < _span.Width)
                {
                    _indexX = x;
                    return true;
                }

                _indexX = 0;

                int y = _indexY + 1;
                if (y < _span.Height)
                {
                    _indexY = y;
                    _Line = _span.GetPixelsScanline(_indexY);
                    return true;
                }

                return false;
            }
            
            public ref readonly TPixel Current => ref _Line[_indexX];
        }

        #endregion
    }
}
