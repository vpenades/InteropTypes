using System;
using System.Collections.Generic;
using System.Numerics;

namespace InteropTypes.Graphics.Drawing.Fonts
{
    readonly struct HersheyGlyphParser
    {
        #region constructor

        /// <summary>
        /// Creates a parser for a Hershey glyph code.
        /// </summary>
        /// <param name="glyph">A valid Hershey glyph code.</param>
        /// <example>
        /// "9MWOMOV RUMUV ROQUQ"
        /// </example>
        public HersheyGlyphParser(string glyph)
        {
            _Glyph = glyph;
            _CoordStart = 0;
            _CoordCount = 0;

            foreach (var c in glyph)
            {
                if (char.IsDigit(c))
                {
                    _CoordCount *= 10;
                    _CoordCount += c - '0';
                    ++_CoordStart;
                    continue;
                }

                break;
            }
        }

        #endregion

        #region data

        private readonly string _Glyph;

        private readonly int _CoordStart;
        private readonly int _CoordCount;

        #endregion

        #region properties

        public int Left => _Glyph[_CoordStart + 0] - 'R';
        public int Right => _Glyph[_CoordStart + 1] - 'R';

        public int Count => _CoordCount - 1;

        public (int, int)? this[int index]
        {
            get
            {
                index += 1;

                var x = _Glyph[_CoordStart + index * 2 + 0];
                if (char.IsWhiteSpace(x)) return null;
                var y = _Glyph[_CoordStart + index * 2 + 1];

                return (x - 'R', y - 'R');
            }
        }

        #endregion

        #region API

        public IEnumerable<((int, int), (int, int))> GetSegments()
        {
            bool moveTo = true;

            var prev = (0, 0);

            for (int i = 0; i < Count; ++i)
            {
                var p = this[i];

                if (!p.HasValue) { moveTo = true; continue; }

                if (!moveTo) yield return (prev, p.Value);

                moveTo = false;
                prev = p.Value;
            }
        }

        public delegate void DrawFontPathCallback(ReadOnlySpan<Point2> points);
        public void DrawPaths(Matrix3x2 xform, DrawFontPathCallback callback)
        {
            Span<Point2> points = stackalloc Point2[256];

            int idx = 0;

            for (int i = 0; i < Count; ++i)
            {
                var p = this[i];

                if (!p.HasValue)
                {
                    if (idx >= 2) callback(points.Slice(0, idx));
                    idx = 0;
                    continue;
                }

                points[idx++] = Vector2.Transform(new Vector2(p.Value.Item1, p.Value.Item2), xform);
            }

            if (idx >= 2) callback(points.Slice(0, idx));
        }

        #endregion
    }
}
