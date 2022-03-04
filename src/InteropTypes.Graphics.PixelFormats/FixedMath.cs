using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.Graphics
{
    /// <summary>
    /// Contains methods for 12+12+8 fixed math.
    /// </summary>
    internal static class FixedMathCC8
    {
        #region constants

        public const int UnitShift = 12;
        public const int UnitToByteShift = UnitShift - 8;
        public const uint UnitValue = (1 << UnitShift) -1;

        #endregion

        #region API

        [MethodImpl(_PrivateConstants.Fastest)]
        public static uint FromFloat(float value)
        {
            value = Math.Max(0, value);            
            return Math.Min(UnitValue, (uint)(value * UnitValue));
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static uint FromByte(uint value)
        {
            System.Diagnostics.Debug.Assert(value < 256);
            return (value << UnitToByteShift) | (value >> (8- UnitToByteShift));
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static Byte ToByte(uint value)
        {
            System.Diagnostics.Debug.Assert(value <= UnitValue);
            return (Byte)(value >> UnitToByteShift);
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static Byte ToByte(uint value, uint reciprocalByte)
        {
            System.Diagnostics.Debug.Assert(value <= UnitValue);
            System.Diagnostics.Debug.Assert(reciprocalByte <= 256 * UnitValue); // could be 256 * 4095 too

            return (Byte)Math.Min(255, (value * reciprocalByte) >> UnitShift);
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static uint ToReciprocalByte(uint dividend)
        {
            System.Diagnostics.Debug.Assert(dividend > 0);
            System.Diagnostics.Debug.Assert(dividend <= UnitValue);

            const uint unitByte = 256 * UnitValue;

            return unitByte / dividend;
        }

        #endregion

        #region old but useful

        [MethodImpl(_PrivateConstants.Fastest)]
        public static uint From255To65535(uint value)
        {
            System.Diagnostics.Debug.Assert(value < 256);

            return value | (value << 8);
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static uint From65535To255(uint value)
        {
            System.Diagnostics.Debug.Assert(value < 65536);
            return value >> 8;
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static uint From255To16384(uint value)
        {
            System.Diagnostics.Debug.Assert(value < 256);

            const int SHIFT = 14;
            const int EXTRA = SHIFT - 8;

            value <<= SHIFT;
            value /= 255;
            value += 1 << EXTRA;
            value -= value >> (SHIFT - EXTRA);

            return value;
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        public static byte From16384To255(uint value)
        {
            System.Diagnostics.Debug.Assert(value <= 16384);

            return (Byte)((value * 255) >> 14);

            // value -= value >> 13;
            // value >>= (14 - 8);

            // return (byte)value;
        }

        #endregion
    }
}
