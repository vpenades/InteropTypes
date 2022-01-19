using System;
using System.Numerics;

namespace InteropDrawing.Backends
{
    [System.Diagnostics.DebuggerDisplay("{_ToDebuggerDisplay(),nq}")]
    class _MemoryDrawingContext<TPixel> :
        IDrawing2D,        
        IBackendViewportInfo,
        IServiceProvider

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

        internal readonly InteropBitmaps.MemoryBitmap<TPixel> _Target;
        private readonly Converter<System.Drawing.Color, TPixel> _ColorConverter;

        private readonly Lazy<Helpers.PolygonScanlines> _PolygonRasterizer;

        private readonly Transforms.Decompose2D _Collapse;

        #endregion

        #region properties

        public int PixelsWidth => _Target.Width;

        public int PixelsHeight => _Target.Height;

        #endregion

        #region API

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(InteropBitmaps.MemoryBitmap<TPixel>)) return _Target;
            if (serviceType == typeof(Transforms.Decompose2D)) return _Collapse;
            if (serviceType.IsAssignableFrom(this.GetType())) return this;
            return null;
        }        

        /// <inheritdoc/>
        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
        {
            _Collapse.DrawAsset(transform, asset, style);
        }

        /// <inheritdoc/>
        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            var dst = _Target.AsSpanBitmap();
            var xform = style.GetTransform() * transform;
            float opacity = style.Color.A;
            opacity /= 255f;

            if (style.Bitmap.Source is InteropBitmaps.IMemoryBitmap typeless)
            {
                var tsrc = typeless.AsSpanBitmap();

                xform = Matrix3x2.CreateScale(1f / tsrc.Width, 1f / tsrc.Height) * xform;

                switch (typeless.PixelFormat.Code)
                {
                    case InteropBitmaps.Pixel.BGRP32.Code:
                        {
                            var src = tsrc.OfType<InteropBitmaps.Pixel.BGRP32>();
                            dst.SetPixels(xform, src, opacity); return;
                        }
                    case InteropBitmaps.Pixel.RGBP32.Code:
                        {
                            var src = tsrc.OfType<InteropBitmaps.Pixel.RGBP32>();
                            dst.SetPixels(xform, src, opacity); return;
                        }
                    case InteropBitmaps.Pixel.BGRA32.Code:
                        {
                            var src = tsrc.OfType<InteropBitmaps.Pixel.BGRA32>();
                            dst.SetPixels(xform, src, opacity); return;
                        }
                    case InteropBitmaps.Pixel.RGBA32.Code:
                        {
                            var src = tsrc.OfType<InteropBitmaps.Pixel.RGBA32>();
                            dst.SetPixels(xform, src, opacity); return;
                        }
                    case InteropBitmaps.Pixel.BGR24.Code:
                        {
                            var src = tsrc.OfType<InteropBitmaps.Pixel.BGR24>();
                            dst.SetPixels(xform, src, opacity); return;
                        }
                    case InteropBitmaps.Pixel.RGB24.Code:
                        {
                            var src = tsrc.OfType<InteropBitmaps.Pixel.RGB24>();
                            dst.SetPixels(xform, src, opacity); return;
                        }
                }
            }            
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void DrawEllipse(Point2 center, float width, float height, ColorStyle style)
        {
            _Collapse.DrawEllipse(center, width, height, style);
        }

        /// <inheritdoc/>
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
