using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors
{    
    using TRANSFORM = System.Numerics.Matrix3x2;    

    partial struct SpanTensor2<T>
    {
        public void FitPixels<TSrcPixel>(Imaging.BitmapSampler<TSrcPixel> source, Imaging.BitmapTransform xform, Imaging.ColorEncoding targetEncoding = Imaging.ColorEncoding.Undefined)
            where TSrcPixel:unmanaged
        {
            var ww = (float)source.Width / (float)this.BitmapSize.Width;
            var hh = (float)source.Height / (float)this.BitmapSize.Height;
            xform.Transform *= TRANSFORM.CreateScale(ww, hh);

            xform.FillPixels(this, source, targetEncoding);
        }        

        public void FillPixels<TSrcPixel>(Imaging.BitmapSampler<TSrcPixel> source, Imaging.BitmapTransform xform, Imaging.ColorEncoding targetEncoding = Imaging.ColorEncoding.Undefined)
            where TSrcPixel : unmanaged
        {
            xform.FillPixels(this, source, targetEncoding);
        }              
    }

    partial struct SpanTensor3<T>
    {
        public void FitPixels<TSrcPixel>(Imaging.BitmapSampler<TSrcPixel> source, Imaging.BitmapTransform xform, Imaging.ColorEncoding targetEncoding = Imaging.ColorEncoding.Undefined)
            where TSrcPixel:unmanaged
        {
            if (this.Dimensions[0] != 3) throw new InvalidOperationException("Dimension[0] is not 3.");

            var ww = (float)source.Width / (float)this.Dimensions[2];
            var hh = (float)source.Height / (float)this.Dimensions[1];
            xform.Transform *= TRANSFORM.CreateScale(ww, hh);

            xform.FillPixels(this[0], this[1], this[2], source, targetEncoding);
        }        
    }

    
}
