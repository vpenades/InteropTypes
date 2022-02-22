using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;
using XYZW = System.Numerics.Vector4;
using PLANE = System.Numerics.Plane;
using COLOR = System.Drawing.Color;


namespace InteropTypes.Graphics.Drawing
{
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    static partial class _PrivateExtensions
    {
        #region drawing

        public static XY ToVector2(this System.Drawing.PointF point) { return new XY(point.X, point.Y); }
        public static XY ToVector2(this System.Drawing.SizeF point) { return new XY(point.Width, point.Height); }

        public static XY ToCenterVector2(this System.Drawing.RectangleF rect) { return rect.Location.ToVector2() + rect.Size.ToVector2() * 0.5f; }

        public static System.Drawing.PointF ToPointF(this XY point) { return new System.Drawing.PointF(point.X, point.Y); }

        public static System.Drawing.SizeF ToSizeF(this XY size) { return new System.Drawing.SizeF(size.X, size.Y); }

        public static System.Drawing.RectangleF MinMaxToRectF(this in (XY Min, XY Max) minmax)
        {
            var size = minmax.Max - minmax.Min;
            return new System.Drawing.RectangleF(minmax.Min.X, minmax.Min.Y, size.X, size.Y);
        }


        #endregion

        #region Numerics

        public static XYZ ColumnX(this in Matrix3x2 m) { return new XYZ(m.M11, m.M21, m.M31); }
        public static XYZ ColumnY(this in Matrix3x2 m) { return new XYZ(m.M12, m.M22, m.M32); }

        public static XYZ MinMaxCenter(this in Matrix3x2 m)
        {
            return new XYZ(m.M11+m.M12, m.M21+m.M22, m.M31+m.M32)*0.5f;
        }

        public static XY ToVector(this (float x, float y) tuple) { return new XY(tuple.x, tuple.y); }

        public static XYZ ToVector(this (float x, float y, float z) tuple) { return new XYZ(tuple.x, tuple.y, tuple.z); }

        public static int IntegerPow(this int value, int exp)
        {
            int result = 1;

            while (exp != 0)
            {
                if ((exp & 1) == 1) result *= value;

                exp >>= 1;
                value *= value;
            }

            return result;
        }

        public static XYZ GetAnyPerpendicular(this XYZ v)
        {
            var mainAxis = v.DominantAxis();

            var u = mainAxis == 2
                ?
                new XYZ(v.X, v.Z, -v.Y) : // used when Z is the dominant
                new XYZ(-v.Y, v.X, v.Z); // used when X or Y are the dominant

            return XYZ.Cross(u, v);
        }

        public static float GetDeterminant3x3(this in Matrix4x4 xform)
        {
            float determinant3x3 =
                +(xform.M13 * xform.M21 * xform.M32)
                + (xform.M11 * xform.M22 * xform.M33)
                + (xform.M12 * xform.M23 * xform.M31)
                - (xform.M12 * xform.M21 * xform.M33)
                - (xform.M13 * xform.M22 * xform.M31)
                - (xform.M11 * xform.M23 * xform.M32);

            return determinant3x3;
        }

        public static float DecomposeScale(this in Matrix3x2 xform)
        {
            var det = xform.GetDeterminant();
            var area = Math.Abs(det);

            #if NETSTANDARD2_1_OR_GREATER
            return MathF.Sqrt(area);
            #else
            return (float)Math.Sqrt(area);
            #endif
        }


        /// <summary>
        /// Calculates the "global scale" of the matrix
        /// </summary>
        /// <param name="matrix">Any matrix</param>
        /// <returns>The average scale being applied by the matrix.</returns>
        public static Single DecomposeScale(this in Matrix4x4 matrix)
        {
            // https://github.com/dotnet/runtime/blob/6cf1b8ec012d52880d46fa4773f60ed52ddc9f3d/src/libraries/System.Private.CoreLib/src/System/Numerics/Matrix4x4.cs#L1735

            var det = matrix.GetDeterminant();
            var volume = Math.Abs(det);

            #if NETSTANDARD2_1_OR_GREATER
            return MathF.Pow(volume, (float)1 / 3);
            #else
            return (float)Math.Pow(volume, (double)1 / 3);
            #endif
        }

        public static (XY Center, Single Radius) TransformCircle(this in Matrix3x2 matrix, in (XY Center, Single Radius) sphere)
        {
            var r = sphere.Radius * matrix.DecomposeScale();
            var c = XY.Transform(sphere.Center, matrix);
            return (c, r);
        }

        public static (XYZ Center, Single Radius) TransformSphere(this in Matrix4x4 matrix, in (XYZ Center, Single Radius) sphere)
        {
            var r = sphere.Radius * matrix.DecomposeScale();
            var c = XYZ.Transform(sphere.Center, matrix);
            return (c, r);
        }

        public static Matrix3x2 MinMaxToMatrix3x2(this in (XYZ Min,XYZ Max) bounds)
        {
            return new Matrix3x2
                ( bounds.Min.X, bounds.Max.X
                , bounds.Min.Y, bounds.Max.Y
                , bounds.Min.Z, bounds.Max.Z
                );
        }

        #endregion

        #region color

        public static bool _IsVisible(this COLOR color) { return color.A != 0; }

        #endregion

        #region Geometry

        public static bool IsInPositiveSideOfPlane(this PLANE plane, Point3 p, Single radius)
        {
            // https://github.com/dotnet/corefx/blob/master/src/System.Numerics.Vectors/src/System/Numerics/Plane.cs#L245

            return PLANE.DotCoordinate(plane, p.XYZ) > radius;
        }

        public static bool IsInPositiveSideOfPlane(this PLANE plane, XYZ p)
        {
            return PLANE.DotCoordinate(plane, p) > 0;
        }
        public static bool IsInPositiveSideOfPlane(this PLANE plane, ReadOnlySpan<Point3> ppp)
        {
            foreach (var p in ppp)
            {
                if (!plane.IsInPositiveSideOfPlane(p.XYZ)) return false;
            }

            return true;
        }

        public static bool IsInPositiveSideOfPlane(this PLANE plane, ReadOnlySpan<XYZ> ppp)
        {
            foreach (var p in ppp)
            {
                if (!plane.IsInPositiveSideOfPlane(p)) return false;
            }

            return true;
        }

        public static bool IsInPositiveSideOfPlane(this PLANE plane, ReadOnlySpan<XYZW> ppp)
        {
            foreach (var p in ppp)
            {
                if (!plane.IsInPositiveSideOfPlane(p.SelectXYZ())) return false;
            }

            return true;
        }

        #endregion

        #region  polygon clipping


        public static int ClipPolygonToPlane(this in PLANE plane, Span<Point3> outVertices, ReadOnlySpan<Point3> inVertices)
        {
            return Parametric.PolygonClipper3.ClipPolygonToPlane(Point3.AsNumerics(outVertices), Point3.AsNumerics(inVertices), plane);
        }

        public static int ClipPolygonToPlane(this in PLANE plane, Span<XYZ> outVertices, ReadOnlySpan<XYZ> inVertices)
        {

            return Parametric.PolygonClipper3.ClipPolygonToPlane(outVertices, inVertices, plane);
        }

        public static int ClipPolygonToPlane(this in PLANE plane, Span<XYZW> outVertices, ReadOnlySpan<XYZW> inVertices)
        {
            var count = Parametric.PolygonClipper4.ClipPolygonToPlane(outVertices, inVertices, plane);

            #if DEBUG
            System.Diagnostics.Debug.Assert(new PLANE(plane.Normal, plane.D+0.0001f).IsInPositiveSideOfPlane(outVertices.Slice(0, count)));
            #endif

            return count;
        }

        /// <summary>
        /// clips a line segment against a plane
        /// </summary>
        /// <param name="plane">plane equation</param>
        /// <param name="a">start point of the line segment</param>
        /// <param name="b">end point of the line segment</param>
        /// <returns>true if the line is totally or partially in the positive side of the plane</returns>
        public static bool ClipLineToPlane(this in PLANE plane, ref XYZ a, ref XYZ b)
        {
            return Parametric.PolygonClipper3.ClipLineToPlane(ref a, ref b, plane);
        }

        public static bool ClipLineToPlane(this in PLANE plane, ref XYZW a, ref XYZW b)
        {
            return Parametric.PolygonClipper4.ClipLineToPlane(ref a, ref b, plane);
        }

        #endregion
    }
}
