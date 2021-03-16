/// Taken from: https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/MathF.cs
/// And modified to remove intrinsics and SIMD support.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System
{
    internal static partial class MathF
    {
        private const float _float_NegativeZero = (float)-0.0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool _float_IsNegative(float f)
        {
            return _BitConverter_SingleToInt32Bits(f) < 0;
        }

        public static float IEEERemainder(float x, float y)
        {
            if (float.IsNaN(x)) return x; // IEEE 754-2008: NaN payload must be preserved
            if (float.IsNaN(y)) return y; // IEEE 754-2008: NaN payload must be preserved

            float regularMod = x % y;

            if (float.IsNaN(regularMod)) return float.NaN;

            if ((regularMod == 0) && _float_IsNegative(x)) return _float_NegativeZero;

            float alternativeResult = regularMod - (Abs(y) * Sign(x));

            if (Abs(alternativeResult) == Abs(regularMod))
            {
                float divisionResult = x / y;
                float roundedResult = Round(divisionResult);

                if (Abs(roundedResult) > Abs(divisionResult))
                {
                    return alternativeResult;
                }
                else
                {
                    return regularMod;
                }
            }

            if (Abs(alternativeResult) < Abs(regularMod))
            {
                return alternativeResult;
            }
            else
            {
                return regularMod;
            }
        }

        public static float Log(float x, float y)
        {
            if (float.IsNaN(x)) return x; // IEEE 754-2008: NaN payload must be preserved
            if (float.IsNaN(y)) return y; // IEEE 754-2008: NaN payload must be preserved

            if (y == 1) return float.NaN;

            if ((x != 1) && ((y == 0) || float.IsPositiveInfinity(y))) return float.NaN;

            return Log(x) / Log(y);
        }        

        public static float MaxMagnitude(float x, float y)
        {
            // This matches the IEEE 754:2019 `maximumMagnitude` function
            //
            // It propagates NaN inputs back to the caller and
            // otherwise returns the input with a larger magnitude.
            // It treats +0 as larger than -0 as per the specification.

            float ax = Abs(x);
            float ay = Abs(y);

            if ((ax > ay) || float.IsNaN(ax)) return x;

            if (ax == ay) return _float_IsNegative(x) ? y : x;

            return y;
        }        

        public static float MinMagnitude(float x, float y)
        {
            // This matches the IEEE 754:2019 `minimumMagnitude` function
            //
            // It propagates NaN inputs back to the caller and
            // otherwise returns the input with a larger magnitude.
            // It treats +0 as larger than -0 as per the specification.

            float ax = Abs(x);
            float ay = Abs(y);

            if ((ax < ay) || float.IsNaN(ax)) return x;

            if (ax == ay) return _float_IsNegative(x) ? x : y;

            return y;
        }

        /// <summary>Returns an estimate of the reciprocal of a specified number.</summary>
        /// <param name="x">The number whose reciprocal is to be estimated.</param>
        /// <returns>An estimate of the reciprocal of <paramref name="x" />.</returns>
        /// <remarks>
        ///    <para>On x86/x64 hardware this may use the <c>RCPSS</c> instruction which has a maximum relative error of <c>1.5 * 2^-12</c>.</para>
        ///    <para>On ARM64 hardware this may use the <c>FRECPE</c> instruction which performs a single Newton-Raphson iteration.</para>
        ///    <para>On hardware without specialized support, this may just return <c>1.0 / x</c>.</para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReciprocalEstimate(float x) => 1.0f / x;

        /// <summary>Returns an estimate of the reciprocal square root of a specified number.</summary>
        /// <param name="x">The number whose reciprocal square root is to be estimated.</param>
        /// <returns>An estimate of the reciprocal square root <paramref name="x" />.</returns>
        /// <remarks>
        ///    <para>On x86/x64 hardware this may use the <c>RSQRTSS</c> instruction which has a maximum relative error of <c>1.5 * 2^-12</c>.</para>
        ///    <para>On ARM64 hardware this may use the <c>FRSQRTE</c> instruction which performs a single Newton-Raphson iteration.</para>
        ///    <para>On hardware without specialized support, this may just return <c>1.0 / Sqrt(x)</c>.</para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReciprocalSqrtEstimate(float x) => 1.0f / Sqrt(x);        
    }
}