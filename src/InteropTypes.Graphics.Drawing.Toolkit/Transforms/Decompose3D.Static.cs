using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;
using POINT3 = InteropTypes.Graphics.Drawing.Point3;

namespace InteropTypes.Graphics.Drawing.Transforms
{
    partial struct Decompose3D
    {
        public static bool DrawAsset(IPrimitiveScene3D dc, in Matrix4x4 transform, ASSET asset, ColorStyle style)
        {
            if (asset == null) return true;

            if (asset is IDrawingBrush<IScene3D> a2) { a2.DrawTo(new Decompose3D(Drawing3DTransform.Create(dc, transform))); return true; }

            if (asset is IDrawingBrush<IPrimitiveScene3D> a1) { a1.DrawTo(Drawing3DTransform.Create(dc,transform)); return true; }            

            // fallback

            return asset is IPseudoImmutable inmutable && DrawAsset(dc, transform, inmutable.ImmutableKey, style);
        }

        public static void DrawSurface(IPrimitiveScene3D dc, ReadOnlySpan<POINT3> vertices, SurfaceStyle style)
        {
            if (vertices.Length == 0 || style.Style.IsEmpty) return;
            if (vertices.Length == 1) { DrawSphere(dc, vertices[0], 0, style.Style); return; }
            if (vertices.Length == 2) { DrawSegment(dc, vertices, 0, style.Style); return; }

            if (!style.Style.HasOutline)
            {
                dc.DrawConvexSurface(vertices, style.Style.FillColor);
                if (style.DoubleSided) _DrawConvexSurfaceReverse(dc, vertices, style.Style.FillColor);
                return;
            }
            
            var direction = GetPlaneDirection(vertices) * style.Style.OutlineWidth * 0.5f;

            Span<POINT3> aaa = stackalloc POINT3[vertices.Length];
            vertices.CopyTo(aaa);
            for (int i = 0; i < aaa.Length; i++) aaa[i] += direction;
            dc.DrawConvexSurface(aaa, style.Style.FillColor);

            Span<POINT3> bbb = stackalloc POINT3[vertices.Length];
            vertices.CopyTo(bbb);
            for (int i = 0; i < bbb.Length; i++) bbb[i] -= direction;
            _DrawConvexSurfaceReverse(dc, bbb, style.Style.FillColor);

            for (int i = 0; i < aaa.Length; i++)
            {
                var j = i > 0 ? i - 1 : aaa.Length - 1;

                var aa0 = aaa[i];
                var aa1 = aaa[j];

                var bb0 = bbb[i];
                var bb1 = bbb[j];

                dc.DrawConvexSurface(POINT3.Array(aa0, aa1, bb1, bb0), style.Style.OutlineColor);
            }
            
        }

        private static Vector3 GetPlaneDirection(ReadOnlySpan<POINT3> vertices)
        {
            Vector3 direction = Vector3.Zero;

            for (int i = 2; i < vertices.Length; i++)
            {
                direction += POINT3.Cross(vertices[i - 2], vertices[i - 1], vertices[i]);
            }

            return Vector3.Normalize(direction);
        }

        private static void _DrawConvexSurfaceReverse(IPrimitiveScene3D dc, ReadOnlySpan<POINT3> vertices, ColorStyle style)
        {
            Span<POINT3> reverse = stackalloc POINT3[vertices.Length];
            vertices.CopyTo(reverse);
            reverse.Reverse();

            dc.DrawConvexSurface(reverse, style);
        }        


        public static void DrawSegment(IPrimitiveScene3D dc, ReadOnlySpan<POINT3> points, SCALAR diameter, LineStyle style)
        {
            System.Diagnostics.Debug.Assert(points.Length > 0);
            bool closed = false;
            if (points[0] == points[points.Length - 1]) { closed = true; points = points.Slice(0, points.Length - 1); }

            if (points.Length == 1)
            {
                DrawSphere(dc, points[0], diameter, style.Style);
                return;
            }

            var d = diameter;
            if (style.Style.HasOutline)
            {
                d -= style.OutlineWidth * 0.5f;

                if (d < 0)
                {
                    d = diameter + style.OutlineWidth * 0.5f;
                    style = style.WithFill(style.Style.OutlineColor).WithOutline(0);
                }
            }

            if (style.Style.HasFill)
            {
                _DrawSegment(dc, points, d, style.WithOutline(0), closed, false);
            }

            if (style.Style.HasOutline)
            {
                _DrawSegment(dc, points, diameter + style.OutlineWidth * 0.5f, style.WithFill(style.OutlineColor).WithOutline(0), closed, true);
            }
        }

        private static void _DrawSegment(IPrimitiveScene3D dc, ReadOnlySpan<POINT3> points, SCALAR diameter, LineStyle style, bool closed, bool flip)
        {
            Parametric.ShapeFactory3D.PointNode.Extrude(dc, points, diameter, closed, 5, flip, style);
        }        

        public static void DrawSphere(IPrimitiveScene3D dc, POINT3 center, SCALAR diameter, OutlineFillStyle brush, int lod = 4)
        {
            // more than 5 lods will create way too many polygons
            lod = Math.Max(1, lod);
            lod = Math.Min(5, lod);

            var radius = diameter * 0.5f;

            if (brush.HasOutline)
            {
                var r = radius + brush.OutlineWidth * 0.5f;

                if (r > 0) Parametric.ShapeFactory3D.PlatonicFactory.DrawOctahedron(dc, center.ToNumerics(), r, lod, brush.OutlineColor, true);
            }

            if (brush.HasFill)
            {
                var r = radius - brush.OutlineWidth * 0.5f;

                if (r > 0) Parametric.ShapeFactory3D.PlatonicFactory.DrawOctahedron(dc, center.ToNumerics(), r, lod, brush.FillColor, false);
            }
        }
    }
}
