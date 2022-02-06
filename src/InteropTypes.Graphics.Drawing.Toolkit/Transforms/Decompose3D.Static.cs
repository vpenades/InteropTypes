using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using COLOR = System.Drawing.Color;
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
            
            if (asset is Asset3D a3) { a3._DrawAsSurfaces(new Decompose3D(Drawing3DTransform.Create(dc, transform))); return true; }

            // fallback

            return asset is IPseudoImmutable inmutable && DrawAsset(dc, transform, inmutable.ImmutableKey, style);
        }

        public static void DrawSurface(IPrimitiveScene3D dc, ReadOnlySpan<POINT3> vertices, SurfaceStyle style)
        {
            dc.DrawConvexSurface(vertices, style.Style.FillColor);
        }

        public static void DrawOutlineAsSegments(IScene3D dc, ReadOnlySpan<POINT3> vertices, Single diameter, ColorStyle color)
        {
            var c = new LineStyle(color);

            for (int i = 1; i < vertices.Length; ++i)
            {
                dc.DrawSegment(vertices[i - 1], vertices[i], diameter, c);
            }

            dc.DrawSegment(vertices[vertices.Length - 1], vertices[0], diameter, c);
        }

        public static void DrawSegment(IPrimitiveScene3D dc, ReadOnlySpan<POINT3> points, SCALAR diameter, LineStyle style)
        {
            if (points.Length < 3)
            {
                if (!Parametric.ShapeFactory3D.DrawCylinder(dc, (points[0], diameter), (points[1], diameter), 4, style))
                {
                    DrawSphere(dc, (points[0] + points[1]) * 0.5f, diameter, 4, style.Style);
                }

                return;
            }

            bool closed = false;

            if (points[0] == points[points.Length - 1]) { closed = true; points = points.Slice(0, points.Length - 1); }

            Parametric.ShapeFactory3D.PointNode.Extrude(dc, points, diameter, closed, 5, false, style);
        }

        public static void DrawSphere(IPrimitiveScene3D dc, POINT3 center, SCALAR diameter, OutlineFillStyle brush)
        {
            DrawSphere(dc, center, diameter, 4, brush);
        }

        public static void DrawSphere(IPrimitiveScene3D dc, POINT3 center, SCALAR diameter, int lod, OutlineFillStyle brush)
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

        

        

    }
}
