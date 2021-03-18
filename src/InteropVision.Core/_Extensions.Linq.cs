using System;
using System.Collections.Generic;
using System.Text;

namespace InteropVision
{
    public static partial class _Extensions
    {
        public static IEnumerable<T> Enumerate<T>(this (T A, T B) items)
        {
            yield return items.A;
            yield return items.B;
        }

        public static IEnumerable<T> Enumerate<T>(this (T A, T B, T C) items)
        {
            yield return items.A;
            yield return items.B;
            yield return items.C;
        }

        public static IEnumerable<T> Enumerate<T>(this (T A, T B, T C, T D) items)
        {
            yield return items.A;
            yield return items.B;
            yield return items.C;
            yield return items.D;
        }

        public static IEnumerable<T> Enumerate<T>(this (T A, T B, T C, T D, T E) items)
        {
            yield return items.A;
            yield return items.B;
            yield return items.C;
            yield return items.D;
            yield return items.E;
        }

        public static IEnumerable<T> Enumerate<T>(this (T A, T B, T C, T D, T E, T F) items)
        {
            yield return items.A;
            yield return items.B;
            yield return items.C;
            yield return items.D;
            yield return items.E;
            yield return items.F;
        }

        public static IEnumerable<T> Enumerate<T>(this (T A, T B, T C, T D, T E, T F, T G) items)
        {
            yield return items.A;
            yield return items.B;
            yield return items.C;
            yield return items.D;
            yield return items.E;
            yield return items.F;
            yield return items.G;
        }

        public static IEnumerable<T> Enumerate<T>(this (T A, T B, T C, T D, T E, T F, T G, T H) items)
        {
            yield return items.A;
            yield return items.B;
            yield return items.C;
            yield return items.D;
            yield return items.E;
            yield return items.F;
            yield return items.G;
            yield return items.H;
        }

        public static int IndexOfBest<T>(this Span<T> span, Func<T, T, bool> selector)
        {
            if (span.Length == 0) return -1;

            var index = 0;

            for (int i = 1; i < span.Length; ++i)
            {
                if (selector(span[i], span[index])) index = i;
            }

            return index;
        }

        public static int IndexOfBest<T>(this ReadOnlySpan<T> span, Func<T, T, bool> selector)
        {
            if (span.Length == 0) return -1;

            var index = 0;
            T best = span[index];

            for (int i = 1; i < span.Length; ++i)
            {
                if (selector(span[i], best)) { index = i; best = span[index]; }
            }

            return index;
        }
    }
}
