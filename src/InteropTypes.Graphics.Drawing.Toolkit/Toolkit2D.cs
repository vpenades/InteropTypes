using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace InteropTypes.Graphics.Drawing
{
    using PRIMITIVE2D = ICoreCanvas2D;
    using CANVAS2DEX = ICanvas2D;

    public static partial class DrawingToolkit
    {
        #region data

        private static readonly System.Threading.ThreadLocal<Record2D> _Model2DBounds = new System.Threading.ThreadLocal<Record2D>(() => new Record2D());

        #endregion

        #region 2D transforms        

        public static void TransformForward(this PRIMITIVE2D dc, Span<POINT2> points)
        {
            if (dc is ITransformer2D xformer) xformer.TransformForward(points);
        }

        public static void TransformInverse(this PRIMITIVE2D dc, Span<POINT2> points)
        {
            if (dc is ITransformer2D xformer) xformer.TransformInverse(points);
        }

        public static POINT2 TransformForward(this PRIMITIVE2D dc, POINT2 point)
        {
            return dc is ITransformer2D xformer
                ? xformer._TransformForward(point)
                : point;
        }

        public static POINT2 TransformInverse(this PRIMITIVE2D dc, POINT2 point)
        {return dc is ITransformer2D xformer
                ? xformer._TransformInverse(point)
                : point;
        }
        private static POINT2 _TransformForward(this ITransformer2D dc, POINT2 point)
        {
            Span<POINT2> span = stackalloc POINT2[1];
            span[0] = point;
            dc.TransformForward(span);
            return span[0];
        }

        private static POINT2 _TransformInverse(this ITransformer2D dc, POINT2 point)
        {
            Span<POINT2> span = stackalloc POINT2[1];
            span[0] = point;
            dc.TransformInverse(span);
            return span[0];
        }

        private static POINT2 _TransformNormalForward(this ITransformer2D dc, POINT2 normal)
        {
            Span<POINT2> span = stackalloc POINT2[1];
            span[0] = normal;
            dc.TransformNormalsForward(span);
            return span[0];
        }

        private static POINT2 _TransformNormalInverse(this ITransformer2D dc, POINT2 normal)
        {
            Span<POINT2> span = stackalloc POINT2[1];
            span[0] = normal;
            dc.TransformNormalsInverse(span);
            return span[0];
        }



        public static Transforms.Canvas2DTransform CreateTransformed(PRIMITIVE2D target, POINT2 physicalSize, POINT2 virtualSize, XFORM2 xform)            
        {
            return Transforms.Canvas2DTransform.Create(target, physicalSize, virtualSize, xform);
        }        

        public static CANVAS2DEX CreateTransformed(PRIMITIVE2D t, POINT2 physicalSize, GDIRECTF virtualBounds)
        {
            return Transforms.Canvas2DTransform.Create(t, physicalSize, virtualBounds);
        }

        #endregion

        #region assets

        public static void DrawAsset(this CANVAS2DEX dc, in XFORM2 transform, Object asset)
        {
            dc.DrawAsset(transform, asset);
        }        
        
        public static GDIRECTF? GetAssetBoundingRect(Object asset)
        {            
            if (asset is Record2D model2D) return model2D.BoundingRect;
            if (asset is IDrawingBrush<CANVAS2DEX> drawable)
            {                
                var mdl = _Model2DBounds.Value;
                mdl.Clear();

                drawable.DrawTo(mdl);
                return mdl.BoundingRect;
            }

            return null;
        }
        
        public static (XY Center,Single Radius)? GetAssetBoundingCircle(Object asset)
        {
            if (asset is Record2D model2D) return model2D.BoundingCircle;
            if (asset is IDrawingBrush<CANVAS2DEX> drawable)
            {
                var mdl = _Model2DBounds.Value;
                mdl.Clear();

                drawable.DrawTo(mdl);
                return mdl.BoundingCircle;
            }
            return null;
        }

        #endregion

        #region API

        public static COLOR WithAlpha(this COLOR color, int alpha)
        {
            return COLOR.FromArgb(alpha, color.R, color.G, color.B);
        }

        public static COLOR WithOpacity(this COLOR color, float opacity)
        {
            var alpha = opacity * (float)color.A;

            if (alpha < 0) alpha = 0;
            else if (alpha > 255) alpha = 255;

            return COLOR.FromArgb((int)alpha, color.R, color.G, color.B);
        }

        #endregion

        #region drawing        

        public static void DrawLines(this PRIMITIVE2D dc, ReadOnlySpan<POINT2> points, SCALAR diameter, in LineStyle style)
        {
            Transforms.Decompose2D.DrawLines(dc, points, diameter, style);
        }

        public static void DrawPolygons(this PRIMITIVE2D dc, ReadOnlySpan<POINT2> points, in PolygonStyle style)
        {
            Transforms.Decompose2D.DrawPolygon(dc, points, style);
        }

        public static void DrawEllipse(this PRIMITIVE2D dc, POINT2 center, float dx, float dy, in OutlineFillStyle style)
        {
            Transforms.Decompose2D.DrawEllipse(dc, center, dx, dy, style);
        }

        public static void DrawLines(this CANVAS2DEX dc, SCALAR diameter, LineStyle style, params POINT2[] points)
        {
            dc.DrawLines(points, diameter, style);
        }

        public static void DrawLine(this CANVAS2DEX dc, POINT2 a, POINT2 b, SCALAR diameter, in LineStyle style)
        {
            Span<POINT2> pp = stackalloc POINT2[2];
            pp[0] = a;
            pp[1] = b;

            dc.DrawLines(pp, diameter, style);
        }

        public static void DrawLine(this PRIMITIVE2D dc, POINT2 a, POINT2 b, float diameter, in ImageStyle style)
        {
            var aa = a.XY;
            var ab = b.XY - aa;            

            var imageXform = style.GetTransform();
            var sx = new XY(imageXform.M11, imageXform.M12).Length();
            var sy = new XY(imageXform.M21, imageXform.M22).Length();
            var sxx = sx + imageXform.M31 * 2; // subtract pivot "head" and "tail"

            var s = new XY(ab.Length(), diameter) / new XY(sxx * sx, sy * sy);
            var r = MathF.Atan2(ab.Y, ab.X);
            var xform = XFORM2.CreateScale(s) * imageXform * XFORM2.CreateRotation(r);

            xform.Translation = aa;

            dc.DrawImage(xform, style);
        }

        public static void DrawCircle(this CANVAS2DEX dc, POINT2 center, SCALAR diameter, in OutlineFillStyle style)
        {
            dc.DrawEllipse(center, diameter, diameter, style);
        }

        public static void DrawRectangle(this CANVAS2DEX dc, GDIRECTF rect, in OutlineFillStyle style)
        {
            dc.DrawRectangle(rect.Location, rect.Size, style);
        }

        public static void DrawRectangle(this CANVAS2DEX dc, XFORM2 rect, in OutlineFillStyle style)
        {
            Span<POINT2> vertices = stackalloc POINT2[4];

            var scaleX = new XY(rect.M11, rect.M12);
            var scaleY = new XY(rect.M21, rect.M22);
            var origin = new XY(rect.M31, rect.M32);

            vertices[0] = origin;
            vertices[1] = origin + scaleX;
            vertices[2] = origin + scaleX + scaleY;
            vertices[3] = origin + scaleY;

            dc.DrawPolygon(vertices, style);
        }

        public static void DrawRectangle(this CANVAS2DEX dc, XFORM2 xform, GDIRECTF rect, in OutlineFillStyle style)
        {
            Span<POINT2> vertices = stackalloc POINT2[4];
            POINT2.FromRect(vertices, rect, false);
            POINT2.Transform(vertices, xform);
            dc.DrawPolygon(vertices, style);
        }

        public static void DrawRectangle(this CANVAS2DEX dc, POINT2 origin, POINT2 size, in OutlineFillStyle style)
        {
            Span<POINT2> vertices = stackalloc POINT2[4];
            
            Parametric.ShapeFactory2D.FillRectangleVertices(vertices, origin, size, 0, 0);

            dc.DrawPolygon(vertices, style);
        }

        public static void DrawRectangle(this CANVAS2DEX dc, GDIRECTF rect, in OutlineFillStyle style, float borderRadius, int arcVertexCount = 6)
        {
            dc.DrawRectangle(rect.Location, rect.Size, style, borderRadius, arcVertexCount);
        }

        public static void DrawRectangle(this CANVAS2DEX dc, POINT2 origin, POINT2 size, in OutlineFillStyle style, float borderRadius, int arcVertexCount = 6)
        {
            if (borderRadius == 0) arcVertexCount = 0;
            
            Span<POINT2> vertices = stackalloc POINT2[Parametric.ShapeFactory2D.GetRectangleVertexCount(arcVertexCount)];
            
            Parametric.ShapeFactory2D.FillRectangleVertices(vertices, origin, size, borderRadius, arcVertexCount);

            dc.DrawPolygon(vertices, style);
        }        

        public static void DrawPolygon(this CANVAS2DEX dc, in OutlineFillStyle style, params POINT2[] points)
        {
            dc.DrawPolygon(points, style);
        }        

        public static void DrawPath(this CANVAS2DEX dc, XFORM2 xform, string path, OutlineFillStyle style)
        {
            Parametric.PathParser.DrawPath(dc, xform, path, style);
        }

        public static void DrawImageBackground(this PRIMITIVE2D dc, ImageStyle style)
        {
            var vflip = false;

            if (dc.TryGetQuadrant(out var quadrant))
            {
                if (quadrant.HasFlag(Quadrant.Top)) vflip = true;
            }

            if (dc.TryGetBackendViewportBounds(out var screenBounds))
            {
                var screenXform = XFORM2.CreateScale(screenBounds.Width, screenBounds.Height);
                screenXform.Translation = new XY(screenBounds.X, screenBounds.Y);

                XFORM2.Invert(style.GetTransform(false, vflip), out var imageXform);

                var xform = imageXform * screenXform;

                dc.DrawImage(xform, style);
            }
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
