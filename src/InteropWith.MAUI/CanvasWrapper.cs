using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropDrawing;

namespace InteropWith.MAUI
{
    public readonly struct CanvasWrapper : IDrawing2D
    {
        private readonly Microsoft.Maui.Graphics.ICanvas _Canvas;

        private void _SetColorStyle(in ColorStyle style)
        {
            // _Canvas.BlendMode = Microsoft.Maui.Graphics.BlendMode.Overlay;
            _Canvas.FillColor = new Microsoft.Maui.Graphics.Color(style.FillColor.ToArgb());
            _Canvas.StrokeColor = new Microsoft.Maui.Graphics.Color(style.OutlineColor.ToArgb());
            _Canvas.StrokeSize = style.OutlineWidth;
        }

        private void _SetLineStyle(in LineStyle style)
        {
            // _Canvas.BlendMode = Microsoft.Maui.Graphics.BlendMode.Overlay;
            _Canvas.FillColor = new Microsoft.Maui.Graphics.Color(0);
            _Canvas.StrokeColor = new Microsoft.Maui.Graphics.Color(style.Style.FillColor.ToArgb());
            _Canvas.StrokeSize = style.Style.OutlineWidth;
        }

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
        {
            throw new NotImplementedException();
        }

        public void DrawEllipse(Point2 center, float width, float height, ColorStyle style)
        {
            _SetColorStyle(style);
            _Canvas.DrawEllipse(center.X, center.Y, width, height);
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle style)
        {
            _SetLineStyle(style);

            var path = new Microsoft.Maui.Graphics.PathF(points[0].X, points[0].Y);

            for (int i = 1; i < points.Length; ++i)
            {
                path.LineTo(points[i].X, points[i].Y);
            }            

            _Canvas.DrawPath(path);
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle style)
        {
            _SetColorStyle(style);

            var path = new Microsoft.Maui.Graphics.PathF(points[0].X, points[0].Y);            

            for(int i=1; i < points.Length; ++i)
            {
                path.LineTo(points[i].X, points[i].Y);
            }

            path.Close();

            _Canvas.DrawPath(path);
        }

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            throw new NotImplementedException();
        }
    }
}
