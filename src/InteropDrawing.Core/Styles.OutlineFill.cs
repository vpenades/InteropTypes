using System;

using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a Fill Color, an Outline Color, and an Outline Size.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{FillColor} {OutlineColor} {OutlineWidth}")]
    public readonly struct OutlineFillStyle
    {
        #region constructors

        public static implicit operator OutlineFillStyle(COLOR fillColor) { return new OutlineFillStyle(fillColor); }
        public static implicit operator OutlineFillStyle((COLOR, float) style) { return new OutlineFillStyle(style.Item1, style.Item2); }
        public static implicit operator OutlineFillStyle((COLOR, COLOR, float) style) { return new OutlineFillStyle(style.Item1, style.Item2, style.Item3); }

        public static implicit operator OutlineFillStyle(ColorStyle fillColor) { return new OutlineFillStyle(fillColor); }
        public static implicit operator OutlineFillStyle((ColorStyle, float) style) { return new OutlineFillStyle(style.Item1, style.Item2); }        
        public static implicit operator OutlineFillStyle((ColorStyle, ColorStyle, float) style) { return new OutlineFillStyle(style.Item1, style.Item2, style.Item3); }

        public OutlineFillStyle(ColorStyle fillColor)
        {
            FillColor = fillColor;
            OutlineColor = ColorStyle.Transparent;
            OutlineWidth = 0;
        }

        public OutlineFillStyle(ColorStyle outColor, float outWidth)
        {
            FillColor = ColorStyle.Transparent;
            OutlineColor = outColor;
            OutlineWidth = outWidth;
        }

        public OutlineFillStyle(ColorStyle fillColor, ColorStyle outColor, float outWidth)
        {
            FillColor = fillColor;
            OutlineColor = outColor;
            OutlineWidth = outWidth;
        }

        #endregion

        #region data

        public readonly ColorStyle FillColor;
        public readonly ColorStyle OutlineColor;
        public readonly float OutlineWidth;

        #endregion

        #region properties

        public bool IsEmpty => !IsVisible;

        public bool IsVisible => HasFill || HasOutline;

        public bool HasFill => FillColor.IsVisible;

        public bool HasOutline => OutlineColor.IsVisible && OutlineWidth > 0;

        #endregion

        #region With * API

        public OutlineFillStyle WithFill(ColorStyle fillColor)
        {
            return new OutlineFillStyle(fillColor, OutlineColor, OutlineWidth);
        }

        public OutlineFillStyle WithOutline(ColorStyle outlineColor, float ow)
        {
            return new OutlineFillStyle(FillColor, outlineColor, ow);
        }

        public OutlineFillStyle WithOutline(ColorStyle outlineColor)
        {
            return new OutlineFillStyle(FillColor, outlineColor, OutlineWidth);
        }

        public OutlineFillStyle WithOutline(float ow)
        {
            return new OutlineFillStyle(FillColor, OutlineColor, ow);
        }

        #endregion
    }
}
