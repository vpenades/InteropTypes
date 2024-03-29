﻿using System;
using System.Collections.Generic;
using System.Text;

using SIZE = System.Drawing.Size;
using POINT = System.Drawing.Point;
using System.Numerics;

namespace InteropTypes.Graphics.Bitmaps
{
    /// <summary>
    /// Represents a Bitmap wrapped around a <see cref="Memory{Byte}"/>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Info.ToDebuggerDisplayString(),nq}")]
    // [System.Diagnostics.DebuggerTypeProxy(typeof(Debug.SpanBitmapProxy<>))]
    public readonly partial struct MemoryBitmap<TPixel>
        
        : IEquatable<MemoryBitmap<TPixel>>
        where TPixel : unmanaged        
    {
        #region debug

        internal ReadOnlySpan<TPixel> _Row0 => AsSpanBitmap().GetScanlinePixels(0);
        internal ReadOnlySpan<TPixel> _Row1 => AsSpanBitmap().GetScanlinePixels(1);
        internal ReadOnlySpan<TPixel> _Row2 => AsSpanBitmap().GetScanlinePixels(2);
        internal ReadOnlySpan<TPixel> _Row3 => AsSpanBitmap().GetScanlinePixels(3);

        #endregion

        #region implicit

        public static implicit operator SpanBitmap(MemoryBitmap<TPixel> bmp) { return new SpanBitmap(bmp._Data.Span, bmp._Info); }

        public static implicit operator SpanBitmap<TPixel>(MemoryBitmap<TPixel> bmp) { return new SpanBitmap<TPixel>(bmp._Data.Span, bmp._Info); }

        public static implicit operator MemoryBitmap(MemoryBitmap<TPixel> bmp) { return new MemoryBitmap(bmp._Data, bmp._Info); }

        #endregion

        #region lifecycle

        public MemoryBitmap(in BitmapInfo info)
        {
            info.ArgumentIsCompatiblePixelFormat<TPixel>();

            _Info = info;
            _Data = info.BitmapByteSize == 0
                ? Array.Empty<Byte>()
                : new Byte[info.BitmapByteSize];
        }

        public MemoryBitmap(Memory<Byte> data, in BitmapInfo info)
        {
            info.ArgumentIsCompatiblePixelFormat<TPixel>();

            _Info = info;
            _Data = data.Slice(0, _Info.BitmapByteSize);
        }

        public MemoryBitmap(int width, int height)
            : this(width, height, PixelFormat.TryIdentifyFormat<TPixel>())
        { }

        public unsafe MemoryBitmap(int width, int height, PixelFormat pixelFormat, int stepByteSize = 0)            
        {
            _Info = BitmapInfo.Create<TPixel>(width, height, pixelFormat, stepByteSize);            
            _Data = new byte[_Info.BitmapByteSize];
        }        

        public MemoryBitmap(Memory<Byte> data, int width, int height, int stepByteSize = 0)
            : this(data, width, height, PixelFormat.TryIdentifyFormat<TPixel>(), stepByteSize)
        { }

        public MemoryBitmap(Memory<Byte> data, int width, int height, PixelFormat pixelFormat, int stepByteSize = 0)            
        {
            _Info = BitmapInfo.Create<TPixel>(width, height, pixelFormat, stepByteSize);
            _Data = data.Slice(0, _Info.BitmapByteSize);
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly BitmapInfo _Info;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly Memory<Byte> _Data;

        /// <inheritdoc/>
        public readonly override int GetHashCode() { return _Data.GetHashCode() ^ _Info.GetHashCode(); }

        /// <inheritdoc/>
        public readonly override bool Equals(object obj) { return obj is MemoryBitmap<TPixel> other && AreEqual(this,other); }

        /// <inheritdoc/>
        public readonly bool Equals(MemoryBitmap<TPixel> other) { return AreEqual(this, other); }

        /// <inheritdoc/>
        public static bool operator ==(in MemoryBitmap<TPixel> a, in MemoryBitmap<TPixel> b) { return AreEqual(a, b); }

        /// <inheritdoc/>
        public static bool operator !=(in MemoryBitmap<TPixel> a, in MemoryBitmap<TPixel> b) { return !AreEqual(a, b); }

        /// <summary>
        /// Indicates whether both instances are equal.
        /// </summary>
        /// <param name="a">The first instance.</param>
        /// <param name="b">The first instance.</param>
        /// <returns>true if both instances represent the same value.</returns>
        public static bool AreEqual(in MemoryBitmap<TPixel> a, in MemoryBitmap<TPixel> b)
        {
            if (a.Info != b.Info) return false;
            if (a._Data.Equals(b._Data)) return false;
            return true;
        }

        #endregion

        #region properties

        public readonly Memory<Byte> Memory => _Data;

        public readonly bool IsEmpty => _Info.IsEmpty || _Data.IsEmpty;

        #endregion

        #region properties - Info

        /// <inheritdoc/>
        public readonly BitmapInfo Info => _Info;

        /// <inheritdoc/>
        public readonly PixelFormat PixelFormat => _Info.PixelFormat;

        /// <inheritdoc/>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public readonly SIZE Size => _Info.Size;

        /// <inheritdoc/>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public readonly int Width => _Info.Width;

        /// <inheritdoc/>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public readonly int Height => _Info.Height;

        /// <summary>
        /// Gets the size of a single pixel, in bytes.
        /// </summary>
        public readonly int PixelByteSize => _Info.PixelByteSize;

        /// <summary>
        /// Gets the number of bytes required to jump from one row to the next, in bytes. This is also known as the ByteStride.
        /// </summary>
        public readonly int StepByteSize => _Info.StepByteSize;

        /// <summary>
        /// Gets the bounds of the bitmap, in pixels.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public readonly BitmapBounds Bounds => _Info.Bounds;

        #endregion        

        #region API - Buffers

        [System.Diagnostics.DebuggerStepThrough]
        public readonly Memory<byte> UseScanlineBytes(int y) { return _Info.UseScanlineBytes(_Data, y); }

        [System.Diagnostics.DebuggerStepThrough]
        public readonly Span<TPixel> UseScanlinePixels(int y) { return _Info.UseScanlinePixels<TPixel>(_Data.Span, y); }

        [System.Diagnostics.DebuggerStepThrough]
        public readonly ReadOnlyMemory<byte> GetScanlineBytes(int y) { return _Info.GetScanlineBytes(_Data, y); }

        [System.Diagnostics.DebuggerStepThrough]
        public readonly ReadOnlySpan<TPixel> GetScanlinePixels(int y) { return _Info.GetScanlinePixels<TPixel>(_Data.Span, y); }

        [System.Diagnostics.DebuggerStepThrough]
        public readonly Memory<TPixel> GetPixelMemory()
        {
            return new MemoryManagers.CastMemoryManager<Byte, TPixel>(_Data).Memory;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public readonly unsafe Memory<TOtherPixel> GetPixelMemory<TOtherPixel>()
            where TOtherPixel:unmanaged
        {
            if (sizeof(TPixel) != sizeof(TOtherPixel)) throw new ArgumentException("pixel size mismatch", typeof(TOtherPixel).Name);

            return new MemoryManagers.CastMemoryManager<Byte, TOtherPixel>(_Data).Memory;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public readonly bool TryGetBuffer(out ArraySegment<Byte> segment)
        {
            return System.Runtime.InteropServices.MemoryMarshal.TryGetArray(_Data, out segment);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public readonly Byte[] ToByteArray()
        {
            if (TryGetBuffer(out ArraySegment<Byte> array) && array.Offset == 0 && array.Array.Length == _Info.BitmapByteSize) return array.Array;

            return _Data.ToArray();
        }

        [System.Diagnostics.DebuggerStepThrough]
        public MemoryBitmap<TPixel> Slice(in BitmapBounds rect)
        {
            var (offset, info) = _Info.Slice(rect);

            if (info.BitmapByteSize == 0) return new MemoryBitmap<TPixel>(info);

            var memory = this._Data.Slice(offset, info.BitmapByteSize);
            return new MemoryBitmap<TPixel>(memory, info);
        }

        #endregion

        #region API - Pixel Ops

        public void SetPixels(System.Drawing.Color color) { SetPixels(Pixel.GetColor<TPixel>(color)); }

        public void SetPixels(TPixel value) { AsSpanBitmap().SetPixels(value); }        

        public void SetPixels<TSrcPixel>(int dstX, int dstY, SpanBitmap<TSrcPixel> src)
            where TSrcPixel:unmanaged
        {
            AsSpanBitmap().SetPixels(dstX, dstY, src);
        }

        public void SetPixels<TSrcPixel>(in Matrix3x2 location, SpanBitmap<TSrcPixel> src, bool useBilinear, in Pixel.RGB96F.MulAdd pixelOp)
            where TSrcPixel : unmanaged
        {
            this.AsSpanBitmap().SetPixels(location, src, useBilinear, pixelOp);
        }

        public void SetPixels<TSrcPixel>(in SpanBitmap<TSrcPixel> src, SpanBitmap.ITransfer transfer)
            where TSrcPixel : unmanaged
        {
            this.AsSpanBitmap().SetPixels(src, transfer);
        }

        public IEnumerable<(POINT Location, TPixel Pixel)> EnumeratePixels()
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    var p = new POINT(x, y);

                    yield return (p, GetPixel(p));
                }
            }
        }

        public MemoryBitmap<TPixel> ToMemoryBitmap(PixelFormat? fmtOverride = null)
        {
            return this.AsSpanBitmap().ToMemoryBitmap(fmtOverride);
        }

        #endregion

        #region API - IO

        public static MemoryBitmap<TPixel> Load(string filePath, params Codecs.IBitmapDecoder[] factory)
        {
            var raw = MemoryBitmap.Load(filePath, factory);
            return _ReadPostprocess(raw);
        }

        public static MemoryBitmap<TPixel> Load(Func<System.IO.Stream> usingStream, params Codecs.IBitmapDecoder[] factory)
        {
            var raw = MemoryBitmap.Load(usingStream, factory);
            return _ReadPostprocess(raw);
        }

        public static MemoryBitmap<TPixel> Read(System.IO.Stream s, params Codecs.IBitmapDecoder[] factory)
        {
            var raw = MemoryBitmap.Read(s, factory);
            return _ReadPostprocess(raw);
        }

        private static MemoryBitmap<TPixel> _ReadPostprocess(MemoryBitmap raw)
        {
            if (raw.Info.IsCompatiblePixel<TPixel>()) return raw.OfType<TPixel>();

            var fmt = raw.Info.WithPixelFormat<TPixel>();
            var dst = new MemoryBitmap<TPixel>(fmt);
            dst.AsTypeless().SetPixels(0, 0, raw);
            return dst;
        }
        

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public readonly void Save(Action<Action<System.IO.FileInfo>> saveCallback, params Codecs.IBitmapEncoder[] factory)
        {
            var image = this;
            saveCallback(finfo => image.Save(finfo.FullName, factory));
        }

        public readonly void Save(string filePath, params Codecs.IBitmapEncoder[] factory)
        {
            this.AsSpanBitmap().Save(filePath, factory);
        }        

        #endregion
    }
}
