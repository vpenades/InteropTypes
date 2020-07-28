using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropBitmaps
{
    // TODO: implement https://devblogs.microsoft.com/dotnet/hardware-intrinsics-in-net-core/

    static class _SpanFloatOps
    {
        private static Span<Vector4> ToVector4(Span<Single> span)
        {
            var fourCount = span.Length & ~3;
            return System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(span.Slice(0, fourCount));
        }

        private static ReadOnlySpan<Vector4> ToVector4(ReadOnlySpan<Single> span)
        {
            var fourCount = span.Length & ~3;
            return System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(span.Slice(0, fourCount));
        }

        

        public static Single Min(ReadOnlySpan<Single> span)
        {
            var vectSpan = ToVector4(span);
            var fourMin = Min(vectSpan);

            var min = fourMin.X;
            if (min > fourMin.Y) min = fourMin.Y;
            if (min > fourMin.Z) min = fourMin.Z;
            if (min > fourMin.W) min = fourMin.W;

            for (int i = vectSpan.Length * 4; i < span.Length; ++i)
            {
                var v = span[i];
                if (min > v) min = v;
            }

            return min;
        }

        public static Single Max(ReadOnlySpan<Single> span)
        {
            var vectSpan = ToVector4(span);
            var fourMax = Max(vectSpan);

            var max = fourMax.X;
            if (max < fourMax.Y) max = fourMax.Y;
            if (max < fourMax.Z) max = fourMax.Z;
            if (max < fourMax.W) max = fourMax.W;

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
            var vectSpan = ToVector4(span);
            var (fourMin, fourMax) = MinMax(vectSpan);

            var min = fourMin.X;
            if (min > fourMin.Y) min = fourMin.Y;
            if (min > fourMin.Z) min = fourMin.Z;
            if (min > fourMin.W) min = fourMin.W;

            var max = fourMax.X;
            if (max < fourMax.Y) max = fourMax.Y;
            if (max < fourMax.Z) max = fourMax.Z;
            if (max < fourMax.W) max = fourMax.W;

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

        public static void FitBetweenZeroAndOne(Span<Single> span)
        {
            var (min, max) = MinMax(span);
            AddAndMultiply(span, -min, 1.0f / (max - min));
        }

        public static void MultiplyAndAdd(Span<Single> span, Single mul, Single add)
        {
            var vectSpan = ToVector4(span);
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
            var vectSpan = ToVector4(span);
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

        public static void CopyPixels(ReadOnlySpan<Single> src, Span<Byte> dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst), dst.Length, src.Length);
            
            var srcVec = System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(src.Slice(0, dst.Length & ~3));

            var minVec = new Vector4(range.min);
            var maxVec = new Vector4(range.max);
            var offVec = new Vector4(transform.offset);

            for (int x = 0; x < srcVec.Length; ++x)
            {
                var v = (offVec + srcVec[x]) * transform.scale;

                v = Vector4.Max(v, minVec);
                v = Vector4.Min(v, maxVec);

                var i = x * 4;

                dst[i + 0] = (Byte)v.X;
                dst[i + 1] = (Byte)v.Y;
                dst[i + 2] = (Byte)v.Z;
                dst[i + 3] = (Byte)v.W;
            }

            for (int x = srcVec.Length * 4; x < dst.Length; ++x)
            {
                var v = (transform.offset + src[x]) * transform.scale;

                if (v < range.min) v = range.min;
                if (v > range.max) v = range.max;

                dst[x] = (Byte)v;
            }
        }

        public static void CopyPixels(ReadOnlySpan<Single> src, Span<Byte> dst, (Single offset, Single scale) transform)
        {
            Guard.AreEqual(nameof(dst), dst.Length, src.Length);

            var srcVec = System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(src.Slice(0, dst.Length & ~3));
            
            var offVec = new Vector4(transform.offset);

            for (int x = 0; x < srcVec.Length; ++x)
            {
                var v = (offVec + srcVec[x]) * transform.scale;
                
                var i = x * 4;

                dst[i + 0] = (Byte)v.X;
                dst[i + 1] = (Byte)v.Y;
                dst[i + 2] = (Byte)v.Z;
                dst[i + 3] = (Byte)v.W;
            }

            for (int x = srcVec.Length * 4; x < dst.Length; ++x)
            {
                var v = (transform.offset + src[x]) * transform.scale;                

                dst[x] = (Byte)v;
            }
        }

        public static void CopyPixels(ReadOnlySpan<Byte> src, Span<Single> dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst), dst.Length, src.Length);

            var dstVec = System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(dst.Slice(0, dst.Length & ~3));

            var minVec = new Vector4(range.min);
            var maxVec = new Vector4(range.max);
            var offVec = new Vector4(transform.offset);

            for (int x = 0; x < dstVec.Length; ++x)
            {
                var i = x * 4;

                var v = (offVec + new Vector4(src[i + 0], src[i + 1], src[i + 2], src[i + 3])) * transform.scale;

                v = Vector4.Max(v, minVec);
                v = Vector4.Min(v, maxVec);

                dstVec[x] = v;
            }

            for (int x = dstVec.Length * 4; x < dst.Length; ++x)
            {
                var v = (transform.offset + src[x]) * transform.scale;

                if (v < range.min) v = range.min;
                if (v > range.max) v = range.max;

                dst[x] = v;
            }
        }

        public static void CopyPixels(ReadOnlySpan<Byte> src, Span<Single> dst, (Single offset, Single scale) transform)
        {
            Guard.AreEqual(nameof(dst), dst.Length, src.Length);

            var dstVec = System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(dst.Slice(0, dst.Length & ~3));            
            var offVec = new Vector4(transform.offset);

            for (int x = 0; x < dstVec.Length; ++x)
            {
                var i = x * 4;

                dstVec[x] = (offVec + new Vector4(src[i + 0], src[i + 1], src[i + 2], src[i + 3])) * transform.scale;
            }

            for (int x = dstVec.Length * 4; x < dst.Length; ++x)
            {
                dst[x] = (transform.offset + src[x]) * transform.scale;
            }
        }



        public static bool SequenceEqual(ReadOnlySpan<float> a, ReadOnlySpan<float> b)
        {
            if (a.Length != b.Length) return false;

            var av = ToVector4(a);
            var bv = ToVector4(b);

            for(int i=0; i < av.Length; ++i)
            {
                if (av[i] != bv[i]) return false;
            }

            for (int i = av.Length * 4; i < a.Length; ++i)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }

        public static void Clamp(Span<Single> span, Single min, Single max)
        {
            var vectSpan = ToVector4(span);
            var vectMin = new Vector4(min);
            var vectMax = new Vector4(max);

            for (int i = 0; i < vectSpan.Length; ++i)
            {
                vectSpan[i] = Vector4.Max(Vector4.Min(vectSpan[i], vectMax), vectMin);
            }

            for (int i = vectSpan.Length * 4; i < span.Length; ++i)
            {
                span[i] = Math.Max(Math.Min(span[i], max), min);
            }
        }


        public static void LerpArray<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right, float amount, Span<T> dst)
            where T:unmanaged
        {
            LerpArray
                (System.Runtime.InteropServices.MemoryMarshal.Cast<T, float>(left)
                , System.Runtime.InteropServices.MemoryMarshal.Cast<T, float>(right)
                , amount
                , System.Runtime.InteropServices.MemoryMarshal.Cast<T, float>(dst)
                );

        }

        public static void LerpArray(ReadOnlySpan<float> left, ReadOnlySpan<float> right, float amount, Span<float> dst)
        {
            var l = ToVector4(left);
            var r = ToVector4(right);
            var d = ToVector4(dst);

            for(int i=0; i < d.Length; ++i)
            {
                d[i] = Vector4.Lerp(l[i], r[i], amount);
            }

            for (int i = d.Length * 4; i < dst.Length; ++i)
            {
                dst[i] = left[i] * (1 - amount) + right[i] * amount;
            }
        }

        public static void LerpArray(ReadOnlySpan<float> left, ReadOnlySpan<float> right, float amount, Span<Byte> dst)
        {
            var l = ToVector4(left);
            var r = ToVector4(right);
            var d = dst.Length / 4;

            for (int i = 0; i < d; ++i)
            {
                var v = Vector4.Lerp(l[i], r[i], amount);

                dst[i * 4 + 0] = (Byte)v.X;
                dst[i * 4 + 1] = (Byte)v.Y;
                dst[i * 4 + 2] = (Byte)v.Z;
                dst[i * 4 + 3] = (Byte)v.W;
            }

            for (int i = d * 4; i < dst.Length; ++i)
            {
                dst[i] = (Byte)(left[i] * (1 - amount) + right[i] * amount);
            }
        }
    }
}
