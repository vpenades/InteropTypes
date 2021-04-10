using System;
using System.Collections.Generic;
using System.Text;

using SIZE = System.Drawing.Size;
using POINT = System.Drawing.Point;

namespace InteropBitmaps
{
    /// <summary>
    /// Represents a Bitmap backed by a <see cref="Span{Byte}"/>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Info.ToDebuggerDisplayString(),nq}")]
    // [System.Diagnostics.DebuggerTypeProxy(typeof(Debug.SpanBitmapProxy))]
    public readonly ref partial struct SpanBitmap
    {
        #region lifecycle
        
        public unsafe SpanBitmap(IntPtr data, in BitmapInfo info, bool isReadOnly = false)
        {
            Guard.NotNull(nameof(data), data);

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

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly BitmapInfo _Info;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly Span<Byte> _Writable;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly ReadOnlySpan<Byte> _Readable;

        public override int GetHashCode() { return _Implementation.CalculateHashCode(_Readable, _Info); }

        #endregion

        #region properties

        public Span<Byte> WritableSpan => _Writable;

        public ReadOnlySpan<Byte> ReadableSpan => _Readable;

        #endregion

        #region properties - Info

        /// <summary>
        /// Gets the layout information of the bitmap; Width, Height, PixelFormat, etc.
        /// </summary>
        public BitmapInfo Info => _Info;

        /// <summary>
        /// Gets the pixel format of the bitmap.
        /// </summary>
        public Pixel.Format PixelFormat => _Info.PixelFormat;

        /// <summary>
        /// Gets the size of the bitmap, in pixels.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public SIZE Size => _Info.Size;

        /// <summary>
        /// Gets the width of the bitmap, in pixels.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Width => _Info.Width;

        /// <summary>
        /// Gets the height of the bitmap, in pixels.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
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
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public BitmapBounds Bounds => _Info.Bounds;

        #endregion        

        #region API - Buffers
        public ReadOnlySpan<Byte> GetScanlineBytes(int y) { return _Info.GetScanlineBytes(_Readable, y); }
        public Span<Byte> UseScanlineBytes(int y) { return _Info.UseScanlineBytes(_Writable, y); }

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

        public void SetPixels(System.Drawing.Color color)
        {
            switch(Info.PixelFormat.PackedFormat)
            {
                case Pixel.Alpha8.Code: SetPixels(Pixel.GetColor<Pixel.Alpha8>(color));break;
                case Pixel.Luminance8.Code: SetPixels(Pixel.GetColor<Pixel.Luminance8>(color)); break;
                case Pixel.Luminance16.Code: SetPixels(Pixel.GetColor<Pixel.Luminance16>(color)); break;

                case Pixel.BGR565.Code: SetPixels(Pixel.GetColor<Pixel.BGR565>(color)); break;
                case Pixel.BGRA4444.Code: SetPixels(Pixel.GetColor<Pixel.BGRA4444>(color)); break;
                case Pixel.BGRA5551.Code: SetPixels(Pixel.GetColor<Pixel.BGRA5551>(color)); break;

                case Pixel.RGB24.Code: SetPixels(Pixel.GetColor<Pixel.RGB24>(color)); break;
                case Pixel.BGR24.Code: SetPixels(Pixel.GetColor<Pixel.BGR24>(color)); break;

                case Pixel.RGBA32.Code: SetPixels(Pixel.GetColor<Pixel.RGBA32>(color)); break;
                case Pixel.BGRA32.Code: SetPixels(Pixel.GetColor<Pixel.BGRA32>(color)); break;
                case Pixel.ARGB32.Code: SetPixels(Pixel.GetColor<Pixel.ARGB32>(color)); break;

                case Pixel.VectorBGR.Code: SetPixels(Pixel.GetColor<Pixel.VectorBGR>(color)); break;
                case Pixel.VectorBGRA.Code: SetPixels(Pixel.GetColor<Pixel.VectorBGRA>(color)); break;
                case Pixel.VectorRGBA.Code: SetPixels(Pixel.GetColor<Pixel.VectorRGBA>(color)); break;
                default:throw new NotImplementedException();
            }
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

        public void SetPixels(Random rnd)
        {
            for(int y=0; y < this.Height; ++y)
            {
                var row = UseScanlineBytes(y);

                var fourCount = row.Length & ~3;

                var rowInts = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, int>(row.Slice(0, fourCount));

                for(int x=0; x < rowInts.Length; ++x)
                {
                    rowInts[x] = rnd.Next();
                }

                for (int x = rowInts.Length * 4; x < row.Length; ++x)
                {
                    row[x] = (Byte)rnd.Next();
                }
            }
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
            var refreshed = false;

            if (!this.Info.Equals(otherInfo))
            {
                otherInfo = this.Info;
                refreshed = true;
            }            

            if (otherData == null || otherData.Length < otherInfo.BitmapByteSize)
            {
                otherData = new byte[this.Info.BitmapByteSize];
                refreshed = true;
            }

            new SpanBitmap(otherData, otherInfo).SetPixels(0, 0, this);

            return refreshed;
        }

        public bool CopyTo<TPixel>(ref MemoryBitmap<TPixel> dst)
            where TPixel : unmanaged
        {
            var newFormat = dst.PixelFormat.IsDefined
                ? dst.PixelFormat
                : Pixel.Format.TryIdentifyPixel<TPixel>();

            var newInfo = this.Info.WithPixelFormat(newFormat);

            var refreshed = false;

            if (!dst.Info.Equals(newInfo))
            {
                dst = new MemoryBitmap<TPixel>(newInfo);
                refreshed = true;
            }

            dst.AsSpanBitmap().AsTypeless().SetPixels(0, 0, this);

            return refreshed;
        }

        public bool CopyTo(ref MemoryBitmap dst, Pixel.Format format)
        {
            var newInfo = this.Info.WithPixelFormat(format);

            var refreshed = false;

            if (!dst.Info.Equals(newInfo))
            {
                dst = new MemoryBitmap(newInfo);
                refreshed = true;
            }

            dst.AsSpanBitmap().SetPixels(0, 0, this);

            return refreshed;
        }

        /// <summary>
        /// Flips the pixels horizontally, vertically, or both, in place.
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <param name="multiThread"></param>
        public void ApplyMirror(bool horizontal, bool vertical, bool multiThread = true)
        {
            Processing._MirrorImplementation.ApplyMirror(this, horizontal, vertical, multiThread);
        }

        #endregion

        #region API - IO        

        public void Save(string filePath, params Codecs.IBitmapEncoder[] factory)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));

            var fmt = Codecs.CodecFactory.ParseFormat(filePath);
            var lzs = new Lazy<System.IO.Stream>(() => System.IO.File.Create(filePath));

            try { Codecs.CodecFactory.Write(lzs, fmt, factory, this); }
            finally { if (lzs.IsValueCreated) lzs.Value.Dispose(); }            
        }

        public void Write(System.IO.Stream stream, Codecs.CodecFormat format, params Codecs.IBitmapEncoder[] factory)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanWrite) throw new ArithmeticException(nameof(stream));

            var lzs = new Lazy<System.IO.Stream>(() => stream);

            Codecs.CodecFactory.Write(lzs, format, factory, this);
        }        

        public void Write(Lazy<System.IO.Stream> stream, Codecs.CodecFormat format, params Codecs.IBitmapEncoder[] factory)
        {
            Codecs.CodecFactory.Write(stream, format, factory, this);
        }
        
        #endregion        
    }    
}
