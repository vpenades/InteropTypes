using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

using MEMMARSHALL = System.Runtime.InteropServices.MemoryMarshal;


namespace InteropTypes.Graphics.Drawing
{
    partial struct Point3
    {
        #region Enumerable

        /// <summary>
        /// Helper method used to convert point params to an array.
        /// </summary>
        /// <param name="points">a sequence of points</param>
        /// <returns>An array of points</returns>
        /// <remarks>
        /// When a function has a <see cref="ReadOnlySpan{Point2}"/> we can
        /// pass a Point2.Params(...) instead.
        /// </remarks>
        [System.Diagnostics.DebuggerStepThrough]
        public static Point3[] Array(params Point3[] points) { return points; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float[] ToArray() { return new float[] { X, Y, Z }; }

        /// <inheritdoc/>
        public readonly IEnumerator<float> GetEnumerator()
        {
            yield return X;
            yield return Y;
            yield return Z;
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            yield return X;
            yield return Y;
            yield return Z;
        }

        #endregion

        #region Centroid

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ Centroid(Point3[] points) { return Centroid(points.AsSpan()); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ Centroid(XYZ[] points) { return Centroid(points.AsSpan()); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ Centroid(ReadOnlySpan<Point3> points)
        {
            if (points.Length == 0) return XYZ.Zero;

            var r = XYZ.Zero;
            foreach (var p in points) { r += p.XYZ; }
            return r / points.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ Centroid(ReadOnlySpan<XYZ> points)
        {
            if (points.Length == 0) return XYZ.Zero;

            var r = XYZ.Zero;
            foreach (var p in points) { r += p; }
            return r / points.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ Centroid(IEnumerable<Point3> points)
        {
            var weight = 1;
            return points.Aggregate(XYZ.Zero, (i, j) => { ++weight; return i + j.XYZ; }) / weight;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ Centroid(IEnumerable<XYZ> points)
        {
            var weight = 1;
            return points.Aggregate((i, j) => { ++weight; return i + j; }) / weight;
        }

        #endregion

        #region Loops
        public static bool IsClosedLoop(ReadOnlySpan<Point3> points)
        {
            return points.Length == 0 || points[0] == points[points.Length - 1];
        }

        #endregion

        #region Conversions

        /// <summary>
        /// exposes the elements of a list as a span of points
        /// </summary>
        /// <typeparam name="T">Typically Vector3, Point3</typeparam>
        /// <param name="points">the imputs to get as a span</param>
        /// <returns>a span of points</returns>
        /// <exception cref="ArgumentException">when the size of the structure don't match</exception>
        /// <remarks>
        /// As long as the span is being in use, the source list must not be modified.
        /// </remarks>
        public static unsafe ReadOnlySpan<Point3> AsPoints<T>(List<T> points)
            where T : unmanaged
        {
            if (points == null || points.Count == 0) return ReadOnlySpan<Point3>.Empty;

            if (sizeof(T) != sizeof(Point3)) throw new ArgumentException("size mismatch", typeof(T).Name);

            return MEMMARSHALL.Cast<T, Point3>(points.GetInternalBuffer());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ReadOnlySpan<Point3> AsPoints<T>(ReadOnlySpan<T> points)
            where T : unmanaged
        {
            if (sizeof(T) != sizeof(Point3)) throw new ArgumentException("size mismatch", typeof(T).Name);

            return MEMMARSHALL.Cast<T, Point3>(points);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<XYZ> AsNumerics(Span<Point3> points) { return MEMMARSHALL.Cast<Point3, XYZ>(points); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<XYZ> AsNumerics(ReadOnlySpan<Point3> points) { return MEMMARSHALL.Cast<Point3, XYZ>(points); }

        #endregion

        #region Transforms

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Span<Point3> points, in Matrix4x4 xform)
        {
            var v3 = AsNumerics(points);

            for (int i = 0; i < points.Length; ++i)
            {
                v3[i] = XYZ.Transform(v3[i], xform);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformNormals(Span<Point3> points, in Matrix4x4 xform)
        {
            var v3 = AsNumerics(points);

            for (int i = 0; i < points.Length; ++i)
            {
                v3[i] = XYZ.TransformNormal(v3[i], xform);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(ReadOnlySpan<Point3> src, Span<Point3> dst, in Matrix4x4 xform)
        {
            var count = Math.Min(src.Length, dst.Length);

            #if NET6_0_OR_GREATER

            ref var srcv = ref MEMMARSHALL.GetReference(AsNumerics(src));
            ref var dstv = ref AsNumerics(dst)[0];

            while (count-- > 0)
            {
                dstv = XYZ.Transform(srcv, xform);
                dstv = ref Unsafe.Add(ref dstv, 1);
                srcv = ref Unsafe.Add(ref srcv, 1);
            }

            #else

            var srcv = AsNumerics(src);
            var dstv = AsNumerics(dst);

            for(int i=0; i < count; ++i) { dstv[i] = XYZ.Transform(srcv[i], xform); }

            #endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformNormals(ReadOnlySpan<Point3> src, Span<Point3> dst, in Matrix4x4 xform)
        {
            var count = Math.Min(src.Length, dst.Length);

            #if NET6_0_OR_GREATER

            ref var srcv = ref MEMMARSHALL.GetReference(AsNumerics(src));
            ref var dstv = ref AsNumerics(dst)[0];

            while (count-- > 0)
            {
                dstv = XYZ.TransformNormal(srcv, xform);
                dstv = ref Unsafe.Add(ref dstv, 1);
                srcv = ref Unsafe.Add(ref srcv, 1);
            }

            #else

            var srcv = AsNumerics(src);
            var dstv = AsNumerics(dst);

            for (int i = 0; i < count; ++i) { dstv[i] = XYZ.TransformNormal(srcv[i], xform); }

            #endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Span<Point3> dst, ReadOnlySpan<Point2> src, float z)
        {
            for (int i = 0; i < dst.Length; ++i)
            {
                dst[i] = new Point3(src[i], z);
            }
        }

        #endregion
    }
}
