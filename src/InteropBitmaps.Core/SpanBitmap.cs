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

        public SpanBitmap(Span<Byte> data, int width, int height, int pixelSize, int scanlineSize = 0)
        {
            Guard.BitmapRect(width, height, pixelSize, scanlineSize);

            _Width = width;
            _Height = height;
            _PixelSize = pixelSize;
            _ScanlineSize = scanlineSize > 0 ? scanlineSize : width * pixelSize;

            _Readable = _Writable = data.Slice(0, _ScanlineSize * _Height);
        }

        public SpanBitmap(ReadOnlySpan<Byte> data, int width, int height, int pixelSize, int scanlineSize = 0)
        {
            Guard.BitmapRect(width, height, pixelSize, scanlineSize);

            _Width = width;
            _Height = height;
            _PixelSize = pixelSize;
            _ScanlineSize = scanlineSize > 0 ? scanlineSize : width * pixelSize;

            _Readable = data.Slice(0, _ScanlineSize * _Height);
            _Writable = null;
        }

        public unsafe SpanBitmap(IntPtr data, int width, int height, int pixelSize, int scanlineSize = 0)
        {
            Guard.BitmapRect(width, height, pixelSize, scanlineSize);

            _Width = width;
            _Height = height;
            _PixelSize = pixelSize;
            _ScanlineSize = scanlineSize > 0 ? scanlineSize : width * pixelSize;

            _Readable = _Writable = new Span<Byte>(data.ToPointer(), _ScanlineSize * _Height);
        }

        #endregion

        #region data

        private readonly ReadOnlySpan<Byte> _Readable;
        private readonly Span<Byte>         _Writable;
        private readonly int _Width;
        private readonly int _Height;
        private readonly int _PixelSize;
        private readonly int _ScanlineSize;

        #endregion

        #region API

        public Span<Byte> WritableSpan => _Writable;

        public ReadOnlySpan<Byte> ReadOnlySpan => _Readable;

        public int Width => _Width;

        public int Height => _Height;

        public (int Width, int Height) Size => (_Width, _Height);

        public int PixelSize => _PixelSize;

        public int ScanlineSize => _ScanlineSize;

        public ReadOnlySpan<Byte> GetBytesScanline(int y) { return _Readable.Slice(y * _ScanlineSize, _Width * _PixelSize); }

        public Span<Byte> UseBytesScanline(int y) { return _Writable.Slice(y * _ScanlineSize, _Width * _PixelSize); }

        public unsafe void PinWritableMemory(Action<(IntPtr Poiter, int Width,int Height, int PixSize, int ScanSize)> onPin)
        {
            if (_Writable.Length == 0) throw new InvalidOperationException();

            fixed (byte* ptr = &_Writable.GetPinnableReference())
            {
                onPin((new IntPtr(ptr), _Width, _Height, _PixelSize, _ScanlineSize));
            }
        }

        public unsafe void PinReadableMemory(Action<(IntPtr Poiter, int Width, int Height, int PixSize, int ScanSize)> onPin)
        {
            if (_Writable.Length == 0) throw new InvalidOperationException();

            fixed (byte* ptr = &_Readable.GetPinnableReference())
            {
                onPin((new IntPtr(ptr), _Width, _Height, _PixelSize, _ScanlineSize));
            }
        }

        #endregion

        #region Bulk API

        public void SetPixels(int dstX, int dstY, SpanBitmap src) { SetPixels(dstX, dstY, src, (0, 0, src.Width, src.Height)); }

        public void SetPixels(int dstX, int dstY, SpanBitmap src, in (int x, int y, int w, int h) srcRect)
        {
            Guard.AreEqual(nameof(src.PixelSize), this.PixelSize, src.PixelSize);

            var xRect = srcRect.Clamp((0, 0, this.Width, this.Height));
            if (xRect.w <= 0 || xRect.h <= 0) return;

            dstX += xRect.x - srcRect.x;
            dstY += xRect.y - srcRect.y;

            for (int y = 0; y < srcRect.h; ++y)
            {
                var srcRow = src.GetBytesScanline(xRect.y + y).Slice(xRect.x * _PixelSize, xRect.w * _PixelSize);
                var dstRow = this.UseBytesScanline(dstY + y).Slice(dstX * _PixelSize, xRect.w * _PixelSize);

                srcRow.CopyTo(dstRow);
            }
        }

        public unsafe SpanBitmap<TPixel> AsSpanBitmap<TPixel>()
            where TPixel : unmanaged
        {
            if (sizeof(TPixel) != _PixelSize) throw new ArgumentException(nameof(TPixel));

            if (_Writable.Length > 0) return new SpanBitmap<TPixel>(_Writable, _Width, _Height, _ScanlineSize);
            else                      return new SpanBitmap<TPixel>(_Readable, _Width, _Height, _ScanlineSize);
        }

        public MemoryBitmap ToMemoryBitmap() { return new MemoryBitmap(_Readable.ToArray(), _Width, _Height, _PixelSize, _ScanlineSize); }

        public unsafe MemoryBitmap<TPixel> ToMemoryBitmap<TPixel>()
            where TPixel : unmanaged
        {
            if (sizeof(TPixel) != _PixelSize) throw new ArgumentException(nameof(TPixel));

            return new MemoryBitmap<TPixel>(_Readable.ToArray(), _Width, _Height, _ScanlineSize);
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

        public unsafe SpanBitmap(Span<TPixel> data, int width, int height, int scanlineSize = 0)
        {
            Guard.BitmapRect(width, height, sizeof(TPixel), scanlineSize);

            _Width = width;
            _Height = height;
            _PixelSize = sizeof(TPixel);
            _ScanlineSize = scanlineSize > 0 ? scanlineSize : width * _PixelSize;

            _Readable = _Writable = System.Runtime.InteropServices.MemoryMarshal
                .Cast<TPixel, Byte>(data)
                .Slice(0, _ScanlineSize * _Height);
        }

        public unsafe SpanBitmap(Span<Byte> data, int width, int height, int scanlineSize = 0)
        {
            Guard.BitmapRect(width, height, sizeof(TPixel), scanlineSize);

            _Width = width;
            _Height = height;
            _PixelSize = sizeof(TPixel);
            _ScanlineSize = scanlineSize > 0 ? scanlineSize : width * _PixelSize;

            _Readable = _Writable = data.Slice(0, _ScanlineSize * _Height);
        }

        public unsafe SpanBitmap(ReadOnlySpan<Byte> data, int width, int height, int scanlineSize = 0)
        {
            Guard.BitmapRect(width, height, sizeof(TPixel), scanlineSize);

            _Width = width;
            _Height = height;
            _PixelSize = sizeof(TPixel);
            _ScanlineSize = scanlineSize > 0 ? scanlineSize : width * _PixelSize;

            _Readable = data.Slice(0, _ScanlineSize * _Height);
            _Writable = null;
        }

        public unsafe SpanBitmap(IntPtr data, int width, int height, int scanlineSize = 0)
        {
            Guard.NotNull(nameof(data), data);
            Guard.BitmapRect(width, height, sizeof(TPixel), scanlineSize);

            _Width = width;
            _Height = height;
            _PixelSize = sizeof(TPixel);
            _ScanlineSize = scanlineSize > 0 ? scanlineSize : width * _PixelSize;

            _Readable = _Writable = new Span<Byte>(data.ToPointer(), _ScanlineSize * _Height);
        }

        #endregion

        #region data

        private readonly ReadOnlySpan<Byte> _Readable;
        private readonly Span<Byte>         _Writable;        
        private readonly int _Width;
        private readonly int _Height;
        private readonly int _PixelSize;
        private readonly int _ScanlineSize;

        #endregion

        #region API

        public Span<Byte> WritableSpan => _Writable;

        public ReadOnlySpan<Byte> ReadOnlySpan => _Readable;

        public int Width => _Width;

        public int Height => _Height;

        public (int Width, int Height) Size => (_Width, _Height);

        public int PixelSize => _PixelSize;

        public ReadOnlySpan<Byte> GetBytesScanline(int y) { return _Readable.Slice(y * _ScanlineSize, _Width * _PixelSize); }

        public ReadOnlySpan<TPixel> GetPixelsScanline(int y)
        {
            var rowBytes = GetBytesScanline(y);

            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(rowBytes);
        }

        public Span<Byte> UseBytesScanline(int y) { return _Writable.Slice(y * _ScanlineSize, _Width * _PixelSize); }        

        public Span<TPixel> UsePixelsScanline(int y)
        {
            var rowBytes = UseBytesScanline(y);

            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(rowBytes);
        }

        public TPixel GetPixel(int x, int y) { return GetPixelsScanline(y)[x]; }

        public void SetPixel(int x, int y, TPixel value) { UsePixelsScanline(y)[x] = value; }
        
        #endregion

        #region API - Bulk operations

        public void SetPixels(int x, int y, IBitmap<TPixel> src) { SetPixels(x, y, src, (0, 0, src.Width, src.Height)); }

        public void SetPixels(int dstX, int dstY, IBitmap<TPixel> src, in (int x, int y, int w, int h) srcRect)
        {
            var scanlines = _GetScanlines(dstX, dstY, (src.Width,src.Height), srcRect);

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
            if (_Writable.Length > 0) return new SpanBitmap(_Writable, _Width, _Height, sizeof(TPixel), _ScanlineSize);
            else                      return new SpanBitmap(_Readable, _Width, _Height, sizeof(TPixel), _ScanlineSize);
        }

        public MemoryBitmap<TPixel> ToMemoryBitmap() { return new MemoryBitmap<TPixel>(_Readable.ToArray(), _Width, _Height, _ScanlineSize); }

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
