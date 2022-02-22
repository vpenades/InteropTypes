using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using InteropTypes.Graphics.Drawing;

using CAMERA = InteropTypes.Graphics.Drawing.CameraTransform3D;
using DRAWABLE = InteropTypes.Graphics.Drawing.IDrawingBrush<InteropTypes.Graphics.Drawing.IScene3D>;

namespace InteropTypes.Graphics.Backends.WPF.Primitives
{
    /// <summary>
    /// Combines a <see cref="DRAWABLE"/> and a <see cref="CAMERA"/> for rendering.
    /// </summary>
    public partial class Scene3DCanvas : Camera3DPanel
    {
        // Todo: add a dispatchertimer to check the Model's immutable flag for automatic refresh

        #region lifecycle

        public Scene3DCanvas()
        {
            this.Unloaded += (s, e) => Scene = null; // allow unregister scene from bubble up.
        }

        #endregion

        #region data

        private static readonly PropertyFactory<Scene3DCanvas> _PropFactory = new PropertyFactory<Scene3DCanvas>();

        private readonly DrawingContext2D _Context2D = new DrawingContext2D();

        #endregion

        #region Dependency - Scene

        static readonly StaticProperty<DRAWABLE> SceneProperty = _PropFactory.Register<DRAWABLE>(nameof(Scene), null, _Update);

        /// <summary>
        /// Represents a drawable object
        /// </summary>
        public DRAWABLE Scene
        {
            get => SceneProperty.GetValue(this);
            set => SceneProperty.SetValue(this, value);
        }

        #endregion

        #region RoutedEvent - bubble up scene change

        // https://stackoverflow.com/questions/4698047/event-bubbling-for-custom-event-in-wpf
        // https://stackoverflow.com/questions/24876053/the-correct-way-to-do-a-tunneled-event

        public static readonly RoutedEvent SceneChangedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(SceneChanged),
            RoutingStrategy.Bubble,
            typeof(SceneChangedRoutedEventHandler),
            typeof(Scene3DCanvas));

        public event SceneChangedRoutedEventHandler SceneChanged
        {
            add { AddHandler(SceneChangedEvent, value); }
            remove { RemoveHandler(SceneChangedEvent, value); }
        }

        protected void RaiseSceneChanged()
        {
            RaiseEvent(new SceneChangedEventArgs(SceneChangedEvent, this, Scene));
        }

        public delegate void SceneChangedRoutedEventHandler(object sender, SceneChangedEventArgs e);

        public class SceneChangedEventArgs : RoutedEventArgs
        {
            public SceneChangedEventArgs(RoutedEvent routedEvent, object source, DRAWABLE scene)
                : base(routedEvent, source)
            {
                Scene = scene;
            }

            public DRAWABLE Scene { get; }
        }

        #endregion

        

        #region API

        private static void _Update(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Scene3DCanvas)?._Update(e.Property, e.OldValue, e.NewValue);
        }

        private void _Update(DependencyProperty p, object oldv, object newv)
        {
            if (p == SceneProperty.Property)
            {
                RaiseSceneChanged();
                this.InvalidateVisual();
            }
        }

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc); // draw background            

            var rsize = this.RenderSize;

            if (double.IsNaN(rsize.Width) || double.IsNaN(rsize.Height)) return;

            var content = this.Scene;
            if (content == null) return;
            if (content is Record3D model && model.IsEmpty) return;

            var projection = Camera.CreateProjectionMatrix((float)(rsize.Width / rsize.Height));            
            
            _Context2D.SetContext(dc);
            _Context2D.DrawScene(rsize, projection, Camera.WorldMatrix, content);
            _Context2D.SetContext(null);
        }        

        #endregion
    }
}
