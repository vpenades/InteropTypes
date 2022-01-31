using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    /// <summary>
    /// Font drawing modes used by <see cref="FontStyle"/>.
    /// </summary>
    [Flags]
    public enum FontAlignStyle
    {
        None,

        /// <summary>
        /// Flips the text based on the underlaying <see cref="ICanvas2D"/>'s <see cref="Quadrant"/>
        /// </summary>
        FlipAuto = 1,
        FlipHorizontal = 2,
        FlipVertical = 4,

        // to determine the text origin, calculate the points of the 4 corners, and
        // based on the values below, apply weights to the points.

        DockLeft = 8,
        DockRight = 16,
        DockTop = 32,
        DockBottom = 64,
        Center = DockLeft | DockRight | DockLeft | DockBottom
    }

    /// <summary>
    /// Style used for font rendering WIP.
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="Toolkit.DrawFont(ICanvas2D, System.Numerics.Matrix3x2, string, FontStyle)"/>.
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Style.FillColor} {Style.OutlineColor} {Style.OutlineWidth} {Strength} {Alignment}")]
    public readonly struct FontStyle
    {
        #region implicit

        public static implicit operator FontStyle(COLOR fillColor) { return new FontStyle(fillColor); }

        public static implicit operator FontStyle((COLOR, Single) style) { return new FontStyle(style.Item1, style.Item2); }

        public static implicit operator FontStyle((COLOR, Single, FontAlignStyle) style) { return new FontStyle(style.Item1, style.Item2, style.Item3); }

        #endregion

        #region constructors

        public FontStyle(OutlineFillStyle color, float strength = 0.1f)
        {
            Style = color;
            Strength = strength;
            Alignment = FontAlignStyle.FlipAuto;
        }

        public FontStyle(OutlineFillStyle color, float strength, FontAlignStyle align)
        {
            Style = color;
            Strength = strength;
            Alignment = align;
        }

        #endregion

        #region data

        public readonly OutlineFillStyle Style;
        public readonly Single Strength;
        public readonly FontAlignStyle Alignment;

        #endregion

        #region properties

        public bool IsVisible => Style.IsVisible;

        #endregion

        #region with* API

        private static readonly FontStyle _Default = new FontStyle(COLOR.Transparent, 0.1f);

        public static readonly FontStyle Gray = _Default.With(COLOR.Gray);
        public static readonly FontStyle Black = _Default.With(COLOR.Black);
        public static readonly FontStyle White = _Default.With(COLOR.White);
        public static readonly FontStyle Red = _Default.With(COLOR.Red);
        public static readonly FontStyle Green = _Default.With(COLOR.Green);
        public static readonly FontStyle Blue = _Default.With(COLOR.Blue);

        private static readonly FontStyle _VFlip = new FontStyle(COLOR.Transparent, 0.1f, FontAlignStyle.FlipVertical);

        public static readonly FontStyle VFlip_Gray = _VFlip.With(COLOR.Gray);
        public static readonly FontStyle VFlip_Black = _VFlip.With(COLOR.Black);
        public static readonly FontStyle VFlip_White = _VFlip.With(COLOR.White);
        public static readonly FontStyle VFlip_Red = _VFlip.With(COLOR.Red);
        public static readonly FontStyle VFlip_Green = _VFlip.With(COLOR.Green);
        public static readonly FontStyle VFlip_Blue = _VFlip.With(COLOR.Blue);

        public FontStyle With(OutlineFillStyle style) { return new FontStyle(style, this.Strength, this.Alignment); }

        public FontStyle With(OutlineFillStyle style, float strength) { return new FontStyle(style, strength, this.Alignment); }

        public FontStyle With(Single strength) { return new FontStyle(this.Style, strength, this.Alignment); }

        public FontStyle With(FontAlignStyle align)
        {
            align |= this.Alignment;

            return new FontStyle(this.Style, this.Strength, align);
        }

        #endregion
    }
}
