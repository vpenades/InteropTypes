using System;
using System.Numerics;

namespace InteropTypes.Graphics.Drawing.Fonts
{
    static class FontDrawing
    {
        public static void DrawTextAsLines(ICanvas2D dc, in Matrix3x2 xform, string text, ColorStyle color)
        {
            HersheyFont0.Instance.DrawTextLineTo(dc,xform,text, color);
        }

        public static void DrawTextAsLines(IScene3D dc, in Matrix4x4 xform, string text, ColorStyle color)
        {
            HersheyFont0.Instance.DrawTextLineTo(dc, xform, text, color);
        }
    }
}
