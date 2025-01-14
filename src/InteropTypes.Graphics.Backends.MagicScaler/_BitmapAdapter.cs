using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

using PhotoSauce.MagicScaler.Transforms;
using PhotoSauce.MagicScaler;

namespace InteropTypes.Graphics.Backends
{
    internal readonly struct _BitmapAdapter : PhotoSauce.MagicScaler.IPixelSource
    {
        #region lifecycle

        public _BitmapAdapter(SpanBitmap bmp)
        {
            PixelFormat? _convertTo = null;

            switch(bmp.PixelFormat)
            {
                case Pixel.Luminance16.Code: _convertTo = Pixel.Luminance8.Format; break;
                case Pixel.Luminance32F.Code: _convertTo = Pixel.Luminance8.Format; break;

                case Pixel.BGR565.Code: _convertTo = Pixel.BGR24.Format; break;
                case Pixel.RGB24.Code: _convertTo = Pixel.BGR24.Format; break;
                case Pixel.RGB96F.Code: _convertTo = Pixel.BGR24.Format; break;

                case Pixel.BGRA5551.Code: _convertTo = Pixel.BGRA32.Format; break;
                case Pixel.BGRA4444.Code: _convertTo = Pixel.BGRA32.Format; break;
                case Pixel.ARGB32.Code: _convertTo = Pixel.BGRA32.Format; break;
                case Pixel.RGBA32.Code: _convertTo = Pixel.BGRA32.Format; break;
                case Pixel.RGBA128F.Code: _convertTo = Pixel.BGRA32.Format; break;
                case Pixel.BGRA128F.Code: _convertTo = Pixel.BGRA32.Format; break;
            }

            _Source = bmp.ToMemoryBitmap(_convertTo);

            Format = _Implementation.ToMagicScalerFormat(_Source.PixelFormat);
        }

        public _BitmapAdapter(MemoryBitmap bmp)
        {
            _Source = bmp;
            Format = _Implementation.ToMagicScalerFormat(_Source.PixelFormat);
        }

        #endregion

        #region data

        private readonly MemoryBitmap _Source;
        public Guid Format { get; }
        public int Width => _Source.Width;
        public int Height => _Source.Height;

        #endregion

        #region API

        public void CopyPixels(Rectangle sourceArea, int cbStride, Span<byte> buffer)
        {
            var croppedSrc = _Source.AsSpanBitmap().Slice((sourceArea.X, sourceArea.Y, sourceArea.Width, sourceArea.Height));

            for (int y = 0; y < croppedSrc.Height; y++)
            {
                var srcRow = croppedSrc.GetScanlineBytes(y);
                var dstRow = buffer.Slice(0, cbStride);
                srcRow.CopyTo(dstRow);

                // If Format is RGB instead of BGR, we could shuffle the bytes of dstRow                

                buffer = buffer.Slice(cbStride);
            }
        }

        public void ResizeTo(SpanBitmap dstBitmap)
        {
            if (_Implementation.ToMagicScalerFormatOrDefault(dstBitmap.PixelFormat) == Guid.Empty) throw new ArgumentException($"{dstBitmap.PixelFormat} not supported", nameof(dstBitmap));

            // https://github.com/saucecontrol/PhotoSauce/discussions/175

            var settings = new ProcessImageSettings
            {
                Width = dstBitmap.Width,
                Height = dstBitmap.Height,
                // The default profile mode may convert to other well-known color spaces,
                // but there is currently no way to retrieve the profile. Normalizing to
                // sRGB ensures the color space is known.
                ColorProfileMode = ColorProfileMode.ConvertToSrgb
            };

            using var pipeline = MagicImageProcessor.BuildPipeline(this, settings);

            // normalize the pixel format to match destination            
            pipeline.AddTransform(new FormatConversionTransform(this.Format));

            _Implementation.CopyTo(pipeline.PixelSource, dstBitmap);
        }

        #endregion
    }
}
