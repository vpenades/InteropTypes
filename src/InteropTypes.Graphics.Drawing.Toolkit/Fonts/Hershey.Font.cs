using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace InteropTypes.Graphics.Drawing.Fonts
{
    public abstract class HersheyFont : IFont
    {
        #region fonts

        public static HersheyFont Default => Simplex;

        public static HersheyFont Simplex => HersheyFont0.Instance;

        public static HersheyFont CreateWithThickness(float lineThickness)
        {
            if (lineThickness <=0) throw new ArgumentOutOfRangeException(nameof(lineThickness));
            return new HersheyFont0(lineThickness);
        }

        #endregion

        #region lifecycle

        protected HersheyFont(float lineThinckness)
        {
            _LineThickness = lineThinckness;
        }

        #endregion

        #region data

        private int? _OffsetV;
        private int? _Height;

        private readonly float _LineThickness;

        #endregion

        #region properties

        public int Height => _UseSettings().height;
        private (int offset,int height) _UseSettings()
        {
            if (!_OffsetV.HasValue || !_Height.HasValue)
            {
                var (o, h) = _CalcFontHeight();

                _OffsetV = o;
                _Height = h;
            }            

            return (_OffsetV.Value, _Height.Value);
        }

        #endregion

        #region API

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

        public RectangleF MeasureTextLine(string text)
        {
            var min = new Vector2(float.MaxValue);
            var max = new Vector2(float.MinValue);

            void _drawPath(ReadOnlySpan<Point2> points)
            {
                foreach(var p in points)
                {
                    min = Vector2.Min(min, p.XY);
                    max = Vector2.Max(max, p.XY);
                }                
            }

            var offset = Vector2.Zero;

            foreach (var c in text)
            {
                _DrawGlyphAsLines(_drawPath, Matrix3x2.Identity, ref offset, c);
            }

            var size = max - min;

            return new RectangleF(min.X, min.Y, size.X, size.Y);
        }

        public void DrawTextLineTo(ICoreCanvas2D target, Matrix3x2 transform, string text, ColorStyle tintColor)
        {
            var offset = transform.Translation;

            if (target is Backends.IBackendCanvas2D backendCanvas && _LineThickness < 1f)
            {
                void _drawPath(ReadOnlySpan<Point2> points) { backendCanvas.DrawThinLines(points, _LineThickness, tintColor); }

                foreach (var c in text)
                {
                    _DrawGlyphAsLines(_drawPath, transform, ref offset, c);
                }
            }
            else
            {
                void _drawPath(ReadOnlySpan<Point2> points) { target.DrawLines(points, _LineThickness, tintColor); }

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

            glyph.DrawPaths(xformFinal, dc, _UseSettings().offset);

            offset += Vector2.TransformNormal(new Vector2(glyph.Right, 0), xformFinal);
        }

        private void _DrawGlyphAsLines(IScene3D dc, in Matrix4x4 xform, ref Vector3 offset, char character, FontStyle color)
        {
            string glyphCode = GetSimplexCode(character);

            if (string.IsNullOrWhiteSpace(glyphCode)) return;

            var glyph = new HersheyGlyphParser(glyphCode);

            offset += Vector3.TransformNormal(new Vector3(-glyph.Left, 0, 0), xform);

            foreach (var s in glyph.GetSegments(_UseSettings().offset))
            {
                var a = new Vector3(s.Item1.Item1, s.Item1.Item2, 0);
                var b = new Vector3(s.Item2.Item1, s.Item2.Item2, 0);
                a = Vector3.TransformNormal(a, xform) + offset;
                b = Vector3.TransformNormal(b, xform) + offset;

                dc.DrawSegment(a, b, color.Strength, color.Color);
            }

            offset += Vector3.TransformNormal(new Vector3(glyph.Right, 0, 0), xform);
        }

        #endregion
    }
}
