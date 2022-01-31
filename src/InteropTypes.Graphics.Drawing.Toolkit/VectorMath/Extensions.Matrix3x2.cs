using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace InteropTypes.Graphics.Drawing
{
    static partial class _SystemNumericsExtensions
    {
        #region projections

        public static Matrix3x2 CreateOrthographic2D(this (float width, float height) size)
        {
            Matrix3x2 result;

            result.M11 = 2f / size.width;
            result.M12 = 0;

            result.M22 = 2f / size.height;
            result.M21 = 0;

            result.M31 = 0;
            result.M32 = 0;

            return result;
        }

        // creates a viewport matrix that converts values in the range (-1,1)x(-1,1) to (0,width)x(0,height)
        public static Matrix3x2 CreateViewport2D(this (float width, float height) size)
        {
            if (size.width <= 0) throw new ArgumentOutOfRangeException(nameof(size.width));
            if (size.height <= 0) throw new ArgumentOutOfRangeException(nameof(size.height));

            Matrix3x2 result;

            result.M11 = size.width * 0.5f;
            result.M12 = 0;

            result.M22 = -size.height * 0.5f;
            result.M21 = 0;

            result.M31 = size.width * 0.5f;
            result.M32 = size.height * 0.5f;

            return result;
        }

        public static Matrix3x2 CreateInverseViewport2D(this (float width, float height) size)
        {
            if (size.width <= 0) throw new ArgumentOutOfRangeException(nameof(size.width));
            if (size.height <= 0) throw new ArgumentOutOfRangeException(nameof(size.height));

            Matrix3x2 result;

            result.M11 = 2.0f/size.width;
            result.M12 = 0;

            result.M22 = -2.0f/size.height;
            result.M21 = 0;

            result.M31 = -1;
            result.M32 = 1;

            return result;
        }

        #endregion

        #region interaction with collections

        public static Matrix3x2 GetMatrix3x2(this float[] array, int index)
        {
            return new Matrix3x2
                (
                array[index + 0], array[index + 1],
                array[index + 2], array[index + 3],
                array[index + 4], array[index + 5]
                );
        }

        public static void CopyTo(this Matrix3x2 src, float[] dst, int index)
        {
            dst[index + 0] = src.M11;
            dst[index + 1] = src.M12;
            dst[index + 2] = src.M21;
            dst[index + 3] = src.M22;
            dst[index + 4] = src.M31;
            dst[index + 5] = src.M32;
        }

        #endregion
    }
}
