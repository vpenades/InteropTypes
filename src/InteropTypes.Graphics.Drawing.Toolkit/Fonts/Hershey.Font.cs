using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace InteropTypes.Graphics.Drawing.Fonts
{
    public abstract class HersheyFont : IFont
    {
        public static HersheyFont Simplex => HersheyFont0.Instance;

        private int _OffsetV = 0;
        private int _Height = -1;
        public int Height
        {
            get
            {
                if (_Height < 0)
                {
                    (_OffsetV, _Height) = _CalcFontHeight();
                }
                
                return _Height;
            }
        }

        private (int offset,int height) _CalcFontHeight()
        {
            int fmin = 0;
            int fmax = int.MinValue;

            foreach (var c in GetValidChars())
            {
                string glyphCode = GetSimplexCode(c);
                if (string.IsNullOrWhiteSpace(glyphCode)) continue;

                var glyph = new HersheyGlyphParser(glyphCode);

                var (min,max) = glyph.GetHeight();

                fmin = Math.Min(min, fmin);
                fmax = Math.Max(max, fmax);
            }

            int h = fmax - fmin;

            h *= 110;
            h /= 100;

            return (-fmin, h);
        }

        protected abstract IEnumerable<Char> GetValidChars();

        protected abstract string GetSimplexCode(char character);

        public Size MeasureTextLine(string text)
        {
            throw new NotImplementedException();
        }

        public void DrawTextLineTo(ICoreCanvas2D target, Matrix3x2 transform, string text, ColorStyle tintColor)
        {
            var offset = transform.Translation;

            if (target is Backends.IBackendCanvas2D backendCanvas)
            {
                void _drawPath(ReadOnlySpan<Point2> points) { backendCanvas.DrawThinLines(points, 0.1f, tintColor); }

                foreach (var c in text)
                {
                    _DrawGlyphAsLines(_drawPath, transform, ref offset, c);
                }
            }
            else
            {
                void _drawPath(ReadOnlySpan<Point2> points) { target.DrawLines(points, 0.1f, tintColor); }

                foreach (var c in text)
                {
                    _DrawGlyphAsLines(_drawPath, transform, ref offset, c);
                }
            }
        }

        public void DrawTextLineTo(IScene3D dc, Matrix4x4 xform, string text, FontStyle color)
        {
            var offset = xform.Translation;

            foreach (var c in text)
            {
                _DrawGlyphAsLines(dc, xform, ref offset, c, color);
            }
        }

        private void _DrawGlyphAsLines(HersheyGlyphParser.DrawFontPathCallback dc, in Matrix3x2 xform, ref Vector2 offset, char character)
        {
            string glyphCode = GetSimplexCode(character);

            if (string.IsNullOrWhiteSpace(glyphCode)) return;

            var glyph = new HersheyGlyphParser(glyphCode);

            offset += Vector2.TransformNormal(new Vector2(-glyph.Left, 0), xform);

            var xformFinal = xform;
            xformFinal.Translation = offset;            

            glyph.DrawPaths(xformFinal, dc, _OffsetV);

            offset += Vector2.TransformNormal(new Vector2(glyph.Right, 0), xformFinal);
        }

        private void _DrawGlyphAsLines(IScene3D dc, in Matrix4x4 xform, ref Vector3 offset, char character, FontStyle color)
        {
            string glyphCode = GetSimplexCode(character);

            if (string.IsNullOrWhiteSpace(glyphCode)) return;

            var glyph = new HersheyGlyphParser(glyphCode);

            offset += Vector3.TransformNormal(new Vector3(-glyph.Left, 0, 0), xform);

            foreach (var s in glyph.GetSegments(_OffsetV))
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
