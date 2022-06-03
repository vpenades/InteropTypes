using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

using static System.FormattableString;

namespace InteropTypes.Diagnostics
{
	internal static class Guard
	{
		public static void IsNull(string paramName, object param)
		{
			if (param == null) return;

			throw new ArgumentException(paramName);
		}

		public static void NotNull(string paramName, IntPtr param)
		{
			if (param != IntPtr.Zero) return;

			throw new ArgumentNullException(paramName);
		}

		public static void NotNull(string paramName, object param)
		{
			if (param != null) return;

			throw new ArgumentNullException(paramName);
		}

		public static void IsTrue(string paramName, bool param)
		{
			if (param) return;

			throw new ArgumentException(paramName);
		}

		public static void IsFalse(string paramName, bool param)
		{
			if (!param) return;

			throw new ArgumentException(paramName);
		}

		public static void AreTheSame<T>(string paramName, T param, T other) where T : class
		{
			if (param == other)
			{
				return;
			}
			throw new ArgumentException(Invariant($"{paramName} is {param} but should be the same reference as {other}"), paramName);
		}

		public static void AreEqual<T>(string paramName, T param, T other) where T : IEquatable<T>
		{
			if (param.Equals(other)) return;

			throw new ArgumentException(Invariant($"{paramName} is {param} but should be equal to {other}"), paramName);
		}

		public static void LessThan<T>(string paramName, T param, T min) where T : IComparable<T>
		{
			if (param.CompareTo(min) < 0) return;

			throw new ArgumentException(Invariant($"{paramName} is {param} but should be less than {min}"), paramName);
		}

		public static void GreaterThan<T>(string paramName, T param, T min) where T : IComparable<T>
		{
			if (param.CompareTo(min) > 0) return;

			throw new ArgumentException(Invariant($"{paramName} is {param} but should be greater than {min}"), paramName);
		}

		public static void EqualOrGreaterThan<T>(string paramName, T param, T min) where T : IComparable<T>
		{
			if (param.CompareTo(min) >= 0) return; 

			throw new ArgumentException(Invariant($"{paramName} is {param} but should be equal or greater than {min}"), paramName);
		}

		public static void EqualOrLessThan<T>(string paramName, T param, T min) where T : IComparable<T>
		{
			if (param.CompareTo(min) <= 0) return;

			throw new ArgumentException(Invariant($"{paramName} is {param} but should be equal or less than {min}"), paramName);
		}

		public static void BitmapRect(int width, int height, int pixelByteSize)
		{
			Guard.GreaterThan<int>(nameof(width), width, 0);
			Guard.GreaterThan<int>(nameof(height), height, 0);
			Guard.GreaterThan<int>(nameof(pixelByteSize), pixelByteSize, 0);
		}

		public static void BitmapRect(int width, int height, int pixelByteSize, int stepByteSize)
		{
			Guard.GreaterThan<int>(nameof(width), width, 0);
			Guard.GreaterThan<int>(nameof(height), height, 0);
			Guard.GreaterThan<int>(nameof(pixelByteSize), pixelByteSize, 0);

			if (stepByteSize > 0)
			{
				Guard.EqualOrGreaterThan<int>(nameof(stepByteSize), stepByteSize, width * pixelByteSize);
			}
		}		
	}
}
