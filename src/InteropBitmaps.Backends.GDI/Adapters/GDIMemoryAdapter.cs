using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Adapters
{
    public struct GDIMemoryAdapter : IDisposable
    {
        #region constructor
        public GDIMemoryAdapter(MemoryBitmap bmp)
        {
            _SourceBitmap = bmp;

            _Handle = bmp.Memory.Pin();            

            var ptr = new PointerBitmap(_Handle.Value, bmp.Info);

            _ProxyBitmap = _Implementation.WrapAsGDIBitmap(ptr);
            _DeviceContext = System.Drawing.Graphics.FromImage(_ProxyBitmap);
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
        
        private System.Drawing.Bitmap _ProxyBitmap;
        private System.Drawing.Graphics _DeviceContext;

        #endregion

        #region properties

        public System.Drawing.Bitmap Bitmap => _ProxyBitmap;

        public System.Drawing.Graphics Canvas => _DeviceContext;           

        #endregion        
    }
}
