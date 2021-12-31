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
            var vectSpan = _ToVector4(span);
            var fourMin = Min(vectSpan);
            var min = _Min(fourMin.X, fourMin.Y, fourMin.Z, fourMin.W);

            for (int i = vectSpan.Length * 4; i < span.Length; ++i)
            {
                var v = span[i];
                if (min > v) min = v;
            }

            return min;
        }

        public static Single Max(ReadOnlySpan<Single> span)
        {
            var vectSpan = _ToVector4(span);
            var fourMax = Max(vectSpan);
            var max = _Max(fourMax.X, fourMax.Y, fourMax.Z, fourMax.W);

            for (int i = vectSpan.Length * 4; i < span.Length; ++i)
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
            var vectSpan = _ToVector4(span);
            var (fourMin, fourMax) = MinMax(vectSpan);
            var min = _Min(fourMin.X, fourMin.Y, fourMin.Z, fourMin.W);
            var max = _Max(fourMax.X, fourMax.Y, fourMax.Z, fourMax.W);

            for (int i = vectSpan.Length * 4; i < span.Length; ++i)
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

            var av = _ToVector4(a);
            var bv = _ToVector4(b);

            // vector4
            for (int i = 0; i < av.Length; ++i)
            {
                if (av[i] != bv[i]) return false;
            }

            // remainder
            for (int i = av.Length * 4; i < a.Length; ++i)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }

        public static void Clamp(Span<Single> span, Single min, Single max)
        {
            var vectSpan = _ToVector4(span);
            var vectMin = new Vector4(min);
            var vectMax = new Vector4(max);

            // vector4
            for (int i = 0; i < vectSpan.Length; ++i)
            {
                vectSpan[i] = Vector4.Max(Vector4.Min(vectSpan[i], vectMax), vectMin);
            }

            // remainder
            for (int i = vectSpan.Length * 4; i < span.Length; ++i)
            {
                span[i] = Math.Max(Math.Min(span[i], max), min);
            }
        }

        public static void MultiplyAndAdd(Span<Single> span, Single mul, Single add)
        {
            var vectSpan = _ToVector4(span);
            var vectMul = new Vector4(mul);
            var vectAdd = new Vector4(add);

            for (int i = 0; i < vectSpan.Length; ++i)
            {
                vectSpan[i] *= vectMul;
                vectSpan[i] += vectAdd;
            }

            for (int i = vectSpan.Length * 4; i < span.Length; ++i)
            {
                span[i] *= mul;
                span[i] += add;
            }
        }

        public static void AddAndMultiply(Span<Single> span, Single add, Single mul)
        {
            var vectSpan = _ToVector4(span);
            var vectMul = new Vector4(mul);
            var vectAdd = new Vector4(add);

            for (int i = 0; i < vectSpan.Length; ++i)
            {
                vectSpan[i] += vectAdd;
                vectSpan[i] *= vectMul;
            }

            for (int i = vectSpan.Length * 4; i < span.Length; ++i)
            {
                span[i] += add;
                span[i] *= mul;
            }
        }

        public static void CopyByteToUnitLUT(ReadOnlySpan<byte> src, Span<float> dst)
        {
            ref byte sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
            ref float dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);

            for(int i=0; i < dst.Length; ++i)
            {
                dPtr = _ByteToFloatLUT[sPtr];
                sPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref sPtr, 1);
                dPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref dPtr, 1);
            }
        }

        public static void CopyByteToUnit(ReadOnlySpan<byte> src, Span<float> dst)
        {
            var d = _ToVector4(dst);

            for (int i = 0; i < d.Length; ++i)
            {
                d[i] = new Vector4(src[i + 0], src[i + 1], src[i + 2], src[i + 3]) / 255f;
            }

            for (int i = d.Length * 4; i < dst.Length; ++i)
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
