using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    static class _Implementation
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
    }
}
