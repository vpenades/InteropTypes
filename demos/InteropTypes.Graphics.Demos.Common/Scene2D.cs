using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using InteropTypes.Graphics.Drawing;

using COLOR = System.Drawing.Color;

namespace InteropTypes
{
    public class _Scene2D : GroupBox.Collection
    {
        protected override void Initialize()
        {        
            Render(50, 50, dc => dc.DrawCircle((25, 25), 50, COLOR.Red));
            Render(50, 50, dc => dc.DrawRectangle((0, 0), (50, 50), (COLOR.Transparent, COLOR.Red, 4)));
            Render(50, 50, dc => dc.DrawRectangle((0, 0), (50, 50), (COLOR.Transparent, COLOR.Red, 4), 12));
            Render(80, 40, dc => dc.DrawTextLine((5, 5), "Hello World!", 15, COLOR.Violet));

            Render(60, 60, _NonConvexPolygon);         
        }        

        private void _NonConvexPolygon(ICanvas2D dc)
        {
            var style = ElapsedTime % 4 < 2
                ? (COLOR.Transparent, COLOR.Black.WithOpacity(0.5f), 5)
                : (COLOR.Black.WithOpacity(0.5f),COLOR.Transparent, 0);

            var scr = new System.Drawing.RectangleF(10, 10, 50, 50);
            var Width = 20f;
            dc.DrawPolygon(style,
                (scr.Left, scr.Top),
                (scr.Right, scr.Top),
                (scr.Right, scr.Bottom),
                (scr.Right - Width, scr.Bottom),
                (scr.Right - Width, scr.Top + Width),
                (scr.Left, scr.Top + Width),
                (scr.Left, scr.Top));
        }
    }    
}
