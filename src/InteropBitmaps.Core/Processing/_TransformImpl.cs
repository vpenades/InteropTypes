using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Processing
{
    using TRANSFORM = System.Numerics.Matrix3x2;

    

    static class _BitmapTransformImplementation
    {
        #region API

        public static void SetPixelsNearest<TPixel>(SpanBitmap<TPixel> dst, SpanBitmap<TPixel> src, TRANSFORM srcXform)
            where TPixel : unmanaged
        {
            void _rowProcessor(Span<TPixel> dst, SpanNearestSampler<TPixel> src, _PixelTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    if (srcIterator.MoveNext(out int sx, out int sy))
                    {
                        dst[i] = src.GetPixelZero(sx, sy);
                    }
                }
            }

            _CopyPixelsNearest(dst, src, srcXform, _rowProcessor);
        }

        public static void FillPixelsNearest<TPixel>(SpanBitmap<TPixel> dst, SpanBitmap<TPixel> src, TRANSFORM srcXform)
            where TPixel : unmanaged
        {
            void _rowProcessor(Span<TPixel> dst, SpanNearestSampler<TPixel> src, _PixelTransformIterator srcIterator)
            {
                for (int dx = 0; dx < dst.Length; ++dx)
                {
                    dst[dx] = src.GetPixelZero(srcIterator.X, srcIterator.Y);
                    srcIterator.MoveNext();
                }
            }

            _CopyPixelsNearest(dst, src, srcXform, _rowProcessor);
        }

        public static void ComposePixelsNearestFast<TDstPixel,TSrcPixel>(SpanBitmap<TDstPixel> dst, SpanBitmap<TSrcPixel> src, TRANSFORM srcXform, float opacity = 1)
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged, Pixel.IPixelCompositionQ<TSrcPixel, TDstPixel>
        {
            var opacityQ = Pixel.ToQuantizedAmount(opacity);

            void _rowProcessor(Span<TDstPixel> dst, SpanNearestSampler<TSrcPixel> src, _PixelTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    if (srcIterator.MoveNext(out int sx, out int sy))
                    {
                        var pix = src.GetPixelZero(sx, sy);

                        dst[i] = dst[i].AlphaBlendWith(pix, opacityQ);
                    }
                }
            }

            _CopyPixelsNearest(dst, src, srcXform, _rowProcessor);
        }

        public static void ComposePixelsNearestFast2<TDstPixel, TSrcPixel>(SpanBitmap<TDstPixel> dst, SpanBitmap<TSrcPixel> src, TRANSFORM srcXform, float opacity = 1)
            where TSrcPixel : unmanaged, Pixel.IComposerApplicatorQ<TDstPixel>
            where TDstPixel : unmanaged
        {
            var opacityQ = Pixel.ToQuantizedAmount(opacity);

            void _rowProcessor(Span<TDstPixel> dst, SpanNearestSampler<TSrcPixel> src, _PixelTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    if (srcIterator.MoveNext(out int sx, out int sy))
                    {
                        src.GetPixelZero(sx, sy).ApplyCompositionTo(ref dst[i], opacityQ);
                    }
                }
            }

            _CopyPixelsNearest(dst, src, srcXform, _rowProcessor);
        }


        public static void ComposePixelsNearest<TSrcPixel>(SpanBitmap dst, SpanBitmap<TSrcPixel> src, TRANSFORM srcXform, float opacity = 1)
            where TSrcPixel : unmanaged, Pixel.IConvertible<Pixel.BGRP32>
        {
            switch(dst.PixelFormat.Code)
            {
                case Pixel.BGR24.Code: ComposePixelsNearest(dst.OfType<Pixel.BGR24>(), src, srcXform, opacity); break;
                // case Pixel.RGB24.Code: ComposePixelsNearestSlow(dst.OfType<Pixel.RGB24>(), src, srcXform, opacity); break;
                default: throw new NotImplementedException();
            }
        }

        public static void ComposePixelsNearest<TDstPixel, TSrcPixel>(SpanBitmap<TDstPixel> dst, SpanBitmap<TSrcPixel> src, TRANSFORM srcXform, float opacity = 1)
            where TSrcPixel : unmanaged, Pixel.IConvertible<Pixel.BGRP32>
            where TDstPixel : unmanaged, Pixel.IPixelCompositionQ<Pixel.BGRP32, TDstPixel>
        {
            var opacityQ = Pixel.ToQuantizedAmount(opacity);

            void _rowProcessor(Span<TDstPixel> dst, SpanNearestSampler<TSrcPixel> src, _PixelTransformIterator srcIterator)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    if (srcIterator.MoveNext(out int sx, out int sy))
                    {
                        var pix = src.GetPixelZero(sx, sy).ToPixel();

                        if (pix.A == 0) continue;

                        // pix.ApplyBlendTo(ref dst[i], opacityQ);

                        dst[i] = dst[i].AlphaBlendWith(pix, opacityQ);
                    }
                }
            }

            _CopyPixelsNearest(dst, src, srcXform, _rowProcessor);
        }

        public static void ComposePixelsNearestSlow<TDstPixel, TSrcPixel>(SpanBitmap<TDstPixel> dst, SpanBitmap<TSrcPixel> src, TRANSFORM srcXform, float opacity = 1)
            where TSrcPixel : unmanaged, Pixel.IConvertible<Pixel.BGRP32>
            where TDstPixel : unmanaged, Pixel.IConvertible<Pixel.BGR24>, Pixel.IPixelFactory<Pixel.BGR24, TDstPixel>
        {
            var opacityQ = Pixel.ToQuantizedAmount(opacity);

            void _rowProcessor(Span<TDstPixel> dst, SpanNearestSampler<TSrcPixel> src, _PixelTransformIterator srcIterator)
            {
                Pixel.BGRP32 tmp0;
                Pixel.BGR24 tmp1;
                var f = default(TDstPixel);

                for (int i = 0; i < dst.Length; ++i)
                {
                    if (srcIterator.MoveNext(out int sx, out int sy))
                    {
                        tmp0 = src.GetPixelZero(sx, sy).ToPixel(); // sample src pixel
                        if (tmp0.A == 0) continue;

                        tmp1 = dst[i].ToPixel(); // sample dst pixel
                        
                        tmp0.ApplyCompositionTo(ref tmp1, opacityQ); // compose

                        dst[i] = f.From(tmp1); // set result
                    }
                }
            }

            _CopyPixelsNearest(dst, src, srcXform, _rowProcessor);
        }

        #endregion

        #region runners

        delegate void _NearestProcessRow<TSrcPixel, TDstPixel>(Span<TDstPixel> dst, SpanNearestSampler<TSrcPixel> src, _PixelTransformIterator srcIterator)
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged;

        static void _CopyPixelsNearest<TDstPixel, TSrcPixel>(SpanBitmap<TDstPixel> dst, SpanBitmap<TSrcPixel> src, TRANSFORM srcXform, _NearestProcessRow<TSrcPixel, TDstPixel> rowProcessor)
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged
        {
            _PixelTransformIterator iter;
            var iterFactory = new _PixelTransformIterator.Factory(srcXform, src.Width, src.Height);
            var destBounds = new BitmapBounds(srcXform, src.Width, src.Height).Clipped(dst.Bounds);

            var srcSampler = new SpanNearestSampler<TSrcPixel>(src);

            for (int dy = destBounds.Top; dy < destBounds.Bottom; ++dy)
            {
                iterFactory.UpdateIterator(destBounds.Left, dy, out iter);

                var dstRow = dst.UseScanlinePixels(dy).Slice(destBounds.Left, destBounds.Width);

                rowProcessor(dstRow, srcSampler, iter);
            }
        }

        #endregion

        #region nested types

        public readonly ref struct SpanNearestSampler<TPixel>
        where TPixel : unmanaged
        {
            public SpanNearestSampler(SpanBitmap<TPixel> rt)
            {
                _BInfo = rt.Info;
                _Bytes = rt.ReadableBytes;
            }

            private readonly ReadOnlySpan<byte> _Bytes;
            private readonly BitmapInfo _BInfo;

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public unsafe TPixel GetPixelZero(int x, int y) { return _BInfo.GetPixelZero<TPixel>(_Bytes, x, y); }
        }

        [Obsolete("WIP")]
        readonly ref struct SpanRenderTarget<TSrcPixel, TDstPixel>
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged, Pixel.IPixelCompositionQ<TSrcPixel, TDstPixel>
        {
            public SpanRenderTarget(SpanBitmap<TDstPixel> rt)
            {
                _Bytes = rt.WritableBytes;
                _ByteStride = rt.Info.StepByteSize;
                _Width = rt.Info.Width;
                _Height = rt.Info.Height;

                if (_ByteStride * _Height > _Bytes.Length) throw new ArgumentException("invalid", nameof(rt));
            }

            private readonly Span<Byte> _Bytes;
            private readonly int _ByteStride;
            private readonly int _Width;
            private readonly int _Height;

            public void SetPixel(int x, int y, TSrcPixel pixel, int amount)
            {
                if (y < 0 || y >= _Height) return;
                if (x < 0 || x >= _Width) return;

                var row = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TDstPixel>(_Bytes.Slice(_ByteStride * y));

                ref var rPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(row);

                rPtr = System.Runtime.CompilerServices.Unsafe.Add(ref rPtr, x);

                rPtr = rPtr.AlphaBlendWith(pixel, amount);
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
