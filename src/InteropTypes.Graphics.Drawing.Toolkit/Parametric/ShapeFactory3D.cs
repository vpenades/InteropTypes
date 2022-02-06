using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using SCALAR = System.Single;
using POINT3 = InteropTypes.Graphics.Drawing.Point3;
using VECTOR3 = System.Numerics.Vector3;

namespace InteropTypes.Graphics.Drawing.Parametric
{
    internal static class ShapeFactory3D
    {
        public struct PointNode
        {
            public VECTOR3 Point;
            public VECTOR3 Direction;
            public float Angle;
            public float Diameter;

            public float GetScale(float maxScale)
            {                
                var nt = MathF.Tan(this.Angle * 0.5f);
                nt = MathF.Sqrt(1 + nt * nt);                
                return Math.Min(nt,maxScale);                
            }

            private static void _Fill(Span<PointNode> nodes, ReadOnlySpan<VECTOR3> points, float diameter, bool closed)
            {
                var joinPoint = closed ? VECTOR3.Normalize(points[points.Length - 1]- points[0]) : VECTOR3.Zero;

                for (int i = 0; i < nodes.Length; i++)
                {
                    nodes[i].Point = points[i];
                    nodes[i].Diameter = diameter;

                    var prevDir = i > 0 ? VECTOR3.Normalize(points[i - 1] - points[i]) : joinPoint;
                    var nextDir = i < points.Length - 1 ? VECTOR3.Normalize(points[i] - points[i + 1]) : joinPoint;                    

                    var angle = POINT3.AngleInRadians(prevDir, nextDir);
                    nodes[i].Angle = float.IsNaN(angle) ? 0 : angle;
                    nodes[i].Direction = -VECTOR3.Normalize(prevDir + nextDir);
                }
            }

            public static void Extrude(IPrimitiveScene3D dc, ReadOnlySpan<POINT3> points,float diameter, bool closed, int divisions, bool flipFaces, LineStyle brush)
            {
                Span<PointNode> nodes = stackalloc PointNode[points.Length];

                _Fill(nodes, POINT3.AsNumerics(points), diameter, closed);
                var mainAxis = _Extrude(dc, nodes, closed, divisions, brush.FillColor, flipFaces);

                if (closed) return;

                Span<POINT3> corners = stackalloc POINT3[divisions];

                nodes[0]._FillSection(corners, divisions, mainAxis);
                if (flipFaces) corners.Reverse();
                nodes[0]._DrawCap(dc, brush.FillColor, brush.StartCap, corners, true);

                nodes[nodes.Length - 1]._FillSection(corners, divisions, mainAxis);
                if (!flipFaces) corners.Reverse();
                nodes[nodes.Length - 1]._DrawCap(dc, brush.FillColor, brush.EndCap, corners, false);
            }            

            private static VECTOR3 _GetMainAxis(ReadOnlySpan<PointNode> nodes)
            {
                if (nodes.Length < 3) return VECTOR3.Zero; // perpendicular

                var axis = VECTOR3.Zero;

                for(int i=2; i < nodes.Length; i++)
                {
                    var ab = nodes[i - 2].Point - nodes[i - 1].Point;
                    var ac = nodes[i - 1].Point - nodes[i - 0].Point;
                    axis += VECTOR3.Cross(ab, ac);
                }

                return VECTOR3.Normalize(axis);
            }


            private void _FillSection(Span<POINT3> points, int divisions, VECTOR3 mainAxis)
            {
                var r = _AdjustNGonRadius(Diameter / 2, divisions);

                var nz = Direction;
                var nx = VECTOR3.Normalize(mainAxis);
                var ny = VECTOR3.Normalize(VECTOR3.Cross(nz, nx));

                var nt = this.GetScale(4);

                for (int i = 0; i < divisions; ++i)
                {
                    var angle = -MathF.PI * 2 * i / divisions;
                    var p = nx * MathF.Cos(angle) + ny * MathF.Sin(angle) * nt;
                    points[i] = Point + p * r;
                }
            }

            private static VECTOR3 _Extrude(IPrimitiveScene3D dc, ReadOnlySpan<PointNode> nodes, bool closed, int divisions, ColorStyle color, bool flipFaces)
            {
                Span<POINT3> aa = stackalloc POINT3[divisions];
                Span<POINT3> bb = stackalloc POINT3[divisions];

                var maixAxis = _GetMainAxis(nodes);

                if (closed)
                {
                    nodes[nodes.Length - 1]._FillSection(aa, divisions, maixAxis);
                    // aa.Reverse();
                }

                for (int s=0; s < nodes.Length; ++s)
                {
                    nodes[s]._FillSection(bb, divisions, maixAxis);

                    if (s > 0 || closed)
                    {
                        for (int i = 0; i < bb.Length; ++i)
                        {
                            var j = (i + 1) % bb.Length;

                            if (flipFaces) dc.DrawConvexSurface(POINT3.Array(aa[i], aa[j], bb[j], bb[i]), color);
                            else dc.DrawConvexSurface(POINT3.Array(aa[j], aa[i], bb[i], bb[j]), color);
                        }
                    }

                    bb.CopyTo(aa);
                }

                return maixAxis;
            }

            private void _DrawCap(IPrimitiveScene3D dc, ColorStyle fillColor, LineCapStyle cap, Span<POINT3> corners, bool dir)
            {
                var axis = Direction * (Diameter * 0.5f);

                if (dir) axis = -axis;

                switch (cap)
                {
                    case LineCapStyle.Round:
                        for (int i = 0; i < corners.Length; ++i)
                        {
                            var j = (i + 1) % corners.Length;

                            var i0 = corners[i].ToNumerics();
                            var j0 = corners[j].ToNumerics();
                            var i1 = VECTOR3.Lerp(Point, i0 + axis, 0.7f);
                            var j1 = VECTOR3.Lerp(Point, j0 + axis, 0.7f);

                            dc.DrawConvexSurface(POINT3.Array(i0, j0, j1, i1), fillColor);
                            dc.DrawConvexSurface(POINT3.Array(Point + axis, i1, j1), fillColor);
                        }
                        break;

                    case LineCapStyle.Triangle:
                        for (int i = 0; i < corners.Length; ++i)
                        {
                            var j = (i + 1) % corners.Length;
                            dc.DrawConvexSurface(POINT3.Array(Point + axis, corners[i], corners[j]), fillColor);
                        }
                        break;


                    default: dc.DrawConvexSurface(corners, fillColor); break;
                }
            }
        }

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

        private static void _DrawCylinderInternal(IPrimitiveScene3D dc, VECTOR3 a, SCALAR aradius, VECTOR3 b, SCALAR bradius, int divisions, ColorStyle color, bool flipFaces, LineCapStyle startCap, LineCapStyle endCap)
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

            Span<POINT3> aa = stackalloc POINT3[divisions];
            Span<POINT3> bb = stackalloc POINT3[divisions];            

            for (int i = 0; i < divisions; ++i)
            {
                var angle = -MathF.PI * 2 * i / divisions;
                var p = nx * MathF.Cos(angle) + ny * MathF.Sin(angle);
                aa[i] = a + p * aradius;
                bb[i] = b + p * bradius;
            }

            for (int i = 0; i < aa.Length; ++i)
            {
                var j = (i + 1) % aa.Length;

                if (flipFaces) _DrawConvex(dc, color, aa[i], aa[j], bb[j], bb[i]);
                else _DrawConvex(dc, color, aa[j], aa[i], bb[i], bb[j]);
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

            _DrawCylinderCap(dc, color, startCap, a, -nz * aradius, aa);

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

            _DrawCylinderCap(dc, color, endCap, b, nz * bradius, bb);
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

        private static void _DrawCylinderCap(IPrimitiveScene3D dc, ColorStyle fillColor, LineCapStyle cap, VECTOR3 center, VECTOR3 axis, Span<POINT3> corners)
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

                        dc.DrawConvexSurface(POINT3.Array(i0, j0, j1, i1), fillColor);
                        dc.DrawConvexSurface(POINT3.Array(center + axis, i1, j1), fillColor);
                    }
                    break;

                case LineCapStyle.Triangle:
                    for (int i = 0; i < corners.Length; ++i)
                    {
                        var j = (i + 1) % corners.Length;
                        dc.DrawConvexSurface(POINT3.Array(center + axis, corners[i], corners[j]), fillColor);
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
