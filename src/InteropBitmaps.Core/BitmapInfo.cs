using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    [System.Diagnostics.DebuggerDisplay("{Width}x{Height}-{PixelFormat}")]
    public readonly struct BitmapInfo : IEquatable<BitmapInfo>
    {
        #region lifecycle        

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
        public readonly int ScanlineSize;
        public readonly PixelFormat PixelFormat;

        public override int GetHashCode() { return Width.GetHashCode() ^ Height.GetHashCode() ^ PixelFormat.GetHashCode(); }

        public static bool AreEqual(in BitmapInfo a, in BitmapInfo b)
        {
            if (a.Width != b.Width) return false;
            if (a.Height != b.Height) return false;
            if (a.PixelSize != b.PixelSize) return false;            
            if (a.PixelFormat != b.PixelFormat) return false;
            if (a.ScanlineSize != b.ScanlineSize) return false;
            return true;
        }

        public bool Equals(BitmapInfo other) { return AreEqual(this, other); }

        public override bool Equals(object obj) { return obj is BitmapInfo other ? AreEqual(this, other) : false; }

        public static bool operator ==(in BitmapInfo a, in BitmapInfo b) { return AreEqual(a, b); }

        public static bool operator !=(in BitmapInfo a, in BitmapInfo b) { return !AreEqual(a, b); }

        #endregion

        #region properties

        // under some circunstances, the last stride of the last row must not be accounted for.
        public int BitmapByteSize => Height == 0 ? 0 : ScanlineSize * (Height - 1) + PixelSize * Width;

        public (int Width, int Height) Size => (Width, Height);

        public BitmapBounds Bounds => new BitmapBounds(0,0,Width, Height);

        #endregion

        #region data

        public (int Offset, BitmapInfo Info) Slice(in BitmapBounds rect)
        {
            Guard.IsTrue(nameof(rect), Bounds.Contains(rect));

            var offset = this.ScanlineSize * rect.X + this.PixelSize * rect.Y;

            var info = new BitmapInfo(rect.Width, rect.Height, this);

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

        public Span<Byte> UsePixels(Span<Byte> bitmap, int x, int y, int pixelCount)
        {
            return bitmap.Slice(y * ScanlineSize + x * PixelSize, PixelSize * pixelCount);
        }

        public ReadOnlySpan<Byte> GetPixel(ReadOnlySpan<Byte> bitmap, int x, int y)
        {
            return bitmap.Slice(y * ScanlineSize + x * PixelSize, PixelSize);
        }

        public ReadOnlySpan<Byte> GetPixels(ReadOnlySpan<Byte> bitmap, int x, int y, int pixelCount)
        {
            return bitmap.Slice(y * ScanlineSize + x * PixelSize, PixelSize * pixelCount);
        }

        #endregion
    }
}
