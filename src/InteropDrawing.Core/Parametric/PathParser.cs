using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


/* https://stackoverflow.com/questions/5115388/parsing-svg-path-elements-with-c-sharp-are-there-libraries-out-there-to-do-t
 * https://github.com/regebro/svg.path/blob/master/src/svg/path/parser.py
 * https://github.com/timrwood/SVGPath/blob/master/SVGPath/SVGPath.swift
 */

namespace InteropDrawing.Parametric
{
    static class PathParser
    {
        public static void DrawPath(IDrawing2D dc, string path)
        {
            var commands = PathCommand.ParsePath(path);

            foreach(var cmd in commands)
            {

            }            
        }

        // https://www.w3.org/TR/SVG/paths.html
        // lowercase: relative values
        // uppercase: absolute values

        [Flags]
        enum CommandType
        {            
            Relative = 0x20,

            AbsMoveTo = 'M',               // M (x y)+
            AbsClosePath = 'Z',            // Z
            AbsLineTo = 'L',               // L (x y)+
            AbsLineHorizontalTo = 'H',     // H x+
            AbsLineVerticalTo = 'V',       // V y+

            AbsCurveTo = 'C',                  // C (x1 y1 x2 y2 x y)+
            AbsSmoothCurveTo = 'S',            // S (x2 y2 x y)+

            AbsQuadraticCurveTo = 'Q',         // Q (x1 y1 x y)+
            AbsSmoothQuadraticCurveTo = 'T',   // T (x y)+

            AbsArc = 'A',                  // A (rx ry x-axis-rotation large-arc-flag sweep-flag x y)+

            RelModeTo = AbsMoveTo | Relative,
            RelClosePath = AbsClosePath | Relative,
            RelLineTo = AbsLineTo | Relative
        }

        struct PathCommand
        {
            public PathCommand(char command, params float[] arguments)
            {
                this.Command = command;
                this.Arguments = arguments;
            }

            public char Command { get; private set; }
            public float[] Arguments { get; private set; }            

            public static PathCommand Parse(string command)
            {
                const string argSeparators = @"[\s,]|(?=-)";

                var cmd = command.Take(1).Single();

                var remainingargs = command.Substring(1);
                
                var splitArgs = Regex
                    .Split(remainingargs, argSeparators)
                    .Where(t => !string.IsNullOrEmpty(t));

                var floatArgs = splitArgs
                    .Select(arg => float.Parse(arg, System.Globalization.CultureInfo.InvariantCulture))
                    .ToArray();

                return new PathCommand(cmd, floatArgs);
            }

            public static IEnumerable<PathCommand> ParsePath(string path)
            {
                // these letters are valid SVG
                const string pathCommands = @"(?=[MZLHVCSQTAmzlhvcsqta])";

                var tokens = Regex
                    .Split(path, pathCommands)
                    .Where(t => !string.IsNullOrEmpty(t));

                foreach (var token in tokens)
                {
                    yield return Parse(token);
                }
            }
        }
    }
}
