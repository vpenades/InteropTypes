using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

        
        public static bool TryConvertSpan<TSrc,TDst>(ReadOnlySpan<TSrc> src, Span<TDst> dst)
            where TSrc: unmanaged
            where TDst: unmanaged
        {
            if (src.Length > dst.Length) throw new ArgumentException("destination too short.", nameof(dst));

            if (typeof(TSrc) == typeof(TDst))
            {
                MEMMARSHALL.Cast<TSrc, TDst>(src).CopyTo(dst);
                return true;                
            }

            if (typeof(TSrc) == typeof(Half) && typeof(TDst) == typeof(Single))
            {
                var srcx = MEMMARSHALL.Cast<TSrc, Half>(src);
                var dstx = MEMMARSHALL.Cast<TDst, Single>(dst);

                #if NET8_0_OR_GREATER
                System.Numerics.Tensors.TensorPrimitives.ConvertToSingle(srcx, dstx);                                
                #else
                for (int i=0; i < srcx.Length; ++i) { dstx[i] = (float)srcx[i]; }                
                #endif
                return true;
            }

            if (typeof(TSrc) == typeof(Single) && typeof(TDst) == typeof(Half))
            {
                var srcx = MEMMARSHALL.Cast<TSrc, Single>(src);
                var dstx = MEMMARSHALL.Cast<TDst, Half>(dst);

                #if NET8_0_OR_GREATER
                System.Numerics.Tensors.TensorPrimitives.ConvertToHalf(srcx, dstx);                
                #else
                for (int i=0; i < srcx.Length; ++i) { dstx[i] = (Half)srcx[i]; }                
                #endif
                return true;
            }

            if (typeof(TSrc) == typeof(Byte) && typeof(TDst) == typeof(float))
            {
                var srcx = MEMMARSHALL.Cast<TSrc, byte>(src);
                var dstx = MEMMARSHALL.Cast<TDst, float>(dst);                

                for (int i=0; i < srcx.Length; ++i) { dstx[i] = srcx[i]; }
                return true;
            }

            if (typeof(TSrc) == typeof(float) && typeof(TDst) == typeof(Byte))
            {
                var srcx = MEMMARSHALL.Cast<TSrc, float>(src);
                var dstx = MEMMARSHALL.Cast<TDst, byte>(dst);

                for (int i = 0; i < srcx.Length; ++i) { dstx[i] = (Byte)srcx[i]; }
                return true;
            }

            return false;
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
