using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Adapters
{
    public struct SkiaMemoryAdapter : IDisposable
    {
        #region constructor
        public SkiaMemoryAdapter(MemoryBitmap bmp)
        {
            _SourceBitmap = bmp;

            _Handle = bmp.Memory.Pin();            

            var ptr = new PointerBitmap(_Handle.Value, bmp.Info);

            _ProxyBitmap = _Implementation.AsSKBitmap(ptr);
            _DeviceContext = new SkiaSharp.SKCanvas(_ProxyBitmap);
        }

        public void Dispose()
        {
            _DeviceContext?.Dispose();
            _DeviceContext = null;

            _ProxyBitmap?.Dispose();
            _ProxyBitmap = null;

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

        public SkiaSharp.SKBitmap Bitmap => _ProxyBitmap;

        public SkiaSharp.SKCanvas Canvas => _DeviceContext;

        #endregion        
    }
}
