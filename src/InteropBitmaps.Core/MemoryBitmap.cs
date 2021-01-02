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
    [System.Diagnostics.DebuggerDisplay("{Info._DebuggerDisplay(),nq}")]
    // [System.Diagnostics.DebuggerTypeProxy(typeof(Debug.SpanBitmapProxy))]
    public readonly struct MemoryBitmap
    {
        #region lifecycle        
        public MemoryBitmap(in BitmapInfo info)
            : this(new byte[info.BitmapByteSize], info) { }

        public MemoryBitmap(Byte[] array, in BitmapInfo info)
        {
            _Info = info;            
            _Data = array.AsMemory().Slice(0, _Info.BitmapByteSize);
        }

        public MemoryBitmap(Memory<Byte> data, in BitmapInfo info)
        {
            _Info = info;
            _Data = data.Slice(0, _Info.BitmapByteSize);
        }

        public MemoryBitmap(int width, int height, Pixel.Format pixelFormat, int stepByteSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, stepByteSize);
            var bytes = new byte[_Info.BitmapByteSize];            
            _Data = bytes;
        }        

        public MemoryBitmap(Memory<Byte> data, int width, int height, Pixel.Format pixelFormat, int stepByteSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, stepByteSize);
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
        public ReadOnlySpan<byte> GetScanlineBytes(int y) { return _Info.GetScanlineBytes(_Data.Span, y); }

        public Memory<TPixel> GetPixelMemory<TPixel>() where TPixel : unmanaged
        {
            return new MemoryManagers.CastMemoryManager<Byte, TPixel>(_Data).Memory;
        }
        
        /// <summary>
        /// Gets the underlaying byte array, as long as the backing <see cref="Memory"/> was constructed from an <see cref="Array"/>.
        /// </summary>
        /// <param name="segment">The underlaying memory <see cref="Array"/></param>
        /// <returns>true if the operation succeeded.</returns>
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

        public static implicit operator SpanBitmap(MemoryBitmap bmp) { return new SpanBitmap(bmp._Data.Span, bmp._Info); }

        public SpanBitmap AsSpanBitmap() { return this; }

        public unsafe MemoryBitmap<TPixel> OfType<TPixel>()
            where TPixel : unmanaged
        { return new MemoryBitmap<TPixel>(_Data, _Info); }

        #endregion

        #region API - Pixel Ops
        
        public void SetPixels(int dstX, int dstY, SpanBitmap src) { AsSpanBitmap().SetPixels(dstX, dstY, src); }        

        public MemoryBitmap Slice(in BitmapBounds rect)
        {
            var (offset, info) = _Info.Slice(rect);
            var memory = this._Data.Slice(offset, info.BitmapByteSize);
            return new MemoryBitmap(memory, info);
        }        
        
        #endregion

        #region API - IO

        public static MemoryBitmap Read(System.IO.Stream s, params Codecs.IBitmapDecoding[] factory)
        {
            Guard.NotNull(nameof(s), s);
            Guard.IsTrue(nameof(s), s.CanRead);
            Guard.GreaterThan(nameof(factory), factory.Length, 0);

            if (!s.CanSeek && factory.Length > 1)
            {
                using (var mem = new System.IO.MemoryStream())
                {
                    s.CopyTo(mem);
                    mem.Position = 0;
                    return Read(mem, factory);
                }
            }

            var startPos = s.Position;

            foreach (var f in factory)
            {
                if (f.TryRead(s, out MemoryBitmap bmp)) return bmp;
                if (s.CanSeek) s.Position = startPos;
            }

            throw new ArgumentException("invalid format", nameof(s));
        }

        public static MemoryBitmap Load(string filePath, params Codecs.IBitmapDecoding[] factory)
        {
            foreach (var f in factory)
            {                
                using (var s = System.IO.File.OpenRead(filePath))
                {
                    if (f.TryRead(s, out MemoryBitmap bmp)) return bmp;
                }                
            }

            throw new ArgumentException("invalid format", nameof(filePath));
        }

        public void Save(string filePath, params Codecs.IBitmapEncoding[] factory)
        {
            this.AsSpanBitmap().Save(filePath, factory);
        }

        public void Write(System.IO.Stream stream, Codecs.CodecFormat format, params Codecs.IBitmapEncoding[] factory)
        {
            this.AsSpanBitmap().Write(stream, format, factory);
        }        

        #endregion        
    }    
}
