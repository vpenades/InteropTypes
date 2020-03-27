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

        public MemoryBitmap(in BitmapInfo info)
            : this(new byte[info.BitmapByteSize], info)
        {            
        }

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

        public MemoryBitmap(int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, scanlineSize);
            var bytes = new byte[_Info.BitmapByteSize];            
            _Data = bytes;
        }        

        public MemoryBitmap(Memory<Byte> data, int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, scanlineSize);            
            _Data = data.Slice(0, _Info.BitmapByteSize);
        }

        #endregion

        #region data
        
        private Memory<Byte> _Data;        
        private BitmapInfo _Info;

        #endregion

        #region properties

        public int Width => _Info.Width;
        public int Height => _Info.Height;
        public int PixelSize => _Info.PixelByteSize;
        public int ScanlineSize => _Info.ScanlineByteSize;

        public PixelFormat PixelFormat => new PixelFormat(_Info.PixelFormat);

        protected BitmapInfo Info => _Info;

        public Memory<Byte> Memory => _Data;

        public SpanBitmap Span => new SpanBitmap(_Data.Span, _Info);

        #endregion

        #region API

        protected void DisposeBuffers()
        {            
            _Data = Memory<Byte>.Empty;
            _Info = default;
        }        

        public void SetPixels(int dstX, int dstY, SpanBitmap src)
        {
            AsSpanBitmap().SetPixels(dstX, dstY, src);
        }
        
        public Span<byte> UseBytesScanline(int y) { return _Info.UseScanline(_Data.Span, y); }

        public SpanBitmap AsSpanBitmap() { return new SpanBitmap(_Data.Span, _Info); }
        
        public unsafe SpanBitmap<TPixel> AsSpanBitmapOfType<TPixel>()
            where TPixel : unmanaged
        { return new SpanBitmap<TPixel>(_Data.Span, _Info); }

        public unsafe MemoryBitmap<TPixel> OfType<TPixel>()
            where TPixel : unmanaged
        { return new MemoryBitmap<TPixel>(_Data, _Info); }        

        public Memory<T> MemoryOfType<T>()
            where T:unmanaged
        { return new MemoryManagers.CastMemoryManager<Byte, T>(_Data).Memory; }


        public bool TryGetBuffer(out ArraySegment<Byte> segment)
        {
            return System.Runtime.InteropServices.MemoryMarshal.TryGetArray(_Data, out segment);
        }

        public Byte[] ToArray()
        {
            if (TryGetBuffer(out ArraySegment<Byte> array) && array.Offset == 0 && array.Array.Length == _Info.BitmapByteSize) return array.Array;
            
            return _Data.ToArray();            
        }

        #endregion

        #region CODECS IO

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

        #endregion        
    }    
}
