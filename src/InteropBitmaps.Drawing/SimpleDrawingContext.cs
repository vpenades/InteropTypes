using System;
using System.Numerics;

namespace InteropDrawing.Backends
{
    [System.Diagnostics.DebuggerDisplay("{_ToDebuggerDisplay(),nq}")]
    sealed class _MemoryDrawingContext<TPixel> : IDrawing2D
        where TPixel: unmanaged
    {
        #region debug

        private string _ToDebuggerDisplay()
        {
            return $"IDrawing2D {_Target.Info.ToDebuggerDisplayString()}";
        }

        #endregion

        #region constructor
        public _MemoryDrawingContext(InteropBitmaps.MemoryBitmap<TPixel> target, Converter<System.Drawing.Color, TPixel> converter)
        {
            _Target = target;
            _ColorConverter = converter;

            _PolygonRasterizer = new Lazy<Helpers.PolygonScanlines>(() => new Helpers.PolygonScanlines(target.Width, target.Height));

            _Collapse = new Transforms.Decompose2D(this);
        }

        #endregion

        #region data

        private readonly InteropBitmaps.MemoryBitmap<TPixel> _Target;
        private readonly Converter<System.Drawing.Color, TPixel> _ColorConverter;

        private readonly Lazy<Helpers.PolygonScanlines> _PolygonRasterizer;

        private readonly Transforms.Decompose2D _Collapse;

        #endregion

        #region API        

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
        {
            _Collapse.DrawAsset(transform, asset, style);
        }

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            throw new NotImplementedException();
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle style)
        {
            if (diameter > 1)
            {
                _Collapse.DrawLines(points, diameter, style);
            }
            else
            {
                var pixColor = _ColorConverter(style.Style.FillColor);

                for (int i = 1; i < points.Length; ++i)
                {
                    var a = points[i - 1];
                    var b = points[i + 0];

                    InteropBitmaps.DrawingExtensions.DrawPixelLine(_Target, a, b, pixColor);
                }
            }
        }

        public void DrawEllipse(Point2 center, float width, float height, ColorStyle style)
        {
            _Collapse.DrawEllipse(center, width, height, style);
        }        

        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle style)
        {
            if (style.HasFill)
            {
                var fillColor = _ColorConverter(style.FillColor);

                foreach (var (y, xmin, xmax) in _PolygonRasterizer.Value.GetScanlines(Point2.AsNumerics(points)))
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

        #endregion
    }

    
}
