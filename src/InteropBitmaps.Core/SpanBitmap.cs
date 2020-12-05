using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <summary>
    /// Represents a Bitmap backed by a <see cref="Span{Byte}"/>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Info._DebuggerDisplay(),nq}")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(Debug.SpanBitmapProxy))]
    public readonly ref partial struct SpanBitmap
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

        public SpanBitmap(Span<Byte> data, int width, int height, Pixel.Format pixelFormat, int stepByteSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, stepByteSize);
            _Readable = _Writable = data.Slice(0, _Info.BitmapByteSize);
        }

        public SpanBitmap(ReadOnlySpan<Byte> data, int width, int height, Pixel.Format pixelFormat, int stepByteSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, stepByteSize);
            _Readable = data.Slice(0, _Info.BitmapByteSize);
            _Writable = null;
        }

        public SpanBitmap Clone(ref Byte[] recyclableBuffer)
        {            
            if (recyclableBuffer == null || recyclableBuffer.Length < this.Info.BitmapByteSize) recyclableBuffer = new byte[this.Info.BitmapByteSize];

            var other = new SpanBitmap(recyclableBuffer, this.Info);

            other.SetPixels(0, 0, this);

            return other;
        }

        #endregion

        #region data

        private readonly BitmapInfo _Info;
        private readonly Span<Byte> _Writable;
        private readonly ReadOnlySpan<Byte> _Readable;

        public override int GetHashCode() { return _Implementation.CalculateHashCode(_Readable, _Info); }

        #endregion

        #region properties

        public BitmapInfo Info => _Info;

        public Span<Byte> WritableSpan => _Writable;

        public ReadOnlySpan<Byte> ReadableSpan => _Readable;

        public int Width => _Info.Width;

        public int Height => _Info.Height;

        public int PixelSize => _Info.PixelByteSize;

        public Pixel.Format PixelFormat => _Info.PixelFormat;

        public int StepByteSize => _Info.StepByteSize;

        public BitmapBounds bounds => _Info.Bounds;

        #endregion

        #region API - Buffers

        public ReadOnlySpan<Byte> GetBytesScanline(int y) { return _Info.GetScanline(_Readable, y); }

        public Span<Byte> UseBytesScanline(int y) { return _Info.UseScanline(_Writable, y); }

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

        /// <summary>
        /// Returns a pixel specific <see cref="SpanBitmap{TPixel}"/>.
        /// </summary>
        /// <typeparam name="TPixel">The pixel type.</typeparam>        
        /// <returns>A <see cref="SpanBitmap{TPixel}"/></returns>
        /// <remarks>
        /// This is the opposite operation of <see cref="SpanBitmap{TPixel}.AsTypeless"/>
        /// </remarks>
        public unsafe SpanBitmap<TPixel> OfType<TPixel>()
            where TPixel : unmanaged
        {
            Guard.IsValidPixelFormat<TPixel>(_Info);

            return _Writable.IsEmpty ? new SpanBitmap<TPixel>(_Readable, _Info) : new SpanBitmap<TPixel>(_Writable, _Info);
        }

        /// <summary>
        /// Creates a <see cref="MemoryBitmap"/> copy from this <see cref="SpanBitmap"/>.
        /// </summary>
        /// <param name="fmtOverride">Format override.</param>
        /// <returns>A new <see cref="MemoryBitmap"/>.</returns>
        public MemoryBitmap ToMemoryBitmap(Pixel.Format? fmtOverride = null)
        {
            if (!fmtOverride.HasValue) return new MemoryBitmap(_Readable.ToArray(), _Info);

            var dst = new MemoryBitmap(this.Width, this.Height, fmtOverride.Value);
            dst.SetPixels(0, 0, this);
            return dst;
        }

        #endregion

        #region API - Pixel Ops

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

        /// <summary>
        /// Fills all the pixels of this bitmap with <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TPixel">The pixel type. Must be compatible with <see cref="SpanBitmap.PixelFormat"/>.</typeparam>
        /// <param name="value">The value to fill.</param>
        public void SetPixels<TPixel>(TPixel value)
            where TPixel : unmanaged
        {
            OfType<TPixel>().SetPixels(value);
        }

        /// <summary>
        /// Blits the pixels of <paramref name="src"/> to this <see cref="SpanBitmap"/> at the <paramref name="dstX"/> and <paramref name="dstY"/> coordinates.
        /// </summary>
        /// <param name="dstX">The destination X position in this <see cref="SpanBitmap"/>.</param>
        /// <param name="dstY">The destination Y position in this <see cref="SpanBitmap"/>.</param>
        /// <param name="src">The source <see cref="SpanBitmap"/>.</param>
        /// <remarks>
        /// If possible, <paramref name="src"/> must have the same <see cref="PixelFormat"/> than this <see cref="SpanBitmap"/>.
        /// If formats do not match, a few, commonly used pixel formats are supported for pixel conversion.
        /// </remarks>
        public void SetPixels(int dstX, int dstY, SpanBitmap src)
        {
            Guard.IsTrue("this", !_Writable.IsEmpty);

            _Implementation.CopyPixels(this, dstX, dstY, src);
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

            new SpanBitmap(otherData,otherInfo).SetPixels(0, 0, this);

            return refreshed;
        }

        public void ApplyMirror(bool horizontal, bool vertical, bool multiThread = true)
        {
            switch(this.PixelFormat.ByteCount)
            {
                case 1: this.OfType<Byte>().ApplyMirror(horizontal, vertical, multiThread); return;
                case 2: this.OfType<UInt16>().ApplyMirror(horizontal, vertical, multiThread); return;
                case 3: this.OfType<Pixel.RGB24>().ApplyMirror(horizontal, vertical, multiThread); return;
                case 4: this.OfType<UInt32>().ApplyMirror(horizontal, vertical, multiThread); return;
                case 8: this.OfType<UInt64>().ApplyMirror(horizontal, vertical, multiThread); return;
                case 12: this.OfType<System.Numerics.Vector3>().ApplyMirror(horizontal, vertical, multiThread); return;
                case 16: this.OfType<System.Numerics.Vector4>().ApplyMirror(horizontal, vertical, multiThread); return;
            }

            throw new InvalidOperationException($"Unsupported pixel size: {this.PixelFormat.ByteCount}");
        }        

        #endregion

        #region API - IO

        public void Write(System.IO.Stream stream, Codecs.CodecFormat format, params Codecs.IBitmapEncoding[] factory)
        {
            var position = stream.Position;

            foreach (var f in factory)
            {
                if (stream.Position != position) throw new InvalidOperationException("incompatible codecs must not write to the stream.");

                if (f.TryWrite(stream, format, this)) return;                
            }
        }

        public void Save(string filePath, params Codecs.IBitmapEncoding[] factory)
        {
            var fmt = Codecs.CodecFactory.ParseFormat(filePath);

            foreach (var f in factory)
            {                
                using (var s = System.IO.File.Create(filePath))
                {
                    if (f.TryWrite(s, fmt, this)) return;
                }
            }

            throw new ArgumentException("invalid format", nameof(filePath));
        }

        #endregion        
    }    
}
