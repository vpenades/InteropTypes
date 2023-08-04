using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using InteropTypes.Graphics.Drawing;

using COLOR = System.Drawing.Color;
using InteropTypes.Graphics.Drawing.Transforms;

namespace InteropTypes
{
    public class _Scene2D : GroupBox.Collection
    {
        protected override void Initialize()
        {        
            Render(50, 50, _DrawCircle);
            Render(50, 50, _DrawRectangle);            
            Render(80, 40, dc => dc.DrawTextLine((5, 5), "Hello World!", 15, COLOR.Violet));

            Render(50, 50, _DrawRoundRectangle);
            Render(120, 60, _DecomposedNonConvexPolygon);
        }

        private OutlineFillStyle GetStyle(COLOR fill, COLOR outline, float thickness)
        {
            var flags = (int)(ElapsedTime % 8);

            if (flags < 2) flags = 2;

            var xfill = (flags & 2) != 0 ? fill : COLOR.Transparent;
            var xoutl = (flags & 4) != 0 ? outline : COLOR.Transparent;

            return (xfill, xoutl, thickness);
        }

        private void _DrawRectangle(ICanvas2D dc)
        {
            var style = GetStyle(COLOR.Red, COLOR.Black, 5);

            dc.DrawRectangle((0, 0), (50, 50), style);
        }

        private void _DrawCircle(ICanvas2D dc)
        {
            var style = GetStyle(COLOR.Red, COLOR.Black, 5);

            var r0 = MathF.Sin(ElapsedTime) * 10;
            var r1 = MathF.Cos(ElapsedTime * 1.5f) * 10;

            dc.DrawEllipse((25, 25), 40 + r0, 40 + r1, style);
        }

        private void _DrawRoundRectangle(ICanvas2D dc)
        {
            var style = GetStyle(COLOR.Red, COLOR.Black, 5);

            var r = MathF.Sin(ElapsedTime) * 10;

            dc.DrawRectangle((0, 0), (50, 50), style, 12f + r);
        }

        private void _DecomposedNonConvexPolygon(ICanvas2D dc)
        {
            _NonConvexPolygon(dc);

            dc = dc.CreateTransformed2D(System.Numerics.Matrix3x2.CreateTranslation(60, 0));
            dc = new Decompose2D(dc);

            _NonConvexPolygon(dc);
        }

        private void _NonConvexPolygon(ICanvas2D dc)
        {
            var style = GetStyle(COLOR.Red, COLOR.Black, 5);

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
