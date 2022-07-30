using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

using SIZE = System.Drawing.Size;

namespace InteropTypes.Graphics.Bitmaps
{
    using Diagnostics;

    /// <summary>
    /// Represents a Bitmap backed by a <see cref="Span{Byte}"/>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Info.ToDebuggerDisplayString(),nq}")]
    // [System.Diagnostics.DebuggerTypeProxy(typeof(Debug.SpanBitmapProxy))]
    public readonly ref partial struct SpanBitmap
    {
        #region lifecycle

        public SpanBitmap(SpanBitmap other, bool isReadOnly = false)
        {
            _Info = other.Info;
            _Readable = other._Readable;
            _Writable = isReadOnly ? null : other._Writable;
        }

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

        public SpanBitmap(Span<Byte> data, int width, int height, PixelFormat pixelFormat, int stepByteSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, stepByteSize);
            _Readable = _Writable = data.Slice(0, _Info.BitmapByteSize);
        }

        public SpanBitmap(ReadOnlySpan<Byte> data, int width, int height, PixelFormat pixelFormat, int stepByteSize = 0)
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
            throw new NotSupportedException();
            #pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
        }

        /// <inheritdoc/>
        public static bool operator ==(in SpanBitmap a, in SpanBitmap b) { return AreEqual(a, b); }

        /// <inheritdoc/>
        public static bool operator !=(in SpanBitmap a, in SpanBitmap b) { return !AreEqual(a, b); }

        /// <summary>
        /// Indicates whether both instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The first instance.</param>
        /// <returns>true if both instances represent the same value.</returns>
        public static bool AreEqual(in SpanBitmap a, in SpanBitmap b)
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
        public SpanBitmap AsReadOnly() { return new SpanBitmap(this, true); }

        [System.Diagnostics.DebuggerStepThrough]
        public ReadOnlySpan<Byte> GetScanlineBytes(int y) { return _Info.GetScanlineBytes(_Readable, y); }

        [System.Diagnostics.DebuggerStepThrough]
        public Span<Byte> UseScanlineBytes(int y) { return _Info.UseScanlineBytes(_Writable, y); }

        /// <summary>
        /// Crops the current <see cref="SpanBitmap"/> sharing the original source memory.
        /// </summary>
        /// <param name="rect">The region to crop.</param>
        /// <returns>A <see cref="SpanBitmap"/> representing the cropped region.</returns>        
        [System.Diagnostics.DebuggerStepThrough]
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

        [System.Diagnostics.DebuggerStepThrough]
        public unsafe SpanBitmap<TPixel> OfTypeOrDefault<TPixel>()
            where TPixel : unmanaged
        {
            return this.Info.IsEmpty
                ? new SpanBitmap<TPixel>()
                : this.OfType<TPixel>();
        }        

        /// <summary>
        /// Returns a pixel specific <see cref="SpanBitmap{TPixel}"/>.
        /// </summary>
        /// <typeparam name="TPixel">The pixel type.</typeparam>        
        /// <returns>A <see cref="SpanBitmap{TPixel}"/></returns>
        /// <remarks>
        /// This is the opposite operation of <see cref="SpanBitmap{TPixel}.AsTypeless"/>
        /// </remarks>
        [System.Diagnostics.DebuggerStepThrough]
        public unsafe SpanBitmap<TPixel> OfType<TPixel>()
            where TPixel : unmanaged
        {
            _Info.ArgumentIsCompatiblePixelFormat<TPixel>();

            return _Writable.IsEmpty
                ? new SpanBitmap<TPixel>(_Readable, _Info)
                : new SpanBitmap<TPixel>(_Writable, _Info);
        }

        /// <summary>
        /// Creates a <see cref="MemoryBitmap"/> copy from this <see cref="SpanBitmap"/>.
        /// </summary>
        /// <param name="fmtOverride">Format override.</param>
        /// <returns>A new <see cref="MemoryBitmap"/>.</returns>
        public MemoryBitmap ToMemoryBitmap(PixelFormat? fmtOverride = null)
        {
            fmtOverride = fmtOverride ?? this.PixelFormat;

            if (fmtOverride.Value == this.PixelFormat) return new MemoryBitmap(_Readable.ToArray(), _Info);

            var dst = new MemoryBitmap(this.Width, this.Height, fmtOverride.Value);
            dst.SetPixels(0, 0, this);
            return dst;
        }

        #endregion

        #region API - Pixel Ops

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
            switch(Info.PixelFormat.Code)
            {
                case Pixel.Alpha8.Code: OfType<Pixel.Alpha8>().SetPixels(color);break;

                case Pixel.Luminance8.Code: OfType<Pixel.Luminance8>().SetPixels(color); break;
                case Pixel.Luminance16.Code: OfType<Pixel.Luminance16>().SetPixels(color); break;
                case Pixel.Luminance32F.Code: OfType<Pixel.Luminance32F>().SetPixels(color); break;

                case Pixel.BGR565.Code: OfType< Pixel.BGR565>().SetPixels(color); break;
                case Pixel.BGRA4444.Code: OfType<Pixel.BGRA4444>().SetPixels(color); break;
                case Pixel.BGRA5551.Code: OfType<Pixel.BGRA5551>().SetPixels(color); break;

                case Pixel.RGB24.Code: OfType<Pixel.RGB24>().SetPixels(color); break;
                case Pixel.BGR24.Code: OfType<Pixel.BGR24>().SetPixels(color); break;

                case Pixel.RGBA32.Code: OfType<Pixel.RGBA32>().SetPixels(color); break;
                case Pixel.BGRA32.Code: OfType<Pixel.BGRA32>().SetPixels(color); break;
                case Pixel.ARGB32.Code: OfType<Pixel.ARGB32>().SetPixels(color); break;

                case Pixel.RGBP32.Code: OfType<Pixel.RGBP32>().SetPixels(color); break;
                case Pixel.BGRP32.Code: OfType<Pixel.BGRP32>().SetPixels(color); break;
                case Pixel.PRGB32.Code: OfType<Pixel.PRGB32>().SetPixels(color); break;

                case Pixel.RGB96F.Code: OfType<Pixel.RGB96F>().SetPixels(color); break;
                case Pixel.BGR96F.Code: OfType<Pixel.BGR96F>().SetPixels(color); break;

                case Pixel.BGRA128F.Code: OfType<Pixel.BGRA128F>().SetPixels(color); break;
                case Pixel.RGBA128F.Code: OfType<Pixel.RGBA128F>().SetPixels(color); break;

                case Pixel.BGRP128F.Code: OfType<Pixel.BGRP128F>().SetPixels(color); break;
                case Pixel.RGBP128F.Code: OfType<Pixel.RGBP128F>().SetPixels(color); break;

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

        /// <summary>
        /// Draws <paramref name="src"/> at the location defined by <paramref name="dstSRT"/>.
        /// </summary>
        /// <param name="dstSRT">Where to draw the image.</param>
        /// <param name="src">Image to draw.</param>
        /// <remarks>
        /// This is equivalent to OpenCV's WarpAffine
        /// </remarks>
        public void SetPixels(in Matrix3x2 dstSRT, SpanBitmap src) { SetPixels(src, new Processing.BitmapTransform(dstSRT, 1)); }

        public void SetPixels(in SpanBitmap src, ITransfer transfer)            
        {
            if (src.PixelFormat != this.PixelFormat) throw new PixelFormatNotSupportedException(src.PixelFormat, nameof(src));

            switch (this.PixelFormat.Code)
            {
                case Pixel.Alpha8.Code: this.OfType<Pixel.Alpha8>().SetPixels(src.OfType<Pixel.Alpha8>(), transfer); break;

                case Pixel.Luminance8.Code: this.OfType<Pixel.Luminance8>().SetPixels(src.OfType<Pixel.Luminance8>(), transfer); break;
                case Pixel.Luminance16.Code: this.OfType<Pixel.Luminance16>().SetPixels(src.OfType<Pixel.Luminance16>(), transfer); break;
                case Pixel.Luminance32F.Code: this.OfType<Pixel.Luminance32F>().SetPixels(src.OfType<Pixel.Luminance32F>(), transfer); break;

                case Pixel.BGR565.Code: this.OfType<Pixel.BGR565>().SetPixels(src.OfType<Pixel.BGR565>(), transfer); break;
                case Pixel.BGRA5551.Code: this.OfType<Pixel.BGRA5551>().SetPixels(src.OfType<Pixel.BGRA5551>(), transfer); break;
                case Pixel.BGRA4444.Code: this.OfType<Pixel.BGRA4444>().SetPixels(src.OfType<Pixel.BGRA4444>(), transfer); break;

                case Pixel.BGR24.Code: this.OfType<Pixel.BGR24>().SetPixels(src.OfType<Pixel.BGR24>(), transfer); break;
                case Pixel.RGB24.Code: this.OfType<Pixel.RGB24>().SetPixels(src.OfType<Pixel.RGB24>(), transfer); break;

                case Pixel.BGRA32.Code: this.OfType<Pixel.BGRA32>().SetPixels(src.OfType<Pixel.BGRA32>(), transfer); break;
                case Pixel.RGBA32.Code: this.OfType<Pixel.RGBA32>().SetPixels(src.OfType<Pixel.RGBA32>(), transfer); break;
                case Pixel.ARGB32.Code: this.OfType<Pixel.ARGB32>().SetPixels(src.OfType<Pixel.ARGB32>(), transfer); break;

                case Pixel.BGRP32.Code: this.OfType<Pixel.BGRP32>().SetPixels(src.OfType<Pixel.BGRP32>(), transfer); break;
                case Pixel.RGBP32.Code: this.OfType<Pixel.RGBP32>().SetPixels(src.OfType<Pixel.RGBP32>(), transfer); break;
                case Pixel.PRGB32.Code: this.OfType<Pixel.PRGB32>().SetPixels(src.OfType<Pixel.PRGB32>(), transfer); break;

                case Pixel.RGB96F.Code: this.OfType<Pixel.RGB96F>().SetPixels(src.OfType<Pixel.RGB96F>(), transfer); break;
                case Pixel.BGR96F.Code: this.OfType<Pixel.BGR96F>().SetPixels(src.OfType<Pixel.BGR96F>(), transfer); break;

                case Pixel.BGRA128F.Code: this.OfType<Pixel.BGRA128F>().SetPixels(src.OfType<Pixel.BGRA128F>(), transfer); break;
                case Pixel.RGBA128F.Code: this.OfType<Pixel.RGBA128F>().SetPixels(src.OfType<Pixel.RGBA128F>(), transfer); break;

                default: throw new NotSupportedException();
            }
        }


        public void ApplyEffect(IEffect effect)            
        {
            if (effect.TryApplyTo(this)) return;

            switch (this.PixelFormat.Code)
            {
                case Pixel.Alpha8.Code: this.OfType<Pixel.Alpha8>().ApplyEffect(effect); break;

                case Pixel.Luminance8.Code: this.OfType<Pixel.Luminance8>().ApplyEffect(effect); break;
                case Pixel.Luminance16.Code: this.OfType<Pixel.Luminance16>().ApplyEffect(effect); break;
                case Pixel.Luminance32F.Code: this.OfType<Pixel.Luminance32F>().ApplyEffect(effect); break;

                case Pixel.BGR565.Code: this.OfType<Pixel.BGR565>().ApplyEffect(effect); break;
                case Pixel.BGRA5551.Code: this.OfType<Pixel.BGRA5551>().ApplyEffect(effect); break;
                case Pixel.BGRA4444.Code: this.OfType<Pixel.BGRA4444>().ApplyEffect(effect); break;

                case Pixel.BGR24.Code: this.OfType<Pixel.BGR24>().ApplyEffect(effect); break;
                case Pixel.RGB24.Code: this.OfType<Pixel.RGB24>().ApplyEffect(effect); break;

                case Pixel.BGRA32.Code: this.OfType<Pixel.BGRA32>().ApplyEffect(effect); break;
                case Pixel.RGBA32.Code: this.OfType<Pixel.RGBA32>().ApplyEffect(effect); break;
                case Pixel.ARGB32.Code: this.OfType<Pixel.ARGB32>().ApplyEffect(effect); break;

                case Pixel.RGB96F.Code: this.OfType<Pixel.RGB96F>().ApplyEffect(effect); break;
                case Pixel.BGR96F.Code: this.OfType<Pixel.BGR96F>().ApplyEffect(effect); break;
                case Pixel.BGRA128F.Code: this.OfType<Pixel.BGRA128F>().ApplyEffect(effect); break;
                case Pixel.RGBA128F.Code: this.OfType<Pixel.RGBA128F>().ApplyEffect(effect); break;

                default: throw new NotSupportedException();
            }
        }


        [Obsolete("Use ToMemoryBitmap", true)]
        public MemoryBitmap<TPixel> CloneAs<TPixel>() where TPixel : unmanaged { return default; }

        public MemoryBitmap<TPixel> ToMemoryBitmap<TPixel>()
            where TPixel : unmanaged
        {
            MemoryBitmap<TPixel> newBitmap = default;
            CopyTo(ref newBitmap);
            return newBitmap;
        }

        public bool CopyTo(ref MemoryBitmap other)
        {
            var refreshed = false;

            if (!BitmapInfo.AreEqual(this.Info, other.Info, false))
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
                : PixelFormat.TryIdentifyFormat<TPixel>();

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

        public bool CopyTo(ref MemoryBitmap dst, PixelFormat format)
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
        /// Tries to in-place convert the pixels to a RGB? format.
        /// </summary>
        /// <param name="newBitmap">If the conversion is successful, a new span with the new format</param>
        /// <returns>true if the conversion was successful</returns>
        /// <remarks>
        /// The current bitmap is no longer valid because its internal
        /// pixel format will not match the actual data.
        /// </remarks>
        public bool TrySetPixelsFormatRGBX(out SpanBitmap newBitmap)
        {
            if (!PixelFormat.TryGetFormatAsRGBX(this.PixelFormat, out var newFmt))
            {
                newBitmap = default;
                return false;
            }

            this.TryConvertPixelsFormat(newFmt, out newBitmap);
            return true;
        }

        /// <summary>
        /// Tries to in-place convert the pixels to a BGR? format.
        /// </summary>
        /// <param name="newBitmap">If the conversion is successful, a new span with the new format</param>
        /// <returns>true if the conversion was successful</returns>
        /// <remarks>
        /// The current bitmap is no longer valid because its internal
        /// pixel format will not match the actual data.
        /// </remarks>
        public bool TryConvertPixelsFormatBGRX(out SpanBitmap newBitmap)
        {
            if (!PixelFormat.TryGetFormatAsBGRX(this.PixelFormat, out var newFmt))
            {
                newBitmap = default;
                return false;
            }

            this.TryConvertPixelsFormat(newFmt, out newBitmap);
            return true;
        }

        /// <summary>
        /// Tries to in-place convert the pixels to a new format.
        /// </summary>
        /// <param name="newFormat">The new pixel format</param>
        /// <param name="newBitmap">The new bitmap, sharing the pixels</param>
        /// <returns></returns>
        /// <remarks>
        /// The current bitmap is no longer valid because its internal
        /// pixel format will not match the actual data.
        /// </remarks>
        public bool TryConvertPixelsFormat(PixelFormat newFormat, out SpanBitmap newBitmap)
        {
            if (this.PixelFormat.ByteCount < newFormat.ByteCount)
            {
                newBitmap = default;
                return false;
            }

            var converter = Pixel.GetByteCopyConverter(this.PixelFormat, newFormat);
            var newInfo = this.Info.WithPixelFormat(newFormat);

            // we need a temporary row because some pixel coverters don't handle self conversion
            Span<byte> tmpRow = stackalloc byte[this.Info.RowByteSize];

            for(int y=0; y < this.Height; ++y)
            {
                var row = UseScanlineBytes(y);
                row.CopyTo(tmpRow);
                row = row.Slice(0, newInfo.RowByteSize);
                converter.Invoke(tmpRow, row);
            }
            
            newBitmap = new SpanBitmap(this._Readable, newInfo);
            return true;
        }

        #endregion

        #region API - IO        

        public void Save(string filePath, params Codecs.IBitmapEncoder[] factory)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));

            var fmt = Codecs.BitmapCodecFactory.ParseFormat(filePath);
            var lzs = new Lazy<System.IO.Stream>(() => System.IO.File.Create(filePath));

            try { Codecs.BitmapCodecFactory.Write(lzs, fmt, factory, this); }
            finally { if (lzs.IsValueCreated) lzs.Value.Dispose(); }            
        }

        public void Write(System.IO.Stream stream, Codecs.CodecFormat format, params Codecs.IBitmapEncoder[] factory)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanWrite) throw new ArithmeticException(nameof(stream));

            var lzs = new Lazy<System.IO.Stream>(() => stream);

            Codecs.BitmapCodecFactory.Write(lzs, format, factory, this);
        }        

        public void Write(Lazy<System.IO.Stream> stream, Codecs.CodecFormat format, params Codecs.IBitmapEncoder[] factory)
        {
            Codecs.BitmapCodecFactory.Write(stream, format, factory, this);
        }        

        #endregion
    }
}
