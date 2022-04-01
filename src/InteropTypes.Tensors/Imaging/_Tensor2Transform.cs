using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors
{
    using TRANSFORM = System.Numerics.Matrix3x2;    
    using POINT = System.Drawing.Point;
    using SIZE = System.Drawing.Size;
    using XY = System.Numerics.Vector2;

    partial struct SpanTensor2<T>
    {
        public static unsafe void FillPixels(SpanTensor2<T> target, SpanTensor2<T> source, in TRANSFORM srcXform, bool useBilinear)
        {
            if (sizeof(T) == 3)
            {
                var srcXYZ = _PixelXYZ24.SpanTensor2Sampler.From(source.Cast<_PixelXYZ24>());
                var dstXYZ = target.Cast<_PixelXYZ24>();
                _Tensor2TransformSource.TransferPixels(srcXYZ, srcXform, dstXYZ, useBilinear);
                return;
            }

            throw new NotImplementedException();
        }

        public static unsafe void FillPixels(SpanTensor2<System.Numerics.Vector3> target, SpanTensor2<T> source, in TRANSFORM srcXform, MultiplyAdd mad, bool useBilinear)
        {
            if (sizeof(T) == 3)
            {
                var srcXYZ = _PixelXYZ24.SpanTensor2Sampler.From(source.Cast<_PixelXYZ24>(), mad);
                _Tensor2TransformSource.TransferPixels(srcXYZ, srcXform, target, useBilinear);
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
            TRANSFORM.Invert(srcXform, out var boundsXform);
            var destBounds = new _BitmapBounds(boundsXform, srcSampler.Width, srcSampler.Height).Clipped(dst.BitmapSize);
            
            _RowTransformIterator iter;            

            for (int dy = destBounds.Top; dy < destBounds.Bottom; ++dy)
            {
                iter = new _RowTransformIterator(destBounds.Left, dy, srcXform);                

                var dstRow = dst[dy].Span.Slice(destBounds.Left, destBounds.Width);

                if (useBilinear) _rowProcessorBilinear(dstRow, srcSampler, iter);
                else _rowProcessorNearest(dstRow, srcSampler, iter);
            }
        }

        public static void TransferPixels(_PixelXYZ24.SpanTensor2Sampler srcSampler, TRANSFORM srcXform, SpanTensor2<_PixelXYZ24> dst, bool useBilinear)
        {
            TRANSFORM.Invert(srcXform, out var boundsXform);
            var destBounds = new _BitmapBounds(boundsXform, srcSampler.Width, srcSampler.Height).Clipped(dst.BitmapSize);            
            
            _RowTransformIterator iter;

            for (int dy = destBounds.Top; dy < destBounds.Bottom; ++dy)
            {
                iter = new _RowTransformIterator(destBounds.Left, dy, srcXform);

                var dstRow = dst[dy].Span.Slice(destBounds.Left, destBounds.Width);

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

    [System.Diagnostics.DebuggerDisplay("{X},{Y} {Width}x{Height}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    readonly struct _BitmapBounds
    {
        #region constructor        

        public _BitmapBounds(int x, int y, int w, int h)
        {
            this.X = x;
            this.Y = y;
            this.Width = Math.Max(0, w);
            this.Height = Math.Max(0, h);
        }

        public _BitmapBounds(TRANSFORM srcXform, float srcW, float srcH)
        {
            var a = srcXform.Translation;
            var b = XY.Transform(new XY(srcW, 0), srcXform);
            var c = XY.Transform(new XY(srcW, srcH), srcXform);
            var d = XY.Transform(new XY(0, srcH), srcXform);

            var min = XY.Min(XY.Min(XY.Min(a, b), c), d);
            var max = XY.Max(XY.Max(XY.Max(a, b), c), d);

            this.X = (int)min.X;
            this.Y = (int)min.Y;

            this.Width = (int)max.X + 1 - this.X;
            this.Height = (int)max.Y + 1 - this.Y;
        }

        #endregion

        #region data

        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        #endregion

        #region properties

        /// <summary>
        /// Gets the area of this object, in pixels
        /// </summary>
        public int Area => Width * Height;

        public POINT Location => new POINT(X, Y);

        public SIZE Size => new SIZE(Width, Height);

        public int Left => this.X;

        public int Top => this.Y;

        public int Right => unchecked(this.X + this.Width);

        public int Bottom => unchecked(this.Y + this.Height);

        #endregion

        #region API        

        public _BitmapBounds Clipped(in SIZE clip) { return _Clip(this, new _BitmapBounds(0, 0, clip.Width, clip.Height)); }

        public _BitmapBounds Clipped(in _BitmapBounds clip) { return _Clip(this, clip); }

        private static _BitmapBounds _Clip(in _BitmapBounds rect, in _BitmapBounds clip)
        {
            var x = rect.X;
            var y = rect.Y;
            var w = rect.Width;
            var h = rect.Height;

            if (x < clip.X) { w -= (clip.X - x); x = clip.X; }
            if (y < clip.Y) { h -= (clip.Y - y); y = clip.Y; }

            if (x + w > clip.X + clip.Width) w -= (x + w) - (clip.X + clip.Width);
            if (y + h > clip.Y + clip.Height) h -= (y + h) - (clip.Y + clip.Height);

            if (w < 0) w = 0;
            if (h < 0) h = 0;

            return new _BitmapBounds(x, y, w, h);
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
