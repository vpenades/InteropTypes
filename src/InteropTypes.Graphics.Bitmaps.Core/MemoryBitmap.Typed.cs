using System;
using System.Collections.Generic;
using System.Text;

using SIZE = System.Drawing.Size;
using POINT = System.Drawing.Point;

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
            _Data = new Byte[info.BitmapByteSize];
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
        public override int GetHashCode() { return _Data.GetHashCode() ^ _Info.GetHashCode(); }

        /// <inheritdoc/>
        public override bool Equals(object obj) { return obj is MemoryBitmap<TPixel> other && AreEqual(this,other); }

        /// <inheritdoc/>
        public bool Equals(MemoryBitmap<TPixel> other) { return AreEqual(this, other); }

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

        public Memory<Byte> Memory => _Data;

        public bool IsEmpty => _Info.IsEmpty || _Data.IsEmpty;

        #endregion

        #region properties - Info

        /// <inheritdoc/>
        public BitmapInfo Info => _Info;

        /// <inheritdoc/>
        public PixelFormat PixelFormat => _Info.PixelFormat;

        /// <inheritdoc/>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public SIZE Size => _Info.Size;

        /// <inheritdoc/>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Width => _Info.Width;

        /// <inheritdoc/>
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

        public Span<byte> UseScanlineBytes(int y) { return _Info.UseScanlineBytes(_Data.Span, y); }
        public Span<TPixel> UseScanlinePixels(int y) { return _Info.UseScanlinePixels<TPixel>(_Data.Span, y); }

        public ReadOnlySpan<byte> GetScanlineBytes(int y) { return _Info.GetScanlineBytes(_Data.Span, y); }
        public ReadOnlySpan<TPixel> GetScanlinePixels(int y) { return _Info.GetScanlinePixels<TPixel>(_Data.Span, y); }


        public Memory<TPixel> GetPixelMemory()
        {
            return new MemoryManagers.CastMemoryManager<Byte, TPixel>(_Data).Memory;
        }

        public unsafe Memory<TOtherPixel> GetPixelMemory<TOtherPixel>()
            where TOtherPixel:unmanaged
        {
            if (sizeof(TPixel) != sizeof(TOtherPixel)) throw new ArgumentException("pixel size mismatch", typeof(TOtherPixel).Name);

            return new MemoryManagers.CastMemoryManager<Byte, TOtherPixel>(_Data).Memory;
        }

        public bool TryGetBuffer(out ArraySegment<Byte> segment)
        {
            return System.Runtime.InteropServices.MemoryMarshal.TryGetArray(_Data, out segment);
        }

        public Byte[] ToByteArray()
        {
            if (TryGetBuffer(out ArraySegment<Byte> array) && array.Offset == 0 && array.Array.Length == _Info.BitmapByteSize) return array.Array;

            return _Data.ToArray();
        }

        #endregion

        #region API - Pixel Ops
        
        public void SetPixels(TPixel value) { AsSpanBitmap().SetPixels(value); }
        
        public void SetPixels(int dstX, int dstY, SpanBitmap<TPixel> src) { AsSpanBitmap().SetPixels(dstX, dstY, src); }

        public void ApplyPixels<TSrcPixel>(int dstX, int dstY, SpanBitmap<TSrcPixel> src, Func<TPixel, TSrcPixel, TPixel> pixelFunc)
            where TSrcPixel:unmanaged
        {
            AsSpanBitmap().ApplyPixels(dstX, dstY, src, pixelFunc);
        }

        public MemoryBitmap<TPixel> Slice(in BitmapBounds rect)
        {
            var (offset, info) = _Info.Slice(rect);
            var memory = this._Data.Slice(offset, info.BitmapByteSize);
            return new MemoryBitmap<TPixel>(memory, info);
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

        public static MemoryBitmap<TPixel> Read(System.IO.Stream s, params Codecs.IBitmapDecoder[] factory)
        {
            var raw = MemoryBitmap.Read(s, factory);
            if (raw.Info.IsCompatiblePixel<TPixel>()) return raw.OfType<TPixel>();

            var fmt = raw.Info.WithPixelFormat<TPixel>();
            var dst = new MemoryBitmap<TPixel>(fmt);
            dst.AsTypeless().SetPixels(0, 0, raw);
            return dst;
        }

        public static MemoryBitmap<TPixel> Load(string filePath, params Codecs.IBitmapDecoder[] factory)
        {
            var raw = MemoryBitmap.Load(filePath, factory);
            if (raw.Info.IsCompatiblePixel<TPixel>()) return raw.OfType<TPixel>();

            var nfo = raw.Info.WithPixelFormat<TPixel>().WithStride(0);
            var dst = new MemoryBitmap<TPixel>(nfo);
            dst.AsTypeless().SetPixels(0, 0, raw);
            return dst;
        }

        public void Save(Action<Action<System.IO.FileInfo>> saveCallback, params Codecs.IBitmapEncoder[] factory)
        {
            var image = this;
            saveCallback(finfo => image.Save(finfo.FullName, factory));
        }

        public void Save(string filePath, params Codecs.IBitmapEncoder[] factory)
        {
            this.AsSpanBitmap().Save(filePath, factory);
        }        

        #endregion
    }
}
