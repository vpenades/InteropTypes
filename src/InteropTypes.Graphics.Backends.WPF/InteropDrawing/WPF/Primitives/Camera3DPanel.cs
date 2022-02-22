using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InteropTypes.Graphics.Backends.WPF.Primitives
{
    using CAMERA = InteropTypes.Graphics.Drawing.CameraTransform3D;

    public class Camera3DPanel : Panel
    {
        #region Dependency - Camera        

        private static readonly PropertyFactory<Camera3DPanel> _PropFactory = new PropertyFactory<Camera3DPanel>();

        /// <summary>
        /// Attached property which can be set at parent elements with <see cref="SetCameraTo(UIElement, CAMERA)"/>
        /// </summary>
        static readonly DependencyProperty CameraProperty = _PropFactory
            .RegisterAttached
            (nameof(Camera)
            , CAMERA.Empty,
            FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            .Property;

        public CAMERA Camera
        {
            get => this.GetValue(CameraProperty) is CAMERA cam ? cam : default;
            set => SetCameraTo(this, value);
        }

        internal static void SetCameraTo(UIElement element, CAMERA value)
        {
            if (value.IsInitialized)
                element?.SetValue(CameraProperty, value);
            else
                element?.SetValue(CameraProperty, null);
        }

        #endregion

        #region RoutedEvent - bubble up scene size changed

        // https://stackoverflow.com/questions/4698047/event-bubbling-for-custom-event-in-wpf
        // https://stackoverflow.com/questions/24876053/the-correct-way-to-do-a-tunneled-event

        public static readonly RoutedEvent SceneSizeChangedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(SceneSizeChanged),
            RoutingStrategy.Bubble,
            typeof(SceneSizeChangedRoutedEventHandler),
            typeof(Camera3DPanel));

        public event SceneSizeChangedRoutedEventHandler SceneSizeChanged
        {
            add { AddHandler(SceneSizeChangedEvent, value); }
            remove { RemoveHandler(SceneSizeChangedEvent, value); }
        }

        protected void RaiseSceneSizeChanged((System.Numerics.Vector3 Center, float Radius) sphere)
        {
            RaiseEvent(new SceneSizeChangedEventArgs(SceneSizeChangedEvent, this, sphere));
        }

        public delegate void SceneSizeChangedRoutedEventHandler(object sender, SceneSizeChangedEventArgs e);

        public class SceneSizeChangedEventArgs : RoutedEventArgs
        {
            public SceneSizeChangedEventArgs(RoutedEvent routedEvent, object source, (System.Numerics.Vector3 Center, float Radius) sphere)
                : base(routedEvent, source)
            {
                Sphere = sphere;
            }

            public (System.Numerics.Vector3 Center, float Radius) Sphere { get; }

            // should add BoundingSphere
        }

        #endregion
    }
}
