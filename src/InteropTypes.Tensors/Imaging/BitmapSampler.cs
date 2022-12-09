using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

using XY = System.Numerics.Vector2;
using TRANSFORM = System.Numerics.Matrix3x2;

using MMARSHALL = System.Runtime.InteropServices.MemoryMarshal;

namespace InteropTypes.Tensors.Imaging
{
    [System.Diagnostics.DebuggerDisplay("{Width}x{Height}x{Encoding}")]
    public readonly ref struct BitmapSampler<TPixel>
        where TPixel : unmanaged
    {
        #region constructor        

        public readonly unsafe BitmapSampler<TOther> Cast<TOther>()
            where TOther : unmanaged
        {
            if (sizeof(TPixel) != sizeof(TOther)) throw new ArgumentException("type size mismatch.", typeof(TOther).Name);
            
            return new BitmapSampler<TOther>(_Bytes, _ByteStride, _LastX, _LastY, _Encoding);
        }

        public static unsafe BitmapSampler<TPixel> From(SpanTensor2<TPixel> tensor, ColorEncoding encoding)
        {
            var w = tensor.BitmapSize.Width;
            var h = tensor.BitmapSize.Height;
            var b = MMARSHALL.Cast<TPixel, byte>(tensor.Span);
            var e = encoding;

            return new BitmapSampler<TPixel>(b, w * sizeof(TPixel), w, h, e);
        }

        public unsafe BitmapSampler(ReadOnlySpan<Byte> buffer, int byteStride, int w, int h, ColorEncoding encoding)
        {
            if (buffer.IsEmpty) throw new ArgumentNullException(nameof(buffer));
            if (w <= 0) throw new ArgumentOutOfRangeException(nameof(w));
            if (h <= 0) throw new ArgumentOutOfRangeException(nameof(h));
            if (w * sizeof(TPixel) > byteStride) throw new ArgumentOutOfRangeException(nameof(byteStride));

            if (byteStride * (h - 1) + w * sizeof(TPixel) > buffer.Length) throw new ArgumentException("buffer too small", nameof(buffer));

            _Bytes = buffer;
            _ByteStride = byteStride;
            _LastX = w -1;
            _LastY = h -1;
            _Encoding = encoding;
        }        

        #endregion

        #region data
        
        internal readonly ReadOnlySpan<Byte> _Bytes;
        internal readonly int _ByteStride;
        internal readonly int _LastX;
        internal readonly int _LastY;
        internal readonly ColorEncoding _Encoding;

        #endregion

        #region properties
        public readonly int Width => _LastX + 1;
        public readonly int Height => _LastY + 1;
        public readonly ColorEncoding Encoding => _Encoding;

        #endregion

        #region API        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Contains(in System.Drawing.Rectangle other)
        {
            if (other.Left < 0) return false;
            if (other.Top < 0) return false;
            if (other.Right > _LastX) return false;
            if (other.Bottom > _LastY) return false;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly unsafe ref readonly TPixel GetPixel(int x, int y)
        {
            #if !NETSTANDARD2_0
            x = Math.Clamp(x, 0, _LastX);
            y = Math.Clamp(y, 0, _LastY);
            #else
            if (x < 0) x = 0;
            else if (x > _LastX) x = _LastX;
            if (y < 0) y = 0;
            else if (y > _LastY) y = _LastY;
            #endif

            return ref MMARSHALL.Cast<Byte, TPixel>(_Bytes.Slice(y * _ByteStride))[x];

            // ref var ptr = ref MMARSHALL.GetReference(_Bytes);
            // ptr = ref Unsafe.Add(ref ptr, y * _ByteStride + x * sizeof(TPixel));
            // return ref Unsafe.As<Byte, TPixel>(ref ptr);
        }        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly unsafe ref readonly TDstPixel _GetPixelAs<TDstPixel>(int x, int y)
            where TDstPixel : unmanaged
        {
            if (sizeof(TPixel) < sizeof(TDstPixel)) throw new InvalidOperationException();

            #if !NETSTANDARD2_0
            x = Math.Clamp(x, 0, _LastX);
            y = Math.Clamp(y, 0, _LastY);
            #else
            if (x < 0) x = 0;
            else if (x > _LastX) x = _LastX;
            if (y < 0) y = 0;
            else if (y > _LastY) y = _LastY;
            #endif

            // return ref MMARSHALL.Cast<Byte, TDstPixel>(_Bytes.Slice(y * _ByteStride))[x];

            ref var ptr = ref MMARSHALL.GetReference(_Bytes);
            ptr = ref Unsafe.Add(ref ptr, y * _ByteStride + x * sizeof(TPixel));
            return ref Unsafe.As<Byte, TDstPixel>(ref ptr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly unsafe TPixel GetSample(int x, int y, int rx, int by, in MultiplyAdd mad)
        {
            if (sizeof(TPixel) == 3)
            {
                var a = _GetPixelAs<_PixelXYZ24>(x, y);
                var b = _GetPixelAs<_PixelXYZ24>(x + 1, y);
                var c = _GetPixelAs<_PixelXYZ24>(x, y + 1);
                var d = _GetPixelAs<_PixelXYZ24>(x + 1, y + 1);

                // _PixelTransformIterator runs at a fraction of 16384
                // and Pixel.InterpolateBilinear runs at a fraction of 1024
                // so we need to divide by 4 to convert the fractions.
                var r = _PixelXYZ24.BiLerp(a, b, c, d, (uint)rx / 16, (uint)by / 16);

                return Unsafe.As<_PixelXYZ24, TPixel>(ref r);
            }

            if (typeof(TPixel) == typeof(Vector3))
            {
                var a = _GetPixelAs<Vector3>(x, y);
                var b = _GetPixelAs<Vector3>(x + 1, y);
                var c = _GetPixelAs<Vector3>(x, y + 1);
                var d = _GetPixelAs<Vector3>(x + 1, y + 1);

                var rxf = (float)rx / (float)16384;
                var byf = (float)by / (float)16384;

                // aggregate with weights
                var r = d * rxf * byf;
                r += c * (1 - rxf) * byf;
                byf = 1 - byf;
                r = b * rxf * byf;
                r += a * (1 - rxf) * byf;

                r = mad.Transform(r);

                return Unsafe.As<Vector3, TPixel>(ref r);
            }

            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly unsafe float GetScalarPixel(int x, int y)
        {
            if (typeof(TPixel) == typeof(float))
            {
                return _GetPixelAs<float>(x, y);
            }

            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly unsafe float GetScalarSample(int x, int y, int rx, int by)
        {
            if (typeof(TPixel) == typeof(float))
            {
                var a = _GetPixelAs<float>(x, y);
                var b = _GetPixelAs<float>(x + 1, y);
                var c = _GetPixelAs<float>(x, y + 1);
                var d = _GetPixelAs<float>(x + 1, y + 1);

                var rxf = (float)rx / (float)16384;
                var byf = (float)by / (float)16384;

                // aggregate with weights
                var r = d * rxf * byf;
                r += c * (1 - rxf) * byf;
                byf = 1 - byf;
                r += b * rxf * byf;
                r += a * (1 - rxf) * byf;

                return r;
            }

            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly unsafe Vector3 GetVector3Pixel(int x, int y)
        {
            if (sizeof(TPixel) == 3)
            {
                var r = _GetPixelAs<_PixelXYZ24>(x, y);
                return new Vector3(r.X, r.Y, r.Z);
            }

            if (typeof(TPixel) == typeof(Vector3))
            {
                return _GetPixelAs<Vector3>(x, y);
            }

            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly unsafe void GetVector3Pixel(int x, int y, out Vector3 result)
        {
            if (sizeof(TPixel) == 3)
            {
                var r = _GetPixelAs<_PixelXYZ24>(x, y);
                result.X = r.X;
                result.Y = r.Y;
                result.Z = r.Z;
                return;
            }

            if (typeof(TPixel) == typeof(Vector3))
            {
                result = _GetPixelAs<Vector3>(x, y);
                return;
            }

            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly unsafe Vector3 GetVector3Sample(int x, int y, int rx, int by)
        {
            if (sizeof(TPixel) == 3)
            {
                var a = _GetPixelAs<_PixelXYZ24>(x, y);
                var b = _GetPixelAs<_PixelXYZ24>(x + 1, y);
                var c = _GetPixelAs<_PixelXYZ24>(x, y + 1);
                var d = _GetPixelAs<_PixelXYZ24>(x + 1, y + 1);

                // _PixelTransformIterator runs at a fraction of 16384
                // and Pixel.InterpolateBilinear runs at a fraction of 1024
                // so we need to divide by 4 to convert the fractions.
                var r = _PixelXYZ24.BiLerp(a, b, c, d, (uint)rx / 16, (uint)by / 16);
                return new Vector3(r.X, r.Y, r.Z);
            }

            if (sizeof(TPixel) == 4) // interpret as RGBA or BGRA; Alpha is discarded
            {
                var a = _GetPixelAs<_PixelXYZ24>(x, y);
                var b = _GetPixelAs<_PixelXYZ24>(x + 1, y);
                var c = _GetPixelAs<_PixelXYZ24>(x, y + 1);
                var d = _GetPixelAs<_PixelXYZ24>(x + 1, y + 1);

                // _PixelTransformIterator runs at a fraction of 16384
                // and Pixel.InterpolateBilinear runs at a fraction of 1024
                // so we need to divide by 4 to convert the fractions.
                var r = _PixelXYZ24.BiLerp(a, b, c, d, (uint)rx / 16, (uint)by / 16);
                return new Vector3(r.X, r.Y, r.Z);
            }

            if (typeof(TPixel) == typeof(Vector3))
            {
                var a = _GetPixelAs<Vector3>(x, y);
                var b = _GetPixelAs<Vector3>(x + 1, y);
                var c = _GetPixelAs<Vector3>(x, y + 1);
                var d = _GetPixelAs<Vector3>(x + 1, y + 1);

                var rxf = (float)rx / (float)16384;
                var byf = (float)by / (float)16384;

                // aggregate with weights
                var r = d * rxf * byf;
                r += c * (1 - rxf) * byf;
                byf = 1 - byf;
                r += b * rxf * byf;
                r += a * (1 - rxf) * byf;

                return r;
            }

            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly unsafe Vector4 GetVector4Pixel(int x, int y)
        {
            if (typeof(TPixel) == typeof(Vector4))
            {
                return _GetPixelAs<Vector4>(x, y);
            }

            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly unsafe Vector4 GetVector4Sample(int x, int y, int rx, int by)
        {
            if (typeof(TPixel) == typeof(Vector4))
            {
                var a = _GetPixelAs<Vector4>(x, y);
                var b = _GetPixelAs<Vector4>(x + 1, y);
                var c = _GetPixelAs<Vector4>(x, y + 1);
                var d = _GetPixelAs<Vector4>(x + 1, y + 1);

                var rxf = (float)rx / (float)16384;
                var byf = (float)by / (float)16384;

                // aggregate with weights
                var r = d * rxf * byf;
                r += c * (1 - rxf) * byf;
                byf = 1 - byf;
                r += b * rxf * byf;
                r += a * (1 - rxf) * byf;

                return r;
            }

            throw new NotImplementedException();
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

        // gets the rectangle representing the source region that contains the pixels to be sampled for this row.
        public readonly System.Drawing.Rectangle GetSourceRect(int targetWidth)
        {
            var xx = _X + _Dx * targetWidth;
            var minX = Math.Min(_X, xx) >> BITSHIFT;
            var maxX = Math.Max(_X, xx) >> BITSHIFT;

            var yy = _Y + _Dy * targetWidth;
            var minY = Math.Min(_Y, yy) >> BITSHIFT;
            var maxY = Math.Max(_Y, yy) >> BITSHIFT;

            return new System.Drawing.Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MoveNext(out int x, out int y)
        {
            x = _X >> BITSHIFT;
            y = _Y >> BITSHIFT;

            _X += _Dx;
            _Y += _Dy;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
