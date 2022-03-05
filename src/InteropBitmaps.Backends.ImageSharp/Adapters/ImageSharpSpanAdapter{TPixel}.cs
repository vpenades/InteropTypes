using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace InteropTypes.Graphics.Adapters
{
    /// <summary>
    /// Wraps a <see cref="SpanBitmap{TPixel}"/> so it can be used as an ImageSharp's <see cref="Image{TPixel}"/>
    /// </summary>
    /// <typeparam name="TPixel"></typeparam>
    public readonly ref struct ImageSharpSpanAdapter<TPixel>
    where TPixel : unmanaged, IPixel<TPixel>
    {
        #region constructor

        public ImageSharpSpanAdapter(SpanBitmap bmp) { _Bitmap = bmp.OfType<TPixel>(); }

        public ImageSharpSpanAdapter(SpanBitmap<TPixel> bmp) { _Bitmap = bmp; }
        
        #endregion

        #region data

        private readonly SpanBitmap<TPixel> _Bitmap;

        #endregion

        #region API

        public Image<TPixel> CloneToImage() { return _Implementation.CloneToImageSharp(_Bitmap); }

        public void Mutate(Action<IImageProcessingContext> operation) { _Implementation.Mutate(_Bitmap, operation); }

        public MemoryBitmap<TPixel> Clone(Action<IImageProcessingContext> operation)
        {
            using (var tmp = CloneToImage())
            {
                tmp.Mutate(operation);

                return tmp.ToMemoryBitmap<TPixel>();
            }
        }

        public MemoryBitmap<TPixel> ToResizedMemoryBitmap(int width, int height) { return Clone(dc => dc.Resize(width, height)); }

        public MemoryBitmap ToResizedMemoryBitmap(ResizeOptions options) { return Clone(dc => dc.Resize(options)); }

        public double CalculateBlurFactor()
        {
            using (var img = CloneToImage())
            {
                return img.GetBlurLevel();
            }
        }        

        #endregion
    }
}
