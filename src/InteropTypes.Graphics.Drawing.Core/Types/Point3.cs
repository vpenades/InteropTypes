using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1036:Override methods on comparable types", Justification = "The API would be misleading")]
    public partial struct Point3
        : IFormattable
        , IEnumerable<Single>
        , IEquatable<Point3>
        , IEquatable<XYZ>
        , IComparable<BoundingSphere>
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
        public static implicit operator Point3(XYZ p)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point3(Random rnd) { return new Point3(rnd); }

        #endregion

        #region constructors

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3(Random rnd) : this()
        {
            if (rnd == null) throw new ArgumentNullException(nameof(rnd));

            #pragma warning disable CA5394 // Do not use insecure randomness
            #if NET6_0_OR_GREATER
            X = rnd.NextSingle();
            Y = rnd.NextSingle();
            Z = rnd.NextSingle();
            #else
            X = (float)rnd.NextDouble();
            Y = (float)rnd.NextDouble();
            Z = (float)rnd.NextDouble();
            #endif
            #pragma warning restore CA5394 // Do not use insecure randomness
        }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3(float x, float y, float z) : this() { X = x; Y = y; Z = z; }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3(XYZ v) : this() { XYZ = v; }

        [System.Diagnostics.DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3(Point2 xy, float z) : this() { XY = xy.XY; Z = z; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3(ReadOnlySpan<float> span) : this() { X = span[0]; Y = span[1]; Z = span[2]; }

         

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
        public XY XY;

        /// <summary>
        /// The Y and Z components of the point.
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(4)]
        public XY YZ;        

        /// <summary>
        /// The X, Y and Z components of the point.
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        public XYZ XYZ;

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public readonly void Deconstruct(out float x, out float y, out float z)
        {
            x = this.X;
            y = this.Y;
            z = this.Z;
        }

        /// <inheritdoc/>
        public readonly override int GetHashCode() => XYZ.GetHashCode();

        /// <inheritdoc/>
        public readonly override bool Equals(object obj)
        {
            if (obj is Point3 otherP) return this.XYZ == otherP.XYZ;
            if (obj is XYZ otherV) return this.XYZ == otherV;
            return false;
        }

        /// <inheritdoc/>
        public readonly bool Equals(Point3 other) => AreEqual(this, other);

        /// <inheritdoc/>
        public readonly bool Equals(XYZ other) => AreEqual(this, other);

        /// <inheritdoc/>
        public static bool operator ==(in Point3 a, in Point3 b) => AreEqual(a, b);

        /// <inheritdoc/>
        public static bool operator !=(in Point3 a, in Point3 b) => !AreEqual(a, b);

        /// <inheritdoc/>
        public static bool operator ==(in Point3 a, in XYZ b) => AreEqual(a, b);

        /// <inheritdoc/>
        public static bool operator !=(in Point3 a, in XYZ b) => !AreEqual(a, b);

        public static bool AreEqual(in Point3 a, in Point3 b) { return a.XYZ == b.XYZ; }        

        public static bool AreEqual(in Point3 a, in Point3 b, float tolerance)
        {
            return XYZ.Distance(a.XYZ, b.XYZ) <= tolerance;
        }

        #endregion

        #region properties

        public static Point3 Zero => XYZ.Zero;
        public static Point3 Half => new Point3(0.5f, 0.5f, 0.5f);
        public static Point3 One => XYZ.One;
        public static Point3 UnitX => XYZ.UnitX;
        public static Point3 UnitY => XYZ.UnitY;
        public static Point3 UnitZ => XYZ.UnitZ;

        public readonly float Length => XYZ.Length();

        /// <summary>
        /// The X and Z components of the point.
        /// </summary>
        public readonly XY XZ => new XY(X, Z);

        public readonly int DominantAxis
        {
            get
            {
                var t = XYZ.Abs(this.XYZ);
                return t.X >= t.Y ? t.X >= t.Z ? 0 : 2 : t.Y >= t.Z ? 1 : 2;
            }
        }

        #endregion

        #region operators

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ operator -(Point3 a) { return -a.XYZ; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ operator *(Point3 a, float b) { return a.XYZ * b; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ operator /(Point3 a, float b) { return a.XYZ / b; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ operator *(Point3 a, Point3 b) { return a.XYZ * b.XYZ; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ operator /(Point3 a, Point3 b) { return a.XYZ / b.XYZ; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ operator +(Point3 a, Point3 b) { return a.XYZ + b.XYZ; }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ operator -(Point3 a, Point3 b) { return a.XYZ - b.XYZ; }

        #endregion

        #region API        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDominantAxis(Point3 point) { return point.DominantAxis; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ Lerp(Point3 a, Point3 b, float amount)
        {
            return XYZ.Lerp(a.XYZ, b.XYZ, amount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XYZ Cross(Point3 a, Point3 b, Point3 c)
        {
            return XYZ.Cross(b.XYZ - a.XYZ, c.XYZ - b.XYZ);
        }

        /// <summary>
        /// Calculates the angle between two vectors.
        /// </summary>
        /// <param name="a">the first vector.</param>
        /// <param name="b">the second vector.</param>
        /// <returns>The angle, in radians.</returns>
        public static float AngleInRadians(Point3 a, Point3 b)
        {
            var dot = XYZ.Dot(a.Normalized(), b.Normalized());
            if (float.IsNaN(dot)) return 0;
            dot = Math.Min(dot, 1);
            dot = Math.Max(dot, -1);
            
            return MathF.Acos(dot);            
        }

        public readonly XYZ Normalized()
        {
            return XYZ.Normalize(this.XYZ);
        }

        public readonly XYZ Normalized(out float length)
        {
            length = this.XYZ.Length();
            return this.XYZ / length;
        }

        public static XYZ Normalize(Point3 value, out float length)
        {
            length = value.XYZ.Length();
            return value.XYZ / length;
        }

        public readonly XYZ WithLength(float len)
        {
            len /= XYZ.Length();
            return XYZ * len;
        }        

        #endregion        

        #region boundings

        /// <summary>
        /// Compares this point against a sphere.
        /// </summary>
        /// <param name="other">the sphere to compare against</param>
        /// <returns>
        /// -1 if inside <paramref name="other"/> sphere.<br/>
        /// 0 if overlapping <paramref name="other"/> sphere.<br/>
        /// 1 if outside <paramref name="other"/> sphere.<br/>
        /// </returns>
        public readonly int CompareTo(BoundingSphere other)
        {
            return XYZ
                .Distance(this.XYZ, other.Center)
                .CompareTo(other.Radius);
        }

        #endregion

        #region API - Bulk        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<float> dst) { dst[0] = X; dst[1] = Y; dst[2] = Z; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<Point3> dst) { dst[0] = this; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(Span<XYZ> dst) { dst[0] = this.XYZ; }        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3 Transform(Point3 p, in System.Numerics.Matrix4x4 xform)
        {
            return XYZ.Transform(p.XYZ, xform);
        }        

        #endregion

        #region conversions

        #if NET6_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref XYZ AsNumerics(ref Point3 point)
        {
            return ref Unsafe.As<Point3, XYZ>(ref point);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref Point3 AsPoint(ref XYZ point)
        {
            return ref Unsafe.As<XYZ, Point3>(ref point);
        }
        #endif

        #endregion
    }
}
