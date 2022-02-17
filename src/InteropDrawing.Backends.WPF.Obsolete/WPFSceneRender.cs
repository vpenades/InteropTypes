using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace InteropDrawing.Backends
{
    public class WPFSceneRender : DispatcherObject
    {
        #region lifecycle

        public WPFSceneRender() { }

        #endregion

        #region data

        private DrawingVisual _VectorDrawing = new DrawingVisual();
        private DrawingImage _VectorImage = new DrawingImage();

        private WPFDrawingContext2D _DeviceContext2D = new WPFDrawingContext2D();        

        private Model2D _SafeClone2D = new Model2D();
        private Model3D _SafeClone3D = new Model3D();

        #endregion

        #region properties

        public DrawingVisual VectorDrawing => _VectorDrawing;
        public ImageSource VectorImage => _VectorImage;

        #endregion

        #region API        

        public void Update(int width, int height, SceneView2D xform, Model2D scene)
        {
            Update(new Size(width, height), xform, scene);
        }

        public void Update(Size? renderSize, SceneView2D xform, Model2D scene)
        {
            if (CheckAccess()) { _UI_Update(renderSize, xform, scene); return; }

            scene.CopyTo(_SafeClone2D);

            this.Dispatcher.Invoke(() => _UI_Update(renderSize, xform, _SafeClone2D));
        }

        public void Update(Size? renderSize, SceneView3D xform, Model3D scene)
        {
            if (CheckAccess()) { _UI_Update(renderSize, xform, scene); return; }

            scene.CopyTo(_SafeClone3D);

            this.Dispatcher.Invoke(() => _UI_Update(renderSize, xform, _SafeClone3D));
        }

        private void _UI_Update(Size? renderSize, SceneView2D xform, Model2D scene)
        {
            _DeviceContext2D.DrawScene(_VectorDrawing, renderSize, xform, scene);
            _VectorImage.Drawing = _VectorDrawing.Drawing;
        }

        private void _UI_Update(Size? renderSize, SceneView3D xform, Model3D scene)
        {
            _DeviceContext2D.DrawScene(_VectorDrawing, renderSize, xform, scene);
            _VectorImage.Drawing = _VectorDrawing.Drawing;
        }

        #endregion
    }
}
