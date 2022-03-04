using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InteropTypes.Graphics.Backends
{
    public abstract class Camera3DService : DependencyObject, IServiceProvider
    {
        #region data

        private Point _LastMousePosition;
        private MouseButtonState _LastMouseState;

        private const float _ZoomRate = 0.3f;

        #endregion

        #region API - input

        public void OnMouseMove(IInputElement sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(sender);
            var delta = pos - _LastMousePosition;
            _LastMousePosition = pos;

            delta *= _ZoomRate;

            OnMouseMove(e, (pos.X,pos.Y), (delta.X,delta.Y));
        }

        protected virtual void OnMouseMove(MouseEventArgs e, Drawing.Point2 pos, Drawing.Point2 delta) { }

        public void OnMouseWheel(IInputElement sender, MouseWheelEventArgs e)
        {
            OnMouseWheel(e, (float)e.Delta * _ZoomRate * 0.01f);
        }

        protected virtual void OnMouseWheel(MouseWheelEventArgs e, float delta) { }

        #endregion

        #region API

        /// <inheritdoc />
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(Drawing.CameraTransform3D)) return GetCameraTransform();
            return null;
        }

        public abstract Drawing.CameraTransform3D GetCameraTransform();

        public void AttachTo(UIElement element)
        {
            WPF.Primitives.Camera3DPanel.SetCameraTo(element, this.GetCameraTransform());
        }

        #endregion
    }
}
