using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace InteropBitmaps.Adapters
{
    /// <summary>
    /// Wraps a <see cref="SpanBitmap{TPixel}"/> so it can be used as an ImageSharp's <see cref="Image{TPixel}"/>
    /// </summary>
    /// <typeparam name="TPixel"></typeparam>
    public readonly ref struct ImageSharpAdapter<TPixel>
    where TPixel : unmanaged, IPixel<TPixel>
    {
        #region constructor

        public ImageSharpAdapter(SpanBitmap<TPixel> bmp) { _Bitmap = bmp; }

        #endregion

        #region data

        private readonly SpanBitmap<TPixel> _Bitmap;

        #endregion

        #region API

        public Image<TPixel> CloneToImageSharp() { return _Implementation.CloneToImageSharp(_Bitmap); }

        public void Mutate(Action<IImageProcessingContext> operation) { _Implementation.Mutate(_Bitmap, operation); }

        public MemoryBitmap<TPixel> Clone(Action<IImageProcessingContext> operation)
        {
            using (var tmp = CloneToImageSharp())
            {
                tmp.Mutate(operation);

                return tmp.ToMemoryBitmap();
            }
        }

        public MemoryBitmap<TPixel> CloneResized(int width, int height) { return Clone(dc => dc.Resize(width, height)); }

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
