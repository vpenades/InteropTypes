using System;
using System.Collections.Generic;
using System.Text;

using SIZE = System.Drawing.Size;
using POINT = System.Drawing.Point;

namespace InteropBitmaps
{
    /// <summary>
    /// Represents a Bitmap wrapped around a <see cref="Memory{Byte}"/>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Info.ToDebuggerDisplayString(),nq}")]
    // [System.Diagnostics.DebuggerTypeProxy(typeof(Debug.SpanBitmapProxy<>))]
    public readonly struct MemoryBitmap<TPixel> : IBitmap<TPixel> where TPixel : unmanaged        
    {
        #region debug

        internal ReadOnlySpan<TPixel> _Row0 => AsSpanBitmap().GetScanlinePixels(0);
        internal ReadOnlySpan<TPixel> _Row1 => AsSpanBitmap().GetScanlinePixels(1);
        internal ReadOnlySpan<TPixel> _Row2 => AsSpanBitmap().GetScanlinePixels(2);
        internal ReadOnlySpan<TPixel> _Row3 => AsSpanBitmap().GetScanlinePixels(3);

        #endregion

        #region lifecycle

        public MemoryBitmap(in BitmapInfo info)
        {
            Guard.IsValidPixelFormat<TPixel>(info);

            _Info = info;
            _Data = new Byte[info.BitmapByteSize];
        }

        public MemoryBitmap(Memory<Byte> data, in BitmapInfo info)
        {
            Guard.IsValidPixelFormat<TPixel>(info);

            _Info = info;
            _Data = data.Slice(0, _Info.BitmapByteSize);
        }

        public MemoryBitmap(int width, int height) : this(width, height, Pixel.Format.TryIdentifyPixel<TPixel>()) { }

        public unsafe MemoryBitmap(int width, int height, Pixel.Format pixelFormat, int stepByteSize = 0)            
        {
            _Info = new BitmapInfo(width, height, pixelFormat, stepByteSize);

            Guard.IsValidPixelFormat<TPixel>(_Info);

            var bytes = new byte[_Info.BitmapByteSize];
            _Data = bytes;
        }        

        public MemoryBitmap(Memory<Byte> data, int width, int height, int stepByteSize = 0)
            : this(data, width, height, Pixel.Format.GetUndefined<TPixel>(), stepByteSize) { }

        public MemoryBitmap(Memory<Byte> data, int width, int height, Pixel.Format pixelFormat, int stepByteSize = 0)            
        {
            _Info = new BitmapInfo(width, height, pixelFormat, stepByteSize);

            Guard.IsValidPixelFormat<TPixel>(_Info);

            _Data = data.Slice(0, _Info.BitmapByteSize);
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly BitmapInfo _Info;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly Memory<Byte> _Data;
        public override int GetHashCode() { return _Implementation.CalculateHashCode(_Data.Span, _Info); }

        #endregion

        #region properties

        public Memory<Byte> Memory => _Data;

        public bool IsEmpty => _Info.IsEmpty || _Data.IsEmpty;

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

        public Span<byte> UseScanlineBytes(int y) { return _Info.UseScanlineBytes(_Data.Span, y); }
        public Span<TPixel> UseScanlinePixels(int y) { return _Info.UseScanlinePixels<TPixel>(_Data.Span, y); }

        public ReadOnlySpan<byte> GetScanlineBytes(int y) { return _Info.GetScanlineBytes(_Data.Span, y); }
        public ReadOnlySpan<TPixel> GetScanlinePixels(int y) { return _Info.GetScanlinePixels<TPixel>(_Data.Span, y); }


        public Memory<TPixel> GetPixelMemory()
        {
            return new MemoryManagers.CastMemoryManager<Byte, TPixel>(_Data).Memory;
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

        #region API - Cast

        public static implicit operator SpanBitmap(MemoryBitmap<TPixel> bmp) { return new SpanBitmap(bmp._Data.Span, bmp._Info); }

        public static implicit operator SpanBitmap<TPixel>(MemoryBitmap<TPixel> bmp) { return new SpanBitmap<TPixel>(bmp._Data.Span, bmp._Info); }

        public static implicit operator MemoryBitmap(MemoryBitmap<TPixel> bmp) { return new MemoryBitmap(bmp._Data, bmp._Info); }

        public SpanBitmap<TPixel> AsSpanBitmap() { return this; }

        public MemoryBitmap AsTypeless() { return new MemoryBitmap(this._Data, this._Info); }

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
        
        public TPixel GetPixel(int x, int y) { return UseScanlinePixels(y)[x]; }

        public void SetPixel(int x, int y, TPixel value) { UseScanlinePixels(y)[x] = value; }

        public TPixel GetPixel(POINT point) { return UseScanlinePixels(point.Y)[point.X]; }

        public void SetPixel(POINT point, TPixel value) { UseScanlinePixels(point.Y)[point.X] = value; }

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

        public MemoryBitmap<TPixel> ToMemoryBitmap(Pixel.Format? fmtOverride = null)
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

            var fmt = raw.Info.WithPixelFormat<TPixel>();
            var dst = new MemoryBitmap<TPixel>(fmt);
            dst.AsTypeless().SetPixels(0, 0, raw);
            return dst;
        }

        public void Save(string filePath, params Codecs.IBitmapEncoder[] factory)
        {
            this.AsSpanBitmap().Save(filePath, factory);
        }        

        #endregion
    }
}
