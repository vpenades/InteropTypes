using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.Tensors
{
    [System.Diagnostics.DebuggerDisplay("{X} {Y} {Z}")]
    struct _PixelXYZ24
    {
        #region data

        public Byte X;
        public Byte Y;
        public Byte Z;

        #endregion

        #region API

        const int _QLERPSHIFT = 11;
        const int _QLERPVALUE = 1 << _QLERPSHIFT;        
        const int _QLERPSHIFTSQUARED = _QLERPSHIFT * 2;
        const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static _PixelXYZ24 BiLerp(_PixelXYZ24 tl, _PixelXYZ24 tr, _PixelXYZ24 bl, _PixelXYZ24 br, uint rx, uint by)
        {
            System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
            System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

            // calculate quantized weights
            var lx = _QLERPVALUE - rx;
            var ty = _QLERPVALUE - by;
            var wtl = lx * ty; // top-left weight
            var wtr = rx * ty; // top-right weight
            var wbl = lx * by; // bottom-left weight
            var wbr = rx * by; // bottom-right weight
            System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

            // lerp
            #if NET5_0_OR_GREATER
            Unsafe.SkipInit<BGR24>(out var result);
            #else
            var result = default(_PixelXYZ24);
            #endif
            result.Z = (Byte)((tl.Z * wtl + tr.Z * wtr + bl.Z * wbl + br.Z * wbr) >> _QLERPSHIFTSQUARED);
            result.Y = (Byte)((tl.Y * wtl + tr.Y * wtr + bl.Y * wbl + br.Y * wbr) >> _QLERPSHIFTSQUARED);
            result.X = (Byte)((tl.X * wtl + tr.X * wtr + bl.X * wbl + br.X * wbr) >> _QLERPSHIFTSQUARED);
            return result;
        }

        #endregion

        #region nested types

        /// <summary>
        /// Helper structure to sample pixels from a <see cref="SpanTensor2{T}"/>
        /// </summary>
        public ref struct SpanTensor2Sampler
        {
            #region constructor

            public static SpanTensor2Sampler From(SpanTensor2<_PixelXYZ24> tensor)
            {
                return From(tensor, new MultiplyAdd(1f / 255f, 0));
            }

            public static SpanTensor2Sampler From(SpanTensor2<_PixelXYZ24> tensor, in MultiplyAdd mad)
            {
                var w = tensor.BitmapSize.Width;
                var h = tensor.BitmapSize.Height;
                var b = System.Runtime.InteropServices.MemoryMarshal.Cast<_PixelXYZ24, byte>(tensor.Span);

                return From(b, w*3, w, h, mad);
            }

            public static SpanTensor2Sampler From(ReadOnlySpan<Byte> buffer, int byteStride, int w, int h)
            {
                return From(buffer, byteStride, w, h, new MultiplyAdd(1f/255f,0));
            }

            public static SpanTensor2Sampler From(ReadOnlySpan<Byte> buffer, int byteStride, int w, int h, in MultiplyAdd mad)
            {
                return new SpanTensor2Sampler(buffer, byteStride, w, h, mad);
            }

            private SpanTensor2Sampler(ReadOnlySpan<Byte> buffer, int byteStride, int w, int h, in MultiplyAdd mad)
            {
                if (byteStride * (w - 1) + w * 3 > buffer.Length) throw new ArgumentException("buffer too small", nameof(buffer));
                if (w * 3 > byteStride) throw new ArgumentOutOfRangeException(nameof(w));

                _Bytes = buffer;
                _ByteStride = byteStride;
                _Width = w;
                _Height = h;

                var (mul, add) = mad.GetVector3();
                _Multiply = mul;
                _Add = add;
            }

            #endregion

            #region data

            private readonly ReadOnlySpan<Byte> _Bytes;
            private readonly int _ByteStride;
            private readonly int _Width;
            private readonly int _Height;            

            private readonly Vector3 _Multiply;
            private readonly Vector3 _Add;

            #endregion

            #region properties

            public int Width => _Width;
            public int Height => _Height;

            #endregion

            #region API

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref readonly _PixelXYZ24 GetPixel(int x, int y)
            {
                #if NETSTANDARD2_1_OR_GREATER
                x = Math.Clamp(x, 0, _Width - 1);
                y = Math.Clamp(y, 0, _Height - 1);
                #else
                if (x < 0) x = 0;
                else if (x >= _Width) x = _Width - 1;
                if (y < 0) y = 0;
                else if (y >= _Height) y = _Height - 1;
                #endif

                return ref System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PixelXYZ24>(_Bytes.Slice(y * _ByteStride))[x];

                /*
                System.Diagnostics.Debug.Assert(y * _ByteStride + x * 3 <= (_Bytes.Length - 3));

                ref var bytes = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(_Bytes);
                bytes = ref Unsafe.Add(ref bytes, y * _ByteStride);

                ref var pixel = ref Unsafe.As<Byte, _PixelXYZ24>(ref bytes);
                return ref Unsafe.Add(ref pixel, x);                
                */
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public _PixelXYZ24 GetSample(int x, int y, int rx, int ry)
            {
                var a = GetPixel(x, y);
                var b = GetPixel(x + 1, y);
                var c = GetPixel(x, y + 1);
                var d = GetPixel(x + 1, y + 1);

                // _PixelTransformIterator runs at a fraction of 16384
                // and Pixel.InterpolateBilinear runs at a fraction of 2048
                // so we need to divide by 4 to convert the fractions.                

                return BiLerp(a, b, c, d, (uint)rx / 8, (uint)ry / 8);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Vector3 GetVectorPixel(int x, int y)
            {
                var p = GetPixel(x, y);
                return new Vector3(p.X, p.Y, p.Z) * _Multiply + _Add;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Vector3 GetVectorSample(int x, int y, int rx, int ry)
            {
                var p = GetSample(x, y, rx, ry);
                return new System.Numerics.Vector3(p.X, p.Y, p.Z) * _Multiply + _Add;
            }

            #endregion
        }

        #endregion
    }
}
