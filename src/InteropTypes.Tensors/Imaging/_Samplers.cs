using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.Tensors.Imaging
{
    ref struct _Sampler2D<TPixel>
        where TPixel:unmanaged
    {
        #region constructor

        public static unsafe _Sampler2D<TPixel> From(SpanTensor2<TPixel> tensor)
        {
            if (sizeof(TPixel) == 3) return From(tensor, new MultiplyAdd(1f / 255f, 0));
            else return From(tensor, MultiplyAdd.Identity);
        }

        public static unsafe _Sampler2D<TPixel> From(SpanTensor2<TPixel> tensor, in MultiplyAdd mad)
        {
            var w = tensor.BitmapSize.Width;
            var h = tensor.BitmapSize.Height;
            var b = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, byte>(tensor.Span);

            return From(b, w * sizeof(TPixel), w, h, mad);
        }

        public static _Sampler2D<TPixel> From(ReadOnlySpan<Byte> buffer, int byteStride, int w, int h)
        {
            return From(buffer, byteStride, w, h, new MultiplyAdd(1f / 255f, 0));
        }

        public static _Sampler2D<TPixel> From(ReadOnlySpan<Byte> buffer, int byteStride, int w, int h, in MultiplyAdd mad)
        {
            return new _Sampler2D<TPixel>(buffer, byteStride, w, h, mad);
        }

        private unsafe _Sampler2D(ReadOnlySpan<Byte> buffer, int byteStride, int w, int h, in MultiplyAdd mad)
        {
            if (byteStride == 0) throw new InvalidOperationException(nameof(byteStride));

            if (w * sizeof(TPixel) > byteStride) throw new ArgumentOutOfRangeException(nameof(w));

            if (byteStride * (h - 1) + w * sizeof(TPixel) > buffer.Length) throw new ArgumentException("buffer too small", nameof(buffer));           

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
        public bool Contains(in System.Drawing.Rectangle other)
        {
            if (other.Left < 0) return false;
            if (other.Top < 0) return false;
            if (other.Right >= _Width) return false;
            if (other.Bottom >= _Height) return false;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly TPixel GetPixel(int x, int y)
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

            return ref System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(_Bytes.Slice(y * _ByteStride))[x];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref readonly TPixel GetPixelUnbounded(int x, int y)
        {
            return ref System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TPixel>(_Bytes.Slice(y * _ByteStride))[x];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref readonly TDstPixel _GetPixel<TDstPixel>(int x, int y)
            where TDstPixel:unmanaged
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

            return ref System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TDstPixel>(_Bytes.Slice(y * _ByteStride))[x];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe TPixel GetSample(int x, int y, int rx, int by)
        {
            if (sizeof(TPixel) == 3)
            {
                var a = _GetPixel<_PixelXYZ24>(x, y);
                var b = _GetPixel<_PixelXYZ24>(x + 1, y);
                var c = _GetPixel<_PixelXYZ24>(x, y + 1);
                var d = _GetPixel<_PixelXYZ24>(x + 1, y + 1);

                // _PixelTransformIterator runs at a fraction of 16384
                // and Pixel.InterpolateBilinear runs at a fraction of 1024
                // so we need to divide by 4 to convert the fractions.
                var r = _PixelXYZ24.BiLerp(a, b, c, d, (uint)rx / 16, (uint)by / 16);

                return Unsafe.As<_PixelXYZ24, TPixel>(ref r);
            }

            if (typeof(TPixel) == typeof(Vector3))
            {
                var a = _GetPixel<Vector3>(x, y);
                var b = _GetPixel<Vector3>(x + 1, y);
                var c = _GetPixel<Vector3>(x, y + 1);
                var d = _GetPixel<Vector3>(x + 1, y + 1);

                var rxf = (float)rx / (float)16384;
                var byf = (float)by / (float)16384;

                // aggregate with weights
                var r = d * rxf * byf;
                r += c * (1 - rxf) * byf;
                byf = 1 - byf;
                r = b * rxf * byf;
                r += a * (1 - rxf) * byf;

                r = r * _Multiply + _Add;

                return Unsafe.As<Vector3, TPixel>(ref r);
            }

            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Vector3 GetVector3Pixel(int x, int y)
        {
            if (sizeof(TPixel) == 3)
            {
                var r = _GetPixel<_PixelXYZ24>(x, y);
                return new Vector3(r.X, r.Y, r.Z) * _Multiply + _Add;
            }

            if (typeof(TPixel) == typeof(Vector3))
            {
                return _GetPixel<Vector3>(x, y) * _Multiply + _Add;
            }

            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Vector3 GetVector3Sample(int x, int y, int rx, int by)
        {
            if (sizeof(TPixel) == 3)
            {
                var a = _GetPixel<_PixelXYZ24>(x, y);
                var b = _GetPixel<_PixelXYZ24>(x + 1, y);
                var c = _GetPixel<_PixelXYZ24>(x, y + 1);
                var d = _GetPixel<_PixelXYZ24>(x + 1, y + 1);

                // _PixelTransformIterator runs at a fraction of 16384
                // and Pixel.InterpolateBilinear runs at a fraction of 1024
                // so we need to divide by 4 to convert the fractions.
                var r = _PixelXYZ24.BiLerp(a, b, c, d, (uint)rx / 16, (uint)by / 16);
                return new Vector3(r.X, r.Y, r.Z) * _Multiply + _Add;
            }

            if (typeof(TPixel) == typeof(Vector3))
            {
                var a = _GetPixel<Vector3>(x, y);
                var b = _GetPixel<Vector3>(x + 1, y);
                var c = _GetPixel<Vector3>(x, y + 1);
                var d = _GetPixel<Vector3>(x + 1, y + 1);

                var rxf = (float)rx / (float)16384;
                var byf = (float)by / (float)16384;

                // aggregate with weights
                var r = d * rxf * byf;
                r += c * (1 - rxf) * byf;
                byf = 1 - byf;
                r += b * rxf * byf;
                r += a * (1 - rxf) * byf;
                
                return r * _Multiply + _Add;
            }

            throw new NotImplementedException();
        }        

        #endregion
    }
}
