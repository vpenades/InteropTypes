using System;
using System.Numerics;

namespace InteropTypes.Graphics.Drawing.Fonts
{
    static class FontDrawing
    {
        public static void DrawFontAsLines(ICanvas2D dc, in Matrix3x2 xform, string text, FontStyle color)
        {
            var offset = xform.Translation;

            foreach (var c in text)
            {
                DrawGlyphAsLines(dc, xform, ref offset, c, color);
            }
        }

        public static void DrawFontAsLines(IScene3D dc, in Matrix4x4 xform, string text, FontStyle color)
        {
            var offset = xform.Translation;

            foreach (var c in text)
            {
                DrawGlyphAsLines(dc, xform, ref offset, c, color);
            }
        }

        public static void DrawGlyphAsLines(ICanvas2D dc, in Matrix3x2 xform, ref Vector2 offset, char character, FontStyle color)
        {
            string glyphCode = VectorFonts.GetSimplexCode(character);

            if (string.IsNullOrWhiteSpace(glyphCode)) return;

            var glyph = new HersheyGlyphParser(glyphCode);

            offset += Vector2.TransformNormal(new Vector2(-glyph.Left, 0), xform);

            var xformFinal = xform;
            xformFinal.Translation = offset;

            void _drawPath(ReadOnlySpan<Point2> points)
            {
                dc.DrawLines(points, color.Strength, color.Style.FillColor);
            }

            glyph.DrawPaths(xformFinal, _drawPath);

            offset += Vector2.TransformNormal(new Vector2(glyph.Right, 0), xformFinal);
        }

        public static void DrawGlyphAsLines(IScene3D dc, in Matrix4x4 xform, ref Vector3 offset, char character, FontStyle color)
        {
            string glyphCode = VectorFonts.GetSimplexCode(character);

            if (string.IsNullOrWhiteSpace(glyphCode)) return;

            var glyph = new HersheyGlyphParser(glyphCode);

            offset += Vector3.TransformNormal(new Vector3(-glyph.Left, 0, 0), xform);

            foreach (var s in glyph.GetSegments())
            {
                var a = new Vector3(s.Item1.Item1, s.Item1.Item2, 0);
                var b = new Vector3(s.Item2.Item1, s.Item2.Item2, 0);
                a = Vector3.TransformNormal(a, xform) + offset;
                b = Vector3.TransformNormal(b, xform) + offset;

                dc.DrawSegment(a, b, color.Strength, color.Style.FillColor);
            }

            offset += Vector3.TransformNormal(new Vector3(glyph.Right, 0, 0), xform);
        }

    }
}
