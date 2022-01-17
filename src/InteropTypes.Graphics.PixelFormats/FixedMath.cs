using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropBitmaps
{
    internal static class FixedMath
    {
        #region constants

        public const int UnitShift = 14;
        public const uint UnitValue = (1 << UnitShift);
        public const uint Q8Value = 255 << UnitShift;

        public const int RcpShift8 = 16;

        #endregion

        #region API

        public static uint FromFloat(float value)
        {
            uint integet = (uint)(value * (float)UnitValue);
            if (integet < 0) integet = 0;
            else if (integet > UnitValue) integet = UnitValue;
            return integet;
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        internal static uint From255(uint value)
        {
            System.Diagnostics.Debug.Assert(value < 256, $"Value is out of range. Expected less than 256, found {value}");
            
            value <<= 8;
            value |= 255;
            value >>= (16 - UnitShift);

            return value;
        }

        [MethodImpl(_PrivateConstants.Fastest)]
        internal static Byte To255(uint value)
        {
            System.Diagnostics.Debug.Assert(value <= UnitValue, $"Value is out of range. Expected less than {UnitValue}, found {value}");

            return (Byte)(value >> (UnitShift - 8));            
        }

        /// <summary>
        /// Converts <paramref name="value"/> to a 255 range value, applying the reciprocal alpha provided by <see cref="GetUnboundedReciprocal8(uint)"/>
        /// </summary>
        /// <param name="value">A value in the range </param>
        /// <param name="rcpa"></param>
        /// <returns></returns>
        [MethodImpl(_PrivateConstants.Fastest)]
        internal static Byte To255(uint value, uint rcpa)
        {
            System.Diagnostics.Debug.Assert(value <= UnitValue, $"Value is out of range. Expected less or equal to {UnitValue}, found {value}");

            return (Byte)((value * rcpa) >> UnitShift);
        }

        /// <summary>
        /// Gets the reciprocal of <paramref name="value"/>, multiplied by 255 and shifted up by <see cref="RcpShift8"/>
        /// </summary>
        /// <param name="value">A value between 1 and <see cref="UnitValue"/> inclusive.</param>
        /// <returns>The reciprocal of alpha.</returns>
        [MethodImpl(_PrivateConstants.Fastest)]
        internal static uint GetUnboundedReciprocal8(uint value)
        {
            System.Diagnostics.Debug.Assert(value != 0, "Value must not be zero.");
            System.Diagnostics.Debug.Assert(value <= UnitValue, $"Value is out of range. Expected less or equal to {UnitValue}, found {value}");            

            value = (1<<UnitShift) / (value >> 8);

            return value;
        }

        #endregion

        #region old but useful

        public static uint From255To16384(uint value)
        {
            const int SHIFT = 14;
            const int EXTRA = SHIFT - 8;

            value <<= SHIFT;
            value /= 255;
            value += 1 << EXTRA;
            value -= value >> (SHIFT - EXTRA);

            return value;
        }

        public static byte From16384To255(uint value)
        {
            return (Byte)((value * 255) >> 14);

            // value -= value >> 13;
            // value >>= (14 - 8);

            // return (byte)value;
        }

        #endregion
    }
}
