using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using MEMMARSHALL = System.Runtime.InteropServices.MemoryMarshal;

namespace InteropTypes.Tensors
{
    internal static class _ArrayUtilities
    {
        /// <summary>
        /// verifies that the memory buffers represented by <paramref name="a"/> and <paramref name="b"/> don't overlap
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <exception cref="ArgumentException"></exception>
        [System.Diagnostics.DebuggerStepThrough]
        public static void VerifyOverlap<T>(ReadOnlySpan<T> a, ReadOnlySpan<T> b)
            where T : unmanaged
        {
            if (a.Overlaps(b)) throw new ArgumentException("buffers must not overlap");
        }

        

        public static void VectorSum(ReadOnlySpan<float> left, ReadOnlySpan<float> right, Span<float> result)
        {
            System.Numerics.Tensors.TensorPrimitives.Add(left, right, result);
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
            

            float sum = 0;

            for (int i = 0; i < span.Length; ++i)
            {
                var v = (float)Math.Exp(span[i]);
                sum += v;
                span[i] = v;
            }

            for (int i = 0; i < span.Length; ++i)
            {
                span[i] /= sum;
            }
        }


        public static void ApplySoftMax(Span<Vector2> span)
        {
            // https://github.com/ShiqiYu/libfacedetection/blob/master/src/facedetectcnn.cpp#L594

            for (int i = 0; i < span.Length; ++i)
            {
                var v1 = span[i].X;
                var v2 = span[i].Y;
                var max = Math.Max(v1, v2);
                v1 -= max;
                v2 -= max;
                v1 = (float)Math.Exp(v1);
                v2 = (float)Math.Exp(v2);
                var vm = v1 + v2;

                span[i] = new Vector2(v1, v2) / vm;
            }
        }
    }
}
