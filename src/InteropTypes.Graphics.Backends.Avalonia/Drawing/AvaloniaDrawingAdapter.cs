using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Avalonia;

using InteropTypes.Graphics.Drawing;

using VENDOR = Avalonia;

using WPFCONTEXT = Avalonia.Media.DrawingContext;
using WPFGEOMETRY = Avalonia.Media.Geometry;

using GDICOLOR = System.Drawing.Color;
using FONTSTYLE = InteropTypes.Graphics.Drawing.FontStyle;

using POINT2 = InteropTypes.Graphics.Drawing.Point2;

namespace InteropTypes.Graphics.Backends.Drawing
{
    /// <summary>
    /// Wraps a <see cref="WPFCONTEXT"/> and
    /// uses it as a factory to create <see cref="IDisposableCanvas2D"/>
    /// </summary>
    public partial class Canvas2DFactory : Avalonia.AvaloniaObject,
        GlobalStyle.ISource
    {
        #region lifecycle              

        public Canvas2DFactory(WPFCONTEXT context) : this()
        {
            SetContext(context);
        }

        public Canvas2DFactory()
        {
            GlobalStyle.TrySetGlobalProperty(ref _GlobalStyle, GlobalStyle.FONT, InteropTypes.Graphics.Drawing.Fonts.HersheyFont.Default);
        }

        #endregion

        #region data

        private WPFCONTEXT _Context;

        private _WPFResourcesCache _ResourcesCache = new _WPFResourcesCache();
        private _WPFGeometryFactory _GeometryFactory = new _WPFGeometryFactory();

        private GlobalStyle _GlobalStyle = new GlobalStyle();

        #endregion        

        #region API - Core Resources

        private void VerifyContext()
        {
            if (_Context == null) throw new InvalidOperationException($"Context is null, call {nameof(SetContext)} first");
        }

        bool GlobalStyle.ISource.TryGetGlobalProperty<T>(string name, out T value)
        {
            return GlobalStyle.TryGetGlobalProperty(_GlobalStyle, name, out value);
        }

        bool GlobalStyle.ISource.TrySetGlobalProperty<T>(string name, T value)
        {
            return GlobalStyle.TrySetGlobalProperty(ref _GlobalStyle, name, value);
        }

        #endregion

        #region API

        public void SetContext(WPFCONTEXT context)
        {
            this.VerifyAccess();
            _Context = context;
        }

        public IDisposableCanvas2D UsingCanvas2D(float w, float h)
        {
            this.VerifyAccess();
            this.VerifyContext();            

            return new _ActualCanvas2DContext(this, w, h, _Context, _GeometryFactory, _ResourcesCache);
        }


        public void DrawScene(Size? viewport, CameraTransform2D camera, IDrawingBrush<ICanvas2D> scene)
        {
            this.VerifyAccess();
            this.VerifyContext();

            var w = (float)(viewport?.Width ?? 100);
            var h = (float)(viewport?.Height ?? 100);

            using (var state = _PushClipRect(viewport))
            {
                using (var dc = UsingCanvas2D(w, h))
                {
                    scene.DrawTo(InteropTypes.Graphics.Drawing.Transforms.Canvas2DTransform.Create((dc, w, h), camera));
                }
            }
        }

        public void DrawScene(Size? viewport, in CameraTransform3D camera, IDrawingBrush<IScene3D> scene)
        {
            this.VerifyAccess();
            this.VerifyContext();

            var w = (float)(viewport?.Width ?? 100);
            var h = (float)(viewport?.Height ?? 100);

            using (var state = _PushClipRect(viewport))
            {
                using (var dc = UsingCanvas2D(w, h))
                {
                    scene.DrawTo(InteropTypes.Graphics.Drawing.Transforms.PerspectiveTransform.Create((dc, w, h), camera));
                }
            }
        }

        public void DrawScene(Size? viewport, CameraTransform2D.ISource xform, IDrawingBrush<ICanvas2D> scene)
        {
            if (xform == null)
            {
                var bounds = DrawingToolkit.GetAssetBoundingRect(scene);
                xform = new SceneView2D().WithSceneBounds(bounds.Value);
            }

            DrawScene(viewport, xform.GetCameraTransform2D(), scene);
        }

        public void DrawScene(Size? viewport, CameraTransform3D.ISource xform, IDrawingBrush<IScene3D> scene)
        {
            if (xform == null)
            {
                var bounds = DrawingToolkit.GetAssetBoundingMinMax(scene);
                xform = new SceneView3D().WithSceneBounds(bounds.Value);
            }

            DrawScene(viewport, xform.GetCameraTransform3D(), scene);
        }

        public void DrawScene(WPFCONTEXT dc, Size? viewport, CameraTransform2D.ISource xform, IDrawingBrush<ICanvas2D> scene)
        {
            this.VerifyAccess();
            SetContext(dc);
            DrawScene(viewport, xform, scene);
            SetContext(null);
        }

        public void DrawScene(WPFCONTEXT dc, Size? viewport, CameraTransform3D.ISource xform, IDrawingBrush<IScene3D> scene)
        {
            this.VerifyAccess();
            SetContext(dc);
            DrawScene(viewport, xform, scene);
            SetContext(null);
        }


        // https://github.com/AvaloniaUI/Avalonia/issues/10442

        

        

        public void DrawScene(Avalonia.Media.Imaging.RenderTargetBitmap target, Size? viewport, CameraTransform2D.ISource xform, IDrawingBrush<ICanvas2D> scene)
        {
            /*
            if (!this.CheckAccess())
            {
                this.Dispatcher.Invoke(() => DrawScene(target, viewport, xform, scene));
                return;
            }*/            

            /*
            var visual = new VENDOR.Media.DrawingVisual();
            DrawScene(visual, viewport, xform, scene);
            target.Render(visual);
            */
        }

        public void DrawScene(Avalonia.Media.Imaging.RenderTargetBitmap target, Size? viewport, CameraTransform3D.ISource xform, IDrawingBrush<IScene3D> scene)
        {
            /*
            if (!this.CheckAccess())
            {
                this.Dispatcher.Invoke(() => DrawScene(target, viewport, xform, scene));
                return;
            }*/

            /*
            var visual = new VENDOR.Media.DrawingVisual();
            DrawScene(visual, viewport, xform, scene);
            target.Render(visual);
            */
        }

        #endregion

        #region API - ICanvasDrawingContext2D , ICanvasDrawingContext3D

        private WPFCONTEXT.PushedState _PushClipRect(Size? viewport)
        {
            if (viewport.HasValue)
            {
                var rect = new Rect(viewport.Value);
                
                var state = _Context.PushClip(rect);
                _Context.DrawRectangle(_ResourcesCache.UseTransparentPen(), rect); // used to force the whole drawing as hit visible

                return state;
            }

            return default;
        }

        #endregion
    }

    struct _ActualCanvas2DContext :
        IDisposableCanvas2D,
        IRenderTargetInfo,
        GlobalStyle.ISource
    {
        #region lifecycle

        public _ActualCanvas2DContext(Canvas2DFactory owner, float pixelWidth, float pixelHeight, WPFCONTEXT context, _WPFGeometryFactory geo, _WPFResourcesCache resources)
        {
            _Owner = owner;
            PixelsWidth = (int)pixelWidth;
            PixelsHeight = (int)pixelHeight;

            DotsPerInchX = DotsPerInchY = 96;

            _Context = context;
            _Resources = resources;

            _Geometries = geo;            
        }

        public void Dispose()
        {
            _Owner = null;
            _Context = null;
            _Resources = null;
        }

        #endregion

        #region data

        private Canvas2DFactory _Owner;
        private WPFCONTEXT _Context;

        private _WPFResourcesCache _Resources;
        private _WPFGeometryFactory _Geometries;

        public int PixelsWidth { get; }

        public int PixelsHeight { get; }

        public float DotsPerInchX { get; }

        public float DotsPerInchY { get; }

        #endregion

        #region API

        readonly bool GlobalStyle.ISource.TryGetGlobalProperty<T>(string name, out T value)
        {
            return GlobalStyle.TryGetGlobalProperty(_Owner, name, out value);
        }

        readonly bool GlobalStyle.ISource.TrySetGlobalProperty<T>(string name, T value)
        {
            return GlobalStyle.TrySetGlobalProperty(_Owner, name, value);
        }

        #endregion

        #region API - ICanvas2D

        private readonly void VerifyDisposed()
        {
            if (_Owner == null) throw new ObjectDisposedException(nameof(_ActualCanvas2DContext));
        }

        /// <inheritdoc/>
        public readonly void DrawAsset(in Matrix3x2 transform, object asset)
        {
            VerifyDisposed();            
            new Graphics.Drawing.Transforms.Decompose2D(this).DrawAsset(transform, asset);
        }

        /// <inheritdoc/>
        public readonly void DrawConvexPolygon(ReadOnlySpan<POINT2> points, ColorStyle color)
        {
            VerifyDisposed();

            var fill = _Resources.UseBrush(color);

            if (fill == null) return;

            var g = _Geometries.CreateGeometry(points, true, true, false);
            _Context.DrawGeometry(fill, null, g);            
        }

        /// <inheritdoc/>
        public readonly void DrawLines(ReadOnlySpan<POINT2> points, float diameter, LineStyle brush)
        {
            VerifyDisposed();

            if (points.Length < 2) return;

            const bool forceOut = false; // true if outline is rendered outside, false if render in between

            var geo = _Geometries.CreateGeometry(points, false, false, true);

            if (brush.Style.HasOutline)
            {
                var outSize = forceOut ? brush.Style.OutlineWidth * 2 : brush.Style.OutlineWidth;

                var pen = _Resources.UsePen(diameter + outSize, brush.WithFill(brush.OutlineColor));
                if (pen != null)
                {
                    if (geo == null) _Context.DrawLine(pen, points[0].ToDevicePoint(), points[1].ToDevicePoint());
                    else _Context.DrawGeometry(null, pen, geo);
                }
            }

            if (brush.Style.HasFill)
            {
                var fillSize = diameter + (forceOut ? 0 : -brush.Style.OutlineWidth);
                if (fillSize <= 0) return;

                if (fillSize < 0.1f) fillSize = 0.1f;

                var pen = _Resources.UsePen(fillSize, brush);
                if (pen != null)
                {
                    if (geo == null) _Context.DrawLine(pen, points[0].ToDevicePoint(), points[1].ToDevicePoint());
                    else _Context.DrawGeometry(null, pen, geo);
                }
            }
        }

        /// <inheritdoc/>
        public readonly void DrawEllipse(POINT2 c, Single width, Single height, OutlineFillStyle brush)
        {
            VerifyDisposed();

            var fill = _Resources.UseBrush(brush.FillColor);
            var pen = _Resources.UsePen(brush.OutlineWidth, brush.OutlineColor);
            if (fill == null && pen == null) return;

            if (false)
            {
                width += brush.OutlineWidth;
                height += brush.OutlineWidth;
            }

            _Context.DrawEllipse(fill, pen, c.ToDevicePoint(), width * 0.5f, height * 0.5f);
        }

        /// <inheritdoc/>
        public readonly void DrawPolygon(ReadOnlySpan<POINT2> points, PolygonStyle brush)
        {
            VerifyDisposed();

            var fill = _Resources.UseBrush(brush.FillColor);
            var pen = _Resources.UsePen(brush.OutlineWidth, brush.OutlineColor);
            if (fill == null && pen == null) return;

            var g = _Geometries.CreateGeometry(points, true, fill != null, pen != null);

            _Context.DrawGeometry(fill, pen, g);
        }


        /// <inheritdoc/>
        public readonly void DrawLabel(POINT2 origin, String text, GDICOLOR penColor)
        {
            VerifyDisposed();

            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public readonly void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            VerifyDisposed();

            if (style.Color.IsEmpty) return;

            var bmp = style.Image;

            var image = _Resources.UseImage(bmp.Source);
            if (image == null) return;

            // System.Diagnostics.Debug.Assert(!(image is CroppedBitmap), "not renderable");
            // System.Diagnostics.Debug.Assert(!(image is BitmapFrame), "not renderable");

            var bmpRect = bmp.GetSourceRectangle();
            
            var srcScale = Matrix3x2.CreateScale(1f / bmpRect.Width, 1f / bmpRect.Height);
            var xform = srcScale * style.GetTransform() * transform;

            var opacity = (float)(style.Color.A) / 255f;

            var dstRect = new Rect(0, 0, bmpRect.Width, bmpRect.Height);

            var matrix = new Matrix(xform.M11, xform.M12, xform.M21, xform.M22, xform.M31, xform.M32);

            using (var start0 = opacity != 0 ? _Context.PushOpacity(opacity) : (WPFCONTEXT.PushedState?)null)
            {
                using (var stat1 = _Context.PushTransform(matrix))
                {
                    _Context.DrawImage(image, bmpRect.ToDeviceRect(), dstRect);
                }
            }
        }

        /// <inheritdoc/>
        public readonly void DrawTextLine(in Matrix3x2 transform, string text, float size, FONTSTYLE font)
        {
            font.DrawDecomposedTo(this, transform, text, size);
        }

        #endregion
    }    

    class _WPFGeometryFactory
    {
        public WPFGEOMETRY CreateGeometry(ReadOnlySpan<Point2> points, bool isClosed, bool isFilled, bool isStroked, bool isSmoothJoin = false)
        {
            var devicePoints = new List<Avalonia.Point>(points.Length);

            foreach (var p in points) devicePoints.Add(p.ToDevicePoint());

            if (isClosed) devicePoints.Add(devicePoints[0]);

            return new Avalonia.Media.PolylineGeometry(devicePoints, isFilled);
        }
    }
}
