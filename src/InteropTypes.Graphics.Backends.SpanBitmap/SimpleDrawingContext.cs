using System;
using System.Drawing;
using System.Numerics;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Drawing;
using InteropTypes.Graphics.Drawing.Transforms;

using GDICOLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Backends
{

    [System.Diagnostics.DebuggerDisplay("{_ToDebuggerDisplay(),nq}")]
    sealed class _MemoryDrawingContext<TPixel> :
        _BaseDrawingContext<TPixel>,        
        IServiceProvider
        where TPixel : unmanaged, Pixel.IValueSetter<Pixel.BGRP32>
    {
        #region constructor
        public _MemoryDrawingContext(MemoryBitmap<TPixel> target, Converter<GDICOLOR, TPixel> converter)
            : base(target.Width,target.Height, converter)
        {
            _Target = target;            
        }

        #endregion

        #region debug

        private string _ToDebuggerDisplay()
        {
            return $"IDrawing2D {_Target.Info.ToDebuggerDisplayString()}";
        }

        #endregion

        #region data

        private readonly MemoryBitmap<TPixel> _Target;

        #endregion

        #region API

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(MemoryBitmap<TPixel>)) return _Target;
            return serviceType.IsAssignableFrom(this.GetType()) ? this : (object)null;
        }

        protected override SpanBitmap<TPixel> GetRenderTarget() => _Target.AsSpanBitmap();

        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("{_ToDebuggerDisplay(),nq}")]
    sealed class _PointerDrawingContext<TPixel> :
        _BaseDrawingContext<TPixel>,        
        IServiceProvider
        where TPixel : unmanaged, Pixel.IValueSetter<Pixel.BGRP32>
    {
        #region constructor
        public _PointerDrawingContext(PointerBitmap target, Converter<GDICOLOR, TPixel> converter)
            : base(target.Width, target.Height, converter)
        {
            _Target = target;
        }

        #endregion

        #region debug

        private string _ToDebuggerDisplay()
        {
            return $"IDrawing2D {_Target.Info.ToDebuggerDisplayString()}";
        }

        #endregion

        #region data

        private readonly PointerBitmap _Target;

        #endregion

        #region API

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(PointerBitmap)) return _Target;
            return serviceType.IsAssignableFrom(this.GetType()) ? this : (object)null;
        }

        protected override SpanBitmap<TPixel> GetRenderTarget() => _Target.AsSpanBitmap().OfType<TPixel>();

        #endregion
    }

    abstract class _BaseDrawingContext<TPixel> :
        Decompose2D.PassToSelf,
        IRenderTargetInfo,
        IBackendCanvas2D,        
        GlobalStyle.ISource
        where TPixel: unmanaged, Pixel.IValueSetter<Pixel.BGRP32>
    {
        #region constructor
        protected _BaseDrawingContext(int width, int height, Converter<GDICOLOR, TPixel> converter)
        {
            PixelsWidth = width;
            PixelsHeight = height;

            _ColorConverter = converter;
            _PolygonRasterizer = new Lazy<Helpers.PolygonScanlines>(() => new Helpers.PolygonScanlines(width, height));            
        }

        #endregion

        #region data
        
        private readonly Converter<GDICOLOR, TPixel> _ColorConverter;

        private readonly Lazy<Helpers.PolygonScanlines> _PolygonRasterizer;

        private MemoryBitmap _TempBitmap;

        private GlobalStyle _GlobalStyle;

        #endregion

        #region properties

        /// <inheritdoc/>
        public int PixelsWidth { get; }

        /// <inheritdoc/>
        public int PixelsHeight { get; }

        /// <inheritdoc/>
        public float DotsPerInchX => 96;

        /// <inheritdoc/>
        public float DotsPerInchY => 96;        

        #endregion

        #region API

        protected abstract SpanBitmap<TPixel> GetRenderTarget();

        bool GlobalStyle.ISource.TryGetGlobalProperty<T>(string name, out T value)
        {
            return GlobalStyle.TryGetGlobalProperty(_GlobalStyle, name, out value);
        }

        bool GlobalStyle.ISource.TrySetGlobalProperty<T>(string name, T value)
        {
            return GlobalStyle.TrySetGlobalProperty(ref _GlobalStyle, name, value);
        }

        private bool _TryGetImageSource(ImageSource src, out SpanBitmap bitmap)
        {
            if (src.Source is SpanBitmap.ISource typeless)
            {
                bitmap = typeless.AsSpanBitmap().AsReadOnly(); if (bitmap.IsEmpty) return false;
                src.WithSourceSize(bitmap.Width, bitmap.Height);
                return true;
            }

            if (src.Source is MemoryBitmap.IDisposableSource isrc)
            {
                bitmap = isrc.AsMemoryBitmap().AsSpanBitmap().AsReadOnly(); if (bitmap.IsEmpty) return false;
                src.WithSourceSize(bitmap.Width, bitmap.Height);
                return true;
            }

            if (src.Source is InterlockedBitmap ilock)
            {
                ilock.DequeueLastOrDefault(innerBmp => innerBmp.AsSpanBitmap().CopyTo(ref _TempBitmap));

                if (_TempBitmap.IsEmpty) { bitmap = default; return false; }
                
                bitmap = _TempBitmap.AsSpanBitmap().AsReadOnly();
                src.WithSourceSize(bitmap.Width, bitmap.Height);
                return true;
                
            }

            bitmap = default;
            return false;
        }

        /// <inheritdoc/>
        public sealed override void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            var xform = style.GetTransform() * transform;
            float opacity = style.Color.A;
            opacity /= 255f;

            var dstBmp = GetRenderTarget();            

            if (_TryGetImageSource(style.Image, out var srcBmp))
            {
                xform = Matrix3x2.CreateScale(1f / srcBmp.Width, 1f / srcBmp.Height) * xform;

                var bmpXform = new Bitmaps.Processing.BitmapTransform(xform, opacity) as SpanBitmap.ITransfer;

                switch (srcBmp.PixelFormat.Code)
                {
                    case Pixel.BGRP32.Code:
                        {
                            var src = srcBmp.OfType<Pixel.BGRP32>();
                            dstBmp.TransferFrom(src, bmpXform); return;
                        }
                    case Pixel.RGBP32.Code:
                        {
                            var src = srcBmp.OfType<Pixel.RGBP32>();
                            dstBmp.TransferFrom(src, bmpXform); return;
                        }
                    case Pixel.BGRA32.Code:
                        {
                            var src = srcBmp.OfType<Pixel.BGRA32>();
                            dstBmp.TransferFrom(src, bmpXform); return;
                        }
                    case Pixel.RGBA32.Code:
                        {
                            var src = srcBmp.OfType<Pixel.RGBA32>();
                            dstBmp.TransferFrom(src, bmpXform); return;
                        }
                    case Pixel.BGR24.Code:
                        {
                            var src = srcBmp.OfType<Pixel.BGR24>();
                            dstBmp.TransferFrom(src, bmpXform); return;
                        }
                    case Pixel.RGB24.Code:
                        {
                            var src = srcBmp.OfType<Pixel.RGB24>();
                            dstBmp.TransferFrom(src, bmpXform); return;
                        }
                }
            }            
        }        

        /// <inheritdoc/>
        public sealed override void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {
            var fillColor = _ColorConverter(color.ToGDI());

            var target = GetRenderTarget();

            foreach (var (y, xmin, xmax) in _PolygonRasterizer.Value.GetScanlines(Point2.AsNumerics(points)))
            {
                target
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

            var target = GetRenderTarget();

            for (int i = 1; i < points.Length; ++i)
            {
                var a = points[i - 1];
                var b = points[i + 0];

                SpanBitmapDrawing.DrawPixelLine(target, a, b, outColor);
            }
        }        

        #endregion
    }
}
