using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing.Parametric
{
    partial class ShapeFactory3D
    {
        public struct PointNode
        {
            #region data

            public XYZ Point;
            public XYZ Direction;
            public XYZ Axis;
            public float Angle;
            public float Diameter;

            #endregion

            #region API

            public readonly float GetScale(float maxScale)
            {                
                var nt = MathF.Tan(this.Angle * 0.5f);
                nt = MathF.Sqrt(1 + nt * nt);                

                return Math.Min(nt, maxScale);
            }

            public static void Extrude(ICoreScene3D dc, ReadOnlySpan<POINT3> points, float diameter, bool closed, int divisions, bool flipFaces, LineStyle brush)
            {
                System.Diagnostics.Debug.Assert(points.Length > 1);
                System.Diagnostics.Debug.Assert(points[0] != points[points.Length - 1]);

                Span<PointNode> nodes = stackalloc PointNode[points.Length];

                _Fill(nodes, POINT3.AsNumerics(points), diameter, closed);
                var mainAxis = _Extrude(dc, nodes, closed, divisions, brush.FillColor, flipFaces);

                if (closed) return;

                Span<POINT3> corners = stackalloc POINT3[divisions];

                var n = nodes[0];
                n._FillSection(corners, divisions, mainAxis + n.Axis);
                if (flipFaces) corners.Reverse();
                n._DrawCap(dc, brush.FillColor, brush.StartCap, corners, true);

                n = nodes[nodes.Length - 1];
                n._FillSection(corners, divisions, mainAxis + n.Axis);
                if (!flipFaces) corners.Reverse();
                n._DrawCap(dc, brush.FillColor, brush.EndCap, corners, false);
            }

            #endregion

            #region core

            private static void _Fill(Span<PointNode> nodes, ReadOnlySpan<XYZ> points, float diameter, bool closed)
            {
                System.Diagnostics.Debug.Assert(points.Length > 1);
                System.Diagnostics.Debug.Assert(points[0] != points[points.Length-1]);

                var joinPoint = closed ? XYZ.Normalize(points[points.Length - 1] - points[0]) : XYZ.Zero;

                for (int i = 0; i < nodes.Length; i++)
                {
                    nodes[i].Point = points[i];
                    nodes[i].Diameter = diameter;

                    var prevDir = i > 0 ? XYZ.Normalize(points[i - 1] - points[i]) : joinPoint;
                    var nextDir = i < points.Length - 1 ? XYZ.Normalize(points[i] - points[i + 1]) : joinPoint;
                    
                    var angle = POINT3.AngleInRadians(prevDir, nextDir);
                    nodes[i].Angle = float.IsNaN(angle) ? 0 : angle;
                    nodes[i].Direction = -XYZ.Normalize(prevDir + nextDir);
                    nodes[i].Axis = XYZ.Normalize(XYZ.Cross(prevDir, nextDir));

                    if (float.IsNaN(nodes[i].Axis.X)) nodes[i].Axis = XYZ.Zero;
                }
            }           

            private static XYZ _GetMainAxis(ReadOnlySpan<PointNode> nodes)
            {
                if (nodes.Length < 2) return XYZ.UnitX;
                if (nodes.Length == 2)
                {                    
                    var d = new POINT3(nodes[1].Point - nodes[0].Point);

                    // calculate a vector perpendicular to D
                    var a = XYZ.Cross(d.XYZ, d.DominantAxis == 0 ? XYZ.UnitY : XYZ.UnitX);

                    System.Diagnostics.Debug.Assert(a != XYZ.Zero);

                    return a;
                }

                var axis = XYZ.Zero;

                for (int i = 2; i < nodes.Length; i++)
                {
                    var ab = nodes[i - 2].Point - nodes[i - 1].Point;
                    var ac = nodes[i - 1].Point - nodes[i - 0].Point;
                    axis += XYZ.Cross(ab, ac);
                }

                return XYZ.Normalize(axis);
            }


            private readonly void _FillSection(Span<POINT3> points, int divisions, XYZ mainAxis)
            {
                var r = _AdjustNGonRadius(Diameter / 2, divisions);

                var nz = Direction;                
                var ny = XYZ.Cross(nz, mainAxis);
                var nx = XYZ.Cross(ny, nz);                

                nx = XYZ.Normalize(nx);
                ny = XYZ.Normalize(ny);

                var nt = this.GetScale(4);

                for (int i = 0; i < divisions; ++i)
                {
                    var angle = -PI * 2 * i / divisions;
                    
                    var p = nx * MathF.Cos(angle) + ny * MathF.Sin(angle) * nt;                    

                    points[i] = Point + p * r;
                }

                POINT3.DebugAssertIsFinite(points);
            }

            private static XYZ _Extrude(ICoreScene3D dc, ReadOnlySpan<PointNode> nodes, bool closed, int divisions, ColorStyle color, bool flipFaces)
            {
                Span<POINT3> aa = stackalloc POINT3[divisions];
                Span<POINT3> bb = stackalloc POINT3[divisions];

                var maixAxis = _GetMainAxis(nodes);

                if (closed)
                {
                    var n = nodes[nodes.Length - 1];

                    n._FillSection(aa, divisions, maixAxis + n.Axis);
                    // aa.Reverse();
                }

                for (int s = 0; s < nodes.Length; ++s)
                {
                    var n = nodes[s];

                    n._FillSection(bb, divisions, maixAxis + n.Axis);

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

            private readonly void _DrawCap(ICoreScene3D dc, ColorStyle fillColor, LineCapStyle cap, Span<POINT3> corners, bool dir)
            {
                var axis = Direction * (Diameter * 0.5f);

                if (dir) axis = -axis;

                switch (cap)
                {
                    case LineCapStyle.Round:
                        for (int i = 0; i < corners.Length; ++i)
                        {
                            var j = (i + 1) % corners.Length;

                            var i0 = corners[i].XYZ;
                            var j0 = corners[j].XYZ;
                            var i1 = XYZ.Lerp(Point, i0 + axis, 0.7f);
                            var j1 = XYZ.Lerp(Point, j0 + axis, 0.7f);

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

            /// <summary>
            /// Adjusts the radius of a circle so a n-Gon will have the same cross area.
            /// </summary>
            /// <param name="circleRadius">The target circle radius</param>
            /// <param name="divisions">The number of corners of the n-Gon</param>
            /// <returns>the radius of the n-Gon corner</returns>
            internal static float _AdjustNGonRadius(float circleRadius, int divisions)
            {
                // from bing: $$r = \sqrt{\frac{A}{\frac{N}{2}\sin\left(\frac{2\pi}{N}\right)}}$$
                // r = sqrt( Area / ( NumVrt * Sin(2Pi / NumVrt ));


                System.Diagnostics.Debug.Assert(circleRadius > 0, nameof(circleRadius));
                System.Diagnostics.Debug.Assert(divisions >= 3, nameof(divisions));

                // taking 2. Given the radius (circumradius) from here: https://www.mathopenref.com/polygonregulararea.html            
                // and resolving 'r' gives this formula:

                var circleArea = PI * circleRadius * circleRadius;
                
                var k = MathF.Sin(PI * 2.0f / divisions);
                return MathF.Sqrt(circleArea * 2 / divisions * k);
            }

            #endregion
        }
    }
}
