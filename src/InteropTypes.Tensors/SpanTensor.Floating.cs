using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using TENSOR2S = InteropTypes.Tensors.SpanTensor2<float>;
using TENSOR3S = InteropTypes.Tensors.SpanTensor3<float>;



namespace InteropTypes.Tensors
{
    public static partial class SpanTensor
    {
        

        /// <summary>
        /// Wraps <paramref name="values"/> into a tensor of (n,2) tensor
        /// </summary>
        /// <param name="values">The array to wrap</param>
        /// <returns>A (n,2) tensor</returns>
        public static TENSOR2S Wrap(params Vector2[] values)
        {
            return new SpanTensor1<Vector2>(values).DownCast<float>();
        }

        /// <summary>
        /// Wraps <paramref name="values"/> into a tensor of (n,3) tensor
        /// </summary>
        /// <param name="values">The array to wrap</param>
        /// <returns>A (n,3) tensor</returns>
        public static TENSOR2S Wrap(params Vector3[] values)
        {
            return new SpanTensor1<Vector3>(values).DownCast<float>();
        }

        /// <summary>
        /// Wraps <paramref name="values"/> into a tensor of (n,4) tensor
        /// </summary>
        /// <param name="values">The array to wrap</param>
        /// <returns>A (n,4) tensor</returns>
        public static TENSOR2S Wrap(params Vector4[] values)
        {
            return new SpanTensor1<Vector4>(values).DownCast<float>();
        }

        /// <summary>
        /// Wraps <paramref name="values"/> into a tensor of (n,4,4) tensor
        /// </summary>
        /// <param name="values">The array to wrap</param>
        /// <returns>A (n,4,4) tensor</returns>
        public static TENSOR3S Wrap(params Matrix4x4[] values)
        {
            return new SpanTensor1<Matrix4x4>(values)
                .DownCast<float>()
                .DownCast<float>()
                .Reshaped(values.Length,4,4);
        }        

        public static Matrix4x4 ToMatrix4x4(TENSOR2S src)
        {
            return src                
                .Reshaped(16)
                .Cast<Matrix4x4>()[0];
        }

        public static void Sum(TENSOR2S left, TENSOR2S right, ref TENSOR2S result)
        {
            if (left.Dimensions != right.Dimensions) throw new ArgumentException(nameof(right.Dimensions));
            if (left.Dimensions != result.Dimensions) result = new TENSOR2S(left.Dimensions);            

            System.Numerics.Tensors.TensorPrimitives.Add(left.Span, right.Span, result.Span);
        }

        /// <summary>
        /// Performs an Algrebraic Matrix Multiplication
        /// </summary>
        /// <param name="a">1st matrix</param>
        /// <param name="b">2nd matrix</param>
        /// <param name="result">result</param>
        public static void MatrixMultiply(TENSOR2S a, TENSOR2S b, TENSOR2S result)
        {
            if (result.Dimensions[0] != a.Dimensions[0]) throw new ArgumentException("dimension 0 mismatch", nameof(result));
            if (result.Dimensions[1] != b.Dimensions[1]) throw new ArgumentException("dimension 1 mismatch", nameof(result));

            // TODO: if we detect the result's memory is shared with a or b, we can do a stackalloc or new array()
            if (a.Span.Overlaps(result.Span)) throw new ArgumentException("memory overlap");
            if (b.Span.Overlaps(result.Span)) throw new ArgumentException("memory overlap");

            // https://en.wikipedia.org/wiki/Matrix_multiplication_algorithm
            
            int rmin = Math.Min(result.Dimensions[0], result.Dimensions[1]);

            for (int y = 0; y < rmin; ++y)
            {
                for (int x = 0; x < rmin; ++x)
                {
                    float sum = 0;

                    for (int i = 0; i < rmin; ++i)
                    {
                        sum += a[y, i] * b[i, x];
                    }

                    result[y, x] = sum;
                }
            }
        }                        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="span"></param>
        /// <remarks>
        /// <see href="https://en.wikipedia.org/wiki/Softmax_function"/>
        /// </remarks>
        public static void ApplySoftMax(Span<float> span)
        {
            _ArrayUtilities.ApplySoftMax(span);
        }

        
        public static void ApplySoftMax(Span<Vector2> span)
        {
            _ArrayUtilities.ApplySoftMax(span);
        }        
    }
}
