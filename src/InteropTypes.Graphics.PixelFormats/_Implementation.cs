using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    internal static class _Implementation
    {
        [System.Diagnostics.Conditional("DEBUG")]
        public static void AssertNoOverlapWith<T>(this Span<T> a, ReadOnlySpan<T> b)
        {
            System.Diagnostics.Debug.Assert(!a.Overlaps(b),"Memory should not overlap");
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void AssertNoOverlapWith<T>(this ReadOnlySpan<T> a, ReadOnlySpan<T> b)
        {
            System.Diagnostics.Debug.Assert(!a.Overlaps(b), "Memory should not overlap");
        }

        [System.Diagnostics.DebuggerStepThrough]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Span<T> OfType<T>(this Span<Byte> span)
            where T:unmanaged
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, T>(span);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> OfType<T>(this ReadOnlySpan<Byte> span)
            where T : unmanaged
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, T>(span);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Span<Byte> AsBytes<T>(this Span<T> span)
            where T : unmanaged
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<T, byte>(span);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<Byte> AsBytes<T>(this ReadOnlySpan<T> span)
            where T : unmanaged
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<T, byte>(span);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Span<Single> AsSingles<T>(this Span<T> span)
            where T : unmanaged
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<T, Single>(span);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<Single> AsSingles<T>(this ReadOnlySpan<T> span)
            where T : unmanaged
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<T, Single>(span);
        }

        /// <summary>
        /// divides an integer by 255
        /// </summary>
        /// <remarks>
        /// <see href="// https://www.reddit.com/r/C_Programming/comments/gudfyk/faster_divide_by_255/">faster divide by 255</see>
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int DivideBy255(this int value)
        {
            return (value + ((value + 257) >> 8)) >> 8;
        }
    }

    /// <remarks>
    /// <see href="http://lolengine.net/blog/2011/3/20/understanding-fast-float-integer-conversions"/>
    /// </remarks>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    struct FastByteToFloat
    {
        [System.Runtime.InteropServices.FieldOffset(0)]
        private float f;
        [System.Runtime.InteropServices.FieldOffset(0)]
        private uint u;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static float ToFloat(Byte b)
        {
            FastByteToFloat x;
            x.u = 0;
            x.f = 32768.0f;
            x.u |= b;
            return x.f - 32768.0f;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(float v)
        {
            System.Diagnostics.Debug.Assert(v >= 0f && v <= 1f);

            FastByteToFloat x;
            x.u = 0;
            x.f = 32768.0f + v * (255.0f / 256.0f);
            return (byte)x.u;
        }
    }
}
