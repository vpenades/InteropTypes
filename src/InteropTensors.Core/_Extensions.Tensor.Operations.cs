using System;
using System.Collections.Generic;
using System.Text;

using System.Numerics.Tensors;

namespace InteropTensors
{
    partial class TensorExtensions
    {
        static int DimensionsDotProduct<T>(this Tensor<T> tensor)
        {
            int count = 1;
            foreach (var dl in tensor.Dimensions) count *= dl;
            return count;
        }

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

        public static void Sum(Tensor<float> left, Tensor<float> right, ref Tensor<float> result)
        {
            if (!left.Dimensions.SequenceEqual(right.Dimensions)) throw new ArgumentException("incompatible dimensions", nameof(right));

            if (result.Dimensions.SequenceEqual(left.Dimensions)) result = null;            

            if (left is DenseTensor<float> denseLeft && right is DenseTensor<float> denseRight && result is DenseTensor<float> denseResult)
            {
                if (result == null) result = new DenseTensor<float>(left.Dimensions);

                VectorSum(denseLeft.Buffer.Span, denseRight.Buffer.Span, denseResult.Buffer.Span);
                return;
            }

            if (result == null)
            {
                result = left is SparseTensor<float> && right is SparseTensor<float>
                    ? new SparseTensor<float>(left.Dimensions)
                    : (Tensor<float>)new DenseTensor<float>(left.Dimensions);
            }

            var count = left.DimensionsDotProduct();

            for (int i=0; i < count; ++i)
            {
                var lv = left.GetValue(i);
                var rv = right.GetValue(i);
                result.SetValue(i, lv + rv);
            }
        }

        // OrdinalMultiply, MatrixMultiply        
    }
}
