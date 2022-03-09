using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using InteropTypes.Graphics.Drawing;

using COLOR = System.Drawing.Color;

namespace InteropTypes
{
    public struct _Scene2D : IDrawingBrush<ICanvas2D>
    {
        public void DrawTo(ICanvas2D dc)
        {
            dc.DrawCircle((0, 0), 50, COLOR.Red);
            dc.DrawCircle((200, 200), 50, COLOR.White);
            dc.DrawRectangle((175, 175), (50, 50), (COLOR.Transparent, COLOR.Red, 4));

            dc.DrawRectangle((480, 200), (130, 130), (COLOR.Transparent, COLOR.Red, 4), 12);

            dc.DrawRectangle((10, 10), (200, 200), (COLOR.Yellow, 2));

            // DrawFlower(_Drawing2D, new XY(450, 450), 4);

            dc.DrawFont((100, 100), 0.75f, "Hello World!", (COLOR.White, 2));

            // var bee = _CreateBeeModel2D(COLOR.Yellow);
            // _Drawing2D.DrawAsset(System.Numerics.Matrix3x2.CreateRotation(t) * System.Numerics.Matrix3x2.CreateTranslation(600, 350), bee, Color.White);
        }
    }    
}
