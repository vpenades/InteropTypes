using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Tensors.Imaging;

namespace InteropTypes.Tensors
{
    partial struct SpanTensor2<T>
    {
        public Imaging.BitmapSampler<T> AsBitmapSampler(Imaging.ColorEncoding encoding)
        {
            return Imaging.BitmapSampler<T>.From(this, encoding);
        }

        public Imaging.TensorBitmap<TElement> AsTensorBitmap<TElement>(Imaging.ColorEncoding encoding)
            where TElement: unmanaged, IConvertible
        {
            return Imaging.TensorBitmap<TElement>.CreateFrom(this, encoding);
        }
    }

    partial struct SpanTensor3<T>
    {
        public unsafe Imaging.TensorBitmap<TElement> AsTensorBitmap<TElement>(Imaging.ColorEncoding encoding)
            where TElement : unmanaged, IConvertible
        {
            if (TensorBitmap<TElement>.ElementsAreCompatible<T>())
            {
                if (TensorBitmap<TElement>.ElementsAreBytes)
                {
                    var src = this.Cast<Byte>();
                    var tmp = new TensorBitmap<Byte>(src, encoding, new ColorRanges(0, 255));
                    if (tmp.TryGetTyped<TElement>(out var result)) return result;
                }

                if (TensorBitmap<TElement>.ElementsAreSingles)
                {
                    var src = this.Cast<float>();
                    var tmp = new TensorBitmap<float>(src, encoding, ColorRanges.Identity);
                    if (tmp.TryGetTyped<TElement>(out var result)) return result;
                }                
            }

            throw new NotImplementedException();

        }
    }
}
