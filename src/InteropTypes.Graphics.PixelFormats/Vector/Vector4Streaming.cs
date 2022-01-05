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

        public static void BytesToUnitsLUT(ReadOnlySpan<byte> src, Span<float> dst)
        {
            ref byte sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
            ref float dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);

            int dLen = dst.Length;

            while (dLen > 0)
            {
                --dLen;
                dPtr = _ByteToFloatLUT[sPtr];
                sPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref sPtr, 1);
                dPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref dPtr, 1);                
            }
        }

        public static void BytesToUnits(ReadOnlySpan<byte> src, Span<float> dst)
        {
            var dst4 = _ToVector4(dst);

            for (int i = 0; i < dst4.Length; ++i)
            {
                dst4[i] = new Vector4(src[i + 0], src[i + 1], src[i + 2], src[i + 3]) / 255f;
            }

            for (int i = dst4.Length * 4; i < dst.Length; ++i)
            {
                dst[i] = ((float)src[i]) / 255f;
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

        public static void SwapX0Z(Span<Byte> data)
        {
            while(data.Length >= 3)
            {
                data[0] ^= data[2];
                data[2] ^= data[0];
                data[0] ^= data[2];
                data = data.Slice(3);
            }
        }

        public static void SwapX0Z0(Span<Byte> data)
        {
            while (data.Length >= 4)
            {
                data[0] ^= data[2];
                data[2] ^= data[0];
                data[0] ^= data[2];
                data = data.Slice(4);
            }
        }
    }
}
