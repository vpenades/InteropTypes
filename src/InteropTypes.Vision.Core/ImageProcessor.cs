using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

using InteropTypes.Tensors;

namespace InteropTypes.Vision
{
    public class ImageProcessor<TPixel> : ITensorImageProcessor<float>
            where TPixel : unmanaged
    {
        #region constructor
        public ImageProcessor() { }
        
        public ImageProcessor(MultiplyAdd transform) { Transform = transform; }

        #endregion

        #region properties

        public MultiplyAdd Transform { get; set; } = MultiplyAdd.Identity;

        #endregion

        #region API        

        public void CopyImage(SpanBitmap src, SpanTensor3<float> dst)
        {
            if (dst.Dimensions[2] == 3)
            {
                var tmpTensor = dst.UpCast<System.Numerics.Vector3>();                

                _FitImage(src, tmpTensor.AsSpanBitmap<System.Numerics.Vector3, TPixel>());

                Transform.ApplyTransformTo(tmpTensor.Span);

                return;
            }

            if (dst.Dimensions[2] == 4)
            {
                var tmpTensor = dst.UpCast<System.Numerics.Vector4>();

                _FitImage(src, tmpTensor.AsSpanBitmap<System.Numerics.Vector4, TPixel>());

                Transform.ApplyTransformTo(tmpTensor.Span);

                return;
            }

            if (dst.Dimensions[0] == 3)
            {
                var h = dst.Dimensions[1];
                var w = dst.Dimensions[2];

                var tmpTensor = new SpanTensor2<System.Numerics.Vector3>(h, w);

                _FitImage(src, tmpTensor.AsSpanBitmap<System.Numerics.Vector3, TPixel>());

                SpanTensor.Copy(tmpTensor, dst, Transform);
                return;
            }

            if (dst.Dimensions[0] == 4)
            {
                var h = dst.Dimensions[0];
                var w = dst.Dimensions[1];

                var tmpTensor = new SpanTensor2<System.Numerics.Vector4>(h, w);

                _FitImage(src, tmpTensor.AsSpanBitmap<System.Numerics.Vector4, TPixel>());

                SpanTensor.Copy(tmpTensor, dst, Transform);
                return;
            }

            throw new NotImplementedException();
        }

        public void CopyImage(SpanTensor3<float> src, SpanBitmap dst)
        {
            throw new NotImplementedException();
        }

        protected virtual void _FitImage(SpanBitmap src, SpanBitmap<TPixel> dst)
        {
            dst.AsTypeless().FitPixels(src);
        }

        #endregion
    }
}
