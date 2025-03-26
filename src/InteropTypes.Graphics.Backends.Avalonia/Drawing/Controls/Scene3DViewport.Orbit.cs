using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends.Controls
{
    using CAMERATEMPLATE = ITemplate<Primitives.Scene3DViewport>;    

    public class OrbitCamera3DViewport : MouseCamera3DViewport
    {
        #region lifecycle

        static OrbitCamera3DViewport()
        {
            AffectsRender<OrbitCamera3DViewport>(CameraYawProperty, CameraPitchProperty, CameraZoomProperty, FieldOfViewProperty);
        }

        #endregion

        #region data        

        private BoundingSphere _SceneBounds;

        #endregion

        #region mouse events        

        protected override void OnMouseDrag(Vector2 delta, PointerEventArgs e)
        {            
            if (e.Pointer.Captured == this)
            {
                CameraYaw -= (float)delta.X;
                CameraPitch -= (float)delta.Y;
            }
        }

        protected override void OnMouseZoom(float zoom, PointerWheelEventArgs e)
        {
            CameraZoom = zoom;
        }

        #endregion

        #region dependency properties        

        public static readonly StyledProperty<float> CameraYawProperty = AvaloniaProperty.Register<OrbitCamera3DViewport,float>(nameof(CameraYaw), 0);
        public static readonly StyledProperty<float> CameraPitchProperty = AvaloniaProperty.Register<OrbitCamera3DViewport, float>(nameof(CameraPitch), 0);
        public static readonly StyledProperty<float> CameraZoomProperty = AvaloniaProperty.Register<OrbitCamera3DViewport, float>(nameof(CameraZoom), 0);
        public static readonly StyledProperty<float> FieldOfViewProperty = AvaloniaProperty.Register<OrbitCamera3DViewport, float>(nameof(FieldOfView), 40); // _ClampFov

        private static float _ClampFov(float fov)
        {
            return Math.Max(10, Math.Min(150, fov));
        }

        public virtual float CameraYaw
        {
            get => GetValue(CameraYawProperty);
            set => SetValue(CameraYawProperty, value);
        }

        public virtual float CameraPitch
        {
            get => GetValue(CameraPitchProperty);
            set => SetValue(CameraPitchProperty, value);
        }

        public virtual float CameraZoom
        {
            get => GetValue(CameraZoomProperty);
            set => SetValue(CameraZoomProperty, value);
        }

        public virtual float FieldOfView
        {
            get => GetValue(FieldOfViewProperty);
            set => SetValue(FieldOfViewProperty, value);
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
