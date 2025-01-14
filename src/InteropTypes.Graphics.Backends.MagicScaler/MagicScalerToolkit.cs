using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

using PhotoSauce.MagicScaler;
using PhotoSauce.MagicScaler.Transforms;

namespace InteropTypes.Graphics.Backends
{
    public static class MagicScalerToolkit
    {
        /// <summary>
        /// Wraps the current <see cref="MemoryBitmap"/> as a <see cref="IPixelSource"/>
        /// </summary>
        /// <param name="bmp">The bitmap to wrap</param>
        /// <returns></returns>
        public static IPixelSource AsPixelSource(MemoryBitmap bmp)
        {
            return new _BitmapAdapter(bmp);
        }

        /// <summary>
        /// Converts the current <see cref="SpanBitmap"/> to a <see cref="IPixelSource"/>
        /// </summary>
        /// <param name="bmp">The bitmap to convert</param>
        /// <returns></returns>
        public static IPixelSource ToPixelSource(SpanBitmap bmp)
        {
            return new _BitmapAdapter(bmp);
        }

        public static IPixelTransform CreateMatrixTransform(Matrix3x2 xform, int dstWidth, int dstHeight)
        {
            return new _MatrixTransform(xform, dstWidth, dstHeight);
        }

        public static void Rescale(SpanBitmap srcBitmap, SpanBitmap dstBitmap)
        {
            _Implementation.RescaleTo(srcBitmap, dstBitmap);
        }

        public static void Rescale(MemoryBitmap srcBitmap, SpanBitmap dstBitmap)
        {
            _Implementation.RescaleTo(srcBitmap, dstBitmap);
        }

        public static MemoryBitmap Rescale(MemoryBitmap srcBitmap, int dstWidth, int dstHeight)
        {
            return  _Implementation.Rescale(srcBitmap, dstWidth, dstHeight);
        }

        public static MemoryBitmap ToMemoryBitmap(IPixelSource src)
        {
            return _Implementation.ToMemoryBitmap(src);
        }
    }
}
