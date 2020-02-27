using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;

namespace InteropBitmaps
{
    public readonly ref struct ImageSharpAdapter
    {
        #region constructor

        public ImageSharpAdapter(SpanBitmap bmp) { _Bitmap = bmp; }

        #endregion

        #region data

        private readonly SpanBitmap _Bitmap;

        #endregion

        #region API

        public Image CloneToImageSharp()
        {
            var dst = _Bitmap.PixelFormat.ImageSharpCreateEmptyImage(_Bitmap.Width,_Bitmap.Height);

            dst.AsSpanBitmap().SetPixels(0,0,_Bitmap);

            return dst;
        }

        public void Save(string filePath)
        {
            using (var img = CloneToImageSharp())
            {
                img.Save(filePath);
            }
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

        /// <summary>
        /// Wraps a <see cref="SpanBitmap{TPixel}"/> so it can be used as an ImageSharp's <see cref="Image{TPixel}"/>
        /// </summary>
        /// <typeparam name="TPixel"></typeparam>
        public readonly ref struct ImageSharpAdapter<TPixel>
        where TPixel : unmanaged, SixLabors.ImageSharp.PixelFormats.IPixel<TPixel>
    {
        #region constructor

        public ImageSharpAdapter(SpanBitmap<TPixel> bmp) { _Bitmap = bmp; }

        #endregion

        #region data

        private readonly SpanBitmap<TPixel> _Bitmap;

        #endregion

        #region API

        public Image<TPixel> CloneToImageSharp()
        {
            var dst = new Image<TPixel>(_Bitmap.Width, _Bitmap.Height);

            for (int y = 0; y < dst.Height; ++y)
            {
                var srcLine = _Bitmap.GetPixelsScanline(y);
                var dstLine = dst.Frames[0].GetPixelRowSpan(y);
                srcLine.CopyTo(dstLine);
            }

            return dst;
        }

        public void Mutate(Action<IImageProcessingContext> operation)
        {
            using (var tmp = CloneToImageSharp())
            {
                tmp.Mutate(operation);

                // if size has changed, throw error.
                if (tmp.Width != _Bitmap.Width || tmp.Height != _Bitmap.Height) throw new ArgumentException("Operations that resize the source image are not allowed.", nameof(operation));

                // transfer pixels back to src.
                _Bitmap.SetPixels(0, 0, tmp.AsSpanBitmap());
            }
        }

        public MemoryBitmap<TPixel> Clone(Action<IImageProcessingContext> operation)
        {
            using (var tmp = CloneToImageSharp())
            {
                tmp.Mutate(operation);

                return tmp.CopyToMemoryBitmap();
            }
        }

        public MemoryBitmap<TPixel> CloneResized(int width, int height)
        {
            return Clone(dc => dc.Resize(width, height));
        }

        public void Save(string filePath)
        {
            using (var img = CloneToImageSharp())
            {
                img.Save(filePath);
            }
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
