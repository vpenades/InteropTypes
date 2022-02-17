using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends.WPF
{
    public class WPFScene3DCanvas : System.Windows.Controls.Canvas
    {
        // Todo: add a dispatchertimer to check the Model's immutable flag for automatic refresh

        #region data

        private readonly WPFDrawingContext2D _Context2D = new WPFDrawingContext2D();

        #endregion

        #region dependency properties

        private static PropertyFactory<WPFScene3DCanvas> _PropFactory = new PropertyFactory<WPFScene3DCanvas>();

        static readonly StaticProperty<IDrawingBrush<IScene3D>> SceneProperty = _PropFactory.Register<IDrawingBrush<IScene3D>>(nameof(Content), null, _Update);
        public IDrawingBrush<IScene3D> Content
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
            (d as WPFScene3DCanvas)?._Update(e.Property, e.OldValue, e.NewValue);
        }

        private void _Update(DependencyProperty p, object oldv, object newv)
        {
            if (p == SceneProperty.Property) this.InvalidateVisual();
            if (p == ViewportProperty.Property) this.InvalidateVisual();
        }

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc); // draw background

            var content = this.Content;
            if (content == null) return;
            if (content is Record3D model && model.IsEmpty) return;            

            _Context2D.DrawScene(dc, this.RenderSize, Viewport, content);
        }        

        #endregion
    }
}
