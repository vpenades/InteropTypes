using System;
using System.Drawing;
using System.Numerics;

using InteropTypes.Graphics.Drawing;
using InteropTypes.Graphics.Drawing.Transforms;

using GDICOLOR = System.Drawing.Color;

namespace InteropDrawing.Backends
{
    [System.Diagnostics.DebuggerDisplay("{_ToDebuggerDisplay(),nq}")]
    class _MemoryDrawingContext<TPixel> :
        Decompose2D.PassToSelf,
        InteropTypes.Graphics.Backends.IBackendCanvas2D,
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
        public _MemoryDrawingContext(InteropBitmaps.MemoryBitmap<TPixel> target, Converter<GDICOLOR, TPixel> converter)
        {
            _Target = target;
            _ColorConverter = converter;

            _PolygonRasterizer = new Lazy<Helpers.PolygonScanlines>(() => new Helpers.PolygonScanlines(target.Width, target.Height));            
        }

        #endregion

        #region data

        internal readonly InteropBitmaps.MemoryBitmap<TPixel> _Target;
        private readonly Converter<GDICOLOR, TPixel> _ColorConverter;

        private readonly Lazy<Helpers.PolygonScanlines> _PolygonRasterizer;        

        #endregion

        #region properties

        /// <inheritdoc/>
        public int PixelsWidth => _Target.Width;

        /// <inheritdoc/>
        public int PixelsHeight => _Target.Height;

        /// <inheritdoc/>
        public float DotsPerInchX => 96;

        /// <inheritdoc/>
        public float DotsPerInchY => 96;

        #endregion

        #region API

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(InteropBitmaps.MemoryBitmap<TPixel>)) return _Target;            
            if (serviceType.IsAssignableFrom(this.GetType())) return this;
            return null;
        }        

        /// <inheritdoc/>
        public sealed override void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            var dst = _Target.AsSpanBitmap();            

            var xform = style.GetTransform() * transform;
            float opacity = style.Color.A;
            opacity /= 255f;

            if (style.Bitmap.Source is InteropBitmaps.IMemoryBitmap typeless)
            {
                style.Bitmap.WithImageSize(typeless.Width, typeless.Height);

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
        public sealed override void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {
            var fillColor = _ColorConverter(color.ToGDI());

            foreach (var (y, xmin, xmax) in _PolygonRasterizer.Value.GetScanlines(Point2.AsNumerics(points)))
            {
                _Target
                    .UseScanlinePixels(y)
                    .Slice(xmin, xmax - xmin)
                    .Fill(fillColor);
            }
        }

        /// <inheritdoc/>
        public float GetPixelsPerUnit() { return 1; }

        /// <inheritdoc/>
        public void DrawThinLines(ReadOnlySpan<Point2> points, float width, ColorStyle color)
        {
            var outColor = _ColorConverter(color.ToGDI());

            for (int i = 1; i < points.Length; ++i)
            {
                var a = points[i - 1];
                var b = points[i + 0];

                InteropBitmaps.DrawingExtensions.DrawPixelLine(_Target, a, b, outColor);
            }
        }

        #endregion
    }

}
