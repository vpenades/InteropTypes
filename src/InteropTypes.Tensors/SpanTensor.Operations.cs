using System;
using System.Collections.Generic;
using System.Text;



namespace InteropTypes.Tensors
{
    partial class SpanTensor
    {
        public static ReadOnlySpan<T> AsReadOnly<T>(this Span<T> span)
        {
            return span;
        }

        public static (T min, T max) GetMinMax<T>(this Span<T> span)
            where T : unmanaged
        {
            return span.AsReadOnly().GetMinMax();            
        }

        public static (T min, T max) GetMinMax<T>(this ReadOnlySpan<T> span)
            where T:unmanaged
        {
            if (span.IsEmpty) return (default, default);

            if (typeof(T) == typeof(float))
            {
                var spanf = System.Runtime.InteropServices.MemoryMarshal.Cast<T, float>(span);
                var minf = System.Numerics.Tensors.TensorPrimitives.Min(spanf);
                var maxf = System.Numerics.Tensors.TensorPrimitives.Max(spanf);

                var mint = System.Runtime.CompilerServices.Unsafe.As<float, T>(ref minf);
                var maxt = System.Runtime.CompilerServices.Unsafe.As<float, T>(ref maxf);

                return (mint,maxt);
            }

            var comparer = Comparer<T>.Default;            

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

        /*
        static int DimensionsDotProduct<T>(this Tensor<T> tensor)
        {
            int count = 1;
            foreach (var dl in tensor.Dimensions) count *= dl;
            return count;
        }*/

        public static void Transpose<T>(SpanTensor2<T> src, SpanTensor2<T> dst)
            where T : unmanaged
        {
            if (src.Dimensions[0] != dst.Dimensions[1]) throw new ArgumentException(nameof(dst.Dimensions));
            if (src.Dimensions[1] != dst.Dimensions[0]) throw new ArgumentException(nameof(dst.Dimensions));

            // keep in mind that although the src and dst "objects" may be different,
            // they might be sharing the same memory.
            _ArrayUtilities.VerifyOverlap<T>(src._Buffer, dst._Buffer);

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
