using System;
using System.Collections.Generic;
using System.Text;

using System.Numerics.Tensors;

namespace InteropTypes.Tensors
{
    partial class SpanTensor
    {
        public static (T min, T max) GetMinMax<T>(this Span<T> span)
        {
            var comparer = Comparer<T>.Default;

            if (span.Length == 0) return (default, default);

            T min = span[0];
            T max = span[0];

            for (int i = 1; i < span.Length; ++i)
            {
                var v = span[i];

                if (comparer.Compare(min, v) < 0) min = v;
                if (comparer.Compare(max, v) > 0) max = v;
            }

            return (min, max);
        }

        static int DimensionsDotProduct<T>(this Tensor<T> tensor)
        {
            int count = 1;
            foreach (var dl in tensor.Dimensions) count *= dl;
            return count;
        }

        public static void Transpose<T>(SpanTensor2<T> src, SpanTensor2<T> dst) where T : unmanaged, IEquatable<T>
        {
            if (src.Dimensions[0] != dst.Dimensions[1]) throw new ArgumentException(nameof(dst.Dimensions));
            if (src.Dimensions[1] != dst.Dimensions[0]) throw new ArgumentException(nameof(dst.Dimensions));

            // keep in mind that although the src and dst "objects" may be different,
            // they might be sharing the same memory.
            if (src._Buffer == dst._Buffer) throw new ArgumentException(nameof(dst._Buffer));

            for (int y = 0; y < dst.Dimensions[0]; ++y)
            {
                for (int x = 0; x < dst.Dimensions[1]; ++x)
                {
                    dst[y, x] = src[x, y];
                }
            }
        }
    }
}
