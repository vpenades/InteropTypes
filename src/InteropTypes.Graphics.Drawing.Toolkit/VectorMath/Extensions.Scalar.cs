using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace InteropDrawing
{
    static partial class _PrivateExtensions
    {
        #region integrity check

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SanityCheckNonNegative(this int i)
        {
            #if DEBUG
            if (i < 0) System.Diagnostics.Debugger.Break();
            #endif

            return i;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long SanityCheckNonNegative(this long l)
        {
            #if DEBUG
            if (l < 0) System.Diagnostics.Debugger.Break();
            #endif

            return l;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SanityCheckNonNegative(this float f)
        {
            #if DEBUG
            if (f < 0) System.Diagnostics.Debugger.Break();
            #endif

            return f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SanityCheckNonNegative(this double d)
        {
            #if DEBUG
            if (d < 0) System.Diagnostics.Debugger.Break();
            #endif

            return d;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNan(this float val) => float.IsNaN(val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNan(this double val) => double.IsNaN(val);

        /// <summary>
        /// Tells if the value is finite and not NaN
        /// </summary>
        /// <remarks>
        /// <see href="https://github.com/dotnet/runtime/blob/5df6cc63151d937724fa0ce8138e69f933052606/src/libraries/System.Private.CoreLib/src/System/Single.cs#L74">DotNet implementation</see>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(this float val)
        {
            return !float.IsNaN(val) && !float.IsInfinity(val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(this double val) => !double.IsNaN(val) && !double.IsInfinity(val);

        #endregion

        #region angular conversion

        private const Single _FloatToDegrees = (Single)(180 / Math.PI);
        private const Double _DoubleToDegrees = 180 / Math.PI;
        private const Single _FloatToRadians = (Single)(Math.PI / 180);
        private const Double _DoubleToRadians = Math.PI / 180;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToDegrees(this float radians) => radians * _FloatToDegrees;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToRadians(this float degrees) => degrees * _FloatToRadians;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToRadians(this int degrees) => (float)degrees * _FloatToRadians;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToDegrees(this double radians) => radians * _DoubleToDegrees;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToRadians(this double degrees) => degrees * _DoubleToRadians;

        #endregion

        #region round

        // https://stackoverflow.com/questions/33557332/can-i-speed-up-my-special-round-function

        public static float FastRound(this float value)
        {
            return value > 0 ? value + 0.5f : value - 0.5f;
        }        

        #endregion

        #region Clamp

        public static byte Clamp(this byte val, byte min, byte max)
        {
            System.Diagnostics.Debug.Assert(min <= max);
            return val > max ? max : (val < min ? min : val);
        }

        public static sbyte Clamp(this sbyte val, sbyte min, sbyte max)
        {
            System.Diagnostics.Debug.Assert(min <= max);
            return val > max ? max : (val < min ? min : val);
        }

        public static ushort Clamp(this ushort val, ushort min, ushort max)
        {
            System.Diagnostics.Debug.Assert(min <= max);
            return val > max ? max : (val < min ? min : val);
        }

        public static short Clamp(this short val, short min, short max)
        {
            System.Diagnostics.Debug.Assert(min <= max);
            return val > max ? max : (val < min ? min : val);
        }

        public static uint Clamp(this uint val, uint min, uint max)
        {
            System.Diagnostics.Debug.Assert(min <= max);
            return val > max ? max : (val < min ? min : val);
        }

        public static int Clamp(this int val, int min, int max)
        {
            System.Diagnostics.Debug.Assert(min <= max);
            return val > max ? max : (val < min ? min : val);
        }

        public static ulong Clamp(this ulong val, ulong min, ulong max)
        {
            System.Diagnostics.Debug.Assert(min <= max);
            return val > max ? max : (val < min ? min : val);
        }

        public static long Clamp(this long val, long min, long max)
        {
            System.Diagnostics.Debug.Assert(min <= max);
            return val > max ? max : (val < min ? min : val);
        }

        public static float Clamp(this float val, float min, float max)
        {
            System.Diagnostics.Debug.Assert(min <= max);
            return val > max ? max : (val < min ? min : val);
        }

        public static double Clamp(this double val, double min, double max)
        {
            System.Diagnostics.Debug.Assert(min <= max);
            return val > max ? max : (val < min ? min : val);
        }

        public static decimal Clamp(this decimal val, decimal min, decimal max)
        {
            System.Diagnostics.Debug.Assert(min <= max);
            return val > max ? max : (val < min ? min : val);
        }

        #endregion
    }
}
