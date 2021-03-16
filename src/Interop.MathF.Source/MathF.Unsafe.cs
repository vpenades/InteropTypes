/// Taken from: https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/MathF.cs
/// And modified to remove intrinsics and SIMD support.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System
{
    internal static partial class MathF
    {
        /// <summary>
        /// Structure used to convert from integer to float and
        /// back without requiring using UNSAFE mode.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Explicit)]
        private struct _Int32BitsSingleFusion
        {
            [System.Runtime.InteropServices.FieldOffset(0)]
            public Int32 Integer;

            [System.Runtime.InteropServices.FieldOffset(0)]
            public Single Floating;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int _BitConverter_SingleToInt32Bits(float value)
        {
            // return *((int*)&value); // Requires UNSAFE

            _Int32BitsSingleFusion tmp = default;
            tmp.Floating = value;
            return tmp.Integer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float _BitConverter_Int32BitsToSingle(int value)
        {            
            // return *((float*)&value); // Requires UNSAFE

            _Int32BitsSingleFusion tmp = default;
            tmp.Integer = value;
            return tmp.Floating;
        }
        
        public static float BitDecrement(float x)
        {
            int bits = _BitConverter_SingleToInt32Bits(x);

            if ((bits & 0x7F800000) >= 0x7F800000)
            {
                // NaN returns NaN
                // -Infinity returns -Infinity
                // +Infinity returns float.MaxValue
                return (bits == 0x7F800000) ? float.MaxValue : x;
            }

            if (bits == 0x00000000)
            {
                // +0.0 returns -float.Epsilon
                return -float.Epsilon;
            }

            // Negative values need to be incremented
            // Positive values need to be decremented

            bits += ((bits < 0) ? +1 : -1);
            return _BitConverter_Int32BitsToSingle(bits);
        }

        public static float BitIncrement(float x)
        {
            int bits = _BitConverter_SingleToInt32Bits(x);

            if ((bits & 0x7F800000) >= 0x7F800000)
            {
                // NaN returns NaN
                // -Infinity returns float.MinValue
                // +Infinity returns +Infinity
                return (bits == unchecked((int)(0xFF800000))) ? float.MinValue : x;
            }

            if (bits == unchecked((int)(0x80000000)))
            {
                // -0.0 returns float.Epsilon
                return float.Epsilon;
            }

            // Negative values need to be decremented
            // Positive values need to be incremented

            bits += ((bits < 0) ? -1 : +1);
            return _BitConverter_Int32BitsToSingle(bits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CopySign(float x, float y)
        {
            const int signMask = 1 << 31;

            // This method is required to work for all inputs,
            // including NaN, so we operate on the raw bits.
            int xbits = _BitConverter_SingleToInt32Bits(x);
            int ybits = _BitConverter_SingleToInt32Bits(y);

            // Remove the sign from x, and remove everything but the sign from y
            xbits &= ~signMask;
            ybits &= signMask;

            // Simply OR them to get the correct sign
            return _BitConverter_Int32BitsToSingle(xbits | ybits);
        }

        private const float SCALEB_C1 = 1.7014118E+38f; // 0x1p127f

        private const float SCALEB_C2 = 1.1754944E-38f; // 0x1p-126f

        private const float SCALEB_C3 = 16777216f; // 0x1p24f

        public static float ScaleB(float x, int n)
        {
            // Implementation based on https://git.musl-libc.org/cgit/musl/tree/src/math/scalblnf.c
            //
            // Performs the calculation x * 2^n efficiently. It constructs a float from 2^n by building
            // the correct biased exponent. If n is greater than the maximum exponent (127) or less than
            // the minimum exponent (-126), adjust x and n to compute correct result.

            float y = x;
            if (n > 127)
            {
                y *= SCALEB_C1;
                n -= 127;
                if (n > 127)
                {
                    y *= SCALEB_C1;
                    n -= 127;
                    if (n > 127)
                    {
                        n = 127;
                    }
                }
            }
            else if (n < -126)
            {
                y *= SCALEB_C2 * SCALEB_C3;
                n += 126 - 24;
                if (n < -126)
                {
                    y *= SCALEB_C2 * SCALEB_C3;
                    n += 126 - 24;
                    if (n < -126)
                    {
                        n = -126;
                    }
                }
            }

            float u = _BitConverter_Int32BitsToSingle(((int)(0x7f + n) << 23));
            return y * u;
        }
    }
}
