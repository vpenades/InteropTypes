using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


/* https://stackoverflow.com/questions/5115388/parsing-svg-path-elements-with-c-sharp-are-there-libraries-out-there-to-do-t
 * https://github.com/regebro/svg.path/blob/master/src/svg/path/parser.py
 * https://github.com/timrwood/SVGPath/blob/master/SVGPath/SVGPath.swift
 * https://github.com/wwwMADwww/ManuPathLib
 */

namespace InteropDrawing.Parametric
{
    public static class PathParser
    {
        public static void DrawPath(IDrawing2D dc, System.Numerics.Matrix3x2 xform, string path, ColorStyle style)
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
            var commands = PathSegment.Parse(path);

            var values = new List<float>();
            var sequence = new List<Point2>();
            
            var tail = Point2.Zero;
            var head = tail;

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

                            _ApplyLineTo(sequence, tail, values, cmd.IsRelative);

                            head = sequence[0];

                            break;
                        }                                        

                    case CommandType.LineTo:
                        {
                            tail = _ApplyLineTo(sequence, tail, values, cmd.IsRelative);
                            break;
                        }

                    default: throw new NotImplementedException();
                }
            }

            if (sequence.Count > 0) yield return sequence.ToArray();
        }

        private static Point2 _ApplyLineTo(List<Point2> dst, Point2 tail, IReadOnlyList<float> src, bool srcIsRelative)
        {
            for (int i = 0; i < src.Count; i += 2)
            {
                var x = src[i + 0];
                var y = src[i + 1];
                var p = new Point2(x, y);

                tail = srcIsRelative ? tail + p : p;

                dst.Add(tail);
            }

            return tail;
        }


        // https://www.w3.org/TR/SVG/paths.html        
        // lowercase: relative values
        // uppercase: absolute values

        
        enum CommandType
        {
            Undefined = 0,

            MoveTo = 'M',               // M (x y)+
            ClosePath = 'Z',            // Z
            LineTo = 'L',               // L (x y)+
            LineHorizontalTo = 'H',     // H x+
            LineVerticalTo = 'V',       // V y+

            CurveTo = 'C',                  // C (x1 y1 x2 y2 x y)+
            SmoothCurveTo = 'S',            // S (x2 y2 x y)+

            QuadraticCurveTo = 'Q',         // Q (x1 y1 x y)+
            SmoothQuadraticCurveTo = 'T',   // T (x y)+

            Arc = 'A',                  // A (rx ry x-axis-rotation large-arc-flag sweep-flag x y)+            
        }

        readonly struct PathSegment
        {
            #region lifecycle

            public static IEnumerable<PathSegment> Parse(string src)
            {
                int offset = 0;

                while(TryParse(src,offset,out var segment))
                {
                    yield return segment;
                    offset += segment.Count;
                }
            }

            private static readonly char[] _Commands = "MZLHVCSQTAmzlhvcsqta".ToCharArray();

            public static bool TryParse(string src, int head, out PathSegment segment)
            {
                head = src.IndexOfAny(_Commands, head);
                if (head < 0) { segment = default; return false; }

                var tail = src.IndexOfAny(_Commands, head + 1);
                if (tail < 0) tail = src.Length;
                tail -= 1;

                if (tail == head) { segment = default; return false; }

                segment = new PathSegment(src, head, tail - head);
                return true;                
            }

            private PathSegment(string src, int ofs, int len)
            {
                _Source = src;

                var c = _Source[ofs];
                _Command = (CommandType)(((int)c) & ~0x20);
                _IsRelative = c >= (int)'a';

                _Offset = ofs;
                _Count = len;
            }

            #endregion

            #region data

            private readonly string _Source;
            private readonly int _Offset;
            private readonly int _Count;

            private readonly CommandType _Command;
            private readonly bool _IsRelative;

            #endregion

            #region properties

            public CommandType Command => _Command;
            public Boolean IsRelative => _IsRelative;
            public int Count => _Count;

            #endregion

            #region API

            public void CopyTo(List<float> values)
            {
                Span<Byte> ascii = stackalloc Byte[_Count - 1];

                for(int i=0; i < ascii.Length; ++i)
                {
                    ascii[i] = (Byte)_Source[_Offset + i + 1];
                }

                _CopyTo(ascii, values);
            }

            private static void _CopyTo(ReadOnlySpan<Byte> ascii, List<float> values)
            {
                while(ascii.Length > 0)
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
                    var c = ascii[0]; 
                    if (c > 0x20 && c != 0x2c) break;
                    ascii = ascii.Slice(1);                    
                }

                return ascii;
            }

            #endregion
        }
    }
}
