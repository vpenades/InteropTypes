using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

using CAMERA = InteropTypes.Graphics.Drawing.CameraTransform3D;
using DRAWABLE = InteropTypes.Graphics.Drawing.IDrawingBrush<InteropTypes.Graphics.Drawing.IScene3D>;

namespace InteropTypes.Graphics.Backends.WPF
{
    public class Scene3DRoot : Primitives.Camera3DPanel, IAddChild
    {
        #region lifecycle

        public Scene3DRoot()
        {
            var isDesign = System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);
            if (isDesign) return;

            _RegisterBubbleUpSceneChanges();
            
            this.Unloaded += Scene3DRoot_Unloaded;
        }        

        private void Scene3DRoot_Unloaded(object sender, RoutedEventArgs e)
        {
            _UnregisterBubbleUpSceneChanges();
        }

        #endregion

        #region data

        private static readonly PropertyFactory<Scene3DRoot> _PropFactory = new PropertyFactory<Scene3DRoot>();

        private readonly DrawingContext2D _Context2D = new DrawingContext2D();

        private Drawing.Record3D _MasterScene = new Drawing.Record3D();

        #endregion        

        #region child scene management

        /* We use a bubbling routed event to propagate Scene changes from child Scene3DCanvas up to this control.
         * So we can keep a collection of all the scenes that need to be rendered.
         * 
         * Note that 3D layering is totally different compared to 2D layering:  in 3D layering we have to merge
         * all the scenes into one for depth sorting.
         */

        private void _RegisterBubbleUpSceneChanges()
        {
            ChildScenesChanged += OnChildSceneChanged;
        }

        private void _UnregisterBubbleUpSceneChanges()
        {
            ChildScenesChanged -= OnChildSceneChanged;
        }

        public event Scene3DSource.SceneChangedRoutedEventHandler ChildScenesChanged
        {
            add { AddHandler(Scene3DSource.SceneChangedEvent, value, true); }
            remove { RemoveHandler(Scene3DSource.SceneChangedEvent, value); }
        }

        private void OnChildSceneChanged(object sender, Scene3DSource.SceneChangedEventArgs e)
        {
            // _UpdateMasterScene();
        }

        #endregion

        #region API

        private void _UpdateMasterScene()
        {
            _MasterScene.Clear();

            foreach (var c in this.Children.OfType<Scene3DSource>())
            {
                var s = c.Scene;
                if (s != null) s.DrawTo(_MasterScene);
            }

            // var sphere = Drawing.Toolkit.GetAssetBoundingSphere(_MasterScene);
            // if (sphere.HasValue) { this.RaiseSceneSizeChanged(sphere.Value); }
        }

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc); // draw background            

            var rsize = this.RenderSize;

            if (double.IsNaN(rsize.Width) || double.IsNaN(rsize.Height)) return;

            var projection = Camera.CreateProjectionMatrix((float)(rsize.Width / rsize.Height));

            _UpdateMasterScene();

            _Context2D.SetContext(dc);
            _Context2D.DrawScene(rsize, projection, Camera.WorldMatrix, _MasterScene);
            _Context2D.SetContext(null);
        }

        #endregion
    }
}
