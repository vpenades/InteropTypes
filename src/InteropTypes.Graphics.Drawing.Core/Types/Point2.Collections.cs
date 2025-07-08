using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

using MEMMARSHALL = System.Runtime.InteropServices.MemoryMarshal;




namespace InteropTypes.Graphics.Drawing
{
    partial struct Point2
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
        public static Point2[] Array(params Point2[] points) { return points; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float[] ToArray() { return new float[] { X, Y }; }

        /// <inheritdoc/>
        public readonly IEnumerator<float> GetEnumerator()
        {
            yield return X;
            yield return Y;
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            yield return X;
            yield return Y;
        }

        #endregion

        #region Centroid

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XY Centroid(Point2[] points) { return Centroid(points.AsSpan()); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XY Centroid(XY[] points) { return Centroid(points.AsSpan()); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XY Centroid(ReadOnlySpan<Point2> points)
        {
            if (points.Length == 0) return XY.Zero;

            var r = XY.Zero;
            foreach (var p in points) { r += p.XY; }
            return r / points.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XY Centroid(ReadOnlySpan<XY> points)
        {
            if (points.Length == 0) return XY.Zero;

            var r = XY.Zero;
            foreach (var p in points) { r += p; }
            return r / points.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XY Centroid(IEnumerable<Point2> points)
        {
            var weight = 1;
            return points.Aggregate(XY.Zero, (i, j) => { ++weight; return i + j.XY; }) / weight;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XY Centroid(IEnumerable<XY> points)
        {
            var weight = 1;
            return points.Aggregate((i, j) => { ++weight; return i + j; }) / weight;
        }

        #endregion

        #region Loops
        public static bool IsClosedLoop(ReadOnlySpan<Point2> points)
        {
            return points.Length == 0 || points[0] == points[points.Length - 1];
        }

        #endregion

        #region Conversions

        /// <summary>
        /// exposes the elements of a list as a span of points
        /// </summary>
        /// <typeparam name="T">Typically Vector2, Point2 or PointF</typeparam>
        /// <param name="points">the imputs to get as a span</param>
        /// <returns>a span of points</returns>
        /// <exception cref="ArgumentException">when the size of the structure don't match</exception>
        /// <remarks>
        /// As long as the span is being in use, the source list must not be modified.
        /// </remarks>
        public static unsafe ReadOnlySpan<Point2> AsPoints<T>(List<T> points)
            where T : unmanaged
        {
            if (points == null || points.Count == 0) return ReadOnlySpan<Point2>.Empty;

            if (sizeof(T) != sizeof(Point2)) throw new ArgumentException("size mismatch", typeof(T).Name);

            return MEMMARSHALL.Cast<T, Point2>(points.GetInternalBuffer());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ReadOnlySpan<Point2> AsPoints<T>(ReadOnlySpan<T> points)
            where T : unmanaged
        {
            if (sizeof(T) != sizeof(Point2)) throw new ArgumentException("size mismatch", typeof(T).Name);

            return MEMMARSHALL.Cast<T, Point2>(points);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<XY> AsNumerics(Span<Point2> points) { return MEMMARSHALL.Cast<Point2, XY>(points); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<XY> AsNumerics(ReadOnlySpan<Point2> points) { return MEMMARSHALL.Cast<Point2, XY>(points); }

        #endregion

        #region Transforms

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Span<Point2> points, in Matrix3x2 xform)
        {
            var v2 = AsNumerics(points);

            for (int i = 0; i < points.Length; ++i)
            {
                v2[i] = XY.Transform(v2[i], xform);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformNormals(Span<Point2> points, in Matrix3x2 xform)
        {
            var v2 = AsNumerics(points);

            for (int i = 0; i < points.Length; ++i)
            {
                v2[i] = XY.TransformNormal(v2[i], xform);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(ReadOnlySpan<Point2> src, Span<Point2> dst, in Matrix3x2 xform)
        {
            var count = Math.Min(src.Length, dst.Length);

            #if NET6_0_OR_GREATER

            ref var srcv = ref MEMMARSHALL.GetReference(AsNumerics(src));
            ref var dstv = ref AsNumerics(dst)[0];

            while (count-- > 0)
            {
                dstv = XY.Transform(srcv, xform);
                dstv = ref Unsafe.Add(ref dstv, 1);
                srcv = ref Unsafe.Add(ref srcv, 1);
            }

            #else

            var srcv = AsNumerics(src);
            var dstv = AsNumerics(dst);

            for(int i=0; i < count; ++i) { dstv[i] = XY.Transform(srcv[i], xform); }

            #endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformNormals(ReadOnlySpan<Point2> src, Span<Point2> dst, in Matrix3x2 xform)
        {
            var count = Math.Min(src.Length, dst.Length);

            #if NET6_0_OR_GREATER

            ref var srcv = ref MEMMARSHALL.GetReference(AsNumerics(src));
            ref var dstv = ref AsNumerics(dst)[0];

            while (count-- > 0)
            {
                dstv = XY.TransformNormal(srcv, xform);
                dstv = ref Unsafe.Add(ref dstv, 1);
                srcv = ref Unsafe.Add(ref srcv, 1);
            }

            #else

            var srcv = AsNumerics(src);
            var dstv = AsNumerics(dst);

            for (int i = 0; i < count; ++i) { dstv[i] = XY.TransformNormal(srcv[i], xform); }

            #endif
        }

        #endregion
    }
}
