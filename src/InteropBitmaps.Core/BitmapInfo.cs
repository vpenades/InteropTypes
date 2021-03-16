using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using SIZE = System.Drawing.Size;
using POINT = System.Drawing.Point;
using RECTI = System.Drawing.Rectangle;

namespace InteropBitmaps
{
    using WSPAN = Span<Byte>;
    using RSPAN = ReadOnlySpan<Byte>;

    /// <summary>
    /// Represents the width, height and pixel format of a bitmap.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{_DebuggerDisplay(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public readonly struct BitmapInfo : IEquatable<BitmapInfo>
    {
        // Todo: Maybe a better name for this struct is BitmapDesc or BitmapLayout

        #region debug

        internal string _DebuggerDisplay() { return $"{PixelFormat._GetDebuggerDisplay()}×{Width}×{Height}"; }

        #endregion

        #region constructor helpers

        public static implicit operator BitmapInfo((SIZE s, Pixel.Format fmt) binfo)
        {
            return new BitmapInfo(binfo.s, binfo.fmt);
        }

        public static implicit operator BitmapInfo((int w, int h, Pixel.Format fmt) binfo)
        {
            return new BitmapInfo(binfo.w, binfo.h, binfo.fmt);
        }

        public BitmapInfo WithPixelFormat(Pixel.Format format) { return new BitmapInfo(this.Width, this.Height, format, this.StepByteSize); }

        public BitmapInfo WithPixelFormat<TPixel>() where TPixel:unmanaged
        {
            var fmt = Pixel.Format.TryIdentifyPixel<TPixel>();
            return this.WithPixelFormat(fmt);
        }

        public BitmapInfo WithSize(int width, int height, int stepByteSize = 0)
        {
            return new BitmapInfo(width, height, this.PixelFormat, stepByteSize);
        }        

        #endregion

        #region constructors

        public BitmapInfo(SIZE size, Pixel.Format pixelFormat, int stepByteSize = 0)
            : this(size.Width,size.Height,pixelFormat,stepByteSize) { }
        
        public BitmapInfo(int width, int height, Pixel.Format pixelFormat, int stepByteSize = 0)
        {
            var pixelByteSize = pixelFormat.ByteCount;

            Guard.BitmapRect(width, height, pixelByteSize, stepByteSize);

            Width = width;
            Height = height;
            StepByteSize = stepByteSize > 0 ? stepByteSize : width * pixelByteSize;

            PixelFormat = pixelFormat;
            PixelByteSize = pixelByteSize;            
        }

        #endregion

        #region data

        /// <summary>
        /// Width of the bitmap, in pixels.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public readonly int Width;

        /// <summary>
        /// Height of the bitmap, in pixels.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public readonly int Height;        

        /// <summary>
        /// Number of bytes to advance from the beginning of one row to the next.
        /// </summary>
        public readonly int StepByteSize;

        /// <summary>
        /// format of the pixel.
        /// </summary>
        public readonly Pixel.Format PixelFormat;

        /// <summary>
        /// Byte size of a single pixel.
        /// </summary>
        public readonly int PixelByteSize;

        public override int GetHashCode()
        {
            // PixelFormat should be irrelevant

            return Width.GetHashCode() ^ Height.GetHashCode() ^ PixelFormat.GetHashCode();
        }

        public static bool AreEqual(in BitmapInfo a, in BitmapInfo b)
        {
            if (a.Width != b.Width) return false;
            if (a.Height != b.Height) return false;
            if (a.PixelByteSize != b.PixelByteSize) return false;            
            if (a.PixelFormat != b.PixelFormat) return false;
            if (a.StepByteSize != b.StepByteSize) return false;
            return true;
        }

        public bool Equals(BitmapInfo other) { return AreEqual(this, other); }

        public override bool Equals(object obj) { return obj is BitmapInfo other ? AreEqual(this, other) : false; }

        public static bool operator ==(in BitmapInfo a, in BitmapInfo b) { return AreEqual(a, b); }

        public static bool operator !=(in BitmapInfo a, in BitmapInfo b) { return !AreEqual(a, b); }

        #endregion

        #region properties

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        public bool IsEmpty => (Width * Height * PixelByteSize) == 0;

        /// <summary>
        /// Gets the number of bytes required to store a bitmap.
        /// </summary>
        /// <remarks>
        /// When using a byte stride, the last row does not need a tailing stride.
        /// </remarks>        
        public int BitmapByteSize => Height == 0 ? 0 : StepByteSize * (Height - 1) + PixelByteSize * Width;

        /// <summary>
        /// Gets the <see cref="SIZE"/> of the bitmap, in pixels.
        /// </summary>
        public SIZE Size => new SIZE(Width, Height);

        /// <summary>
        /// Gets the <see cref="RECTI"/> of the bitmap, in pixels.
        /// </summary>
        public RECTI Rect => new RECTI(0, 0, Width, Height);

        /// <summary>
        /// Gets the <see cref="BitmapBounds"/> of this bitmap, in pixels.
        /// </summary>
        public BitmapBounds Bounds => new BitmapBounds(0, 0, Width, Height);

        /// <summary>
        /// Gets a value indicating whether the buffer can be accessed continuously.
        /// </summary>
        public bool IsContinuous => Width * PixelByteSize == StepByteSize;

        public (SIZE Size, Pixel.Format Format) Layout => (Size, PixelFormat);

        #endregion

        #region API

        public bool Contains(int x, int y)
        {
            if (x < 0) return false;
            if (y < 0) return false;
            if (x >= Width) return false;
            if (y >= Height) return false;
            return true;
        }

        public (int Offset, BitmapInfo Info) Slice(in BitmapBounds rect)
        {
            Guard.IsTrue(nameof(rect), Bounds.Contains(rect));

            var offset = this.StepByteSize * rect.Y + this.PixelByteSize * rect.X;

            // todo: if (Rect.X &1 ^ Rect.Y &1) == 1, pixel format must call SwitchScanlineFormatOrder(

            var info = this.WithSize(rect.Width, rect.Height, this.StepByteSize);

            System.Diagnostics.Debug.Assert(this.StepByteSize == info.StepByteSize);

            return (offset, info);
        }

        public WSPAN UseScanlineBytes(WSPAN bitmap, int y)
        {
            return bitmap.Slice(y * StepByteSize, Width * PixelByteSize);
        }

        public Span<TPixel> UseScanlinePixels<TPixel>(WSPAN bitmap, int y)
            where TPixel : unmanaged
        {
            var scanline = bitmap.Slice(y * StepByteSize, Width * PixelByteSize);
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(scanline);
        }

        public RSPAN GetScanlineBytes(RSPAN bitmap, int y)
        {
            return bitmap.Slice(y * StepByteSize, Width * PixelByteSize);
        }

        public ReadOnlySpan<TPixel> GetScanlinePixels<TPixel>(RSPAN bitmap, int y)
            where TPixel : unmanaged
        {
            var scanline = bitmap.Slice(y * StepByteSize, Width * PixelByteSize);
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(scanline);
        }

        public WSPAN UsePixel(WSPAN bitmap, int x, int y)
        {
            return bitmap.Slice(y * StepByteSize + x * PixelByteSize, PixelByteSize);
        }

        public WSPAN UsePixels(WSPAN bitmap, int x, int y, int pixelCount)
        {
            if (pixelCount - x > Width) throw new ArgumentOutOfRangeException(nameof(pixelCount));

            return bitmap.Slice(y * StepByteSize + x * PixelByteSize, PixelByteSize * pixelCount);
        }

        public RSPAN GetPixel(RSPAN bitmap, int x, int y)
        {
            return bitmap.Slice(y * StepByteSize + x * PixelByteSize, PixelByteSize);
        }

        public RSPAN GetPixels(RSPAN bitmap, int x, int y, int pixelCount)
        {
            if (pixelCount - x > Width) throw new ArgumentOutOfRangeException(nameof(pixelCount));

            return bitmap.Slice(y * StepByteSize + x * PixelByteSize, PixelByteSize * pixelCount);
        }

        #endregion

        #region factory

        public unsafe bool IsCompatiblePixel<TPixel>() where TPixel:unmanaged
        {
            if (sizeof(TPixel) != this.PixelByteSize)
            {
                return false;
            }

            var tformat = Pixel.Format.TryIdentifyPixel<TPixel>();

            return this.PixelFormat == tformat;
        }

        public bool CreateBitmap(ref SpanBitmap bmp)
        {
            if (bmp.Info != this) { bmp = new SpanBitmap(new Byte[this.BitmapByteSize], this); return true; }
            return false;
        }

        public bool CreateBitmap<T>(ref SpanBitmap<T> bmp)
            where T : unmanaged
        {
            if (bmp.Info != this) { bmp = new SpanBitmap<T>(new Byte[this.BitmapByteSize], this); return true; }
            return false;
        }

        public bool CreateBitmap(ref MemoryBitmap bmp)
        {
            if (bmp.Info != this) { bmp = new MemoryBitmap(this); return true; }
            return false;
        }

        public bool CreateBitmap<T>(ref MemoryBitmap<T> bmp)
            where T:unmanaged
        {
            if (bmp.Info != this) { bmp = new MemoryBitmap<T>(this); return true; }
            return false;
        }

        #endregion
    }
}
