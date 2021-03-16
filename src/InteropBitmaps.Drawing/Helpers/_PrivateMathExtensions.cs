using System;
using System.Collections.Generic;
using System.Text;

namespace InteropDrawing.Backends.Helpers
{
    static class _PrivateMathExtensions
    {
        public static float _Clamp(this float value, float min = 0, float max = 1)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        public static int _Clamp(this int value, int min = 0, int max = 1)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        public static bool IsReal(this float value) { return !(float.IsNaN(value) | float.IsInfinity(value)); }

        /// <summary>
        /// returns a rounded integer where the lower integer is choosen when less or equal to int+0.5
        /// </summary>
        /// <param name="value">A float value</param>
        /// <returns>An integer value</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int RoundDown(this float value) { return (int)Math.Ceiling(value - 0.5f); }
    }
}
