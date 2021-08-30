using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

using VECTOR2 = System.Numerics.Vector2;
using GDIPOINT = System.Drawing.Point;
using GDIPOINTF = System.Drawing.PointF;
using GDISIZE = System.Drawing.Size;
using GDISIZEF = System.Drawing.SizeF;

using VECTOR3 = System.Numerics.Vector3;
using System.Linq;

namespace InteropDrawing
{
    /// <summary>
    /// Represents a vector with two single-precision floating-point values.
    /// </summary>
    /// <remarks>
    /// Equivalent to <b>(float,float)</b> <see cref="VECTOR2"/>, <see cref="GDIPOINTF"/> and <see cref="GDISIZEF"/>.
    /// </remarks>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    [System.Diagnostics.DebuggerDisplay("{X} {Y}")]
    public readonly struct Point2
        : IEquatable<Point2>
        , IEquatable<VECTOR2>
        , IEquatable<GDIPOINTF>
        , IEquatable<GDISIZEF>
    {
        #region implicit

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2((float X, float Y) p) { return new Point2(p.X, p.Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2(VECTOR2 p) { return new Point2(p.X, p.Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2(GDIPOINT p) { return new Point2(p.X, p.Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2(GDIPOINTF p) { return new Point2(p.X, p.Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2(GDISIZE p) { return new Point2(p.Width, p.Height); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2(GDISIZEF p) { return new Point2(p.Width, p.Height); }

        #endregion

        #region constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2(float x, float y) { X = x;Y = y; }

        #endregion

        #region data

        public static readonly Point2 Zero = new Point2(0, 0);

        public readonly float X;
        public readonly float Y;

        /// <inheritdoc/>
        public override int GetHashCode() => (X, Y).GetHashCode();

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Point2 otherP) return AreEqual(this, otherP);
            if (obj is VECTOR2 otherV) return AreEqual(this, otherV);
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


        public static bool AreEqual(in Point2 a, in Point2 b) { return a.X == b.X && a.Y == b.Y; }
        public static bool AreEqual(in Point2 a, in VECTOR2 b) { return a.X == b.X && a.Y == b.Y; }
        public static bool AreEqual(in Point2 a, in GDIPOINTF b) { return a.X == b.X && a.Y == b.Y; }
        public static bool AreEqual(in Point2 a, in GDISIZEF b) { return a.X == b.Width && a.Y == b.Height; }

        #endregion

        #region properties

        public bool IsReal => X.IsFinite() && Y.IsFinite();

        #endregion

        #region operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 operator *(Point2 a, Single b) { return new Point2(a.X * b, a.Y * b); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 operator /(Point2 a, Single b) { return new Point2(a.X / b, a.Y / b); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 operator +(Point2 a, Point2 b) { return new Point2(a.X + b.X, a.Y + b.Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 operator -(Point2 a, Point2 b) { return new Point2(a.X - b.X, a.Y - b.Y); }

        #endregion

        #region API

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2 WithX(float x) { return new Point2(x, this.Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2 WithY(float y) { return new Point2(this.X, y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VECTOR2 ToNumerics() { return new VECTOR2(X, Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GDIPOINTF ToGDIPoint() { return new GDIPOINTF(X, Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GDISIZEF ToGDISize() { return new GDISIZEF(X, Y); }

        public System.Drawing.RectangleF ToGDIRectangleOffCenter(float size)
        {
            return new System.Drawing.RectangleF(this.X - size / 2f, this.Y - size / 2f, size, size);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 Center(System.Drawing.Rectangle rect)
        {
            return new Point2(rect.X + (rect.Width / 2f), rect.Y + (rect.Height / 2f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 Centroid(IEnumerable<Point2> points)
        {            
            var weight = 1;
            return points.Aggregate((i, j) => { ++weight; return i + j; }) / weight;            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR2 Centroid(IEnumerable<VECTOR2> points)
        {
            var weight = 1;
            return points.Aggregate((i, j) => { ++weight; return i + j; }) / weight;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 Center(System.Drawing.RectangleF rect)
        {
            return new Point2(rect.X + (rect.Width / 2f), rect.Y + (rect.Height / 2f));
        }

        public static Point2 Lerp(Point2 a, Point2 b, float amount)
        {
            return VECTOR2.Lerp(a.ToNumerics(), b.ToNumerics(), amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 Transform(Point2 p, in System.Numerics.Matrix3x2 xform)
        {
            return VECTOR2.Transform(p.ToNumerics(), xform);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 Transform(float x, float y, in System.Numerics.Matrix3x2 xform)
        {
            return VECTOR2.Transform(new VECTOR2(x, y), xform);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<VECTOR2> AsNumerics(Span<Point2> points)
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Point2, VECTOR2>(points);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<VECTOR2> AsNumerics(ReadOnlySpan<Point2> points)
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Point2, VECTOR2>(points);
        }

        public override string ToString() { return ToNumerics().ToString(); }        

        #endregion
    }

    /// <summary>
    /// Represents a vector with three single-precision floating-point values.
    /// </summary>
    /// <remarks>
    /// Equivalent to <b>(float,float, float)</b> and <see cref="VECTOR3"/>.
    /// </remarks>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    [System.Diagnostics.DebuggerDisplay("{X} {Y} {Z}")]
    public readonly struct Point3
    {
        #region implicit
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point3(VECTOR3 p) { return new Point3(p.X, p.Y, p.Z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point3((float X, float Y, float Z) p) { return new Point3(p.X, p.Y, p.Z); }

        #endregion

        #region constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3(float x, float y, float z) { X = x; Y = y; Z = z; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3(Point2 xy, float z) { X = xy.X; Y = xy.Y; Z = z; }

        #endregion

        #region data

        public static readonly Point3 Zero = new Point3(0, 0, 0);

        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        /// <inheritdoc/>
        public override int GetHashCode() => (X, Y, Z).GetHashCode();

        /// <inheritdoc/>
        public bool Equals(Point3 other) => AreEqual(this, other);

        /// <inheritdoc/>
        public bool Equals(VECTOR3 other) => AreEqual(this, other);

        /// <inheritdoc/>
        public static bool operator ==(in Point3 a, in Point3 b) => AreEqual(a, b);
        /// <inheritdoc/>
        public static bool operator !=(in Point3 a, in Point3 b) => !AreEqual(a, b);

        public static bool AreEqual(in Point3 a, in Point3 b) { return a.X == b.X && a.Y == b.Y && a.Z == b.Z; }
        public static bool AreEqual(in Point3 a, in VECTOR3 b) { return a.X == b.X && a.Y == b.Y && a.Z == b.Z; }

        #endregion

        #region properties

        public bool IsReal => X.IsFinite() && Y.IsFinite() && Z.IsFinite();

        #endregion

        #region operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3 operator *(Point3 a, Single b) { return new Point3(a.X * b, a.Y * b, a.Z * b); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3 operator +(Point3 a, Point3 b) { return new Point3(a.X + b.X, a.Y + b.Y, a.Z+b.Z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3 operator -(Point3 a, Point3 b) { return new Point3(a.X - b.X, a.Y - b.Y, a.Z - b.Z); }

        #endregion

        #region API

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3 WithX(float x) { return new Point3(x, this.Y, this.Z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3 WithY(float y) { return new Point3(this.X, y, this.Z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3 WithZ(float z) { return new Point3(this.X, this.Y, z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3 Normalized() { return VECTOR3.Normalize(new VECTOR3(X, Y, Z)); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2 SelectXY() { return new Point2(X, Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VECTOR3 ToNumerics() { return new VECTOR3(X, Y, Z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3 Transform(Point3 p, in System.Numerics.Matrix4x4 xform)
        {
            return VECTOR3.Transform(p.ToNumerics(), xform);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<VECTOR3> AsNumerics(Span<Point3> points)
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Point3, VECTOR3>(points);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<VECTOR3> AsNumerics(ReadOnlySpan<Point3> points)
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<Point3, VECTOR3>(points);
        }

        public override string ToString() { return ToNumerics().ToString(); }

        #endregion
    }
}
