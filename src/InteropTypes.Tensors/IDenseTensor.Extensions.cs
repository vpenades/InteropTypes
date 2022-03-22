using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Tensors
{
    partial class SpanTensor
    {
        /// <summary>
        /// Throws an exception if <paramref name="tensor"/> name doesn't meet <paramref name="nameConstraint"/>
        /// </summary>
        /// <typeparam name="T">An unmanaged type</typeparam>
        /// <param name="tensor">The tensor to verify.</param>
        /// <param name="nameConstraint">The name contraint to meet.</param>
        /// <returns><paramref name="tensor"/>.</returns>
        public static IDenseTensor<T> VerifyName<T>(this IDenseTensor<T> tensor, Predicate<String> nameConstraint)
            where T:unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));
            if (!nameConstraint(tensor.Name)) throw new ArgumentException($"Name mismatch, found '{tensor.Name}'.");

            return tensor;
        }

        public static IDenseTensor<T> VerifyDimensions<T>(this IDenseTensor<T> tensor, params int[] dims)
            where T : unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));
            if (tensor.Dimensions.Length != dims.Length) throw new ArgumentException($"Dimensions mismatch, expected 4 but found {tensor.Dimensions.Length}.");

            for(int i=0; i < tensor.Dimensions.Length; ++i)
            {
                var d = dims[i]; if (d <= 0) continue;
                if (d != tensor.Dimensions[i]) throw new ArgumentException($"Dimension[{i}] mismatch, expected {d} but found {tensor.Dimensions[i]}.");
            }

            return tensor;
        }

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
    }
}
