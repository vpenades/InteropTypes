using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends.Controls
{
    public class AutoCamera3DViewport : Primitives.Scene3DViewport
    {
        private CameraTransform3D _Camera = CameraTransform3D.Identity;

        protected override void OnSceneChanged(IDrawingBrush<IScene3D> scene)
        {
            if (scene == null) { _Camera = CameraTransform3D.Identity; }
            else
            {
                var record = new Record3D();
                scene.DrawTo(record);

                _Camera = CameraView3D.CreateDefaultFrom(record.BoundingMatrix);
            }
        }

        public override CameraTransform3D GetCameraTransform3D()
        {
            return _Camera;
        }
    }
}
