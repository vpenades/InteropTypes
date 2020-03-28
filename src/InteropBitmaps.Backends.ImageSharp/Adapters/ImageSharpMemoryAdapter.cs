using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Adapters
{
    public struct ImageSharpMemoryAdapter : IDisposable
    {
        #region constructor
        public ImageSharpMemoryAdapter(MemoryBitmap bmp)
        {
            _SourceBitmap = bmp;

            if (bmp.Info.IsContinuous) _ProxyImage = _Implementation.TryWrapImageSharp(bmp);
            else _ProxyImage = _Implementation.CloneToImageSharp(bmp);
        }

        public void Dispose()
        {
            if (!_SourceBitmap.IsEmpty)
            {
                if (_ProxyImage.Width != _SourceBitmap.Width || _ProxyImage.Height != _SourceBitmap.Height) throw new ArgumentException("Operations that resize the source image are not allowed.", nameof(Image));

                if (!_SourceBitmap.Info.IsContinuous)
                {
                    _SourceBitmap.SetPixels(0, 0, _ProxyImage.AsSpanBitmap());
                }

                _SourceBitmap = default;
            }            

            _ProxyImage?.Dispose();
            _ProxyImage = null;
        }

        #endregion

        #region data        

        private MemoryBitmap _SourceBitmap;
        private SixLabors.ImageSharp.Image _ProxyImage;

        #endregion

        #region properties

        public SixLabors.ImageSharp.Image Image => _ProxyImage;

        #endregion
    }
}
