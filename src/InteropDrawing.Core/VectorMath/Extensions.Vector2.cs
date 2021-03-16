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
        public static bool IsReal(this Vector2 v) { return v.X.IsReal() & v.Y.IsReal(); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(this Vector2 v) { return (v.X == 0) & (v.Y == 0); }

        #endregion

        #region elements access        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SelectYX(this Vector2 v) { return new Vector2(v.Y, v.X); }

        #endregion

        #region length & normalization

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 WithLength(this Vector2 v, float len) { return Vector2.Normalize(v) * len; }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Normalized(this Vector2 v) { return Vector2.Normalize(v); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetNormalizedOrDefault(this Vector2 v, float minManhattanLen, Vector2 defval)
        {
            return v.ManhattanLength() <= minManhattanLen ? defval : v.Normalized();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single ManhattanLength(this Vector2 v) { v = Vector2.Abs(v); return v.X + v.Y; }

        #endregion

        #region other

        public static int DominantAxis(this Vector2 v)
        {
            v = Vector2.Abs(v);

            return v.X >= v.Y ? 0 : 1;
        }

        public static Single Angle(this Vector2 v) { return MathF.Atan2(v.Y, v.X); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cross(this Vector2 a, Vector2 b) { return a.X * b.Y - a.Y * b.X; }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 LerpTo(this Vector2 a, Vector2 b, float amount) { return Vector2.Lerp(a, b, amount); }

        #endregion

        #region interaction with collections

        public static Vector2 Center(this IEnumerable<Vector2> collection)
        {
            var center = Vector2.Zero;
            float count = 0;

            foreach (var p in collection) { center += p; count += 1; }

            return center / count;
        }

        public static IEnumerable<Single> SelectMany(this IEnumerable<Vector2> collection)
        {
            foreach (var item in collection)
            {
                yield return item.X;
                yield return item.Y;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetVector2(this float[] array, int index) { return new Vector2(array[index + 0], array[index + 1]); }


        public static Vector2[] ToVector2Array(this Byte[] src)
        {
            var dst = new Vector2[src.Length / 8];

            for(int i=0; i < dst.Length; ++i)
            {
                var x = BitConverter.ToSingle(src, i * 8 + 0);
                var y = BitConverter.ToSingle(src, i * 8 + 4);
                dst[i] = new Vector2(x, y);
            }

            return dst;
        }

        public static Byte[] ToByteArray(this Vector2[] src)
        {
            var dst = new Byte[src.Length * 8];

            for (int i = 0; i < src.Length; ++i)
            {
                BitConverter.GetBytes(src[i].X).CopyTo(dst, i * 8 + 0);
                BitConverter.GetBytes(src[i].Y).CopyTo(dst, i * 8 + 4);
            }

            return dst;
        }

        #endregion

    }
}
