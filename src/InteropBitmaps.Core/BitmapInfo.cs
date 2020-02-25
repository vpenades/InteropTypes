using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    [System.Diagnostics.DebuggerDisplay("[{Width},{Height}]")]
    public readonly struct BitmapInfo
    {
        #region lifecycle

        public BitmapInfo(int width, int height, int pixelSize, int scanlineSize = 0)
        {
            Guard.BitmapRect(width, height, pixelSize, scanlineSize);

            Width = width;
            Height = height;
            PixelSize = pixelSize;
            PixelFormat = InteropBitmaps.PixelFormat.GetUndefinedOfSize(pixelSize);
            ScanlineSize = scanlineSize > 0 ? scanlineSize : width * pixelSize;
        }

        public BitmapInfo(int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            var pixelSize = pixelFormat.ByteCount;

            Guard.BitmapRect(width, height, pixelSize, scanlineSize);

            Width = width;
            Height = height;
            PixelSize = pixelSize;
            PixelFormat = pixelFormat;
            ScanlineSize = scanlineSize > 0 ? scanlineSize : width * pixelSize;
        }

        private BitmapInfo(int width, int height, in BitmapInfo other)
        {
            Width = width;
            Height = height;
            PixelSize = other.PixelSize;
            PixelFormat = other.PixelFormat;
            ScanlineSize = other.ScanlineSize;
        }

        #endregion

        #region data

        public readonly int Width;
        public readonly int Height;
        public readonly int PixelSize;
        public readonly uint PixelFormat;
        public readonly int ScanlineSize;

        #endregion

        #region properties

        public int BitmapByteSize => ScanlineSize * Height;

        public (int Width, int Height) Size => (Width, Height);

        #endregion

        #region data

        public (int Offset, BitmapInfo Info) Slice(int x, int y, int width, int height)
        {
            var offset = this.ScanlineSize * y + this.PixelSize * x;

            var info = new BitmapInfo(width, height, this);

            return (offset, info);
        }

        public Span<Byte> UseScanline(Span<Byte> bitmap, int y)
        {
            return bitmap.Slice(y * ScanlineSize, Width * PixelSize);
        }

        public ReadOnlySpan<Byte> GetScanline(ReadOnlySpan<Byte> bitmap, int y)
        {
            return bitmap.Slice(y * ScanlineSize, Width * PixelSize);
        }

        public Span<Byte> UsePixel(Span<Byte> bitmap, int x, int y)
        {
            return bitmap.Slice(y * ScanlineSize + x * PixelSize, PixelSize);
        }

        public Span<Byte> UsePixels(Span<Byte> bitmap, int x, int y, int count)
        {
            return bitmap.Slice(y * ScanlineSize + x * PixelSize, PixelSize * count);
        }

        public ReadOnlySpan<Byte> GetPixel(ReadOnlySpan<Byte> bitmap, int x, int y)
        {
            return bitmap.Slice(y * ScanlineSize + x * PixelSize, PixelSize);
        }

        public ReadOnlySpan<Byte> GetPixels(ReadOnlySpan<Byte> bitmap, int x, int y, int count)
        {
            return bitmap.Slice(y * ScanlineSize + x * PixelSize, PixelSize * count);
        }

        #endregion
    }
}
