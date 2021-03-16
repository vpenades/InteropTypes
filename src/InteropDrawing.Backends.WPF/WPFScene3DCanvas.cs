using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InteropDrawing.Backends
{
    public class WPFScene3DCanvas : System.Windows.Controls.Canvas
    {
        // Todo: add a dispatchertimer to check the Model's immutable flag for automatic refresh

        #region data

        private readonly WPFDrawingContext2D _Context2D = new WPFDrawingContext2D();

        #endregion

        #region dependency properties

        private static PropertyFactory<WPFScene3DCanvas> _PropFactory = new PropertyFactory<WPFScene3DCanvas>();

        static readonly StaticProperty<Model3D> SceneProperty = _PropFactory.Register<Model3D>(nameof(Scene), null, _Update);
        public Model3D Scene
        {
            get => SceneProperty.GetValue(this);
            set => SceneProperty.SetValue(this, value);
        }

        static readonly StaticProperty<ISceneViewport3D> ViewportProperty = _PropFactory.Register<ISceneViewport3D>(nameof(Viewport), null, _Update);
        public ISceneViewport3D Viewport
        {
            get => ViewportProperty.GetValue(this);
            set => ViewportProperty.SetValue(this, value);
        }

        #endregion

        #region API

        private static void _Update(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WPFScene3DCanvas dc) dc._Update(e.Property, e.OldValue, e.NewValue);
        }

        private void _Update(DependencyProperty p, object oldv, object newv)
        {
            if (p == SceneProperty.Property) this.InvalidateVisual();
            if (p == ViewportProperty.Property) this.InvalidateVisual();
        }

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc); // draw background

            var model = this.Scene;

            if (model != null && !model.IsEmpty)
            {
                _Context2D.DrawScene(dc, this.RenderSize, Viewport, model);
            }            
        }        

        #endregion
    }
}
