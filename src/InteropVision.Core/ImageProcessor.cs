using System;
using System.Collections.Generic;
using System.Text;

using InteropBitmaps;

using InteropTensors;

namespace InteropVision
{
    public class ImageProcessor<TPixel> : ITensorImageProcessor<float>
            where TPixel : unmanaged
    {
        #region constructor
        public ImageProcessor() { }

        public ImageProcessor(Mad3 transform) { Transform = new Mad4(transform); }
        public ImageProcessor(Mad4 transform) { Transform = transform; }

        #endregion

        #region properties

        public Mad4 Transform { get; set; } = Mad4.Identity;

        #endregion

        #region API

        public void CopyImage(SpanBitmap src, SpanTensor3<float> dst)
        {
            if (dst.Dimensions[2] == 3)
            {
                var tmpTensor = dst.UpCast<System.Numerics.Vector3>();

                CopyImage(src, tmpTensor.AsSpanBitmap<TPixel>());

                Transform.SelectXYZ().ApplyTransformTo(tmpTensor.Span);

                return;
            }

            if (dst.Dimensions[2] == 4)
            {
                var tmpTensor = dst.UpCast<System.Numerics.Vector4>();

                CopyImage(src, tmpTensor.AsSpanBitmap<TPixel>());

                Transform.ApplyTransformTo(tmpTensor.Span);

                return;
            }

            if (dst.Dimensions[0] == 3)
            {
                var h = dst.Dimensions[1];
                var w = dst.Dimensions[2];

                var tmpTensor = new SpanTensor2<System.Numerics.Vector3>(h, w);

                CopyImage(src, tmpTensor.AsSpanBitmap<TPixel>());

                SpanTensor.Copy(tmpTensor, dst, Transform.SelectXYZ());
                return;
            }

            if (dst.Dimensions[0] == 4)
            {
                var h = dst.Dimensions[0];
                var w = dst.Dimensions[1];

                var tmpTensor = new SpanTensor2<System.Numerics.Vector4>(h, w);

                CopyImage(src, tmpTensor.AsSpanBitmap<TPixel>());

                SpanTensor.Copy(tmpTensor, dst, Transform);
                return;
            }

            throw new NotImplementedException();
        }

        public void CopyImage(SpanTensor3<float> src, SpanBitmap dst)
        {
            throw new NotImplementedException();
        }

        protected virtual void CopyImage(SpanBitmap src, SpanBitmap<TPixel> dst)
        {
            dst.AsTypeless().FitPixels(src);
        }

        #endregion
    }
}
