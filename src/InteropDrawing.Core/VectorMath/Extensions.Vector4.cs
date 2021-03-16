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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsReal(this Vector4 value) { return value.X.IsReal() && value.Y.IsReal() && value.Z.IsReal() && value.W.IsReal(); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(this Vector4 value) { return value.Equals(Vector4.Zero); }

        #endregion

        #region elements access        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 SelectXYZ(this Vector4 v) { return new Vector3(v.X, v.Y, v.Z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 SelectYZW(this Vector4 v) { return new Vector3(v.X, v.Z, v.W); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SelectXY(this Vector4 v) { return new Vector2(v.X, v.Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SelectYZ(this Vector4 v) { return new Vector2(v.Y, v.Z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SelectZW(this Vector4 v) { return new Vector2(v.Z, v.W); }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 WithXYZ(this Vector4 v, Vector3 p) { v.X = p.X; v.Y = p.Y; v.Z = p.Z; return v; }

        #endregion

        #region collections

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 GetVector4(this float[] array, int index) { return new Vector4(array[index + 0], array[index + 1], array[index + 2], array[index + 3]); }

        public static Vector4 Center(this IEnumerable<Vector4> collection)
        {
            var center = Vector4.Zero;
            float count = 0;

            foreach (var p in collection) { center += p; count += 1; }

            return center / count;
        }

        public static IEnumerable<Single> SelectMany(this IEnumerable<Vector4> collection)
        {
            foreach (var item in collection)
            {
                yield return item.X;
                yield return item.Y;
                yield return item.Z;
                yield return item.W;
            }
        }

        #endregion
    }
}
