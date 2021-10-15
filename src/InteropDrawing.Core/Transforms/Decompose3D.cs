using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using COLOR = System.Drawing.Color;
using ASSET = System.Object;
using SCALAR = System.Single;
using POINT3 = InteropDrawing.Point3;
using VECTOR3 = System.Numerics.Vector3;

namespace InteropDrawing.Transforms
{
    public readonly struct Decompose3D : IDrawing3D
    {
        #region lifecycle
        public Decompose3D(ISurfaceDrawing3D renderTarget)
        {
            _RenderTarget = renderTarget;
            _DecomposeSurfaceOutlines = true;
            _CylinderLod = 6;
            _SphereLod = 3;            
        }

        public Decompose3D(ISurfaceDrawing3D renderTarget, int cylinderLOD, int sphereLOD)
        {
            _RenderTarget = renderTarget;
            _DecomposeSurfaceOutlines = true;
            _CylinderLod = cylinderLOD;
            _SphereLod = sphereLOD;
        }

        #endregion

        #region data

        private readonly ISurfaceDrawing3D _RenderTarget;
        private readonly bool _DecomposeSurfaceOutlines;
        private readonly int _CylinderLod;
        private readonly int _SphereLod;

        #endregion

        #region API - IDrawing3D

        public void DrawAsset(in Matrix4x4 transform, ASSET asset, ColorStyle style)
        {
            if (asset is Asset3D a3d) { a3d._DrawAsSurfaces(this); return; }

            if (asset is IDrawable3D drawable) { drawable.DrawTo(this); return; }            
        }

        public void DrawSegment(POINT3 a, POINT3 b, SCALAR diameter, LineStyle style)
        {
            DrawCylinder(_RenderTarget, a, diameter, b, diameter, _CylinderLod, style);
        }

        public void DrawSphere(POINT3 center, SCALAR diameter, ColorStyle style)
        {
            DrawSphere(_RenderTarget, center, diameter, _SphereLod, style);
        }

        public void DrawSurface(ReadOnlySpan<POINT3> vertices, SurfaceStyle style)
        {
            if (vertices.Length < 3) return;

            if (_DecomposeSurfaceOutlines)
            {
                if (style.Style.HasFill) _RenderTarget.DrawSurface(vertices, (style.Style.FillColor,style.DoubleSided));
                if (style.Style.HasOutline) DrawOutlineAsSegments(this, vertices, style.Style.OutlineWidth, style.Style.OutlineColor);
            }
            else if (style.IsVisible)
            {
                _RenderTarget.DrawSurface(vertices, style);
            }
        }

        #endregion

        #region API - Static

        public static void DrawOutlineAsSegments(IDrawing3D dc, ReadOnlySpan<POINT3> vertices, Single diameter, COLOR color)
        {
            var c = new LineStyle(color);

            for (int i = 1; i < vertices.Length; ++i)
            {
                dc.DrawSegment(vertices[i - 1], vertices[i], diameter, c);
            }

            dc.DrawSegment(vertices[vertices.Length - 1], vertices[0], diameter, c);
        }

        public static void DrawSphere(ISurfaceDrawing3D dc, POINT3 center, SCALAR diameter, int lod, ColorStyle brush)
        {
            lod = lod.Clamp(1, 5); // more than 5 lods will create way too many polygons

            var radius = diameter * 0.5f;

            if (brush.HasOutline)
            {
                var r = radius + brush.OutlineWidth * 0.5f;
                if (r > 0) Parametric.PlatonicFactory.DrawOctahedron(dc, center.ToNumerics(), r, lod, brush.OutlineColor, true);
            }

            if (brush.HasFill)
            {
                var r = radius - brush.OutlineWidth * 0.5f;
                if (r > 0) Parametric.PlatonicFactory.DrawOctahedron(dc, center.ToNumerics(), r, lod, brush.FillColor, false);
            }
        }

        public static void DrawCylinder(ISurfaceDrawing3D dc, POINT3 a, SCALAR diameterA, POINT3 b, SCALAR diameterB, int divisions, LineStyle brush)
        {
            var outr = Math.Abs(brush.Style.OutlineWidth);

            diameterA -= outr;
            diameterB -= outr;

            var aradius = diameterA * 0.5f;
            var bradius = diameterB * 0.5f;

            var av = a.ToNumerics();
            var bv = b.ToNumerics();
            var ab = bv - av;

            // degenerated lines are rendered as "points"
            if (ab.Length() <= Math.Max(aradius, bradius) * 0.1f)
            {
                DrawSphere(dc, (av + bv) * 0.5f, Math.Max(aradius, bradius), 0, brush.Style);
                return;
            }

            ab = VECTOR3.Normalize(ab);

            if (brush.Style.HasOutline)
            {
                var aa = a.ToNumerics();
                var bb = b.ToNumerics();

                if (brush.StartCap != LineCapStyle.Flat) aa -= ab * outr * 0.5f;
                if (brush.EndCap != LineCapStyle.Flat) bb += ab * outr * 0.5f;

                _DrawCylinderInternal(dc, aa, aradius + outr, bb, bradius + outr, divisions, brush.Style.OutlineColor, true, brush.StartCap, brush.EndCap);
            }

            if (brush.Style.HasFill)
            {
                _DrawCylinderInternal(dc, av, aradius, bv, bradius, divisions, brush.Style.FillColor, false, brush.StartCap, brush.EndCap);
            }
        }

        private static void _DrawCylinderInternal(ISurfaceDrawing3D dc, VECTOR3 a, SCALAR aradius, VECTOR3 b, SCALAR bradius, int divisions, COLOR color, bool flipFaces, LineCapStyle startCap, LineCapStyle endCap)
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

            var brush = new SurfaceStyle(color, false);

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

                if (flipFaces) dc.DrawSurface(brush, aa[i], aa[j], bb[j], bb[i]);
                else dc.DrawSurface(brush, aa[j], aa[i], bb[i], bb[j]);
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

        private static void _DrawCylinderCap(ISurfaceDrawing3D dc, SurfaceStyle brush, LineCapStyle cap, VECTOR3 center, VECTOR3 axis, Span<Point3> corners)
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

                        dc.DrawSurface(brush, i0, j0, j1, i1);
                        dc.DrawSurface(brush, center + axis, i1, j1);
                    }
                    break;

                case LineCapStyle.Triangle:
                    for (int i = 0; i < corners.Length; ++i)
                    {
                        var j = (i + 1) % corners.Length;
                        dc.DrawSurface(brush, center + axis, corners[i], corners[j]);
                    }
                    break;


                default: dc.DrawSurface(corners, brush); break;
            }
        }

        #endregion
    }
}
