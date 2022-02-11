using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

using MEMMARSHALL = System.Runtime.InteropServices.MemoryMarshal;

using VECTOR2 = System.Numerics.Vector2;
using GDIPOINT = System.Drawing.Point;
using GDIPOINTF = System.Drawing.PointF;
using GDISIZE = System.Drawing.Size;
using GDISIZEF = System.Drawing.SizeF;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents an (X,Y) point with two single-precision floating-point values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="Point2"/> is not a replacement of <see cref="VECTOR2"/> but a helper
    /// structure used to simplify the use of (X,Y) values.
    /// </para>
    /// <para>
    /// Equivalent to:<br/>
    /// <list type="table">
    /// <item><b>(float,float)</b></item>
    /// <item><see cref="VECTOR2"/></item>
    /// <item><see cref="GDIPOINTF"/></item>
    /// <item><see cref="GDISIZEF"/></item>
    /// </list>
    /// </para>
    /// </remarks>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    [System.Diagnostics.DebuggerDisplay("{XY}")]
    public partial struct Point2
        : IFormattable
        , IEquatable<Point2>
        , IEquatable<VECTOR2>
        , IEquatable<GDIPOINTF>
        , IEquatable<GDISIZEF>
    {
        #region diagnostics

        /// <summary>
        /// Tells if the value is finite and not NaN
        /// </summary>
        /// <remarks>
        /// <see href="https://github.com/dotnet/runtime/blob/5906521ab238e7d5bb8e38ad81e9ce95561b9771/src/libraries/System.Private.CoreLib/src/System/Single.cs#L74">DotNet implementation</see>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool _IsFinite(float val)
        {
            #if NETSTANDARD2_1_OR_GREATER
            return float.IsFinite(val);
            #else
            return !float.IsNaN(val) && !float.IsInfinity(val);
            #endif
        }

        [System.Diagnostics.DebuggerStepThrough]
        [System.Diagnostics.Conditional("DEBUG")]
        public static void DebugGuardIsFinite(ReadOnlySpan<Point2> points)
        {
            foreach (var point in points) System.Diagnostics.Debug.Assert(Point2.IsFinite(point));
        }

        #endregion

        #region implicit

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2((float X, float Y) p)
        {
            #if NET5_0_OR_GREATER
            Unsafe.SkipInit<Point2>(out var pp);
            pp.X = p.X;
            pp.Y = p.Y;
            return pp;
            #else
            return new Point2(p.X, p.Y);
            #endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2(VECTOR2 p)
        {
            #if NET5_0_OR_GREATER
            Unsafe.SkipInit<Point2>(out var pp);
            pp.XY = p;
            return pp;
            #else
            return new Point2(p.X, p.Y);
            #endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2(GDIPOINT p)
        {
            #if NET5_0_OR_GREATER
            Unsafe.SkipInit<Point2>(out var pp);
            pp.X = p.X;
            pp.Y = p.Y;
            return pp;
            #else
            return new Point2(p.X, p.Y);
            #endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2(GDIPOINTF p)
        {
            #if NET5_0_OR_GREATER
            Unsafe.SkipInit<Point2>(out var pp);
            pp.X = p.X;
            pp.Y = p.Y;
            return pp;
            #else
            return new Point2(p.X, p.Y);
            #endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2(GDISIZE p) { return new Point2(p.Width, p.Height); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2(GDISIZEF p) { return new Point2(p.Width, p.Height); }

        #endregion

        #region constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2(float x, float y) : this() { X = x; Y = y; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2(VECTOR2 vector) : this() { XY = vector; }

        /// <summary>
        /// Helper method used to convert point params to an array.
        /// </summary>
        /// <param name="points">a sequence of points</param>
        /// <returns>An array of points</returns>
        /// <remarks>
        /// When a function has a <see cref="ReadOnlySpan{Point2}"/> we can
        /// pass a Point2.Params(...) instead.
        /// </remarks>
        public static Point2[] Array(params Point2[] points) { return points; }

        #endregion

        #region data        

        /// <summary>
        /// The X component of the point.
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        public float X;

        /// <summary>
        /// The Y component of the point.
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(4)]
        public float Y;

        /// <summary>
        /// The X and Y components of the point.
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        public VECTOR2 XY;

        /// <inheritdoc/>
        public override int GetHashCode() => XY.GetHashCode();

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Point2 otherP) return this.XY == otherP.XY;
            if (obj is VECTOR2 otherV) return this.XY == otherV;
            if (obj is GDIPOINTF otherGP) return AreEqual(this, otherGP);
            if (obj is GDISIZEF otherGS) return AreEqual(this, otherGS);            
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(Point2 other) => AreEqual(this, other);

        /// <inheritdoc/>
        public bool Equals(VECTOR2 other) => AreEqual(this, other);

        /// <inheritdoc/>
        public bool Equals(GDIPOINTF other) => AreEqual(this, other);

        /// <inheritdoc/>
        public bool Equals(GDISIZEF other) => AreEqual(this, other);

        /// <inheritdoc/>
        public static bool operator ==(in Point2 a, Point2 b) => AreEqual(a, b);

        /// <inheritdoc/>
        public static bool operator !=(in Point2 a, Point2 b) => !AreEqual(a, b);

        /// <inheritdoc/>
        public static bool operator ==(in Point2 a, VECTOR2 b) => AreEqual(a, b);

        /// <inheritdoc/>
        public static bool operator !=(in Point2 a, VECTOR2 b) => !AreEqual(a, b);


        public static bool AreEqual(in Point2 a, in Point2 b) { return a.XY == b.XY; }

        public static bool AreEqual(in Point2 a, in VECTOR2 b) { return a.XY == b; }

        public static bool AreEqual(in Point2 a, in GDIPOINTF b) { return a.X == b.X && a.Y == b.Y; }

        public static bool AreEqual(in Point2 a, in GDISIZEF b) { return a.X == b.Width && a.Y == b.Height; }

        #endregion

        #region properties

        public static Point2 Zero => VECTOR2.Zero;
        public static Point2 Half => new Point2(0.5f, 0.5f);
        public static Point2 One => VECTOR2.One;
        public static Point2 UnitX => VECTOR2.UnitX;
        public static Point2 UnitY => VECTOR2.UnitY;
        public float Length => XY.Length();
        public VECTOR2 Normalized => VECTOR2.Normalize(this.XY);
        public int DominantAxis
        {
            get
            {
                var t = VECTOR2.Abs(this.XY);
                return t.X >= t.Y ? 0 : 1;
            }
        }

        #endregion

        #region operators

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 operator -(Point2 a) { return -a.XY; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 operator *(Point2 a, float b) { return a.XY * b; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 operator /(Point2 a, float b) { return a.XY / b; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 operator *(Point2 a, Point2 b) { return a.XY * b; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 operator /(Point2 a, Point2 b) { return a.XY / b; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 operator +(Point2 a, Point2 b) { return a.XY + b.XY; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 operator -(Point2 a, Point2 b) { return a.XY - b.XY; }

        #endregion

        #region API

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(Point2 point) { return _IsFinite(point.X) && _IsFinite(point.Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDominantAxis(Point2 point) { return point.DominantAxis; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Center(System.Drawing.Rectangle rect)
        {
            return new VECTOR2(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Center(System.Drawing.RectangleF rect)
        {
            return new VECTOR2(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Lerp(Point2 a, Point2 b, float amount)
        {
            return VECTOR2.Lerp(a.XY, b.XY, amount);
        }

        /// <summary>
        /// Calculates the angle between two vectors.
        /// </summary>
        /// <param name="a">the first vector.</param>
        /// <param name="b">the second vector.</param>
        /// <returns>The angle, in radians.</returns>
        public static float AngleInRadians(Point2 a, Point2 b)
        {
            var dot = VECTOR2.Dot(a.Normalized,b.Normalized);
            if (float.IsNaN(dot)) return 0;
            dot = Math.Min(dot, 1);
            dot = Math.Max(dot, -1);
            #if NETSTANDARD2_1_OR_GREATER
            return MathF.Acos(dot);
            #else
            return  (float)Math.Acos(dot);
            #endif
        }

        /// <summary>
        /// Calculates the Z value of the cross product between two 2D points
        /// </summary>
        /// <param name="a">the first vector.</param>
        /// <param name="b">the second vector.</param>
        /// <returns>The value of Z as if A and B would be 3D vectors.</returns>
        /// <remarks>
        /// <para>
        /// this is equivalent to:<br/>
        /// <c>result = Vector3.Cross(new Vector3(a.X,a.Y,0), new Vector3(b.X,b.Y,0) ).Z;</c>
        /// </para>
        /// <para>The sign of the result represents the "winding" of the axis.</para>
        /// <para>The area can be evaluated as: <c>Abs(result)/2;</c></para>        
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cross(Point2 a, Point2 b)
        {
            return a.X * b.Y -a.Y * b.X;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cross(Point2 a, Point2 b, Point2 c)
        {
            return Cross(b-a, c-b);
        }

        public VECTOR2 WithLength(float len)
        {
            len /= XY.Length();
            return XY * len;
        }

        public static bool IsClosedLoop(ReadOnlySpan<Point2> points) { return points[0] == points[points.Length - 1]; }


        /// <summary>
        /// Gets the point of the segment defined by <paramref name="a"/> - <paramref name="b"/> closest to this point.
        /// </summary>
        /// <param name="a">The begin point of the segment.</param>
        /// <param name="b">The end point of the segment.</param>
        /// <returns>the point of the segment</returns>
        public VECTOR2 GetClosestSegmentPoint(Point2 a, Point2 b)
        {
            var v = b - a;
            var u = VECTOR2.Dot(this.XY - a, v) / v.LengthSquared();

            u = Math.Max(0, u);
            u = Math.Min(1, u);            

            var linePoint = a + u * v;

            return linePoint;
        }

        #endregion

        #region API - Centroid

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Centroid(Point2[] points) { return Centroid(points.AsSpan()); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Centroid(VECTOR2[] points) { return Centroid(points.AsSpan()); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Centroid(ReadOnlySpan<Point2> points)
        {
            if (points.Length == 0) return VECTOR2.Zero;

            var r = VECTOR2.Zero;
            foreach (var p in points) { r += p.XY; }
            return r / points.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Centroid(ReadOnlySpan<VECTOR2> points)
        {
            if (points.Length == 0) return VECTOR2.Zero;

            var r = VECTOR2.Zero;
            foreach (var p in points) { r += p; }
            return r / points.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Centroid(IEnumerable<Point2> points)
        {
            var weight = 1;
            return points.Aggregate(VECTOR2.Zero, (i, j) => { ++weight; return i + j.XY; }) / weight;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Centroid(IEnumerable<VECTOR2> points)
        {
            var weight = 1;
            return points.Aggregate((i, j) => { ++weight; return i + j; }) / weight;
        }

        #endregion

        #region API - Bulk

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<Point2> AsPoints(Span<VECTOR2> points) { return MEMMARSHALL.Cast<VECTOR2, Point2>(points); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<Point2> AsPoints(ReadOnlySpan<VECTOR2> points) { return MEMMARSHALL.Cast<VECTOR2, Point2>(points); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<VECTOR2> AsNumerics(Span<Point2> points) { return MEMMARSHALL.Cast<Point2, VECTOR2>(points); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<VECTOR2> AsNumerics(ReadOnlySpan<Point2> points) { return MEMMARSHALL.Cast<Point2, VECTOR2>(points); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Transform(Point2 p, in System.Numerics.Matrix3x2 xform) { return VECTOR2.Transform(p.XY, xform); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Transform(float x, float y, in System.Numerics.Matrix3x2 xform) { return VECTOR2.Transform(new VECTOR2(x, y), xform); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Span<Point2> points, in System.Numerics.Matrix3x2 xform)
        {
            var v2 = AsNumerics(points);

            for(int i =0; i < points.Length; ++i)
            {
                v2[i] = VECTOR2.Transform(v2[i], xform);
            }
        }        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformNormals(Span<Point2> points, in System.Numerics.Matrix3x2 xform)
        {
            var v2 = AsNumerics(points);

            for (int i = 0; i < points.Length; ++i)
            {
                v2[i] = VECTOR2.TransformNormal(v2[i], xform);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(ReadOnlySpan<Point2> src, Span<Point2> dst, in System.Numerics.Matrix3x2 xform)
        {
            var count = Math.Min(src.Length, dst.Length);

            #if NET5_0_OR_GREATER

            ref var srcv = ref MEMMARSHALL.GetReference(AsNumerics(src));
            ref var dstv = ref AsNumerics(dst)[0];

            while (count-- > 0)
            {
                dstv = VECTOR2.Transform(srcv, xform);
                dstv = ref Unsafe.Add(ref dstv, 1);
                srcv = ref Unsafe.Add(ref srcv, 1);
            }

            #else

            var srcv = AsNumerics(src);
            var dstv = AsNumerics(dst);

            for(int i=0; i < count; ++i) { dstv[i] = VECTOR2.Transform(srcv[i], xform); }

            #endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformNormals(ReadOnlySpan<Point2> src, Span<Point2> dst, in System.Numerics.Matrix3x2 xform)
        {
            var count = Math.Min(src.Length, dst.Length);

            #if NET5_0_OR_GREATER

            ref var srcv = ref MEMMARSHALL.GetReference(AsNumerics(src));
            ref var dstv = ref AsNumerics(dst)[0];

            while (count-- > 0)
            {
                dstv = VECTOR2.TransformNormal(srcv, xform);
                dstv = ref Unsafe.Add(ref dstv, 1);
                srcv = ref Unsafe.Add(ref srcv, 1);
            }

            #else

            var srcv = AsNumerics(src);
            var dstv = AsNumerics(dst);

            for (int i = 0; i < count; ++i) { dstv[i] = VECTOR2.TransformNormal(srcv[i], xform); }

            #endif
        }

        #endregion

        #region conversions

        #if NET5_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref VECTOR2 AsNumerics(ref Point2 point)
        {
            return ref Unsafe.As<Point2, VECTOR2>(ref point);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref Point2 AsPoint(ref VECTOR2 point)
        {
            return ref Unsafe.As<VECTOR2, Point2>(ref point);
        }
        #endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GDIPOINTF ToGDIPoint() { return new GDIPOINTF(X, Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GDISIZEF ToGDISize() { return new GDISIZEF(X, Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VECTOR2 ToNumerics() { return XY; }        

        /// <inheritdoc/>
        public override string ToString() { return XY.ToString(); }

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider) { return XY.ToString(format, formatProvider); }

        #endregion

        #region nested types

        private sealed class _OrdinalIComparable : IComparer<Point2>
        {
            public int Compare(Point2 x, Point2 y)
            {
                var r = x.X.CompareTo(y.X);
                return r != 0 ? r : x.Y.CompareTo(y.Y);
            }
        }

#endregion
    }
}
