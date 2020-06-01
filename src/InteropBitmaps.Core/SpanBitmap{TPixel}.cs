using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <summary>
    /// Represents a Bitmap wrapped around a <see cref="Span{Byte}"/>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{PixelFormat} {Width}x{Height}")]
    public readonly ref struct SpanBitmap<TPixel> where TPixel : unmanaged
    {
        #region lifecycle        

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

        private readonly BitmapInfo _Info;
        private readonly Span<Byte> _Writable;
        private readonly ReadOnlySpan<Byte> _Readable;        

        #endregion

        #region properties

        public BitmapInfo Info => _Info;

        public Span<Byte> WritableSpan => _Writable;

        public ReadOnlySpan<Byte> ReadableSpan => _Readable;

        public int Width => _Info.Width;

        public int Height => _Info.Height;

        public int PixelSize => _Info.PixelByteSize;

        public PixelFormat PixelFormat => _Info.PixelFormat;

        public int StepByteSize => _Info.StepByteSize;

        public BitmapBounds bounds => _Info.Bounds;

        #endregion

        #region API - Buffers

        public ReadOnlySpan<Byte> GetBytesScanline(int y) { return _Info.GetScanline(_Readable, y); }

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

        #endregion

        #region API - Cast

        public unsafe void PinWritablePointer(Action<PointerBitmap> onPin)
        {
            Guard.IsFalse(nameof(SpanBitmap), _Writable.IsEmpty);
            SpanBitmapImpl.PinWritablePointer(_Writable, _Info, onPin);
        }

        public unsafe void PinReadablePointer(Action<PointerBitmap> onPin)
        {
            Guard.IsFalse(nameof(SpanBitmap), _Readable.IsEmpty);
            SpanBitmapImpl.PinReadablePointer(_Readable, _Info, onPin);
        }

        public unsafe TResult PinReadablePointer<TResult>(Func<PointerBitmap, TResult> onPin)
        {
            Guard.IsFalse(nameof(SpanBitmap), _Readable.IsEmpty);
            return SpanBitmapImpl.PinReadablePointer(_Readable, _Info, onPin);
        }

        public static implicit operator SpanBitmap(SpanBitmap<TPixel> other)
        {
            return other._Writable.IsEmpty ? new SpanBitmap(other._Readable, other._Info) : new SpanBitmap(other._Writable, other._Info);
        }

        public unsafe SpanBitmap AsSpanBitmap()
        {
            return _Writable.IsEmpty ? new SpanBitmap(_Readable, _Info) : new SpanBitmap(_Writable, _Info);
        }
        
        public MemoryBitmap<TPixel> ToMemoryBitmap(PixelFormat? fmtOverride = null)
        {
            if (!fmtOverride.HasValue) return new MemoryBitmap<TPixel>(_Readable.ToArray(), _Info);

            var dst = new MemoryBitmap<TPixel>(this.Width, this.Height, fmtOverride.Value);
            dst.SetPixels(0, 0, this);
            return dst;
        }

        #endregion

        #region API - Pixel Ops

        public SpanBitmap<TPixel> Slice(in BitmapBounds rect)
        {
            var (offset, info) = _Info.Slice(rect);

            if (_Writable.IsEmpty)
            {
                var span = _Readable.Slice(offset, info.BitmapByteSize);
                return new SpanBitmap<TPixel>(span, info);
            }
            else
            {
                var span = _Writable.Slice(offset, info.BitmapByteSize);
                return new SpanBitmap<TPixel>(span, info);
            }
        }        

        public TPixel GetPixel(int x, int y) { return GetPixelsScanline(y)[x]; }

        public void SetPixel(int x, int y, TPixel value) { UsePixelsScanline(y)[x] = value; }           

        public void SetPixels(TPixel value)
        {            
            Guard.IsTrue("this", !_Writable.IsEmpty);

            if (_Info.IsContinuous)
            {
                var dst = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(_Writable);
                dst.Fill(value);
                return;
            }

            for (int y = 0; y < _Info.Height; ++y)
            {
                var dst = UsePixelsScanline(y);
                dst.Fill(value);
            }
        }

        public void SetPixels(int dstX, int dstY, SpanBitmap<TPixel> src)
        {
            Guard.IsTrue("this", !_Writable.IsEmpty);

            _Implementation.CopyPixels(this, dstX, dstY, src);
        }

        public void FitPixels(SpanBitmap<TPixel> src) { _Implementation.FitPixelsNearest(this, src); }

        public void ApplyPixels<TSrcPixel>(int dstX, int dstY, SpanBitmap<TSrcPixel> src, Func<TPixel,TSrcPixel,TPixel> pixelFunc)
            where TSrcPixel: unmanaged
        {
            _Implementation.ApplyPixels(this, dstX, dstY, src, pixelFunc);
        }

        public MemoryBitmap<TDstPixel> ToMemoryBitmap<TDstPixel>(PixelFormat fmt, Converter<TPixel, TDstPixel> pixelConverter)
            where TDstPixel:unmanaged
        {
            var dst = new MemoryBitmap<TDstPixel>(this.Width, this.Height, fmt);
            dst.ApplyPixels(0, 0, this, (a, b) => pixelConverter(b));
            return dst;
        }

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

        #region API - IO

        public void Write(System.IO.Stream stream, Codecs.CodecFormat format, params Codecs.IBitmapEncoding[] factory)
        {
            AsSpanBitmap().Write(stream, format, factory);
        }

        public void Save(string filePath, params Codecs.IBitmapEncoding[] factory)
        {
            AsSpanBitmap().Save(filePath, factory);
        }

        #endregion
    }
}
