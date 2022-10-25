using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using SIZE = System.Drawing.Size;

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
        public override int GetHashCode()
        {
            #pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
            throw new NotSupportedException("Spans don't support GetHashCode");
            #pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            #pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
            throw new NotSupportedException("Spans don't support Equality");
            #pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
        }

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

        [System.Diagnostics.DebuggerStepThrough]
        public ReadOnlySpan<Byte> GetScanlineBytes(int y) { return _Info.GetScanlineBytes(_Readable, y); }

        [System.Diagnostics.DebuggerStepThrough]
        public ReadOnlySpan<TPixel> GetScanlinePixels(int y) { return _Info.GetScanlinePixels<TPixel>(_Readable, y); }

        [System.Diagnostics.DebuggerStepThrough]
        public Span<Byte> UseScanlineBytes(int y) { return _Info.UseScanlineBytes(_Writable, y); }

        [System.Diagnostics.DebuggerStepThrough]
        public Span<TPixel> UseScanlinePixels(int y) { return _Info.UseScanlinePixels<TPixel>(_Writable, y); }

        public bool TryUseAllPixels(out Span<TPixel> pixels)
        {
            if (!_Info.IsContinuous) { pixels = default; return false; }

            var bytes = _Writable.Slice(0, _Info.BitmapByteSize);

            pixels = System.Runtime.InteropServices.MemoryMarshal
                .Cast<Byte, TPixel>(bytes);

            System.Diagnostics.Debug.Assert(this.Width * this.Height == pixels.Length);

            return true;
        }

        public bool TryGetAllPixels(out ReadOnlySpan<TPixel> pixels)
        {
            if (!_Info.IsContinuous) { pixels = default; return false; }

            var bytes = _Readable.Slice(0, _Info.BitmapByteSize);

            pixels = System.Runtime.InteropServices.MemoryMarshal
                .Cast<Byte, TPixel>(bytes);

            System.Diagnostics.Debug.Assert(this.Width * this.Height == pixels.Length);

            return true;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public SpanBitmap<TPixel> Slice(in BitmapBounds rect)
        {
            var (offset, info) = _Info.Slice(rect);

            if (info.BitmapByteSize == 0) return new SpanBitmap<TPixel>(Span<byte>.Empty, info);

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

        /// <summary>
        /// Casts the current object to use <typeparamref name="TDstPixel"/>
        /// </summary>
        /// <typeparam name="TDstPixel">The pixel type to cast to.</typeparam>
        /// <returns>A casted bitmap</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <remarks>
        /// This method is usually used in generic methods where we need to cast the generic pixel type to the explicit pixel type.
        /// </remarks>
        public SpanBitmap<TDstPixel> AsExplicit<TDstPixel>()
            where TDstPixel : unmanaged
        {
            if (typeof(TPixel) != typeof(TDstPixel)) throw new ArgumentException("Pixel type mismatch.");

            return _Writable.IsEmpty
                ? new SpanBitmap<TDstPixel>(_Readable, _Info)
                : new SpanBitmap<TDstPixel>(_Writable, _Info);
        }

        public unsafe SpanBitmap<TDstPixel> ReinterpretAs<TDstPixel>()
            where TDstPixel : unmanaged
        {
            if (sizeof(TPixel) != sizeof(TDstPixel)) throw new ArgumentException("Pixel size mismatch.");

            var info = _Info.WithPixelFormat(PixelFormat.TryIdentifyFormat<TDstPixel>());

            return _Writable.IsEmpty
                ? new SpanBitmap<TDstPixel>(_Readable, info)
                : new SpanBitmap<TDstPixel>(_Writable, info);
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

        public void SetPixels(System.Drawing.Color color) { SetPixels(Pixel.GetColor<TPixel>(color)); }

        public void SetPixels(TPixel value)
        {            
            Guard.IsTrue("this", !_Writable.IsEmpty);

            if (TryUseAllPixels(out var allPixels))
            {
                allPixels.Fill(value);
                return;
            }

            for (int y = 0; y < _Info.Height; ++y)
            {
                UseScanlinePixels(y).Fill(value);
            }
        }        

        public void SetPixels<TSrcPixel>(int dstX, int dstY, SpanBitmap<TSrcPixel> src)
            where TSrcPixel: unmanaged
        {
            if (typeof(TPixel) == typeof(TSrcPixel))
            {
                _Implementation.CopyPixels(this, dstX, dstY, src);
            }
            else
            {
                _Implementation.ConvertPixels(this, dstX, dstY, src);
            }
        }            

        public void SetPixels<TSrcPixel>(in Matrix3x2 location, SpanBitmap<TSrcPixel> src, bool useBilinear, in Pixel.RGB96F.MulAdd pixelOp)
            where TSrcPixel : unmanaged
        {            
            this.SetPixels(src, new Processing.BitmapTransform(location, useBilinear, pixelOp));
        }

        public void SetPixels<TSrcPixel, TTransfer>(in SpanBitmap<TSrcPixel> src, in TTransfer transfer)
            where TSrcPixel : unmanaged
            where TTransfer : SpanBitmap.ITransfer
        {
            Guard.IsTrue("this", !_Writable.IsEmpty);

            // 1rd try with generic exact and matching types:
            if (typeof(TPixel) == typeof(TSrcPixel))
            {
                var srcx = src.AsExplicit<TPixel>();
                if (transfer.TryTransfer<TPixel>(srcx, this)) return;
            }

            // 2nd try with generic exact types:
            if (transfer.TryTransfer(src, this)) return;

            // 3rd try with explicit exact types:
            if (transfer is SpanBitmap.ITransfer<TSrcPixel, TPixel> xtransfer)
            {
                if (xtransfer.TryTransfer(src, this)) return;
            }            

            // 4th try typeless:
            if (transfer.TryTransfer(src.AsTypeless(), this.AsTypeless())) return;

            // 5th unable to transfer.
            throw new NotSupportedException($"Transfers from {typeof(TSrcPixel).Name} to {typeof(TPixel).Name} with {transfer.GetType().Name} are not supported.");
        }

        #endregion

        #region API - Effects & Transfers        

        public bool CopyTo(ref MemoryBitmap<TPixel> other, Func<int, Byte[]> memFactory = null)
        {
            var refreshed = false;

            if (!this.Info.Equals(other.Info))
            {
                if (memFactory == null)
                {
                    other = new MemoryBitmap<TPixel>(this.Info);
                }
                else
                {
                    var blen = this.Info.BitmapByteSize;

                    var buff = memFactory(blen);
                    if (buff == null || buff.Length < blen) throw new ArgumentException("invalid buffer", nameof(memFactory));

                    other = new MemoryBitmap<TPixel>(buff, this.Info);
                }

                refreshed = true;
            }

            other.AsSpanBitmap().SetPixels(0, 0, this);

            return refreshed;
        }

        public MemoryBitmap<TDstPixel> ToMemoryBitmap<TDstPixel>(PixelFormat? fmt = null)
            where TDstPixel : unmanaged
        {
            if (!fmt.HasValue) fmt = PixelFormat.TryIdentifyFormat<TDstPixel>();
            var dst = new MemoryBitmap<TDstPixel>(this.Width, this.Height, fmt.Value);

            dst.AsSpanBitmap().SetPixels(0, 0, this);

            return dst;
        }

        public void ApplyEffect(SpanBitmap.IEffect effect)
        {
            // try applying the effect with a known pixel type:
            if (effect.TryApplyTo<TPixel>(this)) return;

            // try applying the effect as bytes:
            if (effect.TryApplyTo(this.AsTypeless())) return;

            throw new NotSupportedException();
        }

        public void Apply(Pixel.IApplyTo<TPixel> pixelEffect)
        {
            for (int y = 0; y < this.Height; ++y)
            {
                var row = this.UseScanlinePixels(y);
                
                for(int i=0; i < row.Length; ++i)
                {
                    pixelEffect.ApplyTo(ref row[i]);
                }
            }
        }

        [Obsolete("Use Apply(Pixel.XXXX.MulAdd)")]
        public void ApplyAddMultiply(float addition, float multiply)
        {
            ApplyMultiplyAdd(multiply, addition * multiply);
        }

        [Obsolete("Use Apply(Pixel.XXXX.MulAdd)")]
        public void ApplyMultiplyAdd(float multiply, float addition)
        {
            if (typeof(TPixel) == typeof(Single)
                || typeof(TPixel) == typeof(Vector2)
                || typeof(TPixel) == typeof(Vector3)
                || typeof(TPixel) == typeof(Vector4)                
                || typeof(TPixel) == typeof(Pixel.Luminance32F)
                || typeof(TPixel) == typeof(Pixel.RGB96F)
                || typeof(TPixel) == typeof(Pixel.BGR96F)
                || typeof(TPixel) == typeof(Pixel.RGBP128F)
                || typeof(TPixel) == typeof(Pixel.BGRP128F)
                )
            {
                for (int y = 0; y < this.Height; ++y)
                {
                    var row = this.UseScanlinePixels(y);
                    var rowf = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, float>(row);
                    Vector4Streaming.MultiplyAdd(rowf, multiply, addition);
                }

                return;
            }

            throw new NotImplementedException();
        }

        [Obsolete("Use Apply(Pixel.XXXX.MulAdd)")]
        public void ApplyAddMultiply(Vector3 addition, Vector3 multiply)
        {
            ApplyMultiplyAdd(multiply, addition * multiply);
        }

        [Obsolete("Use Apply(Pixel.XXXX.MulAdd)")]
        public void ApplyMultiplyAdd(Vector3 multiply, Vector3 addition)
        {
            if (typeof(TPixel) == typeof(Vector3)
                || typeof(TPixel) == typeof(Pixel.RGB96F)
                || typeof(TPixel) == typeof(Pixel.BGR96F)
                )
            {
                for (int y = 0; y < this.Height; ++y)
                {
                    var row = this.UseScanlinePixels(y);
                    var rowf = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Vector3>(row);
                    
                    for(int x=0; x < rowf.Length; ++x)
                    {
                        rowf[x] *= multiply;
                        rowf[x] += addition;
                    }
                }

                return;
            }

            throw new NotImplementedException();
        }

        public void ApplyClamp(float min, float max)
        {
            if (typeof(TPixel) == typeof(Single)
                || typeof(TPixel) == typeof(Vector2)
                || typeof(TPixel) == typeof(Vector3)
                || typeof(TPixel) == typeof(Vector4)
                || typeof(TPixel) == typeof(Pixel.Luminance32F)
                || typeof(TPixel) == typeof(Pixel.RGB96F)
                || typeof(TPixel) == typeof(Pixel.BGR96F)
                || typeof(TPixel) == typeof(Pixel.RGBP128F)
                || typeof(TPixel) == typeof(Pixel.BGRP128F)
                )
            {
                for (int y = 0; y < this.Height; ++y)
                {
                    var row = this.UseScanlinePixels(y);
                    var rowf = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, float>(row);

                    for (int x = 0; x < rowf.Length; ++x)
                    {                        
                        var v = rowf[x];
                        if (v < min) rowf[x] = min;
                        else if (v > max) rowf[x] = max;
                    }
                }

                return;
            }

            throw new NotImplementedException();
        }

        public void ApplyClamp(Vector3 min, Vector3 max)
        {
            if (typeof(TPixel) == typeof(Vector3)
                || typeof(TPixel) == typeof(Pixel.RGB96F)
                || typeof(TPixel) == typeof(Pixel.BGR96F)
                )
            {
                for (int y = 0; y < this.Height; ++y)
                {
                    var row = this.UseScanlinePixels(y);
                    var rowf = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Vector3>(row);

                    for (int x = 0; x < rowf.Length; ++x)
                    {
                        rowf[x] = Vector3.Clamp(rowf[x], min, max);
                    }
                }

                return;
            }

            throw new NotImplementedException();
        }
                
        public void SetPixelsFromYUV420(SpanBitmap<Byte> srcY, SpanBitmap<Byte> srcU, SpanBitmap<Byte> srcV)
        {
            var h = Math.Min(this.Height, srcY.Height) -1;

            for (int y = 0; y < h; y += 2)
            {
                var dstRow0 = this.UseScanlinePixels(y + 0);
                var dstRow1 = this.UseScanlinePixels(y + 1);

                var srcRowY0 = srcY.GetScanlinePixels(y + 0);
                var srcRowY1 = srcY.GetScanlinePixels(y + 1);

                var srcRowU = srcU.GetScanlinePixels(Math.Min(y / 2, srcU.Height - 1));
                var srcRowV = srcV.GetScanlinePixels(Math.Min(y / 2, srcV.Height - 1));

                Pixel.YUV24.KernelRGB.TransferYUV420(dstRow0, dstRow1, srcRowY0, srcRowY1, srcRowU, srcRowV);
            }
        }

        public void SetPixelsFromYUV420(SpanBitmap<Byte> srcY, SpanBitmap<ushort> srcU, SpanBitmap<ushort> srcV)
        {
            var h = Math.Min(this.Height, srcY.Height) - 1;

            for (int y = 0; y < h; y += 2)
            {
                var dstRow0 = this.UseScanlinePixels(y + 0);
                var dstRow1 = this.UseScanlinePixels(y + 1);

                var srcRowY0 = srcY.GetScanlinePixels(y + 0);
                var srcRowY1 = srcY.GetScanlinePixels(y + 1);

                var srcRowU = srcU.GetScanlinePixels(Math.Min(y / 2, srcU.Height - 1));
                var srcRowV = srcV.GetScanlinePixels(Math.Min(y / 2, srcV.Height - 1));

                Pixel.YUV24.KernelRGB.TransferYUV420(dstRow0, dstRow1, srcRowY0, srcRowY1, srcRowU, srcRowV);
            }
        }

        public void SetPixelsFromYUY2(SpanBitmap<ushort> src)
        {
            var h = Math.Min(this.Height, src.Height);

            for (int y = 0; y < h; ++y)
            {
                var srcRow = src.GetScanlinePixels(y);
                var dstRow = this.UseScanlinePixels(y);                              

                Pixel.YUV24.KernelRGB.TransferYUY2(dstRow, srcRow);
            }
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

            public readonly ReadOnlySpan<TPixel> Current => _Line;
        }

        #endregion
    }
}
