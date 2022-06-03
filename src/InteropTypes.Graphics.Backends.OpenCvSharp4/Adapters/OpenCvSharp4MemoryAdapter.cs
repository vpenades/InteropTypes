using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Adapters
{
    
    public struct OpenCvSharp4MemoryAdapter : IDisposable
    {
        #region constructor
        public OpenCvSharp4MemoryAdapter(MemoryBitmap bmp)
        {
            _Handle = bmp.Memory.Pin();

            _SourcePointer = new PointerBitmap(_Handle.Value, bmp.Info);
            _SourceBitmap = bmp;

            _ProxyBitmap = _Implementation.WrapAsMat(_SourcePointer);            
        }

        public void Dispose()
        {
            if (_ProxyBitmap != null)
            {
                if (_ProxyBitmap.Data != _SourcePointer.Pointer)
                {
                    // the proxy content has changed, let's try to retrieve the data
                    throw new NotImplementedException();
                }
            }

            System.Threading.Interlocked.Exchange(ref _ProxyBitmap, null)?.Dispose();

            _Handle?.Dispose();
            _Handle = null;

            _SourceBitmap = default;
        }

        #endregion

        #region data

        private MemoryBitmap _SourceBitmap;
        private PointerBitmap _SourcePointer;

        private System.Buffers.MemoryHandle? _Handle;

        private OpenCvSharp.Mat _ProxyBitmap;        

        #endregion

        #region properties

        public readonly OpenCvSharp.Mat Mat => _ProxyBitmap;        

        #endregion
    }
}
