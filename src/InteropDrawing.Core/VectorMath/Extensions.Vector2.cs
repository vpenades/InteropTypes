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

        public static Single Angle(this (Vector2 a, Vector2 b) vectors)
        {
            var av = Vector2.Normalize(vectors.a);
            var bv = Vector2.Normalize(vectors.b);
            var dot = Vector2.Dot(av,bv).Clamp(-1,1);
            return MathF.Acos(dot);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cross(this Vector2 a, Vector2 b) { return a.X * b.Y - a.Y * b.X; }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Lerp(this (Vector2 A, Vector2 B) line, Single amount) { return Vector2.Lerp(line.A, line.B, amount); }

        /// <summary>
        /// Interpolates over a cuadratic curve defined by 3 points
        /// </summary>
        public static Vector2 LerpCurve(this (Vector2 P1, Vector2 P2, Vector2 P3) curve, float amount)
        {
            var p12 = Vector2.Lerp(curve.P1, curve.P2, amount);
            var p23 = Vector2.Lerp(curve.P2, curve.P3, amount);
            return Vector2.Lerp(p12, p23, amount);
        }

        /// <summary>
        /// Interpolates over a cubic curve defined by 4 points
        /// </summary>
        public static Vector2 LerpCurve(this (Vector2 P1, Vector2 P2, Vector2 P3, Vector2 P4) curve, float amount)
        {
            var squared = amount * amount;
            var cubed = squared * amount;

            // calculate weights
            var w4 = (3.0f * squared) - (2.0f * cubed);
            var w1 = 1 - w4;
            var w3 = cubed - squared;
            var w2 = w3 - squared + amount;

            // convert p2 and p3 to tangent vectors:
            var t12 = curve.P2 - curve.P1;
            var t34 = curve.P4 - curve.P3;

            return curve.P1 * w1 + t12 * w2 + t34 * w3 + curve.P4 * w4;
        }

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
