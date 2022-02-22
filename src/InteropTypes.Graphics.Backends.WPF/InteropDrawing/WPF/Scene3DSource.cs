using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using DRAWABLE = InteropTypes.Graphics.Drawing.IDrawingBrush<InteropTypes.Graphics.Drawing.IScene3D>;

namespace InteropTypes.Graphics.Backends.WPF
{
    // Todo:
    // Add visible;
    // add animation trigger  
    // add whether to contribute to scene focus sphere or not
    // add picking

    /// <summary>
    /// Represents the visual source of <see cref="Drawing.IDrawingBrush{TContext}"/>
    /// </summary>
    /// <remarks>
    /// This is the child of <see cref="Scene3DRoot"/>
    /// </remarks>
    public class Scene3DSource : FrameworkElement
    {
        #region data

        private static readonly PropertyFactory<Scene3DSource> _PropFactory = new PropertyFactory<Scene3DSource>();        

        #endregion

        #region Dependency - Scene

        static readonly StaticProperty<DRAWABLE> SceneProperty = _PropFactory.Register<DRAWABLE>(nameof(Scene), null, _SceneUpdate);

        /// <summary>
        /// Represents a drawable object
        /// </summary>
        public DRAWABLE Scene
        {
            get => SceneProperty.GetValue(this);
            set => SceneProperty.SetValue(this, value);
        }

        private static void _SceneUpdate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Scene3DSource)?._SceneUpdate(e.Property, e.OldValue, e.NewValue);
        }

        private void _SceneUpdate(DependencyProperty p, object oldv, object newv)
        {
            if (p == SceneProperty.Property)
            {
                RaiseSceneChanged();
                this.InvalidateVisual();
            }
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
            typeof(Scene3DSource));

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

            // should add BoundingSphere
        }

        #endregion
    }
}
