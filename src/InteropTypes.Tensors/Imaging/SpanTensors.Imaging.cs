using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors
{    
    using TRANSFORM = System.Numerics.Matrix3x2;    

    partial struct SpanTensor2<T>
    {
        public unsafe void FitPixels<TSrcPixel>(SpanTensor2<TSrcPixel> source, MultiplyAdd mad, bool useBilinear)
            where TSrcPixel : unmanaged
        {
            var w = (float)source.BitmapSize.Width / (float)this.BitmapSize.Width;
            var h = (float)source.BitmapSize.Height / (float)this.BitmapSize.Height;

            var matrix = TRANSFORM.CreateScale(w, h);

            var xform = new Imaging.BitmapTransform
            {
                Transform = matrix,
                ColorTransform = mad,
                UseBilinear = useBilinear
            };

            xform.FillPixels(this, source);
        }

        public unsafe void FillPixels<TSrcPixel>(SpanTensor2<TSrcPixel> source, in TRANSFORM srcXform, MultiplyAdd mad, bool useBilinear)
            where TSrcPixel : unmanaged
        {
            var xform = new Imaging.BitmapTransform
            {
                Transform = srcXform,
                ColorTransform = mad,
                UseBilinear = useBilinear
            };

            xform.FillPixels(this, source);
        }        
    }

    partial struct SpanTensor3<T>
    {
        public unsafe void FitPixels<TSrcPixel>(SpanTensor2<TSrcPixel> source, MultiplyAdd mad, bool useBilinear)
            where TSrcPixel : unmanaged
        {
            if (this.Dimensions[0] != 3) throw new InvalidOperationException("Dimension[0] is not 3.");

            var w = (float)source.BitmapSize.Width / (float)this.Dimensions[2];
            var h = (float)source.BitmapSize.Height / (float)this.Dimensions[1];

            var matrix = TRANSFORM.CreateScale(w, h);

            var xform = new Imaging.BitmapTransform
            {
                Transform = matrix,
                ColorTransform = mad,
                UseBilinear = useBilinear
            };

            xform.FillPixels(this, source);
        }
    }

    
}
