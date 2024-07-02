using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

using InteropTypes.Tensors;
using InteropTypes.Tensors.Imaging;

namespace InteropTypes.Vision
{
    /// <summary>
    /// Helper class used to copy images to a tensor
    /// </summary>
    /// <typeparam name="TPixel"></typeparam>
    public class ImageProcessor<TPixel> : ITensorImageProcessor<float>
            where TPixel : unmanaged, Pixel.IReflection
    {
        #region constructor
        public ImageProcessor()
        {
            TensorFormat = PixelFormat.TryIdentifyFormat<TPixel>();
        }
        
        public ImageProcessor(MultiplyAdd transform) : this()
        {
            ColorTransform = transform;
        }

        #endregion

        #region properties

        public PixelFormat TensorFormat { get; }

        public MultiplyAdd ColorTransform { get; set; } = MultiplyAdd.Identity;

        #endregion

        #region API

        void ITensorImageProcessor<float>.CopyImage(SpanTensor3<float> src, SpanBitmap dst)
        {
            throw new NotImplementedException();
        }

        public void CopyImage(SpanBitmap src, SpanTensor3<float> dst)
        {
            switch(src.PixelFormat.Code)
            {
                case Pixel.BGR24.Code: CopyImage(src.OfType<Pixel.BGR24>(), dst); return;
                case Pixel.RGB24.Code: CopyImage(src.OfType<Pixel.RGB24>(), dst); return;
                case Pixel.BGRA32.Code: CopyImage(src.OfType<Pixel.BGRA32>(), dst); return;
                case Pixel.RGBA32.Code: CopyImage(src.OfType<Pixel.RGBA32>(), dst); return;
                case Pixel.Undefined24.Code: CopyImage(src.OfType<Pixel.Undefined24>(), dst); return;
            }

            throw new NotImplementedException($"{src.PixelFormat}");
        }

        /// <summary>
        /// Uses "Fitting"
        /// </summary>
        /// <typeparam name="TSrcPixel"></typeparam>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CopyImage<TSrcPixel>(SpanBitmap<TSrcPixel> src, SpanTensor3<float> dst)
            where TSrcPixel:unmanaged, Pixel.IReflection
        {
            var dstEncoding = _TensorsExtensions.GetColorEncoding<TPixel>();

            var cmad = ColorTransform;
            if (default(TSrcPixel).IsQuantized) cmad = cmad.ConcatMul(1f / 255f);

            var sampler = src.AsBitmapSampler();            

            if (dst.Dimensions[2] == 3) // dst[h][w][3] INTERLEAVED
            {
                var tmpTensor = dst.UpCast<System.Numerics.Vector3>();
                tmpTensor.AsTensorBitmap<float>(dstEncoding).FitPixels(sampler, cmad);
                return;
            }

            if (dst.Dimensions[2] == 4) // dst[h][w][4] INTERLEAVED
            {
                var tmpTensor = dst.UpCast<System.Numerics.Vector4>();
                tmpTensor.AsTensorBitmap<float>(dstEncoding).FitPixels(sampler, cmad);
                return;
            }

            if (dst.Dimensions[0] == 3) // dst[3][h][w] PLANES
            {
                new TensorBitmap<float>(dst, dstEncoding, ColorRanges.Identity).FitPixels(sampler, cmad);
                return;
            }            

            throw new NotImplementedException();
        }
        
        #endregion
    }
}
