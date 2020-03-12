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
            _Array = new ArraySegment<byte>(array, 0, _Info.BitmapByteSize);
            _Data = array.AsMemory().Slice(0, _Info.BitmapByteSize);
        }

        public MemoryBitmap(Memory<Byte> data, in BitmapInfo info)
        {
            _Info = info;
            _Array = default;
            _Data = data.Slice(0, _Info.BitmapByteSize);
        }

        public MemoryBitmap(int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, scanlineSize);
            var bytes = new byte[_Info.BitmapByteSize];
            _Array = new ArraySegment<byte>(bytes);
            _Data = bytes;
        }        

        public MemoryBitmap(Memory<Byte> data, int width, int height, PixelFormat pixelFormat, int scanlineSize = 0)
        {
            _Info = new BitmapInfo(width, height, pixelFormat, scanlineSize);
            _Array = default;
            _Data = data.Slice(0, _Info.BitmapByteSize);
        }

        #endregion

        #region data

        private readonly ArraySegment<Byte> _Array;
        private readonly Memory<Byte> _Data;        
        private readonly BitmapInfo _Info;

        #endregion

        #region properties

        public int Width => _Info.Width;
        public int Height => _Info.Height;
        public int PixelSize => _Info.PixelSize;
        public int ScanlineSize => _Info.ScanlineSize;

        public PixelFormat PixelFormat => new PixelFormat(_Info.PixelFormat);

        protected BitmapInfo Info => _Info;

        public Memory<Byte> Buffer => _Data;

        public SpanBitmap Span => new SpanBitmap(_Data.Span, _Info);

        #endregion

        #region API

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

        public Byte[] ToArray()
        {
            if (_Array.Offset == 0 && _Array.Array!=null && _Array.Array.Length == _Info.BitmapByteSize) return _Array.Array;
            return _Data.ToArray();            
        }

        #endregion

        #region CODECS IO

        public static MemoryBitmap Read(System.IO.Stream s, params IBitmapDecoding[] factory)
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
                try
                {                    
                    return f.Read(s);                    
                }
                catch (NotSupportedException)
                {
                    if (s.CanSeek) s.Position = startPos;
                }                
            }

            return null;
        }

        public static MemoryBitmap Load(string filePath, params IBitmapDecoding[] factory)
        {
            foreach (var f in factory)
            {
                try
                {
                    using (var s = System.IO.File.OpenRead(filePath))
                    {
                        return f.Read(s);
                    }
                }
                catch (NotSupportedException) { }
            }

            return null;
        }

        public void Save(string filePath, params IBitmapEncoding[] factory)
        {
            var ext = System.IO.Path.GetExtension(filePath);

            foreach (var f in factory)
            {
                try
                {
                    using (var s = System.IO.File.Create(filePath))
                    {
                        f.Write(s, ext, this);
                    }
                }
                catch (NotSupportedException) { }
            }
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
