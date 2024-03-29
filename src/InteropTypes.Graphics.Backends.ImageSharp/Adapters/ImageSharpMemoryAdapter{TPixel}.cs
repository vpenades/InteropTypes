﻿using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

using SixLabors.ImageSharp.PixelFormats;

namespace InteropTypes.Graphics.Adapters
{
    public struct ImageSharpMemoryAdapter<TPixel> : IDisposable
        where TPixel : unmanaged, IPixel<TPixel>
    {
        #region constructor
        public ImageSharpMemoryAdapter(MemoryBitmap<TPixel> bmp)
        {
            _SourceBitmap = bmp;

            if (bmp.Info.IsContinuous) _ProxyImage = _Implementation.WrapAsImageSharp<TPixel>(bmp);
            else _ProxyImage = _Implementation.CloneToImageSharp<TPixel>(bmp);
        }

        public void Dispose()
        {
            if (!_SourceBitmap.IsEmpty)
            {
                if (_ProxyImage.Width != _SourceBitmap.Width || _ProxyImage.Height != _SourceBitmap.Height) throw new ArgumentException("Operations that resize the source image are not allowed.", nameof(Image));

                _Implementation.CopyPixels(_ProxyImage, _SourceBitmap);

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

        public readonly SixLabors.ImageSharp.Image<TPixel> Image => _ProxyImage;

        #endregion
    }
}
