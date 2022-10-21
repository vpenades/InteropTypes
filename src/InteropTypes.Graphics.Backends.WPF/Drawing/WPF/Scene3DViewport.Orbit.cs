using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using InteropTypes.Graphics.Drawing;

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

            var fov = (float)(FieldOfView * Math.PI / 180.0);            

            var camxform = CameraTransform3D
                .CreatePerspective(fov)
                .WithPlanes(0.5f, float.PositiveInfinity)
                .WithAxisMatrix(UpDirectionIsZ ? CameraTransform3D.ZUpAxisMatrix : Matrix4x4.Identity);            

            var targetDist = _SceneBounds.Radius * 3;
            targetDist *= (float)Math.Pow(2, CameraZoom);
            targetDist += _SceneBounds.Radius;

            var yaw = (float)(CameraYaw * Math.PI / 180.0);
            var pitch = (float)(CameraPitch * Math.PI / 180.0);

            camxform.SetOrbitWorldMatrix(this._SceneBounds.Center, targetDist, yaw, pitch, 0);

            return camxform;
        }        

        #endregion
    }
}
