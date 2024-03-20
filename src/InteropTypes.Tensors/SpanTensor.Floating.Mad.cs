using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using ROTENSOR2S = InteropTypes.Tensors.ReadOnlySpanTensor2<float>;
using ROTENSOR2V3 = InteropTypes.Tensors.ReadOnlySpanTensor2<System.Numerics.Vector3>;
using ROTENSOR2V4 = InteropTypes.Tensors.ReadOnlySpanTensor2<System.Numerics.Vector4>;
using ROTENSOR3S = InteropTypes.Tensors.ReadOnlySpanTensor3<float>;

using TENSOR2S = InteropTypes.Tensors.SpanTensor2<float>;
using TENSOR2V3 = InteropTypes.Tensors.SpanTensor2<System.Numerics.Vector3>;
using TENSOR2V4 = InteropTypes.Tensors.SpanTensor2<System.Numerics.Vector4>;
using TENSOR3S = InteropTypes.Tensors.SpanTensor3<float>;

namespace InteropTypes.Tensors
{
    partial class SpanTensor
    {
        public static void ApplyMultiplyAdd(TENSOR2V3 target, in MultiplyAdd xform)
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
        

        public static void Copy(ROTENSOR2V3 src, TENSOR3S dst, in MultiplyAdd xform)
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

        public static void Copy(ROTENSOR2V4 src, TENSOR3S dst, in MultiplyAdd xform)
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

        public static void Copy(ROTENSOR2V3 src, TENSOR2S dstX, TENSOR2S dstY, TENSOR2S dstZ, in MultiplyAdd xform)
        {
            TensorSize2.GuardEquals(nameof(src), nameof(dstX), src.Dimensions, dstX.Dimensions);
            TensorSize2.GuardEquals(nameof(src), nameof(dstY), src.Dimensions, dstY.Dimensions);
            TensorSize2.GuardEquals(nameof(src), nameof(dstZ), src.Dimensions, dstZ.Dimensions);

            for (int y = 0; y < src.Dimensions[0]; ++y)
            {
                var srcRow = src[y].Span;
                var dstRowX = dstX[y].Span;
                var dstRowY = dstY[y].Span;
                var dstRowZ = dstZ[y].Span;

                MultiplyAdd.Transform(srcRow, dstRowX, dstRowY, dstRowZ, xform);
            }
        }

        public static void Copy(ROTENSOR2V4 src, TENSOR2S dstX, TENSOR2S dstY, TENSOR2S dstZ, TENSOR2S dstW, in MultiplyAdd xform)
        {
            TensorSize2.GuardEquals(nameof(src), nameof(dstX), src.Dimensions, dstX.Dimensions);
            TensorSize2.GuardEquals(nameof(src), nameof(dstY), src.Dimensions, dstY.Dimensions);
            TensorSize2.GuardEquals(nameof(src), nameof(dstZ), src.Dimensions, dstZ.Dimensions);
            TensorSize2.GuardEquals(nameof(src), nameof(dstW), src.Dimensions, dstW.Dimensions);

            for (int y = 0; y < src.Dimensions[0]; ++y)
            {
                var srcRow = src[y].Span;
                var dstRowX = dstX[y].Span;
                var dstRowY = dstY[y].Span;
                var dstRowZ = dstZ[y].Span;
                var dstRowW = dstZ[y].Span;

                MultiplyAdd.Transform(srcRow, dstRowX, dstRowY, dstRowZ, dstRowW, xform);
            }
        }

        public static void Copy(ROTENSOR3S src, TENSOR2V3 dst, in MultiplyAdd xform)
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

        public static void Copy(ROTENSOR3S src, TENSOR2V4 dst, in MultiplyAdd xform)
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

        public static void Copy(ROTENSOR2S srcX, ROTENSOR2S srcY, ROTENSOR2S srcZ, TENSOR2V3 dst, in MultiplyAdd xform)
        {
            TensorSize2.GuardEquals(nameof(srcX), nameof(dst), srcX.Dimensions, dst.Dimensions);
            TensorSize2.GuardEquals(nameof(srcY), nameof(dst), srcY.Dimensions, dst.Dimensions);
            TensorSize2.GuardEquals(nameof(srcZ), nameof(dst), srcZ.Dimensions, dst.Dimensions);

            for (int y = 0; y < dst.Dimensions[0]; ++y)
            {
                var srcRowX = srcX[y].Span;
                var srcRowY = srcY[y].Span;
                var srcRowZ = srcZ[y].Span;

                var dstRow = dst[y].Span;

                MultiplyAdd.Transform(srcRowX, srcRowY, srcRowZ, dstRow, xform);
            }
        }

        public static void Copy(ROTENSOR2S srcX, ROTENSOR2S srcY, ROTENSOR2S srcZ, ROTENSOR2S srcW, TENSOR2V4 dst, in MultiplyAdd xform)
        {
            TensorSize2.GuardEquals(nameof(srcX), nameof(dst), srcX.Dimensions, dst.Dimensions);
            TensorSize2.GuardEquals(nameof(srcY), nameof(dst), srcY.Dimensions, dst.Dimensions);
            TensorSize2.GuardEquals(nameof(srcZ), nameof(dst), srcZ.Dimensions, dst.Dimensions);
            TensorSize2.GuardEquals(nameof(srcW), nameof(dst), srcW.Dimensions, dst.Dimensions);

            for (int y = 0; y < dst.Dimensions[0]; ++y)
            {
                var srcRowX = srcX[y].Span;
                var srcRowY = srcY[y].Span;
                var srcRowZ = srcZ[y].Span;
                var srcRowW = srcW[y].Span;

                var dstRow = dst[y].Span;

                MultiplyAdd.Transform(srcRowX, srcRowY, srcRowZ, srcRowW, dstRow, xform);
            }
        }

        public static void Copy(ROTENSOR2V3 src, TENSOR2V3 dst, in MultiplyAdd xform)
        {
            TensorSize2.GuardEquals(nameof(src), nameof(dst), src.Dimensions, dst.Dimensions);

            var srcSpan = src.UpCast<Vector3>().Span;
            var dstSpan = dst.Span;

            MultiplyAdd.Transform(srcSpan, dstSpan, xform);
        }

        public static void Copy(ROTENSOR2V4 src, TENSOR2V4 dst, in MultiplyAdd xform)
        {
            TensorSize2.GuardEquals(nameof(src), nameof(dst), src.Dimensions, dst.Dimensions);

            var srcSpan = src.UpCast<Vector4>().Span;
            var dstSpan = dst.Span;

            MultiplyAdd.Transform(srcSpan, dstSpan, xform);
        }

        public static void Copy(ReadOnlySpanTensor3<Byte> src, TENSOR2V3 dst, MultiplyAdd xform)
        {
            if (src.Dimensions[2] == 3)
            {
                TensorSize2.GuardEquals(nameof(src), nameof(dst), src.Dimensions.Head2, dst.Dimensions);

                var l0 = src.Dimensions[0];
                var l1 = src.Dimensions[1];

                for (int i0 = 0; i0 < l0; ++i0)
                {
                    for (int i1 = 0; i1 < l1; ++i1)
                    {
                        var v = new Vector3(src[i0, i1, 0], src[i0, i1, 1], src[i0, i1, 2]);
                        v = xform.Transform(v);
                        dst[i0, i1] = v;
                    }
                }

                return;
            }

            throw new NotImplementedException();
        }

        public static void Copy(ReadOnlySpanTensor3<Byte> src, TENSOR2S dstX, TENSOR2S dstY, TENSOR2S dstZ, MultiplyAdd xform)
        {
            if (src.Dimensions[2] == 3)
            {
                TensorSize2.GuardEquals(nameof(src), nameof(dstX), src.Dimensions.Head2, dstX.Dimensions);
                TensorSize2.GuardEquals(nameof(src), nameof(dstY), src.Dimensions.Head2, dstY.Dimensions);
                TensorSize2.GuardEquals(nameof(src), nameof(dstZ), src.Dimensions.Head2, dstZ.Dimensions);

                var l0 = src.Dimensions[0];
                var l1 = src.Dimensions[1];

                for (int i0 = 0; i0 < l0; ++i0)
                {
                    for (int i1 = 0; i1 < l1; ++i1)
                    {
                        var v = new Vector3(src[i0, i1, 0], src[i0, i1, 1], src[i0, i1, 2]);
                        v = xform.Transform(v);
                        dstX[i0, i1] = v.X;
                        dstY[i0, i1] = v.Y;
                        dstZ[i0, i1] = v.Z;
                    }
                }

                return;
            }

            throw new NotImplementedException();
        }

        public static void Copy(ROTENSOR2S src, SpanTensor2<Byte> dst, MultiplyAdd xform)
        {
            TensorSize2.GuardEquals(nameof(src), nameof(dst), src.Dimensions, dst.Dimensions);

            var l0 = src.Dimensions[0];
            var l1 = src.Dimensions[1];

            if (xform.IsIdentity)
            {
                for (int i0 = 0; i0 < l0; ++i0)
                {
                    for (int i1 = 0; i1 < l1; ++i1)
                    {
                        var v = src[i0, i1];
                        v = Math.Max(0, v);
                        v = Math.Min(255, v);
                        dst[i0, i1] = (Byte)v;
                    }
                }
            }
            else
            {
                for (int i0 = 0; i0 < l0; ++i0)
                {
                    for (int i1 = 0; i1 < l1; ++i1)
                    {
                        var v = src[i0, i1];
                        v = xform.Transform(v);
                        v = Math.Max(0, v);
                        v = Math.Min(255, v);
                        dst[i0, i1] = (Byte)v;
                    }
                }
            }
        }
    }
}
