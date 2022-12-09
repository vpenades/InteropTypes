using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    static class _PrivateExtensions
    {
        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(this int value, int min, int max)
        {
            #if !NETSTANDARD2_0
            return Math.Clamp(value, min, max);
            #else
            if (value < min) return min;
            else if (value > max) return max;
            return value;
            #endif
        }

        /// <summary>
        /// Tells if the value is finite and not NaN
        /// </summary>
        /// <remarks>
        /// <see href="https://github.com/dotnet/runtime/blob/5906521ab238e7d5bb8e38ad81e9ce95561b9771/src/libraries/System.Private.CoreLib/src/System/Single.cs#L74">DotNet implementation</see>
        /// </remarks>
        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(this float val)
        {
            #if !NETSTANDARD2_0
            return float.IsFinite(val);
            #else
            return !float.IsNaN(val) && !float.IsInfinity(val);
            #endif
        }

        public static bool IsFiniteOrNull(this float? val)
        {
            if (!val.HasValue) return true;

            #if !NETSTANDARD2_0
            return float.IsFinite(val.Value);
            #else
            return !float.IsNaN(val.Value) && !float.IsInfinity(val.Value);
            #endif
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFiniteAndNotZero(this in System.Numerics.Matrix3x2 matrix)
        {
            if (!matrix.IsFinite()) return false;
            if (matrix.GetDeterminant() == 0) return false;
            return true;
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(this in System.Numerics.Matrix3x2 matrix)
        {
            var isfinite = true;

            isfinite &= matrix.M11.IsFinite();
            isfinite &= matrix.M12.IsFinite();

            isfinite &= matrix.M21.IsFinite();
            isfinite &= matrix.M22.IsFinite();

            isfinite &= matrix.M31.IsFinite();
            isfinite &= matrix.M32.IsFinite();

            return isfinite;
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFiniteAndNotZero(this in System.Numerics.Matrix4x4 matrix)
        {
            if (!matrix.IsFinite()) return false;
            if (matrix.GetDeterminant() == 0) return false;
            return true;
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(this in System.Numerics.Matrix4x4 matrix)
        {
            var isfinite = true;

            isfinite &= matrix.M11.IsFinite();
            isfinite &= matrix.M12.IsFinite();
            isfinite &= matrix.M13.IsFinite();
            isfinite &= matrix.M14.IsFinite();

            isfinite &= matrix.M21.IsFinite();
            isfinite &= matrix.M22.IsFinite();
            isfinite &= matrix.M23.IsFinite();
            isfinite &= matrix.M24.IsFinite();

            isfinite &= matrix.M31.IsFinite();
            isfinite &= matrix.M32.IsFinite();
            isfinite &= matrix.M33.IsFinite();
            isfinite &= matrix.M34.IsFinite();

            isfinite &= matrix.M41.IsFinite();
            isfinite &= matrix.M42.IsFinite();
            isfinite &= matrix.M43.IsFinite();
            isfinite &= matrix.M44.IsFinite();

            return isfinite;
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GuardIsFinite(this string param, float value)
        {
            if (!IsFinite(value)) throw new ArgumentException("not finite.", param);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GuardIsFinite(this string param, System.Numerics.Matrix3x2 value)
        {
            if (!IsFinite(value)) throw new ArgumentException("not finite.", param);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GuardIsFinite(this string param, System.Numerics.Matrix4x4 value)
        {
            if (!IsFinite(value)) throw new ArgumentException("not finite.", param);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GuardIsFiniteOrNull(this string param, float? value)
        {            
            if (value.HasValue && !IsFinite(value.Value)) throw new ArgumentException("not finite.", param);
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> GetInternalBuffer<T>(this List<T> list)
            where T:unmanaged
        {
            #if NET6_0_OR_GREATER
            return System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list);
            #else
            return list.ToArray();
            #endif
        }
    }
}
