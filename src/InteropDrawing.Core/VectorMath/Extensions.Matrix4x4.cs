using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace InteropDrawing
{
    static partial class _SystemNumericsExtensions
    {
        #region integrity check
        public static bool IsReal(this Matrix4x4 value) { return value.SelectRowX().IsFinite() && value.SelectRowY().IsFinite() && value.SelectRowZ().IsFinite() && value.SelectRowW().IsFinite(); }

        #endregion

        #region elements access       

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SelectRowX(this Matrix3x2 matrix) { return new Vector2(matrix.M11, matrix.M12); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SelectRowY(this Matrix3x2 matrix) { return new Vector2(matrix.M21, matrix.M22); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SelectRowZ(this Matrix3x2 matrix) { return new Vector2(matrix.M31, matrix.M32); }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 SelectRowX(this Matrix4x4 matrix) { return new Vector4(matrix.M11, matrix.M12, matrix.M13, matrix.M14); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 SelectRowY(this Matrix4x4 matrix) { return new Vector4(matrix.M21, matrix.M22, matrix.M23, matrix.M24); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 SelectRowZ(this Matrix4x4 matrix) { return new Vector4(matrix.M31, matrix.M32, matrix.M33, matrix.M34); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 SelectRowW(this Matrix4x4 matrix) { return new Vector4(matrix.M41, matrix.M42, matrix.M43, matrix.M44); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 SelectColumnX(this Matrix4x4 matrix) { return new Vector4(matrix.M11, matrix.M21, matrix.M31, matrix.M41); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 SelectColumnY(this Matrix4x4 matrix) { return new Vector4(matrix.M12, matrix.M22, matrix.M32, matrix.M42); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 SelectColumnZ(this Matrix4x4 matrix) { return new Vector4(matrix.M13, matrix.M23, matrix.M33, matrix.M43); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 SelectColumnW(this Matrix4x4 matrix) { return new Vector4(matrix.M14, matrix.M24, matrix.M34, matrix.M44); }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 WithRowX(this Matrix4x4 m, Vector4 rowX) { m.M11 = rowX.X; m.M12 = rowX.Y; m.M13 = rowX.Z; m.M14 = rowX.W; return m; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 WithRowY(this Matrix4x4 m, Vector4 rowY) { m.M21 = rowY.X; m.M22 = rowY.Y; m.M23 = rowY.Z; m.M24 = rowY.W; return m; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 WithRowZ(this Matrix4x4 m, Vector4 rowZ) { m.M31 = rowZ.X; m.M32 = rowZ.Y; m.M33 = rowZ.Z; m.M34 = rowZ.W; return m; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 WithRowW(this Matrix4x4 m, Vector4 rowW) { m.M41 = rowW.X; m.M42 = rowW.Y; m.M43 = rowW.Z; m.M44 = rowW.W; return m; }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 WithColumnX(this Matrix4x4 m, Vector4 rowX) { m.M11 = rowX.X; m.M21 = rowX.Y; m.M31 = rowX.Z; m.M41 = rowX.W; return m; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 WithColumnY(this Matrix4x4 m, Vector4 rowY) { m.M12 = rowY.X; m.M22 = rowY.Y; m.M32 = rowY.Z; m.M42 = rowY.W; return m; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 WithColumnZ(this Matrix4x4 m, Vector4 rowZ) { m.M13 = rowZ.X; m.M32 = rowZ.Y; m.M33 = rowZ.Z; m.M43 = rowZ.W; return m; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 WithColumnW(this Matrix4x4 m, Vector4 rowW) { m.M14 = rowW.X; m.M24 = rowW.Y; m.M34 = rowW.Z; m.M44 = rowW.W; return m; }

        #endregion


        public static Matrix4x4 InvertedOrDefault(this Matrix4x4 matrix, Matrix4x4 defval)
        {
            return Matrix4x4.Invert(matrix, out Matrix4x4 mtx) ? mtx : defval;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 InverseFast(this Matrix4x4 r) // move to right & left handed
        {
            // only valid for orthogonal matrices with column W being 0,0,0,1

            // http://content.gpwiki.org/index.php/MathGem:Fast_Matrix_Inversion
            // R' = transpose(R)
            // M = R' * (- (R' * T))

            var rr = r;

            rr.Translation = Vector3.Zero;

            rr = Matrix4x4.Transpose(rr);

            rr.Translation = -Vector3.TransformNormal(r.Translation, rr);

            return rr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 WithTranslation(this Matrix4x4 m, Vector3 translation)
        {
            m.Translation = translation;
            return m;
        }

        #region interaction with collections
        
        public static Matrix4x4 GetMatrix4x4(this float[] array, int index)
        {
            return new Matrix4x4
                (
                array[index + 0], array[index + 1], array[index + 2], array[index + 3],
                array[index + 4], array[index + 5], array[index + 6], array[index + 7],
                array[index + 8], array[index + 9], array[index + 10], array[index + 11],
                array[index + 12], array[index + 13], array[index + 14], array[index + 15]
                );
        }

        public static void CopyTo(this Matrix4x4 src, float[] dst, int index)
        {
            dst[index + 0] = src.M11;
            dst[index + 1] = src.M12;
            dst[index + 2] = src.M13;
            dst[index + 3] = src.M14;

            dst[index + 4] = src.M21;
            dst[index + 5] = src.M22;
            dst[index + 6] = src.M23;
            dst[index + 7] = src.M24;

            dst[index + 8] = src.M31;
            dst[index + 9] = src.M32;
            dst[index + 10] = src.M33;
            dst[index + 11] = src.M34;

            dst[index + 12] = src.M41;
            dst[index + 13] = src.M42;
            dst[index + 14] = src.M43;
            dst[index + 15] = src.M44;
        }

        #endregion
    }
}
