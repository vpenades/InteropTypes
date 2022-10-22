using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using InteropTypes.Graphics.Drawing;

using GDICOLOR = System.Drawing.Color;
using WPFCONTEXT = System.Windows.Media.DrawingContext;
using WPFGEOMETRY = System.Windows.Media.StreamGeometry;
using WPFVISUAL = System.Windows.Media.DrawingVisual;
using WPFRENDERTARGET = System.Windows.Media.Imaging.RenderTargetBitmap;

namespace InteropTypes.Graphics.Backends
{
    /// <summary>
    /// Wraps a <see cref="WPFCONTEXT"/> and
    /// uses it as a factory to create <see cref="IDisposableCanvas2D"/>
    /// </summary>
    public partial class Canvas2DFactory : System.Windows.Threading.DispatcherObject,
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

            _GeometryFactory.Clear();

            return new _ActualCanvas2DContext(this, w, h, _Context, _GeometryFactory, _ResourcesCache);
        }

        public void DrawScene(Size? viewport, CameraTransform2D camera, IDrawingBrush<ICanvas2D> scene)
        {
            this.VerifyAccess();
            this.VerifyContext();

            var w = (float)(viewport?.Width ?? 100);
            var h = (float)(viewport?.Height ?? 100);

            _PushClipRect(viewport);
            using (var dc = UsingCanvas2D(w, h))
            {
                scene.DrawTo(Drawing.Transforms.Canvas2DTransform.Create((dc, w, h), camera));
            }
            _PopClipRect(viewport);
        }

        public void DrawScene(Size? viewport, in CameraTransform3D camera, IDrawingBrush<IScene3D> scene)
        {
            this.VerifyAccess();
            this.VerifyContext();

            var w = (float)(viewport?.Width ?? 100);
            var h = (float)(viewport?.Height ?? 100);

            _PushClipRect(viewport);
            using (var dc = UsingCanvas2D(w, h))
            {
                scene.DrawTo(Drawing.Transforms.PerspectiveTransform.Create((dc, w, h), camera));
            }
            _PopClipRect(viewport);
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

        public void DrawScene(WPFVISUAL target, Size? viewport, CameraTransform2D.ISource xform, IDrawingBrush<ICanvas2D> scene)
        {
            if (!this.CheckAccess())
            {
                this.Dispatcher.Invoke(() => DrawScene(target, viewport, xform, scene));
                return;
            }

            var oldDC = _Context;

            using (var dc = target.RenderOpen())
            {
                SetContext(dc);
                DrawScene(viewport, xform, scene);
                SetContext(null);
            }

            _Context = oldDC;
        }

        public void DrawScene(WPFVISUAL target, Size? viewport, CameraTransform3D.ISource xform, IDrawingBrush<IScene3D> scene)
        {
            if (!this.CheckAccess())
            {
                this.Dispatcher.Invoke(() => DrawScene(target, viewport, xform, scene));
                return;
            }

            var oldDC = _Context;

            using (var dc = target.RenderOpen())
            {
                SetContext(dc);
                DrawScene(viewport, xform, scene);
                SetContext(null);
            }

            _Context = oldDC;
        }

        public void DrawScene(WPFRENDERTARGET target, Size? viewport, CameraTransform2D.ISource xform, IDrawingBrush<ICanvas2D> scene)
        {
            if (!this.CheckAccess())
            {
                this.Dispatcher.Invoke(() => DrawScene(target, viewport, xform, scene));
                return;
            }

            var visual = new System.Windows.Media.DrawingVisual();
            DrawScene(visual, viewport, xform, scene);
            target.Render(visual);
        }

        public void DrawScene(WPFRENDERTARGET target, Size? viewport, CameraTransform3D.ISource xform, IDrawingBrush<IScene3D> scene)
        {
            if (!this.CheckAccess())
            {
                this.Dispatcher.Invoke(() => DrawScene(target, viewport, xform, scene));
                return;
            }

            var visual = new System.Windows.Media.DrawingVisual();
            DrawScene(visual, viewport, xform, scene);
            target.Render(visual);
        }

        #endregion

        #region API - ICanvasDrawingContext2D , ICanvasDrawingContext3D

        private void _PushClipRect(Size? viewport)
        {
            if (viewport.HasValue)
            {
                var rect = new Rect(viewport.Value);
                
                _Context.PushClip(new System.Windows.Media.RectangleGeometry(rect));
                _Context.DrawRectangle(null, _ResourcesCache.UseTransparentPen(), rect); // used to force the whole drawing as hit visible
            }
        }

        private void _PopClipRect(Size? viewport)
        {
            if (viewport.HasValue) _Context.Pop();
        }        

        #endregion
    }

    class _WPFGeometryFactory
    {
        // reusing the geometry is not trivial bevause although
        // it seems we can reuse the geometries just after submiting them,
        // WPF does a lazy rendering, so it can be uses way after we've completed the update.

        private readonly List<WPFGEOMETRY> _Pool = new List<WPFGEOMETRY>();

        private int _Count = 0;

        public void Clear() { _Count = 0; }

        public WPFGEOMETRY CreateGeometry(ReadOnlySpan<Point2> points, bool isClosed, bool isFilled, bool isStroked, bool isSmoothJoin = false)
        {
            while (_Pool.Count <= _Count) _Pool.Add(new WPFGEOMETRY());

            var g = _Pool[_Count++];

            g.Clear();

            using (var gg = g.Open())
            {
                gg.BeginFigure(points[0].ToDevicePoint(), isFilled, isClosed);

                for (int i = 0; i < points.Length; ++i)
                {
                    gg.LineTo(points[i].ToDevicePoint(), isStroked, isSmoothJoin);
                }

                gg.Close();
            }

            return g;
        }
    }

    class _TransformStack
    {
        /*
        private readonly List<System.Windows.Media.MatrixTransform> _TransformCache = new List<System.Windows.Media.MatrixTransform>();
        private int _TransformDepth;

        private void PushMatrix(in Matrix3x2 xform)
        {
            while (_TransformCache.Count <= _TransformDepth)
            {
                _TransformCache.Add(new System.Windows.Media.MatrixTransform());
            }

            var container = _TransformCache[_TransformDepth]; ++_TransformDepth;

            container.Matrix = new System.Windows.Media.Matrix(xform.M11, xform.M12, xform.M21, xform.M22, xform.M31, xform.M32);
            _Context.PushTransform(container);
        }

        private void PopMatrix()
        {
            --_TransformDepth;

            _Context.Pop(); // only valid if we pushed a matrix.
        }*/



        public _TransformStack(WPFCONTEXT context) { _Context = context; }
        private readonly WPFCONTEXT _Context;        

        public void Push(Matrix3x2 xform)
        {
            _Context.PushTransform(new MatrixTransform(xform.M11, xform.M12, xform.M21, xform.M22, xform.M31, xform.M32));            
        }

        public void Pop()
        {            
            _Context.Pop();
        }
    }
    
    struct _ActualCanvas2DContext :
        IDisposableCanvas2D,
        IRenderTargetInfo,
        GlobalStyle.ISource
    {
        #region lifecycle

        public _ActualCanvas2DContext(Canvas2DFactory owner, float pixelWidth,float pixelHeight, WPFCONTEXT context, _WPFGeometryFactory geo, _WPFResourcesCache resources)
        {
            _Owner = owner;
            PixelsWidth = (int)pixelWidth;
            PixelsHeight = (int)pixelHeight;

            DotsPerInchX = DotsPerInchY = 96;

            _Context = context;
            _Resources = resources;

            _Geometries = geo;

            _Transforms = new _TransformStack(context);
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
        private _TransformStack _Transforms;

        public int PixelsWidth { get; }

        public int PixelsHeight { get; }

        public float DotsPerInchX { get; }

        public float DotsPerInchY { get; }

        #endregion

        #region API

        bool GlobalStyle.ISource.TryGetGlobalProperty<T>(string name, out T value)
        {
            return GlobalStyle.TryGetGlobalProperty(_Owner,name,out value);
        }

        bool GlobalStyle.ISource.TrySetGlobalProperty<T>(string name, T value)
        {
            return GlobalStyle.TrySetGlobalProperty(_Owner, name, value);
        }

        #endregion

        #region API - ICanvas2D

        private void VerifyDisposed()
        {
            if (_Owner == null) throw new ObjectDisposedException(nameof(_ActualCanvas2DContext));
        }

        /// <inheritdoc/>
        public void DrawAsset(in Matrix3x2 transform, object asset)
        {
            VerifyDisposed();

            new Drawing.Transforms.Decompose2D(this).DrawAsset(transform, asset);
        }

        /// <inheritdoc/>
        public void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {
            VerifyDisposed();

            var fill = _Resources.UseBrush(color);

            if (fill == null) return;

            var g = _Geometries.CreateGeometry(points, true, true, false);
            _Context.DrawGeometry(fill, null, g);         
        }

        /// <inheritdoc/>
        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle brush)
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
        public void DrawEllipse(Point2 c, Single width, Single height, OutlineFillStyle brush)
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
        public void DrawPolygon(ReadOnlySpan<Point2> points, PolygonStyle brush)
        {
            VerifyDisposed();

            var fill = _Resources.UseBrush(brush.FillColor);
            var pen = _Resources.UsePen(brush.OutlineWidth, brush.OutlineColor);
            if (fill == null && pen == null) return;

            var g = _Geometries.CreateGeometry(points, true, fill != null, pen != null);

            _Context.DrawGeometry(fill, pen, g);            
        }

        /// <inheritdoc/>
        public void DrawLabel(Point2 origin, String text, GDICOLOR penColor)
        {
            VerifyDisposed();

            var culture = System.Globalization.CultureInfo.CurrentUICulture;

            var fmtText = new System.Windows.Media.FormattedText
                (
                text,
                culture,
                culture.TextInfo.IsRightToLeft ? System.Windows.FlowDirection.RightToLeft : System.Windows.FlowDirection.LeftToRight,
                new System.Windows.Media.Typeface("Arial"),
                16,
                penColor.ToDeviceBrush(),
                1
                );

            origin -= new Point2((float)fmtText.Width, (float)fmtText.Height) * 0.5f;

            _Context.DrawText(fmtText, origin.ToDevicePoint());
        }

        /// <inheritdoc/>
        public void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            VerifyDisposed();

            if (style.Color.IsEmpty) return;

            var bmp = style.Image;

            var image = _Resources.UseImage(bmp.Source);
            if (image == null) return;

            System.Diagnostics.Debug.Assert(!(image is CroppedBitmap), "not renderable");
            System.Diagnostics.Debug.Assert(!(image is BitmapFrame), "not renderable");

            var bmpRect = bmp.GetSourceRectangle();
            
            var srcClipOffset = Matrix3x2.CreateTranslation(-bmpRect.X, -bmpRect.Y); // we need to subtract the clip region offset
            var srcScale = Matrix3x2.CreateScale(1f / bmpRect.Width, 1f / bmpRect.Height);
            var xform = srcClipOffset * srcScale * style.GetTransform() * transform;

            _Transforms.Push(xform);
                
                _Context.PushClip(_Resources.UseClipRectangle(bmpRect));

                    if (style.Color.A != 255) { _Context.PushOpacity(style.Color.A / 255f); }            

                    var dstRect = new Rect(0, 0, image.PixelWidth, image.PixelHeight);
                    _Context.DrawImage(image, dstRect);

                    if (style.Color.A != 255) { _Context.Pop(); }
            
                _Context.Pop();

            _Transforms.Pop();
        }        

        /// <inheritdoc/>
        public void DrawTextLine(in Matrix3x2 transform, string text, float size, Drawing.FontStyle font)
        {
            font.DrawDecomposedTo(this, transform, text, size);
        }

        #endregion
    }
}
