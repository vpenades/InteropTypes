using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp.PixelFormats;

namespace InteropBitmaps.Adapters
{
    public struct ImageSharpMemoryAdapter<TPixel> : IDisposable
        where TPixel : unmanaged, IPixel<TPixel>
    {
        #region constructor
        public ImageSharpMemoryAdapter(MemoryBitmap<TPixel> bmp)
        {
            _SourceBitmap = bmp;

            if (bmp.Info.IsContinuous) _ProxyImage = _Implementation.TryWrapImageSharp<TPixel>(bmp);
            else _ProxyImage = _Implementation.CloneToImageSharp<TPixel>(bmp);
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

            System.Threading.Interlocked.Exchange(ref _ProxyImage, null)?.Dispose();
        }

        #endregion

        #region data        

        private MemoryBitmap _SourceBitmap;
        private SixLabors.ImageSharp.Image<TPixel> _ProxyImage;

        #endregion

        #region properties

        public SixLabors.ImageSharp.Image<TPixel> Image => _ProxyImage;

        #endregion
    }
}
