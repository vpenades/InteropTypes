using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTensors.Core
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

            for(int i=0; i < resultSpan.Length; ++i)
            {
                resultSpan[i] = leftSpan[i] + rightSpan[i];
            }
        }
    }
}
