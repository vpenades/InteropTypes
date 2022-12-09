using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a sphere in 3D space.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Center} {Radius}")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1036:Override methods on comparable types", Justification = "The API would be misleading")]
    public struct BoundingSphere
        : IEquatable<BoundingSphere>
        , IFormattable
        , IComparable<Plane>
        , IComparable<BoundingSphere>
        , IDrawingBrush<IScene3D>
    {
        #region constructor

        public static implicit operator BoundingSphere((Point3 Center, float Radius) sphere)
        {
            return new BoundingSphere(sphere.Center, sphere.Radius);
        }

        public static BoundingSphere FromPoints(IEnumerable<Point3> points)
        {
            if (points == null) return Undefined;

            var mdl = _Scene3DBoundsBuilder.CreateEmpty();

            foreach (var point in points) mdl.AddVertex(point.XYZ);

            return mdl.Sphere;
        }

        public static BoundingSphere FromAsset(object asset)
        {
            if (asset is ISource bss) { return bss.GetBoundingSphere(); }

            if (asset is IDrawingBrush<IScene3D> s3d) return From(s3d);
            if (asset is IDrawingBrush<ICoreScene3D> c3d) return From(c3d);

            return Undefined;
        }

        public static BoundingSphere From(IDrawingBrush<IScene3D> drawable)
        {
            if (drawable == null) return Undefined;
            if (drawable is ISource bss) { return bss.GetBoundingSphere(); }

            var builder = _Scene3DBoundsBuilder.CreateEmpty();
            drawable.DrawTo(builder);
            return builder.Sphere;
        }

        public static BoundingSphere From(IDrawingBrush<ICoreScene3D> drawable)
        {
            if (drawable == null) return Undefined;
            if (drawable is ISource bss) { return bss.GetBoundingSphere(); }

            var builder = _Scene3DBoundsBuilder.CreateEmpty();
            drawable.DrawTo(builder);
            return builder.Sphere;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public BoundingSphere(Point3 center, float radius)
        {
            Center = center.XYZ;
            Radius = radius;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public BoundingSphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        #endregion

        #region data

        public Vector3 Center;
        public Single Radius;

        /// <inheritdoc/>
        public readonly override int GetHashCode() { return Center.GetHashCode() ^ Radius.GetHashCode(); }

        /// <inheritdoc/>
        public readonly override bool Equals(object obj) { return obj is BoundingSphere other && Equals(other); }

        /// <inheritdoc/>
        public readonly bool Equals(BoundingSphere other) { return this.Center == other.Center && this.Radius == other.Radius; }

        public static bool operator == (BoundingSphere left, BoundingSphere right) { return left.Equals(right); }

        public static bool operator !=(BoundingSphere left, BoundingSphere right) { return !left.Equals(right); }

        #endregion

        #region properties

        public static BoundingSphere Undefined => new BoundingSphere(new Vector3(float.NaN), float.MinValue);

        public readonly bool IsValid => Radius >= 0 && Point3.IsFinite(Center);

        #endregion

        #region API

        public static BoundingSphere Lerp(BoundingSphere left, BoundingSphere right, float amount)
        {
            return new BoundingSphere(Vector3.Lerp(left.Center, right.Center, amount), left.Radius * (1 - amount) + right.Radius * amount);
        }

        public static bool Overlap(in BoundingSphere left, in BoundingSphere right)
        {
            return left.CompareTo(right) < 1;
        }

        public static BoundingSphere Merge(in BoundingSphere left, in BoundingSphere right)
        {
            var relative = right.Center - left.Center;

            float distance = relative.Length();

            // check if one sphere is inside the other
            if (distance <= left.Radius + right.Radius)
            {
                if (distance <= left.Radius - right.Radius) return left;
                if (distance <= right.Radius - left.Radius) return right;
            }

            float leftRadius = Math.Max(left.Radius - distance, right.Radius);
            float Rightradius = Math.Max(left.Radius + distance, right.Radius);

            relative += (leftRadius - Rightradius) / (2 * distance) * relative;

            return new BoundingSphere(left.Center + relative, (leftRadius + Rightradius) / 2);
        }

        public static BoundingSphere Merge(in BoundingSphere sphere, in Vector3 point)
        {
            var relative = point - sphere.Center;
            float distance = relative.Length();
            
            if (distance <= sphere.Radius) return sphere;
            
            float radius = sphere.Radius + distance;
            relative -= radius / (2 * distance) * relative;
            return new BoundingSphere(sphere.Center + relative, radius / 2);
        }

        public static BoundingSphere Transform(BoundingSphere sphere, in Matrix4x4 transform)
        {
            var center = Vector3.Transform(sphere.Center, transform);
            var radius = sphere.Radius * _DecomposeScale(transform);
            return new BoundingSphere(center, radius);
        }

        internal const float SCALEDECOMPOSITIONEPSILON = 0.00001f;

        private static Single _DecomposeScale(Matrix4x4 matrix)
        {
            var det = matrix.GetDeterminant();
            var volume = Math.Abs(det);

            if (Math.Abs(volume - 1) < SCALEDECOMPOSITIONEPSILON) return 1;

            // scale is the cubic root of volume:
            #if !NETSTANDARD2_0
            return MathF.Pow(volume, 1f / 3f);
            #else
            return (float)Math.Pow(volume, (double)1 / 3);
            #endif
        }        

        /// <summary>
        /// Compares this sphere against another sphere.
        /// </summary>
        /// <param name="other">the sphere to compare against.</param>
        /// <returns>
        /// -1 if inside <paramref name="other"/> sphere.<br/>
        /// 0 if overlapping <paramref name="other"/> sphere.<br/>
        /// 1 if outside <paramref name="other"/> sphere.<br/>
        /// </returns>
        public readonly int CompareTo(BoundingSphere other)
        {
            var dist = Vector3.Distance(this.Center, other.Center);
            if (dist > (this.Radius + other.Radius)) return 1;
            if (dist > other.Radius - this.Radius) return 0;
            return -1;
        }

        /// <summary>
        /// Compares this sphere against a plane.
        /// </summary>
        /// <param name="other">the plane to compare against.</param>
        /// <returns>
        /// -1 if behind <paramref name="other"/> plane.<br/>
        /// 0 if overlapping <paramref name="other"/> plane.<br/>
        /// 1 if over <paramref name="other"/> plane.<br/>
        /// </returns>
        public readonly int CompareTo(Plane other)
        {
            var dot = Plane.DotCoordinate(other, this.Center);
            if (Math.Abs(dot) < Radius) return 0;
            return dot.CompareTo(0);
        }

        #endregion

        #region convert to

        /// <inheritdoc/>
        public readonly override string ToString()
        {
            return new Vector4(Center, Radius).ToString();
        }

        /// <inheritdoc/>
        public readonly string ToString(string format, IFormatProvider formatProvider)
        {
            return new Vector4(Center, Radius).ToString(format, formatProvider);
        }

        /// <inheritdoc/>
        public readonly void DrawTo(IScene3D context)
        {
            var style = ColorStyle.GetDefaultFrom(context, ColorStyle.Red);
            DrawTo(context, style);
        }

        public readonly void DrawTo(IScene3D context, OutlineFillStyle color)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            context.DrawSphere(this.Center, this.Radius * 2, color);
        }

        #endregion

        #region nested types

        /// <summary>
        /// Objects that implement this interface can expose a <see cref="BoundingSphere"/>.
        /// </summary>
        public interface ISource
        {
            /// <summary>
            /// Gets the bounding sphere containing this object.
            /// </summary>
            /// <returns></returns>
            BoundingSphere GetBoundingSphere();
        }

        #endregion
    }    
}