using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

using InteropDrawing;

namespace InteropWith.MAUI
{
    public readonly struct CanvasWrapper : IDrawing2D
    {
        public static IDrawing2D Create(Microsoft.Maui.Graphics.ICanvas canvas)
        {
            return new CanvasWrapper(canvas);
        }

        private CanvasWrapper(Microsoft.Maui.Graphics.ICanvas canvas)
        {
            _Canvas = canvas;
        }

        private readonly Microsoft.Maui.Graphics.ICanvas _Canvas;

        private void _SetColorStyle(in ColorStyle style)
        {
            _Canvas.BlendMode = Microsoft.Maui.Graphics.BlendMode.Normal;
            _Canvas.FillColor = style.FillColor.ToMaui();
            _Canvas.StrokeColor = style.OutlineColor.ToMaui();
            _Canvas.StrokeSize = style.OutlineWidth;
        }

        private void _SetColorStyle(in PolygonStyle style)
        {
            _Canvas.BlendMode = Microsoft.Maui.Graphics.BlendMode.Normal;
            _Canvas.FillColor = style.FillColor.ToMaui();
            _Canvas.StrokeColor = style.OutlineColor.ToMaui();
            _Canvas.StrokeSize = style.OutlineWidth;
        }

        private void _SetLineStyle(in LineStyle style)
        {
            // _Canvas.BlendMode = Microsoft.Maui.Graphics.BlendMode.Overlay;
            _Canvas.FillColor = new Microsoft.Maui.Graphics.Color(0);
            _Canvas.StrokeColor = new Microsoft.Maui.Graphics.Color(style.Style.FillColor.ToArgb());
            _Canvas.StrokeSize = style.Style.OutlineWidth;
        }

        public void DrawAsset(in Matrix3x2 transform, object asset, in ColorStyle style)
        {
            throw new NotImplementedException();
        }

        public void DrawEllipse(Point2 center, float width, float height, in ColorStyle style)
        {
            _SetColorStyle(style);

            if (style.HasFill)
            {
                _Canvas.FillEllipse(center.X, center.Y, width, height);
            }

            if (style.HasOutline)
            {
                _Canvas.DrawEllipse(center.X, center.Y, width, height);
            }            
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, in LineStyle style)
        {
            using (var path = new Microsoft.Maui.Graphics.PathF(points[0].X, points[0].Y))
            {
                for (int i = 1; i < points.Length; ++i)
                {
                    path.LineTo(points[i].X, points[i].Y);
                }

                _SetLineStyle(style);
                _Canvas.DrawPath(path);
            }
        }

        public void FillConvexPolygon(ReadOnlySpan<Point2> points, Color color)
        {
            using (var path = new Microsoft.Maui.Graphics.PathF(points[0].X, points[0].Y))
            {
                for (int i = 1; i < points.Length; ++i)
                {
                    path.LineTo(points[i].X, points[i].Y);
                }

                path.Close();

                _SetColorStyle(color);

                _Canvas.FillPath(path, Microsoft.Maui.Graphics.WindingMode.NonZero);                
            }
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, in PolygonStyle style)
        {
            using (var path = new Microsoft.Maui.Graphics.PathF(points[0].X, points[0].Y))
            {
                for (int i = 1; i < points.Length; ++i)
                {
                    path.LineTo(points[i].X, points[i].Y);
                }

                path.Close();

                _SetColorStyle(style);

                if (style.HasFill) _Canvas.FillPath(path, Microsoft.Maui.Graphics.WindingMode.NonZero);
                if (style.HasOutline) _Canvas.DrawPath(path);
            }
        }

        public void DrawImage(in Matrix3x2 transform, in ImageStyle style)
        {
            throw new NotImplementedException();
        }        
    }
}
