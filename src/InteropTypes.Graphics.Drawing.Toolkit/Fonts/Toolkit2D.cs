using System;
using System.Collections.Generic;
using System.Text;

using POINT2 = InteropTypes.Graphics.Drawing.Point2;
using XFORM2 = System.Numerics.Matrix3x2;

namespace InteropTypes.Graphics.Drawing
{
    partial class DrawingToolkit
    {
        public static void DrawFont(this ICanvas2D dc, POINT2 origin, float size, String text, FontStyle style)
        {
            var xform = XFORM2.CreateScale(size);
            xform.Translation = origin.XY;

            style = style.With(style.Strength * size);

            dc.DrawFont(xform, text, style);
        }

        public static void DrawFont(this ICanvas2D dc, XFORM2 xform, String text, FontStyle style)
        {
            float xflip = 1;
            float yflip = 1;

            if (style.Alignment.HasFlag(FontAlignStyle.FlipHorizontal)) { xflip = -1; }
            if (style.Alignment.HasFlag(FontAlignStyle.FlipVertical)) { yflip = -1; }

            if (style.Alignment.HasFlag(FontAlignStyle.FlipAuto) && dc.TryGetQuadrant(out var q))
            {
                if (q.HasFlag(Quadrant.Top)) yflip *= -1;
            }

            style = style.With(style.Alignment & ~(FontAlignStyle.FlipHorizontal | FontAlignStyle.FlipVertical));

            xform = XFORM2.CreateScale(xflip, yflip) * xform;

            Fonts.FontDrawing.DrawFontAsLines(dc, xform, text, style);
        }

        public static void DrawText(this ICoreCanvas2D dc, XFORM2 xform, string text, FontStyle style)
        {
            if (style.Font is Fonts.IBitmapFont bmpFont)
            {
                bmpFont.DrawTextTo(dc, xform, text, style.Style.FillColor);
                return;
            }

            throw new NotImplementedException();            
        }
    }
}
