using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace InteropDrawing.Backends
{
    public class WPFScene2DCanvas : Canvas
    {
        // Todo: add a dispatchertimer to check the Model's immutable flag for automatic refresh

        #region data

        private readonly WPFDrawingContext2D _Context2D = new WPFDrawingContext2D();

        private Matrix3x2 _CameraMatrix = Matrix3x2.Identity;
        private Matrix3x2 _ProjectionMatrix = Matrix3x2.Identity;
        private Matrix3x2 _InverseFinal = Matrix3x2.Identity;

        #endregion

        #region dependency properties

        private static PropertyFactory<WPFScene2DCanvas> _PropFactory = new PropertyFactory<WPFScene2DCanvas>();

        #endregion

        #region properties - primary transform system

        private static readonly StaticProperty<ISceneViewport2D> TransformProperty = _PropFactory.Register<ISceneViewport2D>(nameof(Transform),null, _Update);
        public ISceneViewport2D Transform
        {
            get => TransformProperty.GetValue(this);
            set => TransformProperty.SetValue(this, value);
        }

        #endregion

        #region properties - fallback transform system

        private static readonly StaticProperty<HorizontalAlignment> HorizontalOriginProperty = _PropFactory.Register(nameof(HorizontalOrigin), HorizontalAlignment.Stretch, _Update);
        public HorizontalAlignment HorizontalOrigin
        {
            get => HorizontalOriginProperty.GetValue(this);
            set => HorizontalOriginProperty.SetValue(this, value);
        }

        private static readonly StaticProperty<VerticalAlignment> VerticalOriginProperty = _PropFactory.Register(nameof(VerticalOrigin), VerticalAlignment.Stretch, _Update);
        public VerticalAlignment VerticalOrigin
        {
            get => VerticalOriginProperty.GetValue(this);
            set => VerticalOriginProperty.SetValue(this, value);
        }

        /// <summary>
        /// TODO: convert to attached property
        /// </summary>
        private static readonly StaticProperty<Matrix3x2> CameraTransformProperty = _PropFactory.Register(nameof(CameraTransform), Matrix3x2.Identity, _Update);
        public Matrix3x2 CameraTransform
        {
            get => CameraTransformProperty.GetValue(this);
            set => CameraTransformProperty.SetValue(this, value);
        }

        // Dock => Left, Top, Right, Bottom

        // DockPosition => Left, Top, Right, Bottom, Fill, None

        // Stretch => None, Fill, Uniform, UniformToFill

        // StretchDirection => UpOnly, DownOnly, Both

        // HorizontalAlignment => Left, Center, Right, Stretch

        // VerticalAlignment => Top, Center, Bottom, Stretch

        // FlowDirection => LeftToRight, RightToLeft        

        #endregion

        #region properties - the scene

        private static readonly StaticProperty<IDrawingBrush<IDrawing2D>> ContentProperty = _PropFactory.Register<IDrawingBrush<IDrawing2D>>(nameof(Content), null, _Update);
        public IDrawingBrush<IDrawing2D> Content
        {
            get => ContentProperty.GetValue(this);
            set => ContentProperty.SetValue(this, value);
        }

        #endregion        

        #region internal wiring

        private static void _Update(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WPFScene2DCanvas dc) dc._Update(e.Property, e.OldValue, e.NewValue);
        }

        private void _Update(DependencyProperty p, object oldv, object newv)
        {
            _UpdateMatrices(this.RenderSize);

            if (p == ContentProperty.Property) this.InvalidateVisual();
            if (p == TransformProperty.Property) this.InvalidateVisual();
            if (p == CameraTransformProperty.Property) this.InvalidateVisual();            
            if (p == HorizontalOriginProperty.Property) this.InvalidateVisual();
            if (p == VerticalOriginProperty.Property) this.InvalidateVisual();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            _UpdateMatrices(sizeInfo.NewSize);

            base.OnRenderSizeChanged(sizeInfo);
        }

        #endregion

        #region API

        private void _UpdateMatrices(Size viewport)
        {
            if (viewport.IsEmpty || viewport.Width == 0 || viewport.Height == 0) return;

            // primary transform
            var xform = Transform;            

            // fallback transform
            if (xform == null) xform = new _Viewport(this.HorizontalOrigin, this.VerticalOrigin, this.CameraTransform, this.Content);

            // calculate matrices
            var (c, p) = xform.GetMatrices((float)viewport.Width, (float)viewport.Height);
            _CameraMatrix = c;
            _ProjectionMatrix = p;

            // calculate inverse matrix
            Matrix3x2.Invert(_CameraMatrix, out Matrix3x2 viewMatrix);
            var final = viewMatrix * _ProjectionMatrix * CreateViewport2D((float)viewport.Width, (float)viewport.Height);
            Matrix3x2.Invert(final, out _InverseFinal);
        }

        public static Matrix3x2 CreateViewport2D(float width, float height)
        {
            Matrix3x2 result;

            result.M11 = width * 0.5f;
            result.M12 = 0;

            result.M22 = -height * 0.5f;
            result.M21 = 0;

            result.M31 = width * 0.5f;
            result.M32 = height * 0.5f;

            return result;
        }

        public Vector2 GetClientToScene(Point relativePoint)
        {
            var p = new Vector2((float)relativePoint.X, (float)relativePoint.Y);            
            p = Vector2.Transform(p, _InverseFinal);
            return p;
        }        

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc); // draw background

            _UpdateMatrices(this.RenderSize);            

            var content = this.Content;
            if (content == null) return;
            if (content is Record2D model && model.IsEmpty) return;            
            
            _Context2D.SetContext(dc);
            _Context2D.DrawScene(this.RenderSize, _ProjectionMatrix, _CameraMatrix, content);
            _Context2D.SetContext(null);
        }

        #endregion

        #region nested types

        struct _Viewport : ISceneViewport2D
        {
            #region constructor
            public _Viewport(HorizontalAlignment h, VerticalAlignment v, Matrix3x2 camera, IDrawingBrush<IDrawing2D> scene)
            {
                HAlignment = h;
                VAlignment = v;
                Camera = camera;

                if (scene != null)
                {
                    var bounds = Toolkit.GetAssetBoundingRect(scene).Value;
                    var min = new Vector2(bounds.X, bounds.Y);
                    var max = min + new Vector2(bounds.Width, bounds.Height);

                    SceneBounds = (min, max);
                }
                else
                {
                    SceneBounds = (Vector2.Zero, Vector2.One);
                }
            }

            #endregion

            #region data

            // projection terms            
            public HorizontalAlignment HAlignment;
            public VerticalAlignment VAlignment;
            public (Vector2 Min, Vector2 Max) SceneBounds;

            public Vector2 SceneSize => SceneBounds.Max - SceneBounds.Min;

            // camera terms
            public Matrix3x2 Camera;

            #endregion

            #region API

            public (Matrix3x2 Camera, Matrix3x2 Projection) GetMatrices(float renderWidth, float renderHeight)
            {
                var size = new Vector2(renderWidth, renderHeight);
                var origin = -Vector2.One;

                if (HAlignment == HorizontalAlignment.Right) { size.X = -size.X; origin.X = - origin.X; }
                if (VAlignment == VerticalAlignment.Top) { size.Y = -size.Y; origin.Y = -origin.Y; }

                if (HAlignment == HorizontalAlignment.Center) { origin.X = 0; }
                if (VAlignment == VerticalAlignment.Center) { origin.Y = 0; }

                if (HAlignment == HorizontalAlignment.Stretch || VAlignment== VerticalAlignment.Stretch)
                {
                    var ar = renderWidth / renderHeight;
                    var mr = SceneSize;
                    var s = Math.Max(mr.X, mr.Y);

                    if (HAlignment == HorizontalAlignment.Stretch) size.X = s * ar;
                    if (VAlignment == VerticalAlignment.Stretch) size.Y = s;
                }

                var proj = CreateOrthographic2D(size, origin);                

                return (Camera, proj);
            }

            public static Matrix3x2 CreateOrthographic2D(Vector2 size, Vector2 origin)
            {
                if (origin.X != -1 && origin.X != 0 && origin.X != 1) throw new ArgumentException(nameof(origin.X));
                if (origin.Y != -1 && origin.Y != 0 && origin.Y != 1) throw new ArgumentException(nameof(origin.Y));

                Matrix3x2 result;

                result.M11 = 2f / size.X;
                result.M12 = 0;

                result.M22 = 2f / size.Y;
                result.M21 = 0;

                result.M31 = origin.X;
                result.M32 = origin.Y;

                return result;
            }

            #endregion
        }

        #endregion
    }
}
