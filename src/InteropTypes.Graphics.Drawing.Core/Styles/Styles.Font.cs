﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Style used for font rendering.
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="ICanvas2D.DrawTextLine(in XFORM2, string, float, FontStyle)"/>.
    /// Use Fonts.HersheyFont.Simplex as default font
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Font} {Color} {Strength} {Alignment}")]
    public readonly struct FontStyle : IEquatable<FontStyle>
    {
        #region implicit        

        public static implicit operator FontStyle(COLOR color) { return new FontStyle(null, color, 0.1f, Fonts.FontAlignStyle.FlipAuto); }

        public static implicit operator FontStyle(ColorStyle color) { return new FontStyle(null, color, 0.1f, Fonts.FontAlignStyle.FlipAuto); }

        public static implicit operator FontStyle((Fonts.IFont, COLOR) style) { return new FontStyle(style.Item1, style.Item2, 0.1f, Fonts.FontAlignStyle.FlipAuto); }

        public static implicit operator FontStyle((Fonts.IFont, Fonts.FontAlignStyle) style) { return new FontStyle(style.Item1, ColorStyle.White, 0.1f, style.Item2); }

        #endregion

        #region constructors

        public FontStyle(Fonts.IFont font, ColorStyle color)
        {
            Font = font;
            Color = color;
            Strength = 0.1f;
            Alignment = Fonts.FontAlignStyle.FlipAuto;
        }

        public FontStyle(Fonts.IFont font, ColorStyle color, float strength)
        {
            Font = font;
            Color = color;
            Strength = strength;
            Alignment = Fonts.FontAlignStyle.FlipAuto;
        }        

        public FontStyle(Fonts.IFont font, ColorStyle color, float strength, Fonts.FontAlignStyle align)
        {
            Font = font;
            Color = color;
            Strength = strength;
            Alignment = align;
        }

        #endregion

        #region data

        public readonly Fonts.IFont Font;
        public readonly Fonts.FontAlignStyle Alignment;
        public readonly ColorStyle Color;
        public readonly float Strength;

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
            var h = Font?.GetHashCode() ?? 0;
            h |= Alignment.GetHashCode();
            h |= Color.GetHashCode();
            h |= Strength.GetHashCode();
            return h;
        }

        /// <inheritdoc/>
        public readonly override bool Equals(object obj)
        {
            return obj is FontStyle other && this.Equals(other);
        }

        /// <inheritdoc/>
        public readonly bool Equals(FontStyle other)
        {
            if (this.Font != other.Font) return false;
            if (this.Alignment != other.Alignment) return false;
            if (this.Color != other.Color) return false;
            if (this.Strength != other.Strength) return false;
            return true;
        }

        /// <inheritdoc/>
        public static bool operator ==(in FontStyle a, in FontStyle b) => a.Equals(b);

        /// <inheritdoc/>
        public static bool operator !=(in FontStyle a, in FontStyle b) => !a.Equals(b);


        #endregion

        #region properties

        public bool IsVisible => Color.IsVisible;

        #endregion

        #region with* API

        private static readonly FontStyle _Default = new FontStyle(null, COLOR.Transparent, 0.1f);

        public static readonly FontStyle Gray = _Default.With(COLOR.Gray);
        public static readonly FontStyle Black = _Default.With(COLOR.Black);
        public static readonly FontStyle White = _Default.With(COLOR.White);
        public static readonly FontStyle Red = _Default.With(COLOR.Red);
        public static readonly FontStyle Green = _Default.With(COLOR.Green);
        public static readonly FontStyle Blue = _Default.With(COLOR.Blue);

        private static readonly FontStyle _VFlip = new FontStyle(null, COLOR.Transparent, 0.1f, Fonts.FontAlignStyle.FlipVertical);

        public static readonly FontStyle VFlip_Gray = _VFlip.With(COLOR.Gray);
        public static readonly FontStyle VFlip_Black = _VFlip.With(COLOR.Black);
        public static readonly FontStyle VFlip_White = _VFlip.With(COLOR.White);
        public static readonly FontStyle VFlip_Red = _VFlip.With(COLOR.Red);
        public static readonly FontStyle VFlip_Green = _VFlip.With(COLOR.Green);
        public static readonly FontStyle VFlip_Blue = _VFlip.With(COLOR.Blue);


        public FontStyle With(Fonts.IFont font) { return new FontStyle(font, Color, Strength, Alignment); }

        public FontStyle With(ColorStyle color) { return new FontStyle(Font, color, Strength, Alignment); }

        public FontStyle With(ColorStyle color, float strength) { return new FontStyle(Font, color, strength, Alignment); }

        public FontStyle With(float strength) { return new FontStyle(Font, Color, strength, Alignment); }

        public FontStyle With(Fonts.FontAlignStyle align)
        {
            align |= Alignment;

            return new FontStyle(Font, Color, Strength, align);
        }

        #endregion

        #region drawing support

        public System.Drawing.RectangleF MeasureTextLine(string text, float size)
        {
            var rect = Font?.MeasureTextLine(text) ?? System.Drawing.RectangleF.Empty;

            if (size <= 0) size = Font?.Height ?? 1;

            var scale = size / (Font?.Height ?? 1);

            rect.X *= scale;
            rect.Y *= scale;
            rect.Width *= scale;
            rect.Height *= scale;

            return rect;
        }

        public void DrawDecomposedTo(ICoreCanvas2D dc, in XFORM2 transform, string text, float size)
        {
            if (size == 0) return;
            if (string.IsNullOrWhiteSpace(text)) return;

            var font = this.Font ?? TryGetDefaultFontFrom(dc).Font;
            if (font == null) return;            

            // alignment transform

            var xform = _GetAlignmentTransform(transform, this.Alignment, dc);

            // size transform

            if (size > 0)
            {
                var scale = size / (float)font.Height;
                xform = XFORM2.CreateScale(scale) * xform;
            }

            font.DrawTextLineTo(dc, xform, text, this.Color);
        }

        private static XFORM2 _GetAlignmentTransform(XFORM2 xform, Fonts.FontAlignStyle align, ICoreCanvas2D dc)
        {
            float xflip = 1;
            float yflip = 1;            

            // if alignment is Auto, let's try to take it from the global style
            if (align.HasFlag(Fonts.FontAlignStyle.FlipAuto))
            {
                align = TryGetDefaultFontFrom(dc).Alignment;
            }

            if (align.HasFlag(Fonts.FontAlignStyle.FlipHorizontal)) { xflip = -1; }
            if (align.HasFlag(Fonts.FontAlignStyle.FlipVertical)) { yflip = -1; }

            // we're still in Auto, let's analyze the backend to check the screen axis alignment
            if (align.HasFlag(Fonts.FontAlignStyle.FlipAuto))
            {
                var axes = _GetCanvasAxes(dc);
                if (axes.X < 0) xflip *= -1;
                if (axes.Y < 0) yflip *= -1;
            }

            return XFORM2.CreateScale(xflip, yflip) * xform;
        }

        private static XY _GetCanvasAxes(ICoreCanvas2D dc)
        {
            if (!(dc is IServiceProvider sprov)) return XY.One;

            if (!(sprov.GetService(typeof(XFORM2)) is XFORM2 canvasXform)) return XY.One;
            return _GetCanvasAxes(canvasXform);
        }

        private static XY _GetCanvasAxes(XFORM2 canvasXform)
        {
            var o = XY.Transform(XY.Zero, canvasXform);
            var v = XY.Transform(XY.One, canvasXform);

            return v - o;
        }

        #endregion

        #region default values        

        public static FontStyle TryGetDefaultFontFrom(Object source)
        {
            return GlobalStyle.TryGetGlobalProperty<FontStyle>(source, GlobalStyle.FONT, out var font)
                ? font
                : default;
        }

        public static FontStyle GetDefaultFontFrom(Object source, FontStyle defval)
        {
            return GlobalStyle.TryGetGlobalProperty<FontStyle>(source, GlobalStyle.FONT, out var font)
                ? font
                : defval;
        }

        public bool TrySetDefaultFontTo(Object target)
        {
            return GlobalStyle.TrySetGlobalProperty(target, GlobalStyle.FONT, this);
        }

        public bool TrySetDefaultFontTo(ref GlobalStyle target)
        {
            return GlobalStyle.TrySetGlobalProperty(ref target, GlobalStyle.FONT, this);
        }

        #endregion
    }
}
