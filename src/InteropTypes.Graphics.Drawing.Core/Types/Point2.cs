using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
        , IEnumerable<Single>
        , IEquatable<Point2>
        , IEquatable<VECTOR2>
        , IEquatable<GDIPOINTF>
        , IEquatable<GDISIZEF>        
    {
        #region diagnostics

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(Point2 point) { return point.X.IsFinite() && point.Y.IsFinite(); }

        [System.Diagnostics.DebuggerStepThrough]
        [System.Diagnostics.Conditional("DEBUG")]
        public static void DebugAssertIsFinite(ReadOnlySpan<Point2> points)
        {
            foreach (var point in points) System.Diagnostics.Debug.Assert(Point2.IsFinite(point));
        }

        #endregion

        #region implicit

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2((double X, double Y) p)
        {
            #if NET5_0_OR_GREATER
            Unsafe.SkipInit<Point2>(out var pp);
            pp.X = (float)p.X;
            pp.Y = (float)p.Y;
            return pp;
            #else
            return new Point2(p.X, p.Y);
            #endif
        }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2(Random rnd) { return new Point2(rnd); }

        #endregion

        #region constructors

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2(Random rnd) : this()
        {
            if (rnd == null) throw new ArgumentNullException(nameof(rnd));

            #pragma warning disable CA5394 // Do not use insecure randomness
            #if NET6_0_OR_GREATER
            X = rnd.NextSingle();
            Y = rnd.NextSingle();
            #else
            X = (float)rnd.NextDouble();
            Y = (float)rnd.NextDouble();
            #endif
            #pragma warning restore CA5394 // Do not use insecure randomness
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2(double x, double y) : this() { X = (float)x; Y = (float)y; }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2(float x, float y) : this() { X = x; Y = y; }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2(VECTOR2 vector) : this() { XY = vector; }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2(GDIPOINT point) : this() { X = point.X; Y = point.Y; }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2(GDIPOINTF point) : this() { X = point.X; Y = point.Y; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2(ReadOnlySpan<float> span) : this() { X = span[0]; Y = span[1]; }        

        /// <summary>
        /// Gets the corner points of a rectangle, in a clockwise order.
        /// </summary>
        /// <param name="rect">The source rectangle</param>
        /// <param name="closed">defines whether to return 4 or 5 points</param>
        /// <returns>A collection of 4 or 5 points.</returns>        
        public static void FromRect(Span<Point2> points, System.Drawing.RectangleF rect, bool closed = false)
        {            
            points[0] = (rect.Left, rect.Top);
            points[1] = (rect.Right, rect.Top);
            points[2] = (rect.Right, rect.Bottom);
            points[3] = (rect.Left, rect.Bottom);
            if (closed && points.Length > 4) points[4] = points[0];           
        }

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
        public readonly override int GetHashCode() => XY.GetHashCode();

        /// <inheritdoc/>
        public readonly override bool Equals(object obj)
        {
            if (obj is Point2 otherP) return this.XY == otherP.XY;
            if (obj is VECTOR2 otherV) return this.XY == otherV;
            if (obj is GDIPOINTF otherGP) return AreEqual(this, otherGP);
            if (obj is GDISIZEF otherGS) return AreEqual(this, otherGS);            
            return false;
        }

        /// <inheritdoc/>
        public readonly bool Equals(Point2 other) => AreEqual(this, other);

        /// <inheritdoc/>
        public readonly bool Equals(VECTOR2 other) => AreEqual(this, other);

        /// <inheritdoc/>
        public readonly bool Equals(GDIPOINTF other) => AreEqual(this, other);

        /// <inheritdoc/>
        public readonly bool Equals(GDISIZEF other) => AreEqual(this, other);

        /// <inheritdoc/>
        public static bool operator ==(in Point2 a, Point2 b) => AreEqual(a, b);

        /// <inheritdoc/>
        public static bool operator !=(in Point2 a, Point2 b) => !AreEqual(a, b);

        /// <inheritdoc/>
        public static bool operator ==(in Point2 a, VECTOR2 b) => AreEqual(a, b);

        /// <inheritdoc/>
        public static bool operator !=(in Point2 a, VECTOR2 b) => !AreEqual(a, b);

        public static bool AreEqual(in Point2 a, in Point2 b) { return a.XY == b.XY; }        

        public static bool AreEqual(in Point2 a, in Point2 b, float tolerance)
        {
            return VECTOR2.Distance(a.XY, b.XY) <= tolerance;
        }

        #endregion

        #region properties

        public static Point2 Zero => VECTOR2.Zero;
        public static Point2 Half => new Point2(0.5f, 0.5f);
        public static Point2 One => VECTOR2.One;
        public static Point2 UnitX => VECTOR2.UnitX;
        public static Point2 UnitY => VECTOR2.UnitY;
        public readonly float Length => XY.Length();
        
        public readonly int DominantAxis
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
        public static VECTOR2 operator *(Point2 a, Point2 b) { return a.XY * b.XY; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 operator /(Point2 a, Point2 b) { return a.XY / b.XY; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 operator +(Point2 a, Point2 b) { return a.XY + b.XY; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 operator -(Point2 a, Point2 b) { return a.XY - b.XY; }

        #endregion

        #region API        

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
            var dot = VECTOR2.Dot(a.Normalized(), b.Normalized());
            if (float.IsNaN(dot)) return 0;
            dot = Math.Min(dot, 1);
            dot = Math.Max(dot, -1);
            #if NETSTANDARD2_0
            return  (float)Math.Acos(dot);
            #else
            return MathF.Acos(dot);            
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

        public readonly VECTOR2 Normalized()
        {
            return VECTOR2.Normalize(this.XY);
        }

        public readonly VECTOR2 Normalized(out float length)
        {
            length = this.XY.Length();
            return this.XY / length;
        }

        public static VECTOR2 Normalize(Point2 value, out float length)
        {
            length = value.XY.Length();
            return value.XY / length;
        }

        public readonly VECTOR2 WithLength(float len)
        {
            len /= XY.Length();
            return XY * len;
        }

        /// <summary>
        /// Gets the point of the segment defined by <paramref name="a"/> - <paramref name="b"/> closest to this point.
        /// </summary>
        /// <param name="a">The begin point of the segment.</param>
        /// <param name="b">The end point of the segment.</param>
        /// <returns>the point of the segment</returns>
        public readonly VECTOR2 GetClosestSegmentPoint(Point2 a, Point2 b)
        {
            var v = b - a;
            var u = VECTOR2.Dot(this.XY - a, v) / v.LengthSquared();

            #if NETSTANDARD2_0            
            u = Math.Max(0, u);
            u = Math.Min(1, u);
            #else
            u = Math.Clamp(u, 0, 1);
            #endif                        

            var linePoint = a + u * v;

            return linePoint;
        }

        #endregion       

        #region API - Bulk        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<float> dst) { dst[0] = X; dst[1] = Y; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<Point2> dst) { dst[0] = this; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<VECTOR2> dst) { dst[0] = this.XY; }        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Transform(Point2 p, in System.Numerics.Matrix3x2 xform) { return VECTOR2.Transform(p.XY, xform); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Transform(float x, float y, in System.Numerics.Matrix3x2 xform) { return VECTOR2.Transform(new VECTOR2(x, y), xform); }        

        #endregion

        #region conversions

        #if NET6_0_OR_GREATER

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref VECTOR2 AsNumerics(ref Point2 point) { return ref Unsafe.As<Point2, VECTOR2>(ref point); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref Point2 AsPoint(ref VECTOR2 point) { return ref Unsafe.As<VECTOR2, Point2>(ref point); }

        #endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly GDIPOINTF ToGDIPoint() { return new GDIPOINTF(X, Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly GDISIZEF ToGDISize() { return new GDISIZEF(X, Y); }        

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
