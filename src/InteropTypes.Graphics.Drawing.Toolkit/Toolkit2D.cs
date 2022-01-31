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
    using PRIMITIVE2D = IPrimitiveCanvas2D;
    using CANVAS2DEX = ICanvas2D;

    public static partial class Toolkit
    {
        #region data

        private static readonly System.Threading.ThreadLocal<Record2D> _Model2DBounds = new System.Threading.ThreadLocal<Record2D>(() => new Record2D());

        #endregion

        #region 2D transforms


        public static bool TryGetBackendViewportBounds(this PRIMITIVE2D dc, out BRECT bounds)
        {
            bounds = BRECT.Empty;

            if (!(dc is IServiceProvider srv)) return false;


/* Unmerged change from project 'InteropTypes.Graphics.Drawing.Toolkit (netstandard2.1)'
Before:
            var vinfo = srv.GetService(typeof(Backends.IBackendViewportInfo)) as Backends.IBackendViewportInfo;
After:
            var vinfo = srv.GetService(typeof(IBackendViewportInfo)) as IBackendViewportInfo;
*/
            var vinfo = srv.GetService(typeof(InteropDrawing.IBackendViewportInfo)) as InteropDrawing.IBackendViewportInfo;
            if (vinfo == null) return false;

            if (!(dc is Transforms.ITransformer2D xform)) return false;
            
            Span<POINT2> points = stackalloc POINT2[4];
            points[0] = (0,0);
            points[1] = (vinfo.PixelsWidth, 0);
            points[2] = (0, vinfo.PixelsHeight);
            points[3] = (vinfo.PixelsWidth, vinfo.PixelsHeight);
            xform.TransformInverse(points);

            foreach(var p in points)
            {
                bounds = BRECT.Union(bounds, new BRECT(p.X, p.Y, 0, 0));
            }            
                
            return true;            
        }

        public static bool TryGetQuadrant(this PRIMITIVE2D dc, out Quadrant quadrant)
        {
            if (dc is Transforms.ITransformer2D xform)
            {
                Span<POINT2> points = stackalloc POINT2[1];                
                points[0] = VECTOR2.One;
                xform.TransformNormalsForward(points);
                quadrant = GetQuadrant(points[0].ToNumerics());
                return true;
            }
            else
            {
                quadrant = default;
                return false;
            }
        }

        /// <summary>
        /// Determines the predominant quadrant from a given transform matrix.
        /// </summary>
        /// <param name="transform">The viewport transform matrix</param>
        /// <returns>The positive quadrant</returns>
        public static Quadrant GetQuadrant(in XFORM2 transform)
        {
            var p0 = VECTOR2.Transform(VECTOR2.Zero,transform);
            var p1 = VECTOR2.Transform(VECTOR2.One, transform);

            return GetQuadrant(p1 - p0);
        }

        public static Quadrant GetQuadrant(in VECTOR2 direction)
        {
            var q = Quadrant.Origin;
            if (direction.X < 0) q |= Quadrant.Left;
            else if (direction.X > 0) q |= Quadrant.Right;

            if (direction.Y < 0) q |= Quadrant.Bottom;
            else if (direction.Y > 0) q |= Quadrant.Top;

            return q;
        }

        public static void TransformForward(this PRIMITIVE2D dc, Span<POINT2> points)
        {
            if (dc is Transforms.ITransformer2D xformer) xformer.TransformForward(points);
        }

        public static void TransformInverse(this PRIMITIVE2D dc, Span<POINT2> points)
        {
            if (dc is Transforms.ITransformer2D xformer) xformer.TransformInverse(points);
        }

        public static POINT2 TransformForward(this PRIMITIVE2D dc, POINT2 point)
        {
            return dc is Transforms.ITransformer2D xformer
                ? xformer._TransformForward(point)
                : point;
        }

        public static POINT2 TransformInverse(this PRIMITIVE2D dc, POINT2 point)
        {
            return dc is Transforms.ITransformer2D xformer
                ? xformer._TransformInverse(point)
                : point;
        }

        private static POINT2 _TransformForward(this Transforms.ITransformer2D dc, POINT2 point)
        {
            Span<POINT2> span = stackalloc POINT2[1];
            span[0] = point;
            dc.TransformForward(span);
            return span[0];
        }

        private static POINT2 _TransformInverse(this Transforms.ITransformer2D dc, POINT2 point)
        {
            Span<POINT2> span = stackalloc POINT2[1];
            span[0] = point;
            dc.TransformInverse(span);
            return span[0];
        }

        private static POINT2 _TransformNormalForward(this Transforms.ITransformer2D dc, POINT2 normal)
        {
            Span<POINT2> span = stackalloc POINT2[1];
            span[0] = normal;
            dc.TransformNormalsForward(span);
            return span[0];
        }

        private static POINT2 _TransformNormalInverse(this Transforms.ITransformer2D dc, POINT2 normal)
        {
            Span<POINT2> span = stackalloc POINT2[1];
            span[0] = normal;
            dc.TransformNormalsInverse(span);
            return span[0];
        }

        public static Transforms.Drawing2DTransform CreateTransformed(PRIMITIVE2D target, POINT2 physicalSize, POINT2 virtualSize, XFORM2 xform)            
        {
            return Transforms.Drawing2DTransform.Create(target, physicalSize, virtualSize, xform);
        }

        public static ICanvas2D CreateTransformed(PRIMITIVE2D t, POINT2 physicalSize, POINT2 virtualSize)
        {
            return Transforms.Drawing2DTransform.Create(t, physicalSize, virtualSize);
        }        

        public static ICanvas2D CreateTransformed(PRIMITIVE2D t, POINT2 physicalSize, BRECT virtualBounds)
        {
            return Transforms.Drawing2DTransform.Create(t, physicalSize, virtualBounds);
        }

        #endregion

        #region assets

        public static void DrawAsset(this CANVAS2DEX dc, in XFORM2 transform, Object asset)
        {
            dc.DrawAsset(transform, asset, ColorStyle.White);
        }        
        
        public static BRECT? GetAssetBoundingRect(Object asset)
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
        
        public static (VECTOR2 Center,Single Radius)? GetAssetBoundingCircle(Object asset)
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
            var aa = a.ToNumerics();
            var ab = b.ToNumerics() - aa;

            var brush = style.Bitmap;
            var brushLen = brush.Width - brush.Pivot.X * 2;

            var ss = new VECTOR2(ab.Length(), diameter) / new VECTOR2(brushLen, brush.Height);
            var rr = MathF.Atan2(ab.Y, ab.X);

            var xform = XFORM2.CreateScale(ss);
            xform *= XFORM2.CreateRotation(rr);

            xform.Translation = aa;

            dc.DrawImage(xform, style);
        }

        public static void DrawCircle(this CANVAS2DEX dc, POINT2 center, SCALAR diameter, in OutlineFillStyle style)
        {
            dc.DrawEllipse(center, diameter, diameter, style);
        }

        public static void DrawRectangle(this CANVAS2DEX dc, BRECT rect, in OutlineFillStyle style)
        {
            dc.DrawRectangle(rect.Location, rect.Size, style);
        }

        public static void DrawRectangle(this CANVAS2DEX dc, XFORM2 rect, in OutlineFillStyle style)
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

        public static void DrawRectangle(this CANVAS2DEX dc, POINT2 origin, POINT2 size, in OutlineFillStyle style)
        {
            Span<POINT2> vertices = stackalloc POINT2[4];

            Parametric.ShapeFactory2D.FillRectangleVertices(vertices, origin, size, 0, 0);

            dc.DrawPolygon(vertices, style);
        }

        public static void DrawRectangle(this CANVAS2DEX dc, BRECT rect, in OutlineFillStyle style, float borderRadius, int arcVertexCount = 6)
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

        public static void DrawFont(this CANVAS2DEX dc, POINT2 origin, float size, String text, FontStyle style)
        {
            var xform = XFORM2.CreateScale(size);
            xform.Translation = origin.ToNumerics();

            style = style.With(style.Strength * size);

            dc.DrawFont(xform, text, style);
        }

        public static void DrawFont(this CANVAS2DEX dc, XFORM2 xform, String text, FontStyle style)
        {
            float xflip = 1;
            float yflip = 1;            

            if (style.Alignment.HasFlag(FontAlignStyle.FlipHorizontal)) { xflip = -1; }
            if (style.Alignment.HasFlag(FontAlignStyle.FlipVertical)) { yflip = -1; }            

            if (style.Alignment.HasFlag(FontAlignStyle.FlipAuto) && dc.TryGetQuadrant(out var q))
            {
                if (q.HasFlag(Quadrant.Bottom)) yflip *= -1;
            }            

            style = style.With(style.Alignment & ~(FontAlignStyle.FlipHorizontal | FontAlignStyle.FlipVertical));

            xform = XFORM2.CreateScale(xflip, yflip) * xform;

            Fonts.FontDrawing.DrawFontAsLines(dc, xform, text, style);
        }

        public static void DrawPath(this CANVAS2DEX dc, XFORM2 xform, string path, OutlineFillStyle style)
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
