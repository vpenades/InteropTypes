using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Advanced;
using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Adapters
{
    class ImageSharpMemoryManager<TPixel> : MemoryManager<Byte>, MemoryBitmap.ISource
        where TPixel : unmanaged, IPixel<TPixel>
    {
        #region lifecycle

        public ImageSharpMemoryManager(Image<TPixel> image, bool owned)
        {
            _Image = image;
            _Owned = owned;

            _Binfo = _Implementation.TryGetExactPixelFormat<TPixel>(out var pfmt)
                ? new BitmapInfo(image.Width, image.Height, pfmt)
                : throw new NotImplementedException(typeof(TPixel).Name);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) // managed resources handling.
            {
                if (_Unmanaged != IntPtr.Zero)
                {
                    var src = new PointerBitmap(_Unmanaged, _Binfo)
                        .AsSpanBitmap();

                    _Implementation.CopyPixels(src, _Image);
                }

                if (_Owned && _Image != null) { System.Threading.Interlocked.Exchange(ref _Image, null)?.Dispose(); }
                
                if (_Unmanaged != IntPtr.Zero) // unamanaged resources handling.
                {
                    // In theory, we should also release this memory when called from
                    // the finalizer. BUT if we've been called by the finalizer is
                    // because our object has not been disposed properly and our ptr
                    // might still be in use, so if we free the memory we could cause
                    // memory corruption. So it's better to leak memory than to
                    // corrupt memory.

                    System.Runtime.InteropServices.Marshal.FreeHGlobal(_Unmanaged);
                    _Unmanaged = IntPtr.Zero;
                }
            }            
        }
        
        #endregion

        #region data

        private bool _Owned;
        private Image<TPixel> _Image;
        private BitmapInfo _Binfo;

        private IntPtr _Unmanaged;

        #endregion

        #region  API

        public MemoryBitmap Bitmap => new MemoryBitmap(this.Memory, _Binfo);

        public override Span<byte> GetSpan()
        {
            if (_Image.DangerousTryGetSinglePixelMemory(out Memory<TPixel> span))
            {
                return System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Byte>(span.Span);
            }

            throw new NotSupportedException();            
        }

        private IntPtr _UseGlobalMemory()
        {
            if (_Unmanaged != IntPtr.Zero) return _Unmanaged;

            var span = GetSpan();

            _Unmanaged = System.Runtime.InteropServices.Marshal.AllocHGlobal(span.Length);

            _Implementation.CopyPixels(_Image, new PointerBitmap(_Unmanaged, _Binfo).AsSpanBitmap());

            return _Unmanaged;
        }

        public unsafe override MemoryHandle Pin(int elementIndex = 0)
        {
            // The ideal solution here would be to get a pinned reference from GetSpan(),
            // but there's no 100% safe way of doing it and return a MemoryHandle.

            // so for now, we will make a copy of the bitmap in unmanaged memory

            // System.Runtime.InteropServices.GCHandle.FromIntPtr()

            var ptr = _UseGlobalMemory();

            ptr += elementIndex;

            return new MemoryHandle(ptr.ToPointer(), default, this);

        }

        public override void Unpin()
        {
            
        }

        #endregion
    }
}
