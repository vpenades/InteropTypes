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

        public ImageSharpAdapter(SpanBitmap bmp)
        {
            _Bitmap = bmp;
            _ImageSharpPixelType = bmp.PixelFormat.ToImageSharpPixelFormat();
        }

        #endregion

        #region data

        private readonly SpanBitmap _Bitmap;
        private readonly Type _ImageSharpPixelType;

        #endregion

        #region API

        public Image ToImageSharp()
        {
            var dst = _Bitmap.PixelFormat.CreateImageSharp(_Bitmap.Width,_Bitmap.Height);

            dst.AsSpanBitmap().SetPixels(0,0,_Bitmap);

            return dst;
        }

        public Image ToImageSharp<TPixel>() where TPixel:unmanaged, SixLabors.ImageSharp.PixelFormats.IPixel<TPixel>
        {
            if (typeof(TPixel) != _ImageSharpPixelType) throw new ArgumentException(nameof(TPixel));

            var dst = _Bitmap.PixelFormat.CreateImageSharp(_Bitmap.Width, _Bitmap.Height);

            dst.AsSpanBitmap().SetPixels(0, 0, _Bitmap);

            return dst;
        }

        public void Save(string filePath)
        {
            using (var img = ToImageSharp())
            {
                img.Save(filePath);
            }
        }

        public double CalculateBlurFactor()
        {
            using (var img = ToImageSharp())
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

        public void Draw(int dstx, int dsty, SpanBitmap<TPixel> other, float opacity)
        {
            var ops = default(TPixel).CreatePixelOperations();

            throw new NotImplementedException();
        }

        #endregion
    }
}
