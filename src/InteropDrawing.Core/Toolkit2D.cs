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
            if (asset is IDrawable2D drawable)
            {
                var mdl = new Model2D();
                drawable.DrawTo(mdl);
                return mdl.BoundingRect;
            }

            return null;
        }
        
        public static (VECTOR2 Center,Single Radius)? GetAssetBoundingCircle(Object asset)
        {
            if (asset is Model2D model2D) return model2D.BoundingCircle;
            if (asset is IDrawable2D drawable)
            {
                var mdl = new Model2D();
                drawable.DrawTo(mdl);
                return mdl.BoundingCircle;
            }
            return null;
        }        

        #endregion

        #region drawing        

        public static void DrawLines(this IDrawing2D dc, SCALAR diameter, LineStyle style, params POINT2[] points)
        {
            dc.DrawLines(points, diameter, style);
        }

        public static void DrawLine(this IDrawing2D dc, POINT2 a, POINT2 b, SCALAR diameter, LineStyle style)
        {
            Span<POINT2> pp = stackalloc POINT2[2];
            pp[0] = a;
            pp[1] = b;

            dc.DrawLines(pp, diameter, style);
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

        public static void DrawCircle(this IDrawing2D dc, POINT2 center, SCALAR diameter, ColorStyle style)
        {
            dc.DrawEllipse(center, diameter, diameter, style);
        }

        public static void DrawRectangle(this IDrawing2D dc, BRECT rect, ColorStyle style)
        {
            dc.DrawRectangle(rect.Location, rect.Size, style);
        }

        public static void DrawRectangle(this IDrawing2D dc, XFORM2 rect, ColorStyle style)
        {
            Span<POINT2> vertices = stackalloc POINT2[4];

            var scaleX = new VECTOR2(rect.M11, rect.M12);
            var scaleY = new VECTOR2(rect.M21, rect.M22);
            var origin = new VECTOR2(rect.M31, rect.M32);

            vertices[0] = origin;
            vertices[1] = origin + scaleX;
            vertices[2] = origin + scaleX + scaleY;
            vertices[3] = origin + scaleY;

            dc.DrawPolygon(vertices, style);
        }

        public static void DrawRectangle(this IDrawing2D dc, POINT2 origin, POINT2 size, ColorStyle style)
        {
            Span<POINT2> vertices = stackalloc POINT2[4];

            Parametric.ShapeFactory.FillRectangleVertices(vertices, origin, size, 0, 0);

            dc.DrawPolygon(vertices, style);
        }

        public static void DrawRectangle(this IDrawing2D dc, BRECT rect, ColorStyle style, float borderRadius, int arcVertexCount = 6)
        {
            dc.DrawRectangle(rect.Location, rect.Size, style, borderRadius, arcVertexCount);
        }

        public static void DrawRectangle(this IDrawing2D dc, POINT2 origin, POINT2 size, ColorStyle style, float borderRadius, int arcVertexCount = 6)
        {
            if (borderRadius == 0) arcVertexCount = 0;

            Span<POINT2> vertices = stackalloc POINT2[Parametric.ShapeFactory.GetRectangleVertexCount(arcVertexCount)];

            Parametric.ShapeFactory.FillRectangleVertices(vertices, origin, size, borderRadius, arcVertexCount);

            dc.DrawPolygon(vertices, style);
        }        

        public static void DrawPolygon(this IDrawing2D dc, ColorStyle style, params POINT2[] points)
        {
            dc.DrawPolygon(points, style);
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

        public static void DrawPath(this IDrawing2D dc, XFORM2 xform, string path, ColorStyle style)
        {
            Parametric.PathParser.DrawPath(dc, xform, path, style);
        }

        #endregion

        #region drawing as polygons

        public static LineStyle GetLineSegmentStyle(this LineStyle brush, int pointCount, int index)
        {
            var startCap = (index - 1) == 0 ? brush.StartCap : LineCapStyle.Triangle;
            var endCap = (index + 1) == pointCount ? brush.EndCap : LineCapStyle.Triangle;

            return new LineStyle(brush.Style, startCap, endCap);
        }

        #endregion
    }
}
