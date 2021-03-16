/// Taken from: https://github.com/dotnet/runtime/blob/master/src/coreclr/System.Private.CoreLib/src/System/MathF.CoreCLR.cs
/// And modified to remove intrinsics and SIMD support.

using System;
using System.Runtime.CompilerServices;

namespace System
{
    [System.Diagnostics.DebuggerStepThrough]
    internal static partial class MathF
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Abs(float x) { return Math.Abs(x); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float x, float y) => Math.Max(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float x, float y) => Math.Min(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Acos(float x) => (float)Math.Acos(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Acosh(float x) => throw new NotImplementedException();
                
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Asin(float x) => (float)Math.Asin(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Asinh(float x) => throw new NotImplementedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Atan(float x) => (float)Math.Atan(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Atanh(float x) => throw new NotImplementedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Atan2(float y, float x) => (float)Math.Atan2(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cbrt(float x) => throw new NotImplementedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Ceiling(float x) => (float)Math.Ceiling(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cos(float x) => (float)Math.Cos(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cosh(float x) => (float)Math.Cosh(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Exp(float x) => (float)Math.Exp(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Floor(float x) => (float)Math.Floor(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float FusedMultiplyAdd(float x, float y, float z) => throw new NotImplementedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ILogB(float x) => throw new NotImplementedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log(float x) => (float)Math.Log(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log2(float x) => throw new NotImplementedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log10(float x) => (float)Math.Log10(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Pow(float x, float y) => (float)Math.Pow(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sin(float x) => (float)Math.Sin(x);

        public static (float Sin, float Cos) SinCos(float x) => throw new NotImplementedException();
                
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sinh(float x) => (float)Math.Sinh(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sqrt(float x) => (float)Math.Sqrt(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Tan(float x) => (float)Math.Tan(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Tanh(float x) => (float)Math.Tanh(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(float x) => (float)Math.Round(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(float x, int digits) => (float)Math.Round(x, digits, MidpointRounding.ToEven);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(float x, MidpointRounding mode) => (float)Math.Round(x, mode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(float x, int digits, MidpointRounding mode) => (float)Math.Round(x, digits, mode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(float x) => Math.Sign(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Truncate(float x) => (float)Math.Truncate(x);
    }
}