using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using SCALAR = System.Single;
using POINT3 = InteropTypes.Graphics.Drawing.Point3;
using VECTOR3 = System.Numerics.Vector3;

namespace InteropTypes.Graphics.Drawing.Parametric
{
    partial class ShapeFactory3D
    {
        public struct PointNode
        {
            #region data

            public VECTOR3 Point;
            public VECTOR3 Direction;
            public VECTOR3 Axis;
            public float Angle;
            public float Diameter;

            #endregion

            #region API

            public float GetScale(float maxScale)
            {
                #if NETSTANDARD2_1_OR_GREATER
                var nt = MathF.Tan(this.Angle * 0.5f);
                nt = MathF.Sqrt(1 + nt * nt);
                #else
                var nt = (float)Math.Tan(this.Angle * 0.5f);
                nt = (float)Math.Sqrt(1 + nt * nt);
                #endif

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

            private static void _Fill(Span<PointNode> nodes, ReadOnlySpan<VECTOR3> points, float diameter, bool closed)
            {
                System.Diagnostics.Debug.Assert(points.Length > 1);
                System.Diagnostics.Debug.Assert(points[0] != points[points.Length-1]);

                var joinPoint = closed ? VECTOR3.Normalize(points[points.Length - 1] - points[0]) : VECTOR3.Zero;

                for (int i = 0; i < nodes.Length; i++)
                {
                    nodes[i].Point = points[i];
                    nodes[i].Diameter = diameter;

                    var prevDir = i > 0 ? VECTOR3.Normalize(points[i - 1] - points[i]) : joinPoint;
                    var nextDir = i < points.Length - 1 ? VECTOR3.Normalize(points[i] - points[i + 1]) : joinPoint;
                    
                    var angle = POINT3.AngleInRadians(prevDir, nextDir);
                    nodes[i].Angle = float.IsNaN(angle) ? 0 : angle;
                    nodes[i].Direction = -VECTOR3.Normalize(prevDir + nextDir);
                    nodes[i].Axis = VECTOR3.Normalize(VECTOR3.Cross(prevDir, nextDir));

                    if (float.IsNaN(nodes[i].Axis.X)) nodes[i].Axis = VECTOR3.Zero;
                }
            }           

            private static VECTOR3 _GetMainAxis(ReadOnlySpan<PointNode> nodes)
            {
                if (nodes.Length < 2) return VECTOR3.UnitX;
                if (nodes.Length == 2)
                {                    
                    var d = new POINT3(nodes[1].Point - nodes[0].Point);

                    // calculate a vector perpendicular to D
                    var a = VECTOR3.Cross(d.XYZ, d.DominantAxis == 0 ? VECTOR3.UnitY : VECTOR3.UnitX);

                    System.Diagnostics.Debug.Assert(a != VECTOR3.Zero);

                    return a;
                }

                var axis = VECTOR3.Zero;

                for (int i = 2; i < nodes.Length; i++)
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
                var ny = VECTOR3.Cross(nz, mainAxis);
                var nx = VECTOR3.Cross(ny, nz);                

                nx = VECTOR3.Normalize(nx);
                ny = VECTOR3.Normalize(ny);

                var nt = this.GetScale(4);

                for (int i = 0; i < divisions; ++i)
                {
                    var angle = -PI * 2 * i / divisions;

                    #if NETSTANDARD2_1_OR_GREATER
                    var p = nx * MathF.Cos(angle) + ny * MathF.Sin(angle) * nt;
                    #else
                    var p = nx * (float)Math.Cos(angle) + ny * (float)Math.Sin(angle) * nt;
                    #endif

                    points[i] = Point + p * r;
                }

                POINT3.DebugAssertIsFinite(points);
            }

            private static VECTOR3 _Extrude(ICoreScene3D dc, ReadOnlySpan<PointNode> nodes, bool closed, int divisions, ColorStyle color, bool flipFaces)
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

            private void _DrawCap(ICoreScene3D dc, ColorStyle fillColor, LineCapStyle cap, Span<POINT3> corners, bool dir)
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

                var circleArea = PI * circleRadius * circleRadius;

                #if NETSTANDARD2_1_OR_GREATER
                var k = MathF.Sin(PI * 2.0f / divisions);
                return MathF.Sqrt(circleArea * 2 / divisions * k);
                #else
                var k = Math.Sin(PI * 2.0f / divisions);
                return (float)Math.Sqrt(circleArea * 2 / divisions * k);
                #endif
            }

            #endregion
        }
    }
}
