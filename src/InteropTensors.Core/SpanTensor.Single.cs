using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using TENSOR2S = InteropTensors.SpanTensor2<float>;
using TENSOR3S = InteropTensors.SpanTensor3<float>;

using TENSOR2V3 = InteropTensors.SpanTensor2<System.Numerics.Vector3>;
using TENSOR2V4 = InteropTensors.SpanTensor2<System.Numerics.Vector4>;

using XFORM3 = InteropTensors.Mad3;
using XFORM4 = InteropTensors.Mad4;

namespace InteropTensors
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

        public static Matrix4x4 ToMatrix4x4(TENSOR2S src)
        {
            return src                
                .Reshaped(16)
                .Cast<Matrix4x4>()[0];
        }

        /// <summary>
        /// Performs an Algrebraic Matrix Multiplication
        /// </summary>
        /// <param name="a">1st matrix</param>
        /// <param name="b">2nd matrix</param>
        /// <param name="result">result</param>
        public static void Multiply(TENSOR2S a, TENSOR2S b, TENSOR2S result)
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

        public static void FitBitmap(TENSOR2V3 dst, InteropBitmaps.SpanBitmap src)
        {
            var dstData = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, byte>(dst.Span);
            var dstBmp = new InteropBitmaps.SpanBitmap(dstData, dst.Dimensions[1], dst.Dimensions[0], InteropBitmaps.Pixel.VectorBGR.Format);

            if (src.PixelFormat.IsFloating)
            {
                dstBmp.FitPixels(src);                
            }
            else
            {
                InteropBitmaps.SpanBitmap.FitPixels(src, dstBmp, (0, 1f / 255f));             
            }            
        }

        public static void ApplyAddMultiply(TENSOR2V3 target, in XFORM3 xform)
        {
            for (int y = 0; y < target.Dimensions[0]; ++y)
            {
                var row = target[y].Span;
                for (int i = 0; i < row.Length; ++i)
                {
                    row[i] = xform.Transform(row[i]); 
                }
            }
        }

        

        public static void Copy(TENSOR2V3 src, TENSOR3S dst, in XFORM3 xform)
        {
            if (dst.Dimensions[0] == 3)
            {
                Copy(src, dst[0], dst[1], dst[2], xform);
                return;
            }
            else if (dst.Dimensions[2] == 3)
            {
                Copy(src, dst.UpCast<Vector3>(), xform);
                return;
            }

            throw new NotImplementedException();
        }

        public static void Copy(TENSOR2V4 src, TENSOR3S dst, in XFORM4 xform)
        {
            if (dst.Dimensions[0] == 4)
            {
                Copy(src, dst[0], dst[1], dst[2], dst[3], xform);
                return;
            }
            else if (dst.Dimensions[2] == 4)
            {
                Copy(src, dst.UpCast<Vector4>(), xform);
                return;
            }

            throw new NotImplementedException();
        }

        public static void Copy(TENSOR2V3 src, TENSOR2S dstX, TENSOR2S dstY, TENSOR2S dstZ, in XFORM3 xform)
        {
            TensorSize2.GuardEquals(nameof(src), nameof(dstX), src.Dimensions, dstX.Dimensions);
            TensorSize2.GuardEquals(nameof(src), nameof(dstY), src.Dimensions, dstY.Dimensions);
            TensorSize2.GuardEquals(nameof(src), nameof(dstZ), src.Dimensions, dstZ.Dimensions);

            for (int y = 0; y < src.Dimensions[0]; ++y)
            {
                var srcRow = src[y].ReadOnlySpan;
                var dstRowX = dstX[y].Span;
                var dstRowY = dstY[y].Span;
                var dstRowZ = dstZ[y].Span;

                XFORM3.Transform(srcRow, dstRowX, dstRowY, dstRowZ, xform);
            }
        }

        public static void Copy(TENSOR2V4 src, TENSOR2S dstX, TENSOR2S dstY, TENSOR2S dstZ, TENSOR2S dstW, in XFORM4 xform)
        {
            TensorSize2.GuardEquals(nameof(src), nameof(dstX), src.Dimensions, dstX.Dimensions);
            TensorSize2.GuardEquals(nameof(src), nameof(dstY), src.Dimensions, dstY.Dimensions);
            TensorSize2.GuardEquals(nameof(src), nameof(dstZ), src.Dimensions, dstZ.Dimensions);
            TensorSize2.GuardEquals(nameof(src), nameof(dstW), src.Dimensions, dstW.Dimensions);

            for (int y = 0; y < src.Dimensions[0]; ++y)
            {
                var srcRow = src[y].ReadOnlySpan;
                var dstRowX = dstX[y].Span;
                var dstRowY = dstY[y].Span;
                var dstRowZ = dstZ[y].Span;
                var dstRowW = dstZ[y].Span;

                XFORM4.Transform(srcRow, dstRowX, dstRowY, dstRowZ, dstRowW, xform);
            }
        }

        public static void Copy(TENSOR3S src, TENSOR2V3 dst, in XFORM3 xform)
        {
            if (src.Dimensions[0] == 3)
            {
                Copy(src[0], src[1], src[2], dst, xform);
                return;
            }
            else if (src.Dimensions[2] == 3)
            {
                Copy(src.UpCast<Vector3>(), dst, xform);
                return;
            }

            throw new NotImplementedException();
        }

        public static void Copy(TENSOR3S src, TENSOR2V4 dst, in XFORM4 xform)
        {
            if (src.Dimensions[0] == 4)
            {
                Copy(src[0], src[1], src[2], src[3], dst, xform);
                return;
            }
            else if (src.Dimensions[2] == 4)
            {
                Copy(src.UpCast<Vector4>(), dst, xform);
                return;
            }

            throw new NotImplementedException();
        }        

        public static void Copy(TENSOR2S srcX, TENSOR2S srcY, TENSOR2S srcZ, TENSOR2V3 dst, in XFORM3 xform)
        {
            TensorSize2.GuardEquals(nameof(srcX), nameof(dst), srcX.Dimensions, dst.Dimensions);
            TensorSize2.GuardEquals(nameof(srcY), nameof(dst), srcY.Dimensions, dst.Dimensions);
            TensorSize2.GuardEquals(nameof(srcZ), nameof(dst), srcZ.Dimensions, dst.Dimensions);            

            for (int y = 0; y < dst.Dimensions[0]; ++y)
            {
                var srcRowX = srcX[y].ReadOnlySpan;
                var srcRowY = srcY[y].ReadOnlySpan;
                var srcRowZ = srcZ[y].ReadOnlySpan;

                var dstRow = dst[y].Span;

                XFORM3.Transform(srcRowX, srcRowY, srcRowZ, dstRow, xform);
            }
        }

        public static void Copy(TENSOR2S srcX, TENSOR2S srcY, TENSOR2S srcZ, TENSOR2S srcW, TENSOR2V4 dst, in XFORM4 xform)
        {
            TensorSize2.GuardEquals(nameof(srcX), nameof(dst), srcX.Dimensions, dst.Dimensions);
            TensorSize2.GuardEquals(nameof(srcY), nameof(dst), srcY.Dimensions, dst.Dimensions);
            TensorSize2.GuardEquals(nameof(srcZ), nameof(dst), srcZ.Dimensions, dst.Dimensions);
            TensorSize2.GuardEquals(nameof(srcW), nameof(dst), srcW.Dimensions, dst.Dimensions);

            for (int y = 0; y < dst.Dimensions[0]; ++y)
            {
                var srcRowX = srcX[y].ReadOnlySpan;
                var srcRowY = srcY[y].ReadOnlySpan;
                var srcRowZ = srcZ[y].ReadOnlySpan;
                var srcRowW = srcW[y].ReadOnlySpan;

                var dstRow = dst[y].Span;

                XFORM4.Transform(srcRowX, srcRowY, srcRowZ, srcRowW, dstRow, xform);
            }
        }
                
        public static void Copy(TENSOR2V3 src, TENSOR2V3 dst, in XFORM3 xform)
        {
            TensorSize2.GuardEquals(nameof(src), nameof(dst), src.Dimensions, dst.Dimensions);

            var srcSpan = src.UpCast<Vector3>().ReadOnlySpan;
            var dstSpan = dst.Span;

            XFORM3.Transform(srcSpan, dstSpan, xform);
        }

        public static void Copy(TENSOR2V4 src, TENSOR2V4 dst, in XFORM4 xform)
        {
            TensorSize2.GuardEquals(nameof(src), nameof(dst), src.Dimensions, dst.Dimensions);

            var srcSpan = src.UpCast<Vector4>().ReadOnlySpan;
            var dstSpan = dst.Span;

            XFORM4.Transform(srcSpan, dstSpan, xform);
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

            for(int i=0; i < span.Length; ++i)
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
    }
}
