using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    using TRANSFORM = System.Numerics.Matrix3x2;    

    using TOPREMULQ = Pixel.IConvertTo;
    using FROMPREMULQ = Pixel.IValueSetter<Pixel.BGRP32>;

    using TOVECTORF = Pixel.IConvertTo;
    using FROMVECTORF = Pixel.IValueSetter<Pixel.RGBP128F>;    

    public partial struct BitmapTransform : SpanBitmap.ITransfer
    {
        public BitmapTransform(in TRANSFORM xform, float opacity = 1)
        {
            Transform = xform;
            Opacity = opacity;
            UseBilinear = false;
        }

        public BitmapTransform(in TRANSFORM xform, bool useBilinear, float opacity)
        {
            Transform = xform;
            Opacity = opacity;
            UseBilinear = useBilinear;
        }

        public TRANSFORM Transform { get; set; }
        public float Opacity { get; set; }

        public bool UseBilinear { get; set; }


        bool SpanBitmap.ITransfer.TryTransfer<TsrcPixel, TDstPixel>(SpanBitmap<TsrcPixel> source, SpanBitmap<TDstPixel> target)
        {
            if (this is SpanBitmap.ITransfer<TsrcPixel, TDstPixel> transferX)
            {
                return transferX.TryTransfer(source, target);
            }

            return false;
        }

        bool SpanBitmap.ITransfer.TryTransfer<TPixel>(SpanBitmap<TPixel> source, SpanBitmap<TPixel> target)
        {
            if (this is SpanBitmap.ITransfer<TPixel, TPixel> transferX)
            {
                return transferX.TryTransfer(source, target);
            }

            return false;
        }

        bool SpanBitmap.ITransfer.TryTransfer(SpanBitmap source, SpanBitmap target)
        {
            // TODO: try to infer the pixel size for basic overwrite transform


            return false;
        }
    }

    static class _BitmapTransformImplementation
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
                    dst[i] = src.GetPixelOrDefault(sx, sy);                    
                }
            }
            
            _ProcessRows(dst, src, srcXform, (_ProcessRowCallback8<TPixel, TPixel, TPixel>)_rowProcessor);
        }        

        public static void OpaquePixelsConvert<TSrcPixel, TDstPixel>(SpanBitmap<TSrcPixel> src, SpanBitmap<TDstPixel> dst, TRANSFORM srcXform, bool useBilinear)
           where TSrcPixel : unmanaged, Pixel.IConvertTo
           where TDstPixel : unmanaged, Pixel.IValueSetter<TSrcPixel>
        {
            void _rowProcessorNearest(Span<TDstPixel> dst, SpanQuantized8Sampler<TSrcPixel, TSrcPixel> src, _RowTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);                    
                    src.GetPixelOrDefault(sx, sy).CopyTo(ref dst[i]);                    
                }
            }

            void _rowProcessorBilinear(Span<TDstPixel> dst, SpanQuantized8Sampler<TSrcPixel, TSrcPixel> src, _RowTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                    var sample = src.GetSampleOrClamp(sx, sy, rx, ry);
                    dst[i].SetValue(sample);
                }
            }

            var p = useBilinear
                ? (_ProcessRowCallback8<TSrcPixel, TSrcPixel, TDstPixel>)_rowProcessorBilinear
                : (_ProcessRowCallback8<TSrcPixel, TSrcPixel, TDstPixel>)_rowProcessorNearest;

            _ProcessRows(dst, src, srcXform, p);
        }

        public static void ComposePixels<TSrcPixel, TDstPixel>(SpanBitmap<TSrcPixel> src, SpanBitmap<TDstPixel> dst, TRANSFORM srcXform, bool useBilinear, float opacity)
            where TSrcPixel : unmanaged, TOPREMULQ
            where TDstPixel : unmanaged, TOPREMULQ, FROMPREMULQ
        {
            int opacityQ = (int)(opacity * 256);

            void _rowProcessorNearest(Span<TDstPixel> dst, SpanQuantized8Sampler<TSrcPixel, Pixel.BGRP32> src, _RowTransformIterator srcIterator)
            {
                Pixel.BGRP32 srcPixel = default;
                Pixel.BGRP32 composer = default;

                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);

                    src.GetPixelOrDefault(sx, sy).CopyTo(ref srcPixel);

                    if (srcPixel.A == 0) continue;                    

                    ref var dstI = ref dst[i];

                    dstI.CopyTo(ref composer);
                    composer.SetSourceOver(srcPixel, opacityQ);
                    dstI.SetValue(composer);
                    
                }
            }

            void _rowProcessorBilinear(Span<TDstPixel> dst, SpanQuantized8Sampler<TSrcPixel, Pixel.BGRP32> src, _RowTransformIterator srcIterator)
            {
                Pixel.BGRP32 srcPixel = default;
                Pixel.BGRP32 composer = default;

                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);

                    srcPixel = src.GetSampleOrClamp(sx, sy, rx, ry);

                    if (srcPixel.A == 0) continue;                    

                    ref var dstI = ref dst[i];

                    dstI.CopyTo(ref composer);
                    composer.SetSourceOver(srcPixel, opacityQ);
                    dstI.SetValue(composer);
                }
            }

            var p = useBilinear
                ? (_ProcessRowCallback8<TSrcPixel, Pixel.BGRP32, TDstPixel>)_rowProcessorBilinear
                : (_ProcessRowCallback8<TSrcPixel, Pixel.BGRP32, TDstPixel>)_rowProcessorNearest;

            _ProcessRows(dst, src, srcXform, p);
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

        #region nested types

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
                if (_Interpolator == null) throw new ArgumentException($"{typeof(Pixel.IQuantizedInterpolator<TSrcPixel,TDstPixel>).Name} not implemented.", typeof(TSrcPixel).Name);
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
            public readonly ref readonly TSrcPixel GetPixelOrDefault(int x, int y)
            {
                return ref _BInfo.GetPixelOrDefault(_Bytes, x, y, _Default);
            }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public readonly TDstPixel GetSampleOrClamp(int x, int y, int rx, int ry)
            {
                var a = GetPixelOrDefault(x, y);
                var b = GetPixelOrDefault(x + 1, y);
                var c = GetPixelOrDefault(x, y + 1);
                var d = GetPixelOrDefault(x + 1, y + 1);

                // _PixelTransformIterator runs at a fraction of 16384
                // and Pixel.InterpolateBilinear runs at a fraction of 1024
                // so we need to divide by 4 to convert the fractions.

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

