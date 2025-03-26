using Avalonia;
using Avalonia.Input;
using Avalonia.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using XYZ = System.Numerics.Vector3;

namespace InteropTypes.Graphics.Backends.Controls
{
    public abstract class MouseCamera3DViewport : Primitives.Scene3DViewport, ICustomHitTest
    {
        #region data

        private float _ZoomFactor = 0;

        private Avalonia.Point _LastMousePosition;

        #endregion

        #region mouse events        

        public bool HitTest(Point point)
        {
            // this override ensures that all the mouse events are handled
            // regardless of hitting a primitive or the background
            return true;
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            e.Pointer.Capture(this);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            e.Pointer.Capture(null);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            var pos = e.GetPosition(this);
            var delta = pos - _LastMousePosition;
            _LastMousePosition = pos;

            delta /= 3f;

            OnMouseDrag(new Vector2((float)delta.X, (float)delta.Y), e);
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);            

            _ZoomFactor += (float)e.Delta.Y / 10;

            OnMouseZoom(_ZoomFactor, e);
        }

        protected virtual void OnMouseDrag(Vector2 delta, PointerEventArgs e) { }

        protected virtual void OnMouseZoom(float zoom, PointerWheelEventArgs e) { }        

        #endregion
    }
}
