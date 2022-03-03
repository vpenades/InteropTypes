using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

using MEMMARSHALL = System.Runtime.InteropServices.MemoryMarshal;

using VECTOR2 = System.Numerics.Vector2;
using VECTOR3 = System.Numerics.Vector3;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents an (X,Y,Z) point with three single-precision floating-point values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="Point3"/> is not a replacement of <see cref="VECTOR3"/> but a helper
    /// structure used to simplify the use of (X,Y,Z) values.
    /// </para>
    /// <para>
    /// Equivalent to:<br/>
    /// <list type="table">
    /// <item><b>(float,float,float)</b></item>
    /// <item><see cref="VECTOR3"/></item>    
    /// </list>
    /// </para>
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{XYZ}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    public partial struct Point3
        : IFormattable
        , IEquatable<Point3>
        , IEquatable<VECTOR3>
    {
        #region diagnostics

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(Point3 point) { return point.X.IsFinite() && point.Y.IsFinite() && point.Z.IsFinite(); }

        [System.Diagnostics.DebuggerStepThrough]
        [System.Diagnostics.Conditional("DEBUG")]
        public static void DebugAssertIsFinite(ReadOnlySpan<Point3> points)
        {
            foreach (var point in points) System.Diagnostics.Debug.Assert(Point3.IsFinite(point));
        }

        #endregion

        #region implicit

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point3(VECTOR3 p)
        {
            #if NET5_0_OR_GREATER
            Unsafe.SkipInit<Point3>(out var pp);
            pp.XYZ = p;
            return pp;
            #else
            return new Point3(p);
            #endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point3((float X, float Y, float Z) p)
        {
            #if NET5_0_OR_GREATER
            Unsafe.SkipInit<Point3>(out var pp);
            pp.X = p.X;
            pp.Y = p.Y;
            pp.Z = p.Z;
            return pp;
            #else
            return new Point3(p.X, p.Y, p.Z);
            #endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point3((Point2 XY, float Z) p)
        {
            #if NET5_0_OR_GREATER
            Unsafe.SkipInit<Point3>(out var pp);
            pp.XY = p.XY.XY;            
            pp.Z = p.Z;
            return pp;
            #else
            return new Point3(p.XY, p.Z);
            #endif
        }

        #endregion

        #region constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3(float x, float y, float z) : this() { X = x; Y = y; Z = z; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3(VECTOR3 v) : this() { XYZ = v; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3(Point2 xy, float z) : this() { XY = xy.XY; Z = z; }        

        /// <summary>
        /// Helper method used to convert point params to an array.
        /// </summary>
        /// <param name="points">a sequence of points</param>
        /// <returns>An array of points</returns>
        /// <remarks>
        /// When a function has a <see cref="ReadOnlySpan{Point2}"/> we can
        /// pass a Point2.Params(...) instead.
        /// </remarks>
        public static Point3[] Array(params Point3[] points) { return points; }

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
        /// The Z component of the point.
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(8)]
        public float Z;        

        /// <summary>
        /// The X and Y component of the point.
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        public VECTOR2 XY;

        /// <summary>
        /// The Y and Z component of the point.
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(4)]
        public VECTOR2 YZ;

        /// <summary>
        /// The X, Y and Z component of the point.
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        public VECTOR3 XYZ;

        /// <inheritdoc/>
        public override int GetHashCode() => XYZ.GetHashCode();

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Point3 otherP) return this.XYZ == otherP.XYZ;
            if (obj is VECTOR3 otherV) return this.XYZ == otherV;
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(Point3 other) => AreEqual(this, other);

        /// <inheritdoc/>
        public bool Equals(VECTOR3 other) => AreEqual(this, other);

        /// <inheritdoc/>
        public static bool operator ==(in Point3 a, in Point3 b) => AreEqual(a, b);

        /// <inheritdoc/>
        public static bool operator !=(in Point3 a, in Point3 b) => !AreEqual(a, b);

        /// <inheritdoc/>
        public static bool operator ==(in Point3 a, in VECTOR3 b) => AreEqual(a, b);

        /// <inheritdoc/>
        public static bool operator !=(in Point3 a, in VECTOR3 b) => !AreEqual(a, b);

        public static bool AreEqual(in Point3 a, in Point3 b) { return a.XYZ == b.XYZ; }

        public static bool AreEqual(in Point3 a, in VECTOR3 b) { return a.XYZ == b; }

        #endregion

        #region properties

        public static Point3 Zero => VECTOR3.Zero;
        public static Point3 Half => new Point3(0.5f, 0.5f, 0.5f);
        public static Point3 One => VECTOR3.One;
        public static Point3 UnitX => VECTOR3.UnitX;
        public static Point3 UnitY => VECTOR3.UnitY;
        public static Point3 UnitZ => VECTOR3.UnitZ;        

        public float Length => XYZ.Length();
        
        public VECTOR3 Normalized => VECTOR3.Normalize(this.XYZ);

        public int DominantAxis
        {
            get
            {
                var t = VECTOR3.Abs(this.XYZ);
                return t.X >= t.Y ? t.X >= t.Z ? 0 : 2 : t.Y >= t.Z ? 1 : 2;
            }
        }

        #endregion

        #region operators

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 operator -(Point3 a) { return -a.XYZ; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 operator *(Point3 a, float b) { return a.XYZ * b; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 operator /(Point3 a, float b) { return a.XYZ / b; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 operator *(Point3 a, Point3 b) { return a.XYZ * b.XYZ; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 operator /(Point3 a, Point3 b) { return a.XYZ / b.XYZ; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 operator +(Point3 a, Point3 b) { return a.XYZ + b.XYZ; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 operator -(Point3 a, Point3 b) { return a.XYZ - b.XYZ; }

        #endregion

        #region API        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDominantAxis(Point3 point) { return point.DominantAxis; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 Lerp(Point3 a, Point3 b, float amount)
        {
            return VECTOR3.Lerp(a.XYZ, b.XYZ, amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 Cross(Point3 a, Point3 b, Point3 c)
        {
            return VECTOR3.Cross(b.XYZ - a.XYZ, c.XYZ - b.XYZ);
        }

        /// <summary>
        /// Calculates the angle between two vectors.
        /// </summary>
        /// <param name="a">the first vector.</param>
        /// <param name="b">the second vector.</param>
        /// <returns>The angle, in radians.</returns>
        public static float AngleInRadians(Point3 a, Point3 b)
        {
            var dot = VECTOR3.Dot(a.Normalized, b.Normalized);
            if (float.IsNaN(dot)) return 0;
            dot = Math.Min(dot, 1);
            dot = Math.Max(dot, -1);
            #if NETSTANDARD2_1_OR_GREATER
            return MathF.Acos(dot);
            #else
            return (float)Math.Acos(dot);
            #endif
        }

        public VECTOR3 WithLength(float len)
        {
            len /= XYZ.Length();
            return XYZ * len;
        }

        public static bool IsClosedLoop(ReadOnlySpan<Point3> points) { return points[0] == points[points.Length - 1]; }

        #endregion

        #region API - Centroid

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 Centroid(Point3[] points) { return Centroid(points.AsSpan()); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 Centroid(VECTOR3[] points) { return Centroid(points.AsSpan()); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 Centroid(ReadOnlySpan<Point3> points)
        {
            if (points.Length == 0) return VECTOR3.Zero;

            var r = VECTOR3.Zero;
            foreach (var p in points) { r += p.XYZ; }
            return r / points.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 Centroid(ReadOnlySpan<VECTOR3> points)
        {
            if (points.Length == 0) return VECTOR3.Zero;

            var r = VECTOR3.Zero;
            foreach (var p in points) { r += p; }
            return r / points.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 Centroid(IEnumerable<Point3> points)
        {
            var weight = 1;
            return points.Aggregate(VECTOR3.Zero, (i, j) => { ++weight; return i + j.XYZ; }) / weight;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 Centroid(IEnumerable<VECTOR3> points)
        {
            var weight = 1;
            return points.Aggregate((i, j) => { ++weight; return i + j; }) / weight;
        }

        #endregion

        #region API - Bulk

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<Point3> AsPoints(Span<VECTOR3> points) { return MEMMARSHALL.Cast<VECTOR3, Point3>(points); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<Point3> AsPoints(ReadOnlySpan<VECTOR3> points) { return MEMMARSHALL.Cast<VECTOR3, Point3>(points); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<VECTOR3> AsNumerics(Span<Point3> points) { return MEMMARSHALL.Cast<Point3, VECTOR3>(points); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<VECTOR3> AsNumerics(ReadOnlySpan<Point3> points) { return MEMMARSHALL.Cast<Point3, VECTOR3>(points); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3 Transform(Point3 p, in System.Numerics.Matrix4x4 xform)
        {
            return VECTOR3.Transform(p.ToNumerics(), xform);
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

        #region conversions

        #if NET5_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref VECTOR3 AsNumerics(ref Point3 point)
        {
            return ref Unsafe.As<Point3, VECTOR3>(ref point);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref Point3 AsPoint(ref VECTOR3 point)
        {
            return ref Unsafe.As<VECTOR3, Point3>(ref point);
        }
        #endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VECTOR3 ToNumerics() { return XYZ; }

        /// <inheritdoc/>        
        public override string ToString() { return XYZ.ToString(); }

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider) { return XYZ.ToString(format, formatProvider); }

        #endregion
    }
}
