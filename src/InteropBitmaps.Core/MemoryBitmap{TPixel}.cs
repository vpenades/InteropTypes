using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <summary>
    /// Represents a Bitmap wrapped around a <see cref="Memory{Byte}"/>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Info._DebuggerDisplay(),nq}")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(Debug.SpanBitmapProxy<>))]
    public readonly struct MemoryBitmap<TPixel> : IBitmap<TPixel> where TPixel : unmanaged
    {
        #region debug

        internal ReadOnlySpan<TPixel> _Row0 => AsSpanBitmap().GetPixelsScanline(0);
        internal ReadOnlySpan<TPixel> _Row1 => AsSpanBitmap().GetPixelsScanline(1);
        internal ReadOnlySpan<TPixel> _Row2 => AsSpanBitmap().GetPixelsScanline(2);
        internal ReadOnlySpan<TPixel> _Row3 => AsSpanBitmap().GetPixelsScanline(3);

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

        public MemoryBitmap(int width, int height) : this(width, height, Pixel.Format.GetUndefined<TPixel>()) { }

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

        private readonly BitmapInfo _Info;
        private readonly Memory<Byte> _Data;

        public override int GetHashCode() { return _Implementation.CalculateHashCode(_Data.Span, _Info); }

        #endregion

        #region properties
        public BitmapInfo Info => _Info;
        public Memory<Byte> Memory => _Data;

        public bool IsEmpty => _Info.IsEmpty;
        public int Width => _Info.Width;
        public int Height => _Info.Height;
        public int PixelSize => _Info.PixelByteSize;
        public int StepByteSize => _Info.StepByteSize;
        public Pixel.Format PixelFormat => _Info.PixelFormat;

        #endregion

        #region API - Buffers

        public Memory<TPixel> GetPixelMemory() { return new MemoryManagers.CastMemoryManager<Byte, TPixel>(_Data).Memory; }

        public Span<byte> UseBytesScanline(int y) { return _Info.UseScanline(_Data.Span, y); }

        private Span<TPixel> UsePixelsScanline(int y)
        {
            var rowBytes = UseBytesScanline(y);
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(rowBytes);
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
        
        public TPixel GetPixel(int x, int y) { return UsePixelsScanline(y)[x]; }

        public void SetPixel(int x, int y, TPixel value) { UsePixelsScanline(y)[x] = value; }        

        public IEnumerable<(int X, int Y, TPixel Pixel)> EnumeratePixels()
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    yield return (x, y, GetPixel(x, y));
                }
            }
        }        

        #endregion

        #region API - IO

        public static MemoryBitmap<TPixel> Read(System.IO.Stream s, params Codecs.IBitmapDecoding[] factory)
        {
            return MemoryBitmap.Read(s, factory).OfType<TPixel>();
        }

        public static MemoryBitmap<TPixel> Load(string filePath, params Codecs.IBitmapDecoding[] factory)
        {
            return MemoryBitmap.Load(filePath, factory).OfType<TPixel>();
        }

        public void Save(string filePath, params Codecs.IBitmapEncoding[] factory)
        {
            this.AsSpanBitmap().Save(filePath, factory);
        }

        #endregion
    }
}
