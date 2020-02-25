using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <summary>
    /// Defines a Bitmap in managed memory
    /// </summary>
    /// <remarks>
    /// Known Derived classes: <see cref="MemoryBitmap{TPixel}"/>
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("Bitmap[{Width},{Height}]")]
    public class MemoryBitmap
    {
        #region lifecycle

        public static implicit operator SpanBitmap(MemoryBitmap bmp) { return bmp.AsSpanBitmap(); }

        /// <summary>
        /// Creates a new bitmap
        /// </summary>
        /// <param name="width">Width in Pixels.</param>
        /// <param name="height">Height in Pixels.</param>
        /// <param name="pixelSize">Number of bytes of a pixel.</param>
        /// <param name="scanlineSize">Number of bytes to advance to the next scanline.</param>
        public MemoryBitmap(int width, int height, int pixelSize, int scanlineSize = 0)
        {
            Guard.BitmapRect(width, height, pixelSize, scanlineSize);

            _Width = width;
            _Height = height;
            _PixelSize = pixelSize;
            _PixelFormat = PixelFormat.GetUndefinedOfSize(pixelSize);
            _ScanlineSize = scanlineSize > 0 ? scanlineSize : width * pixelSize;

            _Data = new byte[_ScanlineSize * height];
        }

        public MemoryBitmap(int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            var pixelSize = pixelFormat.ByteCount;

            Guard.BitmapRect(width, height, pixelSize, scanlineSize);

            _Width = width;
            _Height = height;
            _PixelSize = pixelSize;
            _PixelFormat = pixelFormat;
            _ScanlineSize = scanlineSize > 0 ? scanlineSize : width * pixelSize;

            _Data = new byte[_ScanlineSize * height];
        }

        public MemoryBitmap(Memory<Byte> data, int width, int height, int pixelSize, int scanlineSize = 0)
        {
            Guard.BitmapRect(width, height, pixelSize, scanlineSize);

            _Width = width;
            _Height = height;
            _PixelSize = pixelSize;
            _PixelFormat = PixelFormat.GetUndefinedOfSize(pixelSize);
            _ScanlineSize = scanlineSize > 0 ? scanlineSize : width * pixelSize;

            _Data = data.Slice(0, _ScanlineSize * height);
        }

        public MemoryBitmap(Memory<Byte> data, int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            var pixelSize = pixelFormat.ByteCount;

            Guard.BitmapRect(width, height, pixelSize, scanlineSize);

            _Width = width;
            _Height = height;
            PixelFormat = pixelFormat;
            _PixelSize = pixelSize;
            _PixelFormat = pixelFormat;
            _ScanlineSize = scanlineSize > 0 ? scanlineSize : width * pixelSize;

            _Data = data.Slice(0, _ScanlineSize * height);
        }

        #endregion

        #region data

        private readonly Memory<Byte> _Data;        
        private readonly int _Width;
        private readonly int _Height;
        private readonly int _PixelSize;
        private readonly uint _PixelFormat;
        private readonly int _ScanlineSize;

        #endregion

        #region properties

        public int Width => _Width;
        public int Height => _Height;
        public int PixelSize => _PixelSize;
        public int ScanlineSize => _ScanlineSize;

        #endregion

        #region API        

        public Memory<Byte> Buffer => _Data;

        public PixelFormat PixelFormat { get; }

        public Span<byte> UseBytesScanline(int y)
        {
            var byteStride = _PixelSize * _Width;

            return _Data.Span.Slice(y * byteStride, byteStride);
        }        

        public unsafe MemoryBitmap<TPixel> AsMemoryBitmap<TPixel>()
            where TPixel:unmanaged
        {
            if (sizeof(TPixel) != _PixelSize) throw new ArgumentException(nameof(TPixel));
            return new MemoryBitmap<TPixel>(_Data, _Width, _Height, _ScanlineSize);
        }

        public SpanBitmap AsSpanBitmap() { return new SpanBitmap(_Data.Span, _Width, _Height, _PixelSize, _ScanlineSize); }

        public unsafe SpanBitmap<TPixel> AsSpanBitmapOfType<TPixel>()
            where TPixel : unmanaged
        {
            if (sizeof(TPixel) != _PixelSize) throw new ArgumentException(nameof(TPixel));
            return new SpanBitmap<TPixel>(_Data.Span, _Width, _Height, _ScanlineSize);
        }
        
        #endregion
    }

    /// <summary>
    /// Defines a Bitmap in managed memory
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Bitmap[{Width},{Height}]")]
    public class MemoryBitmap<TPixel> : MemoryBitmap, IBitmap<TPixel> where TPixel : unmanaged
    {
        #region lifecycle

        public static implicit operator SpanBitmap<TPixel>(MemoryBitmap<TPixel> bmp)
        {
            return bmp.AsSpanBitmap();
        }

        public unsafe MemoryBitmap(int width, int height, PixelFormat pixelFormat)
            : base(width, height, sizeof(TPixel))
        {
            Guard.AreEqual(nameof(pixelFormat), pixelFormat.ByteCount, sizeof(TPixel));
        }

        public unsafe MemoryBitmap(int width, int height)
            : base(width, height, sizeof(TPixel)) { }

        public unsafe MemoryBitmap(Memory<Byte> data, int width, int height, int scanlineSize = 0)
            : base(data, width, height, sizeof(TPixel), scanlineSize) { }

        #endregion

        #region API

        private Span<TPixel> UsePixelsScanline(int y)
        {
            var rowBytes = UseBytesScanline(y);
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(rowBytes);
        }

        public TPixel GetPixel(int x, int y) { return UsePixelsScanline(y)[x]; }

        public void SetPixel(int x, int y, TPixel value) { UsePixelsScanline(y)[x] = value; }

        public new SpanBitmap<TPixel> AsSpanBitmap() { return new SpanBitmap<TPixel>(Buffer.Span, Width, Height, ScanlineSize); }

        public IEnumerable<(int X, int Y, TPixel Pixel)> EnumeratePixels()
        {
            for(int y=0; y < Height; ++y)
            {
                for(int x=0; x < Width; ++x)
                {
                    yield return (x, y, GetPixel(x, y));
                }
            }
        }

        #endregion
    }
}
