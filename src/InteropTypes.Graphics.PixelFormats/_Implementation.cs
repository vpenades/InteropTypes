using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    internal static class _Implementation
    {
        public static Span<T> OfType<T>(this Span<Byte> span)
            where T:unmanaged
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, T>(span);
        }

        public static ReadOnlySpan<T> OfType<T>(this ReadOnlySpan<Byte> span)
            where T : unmanaged
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, T>(span);
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
}
