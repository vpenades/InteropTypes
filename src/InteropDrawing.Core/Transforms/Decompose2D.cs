using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;
using POINT2 = InteropDrawing.Point2;
using VECTOR2 = System.Numerics.Vector2;

namespace InteropDrawing.Transforms
{
    public readonly struct Decompose2D : IDrawing2D
    {
        public Decompose2D(IPolygonDrawing2D renderTarget)
        {
            _RenderTarget = renderTarget;
            _DecomposePolygonOutlines = true;
        }

        private readonly IPolygonDrawing2D _RenderTarget;
        private readonly bool _DecomposePolygonOutlines;

        #region API - IDrawing2D

        public void DrawAsset(in Matrix3x2 transform, ASSET asset, ColorStyle style)
        {
            var dc = this.CreateTransformed2D(transform);

            if (asset is IDrawable2D drawable) { drawable.DrawTo(dc); return; }

            // if (asset is string text) { this.DrawFont(transform, text, style); return; }
        }

        public void DrawLines(ReadOnlySpan<POINT2> points, SCALAR diameter, LineStyle style)
        {
            DrawLines(_RenderTarget, points, diameter, style);
        }

        public void DrawEllipse(POINT2 center, SCALAR width, SCALAR height, ColorStyle style)
        {
            DrawEllipse(_RenderTarget, center, width, height, style);
        }

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {            
        }

        public void DrawPolygon(ReadOnlySpan<POINT2> points, ColorStyle style)
        {
            if (points.Length < 3) return;

            if (_DecomposePolygonOutlines)
            {
                if (style.HasFill)
                {
                    _RenderTarget.DrawPolygon(points, style.FillColor);
                }

                if (style.HasOutline)
                {
                    DrawLines(_RenderTarget, points, style.OutlineWidth, style.OutlineColor, true);
                }
            }
            else if (style.IsVisible)
            {
                _RenderTarget.DrawPolygon(points, style);
            }            
        }

        #endregion

        #region API - Static

        public static void DrawLines(IPolygonDrawing2D dc, ReadOnlySpan<POINT2> points, SCALAR diameter, LineStyle style)
        {
            for (int i = 1; i < points.Length; ++i)
            {
                var b = style.GetLineSegmentStyle(points.Length, i);
                _DrawLineAsPolygon(dc, points[i - 1], points[i], diameter, b.Style, b.StartCap, b.EndCap);
            }
        }

        public static void DrawLines(IPolygonDrawing2D dc, ReadOnlySpan<POINT2> points, SCALAR diameter, ColorStyle style, bool closed)
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
                dc.DrawPolygon(segment.Slice(0, 4), style);
                segment = segment.Slice(4);
            }
        }

        private static void _DrawLineAsPolygon(IPolygonDrawing2D dc, POINT2 a, POINT2 b, SCALAR diameter, ColorStyle brush, LineCapStyle startCapStyle, LineCapStyle endCapStyle)
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

            dc.DrawPolygon(vertices, brush);
        }

        public static void DrawEllipse(IPolygonDrawing2D dc, POINT2 center, SCALAR width, SCALAR height, ColorStyle style)
        {
            // calculate number of vertices based on dimensions
            int count = Math.Max((int)width, (int)height);
            if (count < 3) count = 3;

            Span<POINT2> points = stackalloc POINT2[count];
            Parametric.ShapeFactory.FillEllipseVertices(points, center, width, height);

            dc.DrawPolygon(points, style);
        }

        #endregion
    }
}
