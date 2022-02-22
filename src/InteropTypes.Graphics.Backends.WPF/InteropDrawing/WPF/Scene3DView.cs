using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using WPFVECTOR = System.Windows.Media.Media3D.Vector3D;
using DRAWABLE = InteropTypes.Graphics.Drawing.IDrawingBrush<InteropTypes.Graphics.Drawing.IScene3D>;

namespace InteropTypes.Graphics.Backends.WPF
{
    /// <summary>
    /// Represents a view that controls the camera of the scene3D object inside the content.
    /// </summary>
    public class Scene3DView : ContentControl
    {
        #region lifecycle

        public Scene3DView()
        {
            var isDesign = System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);
            if (isDesign) return;

            if (_Camera == null) _Camera = new OrbitCamera3D();

            _RegisterBubbleSceneChanges();
            _RegisterBubbleUpSceneSizeChanges();

            this.Loaded += Scene3DView_Loaded;
            this.Unloaded += Scene3DView_Unloaded;            
        }        

        private void Scene3DView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {           
            _Camera.AttachTo(this);            
        }        

        private void Scene3DView_Unloaded(object sender, RoutedEventArgs e)
        {
            _UnregisterBubbleSceneChanges();
            _UnregisterBubbleUpSceneSizeChanges();
        }

        #endregion

        #region data

        private OrbitCamera3D _Camera;

        private Drawing.CameraTransform3D _LastCamera = Drawing.CameraTransform3D.Empty;

        private readonly Dictionary<Object, DRAWABLE> _VisibleScenes = new Dictionary<Object, DRAWABLE>();

        #endregion

        #region scene size scene management        

        private void _RegisterBubbleUpSceneSizeChanges()
        {
            ChildScenesSizeChanged += OnChildSceneSizeChanged;
        }

        private void _UnregisterBubbleUpSceneSizeChanges()
        {
            ChildScenesSizeChanged -= OnChildSceneSizeChanged;
        }

        public event Primitives.Camera3DPanel.SceneSizeChangedRoutedEventHandler ChildScenesSizeChanged
        {
            add { AddHandler(Primitives.Camera3DPanel.SceneSizeChangedEvent, value, true); }
            remove { RemoveHandler(Primitives.Camera3DPanel.SceneSizeChangedEvent, value); }
        }

        private void OnChildSceneSizeChanged(object sender, Primitives.Camera3DPanel.SceneSizeChangedEventArgs e)
        {
            _Camera.Target = e.Sphere.Center;
            _Camera.Distance = e.Sphere.Radius * 4;
            _UpdateCamera();
        }

        #endregion

        #region child scene management

        /* We use a bubbling routed event to propagate Scene changes from child Scene3DCanvas up to this control.
         * So we can keep a collection of all the scenes that need to be rendered.
         * 
         * Note that 3D layering is totally different compared to 2D layering:  in 3D layering we have to merge
         * all the scenes into one for depth sorting.
         */

        private void _RegisterBubbleSceneChanges()
        {
            ChildScenesChanged += Scene3DView_ChildScenesChanged;
        }

        private void _UnregisterBubbleSceneChanges()
        {
            ChildScenesChanged -= Scene3DView_ChildScenesChanged;
        }

        public event Primitives.Scene3DCanvas.SceneChangedRoutedEventHandler ChildScenesChanged
        {
            add { AddHandler(Primitives.Scene3DCanvas.SceneChangedEvent, value, true); }
            remove { RemoveHandler(Primitives.Scene3DCanvas.SceneChangedEvent, value); }
        }

        private void Scene3DView_ChildScenesChanged(object sender, Primitives.Scene3DCanvas.SceneChangedEventArgs e)
        {
            if (e.Scene == null) _VisibleScenes.Remove(e.Source);
            else _VisibleScenes[e.Source] = e.Scene;
        }        

        #endregion

        #region mouse events

        // https://stackoverflow.com/questions/20899810/previewmousemove-vs-mousemove
        // https://docs.microsoft.com/en-us/dotnet/desktop/wpf/advanced/routed-events-overview?redirectedfrom=MSDN&view=netframeworkdesktop-4.8
        // https://web.archive.org/web/20140225041901/http://www.csharptutorial.in/2012/10/understanding-event-handling-in-wpf.html

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e); // do not call this to block to upper controls

            if (_Camera == null) return;

            _Camera.OnMouseMove(this, e);

            var cam = _Camera.GetCameraTransform();
            if (cam != _LastCamera) _UpdateCamera();
            _LastCamera = cam;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e); // do not call this to block to upper controls

            if (_Camera == null) return;

            _Camera.OnMouseWheel(this, e);

            var cam = _Camera.GetCameraTransform();
            if (cam != _LastCamera) _UpdateCamera();
            _LastCamera = cam;
        }

        private void _UpdateCamera()
        {
            _Camera.Clone().AttachTo(this); // this triggers setting the current camera as an attached property to all child objects
        }

        #endregion
    }    
}
