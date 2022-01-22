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
        public static Boolean IsFinite(this Vector3 v) { return v.X.IsFinite() & v.Y.IsFinite() & v.Z.IsFinite(); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean IsZero(this Vector3 v) { return (v.X == 0) & (v.Y == 0) & (v.Z == 0); }

        #endregion

        #region elements access

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SelectXY(this Vector3 v) { return new Vector2(v.X, v.Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SelectXZ(this Vector3 v) { return new Vector2(v.X, v.Z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SelectYZ(this Vector3 v) { return new Vector2(v.Y, v.Z); }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithX(this Vector3 v, Single x) { v.X = x; return v; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithY(this Vector3 v, Single y) { v.Y = y; return v; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithZ(this Vector3 v, Single z) { v.Z = z; return v; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithX(this Vector3 v, Converter<Vector3, Single> vx) { v.X = vx(v); return v; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithY(this Vector3 v, Converter<Vector3, Single> vy) { v.Y = vy(v); return v; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithZ(this Vector3 v, Converter<Vector3, Single> vz) { v.Z = vz(v); return v; }

        #endregion

        #region length & normalization

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithLength(this Vector3 v, Single len) { return Vector3.Normalize(v) * len; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithLength(this Vector3 v, Func<Vector3, Single> vl) { return Vector3.Normalize(v) * vl(v); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormalizable(this Vector3 v, Single epsilon = 0.00001f) { return v.IsFinite() && v.Length() > epsilon; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Normalized(this Vector3 v) { return Vector3.Normalize(v); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetNormalizedOrDefault(this Vector3 v, Single minManhattanLen, Vector3 defval)
        {
            return v.ManhattanLength() <= minManhattanLen ? defval : v.Normalized();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ManhattanLength(this Vector3 v) { v = Vector3.Abs(v); return v.X + v.Y + v.Z;; }

        #endregion

        #region operations

        public static int DominantAxis(this Vector3 v)
        {
            v = Vector3.Abs(v);

            return v.X >= v.Y ? (v.X >= v.Z ? 0 : 2) : (v.Y >= v.Z ? 1 : 2);
        }

        public static Vector3 PerpendicularAxis(this Vector3 v)
        {
            return Vector3.Cross(v, v.DominantAxis() == 0 ? Vector3.UnitY : Vector3.UnitX);
        }        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Lerp(this (Vector3 A, Vector3 B) line, Single amount) { return Vector3.Lerp(line.A, line.B, amount); }        

        public static Vector3 LerpCurve(this (Vector3 P1, Vector3 P2, Vector3 P3, Vector3 P4) curve, float amount)
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


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Angle(this (Vector3 A, Vector3 B) pair)
        {
            System.Diagnostics.Debug.Assert(pair.A.IsFinite() && pair.B.IsFinite());

            float dot = Vector3
                .Dot(pair.A.Normalized(), pair.B.Normalized())
                .Clamp(-1, 1);

            return Math.Acos(dot);
        }

        public static void CopyVerticesTo(this in (Vector3 Min, Vector3 Max) box, Span<Vector3> dst)
        {
            dst[0] = new Vector3(box.Min.X, box.Min.Y, box.Min.Z);
            dst[1] = new Vector3(box.Min.X, box.Min.Y, box.Max.Z);
            dst[2] = new Vector3(box.Min.X, box.Max.Y, box.Min.Z);
            dst[3] = new Vector3(box.Min.X, box.Max.Y, box.Max.Z);
            dst[4] = new Vector3(box.Max.X, box.Min.Y, box.Min.Z);
            dst[5] = new Vector3(box.Max.X, box.Min.Y, box.Max.Z);
            dst[6] = new Vector3(box.Max.X, box.Max.Y, box.Min.Z);
            dst[7] = new Vector3(box.Max.X, box.Max.Y, box.Max.Z);
        }

        public static void CopyPlanesTo(this in (Vector3 Min, Vector3 Max) box, Span<Plane> dst)
        {
            dst[0] = new Plane(-Vector3.UnitX, box.Min.X);
            dst[1] = new Plane(-Vector3.UnitY, box.Min.Y);
            dst[2] = new Plane(-Vector3.UnitZ, box.Min.Z);
            dst[3] = new Plane(Vector3.UnitX, -box.Max.X);
            dst[4] = new Plane(Vector3.UnitY, -box.Max.Y);
            dst[5] = new Plane(Vector3.UnitZ, -box.Max.Z);
        }

        #endregion

        #region interaction with collections

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetVector3(this float[] array, int index) { return new Vector3(array[index + 0], array[index + 1], array[index + 2]); }

        public static Vector3 Center(this IEnumerable<Vector3> collection)
        {
            var center = Vector3.Zero;
            float count = 0;

            foreach (var p in collection) { center += p; count += 1; }

            return center / count;
        }

        public static (Vector3,Vector3) Bounds(this IEnumerable<Vector3> collection)
        {
            var min = new Vector3(float.MaxValue);
            var max = new Vector3(float.MinValue);

            foreach (var p in collection)
            {
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }

            return (min, max);
        }

        public static IEnumerable<Single> SelectMany(this IEnumerable<Vector3> collection)
        {
            foreach (var item in collection)
            {
                yield return item.X;
                yield return item.Y;
                yield return item.Z;
            }
        }

        #endregion

    }
}
