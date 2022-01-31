using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using COLOR = System.Drawing.Color;
using ASSET = System.Object;
using SCALAR = System.Single;
using POINT3 = InteropDrawing.Point3;
using VECTOR3 = System.Numerics.Vector3;

namespace InteropDrawing.Parametric
{
    internal class ShapeFactory3D
    {
        public static bool DrawCylinder(IPrimitiveScene3D dc, (POINT3 Position, SCALAR Diameter) a, (POINT3 Position, SCALAR Diameter) b, int divisions, LineStyle brush)
        {
            var outr = Math.Abs(brush.Style.OutlineWidth);

            var aradius = (a.Diameter - outr) * 0.5f;
            var bradius = (b.Diameter - outr) * 0.5f;

            var av = a.Position.ToNumerics();
            var bv = b.Position.ToNumerics();
            var ab = bv - av;

            // degenerated lines are rendered as "points"
            if (ab.Length() <= Math.Max(aradius, bradius) * 0.1f)
            {
                return false;
            }

            ab = VECTOR3.Normalize(ab);

            if (brush.Style.HasOutline)
            {
                var aa = a.Position.ToNumerics();
                var bb = b.Position.ToNumerics();

                if (brush.StartCap != LineCapStyle.Flat) aa -= ab * outr * 0.5f;
                if (brush.EndCap != LineCapStyle.Flat) bb += ab * outr * 0.5f;

                _DrawCylinderInternal(dc, aa, aradius + outr, bb, bradius + outr, divisions, brush.Style.OutlineColor, true, brush.StartCap, brush.EndCap);
            }

            if (brush.Style.HasFill)
            {
                _DrawCylinderInternal(dc, av, aradius, bv, bradius, divisions, brush.Style.FillColor, false, brush.StartCap, brush.EndCap);
            }

            return true;
        }

        private static void _DrawCylinderInternal(IPrimitiveScene3D dc, VECTOR3 a, SCALAR aradius, VECTOR3 b, SCALAR bradius, int divisions, COLOR color, bool flipFaces, LineCapStyle startCap, LineCapStyle endCap)
        {
            if (aradius < 0.0001f && bradius < 0.0001f)
            {
                aradius = bradius = 0.0001f;
                divisions = 3;

                // todo: draw a segment with 6 triangles
            }

            aradius = _AdjustNGonRadius(aradius, divisions);
            bradius = _AdjustNGonRadius(bradius, divisions);

            var nz = VECTOR3.Normalize(b - a);
            var nx = VECTOR3.Normalize(nz.PerpendicularAxis());
            var ny = VECTOR3.Normalize(VECTOR3.Cross(nz, nx));

            Span<Point3> aa = stackalloc Point3[divisions];
            Span<Point3> bb = stackalloc Point3[divisions];

            var brush = new ColorStyle(color);

            for (int i = 0; i < divisions; ++i)
            {
                var angle = -MathF.PI * 2 * i / divisions;
                var p = (nx * MathF.Cos(angle) + ny * MathF.Sin(angle));
                aa[i] = a + p * aradius;
                bb[i] = b + p * bradius;
            }

            for (int i = 0; i < aa.Length; ++i)
            {
                var j = (i + 1) % aa.Length;

                if (flipFaces) _DrawConvex(dc, brush, aa[i], aa[j], bb[j], bb[i]);
                else _DrawConvex(dc, brush, aa[j], aa[i], bb[i], bb[j]);
            }

            if (flipFaces)
            {
                for (int i = 0; i < aa.Length / 2; ++i)
                {
                    var j = aa.Length - 1 - i;
                    var k = aa[i];
                    aa[i] = aa[j];
                    aa[j] = k;
                }
            }

            _DrawCylinderCap(dc, brush, startCap, a, -nz * aradius, aa);

            if (!flipFaces)
            {
                for (int i = 0; i < bb.Length / 2; ++i)
                {
                    var j = bb.Length - 1 - i;
                    var k = bb[i];
                    bb[i] = bb[j];
                    bb[j] = k;
                }
            }

            _DrawCylinderCap(dc, brush, endCap, b, nz * bradius, bb);
        }



        /// <summary>
        /// Adjusts the radius of a circle so a n-Gon will have the same cross area.
        /// </summary>
        /// <param name="circleRadius">The target circle radius</param>
        /// <param name="divisions">The number of corners of the n-Gon</param>
        /// <returns>the radius of the n-Gon corner</returns>
        internal static float _AdjustNGonRadius(float circleRadius, int divisions)
        {
            System.Diagnostics.Debug.Assert(circleRadius > 0, nameof(circleRadius));
            System.Diagnostics.Debug.Assert(divisions >= 3, nameof(divisions));

            // taking 2. Given the radius (circumradius) from here: https://www.mathopenref.com/polygonregulararea.html            
            // and resolving 'r' gives this formula:

            var circleArea = MathF.PI * circleRadius * circleRadius;

            var k = MathF.Sin(MathF.PI * 2.0f / divisions);

            return MathF.Sqrt(circleArea * 2 / divisions * k);
        }

        private static void _DrawCylinderCap(IPrimitiveScene3D dc, ColorStyle fillColor, LineCapStyle cap, VECTOR3 center, VECTOR3 axis, Span<Point3> corners)
        {
            switch (cap)
            {
                case LineCapStyle.Round:
                    for (int i = 0; i < corners.Length; ++i)
                    {
                        var j = (i + 1) % corners.Length;

                        var i0 = corners[i].ToNumerics();
                        var j0 = corners[j].ToNumerics();
                        var i1 = VECTOR3.Lerp(center, i0 + axis, 0.7f);
                        var j1 = VECTOR3.Lerp(center, j0 + axis, 0.7f);

                        _DrawConvex(dc, fillColor, i0, j0, j1, i1);
                        _DrawConvex(dc, fillColor, center + axis, i1, j1);
                    }
                    break;

                case LineCapStyle.Triangle:
                    for (int i = 0; i < corners.Length; ++i)
                    {
                        var j = (i + 1) % corners.Length;
                        _DrawConvex(dc, fillColor, center + axis, corners[i], corners[j]);
                    }
                    break;


                default: dc.DrawConvexSurface(corners, fillColor); break;
            }
        }

        private static void _DrawConvex(IPrimitiveScene3D dc, ColorStyle style, params POINT3[] points)
        {
            dc.DrawConvexSurface(points, style);
        }
    }
}
