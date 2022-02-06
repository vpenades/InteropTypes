﻿using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Line cap styles used by <see cref="LineStyle"/>.
    /// </summary>
    public enum LineCapStyle
    {
        /// <summary>
        /// Leave open
        /// </summary>
        None = 0,

        /// <summary>
        /// A cap that does not extend past the last point of the line. If CanvasStrokeStyle.DashCap
        /// is set to Flat, dots will have zero size so only dashes are visible.
        /// </summary>
        Flat = 1,

        /// <summary>
        /// Half of a square that has a length equal to the line thickness.
        /// </summary>
        Square = 2,

        /// <summary>
        /// A semicircle that has a diameter equal to the line thickness.
        /// </summary>
        Round = 3,

        /// <summary>
        /// An isosceles right triangle whose hypotenuse is equal in length to the thickness
        /// of the line.
        /// </summary>
        Triangle = 4
    }

    /// <summary>
    /// Combines an <see cref="OutlineFillStyle"/> with Line Cap styles.
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="ICanvas2D.DrawLines(ReadOnlySpan{Point2}, float, in LineStyle)"/>.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Style.FillColor} {Style.OutlineColor} {Style.OutlineWidth} {StartCap} {EndCap}")]
    public readonly struct LineStyle
    {
        #region implicit

        public static implicit operator LineStyle(COLOR fillColor) { return new LineStyle(fillColor); }
        public static implicit operator LineStyle((COLOR, LineCapStyle) style) { return new LineStyle(style.Item1, style.Item2, style.Item2); }
        public static implicit operator LineStyle((COLOR, LineCapStyle, LineCapStyle) style) { return new LineStyle(style.Item1, style.Item2, style.Item3); }
        public static implicit operator LineStyle((COLOR, float) style) { return new LineStyle(style.Item1, style.Item2); }
        public static implicit operator LineStyle((COLOR, COLOR, float) style) { return new LineStyle(style.Item1, style.Item2, style.Item3); }
        public static implicit operator LineStyle((COLOR, COLOR, float, LineCapStyle) style) { return new LineStyle((style.Item1, style.Item2, style.Item3), style.Item4, style.Item4); }
        public static implicit operator LineStyle((COLOR, COLOR, float, LineCapStyle, LineCapStyle) style) { return new LineStyle((style.Item1, style.Item2, style.Item3), style.Item4, style.Item5); }

        public static implicit operator LineStyle(ColorStyle fillColor)                           { return new LineStyle(fillColor); }
        public static implicit operator LineStyle((ColorStyle, LineCapStyle) style)               { return new LineStyle(style.Item1, style.Item2, style.Item2); }
        public static implicit operator LineStyle((ColorStyle, LineCapStyle, LineCapStyle) style) { return new LineStyle(style.Item1, style.Item2, style.Item3); }
        public static implicit operator LineStyle((ColorStyle, float) style)                      { return new LineStyle(style.Item1, style.Item2); }
        public static implicit operator LineStyle((ColorStyle, ColorStyle, float) style)                             { return new LineStyle(style.Item1, style.Item2, style.Item3); }
        public static implicit operator LineStyle((ColorStyle, ColorStyle, float, LineCapStyle) style)               { return new LineStyle((style.Item1, style.Item2, style.Item3), style.Item4, style.Item4); }
        public static implicit operator LineStyle((ColorStyle, ColorStyle, float, LineCapStyle, LineCapStyle) style) { return new LineStyle((style.Item1, style.Item2, style.Item3), style.Item4, style.Item5); }

        public static implicit operator LineStyle((OutlineFillStyle, LineCapStyle) style) { return new LineStyle(style.Item1, style.Item2, style.Item2); }
        public static implicit operator LineStyle((OutlineFillStyle, LineCapStyle, LineCapStyle) style) { return new LineStyle(style.Item1, style.Item2, style.Item3); }

        #endregion

        #region constructors

        public LineStyle(ColorStyle fillColor)
        {
            Style = fillColor;
            _StartCap = _EndCap = (short)LineCapStyle.Flat;
        }

        public LineStyle(ColorStyle outColor, float outWidth)
        {
            Style = new OutlineFillStyle(outColor, outWidth);
            _StartCap = _EndCap = (short)LineCapStyle.Flat;
        }

        public LineStyle(ColorStyle fillColor, ColorStyle outColor, float outWidth)
        {
            Style = new OutlineFillStyle(fillColor, outColor, outWidth);
            _StartCap = _EndCap = (short)LineCapStyle.Flat;
        }

        public LineStyle(ColorStyle fillColor, LineCapStyle startCap, LineCapStyle endCap)
        {
            Style = new OutlineFillStyle(fillColor);
            _StartCap = (short)startCap;
            _EndCap = (short)endCap;
        }        

        public LineStyle(OutlineFillStyle color, LineCapStyle startCap, LineCapStyle endCap)
        {
            Style = color;
            _StartCap = (short)startCap;
            _EndCap = (short)endCap;
        }

        #endregion

        #region data

        public readonly OutlineFillStyle Style;
        private readonly short _StartCap;
        public readonly short _EndCap;

        #endregion

        #region properties

        public LineCapStyle StartCap => (LineCapStyle)_StartCap;
        public LineCapStyle EndCap => (LineCapStyle)_EndCap;
        public bool IsEmpty => Style.IsEmpty;
        public bool IsVisible => Style.IsVisible;
        public ColorStyle FillColor => Style.FillColor;
        public ColorStyle OutlineColor => Style.OutlineColor;
        public float OutlineWidth => Style.OutlineWidth;

        #endregion

        #region With * API

        private static readonly LineStyle _Default = ColorStyle.Transparent;

        public static readonly LineStyle Gray = ColorStyle.Gray;
        public static readonly LineStyle Black = ColorStyle.Black;
        public static readonly LineStyle White = ColorStyle.White;
        public static readonly LineStyle Red = ColorStyle.Red;
        public static readonly LineStyle Green = ColorStyle.Green;
        public static readonly LineStyle Blue = ColorStyle.Blue;
        public static readonly LineStyle Yellow = ColorStyle.Yellow;

        public LineStyle With(LineCapStyle caps) { return new LineStyle(Style, caps, caps); }

        public LineStyle With(LineCapStyle startCap, LineCapStyle endCap) { return new LineStyle(Style, startCap, endCap); }

        public LineStyle With(OutlineFillStyle style) { return new LineStyle(style, StartCap, EndCap); }

        public LineStyle WithOutline(float ow) { return new LineStyle(Style.WithOutline(ow), StartCap, EndCap); }

        public LineStyle WithOutline(COLOR color, float ow) { return new LineStyle(Style.WithOutline(color, ow), StartCap, EndCap); }

        public bool IsSolid(ref float diameter, out ColorStyle solidColor)
        {
            if (Style.OutlineColor.IsVisible && diameter < Style.OutlineWidth)
            {
                diameter = Style.OutlineWidth;
                solidColor = Style.OutlineColor;
                return true;
            }

            if (!Style.HasOutline) { solidColor = Style.FillColor; return true; }

            solidColor = default;
            return false;
        }

        #endregion
    }
}
