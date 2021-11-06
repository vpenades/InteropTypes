using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTensors
{
    public static partial class SpanTensor
    {
        /// <summary>
        /// Wraps <paramref name="values"/> into a tensor of (n,2) tensor
        /// </summary>
        /// <param name="values">The array to wrap</param>
        /// <returns>A (n,2) tensor</returns>
        public static SpanTensor2<float> Wrap(params Vector2[] values)
        {
            return new SpanTensor1<Vector2>(values).DownCast<float>();
        }

        /// <summary>
        /// Wraps <paramref name="values"/> into a tensor of (n,3) tensor
        /// </summary>
        /// <param name="values">The array to wrap</param>
        /// <returns>A (n,3) tensor</returns>
        public static SpanTensor2<float> Wrap(params Vector3[] values)
        {
            return new SpanTensor1<Vector3>(values).DownCast<float>();
        }

        /// <summary>
        /// Wraps <paramref name="values"/> into a tensor of (n,4) tensor
        /// </summary>
        /// <param name="values">The array to wrap</param>
        /// <returns>A (n,4) tensor</returns>
        public static SpanTensor2<float> Wrap(params Vector4[] values)
        {
            return new SpanTensor1<Vector4>(values).DownCast<float>();
        }

        /// <summary>
        /// Wraps <paramref name="values"/> into a tensor of (n,4,4) tensor
        /// </summary>
        /// <param name="values">The array to wrap</param>
        /// <returns>A (n,4,4) tensor</returns>
        public static SpanTensor3<float> Wrap(params Matrix4x4[] values)
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

        public static Matrix4x4 ToMatrix4x4(SpanTensor2<float> src)
        {
            return src                
                .Reshaped(16)
                .Cast<Matrix4x4>()[0];
        }

        public static void Multiply(SpanTensor2<float> a, SpanTensor2<float> b, SpanTensor2<float> result)
        {
            // TODO: if we detect the result's memory is shared with a or b, we can do a stackalloc.
            if (a._Buffer == result._Buffer) throw new ArgumentException(nameof(result._Buffer));
            if (b._Buffer == result._Buffer) throw new ArgumentException(nameof(result._Buffer));

            // https://en.wikipedia.org/wiki/Matrix_multiplication_algorithm

            var rows = a.Dimensions[0];
            var cols = b.Dimensions[1];
            int rmin = Math.Min(rows, cols);

            for (int y = 0; y < rmin; ++y)
            {
                for (int x = 0; x < rmin; ++x)
                {
                    float sum = 0;

                    for (int z = 0; z < rmin; ++z)
                    {
                        sum += a[y, z] * b[z, x];
                    }

                    result[y, x] = sum;
                }
            }
        }
        

        public static void FitBitmap(SpanTensor2<Vector3> dst, InteropBitmaps.SpanBitmap src)
        {
            var dstData = System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, byte>(dst.Span);
            var dstBmp = new InteropBitmaps.SpanBitmap(dstData, dst.Dimensions[1], dst.Dimensions[0], InteropBitmaps.Pixel.VectorBGR.Format);

            if (src.PixelFormat == InteropBitmaps.Pixel.VectorBGR.Format)
            {
                dstBmp.FitPixels(src);
                return;
            }

            if (src.PixelFormat == InteropBitmaps.Pixel.BGR24.Format)
            {
                InteropBitmaps.SpanBitmap.FitPixels(src, dstBmp, (0, 1f / 255f));
                return;
            }

            if (src.PixelFormat == InteropBitmaps.Pixel.BGRA32.Format)
            {
                InteropBitmaps.SpanBitmap.FitPixels(src, dstBmp, (0, 1f / 255f));
                return;
            }

            throw new NotImplementedException();
        }

        public static void ApplyAddMultiply(SpanTensor2<Vector3> target, Vector3 add, Vector3 multiply)
        {
            for (int y = 0; y < target.Dimensions[0]; ++y)
            {
                var row = target[y].Span;
                for (int i = 0; i < row.Length; ++i)
                {
                    row[i] += add;
                    row[i] *= multiply;
                }
            }
        }

        public static void CopyTo(SpanTensor2<Vector3> src, SpanTensor2<float> dstX, SpanTensor2<float> dstY, SpanTensor2<float> dstZ)
        {
            for (int y = 0; y < src.Dimensions[0]; ++y)
            {
                var srcRow = src[y].Span;
                var dstRowX = dstX[y].Span;
                var dstRowY = dstY[y].Span;
                var dstRowZ = dstZ[y].Span;

                for (int x = 0; x < srcRow.Length; ++x)
                {
                    var val = srcRow[x];
                    dstRowX[x] = val.X;
                    dstRowY[x] = val.Y;
                    dstRowZ[x] = val.Z;
                }
            }
        }

        public static void Copy(SpanTensor2<Vector3> src, SpanTensor2<float> dstX, SpanTensor2<float> dstY, SpanTensor2<float> dstZ, Vector3 add, Vector3 mul)
        {
            for (int y = 0; y < src.Dimensions[0]; ++y)
            {
                var srcRow = src[y].Span;
                var dstRowX = dstX[y].Span;
                var dstRowY = dstY[y].Span;
                var dstRowZ = dstZ[y].Span;

                for (int x = 0; x < srcRow.Length; ++x)
                {
                    var val = srcRow[x];

                    val += add;
                    val *= mul;

                    dstRowX[x] = val.X;
                    dstRowY[x] = val.Y;
                    dstRowZ[x] = val.Z;
                }
            }
        }

        public static void Copy(SpanTensor2<float> srcX, SpanTensor2<float> srcY, SpanTensor2<float> srcZ, Vector3 mul, Vector3 add, SpanTensor2<Vector3> dst)
        {
            for (int y = 0; y < dst.Dimensions[0]; ++y)
            {
                var srcRowX = srcX[y].Span;
                var srcRowY = srcY[y].Span;
                var srcRowZ = srcZ[y].Span;

                var dstRow = dst[y].Span;                

                for (int x = 0; x < dstRow.Length; ++x)
                {
                    var val = new Vector3(srcRowX[x], srcRowY[x], srcRowZ[x]);

                    val *= mul;
                    val += add;

                    dstRow[x] = val;                    
                }
            }
        }
    }
}
