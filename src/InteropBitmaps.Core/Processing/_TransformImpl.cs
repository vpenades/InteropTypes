using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Processing
{
    static class _NearestTransformImplementation
    {
        const int BITSHIFT = 14; // 16384

        struct _PixelInterator
        {
            public _PixelInterator(float dstY, System.Numerics.Matrix3x2 srcXform, int srcW, int srcH)
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
        }
        
        public static void SetPixelsNearest<TPixel>(SpanBitmap<TPixel> dst, SpanBitmap<TPixel> src, System.Numerics.Matrix3x2 srcXform)
            where TPixel : unmanaged
        {
            _PixelInterator c;

            var srcBytes = src.ReadableBytes;
            var srcStride = src.Info.StepByteSize;

            var dstBytes = dst.WritableBytes;
            var dstStride = dst.Info.StepByteSize;

            for (int y = 0; y < dst.Height; ++y)
            {
                c = new _PixelInterator(y, srcXform, src.Width, src.Height);

                var dstRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(dstBytes.Slice(dstStride * y));

                for (int x = 0; x < dst.Height; ++x)
                {
                    if (c.MoveNext(out int cx, out int cy))
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
            _PixelInterator c;

            var srcBytes = src.ReadableBytes;
            var srcStride = src.Info.StepByteSize;

            var dstBytes = dst.WritableBytes;
            var dstStride = dst.Info.StepByteSize;

            for (int y = 0; y < dst.Height; ++y)
            {
                c = new _PixelInterator(y, srcXform, src.Width, src.Height);

                var dstRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(dstBytes.Slice(dstStride * y));

                for (int x = 0; x < dst.Height; ++x)
                {
                    var srcRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(srcBytes.Slice(srcStride * c.Y));
                    dstRow[x] = srcRow[c.X];
                    c.MoveNext();
                }
            }
        }
    }
}
