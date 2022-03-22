using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Numerics.Tensors;
using System.Reflection;
using System.Text;

namespace InteropTypes.Tensors
{
    partial class SpanTensor
    {
        public static SpanTensor1<T> AsSpanTensor1<T>(this IDenseTensor<T> tensor)
            where T : unmanaged
        {
            return new SpanTensor1<T>(tensor.Span, tensor.Dimensions);
        }

        public static SpanTensor2<T> AsSpanTensor2<T>(this IDenseTensor<T> tensor)
            where T : unmanaged
        {
            return new SpanTensor2<T>(tensor.Span, tensor.Dimensions);
        }

        public static SpanTensor3<T> AsSpanTensor3<T>(this IDenseTensor<T> tensor)
            where T : unmanaged
        {
            return new SpanTensor3<T>(tensor.Span, tensor.Dimensions);
        }

        public static SpanTensor4<T> AsSpanTensor4<T>(this IDenseTensor<T> tensor)
            where T : unmanaged
        {
            return new SpanTensor4<T>(tensor.Span, tensor.Dimensions);
        }

        public static SpanTensor5<T> AsSpanTensor5<T>(this IDenseTensor<T> tensor)
            where T : unmanaged
        {
            return new SpanTensor5<T>(tensor.Span, tensor.Dimensions);
        }

        public static SpanTensor1<T> AsSpanTensor1<T>(this IDenseTensor<T> tensor, int idx)
            where T : unmanaged
        {
            return new SpanTensor2<T>(tensor.Span, tensor.Dimensions)[idx];
        }

        public static SpanTensor2<T> AsSpanTensor2<T>(this IDenseTensor<T> tensor, int idx)
            where T : unmanaged
        {
            return new SpanTensor3<T>(tensor.Span, tensor.Dimensions)[idx];
        }

        public static SpanTensor3<T> AsSpanTensor3<T>(this IDenseTensor<T> tensor, int idx)
            where T : unmanaged
        {
            return new SpanTensor4<T>(tensor.Span, tensor.Dimensions)[idx];
        }

        public static SpanTensor4<T> AsSpanTensor4<T>(this IDenseTensor<T> tensor, int idx)
            where T : unmanaged
        {
            return new SpanTensor5<T>(tensor.Span, tensor.Dimensions)[idx];
        }

        public static SpanTensor5<T> AsSpanTensor5<T>(this IDenseTensor<T> tensor, int idx)
            where T : unmanaged
        {
            return new SpanTensor6<T>(tensor.Span, tensor.Dimensions)[idx];
        }

        [Obsolete]
        public static SpanTensor1<T> AsSpanTensor1<T>(this DenseTensor<T> tensor)
            where T : unmanaged, IEquatable<T>
        {
            return new SpanTensor1<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        [Obsolete]
        public static SpanTensor2<T> AsSpanTensor2<T>(this DenseTensor<T> tensor)
            where T : unmanaged, IEquatable<T>
        {
            return new SpanTensor2<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        [Obsolete]
        public static SpanTensor3<T> AsSpanTensor3<T>(this DenseTensor<T> tensor)
            where T : unmanaged, IEquatable<T>
        {
            return new SpanTensor3<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        [Obsolete]
        public static SpanTensor4<T> AsSpanTensor4<T>(this DenseTensor<T> tensor)
            where T : unmanaged, IEquatable<T>
        {
            return new SpanTensor4<T>(tensor.Buffer.Span, tensor.Dimensions);
        }              
    }    
}
