using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

using VECTOR3 = System.Numerics.Vector3;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a vector with three single-precision floating-point values.
    /// </summary>
    /// <remarks>
    /// Equivalent to <b>(float,float, float)</b> and <see cref="VECTOR3"/>.
    /// </remarks>
    
    [System.Diagnostics.DebuggerDisplay("{X} {Y} {Z}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    public readonly partial struct Point3
        : IFormattable
        , IEquatable<Point3>
        , IEquatable<VECTOR3>
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
        public static implicit operator Point3(VECTOR3 p) { return new Point3(p.X,p.Y,p.Z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point3((float X, float Y, float Z) p) { return new Point3(p.X, p.Y, p.Z); }

        #endregion

        #region constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3(float x, float y, float z) : this() { X = x; Y = y; Z = z; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3(Point2 xy, float z) : this() { X = xy.X; Y = xy.Y; Z = z; }

        #endregion

        #region data

        public static readonly Point3 Zero = new Point3(0, 0, 0);

        public static readonly Point3 Half = new Point3(0.5f, 0.5f, 0.5f);

        public static readonly Point3 One = new Point3(1, 1, 1);

        [System.Runtime.InteropServices.FieldOffset(0)]
        public readonly float X;
        [System.Runtime.InteropServices.FieldOffset(4)]
        public readonly float Y;
        [System.Runtime.InteropServices.FieldOffset(8)]
        public readonly float Z;

        [System.Runtime.InteropServices.FieldOffset(0)]
        public readonly VECTOR3 Vector;

        /// <inheritdoc/>
        public override int GetHashCode() => Vector.GetHashCode();

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Point3 otherP) return AreEqual(this, otherP);
            if (obj is VECTOR3 otherV) return AreEqual(this, otherV);            
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

        public static bool AreEqual(in Point3 a, in Point3 b) { return a.Vector == b.Vector; }

        public static bool AreEqual(in Point3 a, in VECTOR3 b) { return a.Vector == b; }

        #endregion

        #region properties

        public bool IsFinite => _IsFinite(X) && _IsFinite(Y) && _IsFinite(Z);

        public float Length => Vector.Length();

        #endregion

        #region operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3 operator *(Point3 a, float b) { return a.Vector * b; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3 operator +(Point3 a, Point3 b) { return a.Vector + b.Vector; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3 operator -(Point3 a, Point3 b) { return a.Vector - b.Vector; }

        #endregion

        #region API

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3 WithX(float x) { return new Point3(x, Y, Z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3 WithY(float y) { return new Point3(X, y, Z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3 WithZ(float z) { return new Point3(X, Y, z); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3 Normalized() { return VECTOR3.Normalize(new VECTOR3(X, Y, Z)); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2 SelectXY() { return new Point2(X, Y); }

        #endregion

        #region API - Bulk

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<Point3> AsPoints(Span<VECTOR3> points)
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<VECTOR3, Point3>(points);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<Point3> AsPoints(ReadOnlySpan<VECTOR3> points)
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<VECTOR3, Point3>(points);
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

        #endregion

        #region conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VECTOR3 ToNumerics() { return Vector; }

        /// <inheritdoc/>        
        public override string ToString() { return ToNumerics().ToString(); }

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider) { return ToNumerics().ToString(format, formatProvider); }

        #endregion
    }
}
