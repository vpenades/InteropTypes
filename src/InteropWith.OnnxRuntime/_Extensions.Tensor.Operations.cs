using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.ML.OnnxRuntime.Tensors
{
    public static class TensorExtensions
    {
        private static void VectorSum(ReadOnlySpan<float> left, ReadOnlySpan<float> right, Span<float> result)
        {
            var idx = 0;

            if (result.Length >= 4)
            {
                var vleft = System.Runtime.InteropServices.MemoryMarshal.Cast<float, System.Numerics.Vector4>(left);
                var vright = System.Runtime.InteropServices.MemoryMarshal.Cast<float, System.Numerics.Vector4>(right);
                var vresult = System.Runtime.InteropServices.MemoryMarshal.Cast<float, System.Numerics.Vector4>(result);

                for(int i=0; i < vresult.Length; ++i)
                {
                    vresult[i] = vleft[i] + vright[i];
                }

                idx = result.Length & ~3;
            }

            for (int i = idx; i < result.Length; ++i)
            {
                result[i] = left[i] + right[i];
            }
        }

        public static void Sum(DenseTensor<float> left, DenseTensor<float> right, ref DenseTensor<float> result)
        {
            if (!left.Dimensions.SequenceEqual(right.Dimensions)) throw new ArgumentException("incompatible dimensions", nameof(right));

            if (result.Dimensions.SequenceEqual(left.Dimensions)) result = null;

            if (result == null) result = new DenseTensor<float>(left.Dimensions);

            VectorSum(left.Buffer.Span, right.Buffer.Span, result.Buffer.Span);
        }

        // OrdinalMultiply, MatrixMultiply        
    }
}
