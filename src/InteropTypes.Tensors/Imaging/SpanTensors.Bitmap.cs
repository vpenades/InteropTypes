using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors
{
    partial struct SpanTensor2<T>
    {
        public Imaging.BitmapSampler<T> AsSampler(Imaging.ColorEncoding encoding)
        {
            return Imaging.BitmapSampler<T>.From(this, encoding);
        }

        public Imaging.TensorBitmap<T> AsBitmap(Imaging.ColorEncoding encoding)
        {
            return new Imaging.TensorBitmap<T>(this,encoding);
        }
    }

    partial struct SpanTensor3<T>
    {
        public Imaging.TensorBitmap<T> AsBitmap(Imaging.ColorEncoding encoding)
        {
            return new Imaging.TensorBitmap<T>(this, encoding);
        }
    }
}
