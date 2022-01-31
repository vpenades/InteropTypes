using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropDrawing;

using COLOR = System.Drawing.Color;

namespace InteropBitmaps.Adapters
{
    class GDIDrawingAdapter : ICanvas2D
    {
        public GDIDrawingAdapter(System.Drawing.Graphics context)
        {
            _Context = context;
        }

        private readonly System.Drawing.Graphics _Context;

        private readonly System.Drawing.PointF[] _Points1 = new System.Drawing.PointF[1];
        private readonly System.Drawing.PointF[] _Points2 = new System.Drawing.PointF[2];
        private readonly System.Drawing.PointF[] _Points3 = new System.Drawing.PointF[3];
        private readonly System.Drawing.PointF[] _Points4 = new System.Drawing.PointF[4];
        private readonly System.Drawing.PointF[] _Points5 = new System.Drawing.PointF[5];
        private readonly System.Drawing.PointF[] _Points6 = new System.Drawing.PointF[6];
        private System.Drawing.PointF[] _PointsX;

        private System.Drawing.PointF[] _UsePoints(int count)
        {
            switch(count)
            {
                case 1: return _Points1;
                case 2: return _Points2;
                case 3: return _Points3;
                case 4: return _Points4;
                case 5: return _Points5;
                case 6: return _Points6;
            }

            Array.Resize(ref _PointsX, count);
            return _PointsX;
        }

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
        {
            throw new NotImplementedException();
        }

        public void DrawEllipse(Point2 center, float width, float height, in OutlineFillStyle style)
        {
            var rect = new System.Drawing.RectangleF(center.X - width * 0.5f, center.Y - height * 0.5f, width, height);

            if (style.HasFill)
            {
                using (var brush = new System.Drawing.SolidBrush(style.FillColor))
                {
                    _Context.FillEllipse(brush, rect);
                }
            }
            if (style.HasOutline)
            {
                using (var pen = new System.Drawing.Pen(style.OutlineColor, style.OutlineWidth))
                {
                    _Context.DrawEllipse(pen, rect);
                }
            }
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, in LineStyle style)
        {
            var gdiPts = _UsePoints(points.Length);
            for (int i = 0; i < points.Length; ++i) gdiPts[i] = points[i].ToGDIPoint();
            
            using (var pen = new System.Drawing.Pen(style.Style.FillColor, diameter))
            {
                _Context.DrawLines(pen, gdiPts);
            }            
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, in PolygonStyle style)
        {
            var gdiPts = _UsePoints(points.Length);
            for (int i = 0; i < points.Length; ++i) gdiPts[i] = points[i].ToGDIPoint();

            if (style.HasFill)
            {
                using(var brush = new System.Drawing.SolidBrush(style.FillColor))
                {
                    _Context.FillPolygon(brush, gdiPts);
                }
            }
            if (style.HasOutline)
            {
                using (var pen = new System.Drawing.Pen(style.OutlineColor, style.OutlineWidth))
                {
                    _Context.DrawPolygon(pen, gdiPts);
                }
            }
        }        

        public void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {
            var gdiPts = _UsePoints(points.Length);
            for (int i = 0; i < points.Length; ++i) gdiPts[i] = points[i].ToGDIPoint();

            using (var brush = new System.Drawing.SolidBrush(color.Color))
            {
                _Context.FillPolygon(brush, gdiPts);
            }
        }

        public void DrawImage(in Matrix3x2 transform, in ImageStyle style)
        {

        }
    }
}
