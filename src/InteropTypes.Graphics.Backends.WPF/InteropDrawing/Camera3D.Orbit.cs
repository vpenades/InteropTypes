using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using XYZ = System.Numerics.Vector3;

using DRAWABLE = InteropTypes.Graphics.Drawing.IDrawingBrush<InteropTypes.Graphics.Drawing.IScene3D>;
using System.Windows;
using System.Windows.Input;

namespace InteropTypes.Graphics.Backends
{
    public class OrbitCamera3D : Camera3DService
    {
        #region lifecycle

        public OrbitCamera3D Clone()
        {
            return new OrbitCamera3D()
            {
                Yaw = this.Yaw,
                Pitch = this.Pitch,
                Target = this.Target,
                Distance = this.Distance
            };
        }

        #endregion

        #region dependency properties

        private static readonly PropertyFactory<OrbitCamera3D> _PropFactory = new PropertyFactory<OrbitCamera3D>();

        static readonly StaticProperty<DRAWABLE> SceneProperty = _PropFactory.Register<DRAWABLE>(nameof(Scene), null, _Update);

        /// <summary>
        /// Represents a drawable object
        /// </summary>
        public DRAWABLE Scene
        {
            get => SceneProperty.GetValue(this);
            set => SceneProperty.SetValue(this, value);
        }

        #endregion

        #region data

        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public XYZ Target { get; set; }
        public float Distance { get; set; } = 20;

        #endregion

        #region  API - Input        

        protected override void OnMouseMove(MouseEventArgs e, Drawing.Point2 pos, Drawing.Point2 delta)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Yaw -= (float)delta.X;
                Pitch -= (float)delta.Y;
            }
        }        

        protected override void OnMouseWheel(MouseWheelEventArgs e, float delta)
        {
            Distance += delta;

            if (Distance < 1) Distance = 1;            
        }

        #endregion

        #region API

        private static void _Update(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as OrbitCamera3D)?._UpdateSceneBounds();
        }

        private void _UpdateSceneBounds()
        {
            if (Scene == null) return;

            var bounds = Drawing.Toolkit.GetAssetBoundingSphere(Scene);

            this.Target = bounds.Value.Center;          // we could have a smooth factor
            this.Distance = bounds.Value.Radius * 10;    // we could have a smooth factor            
        }        

        public override Drawing.CameraTransform3D GetCameraTransform()
        {
            // if (Pitch < -80) Pitch = -80;
            // if (Pitch > -5) Pitch = -5;

            var matrix 
                = Matrix4x4.CreateTranslation(0, 0, -Distance) 
                * Matrix4x4.CreateFromAxisAngle(XYZ.UnitX, (float)(Pitch * Math.PI / 180.0))
                * Matrix4x4.CreateFromAxisAngle(XYZ.UnitY, -(float)(Yaw * Math.PI / 180.0))
                * Matrix4x4.CreateTranslation(this.Target);

            var fov = (float)(40 * Math.PI / 180.0);

            return new Drawing.CameraTransform3D(matrix, fov, null, 0.5f, float.PositiveInfinity);
        }
        
        #endregion
    }
}
