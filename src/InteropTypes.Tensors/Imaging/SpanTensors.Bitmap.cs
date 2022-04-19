using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors
{
    partial struct SpanTensor2<T>
    {
        public Imaging.BitmapSampler<T> AsBitmapSampler(Imaging.ColorEncoding encoding)
        {
            return Imaging.BitmapSampler<T>.From(this, encoding);
        }

        public Imaging.TensorBitmap<T> AsTensorBitmap(Imaging.ColorEncoding encoding)
        {
            return new Imaging.TensorBitmap<T>(this, encoding);
        }
    }

    partial struct SpanTensor3<T>
    {
        public Imaging.TensorBitmap<T> AsBitmapSampler(Imaging.ColorEncoding encoding)
        {
            return new Imaging.TensorBitmap<T>(this, encoding);
        }

        public Imaging.TensorBitmap<T> AsTensorBitmap(Imaging.ColorEncoding encoding)
        {
            return new Imaging.TensorBitmap<T>(this, encoding);
        }
    }
}
