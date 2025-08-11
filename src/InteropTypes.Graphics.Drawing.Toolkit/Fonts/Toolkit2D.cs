using System;

namespace InteropTypes.Graphics.Drawing
{
    partial class DrawingToolkit
    {

        public static void DrawTextLine(this ICanvas2D dc, POINT2 origin, String text, float size, FontStyle style)
        {
            var xform = XFORM2.CreateTranslation(origin.XY);
            dc.DrawTextLine(xform, text, size, style);
        }

        public static void DrawTextLine(this IScene3D dc, XFORM3 xform, String text, ColorStyle color)
        {
            Fonts.FontDrawing.DrawTextAsLines(dc, xform, text, color);
        }
    }
}
