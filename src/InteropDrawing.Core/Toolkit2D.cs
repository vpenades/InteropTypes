using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;

using VECTOR2 = System.Numerics.Vector2;
using BRECT = System.Drawing.RectangleF;

using SCALAR = System.Single;
using POINT2 = InteropDrawing.Point2;
using XFORM2 = System.Numerics.Matrix3x2;

using ASSET = System.Object;

namespace InteropDrawing
{
    public static partial class Toolkit
    {
        #region 2D transforms

        public static IDrawing2D CreateTransformed(IDrawing2D target, Point2 physicalSize, Point2 virtualSize, XFORM2 xform)
        {
            return Transforms.Drawing2DTransform.Create(target, physicalSize, virtualSize, xform);
        }

        public static IDrawing2D CreateTransformed(IDrawing2D t, Point2 physicalSize, Point2 virtualSize)
        {
            return Transforms.Drawing2DTransform.Create(t, physicalSize, virtualSize);
        }        

        public static IDrawing2D CreateTransformed(IDrawing2D t, Point2 physicalSize, BRECT virtualBounds)
        {
            return Transforms.Drawing2DTransform.Create(t, physicalSize, virtualBounds);
        }

        #endregion

        #region assets

        public static void DrawAsset(this IDrawing2D dc, in XFORM2 transform, Object asset)
        {
            dc.DrawAsset(transform, asset, COLOR.White);
        }
        
        public static BRECT? GetAssetBoundingRect(Object asset)
        {
            if (asset is Model2D model2D) return model2D.BoundingRect;
            return null;
        }
        
        public static (VECTOR2 Center,Single Radius)? GetAssetBoundingCircle(Object asset)
        {
            if (asset is Model2D model2D) return model2D.BoundingCircle;
            return null;
        }        

        #endregion

        #region drawing        

        public static void DrawLine(this IDrawing2D dc, POINT2 a, POINT2 b, SCALAR diameter, LineStyle brush)
        {
            Span<POINT2> pp = stackalloc POINT2[2];
            pp[0] = a;
            pp[1] = b;

            dc.DrawLines(pp, diameter, brush);
        }        

        public static void DrawCircle(this IDrawing2D dc, POINT2 center, SCALAR diameter, ColorStyle brush)
        {
            dc.DrawEllipse(center, diameter, diameter, brush);
        }        

        public static void DrawRectangle(this IDrawing2D dc, POINT2 origin, POINT2 size, ColorStyle brush)
        {
            Span<POINT2> vertices = stackalloc POINT2[4];

            Parametric.ShapeFactory.FillRectangleVertices(vertices, origin, size, 0, 0);

            dc.DrawPolygon(vertices, brush);
        }

        public static void DrawRectangle(this IDrawing2D dc, POINT2 origin, POINT2 size, ColorStyle brush, float borderRadius, int arcVertexCount = 6)
        {
            if (borderRadius == 0) arcVertexCount = 0;

            Span<POINT2> vertices = stackalloc POINT2[Parametric.ShapeFactory.GetRectangleVertexCount(arcVertexCount)];

            Parametric.ShapeFactory.FillRectangleVertices(vertices, origin, size, borderRadius, arcVertexCount);

            dc.DrawPolygon(vertices, brush);
        }        

        public static void DrawPolygon(this IDrawing2D dc, ColorStyle brush, params POINT2[] points)
        {
            dc.DrawPolygon(points, brush);
        }

        public static void DrawLine(this IDrawing2D dc, POINT2 a, POINT2 b, float diameter, SpriteStyle style)
        {
            var aa = a.ToNumerics();
            var ab = b.ToNumerics() - aa;

            var brush = style.Bitmap;
            var brushLen = brush.Width - brush.Pivot.X * 2;

            var ss = new VECTOR2(ab.Length(), diameter) / new VECTOR2(brushLen, brush.Height);
            var rr = MathF.Atan2(ab.Y, ab.X);

            var xform = XFORM2.CreateScale(ss);
            xform *= XFORM2.CreateRotation(rr);

            xform.Translation = aa;

            dc.DrawSprite(xform, style);
        }


        public static void DrawFont(this IDrawing2D dc, POINT2 origin, float size, String text, FontStyle style)
        {
            var xform = XFORM2.CreateScale(size);
            xform.Translation = origin.ToNumerics();

            style = style.With(style.Strength * size);

            dc.DrawFont(xform, text, style);
        }

        public static void DrawFont(this IDrawing2D dc, XFORM2 xform, String text, FontStyle style)
        {
            float xflip = 1;
            float yflip = 1;

            if (style.Alignment.HasFlag(FontAlignStyle.FlipHorizontal)) { xflip = -1; }
            if (style.Alignment.HasFlag(FontAlignStyle.FlipVertical)) { yflip = -1; }

            style = style.With(style.Alignment & ~(FontAlignStyle.FlipHorizontal | FontAlignStyle.FlipVertical));

            xform = XFORM2.CreateScale(xflip, yflip) * xform;

            Fonts.FontDrawing.DrawFontAsLines(dc, xform, text, style);
        }        

        #endregion

        #region drawing as polygons

        public static LineStyle GetLineSegmentStyle(this LineStyle brush, int pointCount, int index)
        {
            var startCap = (index - 1) == 0 ? brush.StartCap : LineCapStyle.Triangle;
            var endCap = (index + 1) == pointCount ? brush.EndCap : LineCapStyle.Triangle;

            return new LineStyle(brush.Style, startCap, endCap);
        }

        public static void DrawLinesAsPolygons(this IDrawing2D dc, ReadOnlySpan<POINT2> points, SCALAR diameter, LineStyle style)
        {
            for (int i = 1; i < points.Length; ++i)
            {
                var b = style.GetLineSegmentStyle(points.Length, i);
                _DrawLineAsPolygon(dc, points[i - 1], points[i], diameter, b.Style, b.StartCap, b.EndCap);
            }
        }

        public static void DrawLinesAsPolygons(this IDrawing2D dc, ReadOnlySpan<POINT2> points, SCALAR diameter, ColorStyle style, bool closed)
        {
            if (points.Length < 2) return;
            if (points.Length == 2) closed = false;

            // create segments
            Span<POINT2> segments = stackalloc POINT2[Parametric.ShapeFactory.GetLinesSegmentsVerticesCount(points.Length, closed)];
            Parametric.ShapeFactory.FillLinesSegments(segments, points, diameter, closed);

            // draw segments
            var segment = segments;
            while(segment.Length >= 4)
            {                
                dc.DrawPolygon(segment.Slice(0,4), style);
                segment = segment.Slice(4);
            }
        }        

        private static void _DrawLineAsPolygon(IDrawing2D dc, POINT2 a, POINT2 b, SCALAR diameter, ColorStyle brush, LineCapStyle startCapStyle, LineCapStyle endCapStyle)
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

        public static void DrawEllipseAsPolygon(this IDrawing2D dc, POINT2 center, SCALAR width, SCALAR height, ColorStyle style)
        {
            // calculate number of vertices based on dimensions
            int count = Math.Max((int)width, (int)height);
            if (count < 3) count = 3;

            Span<POINT2> points = stackalloc POINT2[count];
            Parametric.ShapeFactory.FillEllipseVertices(points, center, width, height);

            dc.DrawPolygon(points, style);
        }

        public static void DrawAssetAsPolygons(this IDrawing2D dc, in XFORM2 transform, ASSET asset, ColorStyle style)
        {
            // TODO: if dc is IAssetDrawing, call directly

            dc = dc.CreateTransformed2D(transform);
            dc.DrawAssetAsPolygons(asset, style);
        }

        public static void DrawAssetAsPolygons(this IDrawing2D dc, ASSET asset, ColorStyle style)
        {
            /*
            if (asset is Asset3D a3d)
            {
                a3d._DrawAsSurfaces(dc);
                return;
            }*/

            if (asset is Model2D mdl3D)
            {
                mdl3D.DrawTo(dc);
                return;
            }
        }

        #endregion
    }
}
