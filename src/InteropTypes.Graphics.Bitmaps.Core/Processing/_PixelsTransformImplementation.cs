using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    using TRANSFORM = System.Numerics.Matrix3x2;
    static class _PixelsTransformImplementation
    {
        #region API
        
        public static void OpaquePixelsDirect<TPixel>(SpanBitmap<TPixel> src, SpanBitmap<TPixel> dst, TRANSFORM srcXform)
            where TPixel : unmanaged
        {
            void _rowProcessor(Span<TPixel> dst, SpanQuantized8Sampler<TPixel, TPixel> src, _RowTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);                    
                    dst[i] = src.GetSourcePixelOrDefault(sx, sy);                    
                }
            }
            
            _ProcessRows(dst, src, srcXform, (_ProcessRowCallback8<TPixel, TPixel, TPixel>)_rowProcessor);
        }        

        public static void OpaquePixelsConvert<TSrcPixel, TDstPixel>(SpanBitmap<TSrcPixel> src, SpanBitmap<TDstPixel> dst, TRANSFORM srcXform, bool useBilinear)
           where TSrcPixel : unmanaged, Pixel.IConvertTo
           where TDstPixel : unmanaged
        {
            void _rowProcessorNearest(Span<TDstPixel> dst, SpanQuantized8Sampler<TSrcPixel, TSrcPixel> src, _RowTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);                    
                    src.GetSourcePixelOrDefault(sx, sy).CopyTo(ref dst[i]);                    
                }
            }

            void _rowProcessorBilinear(Span<TDstPixel> dst, SpanQuantized8Sampler<TSrcPixel, TSrcPixel> src, _RowTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                    src.GetSampleOrDefault(sx, sy, rx, ry).CopyTo(ref dst[i]);
                }
            }

            var p = useBilinear
                ? (_ProcessRowCallback8<TSrcPixel, TSrcPixel, TDstPixel>)_rowProcessorBilinear
                : (_ProcessRowCallback8<TSrcPixel, TSrcPixel, TDstPixel>)_rowProcessorNearest;

            _ProcessRows(dst, src, srcXform, p);
        }

        public static void ComposePixels<TSrcPixel, TDstPixel>(SpanBitmap<TSrcPixel> src, SpanBitmap<TDstPixel> dst, TRANSFORM srcXform, bool useBilinear, float opacity)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
            where TDstPixel : unmanaged, Pixel.IConvertTo
        {
            int opacityQ = (int)(opacity * 256);

            void _rowProcessorNearest(Span<TDstPixel> dst, SpanQuantized8Sampler<TSrcPixel, Pixel.BGRP32> src, _RowTransformIterator srcIterator)
            {
                Pixel.BGRP32 srcPixel = default;
                Pixel.BGRP32 composer = default;

                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);

                    src.GetSourcePixelOrDefault(sx, sy).CopyTo(ref srcPixel);

                    if (srcPixel.A == 0) continue;                    

                    ref var dstI = ref dst[i];

                    dstI.CopyTo(ref composer);
                    composer.SetSourceOver(srcPixel, opacityQ);
                    composer.CopyTo(ref dstI);

                }
            }

            void _rowProcessorBilinear(Span<TDstPixel> dst, SpanQuantized8Sampler<TSrcPixel, Pixel.BGRP32> src, _RowTransformIterator srcIterator)
            {
                Pixel.BGRP32 srcPixel = default;
                Pixel.BGRP32 composer = default;

                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);

                    srcPixel = src.GetSampleOrDefault(sx, sy, rx, ry);

                    if (srcPixel.A == 0) continue;                    

                    ref var dstI = ref dst[i];

                    dstI.CopyTo(ref composer);
                    composer.SetSourceOver(srcPixel, opacityQ);
                    composer.CopyTo(ref dstI);
                }
            }

            var p = useBilinear
                ? (_ProcessRowCallback8<TSrcPixel, Pixel.BGRP32, TDstPixel>)_rowProcessorBilinear
                : (_ProcessRowCallback8<TSrcPixel, Pixel.BGRP32, TDstPixel>)_rowProcessorNearest;

            _ProcessRows(dst, src, srcXform, p);
        }

        public static void ConvertPixels<TSrcPixel, TDstPixel, TDstOp>(SpanBitmap<TSrcPixel> src, SpanBitmap<TDstPixel> dst, TRANSFORM srcXform, bool useBilinear, TDstOp dstOp)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
            where TDstPixel : unmanaged
            where TDstOp: unmanaged, Pixel.IApplyTo<TDstPixel>
        {
            void _rowProcessorNearest(Span<TDstPixel> dst, SpanQuantized8Sampler<TSrcPixel, TSrcPixel> src, _RowTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    ref var dsti = ref dst[i];

                    srcIterator.MoveNext(out int sx, out int sy);
                    src.GetSourcePixelOrDefault(sx, sy).CopyTo(ref dsti);
                    dstOp.ApplyTo(ref dsti);
                }
            }

            void _rowProcessorBilinear(Span<TDstPixel> dst, SpanQuantized8Sampler<TSrcPixel, TSrcPixel> src, _RowTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    ref var dsti = ref dst[i];

                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                    src.GetSampleOrDefault(sx, sy, rx, ry).CopyTo(ref dsti);
                    dstOp.ApplyTo(ref dsti);
                }
            }

            var p = useBilinear
                ? (_ProcessRowCallback8<TSrcPixel, TSrcPixel, TDstPixel>)_rowProcessorBilinear
                : (_ProcessRowCallback8<TSrcPixel, TSrcPixel, TDstPixel>)_rowProcessorNearest;

            _ProcessRows(dst, src, srcXform, p);
        }

        public static void PixelsConvert<TSrcPixel>(SpanBitmap<TSrcPixel> src, SpanPlanesXYZ<float> dst, PlanesTransform transform)
           where TSrcPixel : unmanaged, Pixel.IConvertTo
        {
            _ProcessPlanesCallback8<TSrcPixel, TSrcPixel, float> p = null;

            var pop = transform.PixelOp;

            void _rowProcessorNearest(Span<float> dstX, Span<float> dstY, Span<float> dstZ, SpanQuantized8Sampler<TSrcPixel, TSrcPixel> src, _RowTransformIterator srcIterator)
            {
                Pixel.RGB96F rgb = default;                

                for (int i = 0; i < dstX.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);

                    src.GetSourcePixelOrDefault(sx, sy).CopyTo(ref rgb);

                    pop.ApplyTo(ref rgb);

                    dstX[i] = rgb.R;
                    dstY[i] = rgb.G;
                    dstZ[i] = rgb.B;
                }
            }

            void _rowProcessorBilinear(Span<float> dstX, Span<float> dstY, Span<float> dstZ, SpanQuantized8Sampler<TSrcPixel, TSrcPixel> src, _RowTransformIterator srcIterator)
            {
                Pixel.RGB96F rgb = default;

                for (int i = 0; i < dstX.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);

                    src.GetSampleOrDefault(sx, sy, rx, ry).CopyTo(ref rgb);

                    pop.ApplyTo(ref rgb);

                    dstX[i] = rgb.R;
                    dstY[i] = rgb.G;
                    dstZ[i] = rgb.B;
                }
            }

            p = transform.UseBilinear
                ? (_ProcessPlanesCallback8<TSrcPixel, TSrcPixel, float>)_rowProcessorBilinear
                : (_ProcessPlanesCallback8<TSrcPixel, TSrcPixel, float>)_rowProcessorNearest;

            _ProcessPlanes(dst, src, transform.Transform, p);
        }

        #endregion

        #region rows processor

        delegate void _ProcessRowCallback8<TSrcPixel, TTmpPixel, TDstPixel>(Span<TDstPixel> dst, SpanQuantized8Sampler<TSrcPixel, TTmpPixel> src, _RowTransformIterator srcIterator)
            where TSrcPixel : unmanaged
            where TTmpPixel : unmanaged
            where TDstPixel : unmanaged;

        static void _ProcessRows<TSrcPixel, TTmpPixel, TDstPixel>(SpanBitmap<TDstPixel> dst, SpanBitmap<TSrcPixel> src, TRANSFORM srcXform, _ProcessRowCallback8<TSrcPixel, TTmpPixel, TDstPixel> rowProcessor)
            where TSrcPixel : unmanaged
            where TTmpPixel : unmanaged
            where TDstPixel : unmanaged
        {
            _RowTransformIterator iter;
            var iterFactory = new _RowTransformIterator.Factory(srcXform);
            var destBounds = new BitmapBounds(srcXform, src.Width, src.Height).Clipped(dst.Bounds);

            var srcSampler = new SpanQuantized8Sampler<TSrcPixel, TTmpPixel>(src);

            for (int dy = destBounds.Top; dy < destBounds.Bottom; ++dy)
            {
                iterFactory.UpdateIterator(destBounds.Left, dy, out iter);

                var dstRow = dst.UseScanlinePixels(dy).Slice(destBounds.Left, destBounds.Width);

                rowProcessor(dstRow, srcSampler, iter);
            }
        }

        #endregion

        #region planes processor

        delegate void _ProcessPlanesCallback8<TSrcPixel, TTmpPixel, TDstComponent>(Span<TDstComponent> dstX, Span<TDstComponent> dstY, Span<TDstComponent> dstZ, SpanQuantized8Sampler<TSrcPixel, TTmpPixel> src, _RowTransformIterator srcIterator)
            where TSrcPixel : unmanaged
            where TTmpPixel : unmanaged
            where TDstComponent : unmanaged;            

        static void _ProcessPlanes<TSrcPixel, TTmpPixel, TDstComponent>(SpanPlanesXYZ<TDstComponent> dst, SpanBitmap<TSrcPixel> src, TRANSFORM srcXform, _ProcessPlanesCallback8<TSrcPixel, TTmpPixel, TDstComponent> rowProcessor)
            where TSrcPixel : unmanaged
            where TTmpPixel : unmanaged
            where TDstComponent : unmanaged
        {
            _RowTransformIterator iter;
            var iterFactory = new _RowTransformIterator.Factory(srcXform);
            var destBounds = new BitmapBounds(srcXform, src.Width, src.Height).Clipped(dst.Bounds);

            var srcSampler = new SpanQuantized8Sampler<TSrcPixel, TTmpPixel>(src);

            var isBGR = dst.IsBGR;

            for (int dy = destBounds.Top; dy < destBounds.Bottom; ++dy)
            {
                iterFactory.UpdateIterator(destBounds.Left, dy, out iter);

                var dstRowX = dst.X.UseScanlinePixels(dy).Slice(destBounds.Left, destBounds.Width);
                var dstRowY = dst.Y.UseScanlinePixels(dy).Slice(destBounds.Left, destBounds.Width);
                var dstRowZ = dst.Z.UseScanlinePixels(dy).Slice(destBounds.Left, destBounds.Width);
                
                if (isBGR) rowProcessor(dstRowZ, dstRowY, dstRowX, srcSampler, iter);
                else rowProcessor(dstRowX, dstRowY, dstRowZ, srcSampler, iter);
            }
        }

        #endregion

        #region nested types

        /// <summary>
        /// Takes a <see cref="SpanBitmap{TPixel}"/> as source and provides an API to take samples from it.
        /// </summary>
        /// <typeparam name="TSrcPixel">The source pixel format.</typeparam>
        /// <typeparam name="TDstPixel">The pixel format of the output samples.</typeparam>
        internal readonly ref struct SpanQuantized8Sampler<TSrcPixel, TDstPixel>
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged
        {
            #region constructor

            public SpanQuantized8Sampler(SpanBitmap<TSrcPixel> rt)
            {
                _BInfo = rt.Info;
                _Bytes = rt.ReadableBytes;
                _Interpolator = Pixel.GetQuantizedInterpolator<TSrcPixel, TDstPixel>();
                // if (_Interpolator == null) throw new ArgumentException($"{typeof(Pixel.IQuantizedInterpolator<TSrcPixel,TDstPixel>).Name} not implemented.", nameof(rt));
            }

            #endregion

            #region data

            private readonly ReadOnlySpan<byte> _Bytes;
            private readonly BitmapInfo _BInfo;

            private readonly Pixel.IQuantizedInterpolator<TSrcPixel, TDstPixel> _Interpolator;

            private static readonly TSrcPixel _Default;

            #endregion

            #region API

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public readonly ref readonly TSrcPixel GetSourcePixelOrDefault(int x, int y)
            {
                return ref _BInfo.GetPixelOrDefault(_Bytes, x, y, _Default);
            }            

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public readonly TDstPixel GetSampleOrDefault(int x, int y, int rx, int ry)
            {
                var a = GetSourcePixelOrDefault(x, y);
                var b = GetSourcePixelOrDefault(x + 1, y);
                var c = GetSourcePixelOrDefault(x, y + 1);
                var d = GetSourcePixelOrDefault(x + 1, y + 1);

                // _PixelTransformIterator runs at a fraction of 16384
                // and Pixel.InterpolateBilinear runs at a fraction of 1024
                // so we need to divide by 4 to convert the fractions.                

                // TDstPixel r = default;
                // r.SetInterpolatedValue(a, b, c, d, (uint)rx / 16, (uint)ry / 16);                

                return _Interpolator.InterpolateBilinear(a, b, c, d, (uint)rx / 16 , (uint)ry / 16 );
            }            

            #endregion
        }        

        /// <summary>
        /// Iterates over the pixels of a transformed row.
        /// </summary>
        internal struct _RowTransformIterator
        {
            #region factory

            public readonly struct Factory
            {
                public Factory(in TRANSFORM xform)
                {
                    TRANSFORM.Invert(xform, out _Transform);
                }

                private readonly TRANSFORM _Transform;                

                public readonly void UpdateIterator(float x, float y, out _RowTransformIterator iterator)
                {
                    iterator = new _RowTransformIterator(new System.Numerics.Vector2(x,y), _Transform);
                }

                public readonly void UpdateIterator(in System.Numerics.Vector2 dst, out _RowTransformIterator iterator)
                {
                    iterator = new _RowTransformIterator(dst, _Transform);
                }
            }

            /// <summary>
            /// creates a new interator for a given row.
            /// </summary>
            /// <param name="dst">The destination point.</param>
            /// <param name="srcXform">the transform to apply.</param>            
            private _RowTransformIterator(in System.Numerics.Vector2 dst, in TRANSFORM srcXform)
            {
                var origin = System.Numerics.Vector2.Transform(dst, srcXform);
                var delta = System.Numerics.Vector2.TransformNormal(System.Numerics.Vector2.UnitX, srcXform);

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

        #endregion
    }
}

