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

        public BitmapInfo Info => _Info;

        public Span<Byte> WritableSpan => _Writable;

        public ReadOnlySpan<Byte> ReadableSpan => _Readable;

        public int Width => _Info.Width;

        public int Height => _Info.Height;

        public int PixelSize => _Info.PixelSize;

        public PixelFormat PixelFormat => new PixelFormat(_Info.PixelFormat);

        public int ScanlineSize => _Info.ScanlineSize;

        public BitmapBounds bounds => _Info.Bounds;

        #endregion

        #region API

        /// <summary>
        /// Crops the current <see cref="SpanBitmap"/> sharing the original source memory.
        /// </summary>
        /// <param name="rect">The region to crop.</param>
        /// <returns>A <see cref="SpanBitmap"/> representing the cropped region.</returns>
        public SpanBitmap Slice(in BitmapBounds rect)
        {
            var (offset, info) = _Info.Slice(rect);

            if (_Writable.IsEmpty)
            {
                var span = _Readable.Slice(offset, info.BitmapByteSize);
                return new SpanBitmap(span, info);
            }
            else
            {
                var span = _Writable.Slice(offset, info.BitmapByteSize);
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

        public unsafe TResult PinReadableMemory<TResult>(Func<PointerBitmap, TResult> onPin)
        {
            if (_Writable.Length == 0) throw new InvalidOperationException();

            fixed (byte* ptr = &_Readable.GetPinnableReference())
            {
                var ptrBmp = new PointerBitmap(new IntPtr(ptr), _Info, true);

                return onPin(ptrBmp);
            }
        }

        // https://github.com/dotnet/runtime/blob/master/src/libraries/System.Utf8String.Experimental/tests/System/MemoryTests.cs

            /*
        public unsafe void Pin()
        {
            System.Runtime.InteropServices.GCHandle.Alloc(_Writable.GetPinnableReference());
        }*/



        public unsafe SpanBitmap<TPixel> OfType<TPixel>()
            where TPixel : unmanaged
        {
            Guard.IsValidPixelFormat<TPixel>(_Info);

            return _Writable.IsEmpty ? new SpanBitmap<TPixel>(_Readable, _Info) : new SpanBitmap<TPixel>(_Writable, _Info);
        }

        #endregion

        #region Bulk API        

        public void SetPixels(int dstX, int dstY, SpanBitmap src) { _Implementation.CopyPixels(this, dstX, dstY, src); }        

        public MemoryBitmap ToMemoryBitmap(PixelFormat? fmtOverride = null)
        {
            if (!fmtOverride.HasValue) return new MemoryBitmap(_Readable.ToArray(), _Info);

            var dst = new MemoryBitmap(this.Width, this.Height, fmtOverride.Value);
            dst.SetPixels(0, 0, this);
            return dst;
        }

        public void Save(string filePath, params Codecs.IBitmapEncoding[] factory)
        {
            var ext = Codecs.CodecFactory.ParseFormat(filePath);

            foreach (var f in factory)
            {                
                using (var s = System.IO.File.Create(filePath))
                {
                    if (f.TryWrite(s, ext, this)) return;
                }
            }

            throw new ArgumentException("invalid format", nameof(filePath));
        }

        #endregion        
    }    
}
