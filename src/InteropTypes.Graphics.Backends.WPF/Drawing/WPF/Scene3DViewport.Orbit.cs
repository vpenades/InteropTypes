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
        static readonly StaticProperty<float> FieldOfViewProperty = _PropFactory.Register<float>(nameof(FieldOfView), 40, _ClampFov,  PROPERTYFLAGS.AffectsRender);

        private static float _ClampFov(float fov)
        {
            return Math.Max(10, Math.Min(150, fov));
        }

        public virtual float CameraYaw
        {
            get => CameraYawProperty.GetValue(this);
            set => CameraYawProperty.SetValue(this, value);
        }

        public virtual float CameraPitch
        {
            get => CameraPitchProperty.GetValue(this);
            set => CameraPitchProperty.SetValue(this, value);
        }

        public virtual float CameraZoom
        {
            get => CameraZoomProperty.GetValue(this);
            set => CameraZoomProperty.SetValue(this, value);
        }

        public virtual float FieldOfView
        {
            get => FieldOfViewProperty.GetValue(this);
            set => FieldOfViewProperty.SetValue(this, value);
        }

        #endregion

        #region API

        protected override void OnSceneChanged(IDrawingBrush<IScene3D> scene)
        {
            _SceneBounds = BoundingSphere.From(scene);            
        }

        public override CameraTransform3D GetCameraTransform3D()
        {
            if (!_SceneBounds.IsValid)
            {
                // System.Diagnostics.Debug.Fail("invalid camera");
                return CameraTransform3D.Identity;
            }

            var axisMatrix = UpDirectionIsZ
                ? Matrix4x4.CreateRotationX((float)Math.PI / 2)
                : Matrix4x4.Identity;            

            
            var dist = _SceneBounds.Radius * 3;
            dist *= (float)Math.Pow(2, CameraZoom);
            dist += _SceneBounds.Radius;            

            var matrix
                = Matrix4x4.CreateTranslation(0, 0, dist)
                * Matrix4x4.CreateFromAxisAngle(XYZ.UnitX, (float)(CameraPitch * Math.PI / 180.0))
                * Matrix4x4.CreateFromAxisAngle(XYZ.UnitY, -(float)(CameraYaw * Math.PI / 180.0));             
            
            if (this.UpDirectionIsZ)
            {
                matrix
                = Matrix4x4.CreateTranslation(0, -dist, 0)
                * Matrix4x4.CreateFromAxisAngle(XYZ.UnitX, (float)(CameraPitch * Math.PI / 180.0))
                * Matrix4x4.CreateFromAxisAngle(XYZ.UnitZ, -(float)(CameraYaw * Math.PI / 180.0));
            }

            matrix *= Matrix4x4.CreateTranslation(this._SceneBounds.Center);

            var fov = (float)(FieldOfView * Math.PI / 180.0);            

            var camxform = new CameraTransform3D(matrix, fov, null, 0.5f, float.PositiveInfinity);
            camxform.AxisMatrix = axisMatrix;

            return camxform;
        }        

        #endregion
    }
}
