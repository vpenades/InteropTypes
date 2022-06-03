using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Adapters
{
    public struct SkiaMemoryAdapter : IDisposable
    {
        #region constructor
        public SkiaMemoryAdapter(MemoryBitmap bmp)
        {
            _SourceBitmap = bmp;

            _Handle = bmp.Memory.Pin();            

            var ptr = new PointerBitmap(_Handle.Value, bmp.Info);

            _ProxyBitmap = _Implementation.WrapAsSKBitmap(ptr);
            _DeviceContext = new SkiaSharp.SKCanvas(_ProxyBitmap);
        }

        public void Dispose()
        {
            System.Threading.Interlocked.Exchange(ref _DeviceContext, null)?.Dispose();
            System.Threading.Interlocked.Exchange(ref _ProxyBitmap, null)?.Dispose();

            _Handle?.Dispose();
            _Handle = null;

            _SourceBitmap = default;
        }

        #endregion        

        #region data

        private MemoryBitmap _SourceBitmap;

        private System.Buffers.MemoryHandle? _Handle;

        private SkiaSharp.SKBitmap  _ProxyBitmap;
        private SkiaSharp.SKCanvas _DeviceContext;

        #endregion

        #region properties

        public readonly SkiaSharp.SKBitmap Bitmap => _ProxyBitmap;

        public readonly SkiaSharp.SKCanvas Canvas => _DeviceContext;

        #endregion        
    }
}
