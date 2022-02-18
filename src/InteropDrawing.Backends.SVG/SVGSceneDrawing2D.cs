using System;
using System.Collections.Generic;
using System.Numerics;

using BRUSH = System.Drawing.Brush;
using IMAGE = System.Drawing.Image;
using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends
{
    public class SVGSceneDrawing2D : ICanvas2D, IDisposable
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

        private readonly Dictionary<UInt32, BRUSH> _Brushes = new Dictionary<UInt32, BRUSH>();
        private readonly Dictionary<Object, IMAGE> _Images = new Dictionary<Object, IMAGE>();

        #endregion

        #region resources        

        private BRUSH _UseBrush(ColorStyle colorKey)
        {
            var key = colorKey.Packed;

            if (_Brushes.TryGetValue(key, out BRUSH b)) return b;

            b = new System.Drawing.SolidBrush(colorKey.ToGDI());

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
        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle color)
        {
            new InteropTypes.Graphics.Drawing.Transforms.Decompose2D(this).DrawAsset(transform, asset, color);
        }

        /// <inheritdoc/>
        public void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {
            if (!color.IsVisible) return;

            var ppp = new System.Drawing.PointF[points.Length];

            for (int i = 0; i < ppp.Length; ++i)
            {
                ppp[i] = new System.Drawing.PointF(points[i].X, points[i].Y);
            }

            _Context.FillPolygon(_UseBrush(color.ToGDI()), ppp);
        }

        /// <inheritdoc/>
        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle brush)
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

        
        private void _DrawLineInternal(ReadOnlySpan<System.Drawing.PointF> points, float diameter, ColorStyle color, System.Drawing.Drawing2D.LineCap startCap, System.Drawing.Drawing2D.LineCap endCap)
        {
            using (var pen = new System.Drawing.Pen(color.ToGDI(), diameter))
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
        public void DrawEllipse(Point2 center, float width, float height, OutlineFillStyle brush)
        {
            center -= new Point2(width * 0.5f, height * 0.5f);

            if (brush.HasFill) _Context.FillEllipse(_UseBrush(brush.FillColor), center.X, center.Y, width, height);

            if (brush.HasOutline)
            {
                using (var pen = new System.Drawing.Pen(brush.OutlineColor.ToGDI(), brush.OutlineWidth))
                {
                    _Context.DrawEllipse(pen, center.X, center.Y, width, height);
                }
            }
        }

        /// <inheritdoc/>
        public void DrawPolygon(ReadOnlySpan<Point2> points, PolygonStyle brush)
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
                using (var pen = new System.Drawing.Pen(brush.OutlineColor.ToGDI(), brush.OutlineWidth))
                {
                    _Context.DrawPolygon(pen, ppp);
                }
            }
        }

        /// <inheritdoc/>
        public void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            if (!_CanRenderBitmaps) return;

            var bmp = style.Image;

            var img = _UseImage(bmp.Source);
            if (img == null) return;

            style.Image.WithSourceSize(img.Width, img.Height);

            var xform = style.GetTransform() * transform;

            var srcRect = bmp.GetSourceRectangle();            

            if (xform.M12 == 0 && xform.M21 == 0) // no rotation
            {
                var dstRect = new System.Drawing.RectangleF(xform.M31, xform.M32, xform.M11, xform.M22);

                _Context.DrawImage(img, dstRect, srcRect, System.Drawing.GraphicsUnit.Pixel);
            }
            else
            {
                using (var matrix = new System.Drawing.Drawing2D.Matrix(xform.M11, xform.M12, xform.M21, xform.M22, xform.M31, xform.M32))
                {
                    _Context.MultiplyTransform(matrix);

                    var dstRect = new System.Drawing.RectangleF(xform.M31, xform.M32, xform.M11, xform.M22);
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

        public static void SaveToSVG(string filePath, IDrawingBrush<ICanvas2D> scene)
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
