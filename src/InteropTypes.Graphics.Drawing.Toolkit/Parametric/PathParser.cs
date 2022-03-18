using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;


/* https://stackoverflow.com/questions/5115388/parsing-svg-path-elements-with-c-sharp-are-there-libraries-out-there-to-do-t
 * https://github.com/regebro/svg.path/blob/master/src/svg/path/parser.py
 * https://github.com/timrwood/SVGPath/blob/master/SVGPath/SVGPath.swift
 * https://github.com/wwwMADwww/ManuPathLib
 * https://www.w3.org/TR/SVG/paths.html
 */

namespace InteropTypes.Graphics.Drawing.Parametric
{
    public static class PathParser
    {
        public static void DrawPath(ICanvas2D dc, Matrix3x2 xform, string path, OutlineFillStyle style)
        {
            var shapes = ParseShapes(path);

            foreach (var shape in shapes)
            {
                var points = shape.AsSpan();

                if (points[0].Equals(points[points.Length - 1]))
                {
                    points = points.Slice(0, points.Length - 1);
                    dc.DrawPolygon(points, style);
                    continue;
                }

                if (style.HasFill) dc.DrawPolygon(points, style.FillColor);
                if (style.HasOutline) dc.DrawLines(points, style.OutlineWidth, style.OutlineColor);
            }
        }

        public static IEnumerable<Point2[]> ParseShapes(string path)
        {
            var commands = PathSlice.Parse(path);

            var values = new List<float>();
            var sequence = new List<Point2>();

            var head = Point2.Zero; // beginning of the shape, used to close with Z.
            var tcpt = Point2.Zero; // 2nd control point, used for smooth curves.
            var tail = Point2.Zero; // current point.

            foreach (var cmd in commands)
            {
                values.Clear(); cmd.CopyTo(values);

                switch (cmd.Command)
                {
                    case CommandType.ClosePath:
                        {
                            if (sequence.Count > 0)
                            {
                                sequence.Add(head);
                                yield return sequence.ToArray();
                            }
                            sequence.Clear();

                            tail = head;
                            break;
                        }

                    case CommandType.MoveTo:
                        {
                            if (sequence.Count > 0) yield return sequence.ToArray();
                            sequence.Clear();

                            tcpt = tail = _ApplyLineTo(sequence, tail, values, cmd.IsRelative);

                            head = sequence[0];

                            break;
                        }

                    case CommandType.LineHorizontalTo:
                        {
                            tcpt = tail = _ApplyLineTo(sequence, tail, values, cmd.IsRelative, true, false);
                            break;
                        }

                    case CommandType.LineVerticalTo:
                        {
                            tcpt = tail = _ApplyLineTo(sequence, tail, values, cmd.IsRelative, false, true);
                            break;
                        }

                    case CommandType.LineTo:
                        {
                            tcpt = tail = _ApplyLineTo(sequence, tail, values, cmd.IsRelative, true, true);
                            break;
                        }

                    case CommandType.Arc:
                        {
                            tcpt = tail = _ApplyArcTo(sequence, tail, values, cmd.IsRelative);
                            break;
                        }

                    case CommandType.CubicCurveTo:
                        {
                            sequence.Add(tail);

                            (tcpt, tail) = _ApplyCurveTo(sequence, tail, values, cmd.IsRelative);
                            break;
                        }

                    case CommandType.SmoothCubicCurveTo:
                        {
                            sequence.Add(tail);

                            (tcpt, tail) = _ApplySmoothCurveTo(sequence, tcpt.XY, tail, values, cmd.IsRelative);
                            break;
                        }

                    default: throw new NotImplementedException();
                }
            }

            if (sequence.Count > 0) yield return sequence.ToArray();
        }

        private static Point2 _ApplyLineTo(List<Point2> dst, Point2 tail, IReadOnlyList<float> src, bool srcIsRelative, bool h = true, bool v = true)
        {
            int i = 0;

            while (i < src.Count)
            {
                if (srcIsRelative)
                {
                    var x = h ? src[i++] : 0;
                    var y = v ? src[i++] : 0;
                    tail += new Point2(x, y);
                }
                else
                {
                    var x = h ? src[i++] : tail.X;
                    var y = v ? src[i++] : tail.Y;
                    tail = new Point2(x, y);
                }

                dst.Add(tail);
            }

            return tail;
        }

        private static Point2 _ApplyArcTo(List<Point2> dst, Point2 tail, IReadOnlyList<float> src, bool srcIsRelative, bool h = true, bool v = true)
        {
            for (int i = 0; i < src.Count; i += 7)
            {
                var p1 = tail.XY;

                var rr = new Vector2(src[i + 0], src[i + 1]);

                var xr = src[i + 2]; // x axis rotation, in degrees (0-360)
                var lf = src[i + 3]; // large flag
                var sf = src[i + 4]; // sweep flag

                var p2 = new Vector2(src[i + 5], src[i + 6]);
                if (srcIsRelative) p2 += p1;

                tail = p2;

                xr %= 360f;
                xr = xr.ToRadians();

                var arc = (p1, p2, rr, xr, lf != 0, sf != 0);

                for (int j = 1; j <= 10; ++j)
                {
                    var f = j / 10f;
                    var p = arc.LerpArc(f);
                    dst.Add(p);
                }
            }

            return tail;
        }

        private static (Point2, Point2) _ApplyCurveTo(List<Point2> dst, Point2 tail, IReadOnlyList<float> src, bool srcIsRelative)
        {
            var p1 = tail.XY;

            var mid = p1;

            for (int i = 0; i < src.Count; i += 6)
            {
                var p2 = new Vector2(src[i + 0], src[i + 1]);
                if (srcIsRelative) p2 += p1;

                var p3 = new Vector2(src[i + 2], src[i + 3]);
                if (srcIsRelative) p3 += p2;

                var p4 = new Vector2(src[i + 4], src[i + 5]);
                if (srcIsRelative) p4 += p3;

                _AddCurvePoints(dst, p1, p2, p3, p4);

                p1 = p4;
                mid = p3;
                tail = p4;
            }

            return (mid, tail);
        }

        private static (Point2, Point2) _ApplySmoothCurveTo(List<Point2> dst, Vector2 p2, Point2 tail, IReadOnlyList<float> src, bool srcIsRelative)
        {
            var p1 = tail.XY;

            for (int i = 0; i < src.Count; i += 4)
            {
                p2 = p1 + (p1 - p2); // reflect            

                var p3 = new Vector2(src[i + 2], src[i + 3]);
                if (srcIsRelative) p3 += p2;

                var p4 = new Vector2(src[i + 4], src[i + 5]);
                if (srcIsRelative) p4 += p3;

                _AddCurvePoints(dst, p1, p2, p3, p4);

                p2 = p3;
                p1 = p4;
                tail = p4;
            }

            return (p2, tail);
        }

        private static void _AddCurvePoints(List<Point2> dst, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            for (int i = 1; i <= 5; ++i)
            {
                var f = i / 5f;
                var p = (p1, p2, p3, p4).LerpCurve(f);
                dst.Add(p);
            }
        }

        enum CommandType
        {
            Undefined = 0,

            MoveTo = 'M',               // M (x y)+
            ClosePath = 'Z',            // Z
            LineTo = 'L',               // L (x y)+
            LineHorizontalTo = 'H',     // H x+
            LineVerticalTo = 'V',       // V y+

            CubicCurveTo = 'C',                  // C (x1 y1 x2 y2 x y)+
            SmoothCubicCurveTo = 'S',            // S (x2 y2 x y)+

            QuadraticCurveTo = 'Q',         // Q (x1 y1 x y)+
            SmoothQuadraticCurveTo = 'T',   // T (x y)+

            Arc = 'A',                  // A (rx ry x-axis-rotation large-arc-flag sweep-flag x y)+            
        }

        /// <summary>
        /// Represents a slice within a SVG Path string.
        /// </summary>
        /// <remarks>
        /// The slice starts with the command character (M, L, C, Z, etc),
        /// and follows with a sequence of floating point values.
        /// </remarks>
        [System.Diagnostics.DebuggerDisplay("{ToString()}")]
        readonly struct PathSlice
        {
            #region constants

            private static readonly char[] _Commands = "MZLHVCSQTAmzlhvcsqta".ToCharArray();

            #endregion

            #region lifecycle

            public static IEnumerable<PathSlice> Parse(string src)
            {
                int offset = 0;

                while (TryParse(src, offset, out var slice, out var next))
                {
                    System.Diagnostics.Debug.Assert(slice._Length > 0);

                    yield return slice;
                    offset = next;
                }
            }

            public static bool TryParse(string src, int head, out PathSlice slice, out int next)
            {
                head = src.IndexOfAny(_Commands, head);
                if (head < 0) { slice = default; next = -1; return false; }

                var tail = src.IndexOfAny(_Commands, head + 1);
                if (tail < 0) tail = src.Length;
                else tail -= 1;

                if (tail == head) { slice = default; next = -1; return false; }

                slice = new PathSlice(src, head, tail - head);

                next = tail;

                return true;
            }

            private PathSlice(string src, int ofs, int len)
            {
                _Source = src;
                _Offset = ofs;
                _Length = len;

                var c = src[ofs];
                System.Diagnostics.Debug.Assert(_Commands.Contains(c));
                _Command = (CommandType)(c & ~0x20);
                _IsRelative = c >= 'a';
            }

            #endregion

            #region data

            private readonly string _Source;
            private readonly int _Offset;
            private readonly int _Length;

            private readonly CommandType _Command;
            private readonly bool _IsRelative;

            #endregion

            #region properties

            public CommandType Command => _Command;
            public bool IsRelative => _IsRelative;
            public int Count => _Length;

            #endregion

            #region API

            public void CopyTo(List<float> values)
            {
                Span<byte> ascii = stackalloc byte[_Length - 1];

                for (int i = 0; i < ascii.Length; ++i)
                {
                    ascii[i] = (byte)_Source[_Offset + i + 1];
                }

                _CopyTo(ascii, values);
            }

            private static void _CopyTo(ReadOnlySpan<byte> ascii, List<float> values)
            {
                while (ascii.Length > 0)
                {
                    ascii = _SkipWhiteSpacesAndCommas(ascii);

                    if (!System.Buffers.Text.Utf8Parser.TryParse(ascii, out float value, out int valueLen)) throw new InvalidOperationException();

                    values.Add(value);

                    ascii = ascii.Slice(valueLen);
                }
            }

            private static ReadOnlySpan<byte> _SkipWhiteSpacesAndCommas(ReadOnlySpan<byte> ascii)
            {
                // skip leading whitespaces
                while (ascii.Length > 0)
                {
                    // allowed characters should be only those required to represent floating point, a la:  4523E-43 or -23.43

                    var c = ascii[0];
                    System.Diagnostics.Debug.Assert(c < 128, "Invalid character");
                    if (c > 0x20 && c != 0x2c) break;
                    ascii = ascii.Slice(1);
                }

                return ascii;
            }

            public override string ToString()
            {
                return _Source.Substring(_Offset, _Length);
            }

            #endregion
        }
    }
}
