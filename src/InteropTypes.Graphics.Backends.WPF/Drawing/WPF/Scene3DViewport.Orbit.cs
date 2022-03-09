using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using InteropTypes.Graphics.Drawing;

using XYZ = System.Numerics.Vector3;

using PROPERTYFLAGS = System.Windows.FrameworkPropertyMetadataOptions;

namespace InteropTypes.Graphics.Backends.WPF
{
    using CAMERATEMPLATE = Scene3DViewportTemplate;

    public class OrbitCamera3DViewport : MouseCamera3DViewport
    {
        #region template

        internal static CAMERATEMPLATE CreateDefaultTemplate()
        {
            var template = new CAMERATEMPLATE();
            template.VisualTree = new System.Windows.FrameworkElementFactory(typeof(OrbitCamera3DViewport));
            template.Seal();
            return template;
        }

        #endregion

        #region data

        private CameraTransform3D _Camera = CameraTransform3D.Identity;

        private BoundingSphere _SceneBounds;

        #endregion

        #region mouse events

        protected override void OnMouseDrag(Vector2 delta, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                CameraYaw -= (float)delta.X;
                CameraPitch -= (float)delta.Y;
            }
        }

        protected override void OnMouseZoom(float zoom, MouseWheelEventArgs e)
        {
            CameraZoom = zoom;
        }

        #endregion

        #region dependency properties

        private static readonly PropertyFactory<OrbitCamera3DViewport> _PropFactory = new PropertyFactory<OrbitCamera3DViewport>();

        static readonly StaticProperty<float> CameraYawProperty = _PropFactory.Register<float>(nameof(CameraYaw), 0, PROPERTYFLAGS.AffectsRender);
        static readonly StaticProperty<float> CameraPitchProperty = _PropFactory.Register<float>(nameof(CameraPitch), 0, PROPERTYFLAGS.AffectsRender);
        static readonly StaticProperty<float> CameraZoomProperty = _PropFactory.Register<float>(nameof(CameraZoom), 0, PROPERTYFLAGS.AffectsRender);

        public float CameraYaw
        {
            get => CameraYawProperty.GetValue(this);
            set => CameraYawProperty.SetValue(this, value);
        }

        public float CameraPitch
        {
            get => CameraPitchProperty.GetValue(this);
            set => CameraPitchProperty.SetValue(this, value);
        }

        public float CameraZoom
        {
            get => CameraZoomProperty.GetValue(this);
            set => CameraZoomProperty.SetValue(this, value);
        }        

        #endregion

        #region API

        protected override void OnSceneChanged(IDrawingBrush<IScene3D> scene)
        {
            _SceneBounds = BoundingSphere.From(scene);
            if (!_SceneBounds.IsValid) _Camera = CameraTransform3D.Identity;
        }

        public override CameraTransform3D GetCameraTransform3D()
        {
            // if (Pitch < -80) Pitch = -80;
            // if (Pitch > -5) Pitch = -5;

            var z = (float)Math.Pow(2, CameraZoom);

            var dist = _SceneBounds.Radius * 3;
            dist *= z;
            dist += _SceneBounds.Radius;

            var matrix
                = Matrix4x4.CreateTranslation(0, 0, dist)
                * Matrix4x4.CreateFromAxisAngle(XYZ.UnitX, (float)(CameraPitch * Math.PI / 180.0))
                * Matrix4x4.CreateFromAxisAngle(XYZ.UnitY, -(float)(CameraYaw * Math.PI / 180.0))
                * Matrix4x4.CreateTranslation(this._SceneBounds.Center);

            var fov = (float)(40 * Math.PI / 180.0);

            return new CameraTransform3D(matrix, fov, null, 0.5f, float.PositiveInfinity);
        }        

        #endregion
    }
}
