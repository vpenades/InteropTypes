using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using SIZE = System.Drawing.Size;
using POINT = System.Drawing.Point;

namespace InteropTypes.Graphics.Bitmaps
{
    using Diagnostics;

    /// <summary>
    /// Represents a Bitmap wrapped around a <see cref="Span{Byte}"/>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Info.ToDebuggerDisplayString(),nq}")]
    // [System.Diagnostics.DebuggerTypeProxy(typeof(Debug.SpanBitmapProxy<>))]
    public readonly ref partial struct SpanBitmap<TPixel>
        where TPixel : unmanaged
    {
        #region lifecycle

        public SpanBitmap(SpanBitmap<TPixel> other, bool isReadOnly = false)
        {
            _Info = other.Info;
            _Readable = other._Readable;
            _Writable = isReadOnly ? null : other._Writable;
        }

        public unsafe SpanBitmap(IntPtr data, in BitmapInfo info, bool isReadOnly = false)
        {
            Guard.NotNull(nameof(data), data);
            info.ArgumentIsCompatiblePixelFormat<TPixel>();

            _Info = info;

            var span = new Span<Byte>(data.ToPointer(), info.BitmapByteSize);

            _Readable = span;
            _Writable = isReadOnly ? null : span;
        }

        internal SpanBitmap(Span<Byte> data, in BitmapInfo info)
        {
            info.ArgumentIsCompatiblePixelFormat<TPixel>();
            _Info = info;
            _Readable = _Writable = data.Slice(0, _Info.BitmapByteSize);
        }

        internal SpanBitmap(ReadOnlySpan<Byte> data, in BitmapInfo info)
        {
            info.ArgumentIsCompatiblePixelFormat<TPixel>();
            _Info = info;
            _Readable = data.Slice(0, _Info.BitmapByteSize);
            _Writable = null;
        }

        public SpanBitmap(Span<Byte> data, int width, int height, int stepByteSize = 0)
           : this(data, width, height, PixelFormat.TryIdentifyFormat<TPixel>(), stepByteSize)
        { }

        public SpanBitmap(ReadOnlySpan<Byte> data, int width, int height, int stepByteSize = 0)
           : this(data, width, height, PixelFormat.TryIdentifyFormat<TPixel>(), stepByteSize)
        { }

        public unsafe SpanBitmap(Span<Byte> data, int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            _Info = BitmapInfo.Create<TPixel>(width, height, pixelFormat, scanlineSize);            
            _Readable = _Writable = data.Slice(0, _Info.BitmapByteSize);
        }

        public unsafe SpanBitmap(Span<TPixel> data, int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            _Info = BitmapInfo.Create<TPixel>(width, height, pixelFormat, scanlineSize);
            var span = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Byte>(data);
            _Readable = _Writable = span.Slice(0, _Info.BitmapByteSize);
        }

        public unsafe SpanBitmap(ReadOnlySpan<Byte> data, int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            _Info = BitmapInfo.Create<TPixel>(width, height, pixelFormat, scanlineSize);            
            _Readable = data.Slice(0, _Info.BitmapByteSize);
            _Writable = null;
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly BitmapInfo _Info;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly Span<Byte> _Writable;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly ReadOnlySpan<Byte> _Readable;

        /// <inheritdoc/>
        public override int GetHashCode() { throw new NotSupportedException("Spans don't support GetHashCode"); }

        /// <inheritdoc/>
        public override bool Equals(object obj) { throw new NotSupportedException(); }

        /// <inheritdoc/>
        public static bool operator ==(in SpanBitmap<TPixel> a, in SpanBitmap<TPixel> b) { return AreEqual(a, b); }

        /// <inheritdoc/>
        public static bool operator !=(in SpanBitmap<TPixel> a, in SpanBitmap<TPixel> b) { return !AreEqual(a, b); }

        /// <summary>
        /// Indicates whether both instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The first instance.</param>
        /// <returns>true if both instances represent the same value.</returns>
        public static bool AreEqual(in SpanBitmap<TPixel> a, in SpanBitmap<TPixel> b)
        {
            if (a.Info != b.Info) return false;
            if (a._Readable != b._Readable) return false;
            return true;
        }

        #endregion

        #region properties

        public Span<Byte> WritableBytes => _Writable;       

        public ReadOnlySpan<Byte> ReadableBytes => _Readable;

        #endregion

        #region properties - Info

        public bool IsEmpty => _Info.IsEmpty || _Readable.IsEmpty;

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

        [System.Diagnostics.DebuggerStepThrough]
        public SpanBitmap<TPixel> AsReadOnly() { return new SpanBitmap<TPixel>(this, true); }
        public ReadOnlySpan<Byte> GetScanlineBytes(int y) { return _Info.GetScanlineBytes(_Readable, y); }        
        public ReadOnlySpan<TPixel> GetScanlinePixels(int y) { return _Info.GetScanlinePixels<TPixel>(_Readable, y); }        
        public Span<Byte> UseScanlineBytes(int y) { return _Info.UseScanlineBytes(_Writable, y); }        
        public Span<TPixel> UseScanlinePixels(int y) { return _Info.UseScanlinePixels<TPixel>(_Writable, y); }

        #endregion

        #region API - Cast

        public unsafe void PinWritablePointer(Action<PointerBitmap> onPin)
        {
            Guard.IsFalse(nameof(SpanBitmap), _Writable.IsEmpty);
            _SpanBitmapImpl.PinWritablePointer(_Writable, _Info, onPin);
        }

        public unsafe void PinReadablePointer(Action<PointerBitmap> onPin)
        {
            Guard.IsFalse(nameof(SpanBitmap), _Readable.IsEmpty);
            _SpanBitmapImpl.PinReadablePointer(_Readable, _Info, onPin);
        }

        public unsafe TResult PinReadablePointer<TResult>(PointerBitmap.Function1<TResult> onPin)
        {
            Guard.IsFalse(nameof(SpanBitmap), _Readable.IsEmpty);
            return _SpanBitmapImpl.PinReadablePointer(_Readable, _Info, onPin);
        }

        public static implicit operator SpanBitmap(SpanBitmap<TPixel> other)
        {
            return other._Writable.IsEmpty
                ? new SpanBitmap(other._Readable, other._Info)
                : new SpanBitmap(other._Writable, other._Info);
        }

        

        public unsafe SpanBitmap<TDstPixel> ReinterpretAs<TDstPixel>()
            where TDstPixel : unmanaged
        {
            if (sizeof(TPixel) != sizeof(TDstPixel)) throw new ArgumentException("pixels size mismatch.");            

            return _Writable.IsEmpty
                ? new SpanBitmap<TDstPixel>(_Readable, _Info)
                : new SpanBitmap<TDstPixel>(_Writable, _Info);
        }
        
        public MemoryBitmap<TPixel> ToMemoryBitmap(PixelFormat? fmtOverride = null)
        {
            fmtOverride ??= this.PixelFormat;

            if (fmtOverride.Value == this.PixelFormat) return new MemoryBitmap<TPixel>(_Readable.ToArray(), _Info);

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
                var dst = UseScanlinePixels(y);
                dst.Fill(value);
            }
        }

        public void SetPixels(int dstX, int dstY, SpanBitmap<TPixel> src)
        {
            Guard.IsTrue("this", !_Writable.IsEmpty);

            _Implementation.CopyPixels(this, dstX, dstY, src);
        }

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

        public bool CopyTo(ref MemoryBitmap<TPixel> other)
        {
            var refreshed = false;

            if (!this.Info.Equals(other.Info))
            {
                other = new MemoryBitmap<TPixel>(this.Info);
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

        #endregion

        #region API - Effects & Transfers

        public void SetPixels<TSrcPixel>(in Matrix3x2 location, SpanBitmap<TSrcPixel> src,float opacity = 1)
            where TSrcPixel : unmanaged
        {
            var xform = new Processing.BitmapTransform(location, true, opacity);
            this.TransferFrom(src, xform);
        }

        public void SetPixels<TSrcPixel>(in Matrix3x2 location, SpanBitmap<TSrcPixel> src, bool useBilinear, float opacity = 1)
            where TSrcPixel : unmanaged
        {
            var xform = new Processing.BitmapTransform(location, useBilinear, opacity);
            this.TransferFrom(src, xform);
        }

        public void ApplyEffect(SpanBitmap.IEffect effect)
        {
            // try applying the effect with a known pixel type:
            if (effect.TryApplyTo<TPixel>(this)) return;

            // try applying the effect as bytes:
            if (effect.TryApplyTo(this.AsTypeless())) return;

            throw new NotSupportedException();
        }

        public void TransferFrom<TSrcPixel>(in SpanBitmap<TSrcPixel> src, SpanBitmap.ITransfer transfer)
            where TSrcPixel : unmanaged
        {
            // 1st try with explicit exact types:
            if (transfer is SpanBitmap.ITransfer<TSrcPixel, TPixel> xtransfer)
            {
                if (xtransfer.TryTransfer(src, this)) return;
            }

            // 2nd try with generic exact types:
            if (transfer.TryTransfer<TSrcPixel,TPixel>(src.AsReadOnly(), this)) return;
            

            // 3rd try with generic exact and matching types:
            if (typeof(TPixel) == typeof(TSrcPixel))
            {
                var srcx = src.ReinterpretAs<TPixel>();
                if (transfer.TryTransfer<TPixel>(srcx, this)) return;
            }

            // 4th try typeless:
            if (transfer.TryTransfer(src.AsTypeless(), this.AsTypeless())) return;

            // 5th unable to transfer.
            throw new NotSupportedException($"Transfers from {typeof(TSrcPixel).Name} to {typeof(TPixel).Name} with {transfer.GetType().Name} are not supported.");
        }

        #endregion

        #region API - IO

        public void Write(System.IO.Stream stream, Codecs.CodecFormat format, params Codecs.IBitmapEncoder[] factory)
        {
            AsTypeless().Write(stream, format, factory);
        }

        public void Save(string filePath, params Codecs.IBitmapEncoder[] factory)
        {
            AsTypeless().Save(filePath, factory);
        }

        #endregion

        #region nested types
        public ref struct PixelEnumerator
        {
            private readonly SpanBitmap<TPixel> _Span;
            private ReadOnlySpan<TPixel> _Line;
            private int _IndexX;
            private int _IndexY;

            internal PixelEnumerator(SpanBitmap<TPixel> span)
            {
                _Span = span;
                _IndexX = -1;
                _IndexY = 0;
                _Line = span.GetScanlinePixels(_IndexY);
            }

            public bool MoveNext()
            {
                int x = _IndexX + 1;
                if (x < _Span.Width)
                {
                    _IndexX = x;
                    return true;
                }

                _IndexX = 0;

                int y = _IndexY + 1;
                if (y < _Span.Height)
                {
                    _IndexY = y;
                    _Line = _Span.GetScanlinePixels(_IndexY);
                    return true;
                }

                return false;
            }

            public ref readonly TPixel Current => ref _Line[_IndexX];
        }

        public ref struct RowEnumerator
        {
            private readonly SpanBitmap<TPixel> _Span;
            private ReadOnlySpan<TPixel> _Line;            
            private int _IndexY;

            internal RowEnumerator(SpanBitmap<TPixel> span)
            {
                _Span = span;
                _Line = default;
                _IndexY = -1;                
            }

            public bool MoveNext()
            {
                int y = _IndexY + 1;
                if (y < _Span.Height)
                {
                    _IndexY = y;
                    _Line = _Span.GetScanlinePixels(_IndexY);
                    return true;
                }

                return false;
            }

            public ReadOnlySpan<TPixel> Current => _Line;
        }

        #endregion
    }
}
