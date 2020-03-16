using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace InteropBitmaps.Adapters
{
    public readonly ref struct ImageSharpAdapter
    {
        #region constructor

        public ImageSharpAdapter(SpanBitmap bmp)
        {
            _Bitmap = bmp;
            _ImageSharpPixelType = _Implementation.ToPixelFormat(bmp.PixelFormat);
        }

        #endregion

        #region data

        private readonly SpanBitmap _Bitmap;
        private readonly Type _ImageSharpPixelType;

        #endregion

        #region API

        public Image CloneToImageSharp() { return _Implementation.CloneToImageSharp(_Bitmap); }

        public Image<TPixel> CloneToImageSharp<TPixel>() where TPixel:unmanaged, SixLabors.ImageSharp.PixelFormats.IPixel<TPixel>
        {
            if (typeof(TPixel) != _ImageSharpPixelType) throw new ArgumentException(nameof(TPixel));

            return _Implementation.CloneToImageSharp<TPixel>(_Bitmap);
        }        

        public double CalculateBlurFactor()
        {
            using (var img = CloneToImageSharp())
            {
                return img.EvaluateBlurFactor();
            }
        }

        #endregion
    }    
}
