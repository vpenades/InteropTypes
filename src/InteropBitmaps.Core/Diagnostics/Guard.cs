using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropBitmaps
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
			throw new ArgumentException(string.Format("{0} is {1} but should be the same reference as {2}", paramName, param, other), paramName);
		}

		public static void AreEqual<T>(string paramName, T param, T other) where T : IEquatable<T>
		{
			if (param.Equals(other)) return;

			throw new ArgumentException(string.Format("{0} is {1} but should be equal to {2}", paramName, param, other), paramName);
		}

		public static void LessThan<T>(string paramName, T param, T min) where T : IComparable<T>
		{
			if (param.CompareTo(min) < 0) return;

			throw new ArgumentException(string.Format("{0} is {1} but should be less than {2}", paramName, param, min), paramName);
		}

		public static void GreaterThan<T>(string paramName, T param, T min) where T : IComparable<T>
		{
			if (param.CompareTo(min) > 0) return;

			throw new ArgumentException(string.Format("{0} is {1} but should be greater than {2}", paramName, param, min), paramName);
		}

		public static void EqualOrGreaterThan<T>(string paramName, T param, T min) where T : IComparable<T>
		{
			if (param.CompareTo(min) >= 0) return; 

			throw new ArgumentException(string.Format("{0} is {1} but should be equal or greater than {2}", paramName, param, min), paramName);
		}

		public static void EqualOrLessThan<T>(string paramName, T param, T min) where T : IComparable<T>
		{
			if (param.CompareTo(min) <= 0) return;

			throw new ArgumentException(string.Format("{0} is {1} but should be equal or less than {2}", paramName, param, min), paramName);
		}

		public static void BitmapRect(int width, int height, int pixelByteSize, int stepByteSize)
		{
			Guard.GreaterThan<int>("width", width, 0);
			Guard.GreaterThan<int>("height", height, 0);
			Guard.GreaterThan<int>("pixelByteSize", pixelByteSize, 0);

			if (stepByteSize > 0)
			{
				Guard.EqualOrGreaterThan<int>("stepByteSize", stepByteSize, width * pixelByteSize);
			}
		}

		public static unsafe void IsValidPixelFormat<TPixel>(in BitmapInfo info) where TPixel : unmanaged
		{
			if (sizeof(TPixel) != info.PixelByteSize)
			{
				throw new ArgumentException("Invalid pixel size.", "TPixel");
			}

			Type typeFromHandle = typeof(TPixel);
			if (typeFromHandle == typeof(float) || typeFromHandle == typeof(Vector2) || typeFromHandle == typeof(Vector3) || typeFromHandle == typeof(Vector4))
			{
				Type depthType = info.PixelFormat.GetDepthType();
				if (depthType != typeof(float))
				{
					throw new ArgumentException(string.Format("Pixel Format is {0}", depthType), "TPixel");
				}
			}
		}
	}
}
