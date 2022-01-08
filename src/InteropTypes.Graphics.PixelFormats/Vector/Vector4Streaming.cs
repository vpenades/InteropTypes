using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace InteropBitmaps
{
    public static class Vector4Streaming
    {
        private static readonly float[] _ByteToFloatLUT = Enumerable.Range(0, 256).Select(idx => (float)idx / 255f).ToArray();

        private const float _Reciprocal255 = 1f / 255f;

        private static Span<Vector4> _ToVector4(Span<Single> span)
        {            
            return System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(span.Slice(0, span.Length & ~3));
        }

        private static ReadOnlySpan<Vector4> _ToVector4(ReadOnlySpan<Single> span)
        {            
            return System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(span.Slice(0, span.Length & ~3));
        }

        private static Single _Min(Single a, Single b, Single c, Single d)
        {
            var min = a;
            if (min > b) min = b;
            if (min > c) min = c;
            if (min > d) min = d;
            return min;
        }

        private static Single _Max(Single a, Single b, Single c, Single d)
        {
            var min = a;
            if (min < b) min = b;
            if (min < c) min = c;
            if (min < d) min = d;
            return min;
        }

        public static Single Min(ReadOnlySpan<Single> span)
        {
            var span4 = _ToVector4(span);
            var min4 = Min(span4);
            var min = _Min(min4.X, min4.Y, min4.Z, min4.W);

            for (int i = span4.Length * 4; i < span.Length; ++i)
            {
                var v = span[i];
                if (min > v) min = v;
            }

            return min;
        }

        public static Single Max(ReadOnlySpan<Single> span)
        {
            var span4 = _ToVector4(span);
            var max4 = Max(span4);
            var max = _Max(max4.X, max4.Y, max4.Z, max4.W);

            for (int i = span4.Length * 4; i < span.Length; ++i)
            {
                var v = span[i];
                if (max < v) max = v;
            }

            return max;
        }

        public static Vector4 Min(ReadOnlySpan<Vector4> span)
        {
            var min = new Vector4(float.PositiveInfinity);

            for (int i = 0; i < span.Length; ++i)
            {
                min = Vector4.Min(min, span[i]);
            }

            return min;
        }

        public static Vector4 Max(ReadOnlySpan<Vector4> span)
        {
            var max = new Vector4(float.NegativeInfinity);

            for (int i = 0; i < span.Length; ++i)
            {
                max = Vector4.Max(max, span[i]);
            }

            return max;
        }

        public static (Single Min, Single Max) MinMax(ReadOnlySpan<Single> span)
        {
            var span4 = _ToVector4(span);
            var (min4, max4) = MinMax(span4);
            var min = _Min(min4.X, min4.Y, min4.Z, min4.W);
            var max = _Max(max4.X, max4.Y, max4.Z, max4.W);

            for (int i = span4.Length * 4; i < span.Length; ++i)
            {
                var v = span[i];
                if (min > v) min = v;
                if (max < v) max = v;
            }

            return (min, max);
        }

        public static (Vector4 Min, Vector4 Max) MinMax(ReadOnlySpan<Vector4> span)
        {
            var min = new Vector4(float.PositiveInfinity);
            var max = new Vector4(float.NegativeInfinity);

            for (int i = 0; i < span.Length; ++i)
            {
                min = Vector4.Min(min, span[i]);
                max = Vector4.Max(max, span[i]);
            }

            return (min, max);
        }

        public static bool SequenceEqual(ReadOnlySpan<float> a, ReadOnlySpan<float> b)
        {
            // We could use a.SequenceEqual, but we could not benefit from vectorized comparison (Proof needed!)

            if (a.Length != b.Length) return false;

            var a4 = _ToVector4(a);
            var b4 = _ToVector4(b);

            // vector4
            for (int i = 0; i < a4.Length; ++i)
            {
                if (a4[i] != b4[i]) return false;
            }

            // remainder
            for (int i = a4.Length * 4; i < a.Length; ++i)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }

        public static void Clamp(Span<Single> span, Single min, Single max)
        {
            var span4 = _ToVector4(span);
            var min4 = new Vector4(min);
            var max4 = new Vector4(max);

            // vector4
            for (int i = 0; i < span4.Length; ++i)
            {
                span4[i] = Vector4.Max(Vector4.Min(span4[i], max4), min4);
            }

            // remainder
            for (int i = span4.Length * 4; i < span.Length; ++i)
            {
                span[i] = Math.Max(Math.Min(span[i], max), min);
            }
        }

        public static void FitToUnits(Span<Single> span)
        {
            var (min, max) = MinMax(span);
            AddMultiply(span, -min, 1.0f / (max - min));
        }

        public static void MultiplyAdd(Span<Single> span, Single mul, Single add)
        {
            var span4 = _ToVector4(span);
            var mul4 = new Vector4(mul);
            var add4 = new Vector4(add);

            for (int i = 0; i < span4.Length; ++i)
            {
                span4[i] *= mul4;
                span4[i] += add4;
            }

            for (int i = span4.Length * 4; i < span.Length; ++i)
            {
                span[i] *= mul;
                span[i] += add;
            }
        }

        public static void AddMultiply(Span<Single> span, Single add, Single mul)
        {
            var span4 = _ToVector4(span);
            var mul4 = new Vector4(mul);
            var add4 = new Vector4(add);

            for (int i = 0; i < span4.Length; ++i)
            {
                span4[i] += add4;
                span4[i] *= mul4;
            }

            for (int i = span4.Length * 4; i < span.Length; ++i)
            {
                span[i] += add;
                span[i] *= mul;
            }
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void BytesToUnits(ReadOnlySpan<byte> src, Span<float> dst)
        {
            System.Diagnostics.Debug.Assert(dst.Length <= src.Length);

            ref byte sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
            ref float dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
            var l = dst.Length;

            while (l-- > 0)
            {
                dPtr = sPtr * _Reciprocal255;
                dPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref dPtr, 1);
                sPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref sPtr, 1);                
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void BytesToUnitsZYX(ReadOnlySpan<byte> src, Span<float> dst)
        {
            if (dst.Length < src.Length) throw new ArgumentException(nameof(dst));

            ref byte sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
            ref float dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
            var l = dst.Length;

            while (l >= 3)
            {
                l -= 3;
                dPtr = System.Runtime.CompilerServices.Unsafe.Add(ref sPtr, 2) * _Reciprocal255;
                dPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref dPtr, 1);
                dPtr = System.Runtime.CompilerServices.Unsafe.Add(ref sPtr, 1) * _Reciprocal255;
                dPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref dPtr, 1);
                dPtr = System.Runtime.CompilerServices.Unsafe.Add(ref sPtr, 0) * _Reciprocal255;
                dPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref dPtr, 1);
                sPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref sPtr, 3);
            }
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Lerp(ReadOnlySpan<Byte> left, ReadOnlySpan<Byte> right, float amount, Span<Byte> dst)
        {
            Lerp(left, right, (int)(amount * 16384f), dst);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Lerp(ReadOnlySpan<Byte> left, ReadOnlySpan<Byte> right, int amount, Span<Byte> dst)
        {            
            System.Diagnostics.Debug.Assert(dst.Length <= left.Length);
            System.Diagnostics.Debug.Assert(dst.Length <= right.Length);

            var lweight = 16384 - amount;

            ref var lPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(left);
            ref var rPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(right);
            ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);

            var l = dst.Length;

            while (l-- > 0)
            {
                dPtr = (byte)((lPtr * lweight + rPtr * amount) / 16384);
                dPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref dPtr, 1);
                lPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref lPtr, 1);
                rPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref rPtr, 1);                
            }
        }

        public static void Lerp(ReadOnlySpan<float> left, ReadOnlySpan<float> right, float amount, Span<float> dst)
        {
            var l = _ToVector4(left);
            var r = _ToVector4(right);
            var d = _ToVector4(dst);

            for (int i = 0; i < d.Length; ++i)
            {
                d[i] = Vector4.Lerp(l[i], r[i], amount);
            }

            for (int i = d.Length * 4; i < dst.Length; ++i)
            {
                dst[i] = left[i] * (1 - amount) + right[i] * amount;
            }
        }

        public static void Lerp(ReadOnlySpan<float> left, ReadOnlySpan<float> right, float amount, float scale, Span<Byte> dst)
        {
            var l = _ToVector4(left);
            var r = _ToVector4(right);
            var d = dst.Length / 4;
            var s4 = new Vector4(scale);

            for (int i = 0; i < d; ++i)
            {
                var v = Vector4.Lerp(l[i], r[i], amount) * s4;

                dst[i * 4 + 0] = (Byte)v.X;
                dst[i * 4 + 1] = (Byte)v.Y;
                dst[i * 4 + 2] = (Byte)v.Z;
                dst[i * 4 + 3] = (Byte)v.W;
            }

            for (int i = d * 4; i < dst.Length; ++i)
            {
                dst[i] = (Byte)((left[i] * (1 - amount) + right[i] * amount) *scale);
            }
        }

        public static void SwapZYX(Span<Byte> data)
        {
            while(data.Length >= 3)
            {
                data[0] ^= data[2];
                data[2] ^= data[0];
                data[0] ^= data[2];
                data = data.Slice(3);
            }
        }

        public static void SwapZYXW(Span<Byte> data)
        {
            while (data.Length >= 4)
            {
                data[0] ^= data[2];
                data[2] ^= data[0];
                data[0] ^= data[2];
                data = data.Slice(4);
            }
        }

        public static void Noise(Span<byte> data, Random randomizer)
        {
            var data2 = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, ushort>(data.Slice(0, data.Length & ~1));

            for (int i = 0; i < data2.Length; ++i)
            {
                data2[i] = (ushort)randomizer.Next(65536);
            }

            for (int i = data.Length * 2; i < data.Length; ++i)
            {
                data[i] = (byte)randomizer.Next(256);
            }
        }

        public static void Noise(Span<float> data, Random randomizer)
        {
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = (float)randomizer.NextDouble(); // net6 has a NextSingle
            }
        }
    }
}
