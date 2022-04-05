using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors
{
    using TRANSFORM = System.Numerics.Matrix3x2;    
    using XY = System.Numerics.Vector2;

    partial struct SpanTensor2<T>
    {
        public unsafe void FitPixels<TSrcPixel>(SpanTensor2<TSrcPixel> source, MultiplyAdd mad, bool useBilinear)
            where TSrcPixel : unmanaged
        {
            var w = (float)source.BitmapSize.Width / (float)this.BitmapSize.Width;
            var h = (float)source.BitmapSize.Height / (float)this.BitmapSize.Height;

            var matrix = TRANSFORM.CreateScale(w, h);

            FillPixels(source, matrix, mad, useBilinear);
        }

        public unsafe void FillPixels<TSrcPixel>(SpanTensor2<TSrcPixel> source, in TRANSFORM srcXform, MultiplyAdd mad, bool useBilinear)
            where TSrcPixel : unmanaged
        {
            if (sizeof(TSrcPixel) == 3)
            {
                var data = System.Runtime.InteropServices.MemoryMarshal.Cast<TSrcPixel,Byte>(source.Span);
                var w = source.BitmapSize.Width;
                var h = source.BitmapSize.Height;

                FillPixelsBGR24(data, w * 3, w, h, srcXform, mad, useBilinear);
                return;
            }            

            throw new NotImplementedException();
        }

        public unsafe void FillPixelsBGR24(ReadOnlySpan<Byte> source, int byteStride, int width, int height, in TRANSFORM srcXform, MultiplyAdd mad, bool useBilinear)
        {
            var srcXYZ = _PixelXYZ24.SpanTensor2Sampler.From(source, byteStride, width, height, mad);

            if (typeof(T) == typeof(System.Numerics.Vector3))
            {
                var dstXYZ = this.Cast<System.Numerics.Vector3>();
                _Tensor2TransformSource.TransferPixels(srcXYZ, srcXform, dstXYZ, useBilinear);
                return;
            }

            if (sizeof(T) == 3)
            {                
                var dstXYZ = this.Cast<_PixelXYZ24>();
                _Tensor2TransformSource.TransferPixels(srcXYZ, srcXform, dstXYZ, useBilinear);
                return;
            }            

            throw new NotImplementedException();
        }
    }

    static class _Tensor2TransformSource
    {
        #region implementations

        public static void TransferPixels(_PixelXYZ24.SpanTensor2Sampler srcSampler, in TRANSFORM srcXform, SpanTensor2<System.Numerics.Vector3> dst, bool useBilinear)
        {
            _RowTransformIterator iter;            

            for (int dy = 0; dy < dst.BitmapSize.Height; ++dy)
            {
                iter = new _RowTransformIterator(0, dy, srcXform);                

                var dstRow = dst[dy].Span.Slice(0, dst.BitmapSize.Width);

                if (useBilinear) _rowProcessorBilinear(dstRow, srcSampler, iter);
                else _rowProcessorNearest(dstRow, srcSampler, iter);
            }
        }

        public static void TransferPixels(_PixelXYZ24.SpanTensor2Sampler srcSampler, TRANSFORM srcXform, SpanTensor2<_PixelXYZ24> dst, bool useBilinear)
        {
            _RowTransformIterator iter;

            for (int dy = 0; dy < dst.BitmapSize.Height; ++dy)
            {
                iter = new _RowTransformIterator(0, dy, srcXform);

                var dstRow = dst[dy].Span.Slice(0, dst.BitmapSize.Width);

                if (useBilinear) _rowProcessorBilinear(dstRow, srcSampler, iter);
                else _rowProcessorNearest(dstRow, srcSampler, iter);
            }
        }

        static void _rowProcessorNearest(Span<_PixelXYZ24> rowDst, _PixelXYZ24.SpanTensor2Sampler rowSrc, _RowTransformIterator srcIterator)
        {
            for (int i = 0; i < rowDst.Length; ++i)
            {
                srcIterator.MoveNext(out int sx, out int sy);
                rowDst[i] = rowSrc.GetPixel(sx, sy);
            }
        }

        static void _rowProcessorBilinear(Span<_PixelXYZ24> rowDst, _PixelXYZ24.SpanTensor2Sampler rowSrc, _RowTransformIterator srcIterator)
        {
            for (int i = 0; i < rowDst.Length; ++i)
            {
                srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                rowDst[i] = rowSrc.GetSample(sx, sy, rx, ry);
            }
        }

        static void _rowProcessorNearest(Span<System.Numerics.Vector3> rowDst, _PixelXYZ24.SpanTensor2Sampler rowSrc, _RowTransformIterator srcIterator)
        {
            for (int i = 0; i < rowDst.Length; ++i)
            {
                srcIterator.MoveNext(out int sx, out int sy);
                rowDst[i] = rowSrc.GetVectorPixel(sx, sy);
            }
        }

        static void _rowProcessorBilinear(Span<System.Numerics.Vector3> rowDst, _PixelXYZ24.SpanTensor2Sampler rowSrc, _RowTransformIterator srcIterator)
        {
            for (int i = 0; i < rowDst.Length; ++i)
            {
                srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                rowDst[i] = rowSrc.GetVectorSample(sx, sy, rx, ry);
            }
        }

        #endregion
    }    

    /// <summary>
    /// Iterates over the pixels of a transformed row.
    /// </summary>
    internal struct _RowTransformIterator
    {
        #region constructor            

        public _RowTransformIterator(float x, float y, in TRANSFORM srcXform)
        {
            var dst = new XY(x, y);

            var origin = XY.Transform(dst, srcXform);
            var delta = XY.TransformNormal(XY.UnitX, srcXform);

            origin *= 1 << BITSHIFT;
            delta *= 1 << BITSHIFT;

            _X = (int)origin.X;
            _Y = (int)origin.Y;
            _Dx = (int)delta.X;
            _Dy = (int)delta.Y;

            _X += 1 << (BITSHIFT - 1);
            _Y += 1 << (BITSHIFT - 1);
        }

        #endregion

        #region data

        const int BITSHIFT = 14; // 16384
        const int BITMASK = (1 << BITSHIFT) - 1;

        private int _X;
        private int _Y;

        private readonly int _Dx;
        private readonly int _Dy;

        #endregion

        #region API            

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void MoveNext(out int x, out int y)
        {
            x = _X >> BITSHIFT;
            y = _Y >> BITSHIFT;

            _X += _Dx;
            _Y += _Dy;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void MoveNext(out int x, out int y, out int fpx, out int fpy)
        {
            x = _X >> BITSHIFT;
            y = _Y >> BITSHIFT;
            fpx = _X & BITMASK;
            fpy = _Y & BITMASK;

            _X += _Dx;
            _Y += _Dy;
        }

        #endregion
    }
}
