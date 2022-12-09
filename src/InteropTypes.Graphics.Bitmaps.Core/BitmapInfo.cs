using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using SIZE = System.Drawing.Size;
using POINT = System.Drawing.Point;
using RECTI = System.Drawing.Rectangle;

using MEMMARSHAL = System.Runtime.InteropServices.MemoryMarshal;

namespace InteropTypes.Graphics.Bitmaps
{
    using Diagnostics;

    using WSPAN = Span<Byte>;
    using RSPAN = ReadOnlySpan<Byte>;

    /// <summary>
    /// Represents the width, height and pixel format of a bitmap.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{ToDebuggerDisplayString(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public readonly partial struct BitmapInfo
        : IEquatable<BitmapInfo>
    {
        // Todo: Maybe a better name for this struct is BitmapDesc or BitmapLayout

        #region debug
        public string ToDebuggerDisplayString() { return $"{PixelFormat}×{Width}×{Height}"; }

        #endregion

        #region constructor helpers

        public static implicit operator BitmapInfo((SIZE s, PixelFormat fmt) binfo)
        {
            return new BitmapInfo(binfo.s, binfo.fmt);
        }

        public static implicit operator BitmapInfo((int w, int h, PixelFormat fmt) binfo)
        {
            return new BitmapInfo(binfo.w, binfo.h, binfo.fmt);
        }

        public BitmapInfo WithPixelFormat(PixelFormat format, bool tryPreserveByteSize = false)
        {
            if (this.PixelFormat.ByteCount == format.ByteCount && tryPreserveByteSize)
            {
                new BitmapInfo(this.Width, this.Height, format, this.StepByteSize);
            }

            return new BitmapInfo(this.Width, this.Height, format);
        }

        public BitmapInfo WithPixelFormat<TPixel>() where TPixel:unmanaged
        {
            var fmt = PixelFormat.TryIdentifyFormat<TPixel>();
            return this.WithPixelFormat(fmt);
        }

        public BitmapInfo WithPixelFormat<TPixel>(PixelFormat format) where TPixel : unmanaged
        {
            var fmt = PixelFormat.TryIdentifyFormat<TPixel>();
            if (format == default) return this.WithPixelFormat(fmt);

            if (fmt.ByteCount != format.ByteCount) throw new Diagnostics.PixelFormatNotSupportedException(format, nameof(format));
            
            return this.WithPixelFormat(fmt);
        }

        public BitmapInfo WithSize(int width, int height, int stepByteSize = 0)
        {
            return new BitmapInfo(width, height, this.PixelFormat, stepByteSize);
        }

        public BitmapInfo WithStride(int stepByteSize = 0)
        {
            return new BitmapInfo(this.Width, this.Height, this.PixelFormat, stepByteSize);
        }

        #endregion

        #region constructors

        public static BitmapInfo Create<TPixel>(int width, int height, int stepByteSize = 0)
            where TPixel : unmanaged
        {
            var fmt = PixelFormat.TryIdentifyFormat<TPixel>();
            return Create<TPixel>(width, height, fmt, stepByteSize);
        }

        /// <summary>
        /// Creates a new BitmapInfo instance.
        /// </summary>
        /// <typeparam name="TPixel">The pixel type. It must match <paramref name="pixelFormat"/>.</typeparam>
        /// <param name="width">The bitmap width, in pixels. Must be higher than zero</param>
        /// <param name="height">The bitmap height, in pixels. Must be highter than zero</param>
        /// <param name="pixelFormat">The pixel format. It must match <typeparamref name="TPixel"/>.</param>
        /// <param name="stepByteSize">The optional row width, in pixels.</param>
        /// <returns>A new BitmapInfo instance.</returns>
        public static BitmapInfo Create<TPixel>(int width, int height, PixelFormat pixelFormat, int stepByteSize = 0)
            where TPixel: unmanaged
        {
            var bmp = new BitmapInfo(width,height,pixelFormat,stepByteSize);
            bmp.ArgumentIsCompatiblePixelFormat<TPixel>();
            return bmp;
        }        

        public BitmapInfo(SIZE size, PixelFormat pixelFormat, int stepByteSize = 0)
            : this(size.Width,size.Height,pixelFormat,stepByteSize) { }

        public BitmapInfo(int width, int height, PixelFormat pixelFormat)
        {
            var pixelByteSize = pixelFormat.ByteCount;
            if (pixelByteSize <= 0) { this = default; return; }

            Guard.BitmapRect(width, height, pixelByteSize);

            Width = width;
            Height = height;
            StepByteSize = width * pixelByteSize;

            PixelFormat = pixelFormat;
            PixelByteSize = pixelByteSize;
        }

        public BitmapInfo(int width, int height, PixelFormat pixelFormat, int stepByteSize)
        {
            var pixelByteSize = pixelFormat.ByteCount;
            if (pixelByteSize <= 0) { this = default; return; }

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
        public int Width { get; }

        /// <summary>
        /// Height of the bitmap, in pixels.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Height { get; }

        /// <summary>
        /// Number of bytes to advance from the beginning of one row to the next.
        /// </summary>        
        public int StepByteSize { get; }

        /// <summary>
        /// format of the pixel.
        /// </summary>
        public PixelFormat PixelFormat { get; }

        /// <summary>
        /// Byte size of a single pixel.
        /// </summary>
        public int PixelByteSize { get; }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            // PixelFormat should be irrelevant

            return Width.GetHashCode() ^ Height.GetHashCode() ^ PixelFormat.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) { return obj is BitmapInfo other && AreEqual(this, other); }

        /// <inheritdoc/>
        public bool Equals(BitmapInfo other) { return AreEqual(this, other); }

        /// <inheritdoc/>
        public static bool operator ==(in BitmapInfo a, in BitmapInfo b) { return AreEqual(a, b); }

        /// <inheritdoc/>
        public static bool operator !=(in BitmapInfo a, in BitmapInfo b) { return !AreEqual(a, b); }


        public static bool AreEqual(in BitmapInfo a, in BitmapInfo b, bool compareStepSize = true)
        {
            if (a.Width != b.Width) return false;
            if (a.Height != b.Height) return false;
            if (a.PixelByteSize != b.PixelByteSize) return false;
            if (a.PixelFormat != b.PixelFormat) return false;
            if (compareStepSize && a.StepByteSize != b.StepByteSize) return false;
            return true;
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        public bool IsEmpty => PixelByteSize == 0;

        /// <summary>
        /// Gets the number of bytes used by a visible pixel row.
        /// </summary>
        public int RowByteSize => Width * PixelByteSize;

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

        public (SIZE Size, PixelFormat Format) Layout => (Size, PixelFormat);

        #endregion

        #region API

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

            if (rect.Width <=0 || rect.Height <=0) return (0, this.WithSize(0, 0));

            var offset = this.StepByteSize * rect.Y + this.PixelByteSize * rect.X;

            // todo: if (Rect.X &1 ^ Rect.Y &1) == 1, pixel format must call SwitchScanlineFormatOrder(

            var info = this.WithSize(rect.Width, rect.Height, this.StepByteSize);

            System.Diagnostics.Debug.Assert(this.StepByteSize == info.StepByteSize);

            return (offset, info);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WSPAN UseScanlineBytes(WSPAN bitmap, int y)
        {
            return bitmap.Slice(y * StepByteSize, Width * PixelByteSize);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<TPixel> UseScanlinePixels<TPixel>(WSPAN bitmap, int y)
            where TPixel : unmanaged
        {
            var scanline = bitmap.Slice(y * StepByteSize, Width * PixelByteSize);
            return MEMMARSHAL.Cast<Byte, TPixel>(scanline);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RSPAN GetScanlineBytes(RSPAN bitmap, int y)
        {
            return bitmap.Slice(y * StepByteSize, Width * PixelByteSize);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<TPixel> GetScanlinePixels<TPixel>(RSPAN bitmap, int y)
            where TPixel : unmanaged
        {
            var scanline = bitmap.Slice(y * StepByteSize, Width * PixelByteSize);
            return MEMMARSHAL.Cast<Byte, TPixel>(scanline);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WSPAN UsePixel(WSPAN bitmap, int x, int y)
        {
            return bitmap.Slice(y * StepByteSize + x * PixelByteSize, PixelByteSize);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WSPAN UsePixels(WSPAN bitmap, int x, int y, int pixelCount)
        {
            if (pixelCount - x > Width) throw new ArgumentOutOfRangeException(nameof(pixelCount));

            return bitmap.Slice(y * StepByteSize + x * PixelByteSize, PixelByteSize * pixelCount);
        }

        

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RSPAN GetPixels(RSPAN bitmap, int x, int y, int pixelCount)
        {
            if (pixelCount - x > Width) throw new ArgumentOutOfRangeException(nameof(pixelCount));

            return bitmap.Slice(y * StepByteSize + x * PixelByteSize, PixelByteSize * pixelCount);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RSPAN GetPixel(RSPAN bitmap, int x, int y)
        {
            return bitmap.Slice(y * StepByteSize + x * PixelByteSize, PixelByteSize);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref readonly TPixel GetPixelOrDefault<TPixel>(RSPAN data, int x, int y, in TPixel defval)
            where TPixel : unmanaged
        {
            System.Diagnostics.Debug.Assert(sizeof(TPixel) == PixelByteSize, $"pixel type size mismatch, expected {PixelByteSize}, but found {sizeof(TPixel)}");

            if (x < 0) return ref defval;
            else if (x >= Width) return ref defval;
            if (y < 0) return ref defval;
            else if (y >= Height) return ref defval;

            return ref MEMMARSHAL.Cast<Byte, TPixel>(data.Slice(y * StepByteSize))[x];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref readonly TPixel GetPixelOrClamp<TPixel>(RSPAN data, int x, int y)
            where TPixel : unmanaged
        {
            System.Diagnostics.Debug.Assert(sizeof(TPixel) == PixelByteSize, $"pixel type size mismatch, expected {PixelByteSize}, but found {sizeof(TPixel)}");

            #if !NETSTANDARD2_0
            x = Math.Clamp(x, 0, Width - 1);
            y = Math.Clamp(y, 0, Height - 1);
            #else
            if (x < 0) x = 0;
            else if (x >= Width) x = Width - 1;
            if (y < 0) y = 0;
            else if (y >= Height) y = Height - 1;
            #endif

            return ref MEMMARSHAL.Cast<Byte, TPixel>(data.Slice(y*StepByteSize))[x];
        }        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref readonly TPixel GetPixelOrWrap<TPixel>(RSPAN data, int x, int y)
            where TPixel : unmanaged
        {
            x &= ~0x10000000;
            x %= Width;
            y &= ~0x10000000;
            y %= Height;

            return ref MEMMARSHAL.Cast<Byte, TPixel>(data.Slice(y * StepByteSize))[x];
        }

        /// <inheritdoc/>        
        public override string ToString()
        {
            return $"{PixelFormat} x {Width} x {Height}";
        }

        #endregion

        #region factory

        /// <summary>
        /// Checks whether the given <typeparamref name="TPixel"/> is compatible with this pixel format.
        /// </summary>
        /// <typeparam name="TPixel">The pixel type to test</typeparam>
        /// <exception cref="PixelFormatNotSupportedException">Exception thrown if not compatible</exception>
        [System.Diagnostics.DebuggerStepThrough]
        public void ArgumentIsCompatiblePixelFormat<TPixel>() where TPixel : unmanaged
        {
            if (!IsCompatiblePixel<TPixel>()) throw new PixelFormatNotSupportedException($"generic {typeof(TPixel).Name} is not compatible with {this.PixelFormat}");
        }

        public unsafe bool IsCompatiblePixel<TPixel>() where TPixel:unmanaged
        {
            // if we create an empty SpanBitmap or BitmapInfo with "default" all values are zero,
            // but theoretically, it's safe because there's no pixels to access either.
            if (IsEmpty) return true;

            if (sizeof(TPixel) != this.PixelByteSize) return false;

            return PixelFormat.IsCompatibleFormat<TPixel>();
        }

        public bool CreateBitmap(ref SpanBitmap bmp, bool keepPixelStride = false)
        {
            if (!AreEqual(this, bmp.Info, keepPixelStride))
            {
                var nfo = keepPixelStride ? this : WithSize(this.Width, this.Height);

                bmp = new SpanBitmap(new Byte[this.BitmapByteSize], nfo);
                return true;
            }
            return false;
        }

        public bool CreateBitmap<T>(ref SpanBitmap<T> bmp, bool keepPixelStride = false)
            where T : unmanaged
        {
            if (!AreEqual(this, bmp.Info, keepPixelStride))
            {
                var nfo = keepPixelStride ? this : WithSize(this.Width, this.Height);

                bmp = new SpanBitmap<T>(new Byte[this.BitmapByteSize], nfo);
                return true;
            }
            return false;
        }

        public bool CreateBitmap(ref MemoryBitmap bmp, bool keepPixelStride = false)
        {
            if (!AreEqual(this, bmp.Info, keepPixelStride))
            {
                var nfo = keepPixelStride ? this : WithSize(this.Width, this.Height);

                bmp = new MemoryBitmap(nfo);
                return true;
            }
            return false;
        }

        public bool CreateBitmap<T>(ref MemoryBitmap<T> bmp, bool keepPixelStride = false)
            where T:unmanaged
        {
            if (!AreEqual(this, bmp.Info, keepPixelStride))
            {
                var nfo = keepPixelStride ? this : WithSize(this.Width,this.Height);

                bmp = new MemoryBitmap<T>(nfo);
                return true;
            }
            return false;
        }

        #endregion
    }
}
