using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    using TRANSFORM = System.Numerics.Matrix3x2;

    using TOVECTORQ = Pixel.ICopyValueTo<Pixel.QVectorBGRP>;
    using FROMVECTORQ = Pixel.IValueSetter<Pixel.QVectorBGRP>;

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

        public static void _OverwritePixelsNearestDirect<TPixel>(SpanBitmap<TPixel> src, SpanBitmap<TPixel> dst, TRANSFORM srcXform)
            where TPixel : unmanaged, TOVECTORQ
        {
            void _rowProcessor(Span<TPixel> dst, SpanSampler<TPixel> src, _PixelTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    if (srcIterator.MoveNext(out int sx, out int sy))
                    {
                        dst[i] = src.GetPixelOrDefault(sx, sy);
                    }
                }
            }

            _ProcessRows(dst, src, srcXform, _rowProcessor);
        }

        public static void _OverwritePixelsBilinearDirect<TPixel>(SpanBitmap<TPixel> src, SpanBitmap<TPixel> dst, TRANSFORM srcXform)
            where TPixel : unmanaged, TOVECTORQ, FROMVECTORQ
        {
            void _rowProcessor(Span<TPixel> dst, SpanSampler<TPixel> src, _PixelTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    if (srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry))
                    {
                        dst[i] = src.GetPixelOrDefault(sx, sy, rx, ry);
                    }
                }
            }

            _ProcessRows(dst, src, srcXform, _rowProcessor);
        }

        public static void _OverwritePixelsNearestConvert<TDstPixel, TSrcPixel>(SpanBitmap<TSrcPixel> src, SpanBitmap<TDstPixel> dst, TRANSFORM srcXform)
           where TSrcPixel : unmanaged, TOVECTORQ, Pixel.ICopyValueTo<TDstPixel>
           where TDstPixel : unmanaged
        {
            void _rowProcessor(Span<TDstPixel> dst, SpanSampler<TSrcPixel> src, _PixelTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    if (srcIterator.MoveNext(out int sx, out int sy))
                    {
                        src.GetPixelOrDefault(sx, sy).CopyTo(ref dst[i]);
                    }
                }
            }

            _ProcessRows(dst, src, srcXform, _rowProcessor);
        }

        public static void _ComposePixelsNearest<TDstPixel, TSrcPixel>(SpanBitmap<TSrcPixel> src, SpanBitmap<TDstPixel> dst, TRANSFORM srcXform, float opacity = 1)
            where TSrcPixel : unmanaged, TOVECTORQ
            where TDstPixel : unmanaged, TOVECTORQ, FROMVECTORQ
        {
            uint opacityQ = Pixel.QVectorBGRP.FromFloat(opacity);

            void _rowProcessor(Span<TDstPixel> dst, SpanSampler<TSrcPixel> src, _PixelTransformIterator srcIterator)
            {
                var srcQ = new Pixel.QVectorBGRP();
                var dstQ = new Pixel.QVectorBGRP();                

                for (int i = 0; i < dst.Length; ++i)
                {
                    if (srcIterator.MoveNext(out int sx, out int sy))
                    {                        
                        src.GetPixelOrDefault(sx, sy).CopyTo(ref srcQ); // sample src

                        if (srcQ.A == 0) continue;
                        srcQ.ScaleAlpha(opacityQ);

                        ref var dstI = ref dst[i];

                        dstI.CopyTo(ref dstQ);  // get
                        dstQ.SourceOver(srcQ);  // compose
                        dstI.SetValue(dstQ);    // set
                    }
                }
            }

            _ProcessRows(dst, src, srcXform, _rowProcessor);
        }

        #endregion

        #region rows processor

        delegate void _ProcessRowCallback<TSrcPixel, TDstPixel>(Span<TDstPixel> dst, SpanSampler<TSrcPixel> src, _PixelTransformIterator srcIterator)
            where TSrcPixel : unmanaged, TOVECTORQ
            where TDstPixel : unmanaged;

        static void _ProcessRows<TDstPixel, TSrcPixel>(SpanBitmap<TDstPixel> dst, SpanBitmap<TSrcPixel> src, TRANSFORM srcXform, _ProcessRowCallback<TSrcPixel, TDstPixel> rowProcessor)
            where TSrcPixel : unmanaged, TOVECTORQ
            where TDstPixel : unmanaged
        {
            _PixelTransformIterator iter;
            var iterFactory = new _PixelTransformIterator.Factory(srcXform, src.Width, src.Height);
            var destBounds = new BitmapBounds(srcXform, src.Width, src.Height).Clipped(dst.Bounds);

            var srcSampler = new SpanSampler<TSrcPixel>(src);

            for (int dy = destBounds.Top; dy < destBounds.Bottom; ++dy)
            {
                iterFactory.UpdateIterator(destBounds.Left, dy, out iter);

                var dstRow = dst.UseScanlinePixels(dy).Slice(destBounds.Left, destBounds.Width);

                rowProcessor(dstRow, srcSampler, iter);
            }
        }        

        #endregion

        #region nested types

        public ref struct SpanSampler<TPixel>
        where TPixel : unmanaged, TOVECTORQ
        {
            public SpanSampler(SpanBitmap<TPixel> rt)
            {
                _BInfo = rt.Info;
                _Bytes = rt.ReadableBytes;

                _A = _B = _C = _D = default;
            }

            private readonly ReadOnlySpan<byte> _Bytes;
            private readonly BitmapInfo _BInfo;

            private Pixel.QVectorBGRP _A;
            private Pixel.QVectorBGRP _B;
            private Pixel.QVectorBGRP _C;
            private Pixel.QVectorBGRP _D;

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public TPixel GetPixelOrDefault(int x, int y)
            {
                return _BInfo.GetPixelOrDefault<TPixel>(_Bytes, x, y);
            }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public TPixel GetPixelOrDefault(int x, int y, int rx, int ry)
            {
                _BInfo.GetPixelOrDefault<TPixel>(_Bytes, x, y).CopyTo(ref _A);
                _BInfo.GetPixelOrDefault<TPixel>(_Bytes, x + 1, y).CopyTo(ref _B);
                _BInfo.GetPixelOrDefault<TPixel>(_Bytes, x, y + 1).CopyTo(ref _C);
                _BInfo.GetPixelOrDefault<TPixel>(_Bytes, x + 1, y + 1).CopyTo(ref _D);

                var r = Pixel.QVectorBGRP.Lerp(_A, _B, _C, _D, (uint)rx, (uint)ry);

                return _BInfo.GetPixelOrDefault<TPixel>(_Bytes, x, y);
            }
        }     
        

        struct _PixelTransformIterator
        {
            #region factory

            public readonly struct Factory
            {
                public Factory(in TRANSFORM xform, int srcW, int srcH)
                {
                    TRANSFORM.Invert(xform, out _Transform);
                    _Width = srcW;
                    _Height = srcH;
                }

                private readonly TRANSFORM _Transform;
                private readonly int _Width;
                private readonly int _Height;

                public void UpdateIterator(float x, float y, out _PixelTransformIterator iterator)
                {
                    iterator = new _PixelTransformIterator(new System.Numerics.Vector2(x,y), _Transform, _Width, _Height);
                }

                public void UpdateIterator(in System.Numerics.Vector2 dst, out _PixelTransformIterator iterator)
                {
                    iterator = new _PixelTransformIterator(dst, _Transform, _Width, _Height);
                }
            }

            /// <summary>
            /// creates a new interator for a row drawing
            /// </summary>
            /// <param name="dstY">The destination row.</param>
            /// <param name="srcXform">the transform to apply.</param>
            /// <param name="srcW">The source image width in pixels.</param>
            /// <param name="srcH">The source image height in pixels.</param>
            private _PixelTransformIterator(in System.Numerics.Vector2 dst, in TRANSFORM srcXform, int srcW, int srcH)
            {
                var origin = System.Numerics.Vector2.Transform(dst, srcXform);
                var delta = System.Numerics.Vector2.TransformNormal(System.Numerics.Vector2.UnitX, srcXform);

                origin *= 1 << BITSHIFT;
                delta *= 1 << BITSHIFT;

                _X = (int)origin.X;
                _Y = (int)origin.Y;
                _Dx = (int)delta.X;
                _Dy = (int)delta.Y;
                _W = srcW - 1;
                _H = srcH - 1;

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
            private readonly int _W;
            private readonly int _H;

            #endregion

            #region API

            public int X => Math.Max(0, Math.Min(_W, _X >> BITSHIFT));
            public int Y => Math.Max(0, Math.Min(_H, _Y >> BITSHIFT));


            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void MoveNext()
            {
                _X += _Dx;
                _Y += _Dy;
            }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void MoveNextUnbounded(out int x, out int y)
            {
                x = _X >> BITSHIFT;
                y = _Y >> BITSHIFT;

                _X += _Dx;
                _Y += _Dy;
            }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public bool MoveNext(out int x, out int y)
            {
                x = _X >> BITSHIFT;
                y = _Y >> BITSHIFT;

                _X += _Dx;
                _Y += _Dy;

                return x >= 0 & x <= _W & y >= 0 & y <= _H;
            }            

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public bool MoveNext(out int x, out int y, out int fpx, out int fpy)
            {
                x = _X >> BITSHIFT;
                y = _Y >> BITSHIFT;
                fpx = _X & BITMASK;
                fpy = _Y & BITMASK;

                _X += _Dx;
                _Y += _Dy;

                return x >= 0 & x <= _W & y >= 0 & y <= _H;
            }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public bool MoveNext(out int x, out int y, out float fpx, out float fpy)
            {
                x = _X >> BITSHIFT;
                y = _Y >> BITSHIFT;
                fpx = (_X & BITMASK) / (float)(BITMASK + 1);
                fpy = (_Y & BITMASK) / (float)(BITMASK + 1);

                _X += _Dx;
                _Y += _Dy;

                return x >= 0 & x <= _W & y >= 0 & y <= _H;
            }

            #endregion
        }

        #endregion
    }
}

