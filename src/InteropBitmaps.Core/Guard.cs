using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    [System.Diagnostics.DebuggerStepThrough]
    static class Guard
    {
        public static void NotNull(string paramName, IntPtr param)
        {
            if (param != IntPtr.Zero) return;
            throw new ArgumentNullException(paramName);
        }

        public static void NotNull(string paramName, Object param)
        {
            if (param != null) return;
            throw new ArgumentNullException(paramName);
        }        

        public static void AreEqual<T>(string paramName, T param, T other) where T : IEquatable<T>
        {
            if (param.Equals(other)) return;
            throw new ArgumentException($"{paramName} is {param} but should be equal to {other}", paramName);
        }

        public static void LessThan<T>(string paramName, T param, T min) where T : IComparable<T>
        {
            if (param.CompareTo(min) < 0) return;
            throw new ArgumentException($"{paramName} is {param} but should be less than {min}", paramName);
        }

        public static void GreaterThan<T>(string paramName, T param, T min) where T:IComparable<T>
        {
            if (param.CompareTo(min) > 0) return;
            throw new ArgumentException($"{paramName} is {param} but should be greater than {min}", paramName);
        }

        public static void EqualOrGreaterThan<T>(string paramName, T param, T min) where T : IComparable<T>
        {
            if (param.CompareTo(min) >= 0) return;
            throw new ArgumentException($"{paramName} is {param} but should be equal or greater than {min}", paramName);
        }

        public static void EqualOrLessThan<T>(string paramName, T param, T min) where T : IComparable<T>
        {
            if (param.CompareTo(min) <= 0) return;
            throw new ArgumentException($"{paramName} is {param} but should be equal or less than {min}", paramName);
        }

        public static void BitmapRect(int width, int height, int pixelSize, int scanlineSize)
        {
            Guard.GreaterThan(nameof(width), width, 0);
            Guard.GreaterThan(nameof(height), height, 0);
            Guard.GreaterThan(nameof(pixelSize), pixelSize, 0);
            if (scanlineSize > 0) Guard.EqualOrGreaterThan(nameof(scanlineSize), scanlineSize, width * pixelSize);
        }
    }
}
