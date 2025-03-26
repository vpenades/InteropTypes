using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends.Controls;
using InteropTypes.Graphics.Drawing;

namespace InteropTypes
{
    public class CustomCamera3DPanel : OrbitCamera3DViewport
    {
        public override float CameraPitch
        {
            set
            {
                if (value < -80) value = -80;
                if (value > -5) value = -5;
                base.CameraPitch = value;
            }
        }

        protected override void OnPrepareScene(Record3D scene)
        {
            scene.DrawFloorXY((-50, -50, -5), (100, 100), 10, System.Drawing.Color.LightGreen, System.Drawing.Color.LimeGreen);
            scene.DrawPivot((-2, 0, 0), 2);

            // animated
            var tmp = (DateTime.UtcNow - DateTime.Today).TotalSeconds;
            var pos = (5, 10, 5 + (float)Math.Cos(tmp) * 10);
            scene.DrawSphere(pos, 2, System.Drawing.Color.Yellow);

            var fontXform = System.Numerics.Matrix4x4.CreateScale(0.1f, -0.1f, 1) * System.Numerics.Matrix4x4.CreateTranslation(((Point3)pos).XYZ);

            scene.DrawTextLine(fontXform, $"Height:{tmp}", System.Drawing.Color.Black);
        }
    }
}
