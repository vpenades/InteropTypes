using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using COLOR = System.Drawing.Color;
using XFORM = System.Numerics.Matrix3x2;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Style used for font rendering.
    /// </summary>
    /// <remarks>
    /// Style used by <see cref="ICanvas2D.DrawTextLine(in XFORM, string, float, FontStyle)"/>.
    /// Use Fonts.HersheyFont.Simplex as default font
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{Style.FillColor} {Style.OutlineColor} {Style.OutlineWidth} {Strength} {Alignment}")]
    public readonly struct FontStyle
    {
        #region implicit        

        public static implicit operator FontStyle(COLOR color) { return new FontStyle(null, color, 0.1f, Fonts.FontAlignStyle.FlipAuto); }

        public static implicit operator FontStyle(ColorStyle color) { return new FontStyle(null, color, 0.1f, Fonts.FontAlignStyle.FlipAuto); }

        public static implicit operator FontStyle((Fonts.IFont, COLOR) style) { return new FontStyle(style.Item1, style.Item2, 0.1f, Fonts.FontAlignStyle.FlipAuto); }

        public static implicit operator FontStyle((Fonts.IFont, Fonts.FontAlignStyle) style) { return new FontStyle(style.Item1, ColorStyle.White, 0.1f, style.Item2); }

        #endregion

        #region constructors

        public FontStyle(Fonts.IFont font, ColorStyle color, float strength = 0.1f)
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

        public void DrawDecomposedTo(ICoreCanvas2D dc, in XFORM transform, string text, float size)
        {
            if (size == 0) return;

            if (string.IsNullOrWhiteSpace(text)) return;

            var font = this.Font ?? TryGetDefaultFontFrom(dc);

            if (font == null) return;

            var xform = transform;

            // alignment transform

            float xflip = 1;
            float yflip = 1;

            if (Alignment.HasFlag(Fonts.FontAlignStyle.FlipHorizontal)) { xflip = -1; }
            if (Alignment.HasFlag(Fonts.FontAlignStyle.FlipVertical)) { yflip = -1; }

            if (Alignment.HasFlag(Fonts.FontAlignStyle.FlipAuto))
            {
                var axes = _GetCanvasAxes(xform);
                if (axes.X < 0) xflip *= -1;
                if (axes.Y < 0) yflip *= -1;
            }

            xform = XFORM.CreateScale(xflip, yflip) * xform;

            // size transform

            if (size > 0)
            {
                var scale = size / (float)font.Height;
                xform = XFORM.CreateScale(scale) * xform;
            }

            font.DrawTextLineTo(dc, xform, text, this.Color);
        }

        private static System.Numerics.Vector2 _GetCanvasAxes(ICoreCanvas2D dc)
        {
            if (!(dc is IServiceProvider sprov)) return System.Numerics.Vector2.One;

            if (!(sprov.GetService(typeof(XFORM)) is XFORM canvasXform)) return System.Numerics.Vector2.One;
            return _GetCanvasAxes(canvasXform);
        }

        private static Vector2 _GetCanvasAxes(Matrix3x2 canvasXform)
        {
            var o = System.Numerics.Vector2.Transform(System.Numerics.Vector2.Zero, canvasXform);
            var v = System.Numerics.Vector2.Transform(System.Numerics.Vector2.One, canvasXform);

            return v - o;
        }

        #endregion

        #region default values

        public static Fonts.IFont TryGetDefaultFontFrom(Object source)
        {
            return GlobalStyle.TryGetGlobalProperty<Fonts.IFont>(source, GlobalStyle.FONT, out var font)
                ? font
                : default;
        }

        public static Fonts.IFont GetDefaultFontFrom(Object source, Fonts.IFont defval)
        {
            return GlobalStyle.TryGetGlobalProperty<Fonts.IFont>(source, GlobalStyle.FONT, out var font)
                ? font
                : defval;
        }

        public bool TrySetDefaultFontTo(Object target)
        {
            return GlobalStyle.TrySetGlobalProperty(target, GlobalStyle.FONT, this.Font);
        }

        public bool TrySetDefaultFontTo(ref GlobalStyle target)
        {
            return GlobalStyle.TrySetGlobalProperty(ref target, GlobalStyle.FONT, this.Font);
        }

        #endregion
    }
}
