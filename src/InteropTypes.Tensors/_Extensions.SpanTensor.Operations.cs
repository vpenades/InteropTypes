using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors
{
    partial class TensorExtensions
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

        public static void Sum(SpanTensor2<float> left, SpanTensor2<float> right, ref SpanTensor2<float> result)
        {
            if (left.Dimensions != right.Dimensions) throw new ArgumentException(nameof(right.Dimensions));
            if (left.Dimensions != result.Dimensions) result = new SpanTensor2<float>(left.Dimensions);

            var leftSpan = left.Span;
            var rightSpan = right.Span;
            var resultSpan = result.Span;

            if (leftSpan.Overlaps(resultSpan)) throw new ArgumentException("memory overlap",nameof(result));
            if (rightSpan.Overlaps(resultSpan)) throw new ArgumentException("memory overlap", nameof(result));            

            // todo: cast to Vector4 and do a vectorial sum

            for(int i=0; i < resultSpan.Length; ++i)
            {
                resultSpan[i] = leftSpan[i] + rightSpan[i];
            }
        }
    }
}
