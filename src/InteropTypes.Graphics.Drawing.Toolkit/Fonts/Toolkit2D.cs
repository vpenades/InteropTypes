using System;

using POINT2 = InteropTypes.Graphics.Drawing.Point2;
using XFORM2 = System.Numerics.Matrix3x2;

namespace InteropTypes.Graphics.Drawing
{
    partial class DrawingToolkit
    {

        public static void DrawTextLine(this ICanvas2D dc, POINT2 origin, String text, float size, FontStyle style)
        {
            var xform = XFORM2.CreateTranslation(origin.XY);
            dc.DrawTextLine(xform, text, size, style);
        }

        public static void DrawTextLine(this IScene3D dc, System.Numerics.Matrix4x4 xform, String text, ColorStyle color)
        {
            Fonts.FontDrawing.DrawTextAsLines(dc, xform, text, color);
        }
    }
}
