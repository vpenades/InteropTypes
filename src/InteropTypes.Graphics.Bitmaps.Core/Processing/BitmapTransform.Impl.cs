using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    using TRANSFORM = System.Numerics.Matrix3x2;

    using TOVECTORQ = Pixel.ICopyValueTo<Pixel.QVectorBGRP>;
    using FROMVECTORQ = Pixel.IValueSetter<Pixel.QVectorBGRP>;

    using TOVECTORF = Pixel.ICopyValueTo<Pixel.RGBP128F>;
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
            where TPixel : unmanaged, TOVECTORQ
        {
            void _rowProcessor(Span<TPixel> dst, SpanQuantizedSampler<TPixel> src, _RowTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);                    
                    dst[i] = src.GetPixelOrClamp(sx, sy);                    
                }
            }
            
            _ProcessRows(dst, src, srcXform, _rowProcessor);
        }        

        public static void OpaquePixelsConvert<TDstPixel, TSrcPixel>(SpanBitmap<TSrcPixel> src, SpanBitmap<TDstPixel> dst, TRANSFORM srcXform, bool useBilinear)
           where TSrcPixel : unmanaged, TOVECTORQ, Pixel.ICopyValueTo<TDstPixel>
           where TDstPixel : unmanaged, FROMVECTORQ
        {
            void _rowProcessorNearest(Span<TDstPixel> dst, SpanQuantizedSampler<TSrcPixel> src, _RowTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);                    
                    src.GetPixelOrClamp(sx, sy).CopyTo(ref dst[i]);                    
                }
            }

            void _rowProcessorBilinear(Span<TDstPixel> dst, SpanQuantizedSampler<TSrcPixel> src, _RowTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                    var sample = src.GetSampleOrClamp(sx, sy, rx, ry);
                    dst[i].SetValue(sample);
                }
            }

            var p = useBilinear
                ? (_ProcessRowCallback<TSrcPixel, TDstPixel>)_rowProcessorBilinear
                : (_ProcessRowCallback<TSrcPixel, TDstPixel>)_rowProcessorNearest;

            _ProcessRows(dst, src, srcXform, p);
        }

        public static void ComposePixels<TDstPixel, TSrcPixel>(SpanBitmap<TSrcPixel> src, SpanBitmap<TDstPixel> dst, TRANSFORM srcXform, bool useBilinear, float opacity)
            where TSrcPixel : unmanaged, TOVECTORQ
            where TDstPixel : unmanaged, TOVECTORQ, FROMVECTORQ
        {
            uint opacityQ = Pixel.QVectorBGRP.FromFloat(opacity);

            void _rowProcessorNearest(Span<TDstPixel> dst, SpanQuantizedSampler<TSrcPixel> src, _RowTransformIterator srcIterator)
            {
                var srcQ = new Pixel.QVectorBGRP();
                var dstQ = new Pixel.QVectorBGRP();                

                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);
                    
                    src.GetPixelOrClamp(sx, sy).CopyTo(ref srcQ); // sample src

                    if (srcQ.A == 0) continue;
                    srcQ.ScaleAlpha(opacityQ);

                    ref var dstI = ref dst[i];

                    dstI.CopyTo(ref dstQ);  // get
                    dstQ.SourceOver(srcQ);  // compose
                    dstI.SetValue(dstQ);    // set                    
                }
            }

            void _rowProcessorBilinear(Span<TDstPixel> dst, SpanQuantizedSampler<TSrcPixel> src, _RowTransformIterator srcIterator)
            {
                var srcQ = new Pixel.QVectorBGRP();
                var dstQ = new Pixel.QVectorBGRP();

                for (int i = 0; i < dst.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                    
                    srcQ = src.GetSampleOrClamp(sx, sy, rx, ry); // sample src

                    if (srcQ.A == 0) continue;
                    srcQ.ScaleAlpha(opacityQ);

                    ref var dstI = ref dst[i];

                    dstI.CopyTo(ref dstQ);  // get
                    dstQ.SourceOver(srcQ);  // compose
                    dstI.SetValue(dstQ);    // set                    
                }
            }

            var p = useBilinear
                ? (_ProcessRowCallback<TSrcPixel, TDstPixel>)_rowProcessorBilinear
                : (_ProcessRowCallback<TSrcPixel, TDstPixel>)_rowProcessorNearest;

            _ProcessRows(dst, src, srcXform, p);
        }

        #endregion

        #region rows processor

        delegate void _ProcessRowCallback<TSrcPixel, TDstPixel>(Span<TDstPixel> dst, SpanQuantizedSampler<TSrcPixel> src, _RowTransformIterator srcIterator)
            where TSrcPixel : unmanaged, TOVECTORQ
            where TDstPixel : unmanaged;

        static void _ProcessRows<TDstPixel, TSrcPixel>(SpanBitmap<TDstPixel> dst, SpanBitmap<TSrcPixel> src, TRANSFORM srcXform, _ProcessRowCallback<TSrcPixel, TDstPixel> rowProcessor)
            where TSrcPixel : unmanaged, TOVECTORQ
            where TDstPixel : unmanaged
        {
            _RowTransformIterator iter;
            var iterFactory = new _RowTransformIterator.Factory(srcXform);
            var destBounds = new BitmapBounds(srcXform, src.Width, src.Height).Clipped(dst.Bounds);

            var srcSampler = new SpanQuantizedSampler<TSrcPixel>(src);

            for (int dy = destBounds.Top; dy < destBounds.Bottom; ++dy)
            {
                iterFactory.UpdateIterator(destBounds.Left, dy, out iter);

                var dstRow = dst.UseScanlinePixels(dy).Slice(destBounds.Left, destBounds.Width);

                rowProcessor(dstRow, srcSampler, iter);
            }
        }        

        #endregion

        #region nested types

        internal ref struct SpanQuantizedSampler<TPixel>
            where TPixel : unmanaged, TOVECTORQ
        {
            #region constructor

            public SpanQuantizedSampler(SpanBitmap<TPixel> rt)
            {
                _BInfo = rt.Info;
                _Bytes = rt.ReadableBytes;

                _A = _B = _C = _D = default;
            }

            #endregion

            #region data

            private readonly ReadOnlySpan<byte> _Bytes;
            private readonly BitmapInfo _BInfo;

            private Pixel.QVectorBGRP _A;
            private Pixel.QVectorBGRP _B;
            private Pixel.QVectorBGRP _C;
            private Pixel.QVectorBGRP _D;

            #endregion

            #region API

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public ref readonly TPixel GetPixelOrClamp(int x, int y)
            {
                return ref _BInfo.GetPixelOrClamp<TPixel>(_Bytes, x, y);
            }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public Pixel.QVectorBGRP GetSampleOrClamp(int x, int y, int rx, int ry)
            {
                GetPixelOrClamp(x, y).CopyTo(ref _A);
                GetPixelOrClamp(x + 1, y).CopyTo(ref _B);
                GetPixelOrClamp(x, y + 1).CopyTo(ref _C);
                GetPixelOrClamp(x + 1, y + 1).CopyTo(ref _D);

                // _PixelTransformIterator runs at a fraction of 16384
                // and Pixel.QVectorBGRP runs at a fraction of 4096
                // so we need to divide by 4 to convert the fractions.

                return Pixel.QVectorBGRP.Lerp(_A, _B, _C, _D, (uint)rx / 4, (uint)ry / 4);
            }

            #endregion
        }

        internal ref struct SpanFloatingSampler<TPixel>
            where TPixel : unmanaged, TOVECTORF
        {
            #region constructor

            public SpanFloatingSampler(SpanBitmap<TPixel> rt)
            {
                _BInfo = rt.Info;
                _Bytes = rt.ReadableBytes;

                _A = _B = _C = _D = default;
            }

            #endregion

            #region data

            private readonly ReadOnlySpan<byte> _Bytes;
            private readonly BitmapInfo _BInfo;

            private Pixel.RGBP128F _A;
            private Pixel.RGBP128F _B;
            private Pixel.RGBP128F _C;
            private Pixel.RGBP128F _D;

            #endregion

            #region API

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public ref readonly TPixel GetPixelOrClamp(int x, int y)
            {
                return ref _BInfo.GetPixelOrClamp<TPixel>(_Bytes, x, y);
            }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public Pixel.RGBP128F GetSampleOrClamp(int x, int y, int rx, int ry)
            {
                GetPixelOrClamp(x, y).CopyTo(ref _A);
                GetPixelOrClamp(x + 1, y).CopyTo(ref _B);
                GetPixelOrClamp(x, y + 1).CopyTo(ref _C);
                GetPixelOrClamp(x + 1, y + 1).CopyTo(ref _D);                

                return Pixel.RGBP128F.Lerp(_A, _B, _C, _D, rx / 16384f, ry / 16384f);
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

                public void UpdateIterator(float x, float y, out _RowTransformIterator iterator)
                {
                    iterator = new _RowTransformIterator(new System.Numerics.Vector2(x,y), _Transform);
                }

                public void UpdateIterator(in System.Numerics.Vector2 dst, out _RowTransformIterator iterator)
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

