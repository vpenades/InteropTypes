using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Adapters
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

                _Implementation.Copy(_ProxyImage, _SourceBitmap);

                _SourceBitmap = default;
            }

            System.Threading.Interlocked.Exchange(ref _ProxyImage, null)?.Dispose();
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
