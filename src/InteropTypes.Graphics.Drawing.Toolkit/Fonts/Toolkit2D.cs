using System;
using System.Collections.Generic;
using System.Text;

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
            Fonts.FontDrawing.DrawFontAsLines(dc, xform, text, color);
        }   

        [Obsolete("Use DrawTextLine", true)]
        public static void DrawFont(this ICanvas2D dc, POINT2 origin, float size, String text, FontStyle style)
        {
            var xform = XFORM2.CreateScale(size);
            xform.Translation = origin.XY;

            style = style.With(style.Strength * size);

            dc.DrawFont(xform, text, style);
        }

        [Obsolete("Use DrawTextLine", true)]
        public static void DrawFont(this ICanvas2D dc, XFORM2 xform, String text, FontStyle style)
        {
            float xflip = 1;
            float yflip = 1;

            if (style.Alignment.HasFlag(Fonts.FontAlignStyle.FlipHorizontal)) { xflip = -1; }
            if (style.Alignment.HasFlag(Fonts.FontAlignStyle.FlipVertical)) { yflip = -1; }

            if (style.Alignment.HasFlag(Fonts.FontAlignStyle.FlipAuto) && dc.TryGetQuadrant(out var q))
            {
                if (q.HasFlag(Quadrant.Top)) yflip *= -1;
            }

            style = style.With(style.Alignment & ~(Fonts.FontAlignStyle.FlipHorizontal | Fonts.FontAlignStyle.FlipVertical));

            xform = XFORM2.CreateScale(xflip, yflip) * xform;

            Fonts.FontDrawing.DrawFontAsLines(dc, xform, text, style.Color);
        }        
    }
}
