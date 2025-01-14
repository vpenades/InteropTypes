using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

using PhotoSauce.MagicScaler;
using PhotoSauce.MagicScaler.Transforms;

namespace InteropTypes.Graphics.Backends
{
    /// <summary>
    /// Represents an <see cref="IPixelTransform"/> that performs a general
    /// <see cref="Matrix3x2"/> transform operation over the source image.
    /// </summary>
    /// <remarks>
    /// <see href="https://github.com/saucecontrol/PhotoSauce/discussions/161">PhotoSauce/discussions/16</see>
    /// </remarks>
    internal class _MatrixTransform : IPixelTransform
    {
        #region lifecycle
        public _MatrixTransform(Matrix3x2 transform, int dstWidth, int dstHeight)
        {
            Width = dstWidth;
            Height = dstHeight;
            Matrix3x2.Invert(transform, out _Transform);
        }

        public void Init(IPixelSource source)
        {
            _Source = source;
            _SourceBpp = 0;
            if (source.Format == PixelFormats.Grey8bpp) _SourceBpp = 1;
            if (source.Format == PixelFormats.Bgr24bpp) _SourceBpp = 3;
            if (source.Format == PixelFormats.Bgra32bpp) _SourceBpp = 4;            
        }

        #endregion

        #region data

        private IPixelSource _Source;
        private int _SourceBpp;

        private Matrix3x2 _Transform;

        public Guid Format => _Source.Format;        

        public int Width { get; }
        public int Height { get; }

        #endregion

        #region API

        public void CopyPixels(Rectangle sourceArea, int cbStride, Span<byte> buffer)
        {
            // sampling one pixel at a time.
            var srcRect = new Rectangle(0, 0, 1, 1);

            // step vector to the next pixel in source.
            Vector2 srcV = Vector2.TransformNormal(Vector2.UnitX, _Transform);

            for (int y=0; y < sourceArea.Height; ++y)
            {
                var dstP = new Vector2(sourceArea.X, sourceArea.Y + y);
                var srcP = Vector2.Transform(dstP, _Transform);

                var dstRow = buffer.Slice(y * cbStride, cbStride);                

                for (int x = 0; x < sourceArea.Width; ++x)
                {
                    var dstPixel = dstRow.Slice(x * _SourceBpp, _SourceBpp);

                    // set source sampling pixel.
                    srcRect.X = (int)srcP.X;
                    srcRect.Y = (int)srcP.Y;

                    // sample the pixel at Src and copy it to Dst

                    if (srcRect.X < 0 || srcRect.X >= _Source.Width-1 || srcRect.Y < 0 || srcRect.Y >= _Source.Height-1)
                    {
                        dstPixel.Fill(0);
                    }
                    else
                    {
                        _Source.CopyPixels(srcRect, _SourceBpp, dstPixel);
                    }                    

                    srcP += srcV; // advance to the next pixel in the source bitmap
                }
            }
        }

        #endregion
    }
}
