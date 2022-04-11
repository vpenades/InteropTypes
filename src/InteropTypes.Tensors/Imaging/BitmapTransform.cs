using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors.Imaging
{
    using TRANSFORM = System.Numerics.Matrix3x2;

    using SAMPLERXYZ24 = Imaging._Sampler2D<_PixelXYZ24>;
    using SAMPLERXYZ96F = Imaging._Sampler2D<System.Numerics.Vector3>;

    using TENSORXYZ24 = SpanTensor2<_PixelXYZ24>;
    using TENSORXYZ96F = SpanTensor2<System.Numerics.Vector3>;

    using TARGET32F = Span<float>;

    using TARGETXYZ24 = Span<_PixelXYZ24>;
    using TARGETXYZ96F = Span<System.Numerics.Vector3>;

    using SAMPLERITERATOR = Imaging._RowTransformIterator;

    public struct BitmapTransform
    {
        #region data

        public TRANSFORM Transform;
        public bool UseBilinear;
        public MultiplyAdd ColorTransform;

        // TODO: bool SwapComponents; // swap RGB with BGR

        #endregion

        #region API

        public unsafe void FillPixels<TSrcPixel, TDstComp>(SpanTensor3<TDstComp> target, SpanTensor2<TSrcPixel> source)
            where TSrcPixel : unmanaged
            where TDstComp : unmanaged
        {
            if (target.Dimensions[0] != 3) throw new InvalidOperationException("Dimension[0] is not 3.");

            var data = System.Runtime.InteropServices.MemoryMarshal.Cast<TSrcPixel, Byte>(source.Span);
            var w = source.BitmapSize.Width;
            var h = source.BitmapSize.Height;
            var stride = w * sizeof(TSrcPixel);            

            if (sizeof(TSrcPixel) == 3)
            {
                FillPixelsXYZ24(target[0], target[1], target[2], data, stride, w, h);
                return;
            }

            _ThrowUnsupported(typeof(TSrcPixel));
        }

        public unsafe void FillPixels<TSrcPixel, TDstPixel>(SpanTensor2<TDstPixel> target, SpanTensor2<TSrcPixel> source)
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged
        {
            var data = System.Runtime.InteropServices.MemoryMarshal.Cast<TSrcPixel, Byte>(source.Span);
            var w = source.BitmapSize.Width;
            var h = source.BitmapSize.Height;
            var stride = w * sizeof(TSrcPixel);

            if (typeof(TSrcPixel) == typeof(System.Numerics.Vector3))
            {
                FillPixelsXYZ96F(target, data, stride, w, h);
                return;
            }

            if (sizeof(TSrcPixel) == 3)
            {
                FillPixelsXYZ24(target, data, stride, w, h);
                return;
            }

            _ThrowUnsupported(typeof(TSrcPixel));
        }

        public unsafe void FillPixelsXYZ24<TPixel>(SpanTensor2<TPixel> target, ReadOnlySpan<Byte> srcData, int srcByteStride, int srcPixelWidth, int srcPixelHeight)
            where TPixel : unmanaged
        {
            // TODO: if Transform is identity, do a plain copy

            var sampler = SAMPLERXYZ24.From(srcData, srcByteStride, srcPixelWidth, srcPixelHeight, this.ColorTransform);

            if (typeof(TPixel) == typeof(System.Numerics.Vector3))
            {
                var dstXYZ = target.Cast<System.Numerics.Vector3>();
                _TransferPixels(sampler, dstXYZ);
                return;
            }

            if (sizeof(TPixel) == 3)
            {
                var dstXYZ = target.Cast<_PixelXYZ24>();
                _TransferPixels(sampler, dstXYZ);
                return;
            }

            _ThrowUnsupported(typeof(TPixel));
        }

        public unsafe void FillPixelsXYZ24<TComponent>(SpanTensor2<TComponent> targetX, SpanTensor2<TComponent> targetY, SpanTensor2<TComponent> targetZ, ReadOnlySpan<Byte> srcData, int srcByteStride, int srcPixelWidth, int srcPixelHeight)
            where TComponent : unmanaged
        {
            // TODO: if Transform is identity, do a plain copy

            var sampler = SAMPLERXYZ24.From(srcData, srcByteStride, srcPixelWidth, srcPixelHeight, this.ColorTransform);

            if (typeof(TComponent) == typeof(float))
            {
                var dstX = targetX.Cast<float>();
                var dstY = targetY.Cast<float>();
                var dstZ = targetZ.Cast<float>();
                _TransferPixels(sampler, dstX, dstY, dstZ);
                return;
            }

            _ThrowUnsupported(typeof(TComponent));
        }

        public unsafe void FillPixelsXYZ96F<TPixel>(SpanTensor2<TPixel> target, ReadOnlySpan<Byte> srcData, int srcByteStride, int srcPixelWidth, int srcPixelHeight)
            where TPixel : unmanaged
        {
            var sampler = SAMPLERXYZ96F.From(srcData, srcByteStride, srcPixelWidth, srcPixelHeight, this.ColorTransform);

            if (typeof(TPixel) == typeof(System.Numerics.Vector3))
            {
                var dstXYZ = target.Cast<System.Numerics.Vector3>();
                _TransferPixels(sampler, dstXYZ);
                return;
            }

            _ThrowUnsupported(typeof(TPixel));
        }

        static void _ThrowUnsupported(Type type)
        {
            throw new NotImplementedException(type.Name);
        }

        #endregion

        #region API - Core

        internal void _TransferPixels(SAMPLERXYZ24 srcSampler, TENSORXYZ24 dst)
        {
            SAMPLERITERATOR iter;

            for (int dy = 0; dy < dst.BitmapSize.Height; ++dy)
            {
                iter = new SAMPLERITERATOR(0, dy, this.Transform);

                var dstRow = dst[dy].Span.Slice(0, dst.BitmapSize.Width);

                if (this.UseBilinear) _rowProcessorBilinear(dstRow, srcSampler, iter);
                else _rowProcessorNearest(dstRow, srcSampler, iter);
            }
        }

        internal void _TransferPixels(SAMPLERXYZ24 srcSampler, TENSORXYZ96F dst)
        {
            SAMPLERITERATOR iter;

            for (int dy = 0; dy < dst.BitmapSize.Height; ++dy)
            {
                iter = new SAMPLERITERATOR(0, dy, this.Transform);

                var dstRow = dst[dy].Span.Slice(0, dst.BitmapSize.Width);

                if (this.UseBilinear) _rowProcessorBilinear(dstRow, srcSampler, iter);
                else _rowProcessorNearest(dstRow, srcSampler, iter);
            }
        }

        internal void _TransferPixels(SAMPLERXYZ96F srcSampler, TENSORXYZ96F dst)
        {
            SAMPLERITERATOR iter;

            for (int dy = 0; dy < dst.BitmapSize.Height; ++dy)
            {
                iter = new SAMPLERITERATOR(0, dy, this.Transform);

                var dstRow = dst[dy].Span.Slice(0, dst.BitmapSize.Width);

                if (this.UseBilinear) _rowProcessorBilinear(dstRow, srcSampler, iter);
                else _rowProcessorNearest(dstRow, srcSampler, iter);
            }
        }

        internal void _TransferPixels(SAMPLERXYZ24 srcSampler, SpanTensor2<float> dstX, SpanTensor2<float> dstY, SpanTensor2<float> dstZ)
        {
            SAMPLERITERATOR iter;

            for (int dy = 0; dy < dstX.BitmapSize.Height; ++dy)
            {
                iter = new SAMPLERITERATOR(0, dy, this.Transform);

                var dstRowX = dstX[dy].Span.Slice(0, dstX.BitmapSize.Width);
                var dstRowY = dstY[dy].Span.Slice(0, dstY.BitmapSize.Width);
                var dstRowZ = dstZ[dy].Span.Slice(0, dstZ.BitmapSize.Width);

                if (this.UseBilinear) _rowProcessorBilinear(dstRowX, dstRowY, dstRowZ, srcSampler, iter);
                else _rowProcessorNearest(dstRowX, dstRowY, dstRowZ, srcSampler, iter);
            }
        }

        static void _rowProcessorNearest(TARGETXYZ24 rowDst, SAMPLERXYZ24 rowSrc, SAMPLERITERATOR srcIterator)
        {
            for (int i = 0; i < rowDst.Length; ++i)
            {
                srcIterator.MoveNext(out int sx, out int sy);
                rowDst[i] = rowSrc.GetPixel(sx, sy);
            }
        }

        static void _rowProcessorBilinear(TARGETXYZ24 rowDst, SAMPLERXYZ24 rowSrc, SAMPLERITERATOR srcIterator)
        {
            for (int i = 0; i < rowDst.Length; ++i)
            {
                srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                rowDst[i] = rowSrc.GetSample(sx, sy, rx, ry);
            }
        }

        static void _rowProcessorNearest(TARGETXYZ96F rowDst, SAMPLERXYZ24 rowSrc, SAMPLERITERATOR srcIterator)
        {
            for (int i = 0; i < rowDst.Length; ++i)
            {
                srcIterator.MoveNext(out int sx, out int sy);
                rowDst[i] = rowSrc.GetVector3Pixel(sx, sy);
            }
        }

        static void _rowProcessorBilinear(TARGETXYZ96F rowDst, SAMPLERXYZ24 rowSrc, SAMPLERITERATOR srcIterator)
        {
            for (int i = 0; i < rowDst.Length; ++i)
            {
                srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                rowDst[i] = rowSrc.GetVector3Sample(sx, sy, rx, ry);
            }
        }

        static void _rowProcessorNearest(TARGETXYZ96F rowDst, SAMPLERXYZ96F rowSrc, SAMPLERITERATOR srcIterator)
        {
            for (int i = 0; i < rowDst.Length; ++i)
            {
                srcIterator.MoveNext(out int sx, out int sy);
                rowDst[i] = rowSrc.GetVector3Pixel(sx, sy);
            }
        }

        static void _rowProcessorBilinear(TARGETXYZ96F rowDst, SAMPLERXYZ96F rowSrc, SAMPLERITERATOR srcIterator)
        {
            for (int i = 0; i < rowDst.Length; ++i)
            {
                srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                rowDst[i] = rowSrc.GetVector3Sample(sx, sy, rx, ry);
            }
        }


        static void _rowProcessorNearest(TARGET32F rowDstX, TARGET32F rowDstY, TARGET32F rowDstZ, SAMPLERXYZ24 rowSrc, SAMPLERITERATOR srcIterator)
        {
            for (int i = 0; i < rowDstX.Length; ++i)
            {
                srcIterator.MoveNext(out int sx, out int sy);
                var sample = rowSrc.GetVector3Pixel(sx, sy);
                rowDstX[i] = sample.X;
                rowDstY[i] = sample.Y;
                rowDstZ[i] = sample.Z;
            }
        }

        static void _rowProcessorBilinear(TARGET32F rowDstX, TARGET32F rowDstY, TARGET32F rowDstZ, SAMPLERXYZ24 rowSrc, SAMPLERITERATOR srcIterator)
        {
            for (int i = 0; i < rowDstX.Length; ++i)
            {
                srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                var sample = rowSrc.GetVector3Sample(sx, sy, rx, ry);
                rowDstX[i] = sample.X;
                rowDstY[i] = sample.Y;
                rowDstZ[i] = sample.Z;
            }
        }        

        #endregion
    }
}
