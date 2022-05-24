using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes
{
    static class _PrivateExtensions
    {
        // https://github.com/YairHalberstadt/SpanLinq
        public static bool All<T>(this ReadOnlySpan<T> collection, Predicate<T> predicate) where T:unmanaged
        {
            foreach(var item in collection)
            {
                if (!predicate(item)) return false;
            }

            return true;            
        }

    }
}
