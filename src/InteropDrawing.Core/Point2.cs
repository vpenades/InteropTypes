using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

using VECTOR2 = System.Numerics.Vector2;
using GDIPOINT = System.Drawing.Point;
using GDIPOINTF = System.Drawing.PointF;
using GDISIZE = System.Drawing.Size;

/* Unmerged change from project 'InteropDrawing.Core (netstandard2.1)'
Before:
using GDISIZEF = System.Drawing.SizeF;
After:
using GDISIZEF = System.Drawing.SizeF;
using InteropDrawing;
using InteropTypes.Graphics.Drawing;
*/
using GDISIZEF = System.Drawing.SizeF;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a vector with two single-precision floating-point values.
    /// </summary>
    /// <remarks>
    /// Equivalent to <b>(float,float)</b> <see cref="VECTOR2"/>, <see cref="GDIPOINTF"/> and <see cref="GDISIZEF"/>.
    /// </remarks>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    [System.Diagnostics.DebuggerDisplay("{X} {Y}")]
    public readonly struct Point2 : IFormattable
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
        /// <see href="https://github.com/dotnet/runtime/blob/5df6cc63151d937724fa0ce8138e69f933052606/src/libraries/System.Private.CoreLib/src/System/Single.cs#L74">DotNet implementation</see>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool _IsFinite(float val)
        {
            return !float.IsNaN(val) && !float.IsInfinity(val);
        }

        #endregion

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
        public Point2(float x, float y) : this() { X = x; Y = y; }

        #endregion

        #region data

        public static readonly Point2 Zero = new Point2(0, 0);

        public static readonly Point2 Half = new Point2(0.5f, 0.5f);

        public static readonly Point2 One = new Point2(1, 1);

        [System.Runtime.InteropServices.FieldOffset(0)]
        public readonly float X;
        [System.Runtime.InteropServices.FieldOffset(4)]
        public readonly float Y;

        [System.Runtime.InteropServices.FieldOffset(0)]
        public readonly VECTOR2 Vector;

        /// <inheritdoc/>
        public override int GetHashCode() => Vector.GetHashCode();

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


        public static bool AreEqual(in Point2 a, in Point2 b) { return a.Vector == b.Vector; }

        public static bool AreEqual(in Point2 a, in VECTOR2 b) { return a.Vector == b; }

        public static bool AreEqual(in Point2 a, in GDIPOINTF b) { return a.X == b.X && a.Y == b.Y; }

        public static bool AreEqual(in Point2 a, in GDISIZEF b) { return a.X == b.Width && a.Y == b.Height; }

        #endregion

        #region properties

        public bool IsFinite => _IsFinite(X) && _IsFinite(Y);

        #endregion

        #region operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 operator *(Point2 a, float b) { return a.Vector * b; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 operator /(Point2 a, float b) { return a.Vector / b; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 operator +(Point2 a, Point2 b) { return a.Vector + b.Vector; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 operator -(Point2 a, Point2 b) { return a.Vector - b.Vector; }

        #endregion

        #region API

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2 WithX(float x) { return new Point2(x, Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2 WithY(float y) { return new Point2(X, y); }
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 Center(System.Drawing.Rectangle rect)
        {
            return new Point2(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
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
            return new Point2(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
        }

        public static Point2 Lerp(Point2 a, Point2 b, float amount)
        {
            return VECTOR2.Lerp(a.ToNumerics(), b.ToNumerics(), amount);
        }

        #endregion

        #region API - Bulk

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 Transform(Point2 p, in System.Numerics.Matrix3x2 xform)
        {
            return VECTOR2.Transform(p.Vector, xform);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2 Transform(float x, float y, in System.Numerics.Matrix3x2 xform)
        {
            return VECTOR2.Transform(new VECTOR2(x, y), xform);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transform(Span<Point2> points, in System.Numerics.Matrix3x2 xform)
        {
            var v2 = AsNumerics(points);

            while (v2.Length > 0)
            {
                v2[0] = VECTOR2.Transform(v2[0], xform);
                v2 = v2.Slice(1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformNormals(Span<Point2> points, in System.Numerics.Matrix3x2 xform)
        {
            var v2 = AsNumerics(points);

            while (v2.Length > 0)
            {
                v2[0] = VECTOR2.TransformNormal(v2[0], xform);
                v2 = v2.Slice(1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<Point2> AsPoints(Span<VECTOR2> points)
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<VECTOR2, Point2>(points);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<Point2> AsPoints(ReadOnlySpan<VECTOR2> points)
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<VECTOR2, Point2>(points);
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

        #endregion

        #region conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GDIPOINTF ToGDIPoint() { return new GDIPOINTF(X, Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GDISIZEF ToGDISize() { return new GDISIZEF(X, Y); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VECTOR2 ToNumerics() { return new VECTOR2(X, Y); }

        public System.Drawing.RectangleF ToGDIRectangleOffCenter(float size)
        {
            return new System.Drawing.RectangleF(X - size / 2f, Y - size / 2f, size, size);
        }

        /// <inheritdoc/>
        public override string ToString() { return ToNumerics().ToString(); }

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider) { return ToNumerics().ToString(format, formatProvider); }

        #endregion
    }
}
