using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;

using COLOR = System.Drawing.Color;

namespace InteropDrawing.Backends
{
    /// <summary>
    /// Represents an agent able to converts <see cref="IDrawing2D"/> drawing calls to
    /// <see cref="System.Windows.Media.DrawingContext"/> drawing calls.
    /// </summary>
    public partial class WPFDrawingContext2D : System.Windows.Threading.DispatcherObject, IDrawing2D
    {
        #region lifecycle

        private static readonly WPFDrawingContext2D _Default = new WPFDrawingContext2D();

        public static WPFDrawingContext2D Default => _Default;

        public WPFDrawingContext2D() { }

        public WPFDrawingContext2D(System.Windows.Media.DrawingContext context)
        {
            SetContext(context);
        }        

        #endregion

        #region data
        
        private System.Windows.Media.DrawingContext _Context;
        private readonly List<System.Windows.Media.MatrixTransform> _TransformCache = new List<System.Windows.Media.MatrixTransform>();
        private int _TransformDepth = 0;

        private System.Windows.Media.Pen _TransparentPen;

        private readonly Dictionary<Int32, System.Windows.Media.SolidColorBrush> _BrushesCache = new Dictionary<Int32, System.Windows.Media.SolidColorBrush>();

        private readonly Dictionary<Object, System.Windows.Media.Imaging.BitmapSource> _ImagesCache = new Dictionary<Object, System.Windows.Media.Imaging.BitmapSource>();

        #endregion

        #region API - Core Resources

        private void VerifyContext()
        {
            if (_Context == null) throw new InvalidOperationException($"Context is null, call {nameof(SetContext)} first");
        }

        private System.Windows.Media.SolidColorBrush _UseBrush(COLOR color)
        {
            if (color.A == 0) return null; // todo if Alpha is 0 return null;

            if (_BrushesCache.TryGetValue(color.ToArgb(), out System.Windows.Media.SolidColorBrush brush)) return brush;

            brush = color.ToDeviceBrush();

            _BrushesCache[color.ToArgb()] = brush;

            return brush;
        }

        private System.Windows.Media.Pen _UsePen(COLOR color, Single width, LineCapStyle start, LineCapStyle end)
        {
            if (width <= 0) return null;

            var fill = _UseBrush(color); if (fill == null) return null;

            var pen = new System.Windows.Media.Pen(fill, width)
            {
                StartLineCap = start.ToDeviceCapStyle(),
                EndLineCap = end.ToDeviceCapStyle()
            };

            return pen;
        }        

        private System.Windows.Media.Imaging.BitmapSource _UseImage(object imageKey)
        {
            if (_ImagesCache.TryGetValue(imageKey, out System.Windows.Media.Imaging.BitmapSource image)) return image;

            var imagePath = imageKey as String;

            using (var s = System.IO.File.OpenRead(imagePath))
            {
                if (true)
                {
                    var img = new System.Windows.Media.Imaging.BitmapImage();
                    
                    img.BeginInit();
                    img.StreamSource = s;
                    img.EndInit();

                    image = img;
                }
                else
                {
                    image = System.Windows.Media.Imaging.BitmapFrame.Create(s, System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache, System.Windows.Media.Imaging.BitmapCacheOption.None);                    
                }
            }

            image.Freeze();

            _ImagesCache[imageKey] = image;

            return image;            
        }

        private static System.Windows.Media.StreamGeometry _CreateGeometry(ReadOnlySpan<Point2> points, bool isClosed, bool isFilled, bool isStroked, bool isSmoothJoin = false)
        {
            var g = new System.Windows.Media.StreamGeometry();

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

        private void PushMatrix(in Matrix3x2 xform)
        {
            while(_TransformCache.Count <= _TransformDepth)
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
        }

        #endregion

        #region API

        public void SetContext(System.Windows.Media.DrawingContext context)
        {
            this.VerifyAccess();
            _Context = context;
        }

        #endregion

        #region API - IDrawing2D

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle brush)
        {
            this.VerifyAccess();
            new Transforms.Decompose2D(this).DrawAsset(transform, asset, brush);
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle brush)
        {
            this.VerifyAccess();
            this.VerifyContext();            

            const bool forceOut = false; // true if outline is rendered outside, false if render in between

            var geo = points.Length > 2 ? _CreateGeometry(points, false, false, true) : null;            

            if (brush.Style.HasOutline)
            {
                var outSize = forceOut ? brush.Style.OutlineWidth * 2 : brush.Style.OutlineWidth;

                var pen = _UsePen(brush.Style.OutlineColor, diameter + outSize, brush.StartCap, brush.EndCap);

                if (geo == null) _Context.DrawLine(pen, points[0].ToDevicePoint(), points[1].ToDevicePoint());
                else _Context.DrawGeometry(null, pen, geo);
            }

            if (brush.Style.HasFill)
            {
                var fillSize = diameter + (forceOut ? 0 : -brush.Style.OutlineWidth);
                if (fillSize <= 0) return;

                if (fillSize < 0.1f) fillSize = 0.1f;

                var pen = _UsePen(brush.Style.FillColor, fillSize, brush.StartCap, brush.EndCap);

                if (geo == null) _Context.DrawLine(pen, points[0].ToDevicePoint(), points[1].ToDevicePoint());
                else _Context.DrawGeometry(null, pen, geo);
            }            
        }

        public void DrawEllipse(Point2 c, Single width, Single height, ColorStyle brush)
        {
            this.VerifyAccess();
            this.VerifyContext();

            var fill = brush.HasFill ? _UseBrush(brush.FillColor) : null;
            var pen = brush.HasOutline ? _UsePen(brush.OutlineColor, brush.OutlineWidth, LineCapStyle.Flat, LineCapStyle.Flat) : null;

            if (fill == null && pen == null) return;

            if (false)
            {
                width += brush.OutlineWidth;
                height += brush.OutlineWidth;
            }

            _Context.DrawEllipse(fill, pen, c.ToDevicePoint(), width * 0.5f, height * 0.5f);
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle brush)
        {
            this.VerifyAccess();
            this.VerifyContext();

            var fill = _UseBrush(brush.FillColor);
            var pen = _UsePen(brush.OutlineColor, brush.OutlineWidth, LineCapStyle.Flat, LineCapStyle.Flat);

            if (fill == null && pen == null) return;

            var g = _CreateGeometry(points, true, fill != null, pen != null);

            _Context.DrawGeometry(fill, pen, g);
        }        

        public void DrawLabel(Point2 origin, String text, COLOR penColor)
        {
            this.VerifyAccess();
            this.VerifyContext();

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

            origin.X -= (float)fmtText.Width * 0.5f;
            origin.Y -= (float)fmtText.Height * 0.5f;

            _Context.DrawText(fmtText, origin.ToDevicePoint());
        }

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            var bmp = style.Bitmap;

            var image = _UseImage(bmp.Source);

            var recti = new Int32Rect(bmp.Left, bmp.Top, bmp.Width, bmp.Height);
            var cropped = new System.Windows.Media.Imaging.CroppedBitmap(image, recti);

            // PushMatrix(transform);

            var dstRect = new Rect(0, 0, bmp.Width, bmp.Height);
            _Context.DrawImage(cropped, dstRect);

            // PopMatrix();
        }

        #endregion

        #region API - ICanvasDrawingContext2D , ICanvasDrawingContext3D

        private void PushClipRect(Size? viewport)
        {
            if (viewport.HasValue)
            {
                var rect = new Rect(viewport.Value);

                if (_TransparentPen == null) _TransparentPen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Transparent, 1);

                _Context.PushClip(new System.Windows.Media.RectangleGeometry(rect));
                _Context.DrawRectangle(null, _TransparentPen, rect);
            }
        }

        private void PopClipRect(Size? viewport)
        {
            if (viewport.HasValue) _Context.Pop();
        }

        public void DrawScene(Size? viewport, Matrix3x2 prj, Matrix3x2 cam, IDrawable2D scene)
        {
            this.VerifyAccess();
            this.VerifyContext();

            var w = (float)(viewport?.Width ?? 100);
            var h = (float)(viewport?.Height ?? 100);

            PushClipRect(viewport);
            scene.DrawTo(Transforms.Drawing2DTransform.Create((this, w, h), prj, cam));
            PopClipRect(viewport);
        }

        public void DrawScene(Size? viewport, Matrix4x4 prj, Matrix4x4 cam, IDrawable3D scene)
        {
            this.VerifyAccess();
            this.VerifyContext();

            var w = (float)(viewport?.Width ?? 100);
            var h = (float)(viewport?.Height ?? 100);

            PushClipRect(viewport);
            scene.DrawTo(Transforms.PerspectiveTransform.Create((this, w, h), cam, prj));
            PopClipRect(viewport);
        }

        public void DrawScene(Size? viewport, ISceneViewport2D xform, IDrawable2D scene)
        {
            if (xform == null)
            {
                var bounds = Toolkit.GetAssetBoundingRect(scene);
                xform = new SceneView2D().WithSceneBounds(bounds.Value);
            }

            var w = (float)(viewport?.Width ?? 100);
            var h = (float)(viewport?.Height ?? 100);

            var (cam, prj) = xform.GetMatrices(w, h);

            DrawScene(viewport, prj, cam, scene);
        }        

        public void DrawScene(Size? viewport, ISceneViewport3D xform, IDrawable3D scene)
        {
            if (xform == null)
            {
                var bounds = Toolkit.GetAssetBoundingMinMax(scene);
                xform = new SceneView3D().WithSceneBounds(bounds.Value);
            }

            var w = (float)(viewport?.Width ?? 100);
            var h = (float)(viewport?.Height ?? 100);
            var (cam, prj) = xform.GetMatrices(w, h);

            DrawScene(viewport, prj, cam, scene);
        }

        public void DrawScene(System.Windows.Media.DrawingContext dc, Size? viewport, ISceneViewport2D xform, IDrawable2D scene)
        {
            this.VerifyAccess();
            SetContext(dc);
            DrawScene(viewport, xform, scene);
            SetContext(null);
        }

        public void DrawScene(System.Windows.Media.DrawingContext dc, Size? viewport, ISceneViewport3D xform, IDrawable3D scene)
        {
            this.VerifyAccess();
            SetContext(dc);
            DrawScene(viewport, xform, scene);
            SetContext(null);            
        }

        public void DrawScene(System.Windows.Media.DrawingVisual target, Size? viewport, ISceneViewport2D xform, IDrawable2D scene)
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

        public void DrawScene(System.Windows.Media.DrawingVisual target, Size? viewport, ISceneViewport3D xform, IDrawable3D scene)
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

        public void DrawScene(System.Windows.Media.Imaging.RenderTargetBitmap target, Size? viewport, ISceneViewport2D xform, IDrawable2D scene)
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

        public void DrawScene(System.Windows.Media.Imaging.RenderTargetBitmap target, Size? viewport, ISceneViewport3D xform, IDrawable3D scene)
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
    }
}
