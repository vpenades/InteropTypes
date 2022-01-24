using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;
using POINT2 = InteropDrawing.Point2;
using VECTOR2 = System.Numerics.Vector2;
using System.Drawing;

using COLOR = System.Drawing.Color;

namespace InteropDrawing.Transforms
{    
    partial struct Decompose2D
    {
        #region API - Static IVectorsDrawing2D        

        public static void DrawAsset(IAssetDrawing2D dc, in Matrix3x2 transform, ASSET asset, in ColorStyle style)
        {
            if (asset is IDrawingBrush<IDrawing2D> drawable)
            {
                var dcx = dc.CreateTransformed2D(transform) as IDrawing2D;
                drawable.DrawTo(dcx);
                return;
            }

            // if (asset is string text) { this.DrawFont(transform, text, style); return; }
            // if (asset is StringBuilder text) { this.DrawFont(transform, text.ToString(), style); return; }

            // fallback
            if (dc is IDrawing2D rt) { rt.DrawAsset(transform, asset, style); return; }
        }

        public static void DrawPolygon(IPolygonDrawing2D dc, ReadOnlySpan<POINT2> points, in PolygonStyle style)
        {
            if (points.Length < 3) return;

            if (style.HasFill) dc.FillConvexPolygon(points, style.FillColor); // todo: triangulate

            if (!style.HasOutline) _DrawSolidLines(dc, points, style.OutlineWidth, style.OutlineColor, true);
        }

        public static void DrawPolygon(Backends.IDrawingBackend2D dc, ReadOnlySpan<POINT2> points, in PolygonStyle style)
        {
            if (points.Length < 3) return;

            if (style.HasFill) dc.FillConvexPolygon(points, style.FillColor); // todo: triangulate

            if (!style.HasOutline) _DrawSolidLines(dc, points, style.OutlineWidth, style.OutlineColor, true);
        }

        public static void DrawEllipse(IPolygonDrawing2D dc, POINT2 center, SCALAR width, SCALAR height, in ColorStyle style)
        {
            // calculate number of vertices based on dimensions
            int count = Math.Max((int)width, (int)height);
            if (count < 3) count = 3;

            Span<POINT2> points = stackalloc POINT2[count];
            Parametric.ShapeFactory.FillEllipseVertices(points, center, width, height);

            if (style.HasFill) { dc.FillConvexPolygon(points, style.FillColor); }
            if (style.HasOutline) { _DrawSolidLines(dc, points, style.OutlineWidth, style.OutlineColor, true); }
        }

        public static void DrawEllipse(Backends.IDrawingBackend2D dc, POINT2 center, SCALAR width, SCALAR height, in ColorStyle style)
        {
            // calculate number of vertices based on dimensions
            int count = Math.Max((int)width, (int)height);
            if (count < 3) count = 3;

            Span<POINT2> points = stackalloc POINT2[count];
            Parametric.ShapeFactory.FillEllipseVertices(points, center, width, height);

            if (style.HasFill) { dc.FillConvexPolygon(points, style.FillColor); }
            if (style.HasOutline) { _DrawSolidLines(dc, points, style.OutlineWidth, style.OutlineColor, true); }
        }        

        public static void DrawLines(IPolygonDrawing2D dc, ReadOnlySpan<POINT2> points, SCALAR diameter, in LineStyle style)
        {
            var xstyle = style.IsSolid(ref diameter, out var solid)
                ? new LineStyle(solid)
                : style;

            if (style.Style.HasFill)
            {
                for (int i = 1; i < points.Length; ++i)
                {
                    var b = style.GetLineSegmentStyle(points.Length, i);
                    _FillLineAsPolygon(dc, points[i - 1], points[i], diameter, b.Style.FillColor, b.StartCap, b.EndCap);
                }
            }
        }

        public static void DrawLines(Backends.IDrawingBackend2D dc, ReadOnlySpan<POINT2> points, SCALAR diameter, in LineStyle style)
        {
            var xstyle = style.IsSolid(ref diameter, out var solid)
                ? new LineStyle(solid)
                : style;

            var pixelSize = dc.GetThinLinesPixelSize();

            if (diameter <= pixelSize)
            {
                dc.DrawThinLines(points, diameter, style.FillColor);
                return;
            }

            if (style.Style.HasFill)
            {
                for (int i = 1; i < points.Length; ++i)
                {
                    var b = style.GetLineSegmentStyle(points.Length, i);
                    _FillLineAsPolygon(dc, points[i - 1], points[i], diameter, b.Style.FillColor, b.StartCap, b.EndCap);
                }
            }
        }


        private static void _DrawSolidLines(IPolygonDrawing2D dc, ReadOnlySpan<POINT2> points, SCALAR diameter, Color color, bool closed)
        {
            if (points.Length < 2) return;
            if (points.Length == 2) closed = false;

            // create segments
            Span<POINT2> segments = stackalloc POINT2[Parametric.ShapeFactory.GetLinesSegmentsVerticesCount(points.Length, closed)];
            Parametric.ShapeFactory.FillLinesSegments(segments, points, diameter, closed);

            // draw segments
            var segment = segments;
            while (segment.Length >= 4)
            {
                dc.FillConvexPolygon(segment.Slice(0, 4), color);
                segment = segment.Slice(4);
            }
        }

        private static void _FillLineAsPolygon(IPolygonDrawing2D dc, POINT2 a, POINT2 b, SCALAR diameter, Color color, LineCapStyle startCapStyle, LineCapStyle endCapStyle)
        {
            var startCapCount = Parametric.ShapeFactory.GetLineCapVertexCount(startCapStyle);
            var endCapCount = Parametric.ShapeFactory.GetLineCapVertexCount(endCapStyle);
            Span<POINT2> vertices = stackalloc POINT2[startCapCount + endCapCount];

            var aa = a.ToNumerics();
            var bb = b.ToNumerics();

            var delta = bb - aa;

            delta = delta.LengthSquared() <= 1 ? VECTOR2.UnitX : VECTOR2.Normalize(delta);

            Parametric.ShapeFactory.FillLineCapVertices(vertices, 0, aa, delta, diameter, startCapStyle);
            Parametric.ShapeFactory.FillLineCapVertices(vertices, startCapCount, bb, -delta, diameter, endCapStyle);

            dc.FillConvexPolygon(vertices, color);
        }        

        #endregion
    }
}
