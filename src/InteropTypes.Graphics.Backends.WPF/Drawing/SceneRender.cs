using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends
{
    public class SceneRender : DispatcherObject
    {
        #region lifecycle

        public SceneRender() { }

        #endregion

        #region data

        private DrawingVisual _VectorDrawing = new DrawingVisual();
        private DrawingImage _VectorImage = new DrawingImage();

        private Canvas2DFactory _DeviceContext2D = new Canvas2DFactory();        

        private Record2D _SafeClone2D = new Record2D();
        private Record3D _SafeClone3D = new Record3D();

        #endregion

        #region properties

        public DrawingVisual VectorDrawing => _VectorDrawing;
        public System.Windows.Media.ImageSource VectorImage => _VectorImage;

        #endregion

        #region API        

        public void Update(int width, int height, SceneView2D xform, Record2D scene)
        {
            Update(new Size(width, height), xform, scene);
        }

        public void Update(Size? renderSize, SceneView2D xform, Record2D scene)
        {
            if (CheckAccess()) { _UI_Update(renderSize, xform, scene); return; }

            scene.CopyTo(_SafeClone2D);

            this.Dispatcher.Invoke(() => _UI_Update(renderSize, xform, _SafeClone2D));
        }

        public void Update(Size? renderSize, SceneView3D xform, Record3D scene)
        {
            if (CheckAccess()) { _UI_Update(renderSize, xform, scene); return; }

            scene.CopyTo(_SafeClone3D);

            this.Dispatcher.Invoke(() => _UI_Update(renderSize, xform, _SafeClone3D));
        }

        private void _UI_Update(Size? renderSize, SceneView2D xform, Record2D scene)
        {
            _DeviceContext2D.DrawScene(_VectorDrawing, renderSize, xform, scene);
            _VectorImage.Drawing = _VectorDrawing.Drawing;
        }

        private void _UI_Update(Size? renderSize, SceneView3D xform, Record3D scene)
        {
            _DeviceContext2D.DrawScene(_VectorDrawing, renderSize, xform, scene);
            _VectorImage.Drawing = _VectorDrawing.Drawing;
        }

        #endregion
    }
}
