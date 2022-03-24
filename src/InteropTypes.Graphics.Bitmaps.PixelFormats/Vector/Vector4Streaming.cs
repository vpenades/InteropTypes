using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    public static partial class Vector4Streaming
    {
        private const float _Reciprocal255 = 1f / 255f;

        [MethodImpl(_PrivateConstants.Fastest)]
        private static void _Divide4(Span<Single> span, out Span<Vector4> quotient, out Span<Single> remainder)
        {
            var len = span.Length / 4;
            quotient = System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(span.Slice(0, len));
            remainder = span.Slice(len * 4);
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        private static void _Divide4(ReadOnlySpan<Single> span, out ReadOnlySpan<Vector4> quotient, out ReadOnlySpan<Single> remainder)
        {
            var len = span.Length / 4;
            quotient = System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(span.Slice(0, len));
            remainder = span.Slice(len * 4);
        }
        

        [MethodImpl(_PrivateConstants.Fastest)]
        private static Single _Min(Single a, Single b, Single c, Single d)
        {
            var min = a;
            if (min > b) min = b;
            if (min > c) min = c;
            if (min > d) min = d;
            return min;
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        private static Single _Max(Single a, Single b, Single c, Single d)
        {
            var min = a;
            if (min < b) min = b;
            if (min < c) min = c;
            if (min < d) min = d;
            return min;
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static Single Min(ReadOnlySpan<Single> span)
        {
            _Divide4(span, out var span4, out span);
            
            var min4 = Min(span4);
            var min = _Min(min4.X, min4.Y, min4.Z, min4.W);

            for (int i = 0; i < span.Length; ++i)
            {
                var v = span[i];
                if (min > v) min = v;
            }

            return min;
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static Single Max(ReadOnlySpan<Single> span)
        {
            _Divide4(span, out var span4, out span);

            var max4 = Max(span4);
            var max = _Max(max4.X, max4.Y, max4.Z, max4.W);

            for (int i = 0; i < span.Length; ++i)
            {
                var v = span[i];
                if (max < v) max = v;
            }

            return max;
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static Vector4 Min(ReadOnlySpan<Vector4> span)
        {
            var min = new Vector4(float.PositiveInfinity);

            for (int i = 0; i < span.Length; ++i)
            {
                min = Vector4.Min(min, span[i]);
            }

            return min;
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static Vector4 Max(ReadOnlySpan<Vector4> span)
        {
            var max = new Vector4(float.NegativeInfinity);

            for (int i = 0; i < span.Length; ++i)
            {
                max = Vector4.Max(max, span[i]);
            }

            return max;
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static (Single Min, Single Max) MinMax(ReadOnlySpan<Single> span)
        {
            _Divide4(span, out var span4, out span);

            var (min4, max4) = MinMax(span4);
            var min = _Min(min4.X, min4.Y, min4.Z, min4.W);
            var max = _Max(max4.X, max4.Y, max4.Z, max4.W);

            for (int i = 0; i < span.Length; ++i)
            {
                var v = span[i];
                if (min > v) min = v;
                if (max < v) max = v;
            }

            return (min, max);
        }

        [MethodImpl(_PrivateConstants.Fastest)]
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

            _Divide4(a, out var a4, out a);
            _Divide4(b, out var b4, out b);

            // quotient
            for (int i = 0; i < a4.Length; ++i)
            {
                if (a4[i] != b4[i]) return false;
            }

            // remainder
            for (int i = 0; i < a.Length; ++i)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static void Clamp(Span<Single> span, Single min, Single max)
        {
            _Divide4(span, out var span4, out span);
            var min4 = new Vector4(min);
            var max4 = new Vector4(max);

            // quotient
            for (int i = 0; i < span4.Length; ++i)
            {
                span4[i] = Vector4.Max(Vector4.Min(span4[i], max4), min4);
            }

            // remainder
            for (int i = 0; i < span.Length; ++i)
            {
                span[i] = Math.Max(Math.Min(span[i], max), min);
            }
        }

        public static void FitToUnits(Span<Single> span)
        {
            var (min, max) = MinMax(span);
            AddMultiply(span, -min, 1.0f / (max - min));
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static void MultiplyAdd(Span<Single> span, Single mul, Single add)
        {
            _Divide4(span, out var span4, out span);
            var mul4 = new Vector4(mul);
            var add4 = new Vector4(add);

            for (int i = 0; i < span4.Length; ++i)
            {
                span4[i] *= mul4;
                span4[i] += add4;
            }

            for (int i = 0; i < span.Length; ++i)
            {
                span[i] *= mul;
                span[i] += add;
            }
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static void AddMultiply(Span<Single> span, Single add, Single mul)
        {
            _Divide4(span, out var span4, out span);
            var mul4 = new Vector4(mul);
            var add4 = new Vector4(add);

            for (int i = 0; i < span4.Length; ++i)
            {
                span4[i] += add4;
                span4[i] *= mul4;
            }

            for (int i = 0; i < span.Length; ++i)
            {
                span[i] += add;
                span[i] *= mul;
            }
        }


        [MethodImpl(_PrivateConstants.Fastest)]
        public static void BytesToUnits(ReadOnlySpan<byte> src, Span<float> dst)
        {
            System.Diagnostics.Debug.Assert(dst.Length <= src.Length);

            ref byte sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
            ref float dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
            var l = dst.Length;

            while (l-- > 0)
            {
                dPtr = sPtr * _Reciprocal255;
                dPtr = ref Unsafe.Add(ref dPtr, 1);
                sPtr = ref Unsafe.Add(ref sPtr, 1);                
            }
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static void BytesToUnitsZYX(ReadOnlySpan<byte> src, Span<float> dst)
        {
            if (dst.Length < src.Length) throw new ArgumentException(nameof(dst));

            ref byte sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
            ref float dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
            var l = dst.Length;

            while (l >= 3)
            {
                l -= 3;
                dPtr = Unsafe.Add(ref sPtr, 2) * _Reciprocal255; dPtr = ref Unsafe.Add(ref dPtr, 1);
                dPtr = Unsafe.Add(ref sPtr, 1) * _Reciprocal255; dPtr = ref Unsafe.Add(ref dPtr, 1);
                dPtr = Unsafe.Add(ref sPtr, 0) * _Reciprocal255; dPtr = ref Unsafe.Add(ref dPtr, 1);
                sPtr = ref Unsafe.Add(ref sPtr, 3);
            }
        }


        [MethodImpl(_PrivateConstants.Fastest)]
        public static void Lerp(ReadOnlySpan<Byte> left, ReadOnlySpan<Byte> right, float amount, Span<Byte> dst)
        {
            Lerp(left, right, (int)(amount * 16384f), dst);
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static void Lerp(ReadOnlySpan<Byte> left, ReadOnlySpan<Byte> right, int amount, Span<Byte> dst)
        {            
            System.Diagnostics.Debug.Assert(dst.Length <= left.Length);
            System.Diagnostics.Debug.Assert(dst.Length <= right.Length);
            System.Diagnostics.Debug.Assert(!left.Overlaps(dst));
            System.Diagnostics.Debug.Assert(!right.Overlaps(dst));

            var lweight = 16384 - amount;

            ref var lPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(left);
            ref var rPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(right);
            ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);

            var l = dst.Length;

            while (l-- > 0)
            {
                dPtr = (byte)((lPtr * lweight + rPtr * amount) / 16384);
                dPtr = ref Unsafe.Add(ref dPtr, 1);
                lPtr = ref Unsafe.Add(ref lPtr, 1);
                rPtr = ref Unsafe.Add(ref rPtr, 1);                
            }
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static void Lerp(ReadOnlySpan<float> left, ReadOnlySpan<float> right, float amount, Span<float> dst)
        {
            System.Diagnostics.Debug.Assert(dst.Length <= left.Length);
            System.Diagnostics.Debug.Assert(dst.Length <= right.Length);
            System.Diagnostics.Debug.Assert(!left.Overlaps(dst));
            System.Diagnostics.Debug.Assert(!right.Overlaps(dst));

            _Divide4(left, out var left4, out left);
            _Divide4(right, out var right4, out right);
            _Divide4(dst, out var dst4, out dst);            

            for (int i = 0; i < dst4.Length; ++i)
            {
                dst4[i] = Vector4.Lerp(left4[i], right4[i], amount);
            }

            for (int i = 0; i < dst.Length; ++i)
            {
                dst[i] = left[i] * (1 - amount) + right[i] * amount;
            }
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static void Lerp(ReadOnlySpan<float> left, ReadOnlySpan<float> right, float amount, Span<Byte> dst, float scale)
        {
            System.Diagnostics.Debug.Assert(dst.Length <= left.Length);
            System.Diagnostics.Debug.Assert(dst.Length <= right.Length);            

            _Divide4(left, out var left4, out left);
            _Divide4(right, out var right4, out right);
            
            var d = dst.Length / 4;
            var s4 = new Vector4(scale);

            for (int i = 0; i < d; ++i)
            {
                var v = Vector4.Lerp(left4[i], right4[i], amount) * s4;

                dst[i * 4 + 0] = (Byte)v.X;
                dst[i * 4 + 1] = (Byte)v.Y;
                dst[i * 4 + 2] = (Byte)v.Z;
                dst[i * 4 + 3] = (Byte)v.W;
            }

            for (int i = 0; i < dst.Length; ++i)
            {
                dst[i] = (Byte)((left[i] * (1 - amount) + right[i] * amount) *scale);
            }
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static void Lerp(ReadOnlySpan<Byte> left, ReadOnlySpan<Byte> right, int amount, Span<float> dst, float dstScale)
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
                dPtr = dstScale * (float)((lPtr * lweight + rPtr * amount) / 16384);
                dPtr = ref Unsafe.Add(ref dPtr, 1);
                lPtr = ref Unsafe.Add(ref lPtr, 1);
                rPtr = ref Unsafe.Add(ref rPtr, 1);
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
