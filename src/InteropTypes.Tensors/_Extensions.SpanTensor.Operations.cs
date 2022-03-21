using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors
{
    partial class TensorExtensions
    {
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
