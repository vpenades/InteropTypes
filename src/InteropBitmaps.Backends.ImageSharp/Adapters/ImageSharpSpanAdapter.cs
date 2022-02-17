using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace InteropBitmaps.Adapters
{
    public readonly ref struct ImageSharpSpanAdapter
    {
        #region constructor

        public ImageSharpSpanAdapter(SpanBitmap bmp)
        {
            _Bitmap = bmp;
            _Implementation.TryGetExactPixelType(bmp.PixelFormat, out _ImageSharpPixelType);
        }

        #endregion

        #region data

        private readonly SpanBitmap _Bitmap;
        private readonly Type _ImageSharpPixelType;

        #endregion

        #region API

        private ImageSharpSpanAdapter<TPixel> OfType<TPixel>()
            where TPixel : unmanaged, IPixel<TPixel>
        { return new ImageSharpSpanAdapter<TPixel>(_Bitmap); }


        public Image CloneToImage() { return _Implementation.CloneToImageSharp(_Bitmap); }            

        public Image<TPixel> CloneToImage<TPixel>() where TPixel:unmanaged, IPixel<TPixel>
        {
            if (typeof(TPixel) == _ImageSharpPixelType) return _Implementation.CloneToImageSharp<TPixel>(_Bitmap);

            return _Implementation.CloneToImageSharp(_Bitmap).CloneAs<TPixel>();
        }        

        public double CalculateBlurFactor()
        {
            using (var img = CloneToImage())
            {
                return img.GetBlurLevel();
            }
        }

        public void Mutate(Action<IImageProcessingContext> operation) { _Implementation.Mutate(_Bitmap, operation); }

        public MemoryBitmap Clone(Action<IImageProcessingContext> operation)
        {
            using (var tmp = CloneToImage())
            {
                tmp.Mutate(operation);

                return tmp.ToMemoryBitmap();
            }
        }

        public MemoryBitmap ToResizedMemoryBitmap(int width, int height) { return Clone(dc => dc.Resize(width, height)); }

        public MemoryBitmap ToResizedMemoryBitmap(ResizeOptions options) { return Clone(dc => dc.Resize(options)); }

        #endregion
    }    
}
