using System;
using System.Collections.Generic;
using System.Numerics;

using COLOR = System.Drawing.Color;
using BRUSH = System.Drawing.Brush;
using IMAGE = System.Drawing.Image;

namespace InteropDrawing.Backends
{
    public class SVGSceneDrawing2D : IDrawing2D, IDisposable
    {
        #region lifecycle

        public static SVGSceneDrawing2D CreateGDI(System.Drawing.Graphics gdi)
        {
            return new SVGSceneDrawing2D(new SvgNet.SvgGdi.GdiGraphics(gdi), true);
        }

        public static SVGSceneDrawing2D CreateGraphic()
        {
            return new SVGSceneDrawing2D(new SvgNet.SvgGdi.SvgGraphics(), false);
        }

        public SVGSceneDrawing2D(SvgNet.SvgGdi.IGraphics ig, bool canRenderBitmaps)
        {
            _Context = ig;
            _CanRenderBitmaps = canRenderBitmaps;
        }

        public void Dispose()
        {
            _Context = null;

            foreach (var b in _Brushes.Values) b.Dispose();
            _Brushes.Clear();

            foreach (var b in _Images.Values) b.Dispose();
            _Images.Clear();
        }

        #endregion

        #region data

        private SvgNet.SvgGdi.IGraphics _Context;
        private bool _CanRenderBitmaps;

        private readonly Dictionary<Int32, BRUSH> _Brushes = new Dictionary<Int32, BRUSH>();
        private readonly Dictionary<Object, IMAGE> _Images = new Dictionary<Object, IMAGE>();

        #endregion

        #region resources        

        private BRUSH _UseBrush(COLOR colorKey)
        {
            var key = colorKey.ToArgb();

            if (_Brushes.TryGetValue(key, out BRUSH b)) return b;

            b = new System.Drawing.SolidBrush(colorKey);

            _Brushes[key] = b;

            return b;
        }

        private IMAGE _UseImage(object imageKey)
        {
            if (_Images.TryGetValue(imageKey, out IMAGE b)) return b;

            var imagePath = imageKey as String;

            b = IMAGE.FromFile(imagePath); // make a lambda for it

            _Images[imageKey] = b;

            return b;
        }

        private static System.Drawing.Drawing2D.LineCap _ToDevice(LineCapStyle style)
        {
            switch (style)
            {
                case LineCapStyle.Round: return System.Drawing.Drawing2D.LineCap.Round;
                case LineCapStyle.Square: return System.Drawing.Drawing2D.LineCap.Square;
                case LineCapStyle.Triangle: return System.Drawing.Drawing2D.LineCap.Triangle;
                default: return System.Drawing.Drawing2D.LineCap.Flat;
            }
        }

        #endregion

        #region API - IDrawing2D

        /// <inheritdoc/>
        public void DrawAsset(in Matrix3x2 transform, object asset, in ColorStyle brush)
        {
            new Transforms.Decompose2D(this).DrawAsset(transform, asset, brush);
        }

        /// <inheritdoc/>
        public void FillConvexPolygon(ReadOnlySpan<Point2> points, COLOR color)
        {
            if (color.IsEmpty) return;

            var ppp = new System.Drawing.PointF[points.Length];

            for (int i = 0; i < ppp.Length; ++i)
            {
                ppp[i] = new System.Drawing.PointF(points[i].X, points[i].Y);
            }

            _Context.FillPolygon(_UseBrush(color), ppp);
        }

        /// <inheritdoc/>
        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, in LineStyle brush)
        {
            var sc = _ToDevice(brush.StartCap);
            var ec = _ToDevice(brush.EndCap);

            Span<System.Drawing.PointF> devicePoints = stackalloc System.Drawing.PointF[points.Length];

            for (int i = 0; i < devicePoints.Length; ++i)
            {
                devicePoints[i] = new System.Drawing.PointF(points[i].X, points[i].Y);
            }

            if (brush.Style.HasOutline)
            {
                _DrawLineInternal(devicePoints, diameter + brush.Style.OutlineWidth * 2, brush.Style.OutlineColor, sc, ec);
            }

            if (brush.Style.HasFill)
            {
                _DrawLineInternal(devicePoints, diameter, brush.Style.FillColor, sc, ec);
            }
        }

        
        private void _DrawLineInternal(ReadOnlySpan<System.Drawing.PointF> points, float diameter, COLOR color, System.Drawing.Drawing2D.LineCap startCap, System.Drawing.Drawing2D.LineCap endCap)
        {
            using (var pen = new System.Drawing.Pen(color, diameter))
            {
                pen.StartCap = startCap;
                pen.EndCap = endCap;

                if (points.Length == 2)
                {
                    _Context.DrawLine(pen, points[0], points[1]);
                }
                else if (points.Length > 2)
                {
                    _Context.DrawLines(pen, points.ToArray());
                }
            }
        }

        /// <inheritdoc/>
        public void DrawEllipse(Point2 center, float width, float height, in ColorStyle brush)
        {
            center -= new Point2(width * 0.5f, height * 0.5f);

            if (brush.HasFill) _Context.FillEllipse(_UseBrush(brush.FillColor), center.X, center.Y, width, height);

            if (brush.HasOutline)
            {
                using (var pen = new System.Drawing.Pen(brush.OutlineColor, brush.OutlineWidth))
                {
                    _Context.DrawEllipse(pen, center.X, center.Y, width, height);
                }
            }
        }

        /// <inheritdoc/>
        public void DrawPolygon(ReadOnlySpan<Point2> points, in PolygonStyle brush)
        {
            var ppp = new System.Drawing.PointF[points.Length];

            for (int i = 0; i < ppp.Length; ++i)
            {
                ppp[i] = new System.Drawing.PointF(points[i].X, points[i].Y);
            }

            if (brush.HasFill)
            {
                _Context.FillPolygon(_UseBrush(brush.FillColor), ppp);
            }

            if (brush.HasOutline)
            {
                using (var pen = new System.Drawing.Pen(brush.OutlineColor, brush.OutlineWidth))
                {
                    _Context.DrawPolygon(pen, ppp);
                }
            }
        }

        /// <inheritdoc/>
        public void DrawImage(in Matrix3x2 transform, in ImageStyle style)
        {
            if (!_CanRenderBitmaps) return;

            var bmp = style.Bitmap;

            var img = _UseImage(bmp.Source);
            if (img == null) return;

            var o = bmp.UV0;
            var s = bmp.UV2 - bmp.UV0;
            var srcRect = new System.Drawing.RectangleF(o.X, o.Y, s.X, s.Y);

            if (transform.M12 == 0 && transform.M21 == 0) // no rotation
            {
                var dstRect = new System.Drawing.RectangleF(transform.M31, transform.M32, transform.M11 * bmp.Scale, transform.M22 * bmp.Scale);                

                _Context.DrawImage(img, dstRect, srcRect, System.Drawing.GraphicsUnit.Pixel);
            }
            else
            {
                using (var matrix = new System.Drawing.Drawing2D.Matrix(transform.M11, transform.M12, transform.M21, transform.M22, transform.M31, transform.M32))
                {
                    _Context.MultiplyTransform(matrix);

                    var dstRect = new System.Drawing.RectangleF(transform.M31, transform.M32, transform.M11 * bmp.Scale, transform.M22 * bmp.Scale);
                    _Context.DrawImage(img, dstRect, srcRect, System.Drawing.GraphicsUnit.Pixel);

                    _Context.ResetTransform();
                }
            }            
        }

        public string ToSVGContent()
        {
            if (_Context is SvgNet.SvgGdi.SvgGraphics svg) return svg.WriteSVGString();
            return null;
        }

        #endregion

        #region static API

        public static void SaveToSVG(string filePath, IDrawingBrush<IDrawing2D> scene)
        {
            using (var svg = CreateGraphic())
            {
                scene.DrawTo(svg);

                var document = svg.ToSVGContent();

                System.IO.File.WriteAllText(filePath, document);
            }
        }        

        #endregion
    }
}
