using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using InteropTypes.Graphics.Drawing;

using XYZ = System.Numerics.Vector3;

namespace InteropTypes.Graphics.Backends.WPF
{
    using CAMERATEMPLATE = System.Windows.Controls.ControlTemplate;

    public class OrbitCamera3DPanel : Primitives.Scene3DPanel
    {
        public static CAMERATEMPLATE CreateDefaultTemplate()
        {            
            var template = new CAMERATEMPLATE();
            template.VisualTree = new System.Windows.FrameworkElementFactory(typeof(OrbitCamera3DPanel));
            template.Seal();
            return template;
        }

        private CameraTransform3D _Camera = CameraTransform3D.Identity;

        private BoundingSphere _SceneBounds;

        private float _Yaw;
        private float _Pitch;
        private float _Distance;

        private float _ZoomFactor = 0;

        private System.Windows.Point _LastMousePosition;
        

        protected override void OnSceneChanged(IDrawingBrush<IScene3D> scene)
        {
            if (scene == null)
            {
                _Camera = CameraTransform3D.Identity;
                _SceneBounds = default;
            }
            else
            {
                _SceneBounds = BoundingSphere.From(scene);
                _Distance = _SceneBounds.Radius * 4;                
            }

            this.Background = System.Windows.Media.Brushes.Transparent;
        }

        public override CameraTransform3D GetCameraTransform3D()
        {
            // if (Pitch < -80) Pitch = -80;
            // if (Pitch > -5) Pitch = -5;

            var z = (float)Math.Pow(2, _ZoomFactor);

            var dist = _Distance - _SceneBounds.Radius;
            dist *= z;
            dist += _SceneBounds.Radius;

            var matrix
                = Matrix4x4.CreateTranslation(0, 0, dist)
                * Matrix4x4.CreateFromAxisAngle(XYZ.UnitX, (float)(_Pitch * Math.PI / 180.0))
                * Matrix4x4.CreateFromAxisAngle(XYZ.UnitY, -(float)(_Yaw * Math.PI / 180.0))
                * Matrix4x4.CreateTranslation(this._SceneBounds.Center);

            var fov = (float)(40 * Math.PI / 180.0);

            return new CameraTransform3D(matrix, fov, null, 0.5f, float.PositiveInfinity);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            // https://social.msdn.microsoft.com/Forums/vstudio/en-US/c22ebb2e-cb27-4ae3-bfe4-8f66f8a5e9f2/contentpresenter-click-event?forum=wpf

            base.OnPreviewMouseMove(e);

            var pos = e.GetPosition(this);
            var delta = pos - _LastMousePosition;
            _LastMousePosition = pos;

            delta /= 3f;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _Yaw -= (float)delta.X;
                _Pitch -= (float)delta.Y;

                this.InvalidateVisual();
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            _ZoomFactor += (float)e.Delta /1200f;
            this.InvalidateVisual();
        }
    }
}
