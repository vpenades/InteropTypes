﻿using System;
using System.Numerics;

namespace InteropDrawing.Backends
{
    readonly struct _SimpleDrawingContext<TPixel> : IDrawing2D
        where TPixel: unmanaged
    {
        public _SimpleDrawingContext(InteropBitmaps.MemoryBitmap<TPixel> target, Converter<System.Drawing.Color, TPixel> converter)
        {
            _Target = target;
            _ColorConverter = converter;

            _PolygonRasterizer = new Helpers.PolygonScanlines(target.Width, target.Height);
        }

        private readonly InteropBitmaps.MemoryBitmap<TPixel> _Target;
        private readonly Converter<System.Drawing.Color, TPixel> _ColorConverter;

        private readonly Helpers.PolygonScanlines _PolygonRasterizer;

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
        {
            throw new NotImplementedException();
        }

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            throw new NotImplementedException();
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle style)
        {
            if (diameter > 1)
            {
                this.DrawLinesAsPolygons(points, diameter, style);
                return;
            }

            var pixColor = _ColorConverter(style.Style.FillColor);

            for (int i = 1; i < points.Length; ++i)
            {
                var a = points[i - 1];
                var b = points[i + 0];

                InteropBitmaps.DrawingExtensions.DrawPixelLine(_Target, a, b, pixColor);
            }
        }

        public void DrawEllipse(Point2 center, float width, float height, ColorStyle style)
        {
            this.DrawEllipseAsPolygon(center, width, height, style);
        }        

        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle style)
        {
            if (style.HasFill)
            {
                var fillColor = _ColorConverter(style.FillColor);

                foreach (var (y, xmin, xmax) in _PolygonRasterizer.GetScanlines(Point2.AsNumerics(points)))
                {
                    _Target
                        .UseScanlinePixels(y)
                        .Slice(xmin, xmax - xmin)
                        .Fill(fillColor);
                }
            }

            if (style.HasOutline)
            {
                var outColor = _ColorConverter(style.OutlineColor);

                for (int i = 1; i < points.Length; ++i)
                {
                    var a = points[i - 1];
                    var b = points[i + 0];

                    InteropBitmaps.DrawingExtensions.DrawPixelLine(_Target, a, b, outColor);
                }

                InteropBitmaps.DrawingExtensions.DrawPixelLine(_Target, points[points.Length - 1], points[0], outColor);
            }
        }
    }
}