using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    /// <summary>
    /// Line cap styles used by <see cref="LineStyle"/>.
    /// </summary>
    public enum LineCapStyle
    {
        /// <summary>
        /// A cap that does not extend past the last point of the line. If CanvasStrokeStyle.DashCap
        /// is set to Flat, dots will have zero size so only dashes are visible.
        /// </summary>
        Flat = 0,

        /// <summary>
        /// Half of a square that has a length equal to the line thickness.
        /// </summary>
        Square = 1,

        /// <summary>
        /// A semicircle that has a diameter equal to the line thickness.
        /// </summary>
        Round = 2,

        /// <summary>
        /// An isosceles right triangle whose hypotenuse is equal in length to the thickness
        /// of the line.
        /// </summary>
        Triangle = 3
    }

    /// <summary>
    /// Combines an <see cref="ColorStyle"/> with Line Cap styles.
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="IDrawing2D.DrawLines(ReadOnlySpan{Point2}, float, LineStyle)"/>.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Style.FillColor} {Style.OutlineColor} {Style.OutlineWidth} {StartCap} {EndCap}")]
    public readonly struct LineStyle
    {
        #region implicit

        public static implicit operator LineStyle(COLOR fillColor) { return new LineStyle(fillColor); }

        public static implicit operator LineStyle((COLOR, LineCapStyle) style) { return new LineStyle(style.Item1, style.Item2, style.Item2); }

        public static implicit operator LineStyle((COLOR, LineCapStyle, LineCapStyle) style) { return new LineStyle(style.Item1, style.Item2, style.Item3); }

        public static implicit operator LineStyle((ColorStyle, LineCapStyle) style) { return new LineStyle(style.Item1, style.Item2, style.Item2); }

        public static implicit operator LineStyle((ColorStyle, LineCapStyle, LineCapStyle) style) { return new LineStyle(style.Item1, style.Item2, style.Item3); }

        public static implicit operator LineStyle((COLOR, Single) style) { return new LineStyle(style.Item1, style.Item2); }

        public static implicit operator LineStyle((COLOR, COLOR, Single) style) { return new LineStyle(style.Item1, style.Item2, style.Item3); }

        public static implicit operator LineStyle((COLOR, COLOR, Single, LineCapStyle) style) { return new LineStyle((style.Item1, style.Item2, style.Item3), style.Item4, style.Item4); }

        public static implicit operator LineStyle((COLOR, COLOR, Single, LineCapStyle, LineCapStyle) style) { return new LineStyle((style.Item1, style.Item2, style.Item3), style.Item4, style.Item5); }

        #endregion

        #region constructors

        public LineStyle(COLOR fillColor)
        {
            Style = fillColor;
            StartCap = EndCap = LineCapStyle.Flat;
        }

        public LineStyle(COLOR outColor, Single outWidth)
        {
            Style = new ColorStyle(outColor, outWidth);
            StartCap = EndCap = LineCapStyle.Flat;
        }

        public LineStyle(COLOR fillColor, COLOR outColor, Single outWidth)
        {
            Style = new ColorStyle(fillColor, outColor, outWidth);
            StartCap = EndCap = LineCapStyle.Flat;
        }

        public LineStyle(COLOR fillColor, LineCapStyle startCap, LineCapStyle endCap)
        {
            Style = new ColorStyle(fillColor);
            StartCap = startCap;
            EndCap = endCap;
        }

        public LineStyle(ColorStyle color, LineCapStyle startCap, LineCapStyle endCap)
        {
            Style = color;
            StartCap = startCap;
            EndCap = endCap;
        }



        #endregion

        #region data

        public readonly ColorStyle Style;
        public readonly LineCapStyle StartCap;
        public readonly LineCapStyle EndCap;

        #endregion

        #region properties

        public bool IsVisible => Style.IsVisible;

        #endregion

        #region With * API

        private static readonly LineStyle _Default = new LineStyle(COLOR.Transparent);

        public static readonly LineStyle Gray = _Default.With(COLOR.Gray);
        public static readonly LineStyle Black = _Default.With(COLOR.Black);
        public static readonly LineStyle White = _Default.With(COLOR.White);
        public static readonly LineStyle Red = _Default.With(COLOR.Red);
        public static readonly LineStyle Green = _Default.With(COLOR.Green);
        public static readonly LineStyle Blue = _Default.With(COLOR.Blue);
        public static readonly LineStyle Yellow = _Default.With(COLOR.Yellow);

        public LineStyle With(LineCapStyle caps) { return new LineStyle(this.Style, caps, caps); }

        public LineStyle With(LineCapStyle startCap, LineCapStyle endCap) { return new LineStyle(this.Style, startCap, endCap); }

        public LineStyle With(ColorStyle style)
        {
            return new LineStyle(style, StartCap, EndCap);
        }

        public LineStyle WithOutline(Single ow)
        {
            return new LineStyle(Style.WithOutline(ow), StartCap, EndCap);
        }

        public LineStyle WithOutline(COLOR color, Single ow)
        {
            return new LineStyle(Style.WithOutline(color, ow), StartCap, EndCap);
        }

        #endregion
    }
}
