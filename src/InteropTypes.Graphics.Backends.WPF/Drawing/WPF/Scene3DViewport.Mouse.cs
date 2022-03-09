using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

using InteropTypes.Graphics.Drawing;

using XYZ = System.Numerics.Vector3;

namespace InteropTypes.Graphics.Backends.WPF
{
    public abstract class MouseCamera3DViewport : Primitives.Scene3DViewport
    {
        #region data

        private float _ZoomFactor = 0;

        private System.Windows.Point _LastMousePosition;

        #endregion

        #region mouse events        

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            // this override ensures that all the mouse events are handled
            // regardless of hitting a primitive or the background
            //
            // https://stackoverflow.com/questions/3269120/wpf-frameworkelement-not-receiving-mouse-input

            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            // https://social.msdn.microsoft.com/Forums/vstudio/en-US/c22ebb2e-cb27-4ae3-bfe4-8f66f8a5e9f2/contentpresenter-click-event?forum=wpf

            base.OnPreviewMouseMove(e);

            var pos = e.GetPosition(this);
            var delta = pos - _LastMousePosition;
            _LastMousePosition = pos;

            delta /= 3f;

            OnMouseDrag(new Vector2((float)delta.X, (float)delta.Y), e);
        }       

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            _ZoomFactor += (float)e.Delta /1200f;

            OnMouseZoom(_ZoomFactor, e);
        }

        protected virtual void OnMouseDrag(Vector2 delta, MouseEventArgs e) { }

        protected virtual void OnMouseZoom(float zoom, MouseWheelEventArgs e) { }

        #endregion
    }
}
