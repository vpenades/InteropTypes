using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Processing
{
    static class _BitmapTransformImplementation
    {
        #region set

        public static void SetPixelsNearest<TPixel>(SpanBitmap<TPixel> dst, SpanBitmap<TPixel> src, System.Numerics.Matrix3x2 srcXform)
            where TPixel : unmanaged
        {
            _PixelTransformIterator iter;

            var srcBytes = src.ReadableBytes;
            var srcStride = src.Info.StepByteSize;

            var dstBytes = dst.WritableBytes;
            var dstStride = dst.Info.StepByteSize;

            for (int y = 0; y < dst.Height; ++y)
            {
                iter = new _PixelTransformIterator(y, srcXform, src.Width, src.Height);

                var dstRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(dstBytes.Slice(dstStride * y));

                for (int x = 0; x < dst.Width; ++x)
                {
                    if (iter.MoveNext(out int cx, out int cy))
                    {
                        var srcRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(srcBytes.Slice(srcStride * cy));
                        dstRow[x] = srcRow[cx];
                    }
                }
            }
        }        

        public static void FillPixelsNearest<TPixel>(SpanBitmap<TPixel> dst, SpanBitmap<TPixel> src, System.Numerics.Matrix3x2 srcXform)
            where TPixel : unmanaged
        {
            _PixelTransformIterator iter;

            var srcBytes = src.ReadableBytes;
            var srcStride = src.Info.StepByteSize;

            var dstBytes = dst.WritableBytes;
            var dstStride = dst.Info.StepByteSize;

            for (int y = 0; y < dst.Height; ++y)
            {
                iter = new _PixelTransformIterator(y, srcXform, src.Width, src.Height);

                var dstRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(dstBytes.Slice(dstStride * y));

                for (int x = 0; x < dst.Width; ++x)
                {
                    var srcRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(srcBytes.Slice(srcStride * iter.Y));
                    dstRow[x] = srcRow[iter.X];
                    iter.MoveNext();
                }
            }
        }

        public static void FillPixelsLinear<TDstPixel, TSrcPixel>(SpanBitmap<TDstPixel> dst, SpanBitmap<TSrcPixel> src, System.Numerics.Matrix3x2 srcXform)
            where TSrcPixel : unmanaged, Pixel.IPixelBlendOps<TSrcPixel, TDstPixel>
            where TDstPixel : unmanaged
        {
            _PixelTransformIterator iter;

            var srcBytes = src.ReadableBytes;
            var srcStride = src.Info.StepByteSize;

            var dstBytes = dst.WritableBytes;
            var dstStride = dst.Info.StepByteSize;

            Span<TSrcPixel> tmpRow = stackalloc TSrcPixel[dst.Width];

            for (int y = 0; y < dst.Height; ++y)
            {
                var dstRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TDstPixel>(dstBytes.Slice(dstStride * y));

                // 1st pass
                iter = new _PixelTransformIterator(y - 0.25f, srcXform, src.Width, src.Height);

                for (int x = 0; x < dst.Width; ++x)
                {
                    var srcRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TSrcPixel>(srcBytes.Slice(srcStride * iter.Y));
                    tmpRow[x] = srcRow[iter.X];
                    iter.MoveNext();
                }

                // 2nd pass
                iter = new _PixelTransformIterator(y + 0.25f, srcXform, src.Width, src.Height);

                for (int x = 0; x < dst.Width; ++x)
                {
                    var srcRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TSrcPixel>(srcBytes.Slice(srcStride * iter.Y));
                    dstRow[x] = srcRow[iter.X].AverageWith(tmpRow[iter.X]);                    
                    iter.MoveNext();
                }                
            }
        }

        #endregion

        #region composition

        public static void SetPixelsNearest<TDstPixel,TSrcPixel>(SpanBitmap<TDstPixel> dst, SpanBitmap<TSrcPixel> src, System.Numerics.Matrix3x2 srcXform, float opacity = 1)
            where TSrcPixel : unmanaged, Pixel.IPixelIntegerComposition<TDstPixel>
            where TDstPixel : unmanaged            
        {
            _PixelTransformIterator iter;

            var srcBytes = src.ReadableBytes;
            var srcStride = src.Info.StepByteSize;
            var srcWidth = src.Info.RowByteSize;

            var dstBytes = dst.WritableBytes;
            var dstStride = dst.Info.StepByteSize;
            var dstWidth = dst.Info.RowByteSize;

            var integerOpacity = Math.Max(0, Math.Min(65536, (int)(opacity * 65536f)));

            for (int y = 0; y < dst.Height; ++y)
            {
                iter = new _PixelTransformIterator(y, srcXform, src.Width, src.Height);

                var dstRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TDstPixel>(dstBytes.Slice(dstStride * y, dstWidth));

                for (int x = 0; x < dst.Width; ++x)
                {
                    if (iter.MoveNext(out int cx, out int cy))
                    {
                        var srcRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TSrcPixel>(srcBytes.Slice(srcStride * cy, srcWidth));
                        dstRow[x] = srcRow[cx].AlphaBlendWith(dstRow[x], integerOpacity);
                    }
                }
            }
        }

        public static void SetPixelsBilinear<TDstPixel, TSrcPixel>(SpanBitmap<TDstPixel> dst, SpanBitmap<TSrcPixel> src, System.Numerics.Matrix3x2 srcXform, float opacity = 1)
            where TSrcPixel : unmanaged, Pixel.IPixelIntegerComposition<TDstPixel> , Pixel.IPixelBlendOps<TSrcPixel,TSrcPixel>, Pixel.IPixelConvertible<Pixel.RGBP128F>
            where TDstPixel : unmanaged, Pixel.IPixelPremulComposition<TDstPixel>
        {
            _PixelTransformIterator iter;

            var srcBytes = src.ReadableBytes;
            var srcStride = src.Info.StepByteSize;
            var srcWidth = src.Info.RowByteSize;

            var dstBytes = dst.WritableBytes;
            var dstStride = dst.Info.StepByteSize;
            var dstWidth = dst.Info.RowByteSize;            

            for (int y = 0; y < dst.Height; ++y)
            {
                iter = new _PixelTransformIterator(y, srcXform, src.Width, src.Height);

                var dstRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TDstPixel>(dstBytes.Slice(dstStride * y, dstWidth));

                for (int x = 0; x < dst.Width; ++x)
                {
                    if (iter.MoveNext(out int cx0, out int cy0, out float fx, out float fy))
                    {
                        var cx1 = Math.Min(cx0 + 1, src.Info.Width - 1);
                        var cy1 = Math.Min(cy0 + 1, src.Info.Height - 1);

                        var srcA = src.GetPixel(cx0, cy0).ToPixel().PreRGBA;
                        var srcB = src.GetPixel(cx1, cy0).ToPixel().PreRGBA;
                        var srcC = src.GetPixel(cx0, cy1).ToPixel().PreRGBA;
                        var srcD = src.GetPixel(cx1, cy1).ToPixel().PreRGBA;

                        var srcAB = System.Numerics.Vector4.Lerp(srcA, srcB, fx);
                        var srcCD = System.Numerics.Vector4.Lerp(srcC, srcD, fx);
                        var srcABCD = System.Numerics.Vector4.Lerp(srcAB, srcCD, fy);

                        dstRow[x] = dstRow[x].AlphaBlendWith(srcABCD, opacity);
                    }
                }
            }
        }

        #endregion

        #region nested types

        struct _PixelTransformIterator
        {
            const int BITSHIFT = 14; // 16384
            const int BITMASK = (1 << BITSHIFT) - 1;

            /// <summary>
            /// creates a new interator for a row drawing
            /// </summary>
            /// <param name="dstY">The destination row.</param>
            /// <param name="srcXform">the transform to apply.</param>
            /// <param name="srcW">The source image width in pixels.</param>
            /// <param name="srcH">The source image height in pixels.</param>
            public _PixelTransformIterator(float dstY, System.Numerics.Matrix3x2 srcXform, int srcW, int srcH)
            {
                var origin = System.Numerics.Vector2.Transform(new System.Numerics.Vector2(0, dstY), srcXform);
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

            private int _X;
            private int _Y;

            private readonly int _Dx;
            private readonly int _Dy;
            private readonly int _W;
            private readonly int _H;

            public int X => Math.Max(0, Math.Min(_W, _X >> BITSHIFT));
            public int Y => Math.Max(0, Math.Min(_H, _Y >> BITSHIFT));

            public void MoveNext()
            {
                _X += _Dx;
                _Y += _Dy;
            }

            public bool MoveNext(out int x, out int y)
            {
                x = _X >> BITSHIFT;
                y = _Y >> BITSHIFT;

                _X += _Dx;
                _Y += _Dy;

                return x >= 0 & x <= _W & y >= 0 & y <= _H;
            }

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
        }

        #endregion
    }
}
