using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <summary>
    /// Defines a Bitmap in managed memory
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Bitmap[{Width},{Height}]")]
    public class MemoryBitmap<TPixel> : MemoryBitmap, IBitmap<TPixel> where TPixel : unmanaged
    {
        #region lifecycle

        public static implicit operator SpanBitmap<TPixel>(MemoryBitmap<TPixel> bmp) { return bmp.AsSpanBitmap(); }

        public MemoryBitmap(Memory<Byte> data, in BitmapInfo info)
            : base(data, info) { Guard.IsValidPixelFormat<TPixel>(info); }

        public unsafe MemoryBitmap(int width, int height, PixelFormat pixelFormat)
            : base(width, height, pixelFormat)
        {
            Guard.IsValidPixelFormat<TPixel>(this.Info);
        }

        public MemoryBitmap(int width, int height)
            : base(width, height, PixelFormat.GetUndefined<TPixel>()) { }

        public MemoryBitmap(Memory<Byte> data, int width, int height, int scanlineSize = 0)
            : base(data, width, height, PixelFormat.GetUndefined<TPixel>(), scanlineSize) { }

        public unsafe MemoryBitmap(Memory<Byte> data, int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
            : base(data, width, height, pixelFormat, scanlineSize)
        {
            Guard.IsValidPixelFormat<TPixel>(this.Info);
        }

        #endregion

        #region properties

        public new SpanBitmap<TPixel> Span => new SpanBitmap<TPixel>(this.Buffer.Span, this.Info);

        #endregion

        #region API

        private Span<TPixel> UsePixelsScanline(int y)
        {
            var rowBytes = UseBytesScanline(y);
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(rowBytes);
        }

        public TPixel GetPixel(int x, int y) { return UsePixelsScanline(y)[x]; }

        public void SetPixel(int x, int y, TPixel value) { UsePixelsScanline(y)[x] = value; }

        public new SpanBitmap<TPixel> AsSpanBitmap() { return new SpanBitmap<TPixel>(Buffer.Span, this.Info); }

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
    }
}
